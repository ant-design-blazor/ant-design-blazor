using Bunit;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class TagPreviewColorTests : AntDesignTestBase
{
    [Theory]
    [InlineData("#55acee", "#e8f4fd")]
    [InlineData("#cd201f", "#fce9e9")]
    [InlineData("#3b5999", "#edf0f8")]
    public void Custom_tag_color_matches_React_FastColor_95_percent_HSL_lightness(string color, string background)
    {
        var cut = RenderComponent<AntDesign.Tag>(parameters => parameters.Add(x => x.Color, color).AddChildContent("Tag"));
        var style = cut.Find(".ant-tag-filled").GetAttribute("style");
        Assert.Contains($"background-color: {background};", style);
        Assert.Contains($"color:{color};", style);
    }

    [Theory]
    [InlineData(TagVariant.Filled, "filled")]
    [InlineData(TagVariant.Solid, "solid")]
    [InlineData(TagVariant.Outlined, "outlined")]
    public void Status_tag_emits_the_variant_required_by_the_v6_stylesheet(TagVariant variant, string suffix)
    {
        var cut = RenderComponent<AntDesign.Tag>(parameters => parameters.Add(x => x.Color, TagColor.Warning).Add(x => x.Variant, variant));
        Assert.NotNull(cut.Find($".ant-tag-warning.ant-tag-{suffix}"));
    }
}
