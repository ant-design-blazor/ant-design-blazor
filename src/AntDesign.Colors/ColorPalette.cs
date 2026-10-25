using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AntDesign.Colors;

/// <summary>Ant Design v6 palettes, ported from @ant-design/colors 8.0.1 generate.ts.</summary>
/// <remarks>Uses the matching @ant-design/fast-color conversion and rounding rules.
/// This library has no dependency on Blazor, Icons, JavaScript, or System.Drawing.</remarks>
public static class ColorPalette
{
    /// <summary>Resolve a supported CSS color to RGBA channels, using FastColor rules.</summary>
    public static (int R, int G, int B, double A) ToRgba(string? color)
    {
        var parsed = Rgba.Parse(color);
        return (parsed.r, parsed.g, parsed.b, parsed.a);
    }

    /// <summary>Mix colors with FastColor's per-channel and alpha rounding.</summary>
    public static string Mix(string first, string second, double amount = 50) => Rgba.Parse(first).Mix(Rgba.Parse(second), amount).Hex();

    /// <summary>Set alpha and serialize as an RGB(A) CSS value.</summary>
    public static string SetAlpha(string color, double alpha)
    {
        var c = Rgba.Parse(color);
        alpha = Clamp(alpha, 0, 1);
        return alpha == 1 ? $"rgb({c.r}, {c.g}, {c.b})" : $"rgba({c.r}, {c.g}, {c.b}, {alpha.ToString("R", CultureInfo.InvariantCulture)})";
    }

    /// <summary>FastColor lighten: HSV saturation combined with HSL lightness.</summary>
    public static string Lighten(string color, double amount = 10) => AdjustLightness(color, amount);
    /// <summary>FastColor darken.</summary>
    public static string Darken(string color, double amount = 10) => AdjustLightness(color, -amount);

    /// <summary>Replace HSL lightness while preserving hue and HSL saturation.</summary>
    public static string SetLightness(string color, double lightness)
    {
        var c = Rgba.Parse(color);
        return Rgba.FromHsl(c.Hsv().h, c.Hsl().s, lightness, c.a).Hex();
    }

    private static string AdjustLightness(string color, double amount)
    {
        var c = Rgba.Parse(color);
        return Rgba.FromHsl(c.Hsv().h, c.Hsv().s, Clamp(c.Hsl().l + amount / 100, 0, 1), c.a).Hex();
    }
    private static readonly int[] DarkIndexes = { 7, 6, 5, 5, 5, 5, 4, 3, 2, 1 };
    private static readonly int[] DarkAmounts = { 15, 25, 30, 45, 65, 85, 90, 95, 97, 98 };

    /// <summary>Generate all ten light or dark swatches, in upstream order.</summary>
    public static string[] Generate(string? color, bool dark = false, string? backgroundColor = null)
    {
        var primary = Rgba.Parse(color);
        var (h, s, v) = primary.Hsv();
        var patterns = new Rgba[10];
        for (var i = 5; i > 0; i--) patterns[5 - i] = Swatch(h, s, v, i, true);
        patterns[5] = primary;
        for (var i = 1; i <= 4; i++) patterns[5 + i] = Swatch(h, s, v, i, false);
        if (!dark) return patterns.Select(c => c.Hex()).ToArray();
        var background = Rgba.Parse(string.IsNullOrEmpty(backgroundColor) ? "#141414" : backgroundColor);
        return DarkIndexes.Select((index, position) => background.Mix(patterns[index], DarkAmounts[position]).Hex()).ToArray();
    }

    private static Rgba Swatch(double h, double s, double v, int i, bool light)
    {
        var roundedHue = Round(h);
        var hue = roundedHue >= 60 && roundedHue <= 240
            ? roundedHue + (light ? -2 * i : 2 * i) : roundedHue + (light ? 2 * i : -2 * i);
        var saturation = s;
        if (!(h == 0 && s == 0))
        {
            saturation = light ? s - .16 * i : i == 4 ? s + .16 : s + .05 * i;
            saturation = Math.Min(1, saturation);
            if (light && i == 5) saturation = Math.Min(.1, saturation);
            saturation = Round(Math.Max(.06, saturation) * 100) / 100d;
        }
        var value = Round(Clamp(v + (light ? .05 * i : -.15 * i), 0, 1) * 100) / 100d;
        return Rgba.FromHsv(hue, saturation, value);
    }

    private static int Round(double value) => (int)Math.Floor(value + .5);
    private static double Clamp(double value, double min, double max) => Math.Min(max, Math.Max(min, value));

    // Conversion subset used by generate(), ported from FastColor. In particular,
    // retain original HSL/HSV hue and HSV s/v instead of recomputing rounded RGB.
    private sealed class Rgba
    {
        internal readonly int r, g, b;
        internal readonly double a;
        private double? hue, saturation, value, hslSaturation, lightness;
        private Rgba(int r, int g, int b, double a = 1) { this.r = r; this.g = g; this.b = b; this.a = a; }

