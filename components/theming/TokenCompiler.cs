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
        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens) => tokens.Select(token => new ResolvedToken(token.Name, token.Value, token.Source));
    }

    public sealed class DarkThemeAlgorithm : IThemeAlgorithm
    {
        public string Name => "dark";
        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens) => tokens.Select(token => new ResolvedToken(token.Name, token.Value, token.Source));
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
