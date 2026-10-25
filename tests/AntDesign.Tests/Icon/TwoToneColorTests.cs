using System.Linq;
using System.Threading.Tasks;
using AntDesign.Icons;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Icon;

public sealed class TwoToneColorTests : AntDesignTestBase
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Missing_primary_keeps_the_same_default_in_both_icon_APIs_and_the_compatibility_service(string primary)
    {
        var legacy = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "check-circle").Add(x => x.TwoToneColor, primary));
        var generated = RenderComponent<CheckCircleTwoTone>(p => p.Add(x => x.TwoToneColor, new[] { primary }));
        Assert.Contains("#e6f4ff", legacy.Markup);
        Assert.Contains("#1677ff", legacy.Markup);
        Assert.Contains("#e6f4ff", generated.Markup);
        Assert.Equal("#e6f4ff", await Services.GetRequiredService<IconService>().GetSecondaryColor(primary));
        Assert.Empty(JSInterop.Invocations);
    }

#if NET8_0_OR_GREATER
    [Fact]
    public async Task Server_prerender_has_both_colors_before_any_browser_lifecycle_runs()
    {
        await using var renderer = new Microsoft.AspNetCore.Components.Web.HtmlRenderer(
            Services, Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>());
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var legacy = await renderer.RenderComponentAsync<AntDesign.Icon>(
                Microsoft.AspNetCore.Components.ParameterView.FromDictionary(new System.Collections.Generic.Dictionary<string, object>
                {
                    ["Type"] = "check-circle", ["TwoToneColor"] = "#52c41a"
                }));
            var generated = await renderer.RenderComponentAsync<CheckCircleTwoTone>(
                Microsoft.AspNetCore.Components.ParameterView.FromDictionary(new System.Collections.Generic.Dictionary<string, object>
                {
                    ["TwoToneColor"] = new[] { "#52c41a" }
                }));
            foreach (var html in new[] { legacy.ToHtmlString(), generated.ToHtmlString() })
            {
                Assert.Contains("fill=\"#52c41a\"", html);
                Assert.Contains("fill=\"#f6ffed\"", html);
            }
        });
        Assert.Empty(JSInterop.Invocations);
    }
#endif

    [Theory]
    [InlineData("#1677ff", "#e6f4ff")]
    [InlineData("#eb2f96", "#fff0f6")]
    [InlineData("#52c41a", "#f6ffed")]
    [InlineData("#f759ab", "#fff0f5")]
    [InlineData("#2f54eb", "#f0f5ff")]
    [InlineData("#722ed1", "#f9f0ff")]
    [InlineData("#f5222d", "#fff1f0")]
    [InlineData("#000", "#404040")]
    [InlineData("#fff", "#ffffff")]
    [InlineData("red", "#ffeae6")]
    [InlineData("rgb(22,119,255)", "#e6f4ff")]
    [InlineData("hsl(215,100%,54%)", "#e6f4ff")]
    [InlineData("#808080", "#bfbfbf")]
    [InlineData("#00ffff", "#e6fffb")]
    public async Task Legacy_and_generated_icons_resolve_the_same_secondary_on_first_render_without_JS(string primary, string secondary)
    {
        var legacy = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "check-circle").Add(x => x.TwoToneColor, primary));
        var generated = RenderComponent<CheckCircleTwoTone>(p => p.Add(x => x.TwoToneColor, new[] { primary }));
        Assert.Contains(secondary, legacy.FindAll("path").Select(x => x.GetAttribute("fill")));
        Assert.Contains(primary, legacy.FindAll("path").Select(x => x.GetAttribute("fill")));
        Assert.Contains(secondary, generated.FindAll("path").Select(x => x.GetAttribute("fill")));
        var calculation = Services.GetRequiredService<IconService>().GetSecondaryColor(primary);
        Assert.True(calculation.IsCompletedSuccessfully);
        Assert.Equal(secondary, await calculation);
        Assert.Empty(JSInterop.Invocations);
    }

    [Fact]
    public void Primary_changes_and_theme_switches_do_not_leave_stale_secondary_colors()
    {
        var cut = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "check-circle").Add(x => x.TwoToneColor, "#52c41a"));
        cut.SetParametersAndRender(p => p.Add(x => x.TwoToneColor, "#eb2f96"));
        Assert.Contains("#fff0f6", cut.Markup);
        Assert.DoesNotContain("#f6ffed", cut.Markup);
        cut.SetParametersAndRender(p => p.Add(x => x.Theme, IconThemeType.Outline));
        cut.SetParametersAndRender(p => p.Add(x => x.Theme, IconThemeType.TwoTone));
        Assert.Contains("#fff0f6", cut.Markup);
        Assert.Empty(JSInterop.Invocations);
    }

    [Fact]
    public void Default_and_custom_color_instances_remain_independent()
    {
        var colored = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "check-circle").Add(x => x.TwoToneColor, "#52c41a"));
        var normal = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "check-circle").Add(x => x.Theme, IconThemeType.TwoTone));
        Assert.Contains("#1677ff", normal.Markup);
        Assert.Contains("#e6f4ff", normal.Markup);
        Assert.Contains("#52c41a", colored.Markup);
        Assert.Contains("#f6ffed", colored.Markup);
        Assert.Empty(JSInterop.Invocations);
    }

    [Fact]
    public void Custom_svg_groups_and_none_fills_are_preserved_and_colors_are_encoded()
    {
        const string svg = "<svg><g><path fill=\"#333\"/><path fill=\"#E6E6E6\"/></g><path fill=\"none\"/></svg>";
        var cut = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, svg).Add(x => x.TwoToneColor, "red\" onload=\"bad"));
        Assert.Equal(2, cut.FindAll("g > path").Count);
        Assert.Equal("none", cut.Find("svg > path").GetAttribute("fill"));
        Assert.Empty(cut.FindAll("[onload]"));
        Assert.Empty(JSInterop.Invocations);
    }
}
