using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Colors;
using AntDesign.Docs.Components;
using AntDesign.Docs.Pages;
using AntDesign.Docs.Services;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class ColorSpecificationTests : DocumentationTestBase
{
    [Theory]
    [InlineData("en-US", "Colors", "Dust Red")]
    [InlineData("zh-CN", "色彩", "薄暮")]
    public void Page_contains_the_full_React_palette_neutral_strip_tools_and_token_comparison(string language, string title, string red)
    {
        var cut = RenderComponent<AntDesign.Docs.Pages.Colors>(p => p.Add(x => x.Locale, language));
        Assert.Equal(title, cut.Find("h1").TextContent);
        Assert.Equal(12, cut.FindAll(".color-palettes .color-palette").Count);
        Assert.Contains(red, cut.Find(".color-title").TextContent);
        Assert.Equal(120, cut.FindAll(".color-palettes .main-color-item").Count);
        Assert.Equal(10, cut.FindAll(".color-palette-horizontal .main-color[aria-label=gray] button").Count);
        Assert.Equal(13, ColorSpecCatalog.Neutral.Length);
        Assert.Equal(10, ColorSpecCatalog.NeutralPreviewCount);
        Assert.Equal(3, cut.FindComponents<AntDesign.ColorPicker>().Count);
        Assert.Equal(7, cut.FindAll(".color-token-row").Count);
        Assert.Equal(10, cut.FindAll(".color-palette-horizontal-dark .main-color-item").Count);
        Assert.Equal(new[] { "red", "volcano", "orange", "lime", "gold", "yellow", "green", "cyan", "blue", "geekblue", "purple", "magenta" }, ColorSpecCatalog.Palettes.Select(p => p.Name));
        Assert.EndsWith($"{language}/docs/colors.md", cut.Find("a[href$='colors.md']").GetAttribute("href"));
    }

    [Fact]
    public void Swatches_copy_the_actual_value_and_report_success()
    {
        var copy = JSInterop.SetupVoid("navigator.clipboard.writeText", "#e6f4ff").SetVoidResult();
        var cut = RenderComponent<ColorSwatches>(p => p.Add(x => x.Name, "blue").Add(x => x.Colors, ColorPalette.Generate("#1677ff")));
        cut.Find("button").Click();
        cut.WaitForAssertion(() => Assert.Equal("Copied: #e6f4ff", cut.Find("[role=status]").TextContent));
        Assert.Single(copy.Invocations);
    }

    [Fact]
    public async Task Real_picker_changes_recompute_the_whole_palette_and_dark_background()
    {
        var cut = RenderComponent<ColorPaletteTool>(p => p.Add(x => x.Dark, true));
        var pickers = cut.FindComponents<AntDesign.ColorPicker>();
        await cut.InvokeAsync(() => pickers[0].Instance.ValueChanged.InvokeAsync("#808080"));
        cut.WaitForAssertion(() => Assert.Contains("Saturation", cut.Find(".color-palette-picker-validation").TextContent));
        var colors = cut.FindComponent<ColorSwatches>().Instance.Colors;
        Assert.Equal(ColorPalette.Generate("#808080", true).Distinct(), colors);
        await cut.InvokeAsync(() => cut.FindComponents<AntDesign.ColorPicker>()[1].Instance.ValueChanged.InvokeAsync("#30243b"));
        Assert.Equal(ColorPalette.Generate("#808080", true, "#30243b").Distinct(), cut.FindComponent<ColorSwatches>().Instance.Colors);
    }
}
