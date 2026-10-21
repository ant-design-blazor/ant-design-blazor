using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using QRCoder;

namespace AntDesign;

/// <summary>Generates and renders a scannable QR code as inline SVG.</summary>
[Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "", Title = "QRCode", SubTitle = "二维码")]
public partial class QRCode : AntDomComponentBase
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public int Size { get; set; } = 160;
    [Parameter] public string ErrorLevel { get; set; } = "M";
    [Parameter] public string Status { get; set; } = "active";
    [Parameter] public string Color { get; set; } = "#000000";
    [Parameter] public string BackgroundColor { get; set; } = "#ffffff";
    [Parameter] public RenderFragment StatusRender { get; set; }
    [Parameter] public bool Bordered { get; set; } = true;
    [Parameter] public EventCallback OnRefresh { get; set; }

    private string SvgMarkup
    {
        get
        {
            using var data = QRCodeGenerator.GenerateQrCode(Value ?? string.Empty, ParseErrorLevel(ErrorLevel));
            using var renderer = new SvgQRCode(data);
            return renderer.GetGraphic(10, NormalizeColor(Color, "#000000"), NormalizeColor(BackgroundColor, "#ffffff"), true, SvgQRCode.SizingMode.ViewBoxAttribute, null);
        }
    }

    private static string NormalizeColor(string color, string fallback)
    {
        if (string.IsNullOrWhiteSpace(color)) return fallback;
        var value = color.Trim();
        if (value.Length == 4 && value[0] == '#') value = $"#{value[1]}{value[1]}{value[2]}{value[2]}{value[3]}{value[3]}";
        return value.Length == 7 && value[0] == '#' && value.Skip(1).All(Uri.IsHexDigit) ? value : fallback;
    }

    private static QRCodeGenerator.ECCLevel ParseErrorLevel(string level) => level switch
    {
        "L" => QRCodeGenerator.ECCLevel.L,
        "Q" => QRCodeGenerator.ECCLevel.Q,
        "H" => QRCodeGenerator.ECCLevel.H,
        _ => QRCodeGenerator.ECCLevel.M,
    };

    private Task Refresh() => OnRefresh.InvokeAsync(null);
}
