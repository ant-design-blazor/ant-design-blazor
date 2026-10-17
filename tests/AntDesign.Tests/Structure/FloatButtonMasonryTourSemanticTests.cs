using System.Collections.Generic;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Structure;

public sealed class FloatButtonMasonryTourSemanticTests : AntDesignTestBase
{
    public FloatButtonMasonryTourSemanticTests()
    {
        JSInterop.SetupVoid("AntDesign.interop.domManipulationHelper.disableBodyScroll");
    }

    [Fact]
    public void FloatButton_applies_semantic_classes_and_styles_to_root_icon_and_content()
    {
        var cut = RenderComponent<FloatButton>(parameters => parameters
            .Add(component => component.Icon, "search")
            .Add(component => component.Content, "Search")
            .Add(component => component.ClassNames, new Dictionary<string, string>
            {
                ["root"] = "custom-root",
                ["icon"] = "custom-icon",
                ["content"] = "custom-content",
            })
            .Add(component => component.Styles, new Dictionary<string, string>
            {
                ["root"] = "outline:1px solid red",
                ["icon"] = "color:red",
                ["content"] = "font-weight:bold",
            }));

        Assert.Contains("custom-root", cut.Find(".ant-float-btn").ClassList);
        Assert.Contains("custom-icon", cut.Find(".ant-float-btn-icon").ClassList);
        Assert.Contains("custom-content", cut.Find(".ant-float-btn-content").ClassList);
        Assert.Contains("outline:1px solid red", cut.Find(".ant-float-btn").GetAttribute("style"));
        Assert.Contains("color:red", cut.Find(".ant-float-btn-icon").GetAttribute("style"));
        Assert.Contains("font-weight:bold", cut.Find(".ant-float-btn-content").GetAttribute("style"));
    }

    [Fact]
    public void Masonry_applies_semantic_classes_and_styles_to_root_and_items()
    {
        var items = new[] { new MasonryItem<int> { Key = "item", Data = 1 } };
        var cut = RenderComponent<Masonry<int>>(parameters => parameters
            .Add(component => component.Items, items)
            .Add(component => component.ItemRender, item => builder => builder.AddContent(0, item.Data))
            .Add(component => component.ClassNames, new Dictionary<string, string>
            {
                ["root"] = "custom-root",
                ["item"] = "custom-item",
            })
            .Add(component => component.Styles, new Dictionary<string, string>
            {
                ["root"] = "background-color:#fafafa",
                ["item"] = "opacity:.8",
            }));

        Assert.Contains("custom-root", cut.Find(".ant-masonry").ClassList);
        Assert.Contains("custom-item", cut.Find(".ant-masonry-item").ClassList);
        Assert.Contains("background-color:#fafafa", cut.Find(".ant-masonry").GetAttribute("style"));
        Assert.Contains("opacity:.8", cut.Find(".ant-masonry-item").GetAttribute("style"));
    }

    [Fact]
    public void Tour_applies_semantic_classes_and_styles_to_visible_slots()
    {
        var steps = new[]
        {
            new TourStep
            {
                Title = builder => builder.AddContent(0, "First"),
                Description = builder => builder.AddContent(0, "Description"),
                Cover = builder => builder.AddContent(0, "Cover"),
            },
            new TourStep { Title = builder => builder.AddContent(0, "Second") },
        };
        var cut = RenderComponent<Tour>(parameters => parameters
            .Add(component => component.Open, true)
            .Add(component => component.Steps, steps)
            .Add(component => component.ClassNames, new Dictionary<string, string>
            {
                ["root"] = "custom-root",
                ["mask"] = "custom-mask",
                ["section"] = "custom-section",
                ["cover"] = "custom-cover",
                ["title"] = "custom-title",
                ["description"] = "custom-description",
                ["footer"] = "custom-footer",
                ["indicators"] = "custom-indicators",
                ["indicator"] = "custom-indicator",
                ["actions"] = "custom-actions",
            })
            .Add(component => component.Styles, new Dictionary<string, string>
            {
                ["root"] = "outline:1px solid red",
                ["mask"] = "opacity:.4",
                ["section"] = "padding:12px",
                ["cover"] = "color:red",
                ["title"] = "font-weight:bold",
                ["description"] = "line-height:2",
                ["footer"] = "margin-top:8px",
                ["indicators"] = "gap:8px",
                ["indicator"] = "border-radius:0",
                ["actions"] = "justify-content:flex-start",
            }));

        Assert.Contains("custom-root", cut.Find(".ant-tour").ClassList);
        Assert.Contains("custom-mask", cut.Find(".ant-tour-mask").ClassList);
        Assert.Contains("custom-section", cut.Find(".ant-tour-section").ClassList);
        Assert.Contains("custom-cover", cut.Find(".ant-tour-cover").ClassList);
        Assert.Contains("custom-title", cut.Find(".ant-tour-title").ClassList);
        Assert.Contains("custom-description", cut.Find(".ant-tour-description").ClassList);
        Assert.Contains("custom-footer", cut.Find(".ant-tour-footer").ClassList);
        Assert.Contains("custom-indicators", cut.Find(".ant-tour-indicators").ClassList);
        Assert.Contains("custom-indicator", cut.Find(".ant-tour-indicator").ClassList);
        Assert.Contains("custom-actions", cut.Find(".ant-tour-actions").ClassList);
        Assert.Contains("outline:1px solid red", cut.Find(".ant-tour").GetAttribute("style"));
        Assert.Contains("opacity:.4", cut.Find(".ant-tour-mask").GetAttribute("style"));
        Assert.Contains("justify-content:flex-start", cut.Find(".ant-tour-actions").GetAttribute("style"));
    }
}
