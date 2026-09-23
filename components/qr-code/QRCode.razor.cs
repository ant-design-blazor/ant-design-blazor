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
    [Parameter] public bool Bordered { get; set; } = true;
    [Parameter] public EventCallback OnRefresh { get; set; }

    private string SvgMarkup
    {
        get
        {
            using var data = QRCodeGenerator.GenerateQrCode(Value ?? string.Empty, ParseErrorLevel(ErrorLevel));
            using var renderer = new SvgQRCode(data);
            return renderer.GetGraphic();
        }
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
