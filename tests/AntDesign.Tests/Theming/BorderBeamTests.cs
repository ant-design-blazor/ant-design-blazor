using Bunit;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class BorderBeamTests : AntDesignTestBase
{
    public BorderBeamTests() => JSInterop.Mode = JSRuntimeMode.Loose;

    [Fact]
    public void Decoration_is_inside_the_card_without_an_extra_layout_wrapper()
    {
        var cut = RenderComponent<AntDesign.Card>(parameters => parameters
            .AddChildContent("Card body")
            .Add(x => x.Decoration, builder => {
                builder.OpenComponent<BorderBeam>(0);
                builder.AddAttribute(1, "LineWidth", "2px");
                builder.CloseComponent();
            }));
        Assert.Equal("Card body", cut.Find(".ant-card-body").TextContent.Trim());
        Assert.Contains("--ant-border-beam-line-width:2px", cut.Find(".ant-card > .ant-border-beam[aria-hidden=true]").GetAttribute("style"));
        Assert.Contains(JSInterop.Invocations, call => call.Identifier == "AntDesign.interop.borderBeamHelper.update");
    }

    [Fact]
    public void Multiple_beams_use_evenly_distributed_negative_delays()
    {
        var cut = RenderComponent<BorderBeam>(parameters => parameters
            .Add(x => x.Count, 3).Add(x => x.Duration, 9)
            .Add(x => x.Size, "80px").Add(x => x.Outset, "1px"));
        var beams = cut.FindAll(".ant-border-beam");
        Assert.Equal(3, beams.Count);
        Assert.DoesNotContain("--ant-border-beam-delay", beams[0].GetAttribute("style"));
        Assert.Contains("--ant-border-beam-delay:-3s", beams[1].GetAttribute("style"));
        Assert.Contains("--ant-border-beam-delay:-6s", beams[2].GetAttribute("style"));
        Assert.Contains("--ant-border-beam-inset-offset:calc(-1 * 1px)", beams[0].GetAttribute("style"));
    }
}
