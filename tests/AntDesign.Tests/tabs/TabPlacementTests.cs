using Bunit;
using Xunit;

namespace AntDesign.Tests.Tabs;

public sealed class TabPlacementTests : AntDesignTestBase
{
    [Fact]
    public void Start_and_end_map_to_logical_sides_in_ltr()
    {
        var start = Context.RenderComponent<AntDesign.Tabs>(parameters => parameters
            .Add(component => component.TabPlacement, AntDesign.TabPlacement.Start));
        var end = Context.RenderComponent<AntDesign.Tabs>(parameters => parameters
            .Add(component => component.TabPlacement, AntDesign.TabPlacement.End));

        Assert.Contains("ant-tabs-left", start.Find(".ant-tabs").ClassList);
        Assert.Contains("ant-tabs-right", end.Find(".ant-tabs").ClassList);
    }

    [Fact]
    public void Start_and_end_reverse_sides_in_rtl()
    {
        var start = Context.RenderComponent<AntDesign.ConfigProvider>(parameters => parameters
            .Add(component => component.Direction, "RTL")
            .AddChildContent<AntDesign.Tabs>(tabs => tabs
                .Add(component => component.TabPlacement, AntDesign.TabPlacement.Start)));
        var end = Context.RenderComponent<AntDesign.ConfigProvider>(parameters => parameters
            .Add(component => component.Direction, "RTL")
            .AddChildContent<AntDesign.Tabs>(tabs => tabs
                .Add(component => component.TabPlacement, AntDesign.TabPlacement.End)));

        Assert.Contains("ant-tabs-right", start.Find(".ant-tabs").ClassList);
        Assert.Contains("ant-tabs-left", end.Find(".ant-tabs").ClassList);
    }
}
