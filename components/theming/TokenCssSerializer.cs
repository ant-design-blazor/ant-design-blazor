using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AntDesign.Theming
{
    public static class TokenCssSerializer
    {
        public static string SerializeVariables(string selector, IEnumerable<DesignToken> tokens)
        {
            if (string.IsNullOrWhiteSpace(selector)) throw new ArgumentException("A CSS selector is required.", nameof(selector));
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));

            var ordered = tokens
                .OrderBy(token => token.CssVariable, StringComparer.Ordinal)
                .ToList();
            var builder = new StringBuilder();
            builder.Append(selector).Append(" {\n");
            foreach (var token in ordered)
            {
                builder.Append("  ")
                    .Append(token.CssVariable)
                    .Append(": ")
                    .Append(SerializeValue(token.Value))
                    .Append(";\n");
            }
            builder.Append("}\n");
            return builder.ToString();
        }

        private static string SerializeValue(string value)
        {
            return value ?? string.Empty;
        }

        public static string SerializeValue(double value)
        {
            return value.ToString("0.####################", CultureInfo.InvariantCulture);
        }
    }
}
