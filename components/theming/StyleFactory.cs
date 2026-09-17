using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AntDesign.Theming
{
    public interface IThemeContext
    {
        ThemeConfig Config { get; }
        TokenView Token { get; }
        string ScopeSelector { get; }
    }

    public sealed class ThemeContext : IThemeContext
    {
        public ThemeConfig Config { get; }
        public TokenView Token { get; }
        public string ScopeSelector => $":where(.{Config.ThemeScope}-{Config.Key})";

        public ThemeContext(ThemeConfig config, IThemeContext parent = null)
        {
            Config = config ?? new ThemeConfig();
            var values = new Dictionary<string, string>(StringComparer.Ordinal);
            if (parent != null)
            {
                foreach (var item in parent.Token.Values) values[item.Key] = item.Value;
            }
            foreach (var item in Config.Token) values[item.Key] = item.Value;
            Token = new TokenView(values);
        }
    }

    public sealed class StyleContext
    {
        public IThemeContext Theme { get; }
        public TokenView Token => Theme.Token;
        public string CssVar(string name) => Token.CssVar(name);

        public StyleContext(IThemeContext theme)
        {
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
        }
    }

    public sealed class StyleDefinition
    {
        public string Selector { get; set; } = string.Empty;
        public IList<StyleDeclaration> Declarations { get; } = new List<StyleDeclaration>();
        public IList<string> AtRules { get; } = new List<string>();
    }

    public sealed class StyleResult
    {
        public string ClassName { get; internal set; } = string.Empty;
        public string Css { get; internal set; } = string.Empty;
        public string Digest { get; internal set; } = string.Empty;
        public string CacheKey { get; internal set; } = string.Empty;
    }

    public interface IStyleFactory
    {
        StyleResult Create(string component, string name, IThemeContext theme, Func<StyleContext, StyleDefinition> definition);
        string Cx(params string[] classNames);
    }

    public sealed class StyleFactory : IStyleFactory
    {
        private readonly IDictionary<string, StyleResult> _cache = new Dictionary<string, StyleResult>(StringComparer.Ordinal);

        public StyleResult Create(string component, string name, IThemeContext theme, Func<StyleContext, StyleDefinition> definition)
        {
            if (string.IsNullOrWhiteSpace(component)) throw new ArgumentException("Component is required.", nameof(component));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Style name is required.", nameof(name));
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            var context = new StyleContext(theme);
            var style = definition(context) ?? throw new InvalidOperationException("Style definition cannot be null.");
            var source = string.Join("|", component, name, style.Selector, string.Join(";", style.Declarations.Select(x => $"{x.Property}:{x.Value}")), string.Join(";", style.AtRules), theme.ScopeSelector);
            var digest = Sha256(source);
            if (_cache.TryGetValue(digest, out var cached)) return cached;
            var result = new StyleResult
            {
                ClassName = $"{component.ToLowerInvariant()}-{name.ToLowerInvariant()}-{digest.Substring(0, 8)}",
                Css = Serialize(style),
                Digest = digest,
                CacheKey = $"{component}:{name}:{theme.Config.Key}:{digest}"
            };
            _cache[digest] = result;
            return result;
        }

        public string Cx(params string[] classNames)
        {
            return string.Join(" ", (classNames ?? Array.Empty<string>())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .SelectMany(value => value.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                .Distinct(StringComparer.Ordinal));
        }

        private static string Serialize(StyleDefinition style)
        {
            var builder = new StringBuilder();
            builder.Append(style.Selector).Append(" {\n");
            foreach (var declaration in style.Declarations) builder.Append("  ").Append(declaration.Property).Append(": ").Append(declaration.Value).Append(";\n");
            builder.Append("}\n");
            return builder.ToString();
        }

        private static string Sha256(string value)
        {
            using var hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
