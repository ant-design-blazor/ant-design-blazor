using System.Linq;
using AntDesign.Docs.Components;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class HomeThemePreviewTests : DocumentationTestBase
{
    [Fact]
    public void Renders_all_themes_and_switches_view_and_theme()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        var swatches = cut.FindAll("button.home-theme-swatch");
        Assert.Equal(13, swatches.Count);
        Assert.Equal(
            new[] { "Ant Design", "MUI", "shadcn", "Bootstrap", "Cartoon", "Dark", "Illustration", "Glass", "Geek", "Lark", "Blossom", "Ant Design V4", "Serene" },
            swatches.Select(item => item.GetAttribute("aria-label")));
        Assert.Equal("true", swatches[0].GetAttribute("aria-selected"));
        Assert.All(swatches.Skip(1), swatch => Assert.Equal("false", swatch.GetAttribute("aria-selected")));
        Assert.NotNull(cut.Find(".home-theme-components"));
        Assert.Contains("@media(max-width:600px)", cut.Find("style").TextContent);

        cut.FindAll(".home-theme-tabs button")[1].Click();
        Assert.Empty(cut.FindAll(".home-theme-components"));
        Assert.NotEmpty(cut.FindAll(".home-theme-dashboard"));

        var lightCss = cut.Find("style").TextContent;
        cut.Find("button[title='Dark']").Click();

        Assert.NotEqual(lightCss, cut.Find("style").TextContent);
        Assert.Contains("--home-theme-primary:#1668dc", cut.Find("style").TextContent);
        Assert.Equal("true", cut.Find("button[title='Dark']").GetAttribute("aria-selected"));
        Assert.Equal("false", cut.Find("button[title='Ant Design']").GetAttribute("aria-selected"));
    }

    [Fact]
    public void Exposes_keyboard_and_screen_reader_theme_controls()
    {
        var cut = Context.RenderComponent<HomeThemePreview>();

        Assert.NotNull(cut.Find("[role='tablist'][aria-label='Preview mode']"));
        Assert.NotNull(cut.Find("[role='tablist'][aria-label='Theme']"));
        Assert.All(cut.FindAll("button.home-theme-swatch"), swatch =>
        {
            Assert.NotEmpty(swatch.GetAttribute("title") ?? string.Empty);
            Assert.NotEmpty(swatch.GetAttribute("aria-label") ?? string.Empty);
        });
    }
}
