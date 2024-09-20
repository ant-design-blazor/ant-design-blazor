// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using AntDesign.Docs.Build.CLI.Documentations;
using AntDesign.Docs.Build.CLI.Extensions;
using AntDesign.Docs.Build.CLI.Services.Translation;
using AntDesign.Docs.Build.CLI.Utils;
using LoxSmoke.DocXml;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Command
{
    public class GenerateDemoJsonCommand : IAppCommand
    {
        private const string DocsAssemblyName = "AntDesign.Docs";
        private static readonly Regex _orderedListRegex = new("<list type=\"number\">([\\s\\S]*?)<\\/list>");
        private static readonly Regex _seeRegex = new("<see(also)? cref=\"([A-z]:)?([A-z0-9:.`]+?)\" ?\\/>");
        private static readonly Regex _twoOrMoreWhitespaceRegex = new("[\\s]{2,}");
        private static readonly Regex _unorderedListRegex = new("<list type=\"bullet\">([\\s\\S]*?)<\\/list>");
        private readonly Type _componentBaseType = typeof(AntComponentBase);

        private readonly IEnumerable<string> _methodNamesToIgnore = new List<string>
        {
            "Dispose",
            "DisposeAsync",
            "Equals",
            "GetHashCode",
            "GetType",
            "ToString",

            // Lifecycle methods in Blazor components
            "SetParametersAsync",
            "OnInitialized",
            "OnInitializedAsync",
            "OnParametersSet",
            "OnParametersSetAsync",
            "OnAfterRender",
            "OnAfterRenderAsync",
            "StateHasChanged"
        };

        private readonly IEnumerable<MemberTypes> _supportedMemberTypes = new List<MemberTypes>
        {
            MemberTypes.Field,
            MemberTypes.Property,
            MemberTypes.Event
        };

        private readonly ITranslate _translate;
        private string _currentDirectory;
        private string _demoDirectory;
        private XmlDocument _document;
        private Assembly _libraryAssembly;

        public GenerateDemoJsonCommand(ITranslate translate)
        {
            _translate = translate;
        }

        public string Name => "demo2json";

        public void Execute(CommandLineApplication command)
        {
            command.Description = "Generate json file of demos";
            command.HelpOption();

            var directoryArgument = command.Argument(
                "source", "[Required] The directory of demo files.");

            var outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            command.OnExecute(async () =>
            {
                var source = directoryArgument.Value;
                var output = outputArgument.Value;

                if (string.IsNullOrEmpty(source) || !Directory.Exists(source))
                {
                    Console.WriteLine($"Invalid source: {source}");
                    return 1;
                }

                if (string.IsNullOrEmpty(output))
                {
                    output = "./";
                }

                _currentDirectory = Directory.GetCurrentDirectory();
                _demoDirectory = Path.Combine(_currentDirectory, source);

                try
                {
                    await GenerateDocumentation(output);

                    await _translate.BackupTranslations();
                }
                catch (Exception ex)
                {
                    await _translate.BackupTranslations(false);

                    Console.WriteLine(ex);

                    return -1;
                }

                return 0;
            });
        }

        private static string? GetDefaultFromComponentDocsNode(XmlNode node, string propertyTypeName)
        {
            if (node is null || !node.HasChildNodes)
            {
                return null;
            }

            var value = node.ChildNodes.Cast<XmlNode>().SingleOrDefault(x => x.Name == "default")?.Attributes["value"]?.Value?.Trim();

            return value is null
                ? null
                : WebUtility.HtmlDecode(value);
        }

        private static string GetNameWithoutGenerics(Type type)
        {
            var name = type.Name;

            var genericArguments = type.GetGenericArguments();

            return genericArguments.Any()
                ? name.Replace($"`{genericArguments.Length}", string.Empty)
                : name;
        }

        private static IEnumerable<string> GetSeeAlsoFromComponentDocsNode(XmlNode node)
        {
            if (node is null || !node.HasChildNodes)
            {
                return Enumerable.Empty<string>();
            }

            var seeReferences = node.ChildNodes.Cast<XmlNode>()
                .Where(x => x.Name == "seealso")
                .Select(x => x.Attributes["cref"]?.Value)
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.StartsWith("T")); // Only supporting "see"ing Types currently

            return seeReferences;
        }

        private static bool IsMatchingLanguageOrEnglishAndEmpty(XmlNode node, string language)
        {
            var nodeLanguage = node.Attributes["xml:lang"]?.Value?.Trim() ?? Constants.EnglishLanguage;

            return nodeLanguage == language;
        }

        private async Task GenerateDocumentation(string output)
        {
            LoadAssembly();
            LoadXmlCommentsDocument();

            var componentsDocsByLanguage = new Dictionary<string, List<DemoComponent>>()
            {
                { Constants.EnglishLanguage, new List<DemoComponent>() },
                { Constants.ChineseLanguage, new List<DemoComponent>() }
            };

            var libraryComponents = _libraryAssembly.GetTypes().Where(x => x.GetType() != _componentBaseType);
            foreach (var component in libraryComponents)
            {
                var componentName = GetNameWithoutGenerics(component);
                var componentDocs = GetMemberXmlNodes(component);
                var docAttribute = component.GetCustomAttribute<DocumentationAttribute>();
                if (docAttribute is null)
                {
                    continue;
                }

                // Build data required for docs
                var title = docAttribute.Title ?? componentName;
                var componentSummary = await GetAllLanguageSummaries(componentDocs);
                var pageUrl = GetAllLanguagesUrlToComponentDocumentation(component);
                var apiDocs = await GetAllLanguagesApiDocs(component);
                var seeAlso = await GetAllLanguagesSeeAlsoDocs(componentDocs);
                var faqs = GetAllLanguagesFaqDocs(componentName);
                var demos = GetAllLanguagesDemos(componentName);
                var docs = GetAllLanguagesComponentDocs(componentName);

                // Build docs for multiple languages
                componentsDocsByLanguage[Constants.EnglishLanguage].Add(GenerateApiDocumentation.ForComponent(Constants.EnglishLanguage, docAttribute, title, docs, pageUrl, apiDocs, seeAlso, faqs, demos));
                componentsDocsByLanguage[Constants.ChineseLanguage].Add(GenerateApiDocumentation.ForComponent(Constants.ChineseLanguage, docAttribute, title, docs, pageUrl, apiDocs, seeAlso, faqs, demos));
            }

            // recognize componetns by doc files
            var docComponents = GenerateCompentFromDoc();
            foreach (var item in docComponents)
                foreach (var component in item)
                    componentsDocsByLanguage[component.Key].Add(component.Value);

            foreach (var language in componentsDocsByLanguage)
            {
                WriteFiles(language.Value, language.Key, output);
            }
        }

        private async Task<IEnumerable<IApiDocumentation>> GetApiDocs(MemberInfo[] allPublicMembers, string language)
        {
            var allDocs = new List<IApiDocumentation>();

            var nonMethodMembers = allPublicMembers.Where(x => x.MemberType != MemberTypes.Method && IgnoreMember(x) == false).ToList();
            if (nonMethodMembers.Any())
            {
                foreach (var member in nonMethodMembers)
                {
                    var memberDocs = GetMemberXmlNodes(member);
                    var propertyType = GetPropertyType(member as PropertyInfo);
                    var obsoleteMessage = await GetObsoleteMessage(member, language);

                    allDocs.Add(new PropertyDocumentation
                    {
                        Name = member.Name,
                        IsParameter = member.GetCustomAttribute<ParameterAttribute>() is not null,
                        IsMethod = false,
                        Summary = await GetSummaryFromComponentDocsNode(memberDocs, language),
                        ObsoleteMessage = obsoleteMessage,
                        Type = propertyType,
                        Default = GetDefaultFromComponentDocsNode(memberDocs, propertyType)
                    });
                }
            }

            var methodMembers = allPublicMembers
                .Where(x => x.MemberType == MemberTypes.Method)
                .Select(x => x as MethodInfo)
                .Where(x => !x.IsSpecialName && !_methodNamesToIgnore.Contains(x.Name))
                .ToList();

            if (methodMembers.Any())
            {
                foreach (var member in methodMembers)
                {
                    var memberDocs = GetMemberXmlNodes(member);

                    allDocs.Add(new MethodDocumentation
                    {
                        Signature = GetMethodSignature(member),
                        ReturnType = GetTypeName(member.ReturnType),
                        Summary = await GetSummaryFromComponentDocsNode(memberDocs, language)
                    });
                }
            }

            return allDocs;
        }

        private async Task<IEnumerable<IApiDocumentation>> GetEnumDocs(Type type, string language)
        {
            var allDocs = new List<IApiDocumentation>();
            var allPublicMembers = type.GetMembers();
            var nonMethodMembers = allPublicMembers.Where(x => x.MemberType != MemberTypes.Method && IgnoreMember(x) == false).ToList();
            if (nonMethodMembers.Any())
            {
                foreach (var member in nonMethodMembers)
                {
                    var propertyInfo = member as FieldInfo;
                    if (propertyInfo.IsSpecialName)
                    {
                        continue;
                    }

                    allDocs.Add(new EnumDocumentation()
                    {
                        Name = member.Name,
                        UnderlyingType = Enum.GetUnderlyingType(type).Name,
                        Summary = await GetSummaryFromComponentDocsNode(GetMemberXmlNodes(member), language),
                        ObsoleteMessage = await GetObsoleteMessage(member, language)
                    });
                }
            }

            return allDocs;
        }

        private string GetFaqDocs(string componentName, string language)
        {
            var faqFile = new FileInfo(Path.Join(_demoDirectory, $"Components\\{componentName}\\faq.{language}.md"));
            if (faqFile.Exists)
            {
                var faqFileContent = File.ReadAllText(faqFile.FullName);
                return Markdown.ToHtml(faqFileContent);
            }

            return null;
        }

        private string GetComponentDocs(string componentName, string language)
        {
            var componentFile = new FileInfo(Path.Join(_demoDirectory, $"Components\\{componentName}\\doc\\index.{language}.md"));
            if (componentFile.Exists)
            {
                var faqFileContent = File.ReadAllText(componentFile.FullName);
                (Dictionary<string, string> Meta, string desc, string apiDoc) docData = DocParser.ParseDemoDoc(faqFileContent);
                return docData.desc;
            }

            return componentFile.FullName;
        }

        private string GetMemberXmlName(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                    return XmlDocId.MethodId(member as MethodBase);

                case MemberTypes.Event:
                    return XmlDocId.EventId(member);

                case MemberTypes.Field:
                    return XmlDocId.FieldId(member);

                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                    return XmlDocId.TypeId(member as TypeInfo);

                case MemberTypes.Property:
                    return XmlDocId.PropertyId(member);

                default:
                    throw new ArgumentException("Unknown member type", nameof(member));
            }
        }

        private XmlNode GetMemberXmlNodes(MemberInfo member)
        {
            var name = GetMemberXmlName(member);

            return GetXmlNodesForName(name);
        }

        private string GetMethodSignature(MethodInfo method)
        {
            var parameters = method.GetParameters().Select(x => $"{GetTypeName(x.ParameterType)} {x.Name}");
            var signature = $"{method.Name}({string.Join(", ", parameters)})";

            return signature;
        }

        private async Task<string> GetObsoleteMessage(MemberInfo member, string language)
        {
            var message = member.GetCustomAttribute<ObsoleteAttribute>()?.Message;

            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            if (language == Constants.EnglishLanguage)
            {
                return message;
            }

            var translated = await _translate.TranslateText(message, language);

            return string.IsNullOrWhiteSpace(translated)
                ? null
                : translated;
        }

        private string GetPropertyType(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
            {
                return string.Empty;
            }

            return GetTypeName(propertyInfo.PropertyType);
        }

        private async Task<string> GetSummaryFromComponentDocsNode(XmlNode node, string language)
        {
            if (node is null || !node.HasChildNodes)
            {
                return string.Empty;
            }

            var languageSummary = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "summary" && IsMatchingLanguageOrEnglishAndEmpty(x, language));
            if (languageSummary is null)
            {
                if (language == Constants.EnglishLanguage)
                {
                    return string.Empty;
                }

                var englishSummary = await GetSummaryFromComponentDocsNode(node, Constants.EnglishLanguage);
                if (string.IsNullOrWhiteSpace(englishSummary))
                {
                    return string.Empty;
                }

                var translated = await _translate.TranslateText(englishSummary, language);
                return string.IsNullOrWhiteSpace(translated)
                    ? string.Empty
                    : translated;
            }

            var trimmedSummary = _twoOrMoreWhitespaceRegex.Replace(languageSummary.InnerXml.Trim(), " ");

            trimmedSummary = _seeRegex.Replace(trimmedSummary, match =>
            {
                var isType = match.Groups[2].Value == "T:";
                var name = match.Groups[3].Value;
                var type = _libraryAssembly.GetType(name);
                var urlToTypeDocumentation = isType
                    ? GetUrlToComponentDocumentation(type, language)
                    : null;

                if (type is not null)
                {
                    name = isType
                        ? GetNameWithoutGenerics(type)
                        : GetNameWithoutGenerics(type.DeclaringType) + "." + GetNameWithoutGenerics(type);
                }
                name = $"<code>{name}</code>";

                return urlToTypeDocumentation is null
                    ? name
                    : $"<a href=\"{urlToTypeDocumentation}\">{name}</a>";
            });

            trimmedSummary = _unorderedListRegex.Replace(trimmedSummary, "<ul>$1</ul>");
            trimmedSummary = _orderedListRegex.Replace(trimmedSummary, "<ol>$1</ol>");
            trimmedSummary = trimmedSummary.Replace("<item>", "<li>");
            trimmedSummary = trimmedSummary.Replace("</item>", "</li>");
            trimmedSummary = trimmedSummary.Replace("<c>", "<code>");
            trimmedSummary = trimmedSummary.Replace("</c>", "</code>");
            trimmedSummary = trimmedSummary.Replace("<para>", "<p>");
            trimmedSummary = trimmedSummary.Replace("</para>", "</p>");

            return trimmedSummary;
        }

        private string GetTypeName(Type type)
        {
            var name = type.Name;
            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();

                name = GetNameWithoutGenerics(type);
                name += "&lt;" + string.Join(", ", genericArguments.Select(GetTypeName)) + "&gt;";

                if (name.StartsWith("Nullable&lt;"))
                {
                    name = name.Substring(12).Replace("&gt;", "?");
                }
            }

            return name.ToUpperInvariant() == "VOID"
                ? name.ToLowerInvariant()
                : name;
        }

        private string? GetUrlToComponentDocumentation(Type component, string language)
        {
            var docAttribute = component.GetCustomAttribute<DocumentationAttribute>();
            if (docAttribute is null)
            {
                return null;
            }

            var title = docAttribute.Title ?? GetNameWithoutGenerics(component);
            return $"/{language}/components/{title.ToLowerInvariant()}";
        }

        private XmlNode GetXmlNodesForName(string name)
        {
            var memberDocs = _document.SelectSingleNode($"/doc/members/member[@name=\"{name}\"]");
            if (memberDocs is null || memberDocs.ChildNodes.Count == 0)
            {
                return memberDocs;
            }

            var childNodes = memberDocs.ChildNodes.Cast<XmlNode>();
            var inheritdocs = childNodes.Where(x => x.Name == "inheritdoc");
            if (!inheritdocs.Any())
            {
                return memberDocs;
            }

            foreach (var inherited in inheritdocs)
            {
                var inheritCref = inherited.Attributes?["cref"]?.Value?.Trim() ?? string.Empty;
                var nodesForInherited = GetXmlNodesForName(inheritCref)?.ChildNodes?.Cast<XmlNode>();

                if (nodesForInherited is null || !nodesForInherited.Any())
                {
                    continue;
                }

                foreach (var node in nodesForInherited)
                {
                    if (childNodes.Any(x => x.Name == node.Name))
                    {
                        continue;
                    }

                    memberDocs.AppendChild(node);
                }
            }

            return memberDocs;
        }

        private bool IgnoreMember(MemberInfo member)
        {
            return !_supportedMemberTypes.Contains(member.MemberType)
                || member.GetCustomAttribute<CascadingParameterAttribute>() is not null
                || member.GetCustomAttribute<InjectAttribute>() is not null;
        }

        private void LoadAssembly()
        {
            _libraryAssembly = Assembly.Load("AntDesign");
        }

        private void LoadXmlCommentsDocument()
        {
            var xmlPath = _libraryAssembly.Location.Replace(".dll", ".xml");
            _document = new XmlDocument();
            _document.Load(xmlPath);
        }

        #region Get All Languages Methods

        private async Task<IDictionary<string, IEnumerable<IApiDocumentation>>> GetAllLanguagesApiDocs(Type type)
        {
            var allPublicMembers = type.GetMembers();

            return new Dictionary<string, IEnumerable<IApiDocumentation>>
            {
                { Constants.EnglishLanguage, await GetApiDocs(allPublicMembers, Constants.EnglishLanguage) },
                { Constants.ChineseLanguage, await GetApiDocs(allPublicMembers, Constants.ChineseLanguage) }
            };
        }

        private IEnumerable<DemoInformation> GetAllLanguagesDemos(string componentName)
        {
            // Does not need translation - the demo should have each language currently

            var demoDirectoryInfo = new DirectoryInfo(_demoDirectory);
            var componentDemoDirectory = demoDirectoryInfo.GetDirectories().FirstOrDefault(x => x.Name == "Components")
                ?.GetDirectories().FirstOrDefault(x => x.Name == componentName)?
                .GetDirectories().FirstOrDefault(x => x.Name == "demo");

            if (componentDemoDirectory?.Exists == true)
            {
                var demoFiles = componentDemoDirectory.GetFileSystemInfos().GroupBy(x => x.Name
                    .Replace(x.Extension, "")
                    .Replace("-", "")
                    .Replace("_", "")
                    .Replace("Demo", "")
                    .ToLower());

                var order = 0;
                foreach (var fileGroup in demoFiles)
                {
                    var razorFile = fileGroup.FirstOrDefault(x => x.Extension == ".razor");
                    var descriptionFile = fileGroup.FirstOrDefault(x => x.Extension == ".md");
                    var descriptionContent = DocParser.ParseDescription(File.ReadAllText(descriptionFile.FullName));
                    var demoType = $"{demoDirectoryInfo.Name}{razorFile.FullName.Replace(demoDirectoryInfo.FullName, "").Replace("/", ".").Replace("\\", ".").Replace(razorFile.Extension, "")}";

                    yield return new DemoInformation
                    {
                        DescriptionContent = descriptionContent,
                        Code = File.ReadAllText(razorFile.FullName) ?? null,
                        Name = descriptionFile.Name.Replace(".md", ""),
                        Type = demoType
                    };

                    ++order;
                }
            }
        }

        private async Task<IDictionary<string, IEnumerable<IApiDocumentation>>> GetAllLanguagesEnumDocs(Type type)
        {
            return new Dictionary<string, IEnumerable<IApiDocumentation>>
            {
                { Constants.EnglishLanguage, await GetEnumDocs(type, Constants.EnglishLanguage) },
                { Constants.ChineseLanguage, await GetEnumDocs(type, Constants.ChineseLanguage) }
            };
        }

        private Dictionary<string, string> GetAllLanguagesFaqDocs(string componentName)
        {
            // Does not need translation - there should be one doc per language for these

            return new Dictionary<string, string>
            {
                { Constants.EnglishLanguage, GetFaqDocs(componentName, Constants.EnglishLanguage) },
                { Constants.ChineseLanguage, GetFaqDocs(componentName, Constants.ChineseLanguage) }
            };
        }

        private Dictionary<string, string> GetAllLanguagesComponentDocs(string componentName)
        {
            // Does not need translation - there should be one doc per language for these

            return new Dictionary<string, string>
            {
                { Constants.EnglishLanguage, GetComponentDocs(componentName, Constants.EnglishLanguage) },
                { Constants.ChineseLanguage, GetComponentDocs(componentName, Constants.ChineseLanguage) }
            };
        }

        /// <returns>{ seeAlsoName: { language: [docs] } }</returns>
        private async Task<IDictionary<string, IDictionary<string, IEnumerable<IApiDocumentation>>>> GetAllLanguagesSeeAlsoDocs(XmlNode componentDocs)
        {
            // Does not need translation - the "GetAllLanguages..." methods called below will translate if needed

            var allSeeAlsos = new Dictionary<string, IDictionary<string, IEnumerable<IApiDocumentation>>>();

            var seeAlsoRefs = GetSeeAlsoFromComponentDocsNode(componentDocs);
            if (seeAlsoRefs.Any())
            {
                var allDocs = new List<IApiDocumentation>();
                var seeRefTypes = seeAlsoRefs.Select(x => _libraryAssembly.GetType(x.Replace("T:", string.Empty)));

                foreach (var type in seeRefTypes)
                {
                    var name = GetNameWithoutGenerics(type);

                    var typeApiDocs = type.IsEnum
                        ? await GetAllLanguagesEnumDocs(type)
                        : await GetAllLanguagesApiDocs(type);

                    allSeeAlsos.Add(name, typeApiDocs);
                }
            }

            return allSeeAlsos;
        }

        private async Task<IDictionary<string, string>> GetAllLanguageSummaries(XmlNode node)
        {
            return new Dictionary<string, string>
            {
                { Constants.EnglishLanguage, await GetSummaryFromComponentDocsNode(node, Constants.EnglishLanguage) },
                { Constants.ChineseLanguage, await GetSummaryFromComponentDocsNode(node, Constants.ChineseLanguage) }
            };
        }

        private IDictionary<string, string> GetAllLanguagesUrlToComponentDocumentation(Type component)
        {
            // URLs don't need translated

            return new Dictionary<string, string>
            {
                { Constants.EnglishLanguage, GetUrlToComponentDocumentation(component, Constants.EnglishLanguage) },
                { Constants.ChineseLanguage, GetUrlToComponentDocumentation(component, Constants.ChineseLanguage) }
            };
        }

        #endregion Get All Languages Methods

        private IList<Dictionary<string, DemoComponent>> GenerateCompentFromDoc()
        {
            DirectoryInfo demoDirectoryInfo = new DirectoryInfo(_demoDirectory);

            IList<Dictionary<string, DemoComponent>> componentList = null;
            IList<string> demoTypes = null;

            var directories = demoDirectoryInfo.GetFileSystemInfos().Where(x => x.Name != "Components")
                .SelectMany(x => (x as DirectoryInfo).GetFileSystemInfos());

            foreach (FileSystemInfo component in directories)
            {
                if (!(component is DirectoryInfo componentDirectory)) continue;

                FileSystemInfo docDir = componentDirectory.GetFileSystemInfos("doc")?.FirstOrDefault();
                FileSystemInfo demoDir = componentDirectory.GetFileSystemInfos("demo")?.FirstOrDefault();

                Dictionary<string, DemoComponent> componentDic = new Dictionary<string, DemoComponent>();

                if (docDir != null && docDir.Exists)
                {
                    foreach (FileSystemInfo docItem in (docDir as DirectoryInfo).GetFileSystemInfos())
                    {
                        string language = docItem.Name.Replace("index.", "").Replace(docItem.Extension, "");
                        string content = File.ReadAllText(docItem.FullName);
                        (Dictionary<string, string> Meta, string desc, string apiDoc) docData = DocParser.ParseDemoDoc(content);

                        componentDic.Add(language, new DemoComponent()
                        {
                            Category = docData.Meta["category"],
                            Title = docData.Meta["title"],
                            SubTitle = docData.Meta.TryGetValue("subtitle", out string subtitle) ? subtitle : null,
                            Type = docData.Meta["type"],
                            Desc = docData.desc,
                            ApiDoc = docData.apiDoc.Replace("<h2>API</h2>", $"<h2 id=\"API\"><span>API</span><a href=\"{language}/components/{docData.Meta["title"].ToLower()}#API\" class=\"anchor\">#</a></h2>"),
                            Cols = docData.Meta.TryGetValue("cols", out var cols) ? int.Parse(cols) : (int?)null,
                            Cover = docData.Meta.TryGetValue("cover", out var cover) ? cover : null,
                        });
                    }
                }

                if (demoDir != null && demoDir.Exists)
                {
                    foreach (IGrouping<string, FileSystemInfo> demo in (demoDir as DirectoryInfo).GetFileSystemInfos()
                        .GroupBy(x => x.Name
                            .Replace(x.Extension, "")
                            .Replace("-", "")
                            .Replace("_", "")
                            .Replace("Demo", "")
                            .ToLower()))
                    {
                        List<FileSystemInfo> showCaseFiles = demo.ToList();
                        FileSystemInfo razorFile = showCaseFiles.FirstOrDefault(x => x.Extension == ".razor");
                        FileSystemInfo descriptionFile = showCaseFiles.FirstOrDefault(x => x.Extension == ".md");
                        string code = razorFile != null ? File.ReadAllText(razorFile.FullName) : null;

                        string demoType = $"{demoDirectoryInfo.Name}{razorFile.FullName.Replace(demoDirectoryInfo.FullName, "").Replace(Path.DirectorySeparatorChar, '.').Replace(razorFile.Extension, "")}";

                        (DescriptionYaml Meta, string Style, Dictionary<string, string> Descriptions) descriptionContent = descriptionFile != null ? DocParser.ParseDescription(File.ReadAllText(descriptionFile.FullName)) : default;

                        demoTypes ??= new List<string>();
                        demoTypes.Add(demoType);

                        foreach (KeyValuePair<string, string> title in descriptionContent.Meta.Title)
                        {
                            string language = title.Key;
                            List<DemoItem> list = componentDic[language].DemoList ??= new List<DemoItem>();

                            list.Add(new DemoItem()
                            {
                                Title = title.Value,
                                Order = descriptionContent.Meta.Order,
                                Iframe = descriptionContent.Meta.Iframe,
                                Code = code,
                                Description = descriptionContent.Descriptions[title.Key],
                                Name = descriptionFile?.Name.Replace(".md", ""),
                                Style = descriptionContent.Style,
                                Debug = descriptionContent.Meta.Debug,
                                Docs = descriptionContent.Meta.Docs,
                                Type = demoType,
                                Link = descriptionContent.Meta.Link
                            });
                        }
                    }
                }

                componentList ??= new List<Dictionary<string, DemoComponent>>();
                componentList.Add(componentDic);
            }

            return componentList;
        }

        private void WriteFiles(IEnumerable<DemoComponent> components, string language, string output)
        {
            var componentJson = JsonSerializer.Serialize(components, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            var configFileDirectory = Path.Combine(_currentDirectory, output);

            if (!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }

            var configFilePath = Path.Combine(configFileDirectory, $"components.{language}.json");

            if (File.Exists(configFilePath))
            {
                File.Delete(configFilePath);
            }

            File.WriteAllText(configFilePath, componentJson);

            Console.WriteLine("Generate demo file to {0}", configFilePath);
        }
    }
}
