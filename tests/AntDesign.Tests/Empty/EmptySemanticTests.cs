using System.Collections.Generic;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Empty;

public sealed class EmptySemanticTests : AntDesignTestBase
{
    [Fact]
    public void Applies_semantic_classes_and_styles_to_all_documented_slots()
    {
        IReadOnlyDictionary<string, string> classNames = new Dictionary<string, string>
        {
            ["root"] = "custom-root",
            ["image"] = "custom-image",
            ["description"] = "custom-description",
            ["footer"] = "custom-footer"
        };
        IReadOnlyDictionary<string, string> styles = new Dictionary<string, string>
        {
            ["root"] = "background-color:#f5f5f5",
            ["image"] = "filter:grayscale(100%)",
            ["description"] = "font-weight:bold",
            ["footer"] = "margin-top:16px"
        };

        var cut = Context.RenderComponent<AntDesign.Empty>(parameters => parameters
            .Add(component => component.ClassNames, classNames)
            .Add(component => component.Styles, styles)
            .Add(component => component.ImageStyle, "height:60px")
            .Add(component => component.Description, "No data")
            .AddChildContent("Create now"));

        Assert.Contains("custom-root", cut.Find(".ant-empty").ClassList);
        Assert.Contains("custom-image", cut.Find(".ant-empty-image").ClassList);
        Assert.Contains("custom-description", cut.Find(".ant-empty-description").ClassList);
        Assert.Contains("custom-footer", cut.Find(".ant-empty-footer").ClassList);
        Assert.Contains("background-color:#f5f5f5", cut.Find(".ant-empty").GetAttribute("style"));
        Assert.Contains("height:60px", cut.Find(".ant-empty-image").GetAttribute("style"));
        Assert.Contains("filter:grayscale(100%)", cut.Find(".ant-empty-image").GetAttribute("style"));
        Assert.Contains("font-weight:bold", cut.Find(".ant-empty-description").GetAttribute("style"));
        Assert.Contains("margin-top:16px", cut.Find(".ant-empty-footer").GetAttribute("style"));
    }
}
