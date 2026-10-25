using System.Linq;
using AntDesign.Docs.Components;
using AntDesign.Docs.Services;
using AntDesign.Theming;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class HomeThemePreviewTests : DocumentationTestBase
{
    public HomeThemePreviewTests()
    {
        Context.JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 300, Height = 32 });
    }
    [Fact]
    public void Renders_all_themes_and_switches_view_and_theme()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        var swatches = cut.FindAll("button.home-theme-swatch");
        Assert.Equal(13, swatches.Count);
        Assert.Equal(
            GeneratedPreviewThemes.All.Select(theme => theme.Name),
            swatches.Select(item => item.GetAttribute("aria-label")));
        Assert.Equal("true", swatches[0].GetAttribute("aria-selected"));
        Assert.All(swatches.Skip(1), swatch => Assert.Equal("false", swatch.GetAttribute("aria-selected")));
        Assert.NotNull(cut.Find(".home-theme-components"));
        Assert.Contains("@media(max-width:600px)", cut.Find("style").TextContent);
        Assert.Contains("@media(width < 768px)", cut.Find("#home-theme-preview-style").TextContent);
        Assert.DoesNotContain("100vw", cut.Find("#home-theme-preview-style").TextContent);
        Assert.Contains("overflow-x:auto", cut.Find("#home-theme-preview-style").TextContent);
        Assert.Contains("flex-shrink:0", cut.Find("#home-theme-preview-style").TextContent);

        cut.FindAll(".home-theme-tabs .ant-segmented-item-label")[1].Click();
        Assert.Empty(cut.FindAll(".home-theme-components"));
        Assert.NotEmpty(cut.FindAll(".home-theme-dashboard"));

        var lightCss = cut.Find("#home-theme-preview-style").TextContent;
        cut.Find("button[title='Dark']").Click();

        cut.WaitForAssertion(() => Assert.NotEqual(lightCss, cut.Find("#home-theme-preview-style").TextContent));
        Assert.Contains("--home-theme-primary:#1668dc", cut.Find("#home-theme-preview-style").TextContent);
        Assert.Equal("true", cut.Find("button[title='Dark']").GetAttribute("aria-selected"));
        Assert.Equal("false", cut.Find("button[title='Ant Design']").GetAttribute("aria-selected"));
    }

    [Fact]
    public void Exposes_keyboard_and_screen_reader_theme_controls()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        Assert.Equal(2, cut.FindAll(".home-theme-tabs input[type=radio]").Count);
        Assert.NotNull(cut.Find("[role='tablist'][aria-label='Theme']"));
        Assert.All(cut.FindAll("button.home-theme-swatch"), swatch =>
        {
            Assert.NotEmpty(swatch.GetAttribute("title") ?? string.Empty);
            Assert.NotEmpty(swatch.GetAttribute("aria-label") ?? string.Empty);
            Assert.Equal("tab", swatch.GetAttribute("role"));
        });
    }

    [Fact]
    public void Every_theme_uses_its_generated_global_and_component_configuration()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();
        foreach (var definition in GeneratedPreviewThemes.All)
        {
            cut.Find($"button[data-theme-key='{definition.Key}']").Click();
            var provider = cut.FindComponent<ConfigProvider>().Instance;
            Assert.False(provider.ThemeConfig.Inherit);
            Assert.Equal(definition.Key, provider.ThemeConfig.Key);
            Assert.Equal(definition.Algorithm, provider.ResolvedAlgorithm);
            Assert.Equal(definition.Tokens.Count, provider.ThemeConfig.GetExplicitTokenValues().Count);
            Assert.Equal(definition.Components.Count, provider.ThemeConfig.Components.Count);
            foreach (var component in definition.Components)
                foreach (var token in component.Value)
                    Assert.Equal(token.Value, provider.ThemeConfig.Components[component.Key][token.Key]);
        }
        Assert.NotEmpty(cut.FindAll("svg title").Where(title => title.TextContent == "Serene Icon"));
        Assert.All(cut.FindComponents<AntDesign.Progress>().Where(p => p.Instance.Type == ProgressType.Line), p => Assert.True(p.Instance.ShowInfo));
        Assert.Empty(cut.FindAll(".home-theme-progress-list > span"));
        Assert.NotNull(cut.Find(".ant-space-compact"));
        Assert.Equal(2, cut.FindAll(".home-theme-compact button").Count);
        Assert.NotNull(cut.Find(".adb-home-theme-preview-main-card > .ant-border-beam[aria-hidden=true]"));
        Assert.NotNull(cut.Find(".home-theme-segments .anticon-message"));
        Assert.NotNull(cut.Find(".home-theme-segments .anticon-mail"));
    }

    [Fact]
    public void Progress_uses_the_v6_rail_and_inline_circle_hides_its_label()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();
        Assert.All(cut.FindAll(".home-theme-progress-list .ant-progress-rail"), rail => Assert.Contains("height:8px", rail.GetAttribute("style")));
        Assert.Empty(cut.FindAll(".ant-progress-inline-circle .ant-progress-text"));
        Assert.Equal("15", cut.Find(".ant-progress-inline-circle .ant-progress-circle-rail").GetAttribute("stroke-width"));
    }

    [Fact]
    public void Copied_theme_contains_complete_CSharp_configuration()
    {
        var config = PreviewThemeConfigFactory.Create("mui");
        config.Inherit = false;
        var code = PreviewThemeCodeFormatter.Format(config);
        Assert.Contains("var theme = new ThemeConfig", code);
        Assert.Contains("[\"colorPrimary\"] = \"#1976d2\"", code);
        Assert.Contains("[\"Button\"] = new Dictionary<string, string>", code);
        Assert.Contains("[\"paddingInline\"] = \"16\"", code);
        Assert.Contains("Inherit = false", code);
        var tree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);
        Assert.Empty(tree.GetDiagnostics().Where(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error));
    }
}
