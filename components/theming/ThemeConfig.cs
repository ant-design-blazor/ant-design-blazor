using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
        public GeneratedSeedToken Seed { get; set; } = new GeneratedSeedToken();
        public GeneratedMapToken Map { get; set; } = new GeneratedMapToken();
        public GeneratedAliasToken Alias { get; set; } = new GeneratedAliasToken();
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

        public IReadOnlyDictionary<string, string> GetResolvedTokenValues()
        {
            var seed = ReadObject(Seed);
            var algorithm = ThemeAlgorithmFactory.Create(Algorithm);
            var resolved = algorithm.Resolve(seed.Select(item => new SeedToken { Name = item.Key, Value = item.Value }));
            var values = resolved.ToDictionary(item => item.Name, item => item.Value, StringComparer.Ordinal);
            MergeObject(values, Map, includeNestedMaps: true);
            MergeObject(values, Alias, includeNestedMaps: false);
            foreach (var pair in Token.Values) values[pair.Key] = pair.Value;
            return values;
        }

        public IReadOnlyDictionary<string, IDictionary<string, string>> GetResolvedComponentValues()
        {
            var result = new Dictionary<string, IDictionary<string, string>>(Components, StringComparer.OrdinalIgnoreCase);
            foreach (var property in typeof(GeneratedComponentTokenSet).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var token = property.GetValue(ComponentTokens);
                if (token == null) continue;
                var values = ReadObject(token);
                if (values.Count > 0) result[property.Name] = values;
            }
            return result;
        }

        private static Dictionary<string, string> ReadObject(object value)
        {
            var result = new Dictionary<string, string>(StringComparer.Ordinal);
            if (value == null) return result;
            foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!property.CanRead || property.PropertyType.IsClass && property.PropertyType != typeof(string)) continue;
                var item = property.GetValue(value);
                var serialized = SerializeScalar(item);
                if (serialized != null) result[ToCamelCase(property.Name)] = serialized;
            }
            return result;
        }

        private static void MergeObject(IDictionary<string, string> target, object value, bool includeNestedMaps)
        {
            foreach (var pair in ReadObject(value)) target[pair.Key] = pair.Value;
            if (!includeNestedMaps || value == null) return;
            foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var nested = property.GetValue(value);
                if (nested is System.Collections.IDictionary dictionary)
                    foreach (System.Collections.DictionaryEntry item in dictionary)
                        if (item.Key != null && item.Value != null) target[item.Key.ToString()] = item.Value.ToString();
            }
        }

        private static string SerializeScalar(object value)
        {
            if (value == null) return null;
            if (value is string text) return string.IsNullOrWhiteSpace(text) ? null : text;
            if (value is bool boolean) return boolean ? "true" : null;
            if (value is double number) return number == 0 ? null : number.ToString("0.####################", CultureInfo.InvariantCulture);
            if (value is float single) return single == 0 ? null : single.ToString("0.####################", CultureInfo.InvariantCulture);
            if (value is int integer) return integer == 0 ? null : integer.ToString(CultureInfo.InvariantCulture);
            return value.ToString();
        }

        private static string ToCamelCase(string value) => string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value.Substring(1);
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
