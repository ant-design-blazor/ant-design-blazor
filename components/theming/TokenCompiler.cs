using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AntDesign.Theming
{
    public interface IThemeAlgorithm
    {
        string Name { get; }
        IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens);
    }

    public sealed class DefaultThemeAlgorithm : IThemeAlgorithm
    {
        public string Name => "default";
        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens)
        {
            var values = tokens.ToDictionary(token => token.Name, token => token.Value, StringComparer.Ordinal);
            AddDerived(values, "controlHeightSM", values.Get("controlHeight", "32"), value => Scale(value, .75));
            AddDerived(values, "controlHeightXS", values.Get("controlHeight", "32"), value => Scale(value, .5));
            AddDerived(values, "controlHeightLG", values.Get("controlHeight", "32"), value => Scale(value, 1.25));
            AddDerived(values, "sizeXXL", values.Get("sizeUnit", "4"), value => Scale(value, double.Parse(values.Get("sizeStep", "4")) + 8));
            AddDerived(values, "sizeXL", values.Get("sizeUnit", "4"), value => Scale(value, double.Parse(values.Get("sizeStep", "4")) + 4));
            AddDerived(values, "sizeLG", values.Get("sizeUnit", "4"), value => Scale(value, double.Parse(values.Get("sizeStep", "4")) + 2));
            AddDerived(values, "size", values.Get("sizeUnit", "4"), value => Scale(value, double.Parse(values.Get("sizeStep", "4"))));
            AddDerived(values, "sizeSM", values.Get("sizeUnit", "4"), value => Scale(value, double.Parse(values.Get("sizeStep", "4")) - 1));
            if (values.TryGetValue("colorTextBase", out var text)) values["colorText"] = ColorMath.Alpha(text, .88);
            if (values.TryGetValue("colorBgBase", out var background)) values["colorBgContainer"] = background;
            foreach (var name in new[] { "colorPrimary", "colorSuccess", "colorWarning", "colorError", "colorInfo", "colorLink" })
            {
                if (!values.TryGetValue(name, out var color)) continue;
                var palette = AntColorPalette.Generate(color);
                for (var index = 0; index < palette.Length; index++)
                {
                    values[$"{name}{index + 1}"] = palette[index];
                    values[$"{name}-{index + 1}"] = palette[index];
                }
            }
            return values.Select(item => new ResolvedToken(item.Key, item.Value)).OrderBy(item => item.Name, StringComparer.Ordinal);
        }

        protected static void AddDerived(IDictionary<string, string> values, string name, string source, Func<string, string> derive)
        {
            if (!values.ContainsKey(name)) values[name] = derive(source);
        }

        protected static string Scale(string value, double factor)
        {
            if (!double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var number)) return value;
            return (number * factor).ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public sealed class DarkThemeAlgorithm : IThemeAlgorithm
    {
        public string Name => "dark";
        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens)
        {
            var values = new DefaultThemeAlgorithm().Resolve(tokens).ToList();
            var map = values.ToDictionary(token => token.Name, token => token.Value, StringComparer.Ordinal);
            foreach (var name in new[] { "colorPrimary", "colorSuccess", "colorWarning", "colorError", "colorInfo", "colorLink" })
            {
                if (!map.TryGetValue(name, out var color)) continue;
                var palette = AntColorPalette.GenerateDark(color, "#141414");
                for (var index = 0; index < palette.Length; index++) map[$"{name}{index + 1}"] = palette[index];
            }
            foreach (var key in map.Keys.Where(key => key.StartsWith("colorBg", StringComparison.Ordinal)).ToList())
                if (map[key] == "#ffffff") map[key] = "#141414";
            return map.Select(item => new ResolvedToken(item.Key, item.Value)).OrderBy(item => item.Name, StringComparer.Ordinal);
        }
    }

    internal static class ColorMath
    {
        public static string Alpha(string color, double alpha)
        {
            if (color == null) return string.Empty;
            if (color.StartsWith("#", StringComparison.Ordinal) && color.Length == 7 &&
                int.TryParse(color.Substring(1, 2), System.Globalization.NumberStyles.HexNumber, null, out var r) &&
                int.TryParse(color.Substring(3, 2), System.Globalization.NumberStyles.HexNumber, null, out var g) &&
                int.TryParse(color.Substring(5, 2), System.Globalization.NumberStyles.HexNumber, null, out var b))
                return $"rgba({r}, {g}, {b}, {alpha.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)})";
            return color;
        }
    }

    internal static class AntColorPalette
    {
        private const double HueStep = 2, SaturationStep = .16, SaturationStep2 = .05, BrightnessStep1 = .05, BrightnessStep2 = .15;

        public static string[] Generate(string value)
        {
            if (!TryHex(value, out var r, out var g, out var b)) return Array.Empty<string>();
            ToHsv(r, g, b, out var h, out var s, out var v);
            var colors = new List<string>();
            for (var i = 5; i > 0; i--) colors.Add(FromHsv(Hue(h, i, true), Saturation(h, s, i, true), Value(v, i, true)));
            colors.Add(value.ToLowerInvariant());
            for (var i = 1; i <= 4; i++) colors.Add(FromHsv(Hue(h, i, false), Saturation(h, s, i, false), Value(v, i, false)));
            return colors.ToArray();
        }

        public static string[] GenerateDark(string value, string background)
        {
            var patterns = Generate(value);
            var indexes = new[] { 7, 6, 5, 5, 5, 5, 4, 3, 2, 1 };
            var amounts = new[] { 15, 25, 30, 45, 65, 85, 90, 95, 97, 98 };
            return indexes.Select((index, position) => Mix(background, patterns[index], amounts[position])).ToArray();
        }

        private static double Hue(double h, int i, bool light)
        {
            var result = h >= 60 && h <= 240 ? h + (light ? -HueStep * i : HueStep * i) : h + (light ? HueStep * i : -HueStep * i);
            return (result % 360 + 360) % 360;
        }

        private static double Saturation(double h, double s, int i, bool light)
        {
            if (h == 0 && s == 0) return 0;
            var result = light ? s - SaturationStep * i : i == 4 ? s + SaturationStep : s + SaturationStep2 * i;
            if (light && i == 5 && result > .1) result = .1;
            return Math.Round(Math.Clamp(result, .06, 1) * 100, MidpointRounding.AwayFromZero) / 100;
        }

        private static double Value(double v, int i, bool light) => Math.Round(Math.Clamp(v + (light ? BrightnessStep1 * i : -BrightnessStep2 * i), 0, 1) * 100, MidpointRounding.AwayFromZero) / 100;

        private static string FromHsv(double h, double s, double v)
        {
            var sector = h / 60; var i = (int)Math.Floor(sector); var f = sector - i;
            var p = Round(v * (1 - s) * 255); var q = Round(v * (1 - s * f) * 255); var t = Round(v * (1 - s * (1 - f)) * 255); var vv = Round(v * 255);
            var channels = i switch { 0 => (vv, t, p), 1 => (q, vv, p), 2 => (p, vv, t), 3 => (p, q, vv), 4 => (t, p, vv), _ => (vv, p, q) };
            return $"#{channels.Item1:X2}{channels.Item2:X2}{channels.Item3:X2}".ToLowerInvariant();
        }

        private static int Round(double value) => (int)Math.Floor(value + .5);

        private static string Mix(string first, string second, int amount)
        {
            if (!TryHex(first, out var r1, out var g1, out var b1) || !TryHex(second, out var r2, out var g2, out var b2)) return second;
            var p = amount / 100d;
            return $"#{Round((r2 - r1) * p + r1):X2}{Round((g2 - g1) * p + g1):X2}{Round((b2 - b1) * p + b1):X2}".ToLowerInvariant();
        }

        private static bool TryHex(string value, out int r, out int g, out int b)
        {
            r = g = b = 0; var hex = value?.Trim().TrimStart('#');
            if (hex?.Length == 3) hex = string.Concat(hex[0], hex[0], hex[1], hex[1], hex[2], hex[2]);
            return hex?.Length == 6 && int.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out r) && int.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out g) && int.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out b);
        }

        private static void ToHsv(int r, int g, int b, out double h, out double s, out double v)
        {
            var rf = r / 255d; var gf = g / 255d; var bf = b / 255d; var max = Math.Max(rf, Math.Max(gf, bf)); var min = Math.Min(rf, Math.Min(gf, bf)); var delta = max - min;
            h = delta == 0 ? 0 : max == rf ? 60 * ((gf - bf) / delta + (gf < bf ? 6 : 0)) : max == gf ? 60 * ((bf - rf) / delta + 2) : 60 * ((rf - gf) / delta + 4);
            h = Math.Floor(h + .5); s = max == 0 ? 0 : delta / max; v = max;
        }
    }

    internal static class TokenDictionaryExtensions
    {
        public static string Get(this IDictionary<string, string> values, string name, string fallback)
        {
            return values.TryGetValue(name, out var value) ? value : fallback;
        }
    }

    public static class PresetColorPalettes
    {
        private static readonly IReadOnlyDictionary<string, string> PrimaryColors = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["red"] = "#f5222d", ["volcano"] = "#fa541c", ["orange"] = "#fa8c16", ["gold"] = "#faad14",
            ["yellow"] = "#fadb14", ["lime"] = "#a0d911", ["green"] = "#52c41a", ["cyan"] = "#13c2c2",
            ["blue"] = "#1677ff", ["geekblue"] = "#2f54eb", ["purple"] = "#722ed1", ["magenta"] = "#eb2f96",
            ["grey"] = "#8c8c8c"
        };

        public static IReadOnlyDictionary<string, string[]> All { get; } = PrimaryColors.ToDictionary(item => item.Key, item => AntColorPalette.Generate(item.Value), StringComparer.Ordinal);
        public static IReadOnlyDictionary<string, string[]> Dark { get; } = PrimaryColors.ToDictionary(item => item.Key, item => AntColorPalette.GenerateDark(item.Value, "#141414"), StringComparer.Ordinal);
        public static string Primary(string name) => PrimaryColors.TryGetValue(name, out var color) ? color : null;
    }

    public sealed class TokenCompilationManifest
    {
        public string Layer { get; set; } = "token-values";
        public string Theme { get; set; } = "default";
        public string Scope { get; set; } = ":root";
        public string Algorithm { get; set; } = "default";
        public string Digest { get; set; } = string.Empty;
        public string ManagedFile { get; set; } = "token-values.css";
        public string CacheKey { get; set; } = string.Empty;
    }

    public sealed class TokenCompilationResult
    {
        public IReadOnlyList<ResolvedToken> Resolved { get; internal set; } = Array.Empty<ResolvedToken>();
        public IReadOnlyList<CssVarToken> CssVariables { get; internal set; } = Array.Empty<CssVarToken>();
        public string Css { get; internal set; } = string.Empty;
        public TokenCompilationManifest Manifest { get; internal set; } = new();
    }

    public static class TokenCompiler
    {
        public static TokenCompilationResult Compile(
            IEnumerable<DesignToken> tokens,
            string selector,
            string theme = "default",
            IThemeAlgorithm algorithm = null,
            string managedFile = "token-values.css")
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            if (string.IsNullOrWhiteSpace(selector)) throw new ArgumentException("A CSS selector is required.", nameof(selector));
            algorithm ??= new DefaultThemeAlgorithm();
            var resolved = algorithm.Resolve(tokens).OrderBy(token => token.Name, StringComparer.Ordinal).ToList();
            var cssTokens = resolved.Select(token => new DesignToken
            {
                Name = token.Name,
                Value = token.Value,
                CssVariable = $"--ant-{TokenView.ToKebabCase(token.Name)}",
                Source = token.Source
            }).ToList();
            var css = TokenCssSerializer.SerializeVariables(selector, cssTokens);
            var digest = Sha256(css);
            var normalizedTheme = theme ?? "default";
            return new TokenCompilationResult
            {
                Resolved = resolved,
                CssVariables = cssTokens.Select(token => new CssVarToken(token.Name, token.CssVariable)).ToList(),
                Css = css,
                Manifest = new TokenCompilationManifest
                {
                    Theme = normalizedTheme,
                    Scope = selector,
                    Algorithm = algorithm.Name,
                    Digest = digest,
                    ManagedFile = managedFile ?? string.Empty,
                    CacheKey = $"token-values:{normalizedTheme}:{selector}:{algorithm.Name}:{digest}"
                }
            };
        }

        private static string Sha256(string value)
        {
            using var hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
