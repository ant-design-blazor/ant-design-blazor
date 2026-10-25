using System;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Input;

public sealed class InputOTPTests : AntDesignTestBase
{
    public InputOTPTests() => JSInterop.Mode = JSRuntimeMode.Loose;

    [Fact]
    public void Renders_React_OTP_wrappers_and_real_sized_variant_inputs()
    {
        var cut = RenderComponent<InputOTP>(parameters => parameters
            .Add(x => x.DefaultValue, "4320")
            .Add(x => x.Size, InputSize.Large)
            .Add(x => x.Variant, InputVariant.Filled));
        Assert.NotNull(cut.Find(".ant-otp.ant-otp-lg[role=group]"));
        Assert.Equal(6, cut.FindAll(".ant-otp-input-wrapper[role=presentation]").Count);
        Assert.Equal(6, cut.FindComponents<AntDesign.Input<string>>().Count);
        Assert.Equal(new[] { "4", "3", "2", "0", "", "" }, cut.FindAll("input").Select(input => input.GetAttribute("value")));
        Assert.All(cut.FindAll("input"), input => {
            Assert.Equal("1", input.GetAttribute("size"));
            Assert.Contains("ant-input-filled", input.ClassList);
            Assert.Contains("ant-input-lg", input.ClassList);
        });
    }

    [Fact]
    public void Pasted_text_is_formatted_distributed_and_completes_only_once()
    {
        var completed = 0;
        string result = null;
        string[] cells = null;
        var cut = RenderComponent<InputOTP>(parameters => parameters
            .Add(x => x.Length, 4)
            .Add(x => x.Formatter, value => value.ToUpperInvariant())
            .Add(x => x.OnInput, value => cells = value)
            .Add(x => x.OnChange, value => { result = value; completed++; }));
        cut.Find("input").Input("ab");
        Assert.Equal(new[] { "A", "B" }, cells);
        Assert.Equal(0, completed);
        cut.FindAll("input")[2].Input("cd5");
        Assert.Equal("ABCD", result);
        Assert.Equal(1, completed);
        cut.FindAll("input")[3].Input("D");
        Assert.Equal(1, completed);
        Assert.Equal(new[] { "A", "B", "C", "D" }, cut.FindAll("input").Select(input => input.GetAttribute("value")));
    }

    [Fact]
    public void Controlled_value_mask_separator_disabled_and_length_updates_are_supported()
    {
        var cut = RenderComponent<InputOTP>(parameters => parameters
            .Add(x => x.Value, "1234")
            .Add(x => x.Length, 4)
            .Add(x => x.Mask, "*")
            .Add(x => x.SeparatorText, "-")
            .Add(x => x.Disabled, true));
        Assert.Equal(3, cut.FindAll(".ant-otp-separator").Count);
        Assert.Equal(4, cut.FindAll(".ant-otp-mask-icon[aria-hidden=true]").Count);
        Assert.All(cut.FindAll("input"), input => { Assert.True(input.HasAttribute("disabled")); Assert.Equal("password", input.GetAttribute("type")); });
        cut.SetParametersAndRender(parameters => parameters.Add(x => x.Value, "9").Add(x => x.Length, 2).Add(x => x.Disabled, false));
        Assert.Equal(new[] { "9", "" }, cut.FindAll("input").Select(input => input.GetAttribute("value")));
        Assert.Single(cut.FindAll(".ant-otp-mask-icon"));
    }
}
