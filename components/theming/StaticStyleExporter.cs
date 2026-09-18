using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AntDesign.Theming
{
    public sealed class StaticStyleRequest
    {
        public ThemeConfig Theme { get; set; } = new ThemeConfig();
        public string Selector { get; set; }
        public string StaticCss { get; set; } = string.Empty;
        public bool IncludeStyleTag { get; set; } = true;
        public string Nonce { get; set; }
    }

    public sealed class StaticStyleResult
    {
        public string Css { get; internal set; } = string.Empty;
        public string StyleTag { get; internal set; } = string.Empty;
        public string Digest { get; internal set; } = string.Empty;
        public string CacheKey { get; internal set; } = string.Empty;
        public string Scope { get; internal set; } = string.Empty;
        public string Algorithm { get; internal set; } = string.Empty;
    }

    public interface IStaticStyleExporter
    {
        StaticStyleResult Export(StaticStyleRequest request);
    }

    public sealed class StaticStyleExporter : IStaticStyleExporter
    {
        public StaticStyleResult Export(StaticStyleRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var theme = request.Theme ?? new ThemeConfig();
            var selector = string.IsNullOrWhiteSpace(request.Selector)
                ? $":where(.{theme.ThemeScope}[data-theme='{theme.Key}'])"
                : request.Selector;
            var tokens = theme.GetResolvedTokenValues();
            var tokenCss = TokenCssSerializer.SerializeVariables(selector, tokens.Select(item => new DesignToken
            {
                Name = item.Key,
                Value = item.Value,
                CssVariable = $"--ant-{TokenView.ToKebabCase(item.Key)}"
            }));
            var componentCss = SerializeComponentVariables(selector, theme.GetResolvedComponentValues());
            var css = Join(request.StaticCss, tokenCss, componentCss);
            var digest = Sha256(css);
            var result = new StaticStyleResult
            {
                Css = css,
                Digest = digest,
                CacheKey = $"ssr:{theme.Key}:{theme.Algorithm}:{selector}:{digest}",
                Scope = selector,
                Algorithm = theme.Algorithm ?? "default"
            };
            if (request.IncludeStyleTag)
            {
                var nonce = string.IsNullOrWhiteSpace(request.Nonce) ? string.Empty : $" nonce=\"{EscapeAttribute(request.Nonce)}\"";
                result.StyleTag = $"<style data-ant-design-ssr=\"{digest}\"{nonce}>\n{css}</style>";
            }
            return result;
        }

        private static string SerializeComponentVariables(string selector, IReadOnlyDictionary<string, IDictionary<string, string>> components)
        {
            var tokens = new List<DesignToken>();
            foreach (var component in components.OrderBy(item => item.Key, StringComparer.Ordinal))
                foreach (var token in component.Value.OrderBy(item => item.Key, StringComparer.Ordinal))
                    tokens.Add(new DesignToken
                    {
                        Name = $"{component.Key}-{token.Key}",
                        Value = token.Value,
                        CssVariable = $"--ant-{TokenView.ToKebabCase(component.Key)}-{TokenView.ToKebabCase(token.Key)}"
                    });
            return tokens.Count == 0 ? string.Empty : TokenCssSerializer.SerializeVariables(selector, tokens);
        }

        private static string Join(params string[] parts)
        {
            return string.Join(string.Empty, parts.Where(part => !string.IsNullOrEmpty(part)).Select(part => part.EndsWith("\n", StringComparison.Ordinal) ? part : part + "\n"));
        }

        private static string EscapeAttribute(string value) => value.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");

        private static string Sha256(string value)
        {
            using var hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
