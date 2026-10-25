using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AntDesign.Docs.Generator;
using AntDesign.Docs.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class LlmsGeneratorTests
{
    private sealed class Input : AdditionalText
    {
        public override string Path { get; }
        private readonly string _text;
        public Input(string path, string text) { Path = path; _text = text; }
        public override SourceText GetText(System.Threading.CancellationToken cancellationToken = default) => SourceText.From(_text);
    }

    [Fact]
    public void Generator_prepares_packed_Markdown_at_compile_time_and_reacts_to_input_changes()
    {
        string Generate(string description)
        {
            GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { new LlmsMarkdownGenerator().AsSourceGenerator() }, additionalTexts: new AdditionalText[] {
                new Input("/docs/meta/components.en-US.json", "[{\"Category\":\"Components\",\"Title\":\"Select\",\"Desc\":\"<p>" + description + "</p>\",\"ApiDoc\":\"<h2>API</h2>\",\"DemoList\":[]}]"),
                new Input("/docs/antd-v6/llms-tokens.json", "{}"),
            });
            driver = driver.RunGenerators(CSharpCompilation.Create("GeneratedTest"));
            var result = driver.GetRunResult();
            Assert.Empty(result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
            var source = result.GeneratedTrees.Single().ToString();
            Assert.Contains("System.Lazy<string>", source);
            Assert.DoesNotContain("FromHtml", source);
            var packed = Regex.Match(source, "Decode\\(\"([^\"]+)\"\\)").Groups[1].Value;
            using var bytes = new MemoryStream(Convert.FromBase64String(packed));
            using var gzip = new GZipStream(bytes, CompressionMode.Decompress);
            using var text = new StreamReader(gzip, Encoding.UTF8);
            return text.ReadToEnd();
        }
        Assert.Contains("First version", Generate("First version"));
        var changed = Generate("Updated version");
        Assert.Contains("Updated version", changed);
        Assert.DoesNotContain("First version", changed);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void General_docs_including_colors_have_generated_Markdown(string language)
    {
        Assert.Contains("colors", ComponentMarkdown.Docs(language));
        Assert.Contains("ColorPalette.Generate", ComponentMarkdown.GetDoc("colors", language));
        Assert.DoesNotContain("order: 7.1", ComponentMarkdown.GetDoc("colors", language));
        Assert.Contains("Ant Design", ComponentMarkdown.GetDoc("introduce", language));
        Assert.Null(ComponentMarkdown.GetDoc("not-a-page", language));
    }

    [Fact]
    public void Missing_token_input_is_a_build_error_not_a_silently_incomplete_document()
    {
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { new LlmsMarkdownGenerator().AsSourceGenerator() }, additionalTexts: new AdditionalText[] {
            new Input("/docs/meta/components.en-US.json", "[]"),
        });
        var result = driver.RunGenerators(CSharpCompilation.Create("MissingTokenInput")).GetRunResult();
        var error = Assert.Single(result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
        Assert.Equal("ADBLLMS001", error.Id);
        Assert.Contains("llms-tokens.json", error.GetMessage());
        Assert.Empty(result.GeneratedTrees);
    }
}
