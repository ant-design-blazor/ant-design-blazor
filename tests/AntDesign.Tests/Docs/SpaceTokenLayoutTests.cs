using AntDesign.Docs.Components;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class SpaceTokenLayoutTests : DocumentationTestBase
{
    [Theory]
    [InlineData(SpaceSize.Small, "small")]
    [InlineData(SpaceSize.Middle, "middle")]
    [InlineData(SpaceSize.Large, "large")]
    public void Named_gaps_use_token_CSS_classes_not_hard_coded_inline_pixels(SpaceSize size, string name)
    {
        var cut = RenderComponent<Space>(p => p.Add(x => x.Size, size)
            .AddChildContent<SpaceItem>(item => item.AddChildContent("A")));
        var root = cut.Find(".ant-space");
        Assert.Contains("ant-space-gap-row-" + name, root.ClassList);
        Assert.Contains("ant-space-gap-col-" + name, root.ClassList);
        Assert.Null(root.GetAttribute("style"));
        cut.SetParametersAndRender(p => p.Add(x => x.Size, "12px"));
        Assert.DoesNotContain(root.ClassList, cls => cls.StartsWith("ant-space-gap-"));
        Assert.Contains("column-gap:12px", root.GetAttribute("style"));
    }

    [Fact]
    public void Separator_does_not_halve_the_requested_gap_or_add_a_legacy_margin()
    {
        var cut = RenderComponent<Space>(p => p.Add(x => x.Size, "12px")
            .Add(x => x.Split, builder => builder.AddContent(0, "|"))
            .AddChildContent<SpaceItem>(item => item.AddChildContent("A"))
            .AddChildContent<SpaceItem>(item => item.AddChildContent("B")));
        Assert.Contains("column-gap:12px", cut.Find(".ant-space").GetAttribute("style"));
        Assert.Null(cut.Find(".ant-space-item-separator").GetAttribute("style"));
    }
}
