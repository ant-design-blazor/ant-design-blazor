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

    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void Undescribed_component_tokens_keep_the_description_cell_empty(string language)
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "button")
            .Add(x => x.Language, language));

        foreach (var (name, descriptionZh) in new[]
                 {
                     ("dashedBgDisabled", "type='dashed' 禁用状态下的背景颜色"),
                     ("defaultBgDisabled", "type='default' 禁用状态下的背景颜色"),
                 })
        {
            var row = cut.FindAll("table:not(.global-token-table) tbody tr")
                .Single(row => row.Children[0].TextContent == name);
            Assert.Equal(language == "zh-CN" ? descriptionZh : string.Empty, row.Children[1].TextContent.Trim());
        }
    }

    [Fact]
    public void DatePicker_keeps_documented_inherited_addon_background_token()
    {
        var token = ComponentTokenCatalog.Get("date-picker").Single(token => token.Name == "addonBg");
        Assert.Equal("Background color of addon", token.DescriptionEn);
        Assert.Equal("rgba(0,0,0,0.02)", token.DefaultValue);

        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "date-picker").Add(x => x.Language, "en-US"));
        Assert.Contains(cut.FindAll("#design-token table:not(.global-token-table) tbody tr"), row => row.Children[0].TextContent == "addonBg");
    }

    [Fact]
    public void TimePicker_reuses_the_React_DatePicker_token_tables()
    {
        Assert.Equal(ComponentTokenCatalog.Get("date-picker").Select(token => token.Name), ComponentTokenCatalog.Get("time-picker").Select(token => token.Name));
        Assert.Equal(ComponentTokenCatalog.GetGlobal("date-picker").Select(token => token.Name), ComponentTokenCatalog.GetGlobal("time-picker").Select(token => token.Name));

        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "time-picker").Add(x => x.Language, "en-US"));
        Assert.Equal(40, cut.FindAll("#design-token table:not(.global-token-table) tbody tr").Count);
        Assert.Equal(63, cut.FindAll("#design-token .global-token-table tbody tr").Count);
    }

    [Theory]
    [InlineData("en-US", "React v6 does not define component-specific Design Tokens")]
    [InlineData("zh-CN", "React v6 未为此组件定义专属 Design Token")]
    public void Components_without_registered_tokens_explain_the_empty_token_section(string language, string expected)
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "affix")
            .Add(x => x.Language, language));

        Assert.Contains(expected, cut.Find("#design-token").TextContent);
        Assert.Equal($"/{language}/docs/customize-theme#global-token-reference", cut.Find("#design-token a").GetAttribute("href"));
        Assert.Empty(cut.FindAll("#design-token table"));
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void Icon_empty_token_section_matches_the_React_heading_only(string language)
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "icon")
            .Add(x => x.Language, language));

        Assert.Equal("Design Token", cut.Find("#design-token h2").TextContent.Trim());
        Assert.Empty(cut.FindAll("#design-token p, #design-token table"));
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

    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public void AutoComplete_renders_the_shared_Select_component_and_global_tokens(string language)
    {
        var cut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "AutoComplete")
            .Add(x => x.Language, language));

        Assert.Equal(24, cut.FindAll("#design-token table:not(.global-token-table) tbody tr").Count);
        Assert.Equal(61, cut.FindAll("#design-token .global-token-table tbody tr").Count);
        Assert.DoesNotContain("does not define component-specific Design Tokens", cut.Find("#design-token").TextContent);
        Assert.Equal(ComponentTokenCatalog.Get("Select").Select(token => token.Name), ComponentTokenCatalog.Get("AutoComplete").Select(token => token.Name));
        Assert.Equal(ComponentTokenCatalog.GetGlobal("Select").Select(token => token.Name), ComponentTokenCatalog.GetGlobal("AutoComplete").Select(token => token.Name));
        Assert.Equal(24, ComponentTokenCatalog.Get("auto-complete").Count);

        var routeKeyCut = RenderComponent<ComponentTokenDocumentation>(parameters => parameters
            .Add(x => x.Component, "auto-complete")
            .Add(x => x.Language, language));
        Assert.Equal(24, routeKeyCut.FindAll("#design-token table:not(.global-token-table) tbody tr").Count);
        Assert.Equal(61, routeKeyCut.FindAll("#design-token .global-token-table tbody tr").Count);
    }

    [Fact]
    public void Every_documented_global_token_has_a_runtime_default()
    {
        var defaults = new ThemeConfig().GetResolvedTokenValues();
        var names = GeneratedGlobalTokens.All.Select(value => value.Name).Distinct();
        Assert.All(names, name => Assert.True(defaults.TryGetValue(name, out var value) && !string.IsNullOrWhiteSpace(value), $"Missing runtime default for {name}"));
    }
}
