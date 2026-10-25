using System.Linq;
using AntDesign.Docs.Components;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class HomeBannerBackgroundTests : DocumentationTestBase
{
    [Fact]
    public void Banner_uses_three_decorative_bubbles_with_upstream_geometry_and_theme_tokens()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var module = JSInterop.SetupModule("./_content/AntDesign.Docs/js/home-background.js");
        module.SetupVoid("mount", _ => true).SetVoidResult();
        var cut = RenderComponent<HomeBannerBackground>();
        var root = cut.Find(".home-luminous-background[aria-hidden=true]");
        var bubbles = root.QuerySelectorAll("[data-desc=luminous-bubble]");
        Assert.Equal(3, bubbles.Length);
        Assert.Equal(new[] { "2", "1", "1" }, bubbles.Select(x => x.GetAttribute("data-offset-x")));
        Assert.Equal(new[] { "1", "1", "2" }, bubbles.Select(x => x.GetAttribute("data-offset-y")));
        Assert.Contains("background:#ee35f1", bubbles[0].GetAttribute("style"));
        Assert.Contains("left:30vw;top:80vh;background:#5939dc", bubbles[1].GetAttribute("style"));
        Assert.Contains("left:100vw;top:50vh;background:#00d6ff", bubbles[2].GetAttribute("style"));
        var css = cut.Find("style").TextContent;
        Assert.Contains("var(--ant-color-bg-container)", css);
        Assert.Contains("filter:blur(100px)", css);
        Assert.Contains("transition:all 5s ease-in-out", css);
        Assert.Contains("[data-theme=dark]", css);
        Assert.Contains("prefers-reduced-motion:reduce", css);
        Assert.Empty(cut.FindAll("img,svg,button,a"));
        Assert.Single(module.Invocations["mount"]);
    }

    [Fact]
    public async System.Threading.Tasks.Task Disposing_the_banner_stops_its_client_animation()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var module = JSInterop.SetupModule("./_content/AntDesign.Docs/js/home-background.js");
        module.SetupVoid("mount", _ => true).SetVoidResult();
        module.SetupVoid("unmount", _ => true).SetVoidResult();
        var cut = RenderComponent<HomeBannerBackground>();
        await cut.Instance.DisposeAsync();
        await cut.Instance.DisposeAsync();
        Assert.Single(module.Invocations["unmount"]);
    }
}
