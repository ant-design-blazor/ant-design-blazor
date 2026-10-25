using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Icon;

public sealed class IconParameterUpdateTests : AntDesignTestBase
{
    [Fact]
    public void Accessibility_attributes_follow_parameter_changes_and_remove_old_values()
    {
        var cut = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "home")
            .Add(x => x.AriaLabel, "Before").Add(x => x.Alt, "Old alt"));
        cut.SetParametersAndRender(p => p.Add(x => x.AriaLabel, "After").Add(x => x.Role, ""));
        Assert.Equal("After", cut.Find("span").GetAttribute("aria-label"));
        Assert.Equal("true", cut.Find("span").GetAttribute("aria-hidden"));
        Assert.False(cut.Find("span").HasAttribute("role"));
        Assert.False(cut.Find("span").HasAttribute("alt"));
        cut.SetParametersAndRender(p => p.Add(x => x.Role, "img").Add(x => x.Alt, "New alt"));
        Assert.Equal("img", cut.Find("span").GetAttribute("role"));
        Assert.Equal("New alt", cut.Find("span").GetAttribute("alt"));
        Assert.False(cut.Find("span").HasAttribute("aria-hidden"));
    }

    [Fact]
    public void A_click_callback_added_after_initial_render_is_registered_with_the_React_tabindex_default()
    {
        var clicks = 0;
        var cut = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "home"));
        Assert.Null(cut.Find("span").GetAttribute("tabindex"));
        var callback = EventCallback.Factory.Create<MouseEventArgs>(this, () => clicks++);
        cut.SetParametersAndRender(p => p.Add(x => x.OnClick, callback));
        cut.Find("span").Click();
        Assert.Equal(1, clicks);
        Assert.Equal("-1", cut.Find("span").GetAttribute("tabindex"));
        cut.SetParametersAndRender(p => p.Add(x => x.TabIndex, "0"));
        Assert.Equal("0", cut.Find("span").GetAttribute("tabindex"));
    }

    [Fact]
    public void An_unset_name_is_safe_and_can_be_supplied_later()
    {
        var cut = RenderComponent<AntDesign.Icon>();
        Assert.Empty(cut.FindAll("svg"));
        cut.SetParametersAndRender(p => p.Add(x => x.Type, "home"));
        Assert.Single(cut.FindAll("svg"));
    }
}
