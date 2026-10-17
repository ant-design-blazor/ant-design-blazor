using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

/// <summary>Interactive color selector with HSB controls, opacity adjustment, presets, and formatted value input.</summary>
[Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "", Title = "ColorPicker", SubTitle = "颜色选择器")]
public partial class ColorPicker : AntDomComponentBase
{
    [Parameter] public string Value { get; set; } = "#1677ff";
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public bool Open { get; set; }
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }
    /// <summary>Trigger size: small, middle, or large.</summary>
    /// <default value="middle" />
    [Parameter] public string Size { get; set; } = "middle";
    [Parameter] public bool ShowText { get; set; }
    [Parameter] public bool Disabled { get; set; }
    /// <summary>Whether to show controls that clear the selected color.</summary>
    [Parameter] public bool AllowClear { get; set; }
    /// <summary>Named preset groups displayed in the panel.</summary>
    [Parameter] public IReadOnlyList<ColorPickerPreset> Presets { get; set; } = Array.Empty<ColorPickerPreset>();
    /// <summary>Raised when the selected color is cleared.</summary>
    [Parameter] public EventCallback OnClear { get; set; }
    /// <summary>Display format used by the panel input and trigger text: hex, rgb, or hsb.</summary>
    [Parameter] public string Format { get; set; } = "hex";
    [Parameter] public EventCallback<string> FormatChanged { get; set; }
    [Parameter] public bool DisabledAlpha { get; set; }
    [Parameter] public bool DisabledFormat { get; set; }
    [Parameter] public string AriaLabel { get; set; } = "Choose color";

    private bool IsCleared => string.Equals(Value, "transparent", StringComparison.OrdinalIgnoreCase);
    private string CurrentValue => string.IsNullOrWhiteSpace(Value) ? "#1677ff" : Value;
    private string InputValue => IsCleared ? "1677ff" : ToHex(Hsva)[1..];
    private (int R, int G, int B) Rgb => HsvToRgb(Hsva.H, Hsva.S, Hsva.V);
    private RenderFragment HashPrefix => builder =>
    {
        builder.OpenElement(0, "span");
        builder.AddContent(1, "#");
        builder.CloseElement();
    };
    private (double H, double S, double V, double A) Hsva => ParseColor(CurrentValue);
    private string HueColor => $"hsl({Hsva.H:0},100%,50%)";
    private string OpaqueColor => ToRgbString(Hsva with { A = 1 });
    private bool _draggingSaturation;
    private bool _draggingHue;
    private bool _draggingAlpha;
    private ElementReference _saturationElement;
    private ElementReference _hueElement;
    private ElementReference _alphaElement;
    private double _saturationWidth;
    private double _saturationHeight;
    private double _hueWidth;
    private double _alphaWidth;
    private string _format = "hex";
    private string TriggerClass
    {
        get
        {
            var sizeClass = Size?.ToLowerInvariant() switch
            {
                "small" => " ant-color-picker-sm",
                "large" => " ant-color-picker-lg",
                _ => string.Empty
            };
            var stateClass = (Open && !Disabled ? " ant-color-picker-trigger-active" : string.Empty)
                + (Disabled ? " ant-color-picker-trigger-disabled" : string.Empty);
            return $"ant-color-picker-trigger ant-color-picker-css-var{sizeClass}{stateClass} {Class}".Trim();
        }
    }

    private string DisplayValue => IsCleared ? "Transparent" : ToFormatString(Hsva, _format);

    protected override void OnParametersSet()
    {
        if (Format is "hex" or "rgb" or "hsb") _format = Format;
    }

    private string GetPresetClass(string color)
    {
        var isBright = TryGetBrightness(color, out var brightness) && brightness > 192;
        return "ant-color-picker-presets-color"
            + (string.Equals(CurrentValue, color, StringComparison.OrdinalIgnoreCase) ? " ant-color-picker-presets-color-checked" : string.Empty)
            + (isBright ? " ant-color-picker-presets-color-bright" : string.Empty);
    }

    private static bool TryGetBrightness(string color, out int brightness)
    {
        brightness = 0;
        if (string.IsNullOrWhiteSpace(color)) return false;
        var hex = color.TrimStart('#');
        if (hex.Length == 3)
        {
            hex = string.Concat(hex[0], hex[0], hex[1], hex[1], hex[2], hex[2]);
        }
        if (hex.Length != 6 || !int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var rgb)) return false;
        var red = (rgb >> 16) & 0xff;
        var green = (rgb >> 8) & 0xff;
        var blue = rgb & 0xff;
        brightness = (int)(red * 0.299 + green * 0.587 + blue * 0.114);
        return true;
    }

    private async Task Toggle(MouseEventArgs _)
    {
        if (!Disabled)
        {
            Open = !Open;
            await OpenChanged.InvokeAsync(Open);
        }
    }

    private Task OnHexInput(ChangeEventArgs args)
    {
        var text = args.Value?.ToString() ?? string.Empty;
        if (TryParseColor(text, out var parsed))
        {
            Value = ToColorString(parsed);
            return ValueChanged.InvokeAsync(Value);
        }
        return Task.CompletedTask;
    }

    private Task OnFormatSelected(string format)
    {
        if (format is not ("hex" or "rgb" or "hsb")) return Task.CompletedTask;
        _format = format;
        Format = format;
        return FormatChanged.InvokeAsync(format);
    }

    private Task OnRedChanged(double red) => SetRgbColor((int)Math.Round(red), Rgb.G, Rgb.B);
    private Task OnGreenChanged(double green) => SetRgbColor(Rgb.R, (int)Math.Round(green), Rgb.B);
    private Task OnBlueChanged(double blue) => SetRgbColor(Rgb.R, Rgb.G, (int)Math.Round(blue));
    private Task OnHueChanged(double hue) => SetColor(hue, Hsva.S, Hsva.V, Hsva.A);
    private Task OnSaturationChanged(double saturation) => SetColor(Hsva.H, saturation / 100, Hsva.V, Hsva.A);
    private Task OnBrightnessChanged(double brightness) => SetColor(Hsva.H, Hsva.S, brightness / 100, Hsva.A);
    private Task OnAlphaChanged(double alpha) => SetColor(Hsva.H, Hsva.S, Hsva.V, alpha / 100);

    private Task SetRgbColor(int red, int green, int blue)
    {
        var color = RgbToHsva(red, green, blue, Hsva.A);
        return SetColor(color.H, color.S, color.V, color.A);
    }

    private static string FormatHue(double hue) => Math.Round(hue).ToString(System.Globalization.CultureInfo.InvariantCulture);
    private static string FormatPercent(double value) => $"{Math.Round(value)}%";

    private async Task StartSaturation(PointerEventArgs args)
    {
        _draggingSaturation = true;
        var rect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _saturationElement);
        _saturationWidth = (double)(rect?.Width ?? 0);
        _saturationHeight = (double)(rect?.Height ?? 0);
        await UpdateSaturation(args);
    }

    private Task MoveSaturation(PointerEventArgs args) => _draggingSaturation ? UpdateSaturation(args) : Task.CompletedTask;
    private void StopSaturation(PointerEventArgs _) => _draggingSaturation = false;
    private async Task StartHue(PointerEventArgs args)
    {
        _draggingHue = true;
        var rect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _hueElement);
        _hueWidth = (double)(rect?.Width ?? 0);
        await UpdateHue(args);
    }
    private Task MoveHue(PointerEventArgs args) => _draggingHue ? UpdateHue(args) : Task.CompletedTask;
    private void StopHue(PointerEventArgs _) => _draggingHue = false;
    private async Task StartAlpha(PointerEventArgs args)
    {
        _draggingAlpha = true;
        var rect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _alphaElement);
        _alphaWidth = (double)(rect?.Width ?? 0);
        await UpdateAlpha(args);
    }
    private Task MoveAlpha(PointerEventArgs args) => _draggingAlpha ? UpdateAlpha(args) : Task.CompletedTask;
    private void StopAlpha(PointerEventArgs _) => _draggingAlpha = false;

    private Task UpdateSaturation(PointerEventArgs args) => _saturationWidth > 0 && _saturationHeight > 0
        ? SetColor(Hsva.H, Clamp(args.OffsetX / _saturationWidth), 1 - Clamp(args.OffsetY / _saturationHeight), Hsva.A)
        : Task.CompletedTask;
    private Task UpdateHue(PointerEventArgs args) => _hueWidth > 0
        ? SetColor(Clamp(args.OffsetX / _hueWidth) * 359, Hsva.S, Hsva.V, Hsva.A)
        : Task.CompletedTask;
    private Task UpdateAlpha(PointerEventArgs args) => _alphaWidth > 0
        ? SetColor(Hsva.H, Hsva.S, Hsva.V, Clamp(args.OffsetX / _alphaWidth))
        : Task.CompletedTask;

    private Task HandleHueKeyDown(KeyboardEventArgs args) => args.Key switch
    {
        "ArrowLeft" or "ArrowDown" => SetColor(Hsva.H - 1, Hsva.S, Hsva.V, Hsva.A),
        "ArrowRight" or "ArrowUp" => SetColor(Hsva.H + 1, Hsva.S, Hsva.V, Hsva.A),
        _ => Task.CompletedTask
    };

    private Task HandleAlphaKeyDown(KeyboardEventArgs args) => args.Key switch
    {
        "ArrowLeft" or "ArrowDown" => SetColor(Hsva.H, Hsva.S, Hsva.V, Hsva.A - 0.01),
        "ArrowRight" or "ArrowUp" => SetColor(Hsva.H, Hsva.S, Hsva.V, Hsva.A + 0.01),
        _ => Task.CompletedTask
    };

    private Task SetColor(double h, double s, double v, double a)
    {
        if (Disabled) return Task.CompletedTask;
        h = (h % 360 + 360) % 360;
        Value = ToColorString((h, Clamp(s), Clamp(v), Clamp(a)));
        return ValueChanged.InvokeAsync(Value);
    }

    private async Task Clear(MouseEventArgs _)
    {
        if (Disabled) return;
        Value = "transparent";
        await ValueChanged.InvokeAsync(Value);
        await OnClear.InvokeAsync();
    }

    private Task OnClearKeyDown(KeyboardEventArgs args)
    {
        return args.Key is "Enter" or " " ? Clear(new MouseEventArgs()) : Task.CompletedTask;
    }

    private Task SelectPreset(string color)
    {
        if (Disabled) return Task.CompletedTask;
        Value = color;
        return ValueChanged.InvokeAsync(Value);
    }

    private Task OnPresetKeyDown(string color, KeyboardEventArgs args)
    {
        return args.Key is "Enter" or " " ? SelectPreset(color) : Task.CompletedTask;
    }

    private static double Clamp(double value) => Math.Max(0, Math.Min(1, value));

    private static (double H, double S, double V, double A) ParseColor(string value)
    {
        if (TryParseColor(value, out var color)) return color;
        return (215, 0.93, 1, 1);
    }

    private static bool TryParseColor(string value, out (double H, double S, double V, double A) color)
    {
        color = (0, 0, 0, 1);
        if (string.IsNullOrWhiteSpace(value) || value == "transparent") return false;
        var text = value.Trim();
        if (text.StartsWith('#') || (text.Length is 3 or 6 && text.All(Uri.IsHexDigit)))
        {
            var hex = text.StartsWith('#') ? text[1..] : text;
            if (hex.Length == 3) hex = string.Concat(hex[0], hex[0], hex[1], hex[1], hex[2], hex[2]);
            if (hex.Length != 6 || !int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var rgb)) return false;
            color = RgbToHsva((rgb >> 16) & 255, (rgb >> 8) & 255, rgb & 255, 1);
            return true;
        }
        if (text.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            var values = text[(text.IndexOf('(') + 1)..].TrimEnd(')').Split(',', StringSplitOptions.TrimEntries);
            if (values.Length is < 3 or > 4
                || !double.TryParse(values[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var red)
                || !double.TryParse(values[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var green)
                || !double.TryParse(values[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var blue)) return false;
            var alpha = values.Length == 4 && double.TryParse(values[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var parsedAlpha) ? parsedAlpha : 1;
            color = RgbToHsva((int)red, (int)green, (int)blue, alpha);
            return true;
        }
        if (text.StartsWith("hsb", StringComparison.OrdinalIgnoreCase))
        {
            var values = text[(text.IndexOf('(') + 1)..].TrimEnd(')').Split(',', StringSplitOptions.TrimEntries);
            if (values.Length is < 3 or > 4
                || !double.TryParse(values[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var hue)
                || !double.TryParse(values[1].TrimEnd('%'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var saturation)
                || !double.TryParse(values[2].TrimEnd('%'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var brightness)) return false;
            var alpha = values.Length == 4 && double.TryParse(values[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var parsedAlpha) ? parsedAlpha : 1;
            color = (hue, Clamp(saturation / 100), Clamp(brightness / 100), Clamp(alpha));
            return true;
        }
        return false;
    }

    private static (double H, double S, double V, double A) RgbToHsva(int red, int green, int blue, double alpha)
    {
        var r = red / 255d; var g = green / 255d; var b = blue / 255d;
        var max = Math.Max(r, Math.Max(g, b)); var min = Math.Min(r, Math.Min(g, b)); var delta = max - min;
        var hue = delta == 0 ? 0 : max == r ? 60 * (((g - b) / delta) % 6) : max == g ? 60 * ((b - r) / delta + 2) : 60 * ((r - g) / delta + 4);
        if (hue < 0) hue += 360;
        return (hue, max == 0 ? 0 : delta / max, max, Clamp(alpha));
    }

    private static (int R, int G, int B) HsvToRgb(double h, double s, double v)
    {
        var chroma = v * s; var x = chroma * (1 - Math.Abs((h / 60 % 2) - 1)); var m = v - chroma;
        var (r, g, b) = h switch
        {
            < 60 => (chroma, x, 0d), < 120 => (x, chroma, 0d), < 180 => (0d, chroma, x),
            < 240 => (0d, x, chroma), < 300 => (x, 0d, chroma), _ => (chroma, 0d, x)
        };
        return ((int)Math.Round((r + m) * 255), (int)Math.Round((g + m) * 255), (int)Math.Round((b + m) * 255));
    }

    private static string ToHex((double H, double S, double V, double A) hsva)
    {
        var (r, g, b) = HsvToRgb(hsva.H, hsva.S, hsva.V);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private static string ToRgbString((double H, double S, double V, double A) hsva)
    {
        var (r, g, b) = HsvToRgb(hsva.H, hsva.S, hsva.V);
        return hsva.A >= 0.999 ? $"rgb({r}, {g}, {b})" : $"rgba({r}, {g}, {b}, {hsva.A:0.##})";
    }

    private static string ToColorString((double H, double S, double V, double A) hsva) => hsva.A >= 0.999 ? ToHex(hsva) : ToRgbString(hsva);

    private static string ToFormatString((double H, double S, double V, double A) hsva, string format) => format switch
    {
        "rgb" => ToRgbString(hsva),
        "hsb" => $"hsb({hsva.H:0}, {hsva.S * 100:0}, {hsva.V * 100:0}{(hsva.A < 0.999 ? $", {hsva.A:0.##}" : string.Empty)})",
        _ => hsva.A < 0.999 ? $"{ToHex(hsva)},{hsva.A * 100:0}%" : ToHex(hsva)
    };
}

/// <summary>A labeled set of preset colors for <see cref="ColorPicker"/>.</summary>
public sealed class ColorPickerPreset
{
    /// <summary>Visible group label.</summary>
    public string Label { get; set; } = string.Empty;
    /// <summary>CSS color values offered in this group.</summary>
    public IReadOnlyList<string> Colors { get; set; } = Array.Empty<string>();
}
