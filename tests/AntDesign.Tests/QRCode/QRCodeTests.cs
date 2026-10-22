using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using Xunit;

namespace AntDesign.Tests.QRCode;

public sealed class QRCodeTests : AntDesignTestBase
{
    public QRCodeTests() => JSInterop.Mode = JSRuntimeMode.Loose;

    [Fact]
    public void Default_canvas_is_drawn_and_can_switch_to_svg()
    {
        var cut = RenderComponent<AntDesign.QRCode>(parameters => parameters.Add(x => x.Value, "https://ant.design/"));
        Assert.Single(cut.FindAll(".ant-qrcode > canvas"));
        Assert.Contains(JSInterop.Invocations, call => call.Identifier == JSInteropConstants.QRCodeHelper.Draw);

        cut.SetParametersAndRender(parameters => parameters.Add(x => x.Type, "svg"));
        Assert.Empty(cut.FindAll("canvas"));
        Assert.Equal(2, cut.FindAll(".ant-qrcode > svg > path").Count);
        Assert.NotEmpty(cut.Find("svg > path:last-of-type").GetAttribute("d")!);
    }

    [Fact]
    public void Svg_supports_rectangular_icons_css_colors_and_explicit_module_margin()
    {
        var cut = RenderComponent<AntDesign.QRCode>(parameters => parameters
            .Add(x => x.Value, "https://ant.design/")
            .Add(x => x.Type, "svg")
            .Add(x => x.Icon, "logo.svg")
            .Add(x => x.IconSize, new QRCodeIconSize { Width = 32, Height = 16 })
            .Add(x => x.Color, "rgb(20, 40, 60)")
            .Add(x => x.Bordered, false));
        var oldViewBox = cut.Find("svg").GetAttribute("viewBox")!.Split(' ');
        var image = cut.Find("svg > image");
        Assert.Equal("logo.svg", image.GetAttribute("href"));
        Assert.Equal(2, double.Parse(image.GetAttribute("width")!, System.Globalization.CultureInfo.InvariantCulture)
            / double.Parse(image.GetAttribute("height")!, System.Globalization.CultureInfo.InvariantCulture));
        Assert.Equal("rgb(20, 40, 60)", cut.Find("svg > path:last-of-type").GetAttribute("fill"));
        Assert.Contains("ant-qrcode-borderless", cut.Find(".ant-qrcode").ClassList);

        cut.SetParametersAndRender(parameters => parameters.Add(x => x.MarginSize, 4));
        Assert.Equal(int.Parse(oldViewBox[2]) + 8, int.Parse(cut.Find("svg").GetAttribute("viewBox")!.Split(' ')[2]));
    }

    [Fact]
    public void Status_cover_preserves_renderer_and_refresh_callback_and_semantic_styles()
    {
        var refreshed = false;
        var cut = RenderComponent<AntDesign.QRCode>(parameters => parameters
            .Add(x => x.Value, "https://ant.design/")
            .Add(x => x.Status, "expired")
            .Add(x => x.OnRefresh, () => refreshed = true)
            .Add(x => x.Locale, new QRCodeLocale { Expired = "已过期", Refresh = "刷新" })
            .Add(x => x.ClassNames, new Dictionary<string, string> { ["root"] = "qr-root", ["cover"] = "qr-cover" })
            .Add(x => x.Styles, new Dictionary<string, string> { ["cover"] = "color:red" }));

        Assert.NotNull(cut.Find(".qr-root > canvas"));
        Assert.Equal("color:red", cut.Find(".qr-cover").GetAttribute("style"));
        Assert.Equal("已过期", cut.Find(".ant-qrcode-expired").TextContent);
        cut.Find(".ant-qrcode-cover button").Click();
        Assert.True(refreshed);
    }

    [Fact]
    public async Task Download_uses_the_current_renderer_and_empty_value_renders_nothing()
    {
        var cut = RenderComponent<AntDesign.QRCode>(parameters => parameters.Add(x => x.Value, "https://ant.design/"));
        await cut.Instance.DownloadAsync("custom.png");
        Assert.Contains(JSInterop.Invocations, call => call.Identifier == JSInteropConstants.QRCodeHelper.Download && Equals(call.Arguments[1], "custom.png"));

        cut.SetParametersAndRender(parameters => parameters.Add(x => x.Value, string.Empty));
        Assert.Empty(cut.FindAll(".ant-qrcode"));
    }
}
