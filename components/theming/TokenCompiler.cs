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
            return values.Select(token => token.Name.StartsWith("colorBg", StringComparison.Ordinal) && token.Value == "#ffffff"
                ? new ResolvedToken(token.Name, "#141414", token.Source)
                : token);
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

    internal static class TokenDictionaryExtensions
    {
        public static string Get(this IDictionary<string, string> values, string name, string fallback)
        {
            return values.TryGetValue(name, out var value) ? value : fallback;
        }
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
