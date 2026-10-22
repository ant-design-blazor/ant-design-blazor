using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using QRCoder;

namespace AntDesign;

/// <summary>Generates a scannable QR code as canvas or inline SVG.</summary>
[Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "", Title = "QRCode", SubTitle = "二维码")]
public partial class QRCode : AntDomComponentBase
{
    [Parameter] public string Value { get; set; } = string.Empty;
    /// <summary>Rendering format: canvas or svg.</summary>
    [Parameter] public string Type { get; set; } = "canvas";
    [Parameter] public int Size { get; set; } = 160;
    [Parameter] public string ErrorLevel { get; set; } = "M";
    /// <summary>Use a higher error correction level when it fits in the same QR version.</summary>
    [Parameter] public bool BoostLevel { get; set; } = true;
    /// <summary>Quiet-zone width in modules.</summary>
    [Parameter] public int MarginSize { get; set; }
    /// <summary>URL of the image displayed in the center of the QR code.</summary>
    [Parameter] public string Icon { get; set; }
    /// <summary>Square icon size, or separate width and height, in pixels.</summary>
    [Parameter] public OneOf<int, QRCodeIconSize> IconSize { get; set; } = 40;
    [Parameter] public string Status { get; set; } = "active";
    /// <summary>Foreground CSS color. Defaults to the theme text color.</summary>
    [Parameter] public string Color { get; set; }
    [Parameter] public string BackgroundColor { get; set; } = "transparent";
    [Parameter] public RenderFragment StatusRender { get; set; }
    /// <summary>Status template with the current status, locale, and refresh callback.</summary>
    [Parameter] public RenderFragment<QRCodeStatusRenderInfo> StatusTemplate { get; set; }
    [Parameter] public bool Bordered { get; set; } = true;
    [Parameter] public EventCallback OnRefresh { get; set; }
    [Parameter] public QRCodeLocale Locale { get; set; } = LocaleProvider.CurrentLocale.QRCode;
    [Parameter] public IReadOnlyDictionary<string, string> ClassNames { get; set; }
    [Parameter] public IReadOnlyDictionary<string, string> Styles { get; set; }

    private bool[][] _matrix;
    private string _matrixValue;
    private string _matrixErrorLevel;
    private bool _matrixBoostLevel;

