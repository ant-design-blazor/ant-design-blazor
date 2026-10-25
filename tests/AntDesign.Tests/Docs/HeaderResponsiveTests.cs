using System.Globalization;
using System.Linq;
using System.Net.Http;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using AntDesign.Extensions.Localization;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class HeaderResponsiveTests : DocumentationTestBase
{
    public HeaderResponsiveTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<AntDesign.JsInterop.HtmlElement>(JSInteropConstants.GetFirstChildDomInfo, _ => true)
            .SetResult(new AntDesign.JsInterop.HtmlElement { ClientWidth = 56, ClientHeight = 64 });
        var locale = new Mock<ILocalizationService>();
        locale.SetupGet(x => x.CurrentCulture).Returns(CultureInfo.GetCultureInfo("en-US"));
        Services.AddSingleton(locale.Object);
        Services.AddSingleton(new HttpClient());
        Services.AddScoped<DemoService>();
    }

    [Fact]
    public void Desktop_exposes_a_real_additional_submenu_for_the_crowded_breakpoint()
    {
        var cut = RenderComponent<HeaderMenu>();
        Assert.False(cut.FindComponent<AntDesign.Menu>().Instance.DisabledOverflow);
        Assert.NotEmpty(cut.FindAll("[data-menu-overflow-item]"));
        Assert.Single(cut.FindAll("[data-menu-overflow-rest]"));
        Assert.NotNull(cut.Find(".header-more-options.ant-menu-submenu"));
        Assert.NotNull(cut.Find(".header-desktop-utilities"));
        var submenu = cut.FindComponents<SubMenu>().Single(item => item.Instance.Key == "additional");
        // Horizontal submenu content is lazy until its overlay opens. Render
        // the exact fragment in a Menu context instead of asserting hidden DOM.
        var content = RenderComponent<AntDesign.Menu>(parameters => parameters.AddChildContent(submenu.Instance.ChildContent));
        Assert.Equal(3, content.FindComponents<MenuItem>().Count);
        Assert.Contains("Github", content.Markup);
        Assert.Contains("简体中文", content.Markup);
        Assert.Contains("RTL", content.Markup);
    }

    [Fact]
    public void Mobile_renders_additional_actions_inline_not_in_a_hidden_desktop_group()
    {
        var cut = RenderComponent<HeaderMenu>(parameters => parameters.Add(x => x.IsMobile, true));
        Assert.True(cut.FindComponent<AntDesign.Menu>().Instance.DisabledOverflow);
        Assert.Empty(cut.FindAll(".header-desktop-utilities,.header-more-options"));
        Assert.Contains("Github", cut.Markup);
        Assert.Contains("简体中文", cut.Markup);
        Assert.Contains("RTL", cut.Markup);
    }
}
