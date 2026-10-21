using System.Collections.Generic;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Transfer;

public sealed class TransferV6StructureTests : AntDesignTestBase
{
    public TransferV6StructureTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Applies_shared_and_direction_specific_semantic_classes_and_styles()
    {
        var classNames = new Dictionary<string, string>
        {
            ["root"] = "custom-root",
            ["section"] = "custom-section",
            ["source.section"] = "custom-source-section",
            ["target.section"] = "custom-target-section",
            ["header"] = "custom-header",
            ["target.header"] = "custom-target-header",
            ["source.title"] = "custom-source-title",
            ["source.body"] = "custom-source-body",
            ["target.list"] = "custom-target-list",
            ["source.item"] = "custom-source-item",
            ["target.itemIcon"] = "custom-target-item-icon",
            ["source.itemContent"] = "custom-source-item-content",
            ["source.footer"] = "custom-source-footer",
            ["target.footer"] = "custom-target-footer",
            ["actions"] = "custom-actions"
        };
        var styles = new Dictionary<string, string>
        {
            ["root"] = "padding: 4px;",
            ["section"] = "height: 200px;",
            ["source.section"] = "background: rgb(255, 247, 230);",
            ["header"] = "color: red;",
            ["target.header"] = "color: blue;",
            ["source.item"] = "font-weight: bold;",
            ["target.itemIcon"] = "outline: 1px solid rgb(22, 119, 255);",
            ["actions"] = "gap: 8px;"
        };
        var data = new[]
        {
            new TransferItem { Key = "source", Title = "Source item" },
            new TransferItem { Key = "target", Title = "Target item" }
        };

        var transfer = RenderComponent<AntDesign.Transfer>(parameters => parameters
            .Add(component => component.DataSource, data)
            .Add(component => component.TargetKeys, new[] { "target" })
            .Add(component => component.Titles, new[] { "Source", "Target" })
            .Add(component => component.Footer, "Footer")
            .Add(component => component.ClassNames, classNames)
            .Add(component => component.Styles, styles)
            .Add(component => component.ListStyle, _ => "min-width: 180px;"));

        var root = transfer.Find(".ant-transfer");
        Assert.Contains("custom-root", root.ClassList);
        Assert.Contains("padding: 4px", root.GetAttribute("style"));

        var source = transfer.Find(".custom-source-section");
        Assert.Contains("custom-section", source.ClassList);
        Assert.Contains("height: 200px", source.GetAttribute("style"));
        Assert.Contains("background: rgb(255, 247, 230)", source.GetAttribute("style"));
        Assert.Contains("custom-header", source.QuerySelector(".ant-transfer-list-header")!.ClassList);
        Assert.Contains("color: red", source.QuerySelector(".ant-transfer-list-header")!.GetAttribute("style"));
        Assert.Contains("custom-source-title", source.QuerySelector(".ant-transfer-list-header-title")!.ClassList);
        Assert.Contains("custom-source-body", source.QuerySelector(".ant-transfer-list-body")!.ClassList);
        Assert.Contains("custom-source-item", source.QuerySelector(".ant-transfer-list-content-item")!.ClassList);
        Assert.Contains("custom-source-item-content", source.QuerySelector(".ant-transfer-list-content-item-text")!.ClassList);
        Assert.Contains("custom-source-footer", source.QuerySelector(".ant-transfer-list-footer")!.ClassList);

        var target = transfer.Find(".custom-target-section");
        Assert.Contains("custom-section", target.ClassList);
        Assert.Contains("custom-target-header", target.QuerySelector(".ant-transfer-list-header")!.ClassList);
        Assert.Contains("color: blue", target.QuerySelector(".ant-transfer-list-header")!.GetAttribute("style"));
        Assert.Contains("custom-target-list", target.QuerySelector(".ant-transfer-list-content")!.ClassList);
        var targetItemIcon = target.QuerySelector(".ant-transfer-list-content-item .ant-transfer-list-checkbox")!;
        Assert.Contains("custom-target-item-icon", targetItemIcon.ClassList);
        Assert.Contains("outline: 1px solid rgb(22, 119, 255)", targetItemIcon.GetAttribute("style"));
        Assert.Contains("custom-target-footer", target.QuerySelector(".ant-transfer-list-footer")!.ClassList);
        Assert.Contains("min-width: 180px", target.GetAttribute("style"));

        Assert.Contains("custom-actions", transfer.Find(".ant-transfer-actions").ClassList);
        Assert.Contains("gap: 8px", transfer.Find(".ant-transfer-actions").GetAttribute("style"));
    }
}