    private bool HasValue => !string.IsNullOrEmpty(Value);
    private bool IsSvg => string.Equals(Type, "svg", StringComparison.OrdinalIgnoreCase);
    private int ModuleCount => (_matrix?.Length ?? 0) + Math.Max(MarginSize, 0) * 2;
    private double IconWidth => IconSize.IsT0 ? IconSize.AsT0 : IconSize.AsT1?.Width ?? 40;
    private double IconHeight => IconSize.IsT0 ? IconSize.AsT0 : IconSize.AsT1?.Height ?? 40;
    private double ImageWidth => Math.Max(0, IconWidth) * ModuleCount / Math.Max(Size, 1);
    private double ImageHeight => Math.Max(0, IconHeight) * ModuleCount / Math.Max(Size, 1);
    private double ImageX => (ModuleCount - ImageWidth) / 2;
    private double ImageY => (ModuleCount - ImageHeight) / 2;
    private string BackgroundPath => $"M0,0 h{ModuleCount}v{ModuleCount}H0z";
    private string ForegroundPath => BuildPath(!string.IsNullOrEmpty(Icon));
    private QRCodeStatusRenderInfo StatusInfo => new QRCodeStatusRenderInfo(Status, Locale, Refresh);
    private string RootClass => Join(" ", "ant-qrcode", !Bordered ? "ant-qrcode-borderless" : null, Class, SemanticValue(ClassNames, "root"));
    private string RootStyle => GetComponentStyle("QRCode", Join("; ", $"background-color: {BackgroundColor}", $"width: {Size}px", $"height: {Size}px", SemanticValue(Styles, "root"), Style).TrimEnd(';') + ";");

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!HasValue || (_matrix != null && _matrixValue == Value && _matrixErrorLevel == ErrorLevel && _matrixBoostLevel == BoostLevel)) return;

        var level = ParseErrorLevel(ErrorLevel);
        using var data = QRCodeGenerator.GenerateQrCode(Value, level);
        _matrix = ReadMatrix(data);
        if (BoostLevel)
        {
            var levels = new[] { QRCodeGenerator.ECCLevel.L, QRCodeGenerator.ECCLevel.M, QRCodeGenerator.ECCLevel.Q, QRCodeGenerator.ECCLevel.H };
            foreach (var candidate in levels.Skip(Array.IndexOf(levels, level) + 1))
            {
                using var boosted = QRCodeGenerator.GenerateQrCode(Value, candidate);
                if (boosted.ModuleMatrix.Count != data.ModuleMatrix.Count) break;
                _matrix = ReadMatrix(boosted);
            }
        }
        _matrixValue = Value;
        _matrixErrorLevel = ErrorLevel;
        _matrixBoostLevel = BoostLevel;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (HasValue && !IsSvg)
            await Js.InvokeVoidAsync(JSInteropConstants.QRCodeHelper.Draw, Ref, Size, ModuleCount,
                BuildPath(false), ForegroundPath, Color, BackgroundColor, Icon,
                new[] { ImageX, ImageY, ImageWidth, ImageHeight });
    }

    /// <summary>Downloads the currently rendered QR code as a PNG or SVG file.</summary>
    public ValueTask DownloadAsync(string fileName = null) => HasValue
        ? Js.InvokeVoidAsync(JSInteropConstants.QRCodeHelper.Download, Ref, fileName)
        : default;

    private static bool[][] ReadMatrix(QRCodeData data)
    {
        // QRCoder includes four quiet-zone modules on each edge. React's
        // marginSize is explicit and defaults to zero, so strip that padding.
        var size = data.ModuleMatrix.Count - 8;
        return Enumerable.Range(0, size).Select(y => Enumerable.Range(0, size)
            .Select(x => data.ModuleMatrix[y + 4][x + 4]).ToArray()).ToArray();
    }

    private string BuildPath(bool excavate)
    {
        if (_matrix == null) return string.Empty;
        var margin = Math.Max(MarginSize, 0);
        var path = new StringBuilder();
        for (var y = 0; y < _matrix.Length; y++)
        {
            var start = -1;
            for (var x = 0; x <= _matrix.Length; x++)
            {
                var underIcon = excavate && x + margin >= Math.Floor(ImageX) && x + margin < Math.Ceiling(ImageX + ImageWidth)
                    && y + margin >= Math.Floor(ImageY) && y + margin < Math.Ceiling(ImageY + ImageHeight);
                if (x < _matrix.Length && _matrix[y][x] && !underIcon)
                {
                    if (start < 0) start = x;
                }
                else if (start >= 0)
                {
                    path.Append($"M{start + margin} {y + margin}h{x - start}v1H{start + margin}z");
                    start = -1;
                }
            }
        }
        return path.ToString();
    }

    private static string Number(double value) => value.ToString(CultureInfo.InvariantCulture);
    private static string SemanticValue(IReadOnlyDictionary<string, string> values, string key) =>
        values != null && values.TryGetValue(key, out var value) ? value : null;
    private static string Join(string separator, params string[] values) => string.Join(separator, values.Where(value => !string.IsNullOrWhiteSpace(value)));

    private static QRCodeGenerator.ECCLevel ParseErrorLevel(string level) => level switch
    {
        "L" => QRCodeGenerator.ECCLevel.L,
        "Q" => QRCodeGenerator.ECCLevel.Q,
        "H" => QRCodeGenerator.ECCLevel.H,
        _ => QRCodeGenerator.ECCLevel.M,
    };

    private Task Refresh() => OnRefresh.InvokeAsync(null);
}
