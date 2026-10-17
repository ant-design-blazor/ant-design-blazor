using Bunit;
using AntDesign.JsInterop;
using Xunit;

namespace AntDesign.Tests.Grid;

public sealed class GridTests : AntDesignTestBase
{
    [Fact]
    public void Default_row_matches_react_default_dom()
    {
        var cut = Context.RenderComponent<AntDesign.Row>(parameters =>
            parameters.AddChildContent<AntDesign.Col>(column => column
                .Add(component => component.Span, 24)
                .AddChildContent("col")));

        var row = cut.Find(".ant-row");
        Assert.Equal("ant-row", row.GetAttribute("class"));
        Assert.False(row.HasAttribute("style"));
        Assert.Equal("ant-col ant-col-24", cut.Find(".ant-col").GetAttribute("class"));
        Assert.False(cut.Find(".ant-col").HasAttribute("style"));
        Assert.Equal("col", cut.Find(".ant-col").TextContent);
    }

    [Fact]
    public void Explicit_alignment_and_gutter_keep_their_classes_and_styles()
    {
        var cut = Context.RenderComponent<AntDesign.Row>(parameters => parameters
            .Add(component => component.Align, RowAlign.Middle)
            .Add(component => component.Justify, RowJustify.Center)
            .Add(component => component.Gutter, 16)
            .AddChildContent<AntDesign.Col>(column => column
                .Add(component => component.Class, "gutter-row")
                .Add(component => component.Span, 12)
                .AddChildContent("column")));

        var row = cut.Find(".ant-row");
        Assert.Equal("ant-row ant-row-center ant-row-middle", row.GetAttribute("class"));
        Assert.Equal("margin-inline: -8px;", row.GetAttribute("style"));
        Assert.Equal("ant-col ant-col-12 gutter-row", cut.Find(".ant-col").GetAttribute("class"));
        Assert.Equal("padding-inline: 8px;", cut.Find(".ant-col").GetAttribute("style"));
    }

    [Fact]
    public void Responsive_gutter_uses_the_requested_prerender_breakpoint()
    {
        var gutter = new System.Collections.Generic.Dictionary<string, int>
        {
            ["xs"] = 8,
            ["lg"] = 32
        };
        JSInterop.Setup<Window>(JSInteropConstants.GetWindow, _ => true)
            .SetResult(new Window { InnerWidth = 1280m });
        var cut = Context.RenderComponent<AntDesign.Row>(parameters => parameters
            .Add(component => component.Gutter, gutter)
            .Add(component => component.DefaultBreakpoint, BreakpointType.Xl)
            .AddChildContent<AntDesign.Col>(column => column.AddChildContent("column")));

        Assert.Equal("margin-inline: -16px;", cut.Find(".ant-row").GetAttribute("style"));
        Assert.Equal("padding-inline: 16px;", cut.Find(".ant-col").GetAttribute("style"));
    }

    [Fact]
    public void String_gutter_uses_css_calc_and_logical_padding()
    {
        var cut = Context.RenderComponent<AntDesign.Row>(parameters => parameters
            .Add(component => component.Gutter, "2rem")
            .AddChildContent<AntDesign.Col>(column => column.AddChildContent("column")));

        Assert.Equal("margin-inline: calc(-1rem);", cut.Find(".ant-row").GetAttribute("style"));
        Assert.Equal("padding-inline: calc(1rem);", cut.Find(".ant-col").GetAttribute("style"));
    }
}
