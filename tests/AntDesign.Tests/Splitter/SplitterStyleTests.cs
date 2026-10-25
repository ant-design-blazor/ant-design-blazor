using Xunit;

namespace AntDesign.Tests.Splitter;

public sealed class SplitterStyleTests
{
    [Theory]
    [InlineData("40%", "flex-basis:40%;flex-grow:0;")]
    [InlineData("240px", "flex-basis:240px;flex-grow:0;")]
    [InlineData(null, "flex-basis:auto;flex-grow:1;")]
    public void Panel_styles_use_react_flex_basis_without_legacy_dimension_constraints(string? size, string expected)
    {
        var splitter = new AntDesign.Splitter();
        var panel = new AntDesign.SplitterPanel
        {
            DefaultSize = size,
            Min = "20%",
            Max = "70%",
        };

        Assert.Equal(expected, splitter.GetPaneStyle(panel));
    }
}
