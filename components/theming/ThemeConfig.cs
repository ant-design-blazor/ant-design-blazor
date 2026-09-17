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
