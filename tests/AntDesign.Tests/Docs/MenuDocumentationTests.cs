using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class MenuDocumentationTests : DocumentationTestBase
{
    [Fact]
    public async Task Real_demos_render_inside_a_local_scroll_wrapper()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, _ => true)
            .SetResult(new AntDesign.JsInterop.HtmlElement { ClientWidth = 100 });
        var locale = new Mock<ILocalizationService>();
        locale.SetupGet(x => x.CurrentCulture).Returns(CultureInfo.GetCultureInfo("en-US"));
        Services.AddSingleton(locale.Object);
        Services.AddSingleton(new HttpClient());
        Services.AddScoped<DemoService>();
        var menu = await Services.GetRequiredService<DemoService>().GetComponentAsync("components/menu");
        var demo = menu.DemoList.Single(x => x.Name == "responsive");
        var cut = RenderComponent<AntDesign.Docs.Shared.CodeBox>(p => p.Add(x => x.Demo, demo).Add(x => x.ComponentName, "Menu"));
        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".code-box-demo > .demo .ant-menu")));
        Assert.Matches(@"\.code-box-demo\s*>\s*\.demo\s*\{\s*overflow:\s*auto;?\s*\}", string.Join("\n", cut.FindAll("style").Select(x => x.TextContent)));
    }

    // React index explicitly marks seven debug examples; Blazor-only examples
    // do not compensate for a missing React name.
    [Theory]
    [InlineData("en-US")]
    [InlineData("zh-CN")]
    public async Task Every_React_menu_example_has_a_registered_implementation(string language)
    {
        var locale = new Mock<ILocalizationService>();
        locale.SetupGet(x => x.CurrentCulture).Returns(CultureInfo.GetCultureInfo(language));
        var service = new DemoService(locale.Object, new HttpClient(), Services.GetService(typeof(Microsoft.AspNetCore.Components.NavigationManager)) as Microsoft.AspNetCore.Components.NavigationManager);
        var menu = await service.GetComponentAsync("components/menu");
        Assert.NotNull(menu);
        var publicNames = new[] { "horizontal", "inline", "inline-collapsed", "tooltip", "sider-current", "vertical", "theme", "submenu-theme", "switch-mode", "style-class", "custom-popup-render" };
        var debugNames = new[] { "horizontal-dark", "style-debug", "menu-v4", "component-token", "extra-style", "extra-collapsed-debug", "collapsed-icon-debug" };
        foreach (var name in publicNames.Concat(debugNames))
        {
            var demo = Assert.Single(menu.DemoList.Where(x => x.Name == name));
            Assert.Equal(debugNames.Contains(name), demo.Debug);
            Assert.NotEqual(typeof(AntDesign.Template), service.GetShowCase(demo.Type));
            Assert.False(string.IsNullOrWhiteSpace(demo.Description));
            Assert.False(string.IsNullOrWhiteSpace(demo.Code));
        }
    }
}
