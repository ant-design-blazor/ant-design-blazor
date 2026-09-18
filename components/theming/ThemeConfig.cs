using System;
using System.Collections.Generic;

namespace AntDesign.Theming
{
    public sealed class ThemeToken
    {
        private readonly IDictionary<string, string> _values = new Dictionary<string, string>(StringComparer.Ordinal);

        public ThemeToken() { }

        public ThemeToken(IEnumerable<KeyValuePair<string, string>> values)
        {
            if (values == null) return;
            foreach (var value in values) this[value.Key] = value.Value;
        }

        public string this[string name]
        {
            get => name != null && _values.TryGetValue(name, out var value) ? value : null;
            set { if (!string.IsNullOrWhiteSpace(name)) _values[name] = value; }
        }

        public string ColorPrimary { get => this["colorPrimary"]; set => this["colorPrimary"] = value; }
        public string ColorText { get => this["colorText"]; set => this["colorText"] = value; }
        public string ColorBgContainer { get => this["colorBgContainer"]; set => this["colorBgContainer"] = value; }
        public string ColorBorder { get => this["colorBorder"]; set => this["colorBorder"] = value; }
        public string BorderRadius { get => this["borderRadius"]; set => this["borderRadius"] = value; }
        public string ControlHeight { get => this["controlHeight"]; set => this["controlHeight"] = value; }

        public IReadOnlyDictionary<string, string> Values => new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(_values);
    }

    public sealed class ThemeConfig
    {
        public string Key { get; set; } = "default";
        public string Algorithm { get; set; } = "default";
        public bool CssVar { get; set; } = true;
        public string ThemeScope { get; set; } = "ant-theme";
        public ThemeToken Token { get; set; } = new ThemeToken();
        public IDictionary<string, IDictionary<string, string>> Components { get; set; } = new Dictionary<string, IDictionary<string, string>>(StringComparer.Ordinal);
        public GeneratedComponentTokenSet ComponentTokens { get; set; } = new();
        public IDictionary<string, object> ComponentTokenValues { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public ThemeConfig SetComponentToken<TToken>(string component, TToken token)
        {
            if (string.IsNullOrWhiteSpace(component)) throw new ArgumentException("A component name is required.", nameof(component));
            if (token == null) throw new ArgumentNullException(nameof(token));
            ComponentTokenValues[component] = token;
            var property = System.Array.Find(typeof(GeneratedComponentTokenSet).GetProperties(), item => string.Equals(item.Name, component, StringComparison.OrdinalIgnoreCase));
            if (property != null && property.PropertyType.IsAssignableFrom(typeof(TToken))) property.SetValue(ComponentTokens, token);
            var values = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var tokenProperty in typeof(TToken).GetProperties())
            {
                var value = tokenProperty.GetValue(token);
                if (value != null) values[tokenProperty.Name] = value.ToString();
            }
            Components[component] = values;
            return this;
        }

        public TToken GetComponentToken<TToken>(string component) where TToken : class
        {
            return ComponentTokenValues.TryGetValue(component, out var token) ? token as TToken : null;
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
