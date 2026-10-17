using Bunit;
using Xunit;

namespace AntDesign.Tests.Flex;

public sealed class FlexTests : AntDesignTestBase
{
    [Fact]
    public void Enum_gap_and_wrap_use_only_their_semantic_classes()
    {
        var cut = Context.RenderComponent<AntDesign.Flex>(parameters => parameters
            .Add(component => component.Gap, FlexGap.Small)
            .Add(component => component.Wrap, FlexWrap.Wrap)
            .AddChildContent("buttons"));

        var root = cut.Find("div");
        Assert.Equal("ant-flex ant-flex-wrap-wrap ant-flex-gap-small", root.GetAttribute("class"));
        Assert.False(root.HasAttribute("style"));
    }

    [Fact]
    public void Vertical_default_alignment_uses_stretch_without_a_normal_alignment_class()
    {
        var cut = Context.RenderComponent<AntDesign.Flex>(parameters => parameters
            .Add(component => component.Direction, FlexDirection.Vertical)
            .AddChildContent("content"));

        Assert.Equal("ant-flex ant-flex-align-stretch ant-flex-vertical", cut.Find("div").GetAttribute("class"));
    }

    [Theory]
    [InlineData(FlexGap.Middle)]
    [InlineData(FlexGap.Medium)]
    public void Middle_and_medium_gaps_share_the_v6_class(FlexGap gap)
    {
        var cut = Context.RenderComponent<AntDesign.Flex>(parameters => parameters
            .Add(component => component.Gap, gap)
            .AddChildContent("content"));

        Assert.Equal("ant-flex ant-flex-gap-medium", cut.Find("div").GetAttribute("class"));
    }
}
