using AntDesign.Docs.Services;
using AntDesign.Docs.Generator;
using Xunit;
using System;
using System.Text.RegularExpressions;
using AntDesign.Theming;

namespace AntDesign.Tests.Docs;

public sealed class ComponentMarkdownTests
{
    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void Every_compiled_component_has_a_complete_export(string language)
    {
        var names = ComponentMarkdown.Components(language);
        Assert.True(names.Length > 60);
        foreach (var name in names)
        {
            var md = ComponentMarkdown.Get(name, language);
            Assert.StartsWith("# " + name, md);
            Assert.Contains("## API", md);
            Assert.Contains("## Semantic DOM", md);
            Assert.Contains("## Design Token", md);
            Assert.DoesNotContain("\uE000", md);
            var key = Regex.Replace(Regex.Replace(name, "([A-Z]+)([A-Z][a-z])", "$1-$2"), "([a-z0-9])([A-Z])", "$1-$2").ToLowerInvariant();
            foreach (var token in ComponentTokenCatalog.Get(key))
                Assert.Contains("| " + token.Name + " |", md);
            foreach (var node in SemanticDomCatalog.Get(key))
                Assert.Contains("| " + node.Name + " |", md);
        }
    }
    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void Select_document_includes_real_Blazor_API_tokens_semantics_and_Razor_demos(string language)
    {
        var md = ComponentMarkdown.Get("select", language);
        Assert.StartsWith("# Select", md);
        Assert.Contains("@using AntDesign", md);
        Assert.Contains("| Variant |", md);
        Assert.Contains("## Semantic DOM", md);
        Assert.Contains("popup.root", md);
        Assert.Contains("## Design Token", md);
        Assert.Contains("optionHeight", md);
        Assert.Contains("```razor", md);
        Assert.Contains("<Select", md);
        Assert.DoesNotContain("<table", md);
        Assert.DoesNotContain("<h2", md);
        Assert.DoesNotContain("<!--", md);
    }

    [Fact]
    public void Multiword_component_names_resolve_the_same_semantics_and_tokens_as_the_page()
    {
        Assert.Contains(".ant-input-number", ComponentMarkdown.Get("inputnumber", "en-US"));
        Assert.Contains("controlWidth", ComponentMarkdown.Get("inputnumber", "en-US"));
        Assert.Contains(".ant-float-btn", ComponentMarkdown.Get("floatbutton", "en-US"));
        Assert.Contains(".ant-select", ComponentMarkdown.Get("treeselect", "en-US"));
    }

    [Fact]
    public void Unsupported_routes_return_no_document()
    {
        Assert.Null(ComponentMarkdown.Get("not-a-component", "en-US"));
        Assert.Null(ComponentMarkdown.Get("select", "../"));
    }

    [Fact]
    public void Converter_preserves_generic_types_and_escapes_table_pipes()
    {
        var md = LlmsMarkdownBuilder.FromHtml("<h2>API</h2><table><tr><th>Type</th></tr><tr><td><code>RenderFragment&lt;T&gt; | null</code></td></tr></table>");
        Assert.Contains("## API", md);
        Assert.Contains("| `RenderFragment<T> \\| null` |", md);
        Assert.Contains("| --- |", md);
        Assert.Equal("``AntDesign.Select`2``", LlmsMarkdownBuilder.FromHtml("<code>AntDesign.Select`2</code>"));
    }
}
