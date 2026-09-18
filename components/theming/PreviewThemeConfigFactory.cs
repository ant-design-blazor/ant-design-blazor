using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign.Theming
{
    public static class PreviewThemeConfigFactory
    {
        public static ThemeConfig Create(string key)
        {
            var definition = GeneratedPreviewThemes.All.FirstOrDefault(theme => string.Equals(theme.Key, key, StringComparison.OrdinalIgnoreCase));
            if (definition == null) throw new KeyNotFoundException($"Unknown preview theme: {key}");
            return Create(definition);
        }

        public static ThemeConfig Create(PreviewThemeDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            var config = new ThemeConfig
            {
                Key = definition.Key,
                Algorithm = definition.Algorithm,
                Token = new ThemeToken(definition.Tokens)
            };
            foreach (var component in definition.Components)
                config.Components[component.Key] = new Dictionary<string, string>(component.Value, StringComparer.Ordinal);
            return config;
        }
    }
}
