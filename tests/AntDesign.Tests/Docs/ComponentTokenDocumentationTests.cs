using System.Linq;
using AntDesign.Docs.Components;
using AntDesign.Docs.Services;
using AntDesign.Theming;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class ComponentTokenDocumentationTests : AntDesignTestBase
{
    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void Component_token_tables_show_the_actual_CSharp_types(string language)
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "collapse").Add(x => x.Language, language));
        var row = cut.FindAll("table:not(.global-token-table) tbody tr")
            .Single(row => row.Children[0].TextContent == "headerPadding");
        Assert.Equal("OneOf<string, double>", row.Children[3].TextContent);
        Assert.DoesNotContain("CSSProperties", cut.Find("table:not(.global-token-table)").TextContent);
        Assert.Equal("CSSProperties['padding']", ComponentTokenCatalog.Get("collapse").Single(token => token.Name == "headerPadding").Type);
    }

    [Fact]
    public void Every_generated_component_token_type_matches_its_real_property()
    {
        // Reflection is confined to the test oracle, never runtime docs.
        var owners = typeof(GeneratedComponentTokenSet).GetProperties()
            .ToDictionary(property => property.Name, property => property.PropertyType, System.StringComparer.Ordinal);
        foreach (var token in GeneratedComponentTokens.All)
        {
            // Keep case-sensitive generated names: qr-code and the legacy
            // reserved qrcode entry deliberately have distinct token classes.
            var ownerName = System.Text.RegularExpressions.Regex.Replace(token.Component, "(^|[-_\\s])([A-Za-z0-9])",
                match => match.Groups[2].Value.ToUpperInvariant());
            var owner = owners[ownerName];
            var propertyName = char.ToUpperInvariant(token.Name[0]) + token.Name.Substring(1);
            var property = owner.GetProperty(propertyName);
            Assert.NotNull(property);
            Assert.Equal(FormatType(property.PropertyType), token.CSharpType);
        }
    }

    private static string FormatType(System.Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(double)) return "double";
        if (type == typeof(bool)) return "bool";
        return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(FormatType)) + ">";
    }

    [Fact]
    public void Qrcode_renders_the_global_tokens_it_uses_in_both_locales()
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters.Add(x => x.Component, "qr-code"));
        Assert.Equal(12, cut.FindAll(".global-token-table tbody tr").Count);
        Assert.Contains("rgba(0, 0, 0, 0.88)", cut.Markup);
        Assert.Contains("Default text color", cut.Markup);

        cut.SetParametersAndRender(parameters => parameters.Add(x => x.Language, "zh-CN"));
        Assert.Equal(12, cut.FindAll(".global-token-table tbody tr").Count);
        Assert.Contains("最深的文本色", cut.Find(".global-token-table").TextContent);
        Assert.Equal(ComponentTokenCatalog.GetGlobal("QRCode").Count, ComponentTokenCatalog.GetGlobal("qr-code").Count);
    }

    [Fact]
    public void Every_documented_global_token_has_a_runtime_default()
    {
        var defaults = new ThemeConfig().GetResolvedTokenValues();
        var names = GeneratedGlobalTokens.All.Select(value => value.Name).Distinct();
        Assert.All(names, name => Assert.True(defaults.TryGetValue(name, out var value) && !string.IsNullOrWhiteSpace(value), $"Missing runtime default for {name}"));
    }
}