        internal (double h, double s, double v) Hsv()
        {
            var max = Math.Max(r, Math.Max(g, b));
            var delta = max - Math.Min(r, Math.Min(g, b));
            return (hue ?? (delta == 0 ? 0 : Round(60 * (r == max ? (g - b) / (double)delta + (g < b ? 6 : 0)
                : g == max ? (b - r) / (double)delta + 2 : (r - g) / (double)delta + 4))),
                saturation ?? (max == 0 ? 0 : delta / (double)max), value ?? max / 255d);
        }

        internal string Hex() => $"#{r:x2}{g:x2}{b:x2}" + (a >= 0 && a < 1 ? Round(a * 255).ToString("x2", CultureInfo.InvariantCulture) : "");
        internal (double s, double l) Hsl()
        {
            var max = Math.Max(r, Math.Max(g, b)); var min = Math.Min(r, Math.Min(g, b));
            var l = lightness ?? (max + min) / 510d;
            return (hslSaturation ?? (max == min ? 0 : (max - min) / 255d / (1 - Math.Abs(2 * l - 1))), l);
        }
        internal Rgba Mix(Rgba other, double amount)
        {
            var p = amount / 100;
            return new Rgba(Round((other.r - r) * p + r), Round((other.g - g) * p + g),
                Round((other.b - b) * p + b), Round(((other.a - a) * p + a) * 100) / 100d);
        }

        internal static Rgba FromHsv(double h, double s, double v, double a = 1)
        {
            h = (h % 360 + 360) % 360;
            var sector = h / 60; var i = (int)Math.Floor(sector); var f = sector - i;
            var p = Round(v * (1 - s) * 255); var q = Round(v * (1 - s * f) * 255);
            var t = Round(v * (1 - s * (1 - f)) * 255); var vv = Round(v * 255);
            var (r, g, b) = i switch { 0 => (vv, t, p), 1 => (q, vv, p), 2 => (p, vv, t), 3 => (p, q, vv), 4 => (t, p, vv), _ => (vv, p, q) };
            return new Rgba(r, g, b, a) { hue = h, saturation = s, value = v };
        }

        internal static Rgba FromHsl(double h, double s, double l, double a)
        {
            h = (h % 360 + 360) % 360;
            var chroma = (1 - Math.Abs(2 * l - 1)) * s;
            var x = chroma * (1 - Math.Abs(h / 60 % 2 - 1)); var m = l - chroma / 2;
            var (r, g, b) = (int)(h / 60) switch { 0 => (chroma, x, 0d), 1 => (x, chroma, 0d), 2 => (0d, chroma, x), 3 => (0d, x, chroma), 4 => (x, 0d, chroma), _ => (chroma, 0d, x) };
            return new Rgba(Round((r + m) * 255), Round((g + m) * 255), Round((b + m) * 255), a) { hue = h, hslSaturation = s, lightness = l };
        }

        internal static Rgba Parse(string? input)
        {
            var text = input?.Trim() ?? "";
            if (NamedColors.Values.TryGetValue(text, out var named)) text = named;
            var hex = text.TrimStart('#');
            if (Regex.IsMatch(hex, "\\A[0-9a-fA-F]{3,8}\\z"))
            {
                if (hex.Length is 3 or 4) hex = string.Concat(hex.Select(c => new string(c, 2)));
                if (hex.Length is 6 or 8)
                    return new Rgba(Convert.ToInt32(hex.Substring(0, 2), 16), Convert.ToInt32(hex.Substring(2, 2), 16),
                        Convert.ToInt32(hex.Substring(4, 2), 16), hex.Length == 8 ? Convert.ToInt32(hex.Substring(6, 2), 16) / 255d : 1);
            }
            if (text.StartsWith("rgb", StringComparison.Ordinal) || text.StartsWith("hsl", StringComparison.Ordinal)
                || text.StartsWith("hsv", StringComparison.Ordinal) || text.StartsWith("hsb", StringComparison.Ordinal))
            {
                // Same accepted CSS number/percentage tokens as FastColor splitColorStr.
                var body = text.Contains("(") ? text.Substring(text.IndexOf('(') + 1).Split(')')[0] : text;
                var cells = Regex.Matches(body, @"\d*\.?\d+%?").Cast<Match>().Select(m => m.Value).ToArray();
                double Cell(int i) => i < cells.Length ? double.Parse(cells[i].TrimEnd('%'), CultureInfo.InvariantCulture) : 0;
                var alpha = cells.Length > 3 ? Cell(3) / (cells[3].EndsWith("%", StringComparison.Ordinal) ? 100 : 1) : 1;
                if (text.StartsWith("rgb", StringComparison.Ordinal))
                {
                    int Channel(int i) => Round(i < cells.Length && cells[i].EndsWith("%", StringComparison.Ordinal) ? Cell(i) / 100 * 255 : Cell(i));
                    return new Rgba(Channel(0), Channel(1), Channel(2), alpha);
                }
                return text.StartsWith("hsl", StringComparison.Ordinal) ? FromHsl(Cell(0), Cell(1) / 100, Cell(2) / 100, alpha)
                    : FromHsv(Cell(0), Cell(1) / 100, Cell(2) / 100, alpha);
            }
            // FastColor initializes unknown/empty string inputs to opaque black.
            return new Rgba(0, 0, 0);
        }
    }
}
