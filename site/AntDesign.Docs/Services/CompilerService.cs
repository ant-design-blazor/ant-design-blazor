// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;

namespace AntDesign.Docs.Services
{
    public class CompilerService
    {
        private sealed class CollectibleAssemblyLoadContext : AssemblyLoadContext
        {
            public CollectibleAssemblyLoadContext() : base(isCollectible: true)
            {
            }
        }

        private sealed class InMemoryProjectItem : RazorProjectItem
        {
            private readonly string _source;
            private readonly string _fileName;

            public override string BasePath => _fileName;

            public override string FilePath => _fileName;

            public override string PhysicalPath => _fileName;

            public override bool Exists => true;

            public override Stream Read()
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(_source));
            }

            private readonly string _fileKind = FileKinds.Component;

            public override string FileKind => _fileKind;

            public InMemoryProjectItem(string source)
            {
                this._source = source;
                _fileName = "DynamicComponent.razor";
                _fileKind = "component";
            }
        }

        private CSharpCompilation _compilation;
        private RazorProjectEngine _razorProjectEngine;
        private CollectibleAssemblyLoadContext _loadContext;

        private AssemblyLoadContext GetLoadContext()
        {
            if (_loadContext != null)
            {
                _loadContext.Unload();
                GC.Collect();
            }

            _loadContext = new CollectibleAssemblyLoadContext();

            return _loadContext; ;
        }
        private const string Imports = @"@using System.Net.Http
@using System
@using System.Collections.Generic
@using System.Linq
@using System.Threading.Tasks
@using Microsoft.AspNetCore.Components
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using AntDesign
@using AntDesign.Docs.Pages
@using AntDesign.Docs.Shared
@using System.Text.Json
";
        private readonly HttpClient _httpClient;

        public CompilerService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        private async Task InitializeAsync()
        {
            var streams = await GetStreamsAsync();

            var referenceAssemblies = streams.Select(stream => MetadataReference.CreateFromStream(stream)).ToList();

            _compilation = CSharpCompilation.Create(
                 "AntDesign.Docs.DynamicAssembly",
                 Array.Empty<SyntaxTree>(),
                 referenceAssemblies,
                 new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            _razorProjectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, RazorProjectFileSystem.Create("/"), builder =>
            {
                builder.SetRootNamespace("AntDesign.Docs.Demo");

                CompilerFeatures.Register(builder);

                var metadataReferenceFeature = new DefaultMetadataReferenceFeature { References = referenceAssemblies.ToArray() };

                builder.Features.Add(metadataReferenceFeature);

                builder.Features.Add(new CompilationTagHelperFeature());
            });
        }


        private async Task<IEnumerable<Stream>> GetStreamsAsync()
        {
            var referenceAssemblyRoots = new[]
            {
                 typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly, // System.Runtime
                 typeof(ComponentBase).Assembly,
                 typeof(AntDesign._Imports).Assembly,
                 typeof(ExpandoObject).Assembly,
                 typeof(_Imports).Assembly,
                 typeof(System.ValueType).Assembly,
                 typeof(OneOf.IOneOf).Assembly,
             };

            var referencedAssemblies = referenceAssemblyRoots
              .SelectMany(assembly => assembly.GetReferencedAssemblies().Append(assembly.GetName()))
              .Select(Assembly.Load)
              .Distinct()
              .ToList();

            if (referencedAssemblies.Any(assembly => string.IsNullOrEmpty(assembly.Location)))
            {
                var list = new List<Stream>();

                await Task.WhenAll(
                    referencedAssemblies.Select(async assembly =>
                    {
                        var result = await _httpClient.GetAsync($"/_framework/{assembly.GetName().Name}.dll");

                        result.EnsureSuccessStatusCode();

                        list.Add(await result.Content.ReadAsStreamAsync());
                    })
                );

                return list;
            }
            else
            {
                return referencedAssemblies.Select(assembly => File.OpenRead(assembly.Location));
            }
        }

        public async Task<Type> CompileAsync(string source)
        {
            if (_compilation == null)
            {
                await InitializeAsync();
            }

            var projectItem = new InMemoryProjectItem(source);

            var codeDocument = _razorProjectEngine.Process(RazorSourceDocument.ReadFrom(projectItem), FileKinds.Component,
                new[] { RazorSourceDocument.Create(Imports, "_Imports.razor") }, null);

            var csharpDocument = codeDocument.GetCSharpDocument();

            var syntaxTree = CSharpSyntaxTree.ParseText(csharpDocument.GeneratedCode, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp10));

            _compilation = _compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(syntaxTree);

            var errors = _compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error);

            if (errors.Any())
            {
                throw new ApplicationException(string.Join(Environment.NewLine, errors.Select(e => e.GetMessage())));
            }

            using var stream = new MemoryStream();

            var emitResult = _compilation.Emit(stream);

            if (!emitResult.Success)
            {
                throw new ApplicationException(string.Join(Environment.NewLine, emitResult.Diagnostics.Select(d => d.GetMessage())));
            }

            stream.Seek(0, SeekOrigin.Begin);

            var assembly = GetLoadContext().LoadFromStream(stream);

            var type = assembly.GetType("AntDesign.Docs.Demo.DynamicComponent");

            return type;
        }
    }
}
