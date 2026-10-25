using Bunit;
using Xunit;

namespace AntDesign.Tests;

public sealed class ButtonAccessibilityTests : AntDesignTestBase
{
    [Fact]
    public void Text_toggle_uses_v6_button_styles_and_exposes_its_pressed_state()
    {
        var cut = RenderComponent<AntDesign.Button>(p => p
            .Add(x => x.Type, ButtonType.Text).Add(x => x.Size, ButtonSize.Small)
            .Add(x => x.Icon, "bug").Add(x => x.AriaPressed, false).AddChildContent("Debug"));
        var button = cut.Find("button.ant-btn-variant-text.ant-btn-color-default.ant-btn-sm");
        Assert.Equal("false", button.GetAttribute("aria-pressed"));
        Assert.NotNull(button.QuerySelector(".ant-btn-icon .anticon-bug"));
        cut.SetParametersAndRender(p => p.Add(x => x.AriaPressed, true));
        Assert.Equal("true", cut.Find("button").GetAttribute("aria-pressed"));
        cut.SetParametersAndRender(p => p.Add(x => x.AriaPressed, (bool?)null));
        Assert.Null(cut.Find("button").GetAttribute("aria-pressed"));
    }
}
