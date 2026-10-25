using System;
using System.Globalization;

namespace AntDesign.Colors;

/// <summary>Color operations used by the v6 neutral, alias and shadow token algorithms.</summary>
public static class ThemeColorMath
{
    /// <summary>Replace HSL lightness.</summary>
    public static string WithHslLightness(string color, double lightness) => ColorPalette.SetLightness(color, lightness);
    /// <summary>Set alpha.</summary>
    public static string Alpha(string color, double alpha) => ColorPalette.SetAlpha(color, alpha);
    /// <summary>Multiply the existing alpha.</summary>
    public static string WithAlpha(string color, double alpha) => Alpha(color, ColorPalette.ToRgba(color).A * alpha);
    /// <summary>Darken with FastColor rules.</summary>
    public static string Darken(string color, double amount) => ColorPalette.Darken(color, amount);
    /// <summary>Lighten with FastColor rules.</summary>
    public static string Lighten(string color, double amount) => ColorPalette.Lighten(color, amount);
    /// <summary>Mix colors.</summary>
    public static string Mix(string first, string second, int amount) => ColorPalette.Mix(first, second, amount);

    /// <summary>Port of Ant Design v6 theme/util/getAlphaColor.ts.</summary>
    public static string GetAlphaColor(string frontColor, string backgroundColor)
    {
        var front = ColorPalette.ToRgba(frontColor);
        if (front.A < 1) return frontColor;
        var background = ColorPalette.ToRgba(backgroundColor);
        for (var alpha = .01; alpha <= 1; alpha += .01)
        {
            int Channel(int f, int b) => (int)Math.Floor((f - b * (1 - alpha)) / alpha + .5);
            var r = Channel(front.R, background.R); var g = Channel(front.G, background.G); var b = Channel(front.B, background.B);
            if (r is >= 0 and <= 255 && g is >= 0 and <= 255 && b is >= 0 and <= 255)
                return $"rgba({r}, {g}, {b}, {(Math.Floor(alpha * 100 + .5) / 100).ToString("0.##", CultureInfo.InvariantCulture)})";
        }
        return ColorPalette.SetAlpha(frontColor, 1);
    }
}
