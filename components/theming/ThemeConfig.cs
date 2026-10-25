using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AntDesign.Theming
{
    public sealed partial class ThemeToken
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

        public IReadOnlyDictionary<string, string> Values => new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(_values);
    }

    public sealed class ThemeConfig
    {
        public string Key { get; set; } = "default";
        private string _algorithm;
        public string Algorithm { get => _algorithm ?? "default"; set => _algorithm = value; }
        internal bool HasExplicitAlgorithm => _algorithm != null;
        public bool Inherit { get; set; } = true;
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
                if (value == null || value is string text && string.IsNullOrWhiteSpace(text)) continue;
                if (value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal
                    && Convert.ToDouble(value, CultureInfo.InvariantCulture) == 0)
                    continue;
                values[tokenProperty.Name] = value.ToString();
            }
            Components[component] = values;
            return this;
        }

        public TToken GetComponentToken<TToken>(string component) where TToken : class
        {
            return ComponentTokenValues.TryGetValue(component, out var token) ? token as TToken : null;
        }

        public IReadOnlyDictionary<string, string> GetExplicitTokenValues()
        {
            var values = ReadObject(Seed);
            MergeObject(values, Map, includeNestedMaps: true);
            MergeObject(values, Alias, includeNestedMaps: false);
            foreach (var pair in Token.Values) values[pair.Key] = pair.Value;
            return values;
        }

        public IReadOnlyDictionary<string, string> GetResolvedTokenValues(IReadOnlyDictionary<string, string> inherited = null, string inheritedAlgorithm = null)
        {
            var seed = !Inherit || inherited == null
                ? new Dictionary<string, string>(StringComparer.Ordinal)
                : new Dictionary<string, string>(inherited, StringComparer.Ordinal);
            foreach (var pair in GetExplicitTokenValues()) seed[pair.Key] = pair.Value;
            var algorithm = ThemeAlgorithmFactory.Create(HasExplicitAlgorithm ? Algorithm : (Inherit ? inheritedAlgorithm : null) ?? Algorithm);
            var resolved = algorithm.Resolve(seed.Select(item => new SeedToken { Name = item.Key, Value = item.Value }));
            var values = resolved.ToDictionary(item => item.Name, item => item.Value, StringComparer.Ordinal);
            var overrides = seed.Where(pair => !GeneratedSeedTokenDefaults.All.ContainsKey(pair.Key)).ToArray();
            // formatToken merges map overrides before deriving aliases, then
            // reapplies explicit aliases. Otherwise colorText changes would not
            // reach colorTextHeading, and sizeSM would not reach paddingSM.
            foreach (var pair in overrides) values[pair.Key] = pair.Value;
            DefaultThemeAlgorithm.AddAliasMap(values);
            DefaultThemeAlgorithm.AddResponsiveAndShadowMap(values);
            // React reapplies map/alias overrides after derivation, but seed
            // inputs must retain the algorithm's result (e.g. dark primary or
            // compact controlHeight/fontSize).
            foreach (var pair in overrides) values[pair.Key] = pair.Value;
            return values;
        }

        public IReadOnlyDictionary<string, IDictionary<string, string>> GetResolvedComponentValues(IReadOnlyDictionary<string, IDictionary<string, string>> inherited = null,
            IReadOnlyDictionary<string, string> inheritedTokens = null, string inheritedAlgorithm = null)
        {
            var result = new Dictionary<string, IDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            if (Inherit && inherited != null)
                foreach (var item in inherited)
                    result[item.Key] = new Dictionary<string, string>(item.Value, StringComparer.Ordinal);
            foreach (var component in Components)
            {
                if (!result.TryGetValue(component.Key, out var values)) result[component.Key] = values = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (var value in component.Value) values[value.Key] = value.Value;
            }
            foreach (var property in typeof(GeneratedComponentTokenSet).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var token = property.GetValue(ComponentTokens);
                if (token == null) continue;
                var values = ReadObject(token);
                if (values.Count > 0)
                {
                    if (!result.TryGetValue(property.Name, out var componentValues)) result[property.Name] = componentValues = new Dictionary<string, string>(StringComparer.Ordinal);
                    foreach (var value in values) componentValues[value.Key] = value.Value;
                }
            }
            ResolveInputComponentValues(result, GetResolvedTokenValues(inheritedTokens, inheritedAlgorithm));
            return result;
        }

        private static void ResolveInputComponentValues(IDictionary<string, IDictionary<string, string>> components, IReadOnlyDictionary<string, string> tokens)
        {
            // Input's component tokens are derived from global design tokens in
            // Ant Design. Resolve them here too, so a ConfigProvider override of
            // controlHeight/fontSize affects the actual input CSS variables.
            var input = components.TryGetValue("Input", out var configured)
                ? new Dictionary<string, string>(configured, StringComparer.Ordinal)
                : new Dictionary<string, string>(StringComparer.Ordinal);
            components["Input"] = input;

            string Token(string name, string fallback = null) => tokens.TryGetValue(name, out var value) && !string.IsNullOrWhiteSpace(value) ? value : fallback;
            double Number(string name, double fallback = 0) => double.TryParse(Token(name), NumberStyles.Float, CultureInfo.InvariantCulture, out var value) && value != 0 ? value : fallback;
            double Value(string name, double fallback) => input.TryGetValue(name, out var value) && double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) && parsed != 0 ? parsed : fallback;
            string Format(double value) => value.ToString("0.####################", CultureInfo.InvariantCulture);
            double Round(double value) => Math.Round(value, 1, MidpointRounding.AwayFromZero);

            var fontSize = Value("inputFontSize", Number("fontSize", 14));
            var fontSizeSm = Value("inputFontSizeSM", fontSize);
            var fontSizeLg = Value("inputFontSizeLG", Number("fontSizeLG", 16));
            var lineHeight = Number("lineHeight", 1.5714285714285714);
            var lineHeightLg = Number("lineHeightLG", 1.5);
            var lineWidth = Number("lineWidth", 1);
            var controlHeight = Number("controlHeight", 32);
            var controlHeightSm = Number("controlHeightSM", 24);
            var controlHeightLg = Number("controlHeightLG", 40);

            void SetResolved(string name, string value)
            {
                input[name] = value;
            }

            var customBaseFont = Number("fontSize", 14) != 14 || input.ContainsKey("inputFontSize");
            var customSmallFont = Number("fontSizeSM", 12) != 12 || input.ContainsKey("inputFontSizeSM");
            var customLargeFont = Number("fontSizeLG", 16) != 16 || input.ContainsKey("inputFontSizeLG");
            if (customBaseFont) SetResolved("inputFontSize", Format(fontSize));
            if (customSmallFont) SetResolved("inputFontSizeSM", Format(fontSizeSm));
            if (customLargeFont) SetResolved("inputFontSizeLG", Format(fontSizeLg));

            if (Number("controlHeight", 32) != 32 || customBaseFont || input.ContainsKey("paddingBlock"))
                SetResolved("paddingBlock", Format(Math.Max(Round((controlHeight - fontSize * lineHeight) / 2) - lineWidth, 0)));
            if (Number("controlHeightSM", 24) != 24 || customSmallFont || input.ContainsKey("paddingBlockSM"))
                SetResolved("paddingBlockSM", Format(Math.Max(Round((controlHeightSm - fontSizeSm * lineHeight) / 2) - lineWidth, 0)));
            if (Number("controlHeightLG", 40) != 40 || customLargeFont || input.ContainsKey("paddingBlockLG"))
                SetResolved("paddingBlockLG", Format(Math.Max(Math.Ceiling(((controlHeightLg - fontSizeLg * lineHeightLg) / 2) * 10) / 10 - lineWidth, 0)));
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
            // Unwrap the union before serialization. Its diagnostic ToString()
            // ("System.String: ") is not a CSS value and must not overwrite an
            // explicitly configured dynamic token with an unset typed default.
            if (value is OneOf.IOneOf union) return SerializeScalar(union.Value);
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

            // Keep acronym suffixes such as LG/SM/XXS together. React's v6
            // token names use `control-height-lg`, not `control-height-l-g`.
            var result = System.Text.RegularExpressions.Regex.Replace(value, "([A-Z]+)([A-Z][a-z])", "$1-$2");
            result = System.Text.RegularExpressions.Regex.Replace(result, "([a-z0-9])([A-Z])", "$1-$2");
            return result.ToLowerInvariant();
        }
    }
}
