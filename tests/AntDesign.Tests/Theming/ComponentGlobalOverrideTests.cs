using AntDesign.Theming;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class ComponentGlobalOverrideTests : AntDesignTestBase
{
    public ComponentGlobalOverrideTests() => JSInterop.Mode = JSRuntimeMode.Loose;

    [Fact]
    public void Global_component_overrides_are_local_to_the_target_root()
    {
        var theme = PreviewThemeConfigFactory.Create("mui");
        var cut = RenderComponent<ConfigProvider>(parameters => parameters
            .Add(x => x.ThemeConfig, theme)
            .AddChildContent((RenderFragment)(builder => {
                builder.OpenComponent<AntDesign.Button>(0); builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "MUI"))); builder.CloseComponent();
                builder.OpenComponent<AntDesign.Input<string>>(2); builder.CloseComponent();
            })));
        var buttonStyle = cut.Find("button").GetAttribute("style");
        Assert.Contains("--ant-control-height:36px", buttonStyle);
        Assert.Contains("--ant-border-radius:4px", buttonStyle);
        Assert.Contains("--ant-button-padding-inline:16px", buttonStyle);
        Assert.Contains("--ant-button-font-weight:500;", buttonStyle);
        Assert.Contains("--ant-button-content-line-height:1.75;", buttonStyle);
        Assert.DoesNotContain("--ant-button-padding-inline", cut.Find("input").GetAttribute("style"));
    }

    [Fact]
    public void Inline_dialog_reuses_the_real_panel_and_does_not_mount_a_portal_or_focus_trap()
    {
        var closed = false;
        var cut = RenderComponent<Dialog>(parameters => parameters
            .Add(x => x.PurePanel, true)
            .Add(x => x.Visible, true)
            .Add(x => x.Config, new DialogOptions { Title = "Preview", OnCancel = args => { closed = true; return System.Threading.Tasks.Task.CompletedTask; } })
            .AddChildContent("Real modal content"));
        Assert.NotNull(cut.Find(".ant-modal-pure-panel[role=dialog]"));
        Assert.Equal("Real modal content", cut.Find(".ant-modal-body").TextContent.Trim());
        Assert.Empty(cut.FindAll(".ant-modal-wrap, .ant-modal-mask, [aria-hidden=true]"));
        cut.Find(".ant-modal-close").Click();
        Assert.True(closed);
        Assert.DoesNotContain(JSInterop.Invocations, invocation => invocation.Identifier == JSInteropConstants.AddElementTo);
    }
}
