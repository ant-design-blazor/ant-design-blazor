using AntDesign.JsInterop;
using AntDesign.Core.JsInterop.Modules.Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Select;

public class SelectApiTests : AntDesignTestBase
{
    public SelectApiTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.SetupVoid(JSInteropConstants.SetDomAttribute, _ => true).SetVoidResult();
        JSInterop.SetupVoid(JSInteropConstants.AddPreventKeys, _ => true).SetVoidResult();
        JSInterop.SetupVoid(JSInteropConstants.AddPreventEnterOnOverlayVisible, _ => true).SetVoidResult();
        JSInterop.SetupVoid(JSInteropConstants.SelectHelper.ScrollToIndex, _ => true).SetVoidResult();
        JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true).SetResult(new DomRect { Width = 120 });
        JSInterop.Setup<HtmlElement>(JSInteropConstants.GetDomInfo, _ => true).SetResult(new HtmlElement());
        JSInterop.Setup<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer, _ => true)
            .SetResult(new OverlayPosition { Placement = Placement.BottomLeft, ZIndex = 5000 });
    }

    [Fact]
    public void Select_forwards_search_input_attributes_and_root_mouse_events()
    {
        var mouseDownCount = 0;
        var clickCount = 0;
        var component = RenderComponent<AntDesign.Select<string, string>>(parameters => parameters
            .Add(select => select.TabIndex, 3)
            .Add(select => select.MaxLength, 12)
            .Add(select => select.OnMouseDown, EventCallback.Factory.Create<MouseEventArgs>(this, _ => mouseDownCount++))
            .Add(select => select.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, _ => clickCount++)));

        var input = component.Find(".ant-select input");
        Assert.Equal("3", input.GetAttribute("tabindex"));
        Assert.Equal("12", input.GetAttribute("maxlength"));

        component.Find(".ant-select").TriggerEvent("onmousedown", new MouseEventArgs());
        component.Find(".ant-select").TriggerEvent("onclick", new MouseEventArgs());
        Assert.Equal(1, mouseDownCount);
        Assert.Equal(1, clickCount);
    }

    [Fact]
    public void Default_active_first_option_highlights_without_selecting()
    {
        var options = new[] { "jack", "lucy" };
        var component = RenderComponent<AntDesign.Select<string, string>>(parameters => parameters
            .Add(select => select.DataSource, options)
            .Add(select => select.ItemValue, item => item)
            .Add(select => select.ItemLabel, item => item)
            .Add(select => select.DefaultActiveFirstOption, true));

        Assert.Equal(string.Empty, component.Find(".ant-select-content").TextContent.Trim());
        Assert.Empty(component.FindAll(".ant-select-item-option-selected"));
        Assert.Equal("jack", component.Find(".ant-select-item-option-active").TextContent.Trim());
    }

    [Fact]
    public void Direction_override_applies_to_select_and_popup_placement()
    {
        var component = RenderComponent<AntDesign.Select<string, string>>(parameters => parameters
            .Add(select => select.Direction, "rtl")
            .Add(select => select.Placement, Placement.BottomLeft)
            .Add(select => select.DefaultOpen, true)
            .Add(select => select.DataSource, new[] { "jack" })
            .Add(select => select.ItemValue, item => item)
            .Add(select => select.ItemLabel, item => item));

        Assert.Contains("ant-select-rtl", component.Find(".ant-select").ClassList);
        Assert.Contains("ant-select-dropdown-rtl", component.Find(".ant-select-dropdown").ClassList);
        Assert.Equal(Placement.BottomRight, component.FindComponent<AntDesign.Internal.OverlayTrigger>().Instance.PlacementWithRTL);
    }

    [Fact]
    public void Ltr_direction_override_wins_over_rtl_config_provider()
    {
        var component = RenderComponent<AntDesign.ConfigProvider>(parameters => parameters
            .Add(provider => provider.Direction, "RTL")
            .AddChildContent<AntDesign.Select<string, string>>(select => select
                .Add(item => item.Direction, "ltr")
                .Add(item => item.Placement, Placement.BottomLeft)
                .Add(item => item.DefaultOpen, true)
                .Add(item => item.DataSource, new[] { "jack" })
                .Add(item => item.ItemValue, item => item)
                .Add(item => item.ItemLabel, item => item)));

        Assert.DoesNotContain("ant-select-rtl", component.Find(".ant-select").ClassList);
        Assert.DoesNotContain("ant-select-dropdown-rtl", component.Find(".ant-select-dropdown").ClassList);
        Assert.Equal(Placement.BottomLeft, component.FindComponent<AntDesign.Internal.OverlayTrigger>().Instance.PlacementWithRTL);
    }
}
