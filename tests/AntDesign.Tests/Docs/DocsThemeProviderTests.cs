using AntDesign.Docs.Components;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class DocsThemeProviderTests : DocumentationTestBase
{
    public DocsThemeProviderTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 300, Height = 32 });
    }

    [Fact]
    public async System.Threading.Tasks.Task Root_palette_is_owned_by_the_selected_stylesheet_not_a_light_inline_override()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = RenderComponent<DocsThemeProvider>(parameters => parameters
            .AddChildContent<HomeBannerBackground>());
        Assert.Equal(string.Empty, cut.FindComponent<ConfigProvider>().Instance.ThemeStyle);
        await cut.Instance.ApplyAsync("dark", false);
        Assert.Equal("dark", cut.FindComponent<ConfigProvider>().Instance.ResolvedAlgorithm);
        Assert.Equal("dark", cut.Find(".ant-theme").GetAttribute("data-theme"));
        Assert.DoesNotContain("--ant-color-bg-container", cut.Find(".ant-theme").GetAttribute("style") ?? string.Empty);
        await cut.Instance.ApplyAsync("light", true);
        Assert.Equal("compact", cut.FindComponent<ConfigProvider>().Instance.ResolvedAlgorithm);
        Assert.Equal("light-compact", cut.Instance.Theme.Key);
    }

    [Fact]
    public async System.Threading.Tasks.Task Site_appearance_changes_select_the_corresponding_preview_but_local_preview_changes_stay_local()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = RenderComponent<DocsThemeProvider>(parameters => parameters
            .AddChildContent<HomeThemePreview>());
        await cut.Instance.ApplyAsync("dark", false);
        cut.WaitForAssertion(() => Assert.Equal("true", cut.Find("button[title=Dark]").GetAttribute("aria-selected")));
        cut.Find("button[title=MUI]").Click();
        Assert.Equal("dark", cut.Instance.Appearance);
        cut.WaitForAssertion(() => Assert.Equal("true", cut.Find("button[title=MUI]").GetAttribute("aria-selected")));
        await cut.Instance.ApplyAsync("light", false);
        cut.WaitForAssertion(() => Assert.Equal("true", cut.Find("button[data-theme-key=light]").GetAttribute("aria-selected")));
    }
}
