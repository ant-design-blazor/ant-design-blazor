using System;
using System.Collections.Generic;
using System.Globalization;

namespace AntDesign.Theming
{
    /// <summary>
    /// Formats runtime theme values using the same numeric CSS variable rules as
    /// @ant-design/cssinjs: dimensional numbers receive px, while unitless
    /// tokens remain numbers.
    /// </summary>
    public static class CssVariableValueFormatter
    {
        private static readonly HashSet<string> Unitless = new(StringComparer.Ordinal)
        {
            "lineHeight",
            "lineHeightSM",
            "lineHeightLG",
            "lineHeightHeading1",
            "lineHeightHeading2",
            "lineHeightHeading3",
            "lineHeightHeading4",
            "lineHeightHeading5",
            "opacityLoading",
            "fontWeightStrong",
            "zIndexPopupBase",
            "zIndexBase",
            "opacityImage",
        };

        public static string Format(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
                return value;

            // A component override can contain both component and global tokens.
            // Keep the original token's unit semantics after prefixing its name.
            var tokenName = name.Substring(name.LastIndexOf('-') + 1);
            if (number == 0 || Unitless.Contains(name) || Unitless.Contains(tokenName)
                || tokenName.StartsWith("lineHeight", StringComparison.Ordinal)
                || tokenName.StartsWith("contentLineHeight", StringComparison.Ordinal)
                || tokenName.StartsWith("fontWeight", StringComparison.Ordinal)
                || tokenName.StartsWith("zIndex", StringComparison.Ordinal)
                || tokenName.StartsWith("opacity", StringComparison.Ordinal))
                return value;

            return $"{value}px";
        }
    }
}
