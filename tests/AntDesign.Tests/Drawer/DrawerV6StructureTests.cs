using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Drawer;

public sealed class DrawerV6StructureTests : AntDesignTestBase
{
    public DrawerV6StructureTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void OpenDrawerUsesV6DialogAndHeaderSemanticStructure()
    {
        var drawer = RenderComponent<AntDesign.Drawer>(parameters => parameters
            .Add(component => component.Visible, true)
            .Add(component => component.Title, "Drawer title")
            .Add(component => component.Extra, (RenderFragment)(builder => builder.AddContent(0, "Extra action")))
            .Add(component => component.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "Drawer body")))
            .Add(component => component.Footer, (RenderFragment)(builder => builder.AddContent(0, "Drawer footer"))));

        var dialog = drawer.Find(".ant-drawer-section[role='dialog'][aria-modal='true']");
        dialog.QuerySelector(".ant-drawer-header > .ant-drawer-header-title > .ant-drawer-close").Should().NotBeNull();
        dialog.QuerySelector(".ant-drawer-header-title > .ant-drawer-title")!.TextContent.Trim().Should().Be("Drawer title");
        dialog.QuerySelector(".ant-drawer-body")!.TextContent.Trim().Should().Be("Drawer body");
        dialog.QuerySelector(".ant-drawer-extra")!.TextContent.Trim().Should().Be("Extra action");
        dialog.QuerySelector(".ant-drawer-footer")!.TextContent.Trim().Should().Be("Drawer footer");
        drawer.Find(".ant-drawer-mask").Should().NotBeNull();
        drawer.Find(".ant-drawer-content-wrapper > .ant-drawer-section").Should().NotBeNull();
        drawer.FindAll(".ant-drawer-content").Should().BeEmpty();
    }
}
