using System.Linq;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class SelectDemoParityTests : DocumentationTestBase
{
    [Fact]
    public void Semantic_style_demo_uses_real_prefix_placeholder_and_popup_slots()
    {
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect());
        JSInterop.SetupVoid("AntDesign.interop.styleHelper.addClsToFirstChild", _ => true).SetVoidResult();
        var cut = RenderComponent<AntDesign.Docs.Demos.Components.Select.demo.StyleClass>();
        var selects = cut.FindAll(".ant-flex > .ant-select");
        Assert.Equal(2, selects.Count);
        Assert.True(selects[0].ClassList.Contains("ant-select-outlined"));
        Assert.True(selects[1].ClassList.Contains("ant-select-filled"));
        foreach (var (select, color, placeholder) in new[]
        {
            (selects[0], "#1890ff", "Object"), (selects[1], "#722ed1", "Function")
        })
        {
            Assert.True(select.ClassList.Contains("adb-select-semantic-demo-root"));
            Assert.Equal("ant-select-prefix", select.Children[0].ClassName);
            Assert.Equal("DIV", select.Children[0].TagName);
            Assert.Equal("color:" + color, select.Children[0].GetAttribute("style"));
            Assert.NotNull(select.Children[0].QuerySelector(".anticon-meh"));
            Assert.Equal("ant-select-content", select.Children[1].ClassName);
            Assert.Equal(placeholder, select.QuerySelector(".ant-select-placeholder").TextContent);
            Assert.Equal("DIV", select.QuerySelector(".ant-select-placeholder").TagName);
            Assert.Null(select.QuerySelector("input").GetAttribute("style"));
            Assert.Equal("color:" + color, select.QuerySelector(".ant-select-suffix").GetAttribute("style"));
        }
        Assert.Equal("border:1px solid #722ed1", cut.FindComponents<AntDesign.Select<string, string>>()[1].Instance.Styles["popup.root"]);
        Assert.Equal(new[] { "GuangZhou", "ShenZhen", "GuangZhou", "ShenZhen" },
            cut.FindComponents<AntDesign.SelectOption<string, string>>().Select(option => option.Instance.Value));
        Assert.Contains("border-radius:8px;width:300px", cut.Find("style").TextContent);
    }

    [Fact]
    public void Basic_selects_have_the_same_Space_item_boundaries_as_React()
    {
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect());
        JSInterop.SetupVoid("AntDesign.interop.styleHelper.addClsToFirstChild", _ => true).SetVoidResult();
        var cut = RenderComponent<AntDesign.Docs.Demos.Components.Select.demo.Basic>();
        var items = cut.FindAll(".ant-space > .ant-space-item");
        Assert.Equal(4, items.Count);
        Assert.All(items, item => Assert.Single(item.QuerySelectorAll(":scope > .ant-select")));
        cut.WaitForAssertion(() => Assert.Single(items[3].QuerySelectorAll("button.ant-select-clear")));
        Assert.Equal("Yiminghe", cut.FindComponents<AntDesign.SelectOption<string, string>>()
            .Single(option => option.Instance.Label == "yiminghe").Instance.Value);
    }
}
