using AntDesign.Docs.Components;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class CollapseDemoParityTests : DocumentationTestBase
{
    public CollapseDemoParityTests() => JSInterop.Mode = JSRuntimeMode.Loose;

    [Fact]
    public void Semantic_preview_uses_React_single_expanded_panel_and_all_five_real_slots()
    {
        var cut = RenderComponent<CollapseSemanticPreview>();
        Assert.Single(cut.FindComponents<Panel>());
        Assert.Equal("This is panel header", cut.Find(".ant-collapse-title").TextContent.Trim());
        Assert.Equal("This is panel body", cut.Find(".ant-collapse-body p").TextContent);
        Assert.Equal("true", cut.Find(".ant-collapse-header").GetAttribute("aria-expanded"));
        Assert.Single(cut.FindAll(".ant-collapse-expand-icon"));
    }

    [Fact]
    public void Component_token_demo_uses_React_values_and_three_real_panels()
    {
        var cut = RenderComponent<AntDesign.Docs.Demos.Components.Collapse.demo.ComponentToken>();
        Assert.Equal(3, cut.FindComponents<Panel>().Count);
        var style = cut.Find(".ant-collapse").GetAttribute("style");
        Assert.Contains("--ant-collapse-header-padding:0px 10px 20px 30px", style);
        Assert.Contains("--ant-collapse-content-padding:0px 10px 20px 30px", style);
        Assert.Contains("--ant-collapse-header-bg:#eaeeff", style);
        Assert.Contains("--ant-collapse-content-bg:#e6f7ff", style);
        Assert.Empty(cut.FindAll(".ant-collapse-body"));
        cut.FindAll(".ant-collapse-header")[0].Click();
        Assert.Equal(string.Concat(System.Linq.Enumerable.Repeat("Ant Design! ", 26)), cut.Find(".ant-collapse-body").TextContent);
    }
}
