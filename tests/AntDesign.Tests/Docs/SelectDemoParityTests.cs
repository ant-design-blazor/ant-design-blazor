using System.Linq;
using System.Globalization;
using Bunit;
using AntDesign.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
            (selects[0], "rgb(24, 144, 255)", "Object"), (selects[1], "rgb(114, 46, 209)", "Function")
        })
        {
            Assert.True(select.ClassList.Contains("adb-select-semantic-demo-root"));
            Assert.Equal("ant-select-prefix", select.Children[0].ClassName);
            Assert.Equal("DIV", select.Children[0].TagName);
            Assert.Equal("color: " + color + ";", select.Children[0].GetAttribute("style"));
            Assert.NotNull(select.Children[0].QuerySelector(".anticon-meh"));
            Assert.Equal("ant-select-content", select.Children[1].ClassName);
            Assert.Equal(placeholder, select.QuerySelector(".ant-select-placeholder").TextContent);
            Assert.Equal("DIV", select.QuerySelector(".ant-select-placeholder").TagName);
            Assert.Null(select.QuerySelector("input").GetAttribute("style"));
            Assert.Equal("color: " + color + ";", select.QuerySelector(".ant-select-suffix").GetAttribute("style"));
        }
        Assert.Equal("border:1px solid #722ed1", cut.FindComponents<AntDesign.Select<string, string>>()[1].Instance.Styles["popup.root"]);
        Assert.Equal(4, cut.FindComponents<AntDesign.SelectOption<string, string>>().Count);
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
    }

    [Fact]
    public void Semantic_preview_matches_react_popup_root_layering()
    {
        var locale = new Mock<ILocalizationService>();
        locale.SetupGet(service => service.CurrentCulture).Returns(CultureInfo.GetCultureInfo("en-US"));
        Services.AddSingleton(locale.Object);
        JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
            .SetResult(new AntDesign.JsInterop.DomRect { Width = 300 });
        JSInterop.Setup<AntDesign.JsInterop.HtmlElement>(JSInteropConstants.GetDomInfo, _ => true)
            .SetResult(new AntDesign.JsInterop.HtmlElement());
        JSInterop.Setup<System.Collections.Generic.Dictionary<string, AntDesign.JsInterop.HtmlElement>>(
                JSInteropConstants.GetElementsDomInfo, _ => true)
            .SetResult(new System.Collections.Generic.Dictionary<string, AntDesign.JsInterop.HtmlElement>());
        JSInterop.Setup<AntDesign.Core.JsInterop.Modules.Components.OverlayPosition>(
                JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer, _ => true)
            .SetResult(new AntDesign.Core.JsInterop.Modules.Components.OverlayPosition
            {
                Placement = AntDesign.Placement.BottomLeft,
                ZIndex = 1
            });
        var cut = RenderComponent<AntDesign.Docs.Demos.Components.Select.demo._semantic>();

        Assert.Equal("z-index: 1", cut.Find(".ant-select-dropdown").GetAttribute("style")
            ?.Split(';').Select(declaration => declaration.Trim())
            .Single(declaration => declaration.StartsWith("z-index:", System.StringComparison.Ordinal)));
    }
}
