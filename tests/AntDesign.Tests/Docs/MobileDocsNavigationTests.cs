using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using AntDesign.Extensions.Localization;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class MobileDocsNavigationTests : DocumentationTestBase
{
    private readonly BunitJSModuleInterop _module;
    public MobileDocsNavigationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        _module = JSInterop.SetupModule("./_content/AntDesign.Docs/js/mobile-docs-navigation.js");
        _module.SetupVoid("setOpen", _ => true).SetVoidResult();
        _module.SetupVoid("unmount", _ => true).SetVoidResult();
        JSInterop.Setup<AntDesign.JsInterop.Window>(JSInteropConstants.GetWindow, _ => true)
            .SetResult(new AntDesign.JsInterop.Window { InnerWidth = 390 });
        var locale = new Mock<ILocalizationService>();
        locale.SetupGet(x => x.CurrentCulture).Returns(CultureInfo.GetCultureInfo("en-US"));
        Services.AddSingleton(locale.Object);
        Services.AddSingleton(new HttpClient());
        Services.AddScoped<DemoService>();
        Services.GetRequiredService<NavigationManager>().NavigateTo("/en-US/components/menu");
    }

    [Fact]
    public void Closed_navigation_does_not_render_menu_links()
    {
        var cut = RenderComponent<MobileDocsNavigation>();
        Assert.Empty(cut.FindComponents<AntDesign.Drawer>());
        Assert.True(cut.FindComponent<Sider>().Instance.Collapsed);
        Assert.Equal(0, cut.FindComponent<Sider>().Instance.CollapsedWidth);
        Assert.Empty(cut.FindComponents<NavMenu>());
        Assert.Empty(cut.FindAll(".adb-docs-mobile-navigation-mask"));
        Assert.Empty(cut.FindAll(".ant-menu a"));
        Assert.Equal("false", cut.Find(".ant-layout-sider-zero-width-trigger").GetAttribute("aria-expanded"));
    }

    [Fact]
    public async Task Sider_keeps_its_zero_width_trigger_for_expanding_and_collapsing()
    {
        var cut = RenderComponent<MobileDocsNavigation>();
        await cut.Find(".ant-layout-sider-zero-width-trigger").ClickAsync(new());
        Assert.False(cut.FindComponent<Sider>().Instance.Collapsed);
        Assert.Empty(cut.FindComponents<AntDesign.Drawer>());
        Assert.NotEmpty(cut.FindAll(".ant-menu a"));
        Assert.Equal("true", cut.Find(".adb-docs-mobile-navigation-mask").GetAttribute("aria-hidden"));
        Assert.Contains(cut.FindAll("style"), style => style.TextContent.Contains("pointer-events:none"));
        Assert.Equal("true", cut.Find(".ant-layout-sider-zero-width-trigger").GetAttribute("aria-expanded"));
        Assert.Empty(cut.FindAll(".ant-layout-sider-trigger"));
        await cut.Find(".ant-layout-sider-zero-width-trigger").ClickAsync(new());
        cut.WaitForAssertion(() => Assert.Empty(cut.FindComponents<NavMenu>()));
        Assert.True(cut.FindComponent<Sider>().Instance.Collapsed);
        Assert.Empty(cut.FindAll(".adb-docs-mobile-navigation-mask"));
    }

    [Fact]
    public async Task Choosing_a_navigation_item_closes_and_removes_the_menu()
    {
        var cut = RenderComponent<MobileDocsNavigation>();
        await cut.Find(".ant-layout-sider-zero-width-trigger").ClickAsync(new());
        await cut.Find(".ant-menu-item").ClickAsync(new());
        cut.WaitForAssertion(() => Assert.True(cut.FindComponent<Sider>().Instance.Collapsed));
        Assert.Empty(cut.FindAll(".ant-menu a"));
    }

    [Fact]
    public async Task Outside_click_callback_closes_the_sider_and_removes_the_transparent_mask()
    {
        var cut = RenderComponent<MobileDocsNavigation>();
        await cut.Find(".ant-layout-sider-zero-width-trigger").ClickAsync(new());
        Assert.Single(cut.FindAll(".adb-docs-mobile-navigation-mask"));
        await cut.Instance.CloseFromOutside();
        cut.WaitForAssertion(() => Assert.True(cut.FindComponent<Sider>().Instance.Collapsed));
        Assert.Empty(cut.FindAll(".adb-docs-mobile-navigation-mask"));
        Assert.Equal("false", cut.Find(".ant-layout-sider-zero-width-trigger").GetAttribute("aria-expanded"));
        Assert.Contains(_module.Invocations, call => call.Identifier == "setOpen" && Equals(call.Arguments[1], false));
    }

    [Fact]
    public async Task Disposing_navigation_removes_its_document_listener()
    {
        var cut = RenderComponent<MobileDocsNavigation>();
        await cut.Find(".ant-layout-sider-zero-width-trigger").ClickAsync(new());
        await cut.Instance.DisposeAsync();
        Assert.Single(_module.Invocations.Where(call => call.Identifier == "unmount"));
    }
}
