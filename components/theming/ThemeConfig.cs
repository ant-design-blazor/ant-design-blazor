using System;
using System.Collections.Generic;

namespace AntDesign.Theming
{
    public sealed class ThemeConfig
    {
        public string Key { get; set; } = "default";
        public string Algorithm { get; set; } = "default";
        public bool CssVar { get; set; } = true;
        public string ThemeScope { get; set; } = "ant-theme";
        public IDictionary<string, string> Token { get; set; } = new Dictionary<string, string>(StringComparer.Ordinal);
        public IDictionary<string, IDictionary<string, string>> Components { get; set; } = new Dictionary<string, IDictionary<string, string>>(StringComparer.Ordinal);
        public IDictionary<string, object> ComponentTokens { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public ThemeConfig SetComponentToken<TToken>(string component, TToken token)
        {
            if (string.IsNullOrWhiteSpace(component)) throw new ArgumentException("A component name is required.", nameof(component));
            if (token == null) throw new ArgumentNullException(nameof(token));
            ComponentTokens[component] = token;
            var values = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var property in typeof(TToken).GetProperties())
            {
                var value = property.GetValue(token);
                if (value != null) values[property.Name] = value.ToString();
            }
            Components[component] = values;
            return this;
        }

        public TToken GetComponentToken<TToken>(string component) where TToken : class
        {
            return ComponentTokens.TryGetValue(component, out var token) ? token as TToken : null;
        }
    }

    public sealed class TokenView
    {
        private readonly IReadOnlyDictionary<string, string> _values;

        internal TokenView(IReadOnlyDictionary<string, string> values)
        {
            _values = values;
        }

        public IReadOnlyDictionary<string, string> Values => _values;

        public string Get(string name, string fallback = null)
        {
            return _values.TryGetValue(name, out var value) ? value : fallback;
        }

        public string CssVar(string name)
        {
            return $"var(--ant-{ToKebabCase(name)})";
        }

        public static string ToKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var result = new System.Text.StringBuilder();
            for (var index = 0; index < value.Length; index++)
            {
                var character = value[index];
                if (char.IsUpper(character) && index > 0) result.Append('-');
                result.Append(char.ToLowerInvariant(character));
            }
            return result.ToString();
        }
    }
}
