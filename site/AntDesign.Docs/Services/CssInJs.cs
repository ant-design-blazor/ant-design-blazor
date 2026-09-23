using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AntDesign.Docs.Services;

/// <summary>A small object-to-CSS class generator for documentation components.</summary>
public static class CssInJs
{
    public static CssStyleSheet CreateStyles(string componentName, object definitions)
    {
        if (string.IsNullOrWhiteSpace(componentName)) throw new ArgumentException("A component name is required.", nameof(componentName));
        if (definitions == null) throw new ArgumentNullException(nameof(definitions));

        var classes = new Dictionary<string, string>(StringComparer.Ordinal);
        var css = new StringBuilder();
        var prefix = ToIdentifier(componentName);

        foreach (var (name, rule) in Properties(definitions))
        {
            var className = $"adb-{prefix}-{ToIdentifier(name)}";
            classes.Add(name, className);
            AppendRule(css, $".{className}", rule);
        }

        return new CssStyleSheet(classes, css.ToString());
    }

    private static void AppendRule(StringBuilder css, string selector, object rule)
    {
        var declarations = new List<(string Name, string Value)>();
        var nested = new List<(string Key, object Value)>();
        foreach (var (name, value) in Properties(rule))
        {
            if (value == null) continue;
            if (IsScalar(value)) declarations.Add((ToKebabCase(name), ToCssValue(name, value)));
            else nested.Add((name, value));
        }

        if (declarations.Count > 0)
        {
            css.Append(selector).Append('{');
            foreach (var declaration in declarations)
                css.Append(declaration.Name).Append(':').Append(declaration.Value).Append(';');
            css.Append('}');
        }

        foreach (var (key, value) in nested)
        {
            if (key.StartsWith('@'))
            {
                var inner = new StringBuilder();
                AppendRule(inner, selector, value);
                css.Append(key).Append('{').Append(inner).Append('}');
            }
            else
            {
                var nestedSelector = key.Contains('&', StringComparison.Ordinal)
                    ? key.Replace("&", selector, StringComparison.Ordinal)
                    : $"{selector} {key}";
                AppendRule(css, nestedSelector, value);
            }
        }
    }

    private static IEnumerable<(string Name, object Value)> Properties(object value)
    {
        if (value is IDictionary<string, object> objectDictionary)
            return objectDictionary.Select(item => (item.Key, item.Value));
        if (value is IDictionary<string, string> stringDictionary)
            return stringDictionary.Select(item => (item.Key, (object)item.Value));

        return value.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetIndexParameters().Length == 0)
            .Select(property => (property.Name, property.GetValue(value)!));
    }

    private static bool IsScalar(object value) => value is string || value.GetType().IsPrimitive || value is decimal;

    private static string ToCssValue(string propertyName, object value)
    {
        if (value is not IConvertible convertible || value is string) return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        var text = convertible.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        if (value is bool) return text.ToLowerInvariant();
        if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number) && number != 0 && !UnitlessProperties.Contains(propertyName))
            return $"{text}px";
        return text;
    }

    private static string ToKebabCase(string value)
    {
        var result = new StringBuilder(value.Length + 4);
        foreach (var character in value)
        {
            if (char.IsUpper(character)) result.Append('-').Append(char.ToLowerInvariant(character));
            else result.Append(character);
        }
        return result.ToString();
    }

    private static string ToIdentifier(string value)
    {
        var result = new StringBuilder(value.Length);
        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (char.IsUpper(character) && index > 0 && (char.IsLower(value[index - 1]) || char.IsDigit(value[index - 1])) && result.Length > 0 && result[result.Length - 1] != '-')
                result.Append('-');
            if (char.IsLetterOrDigit(character) || character == '-') result.Append(char.ToLowerInvariant(character));
            else if (result.Length > 0 && result[result.Length - 1] != '-') result.Append('-');
        }
        return result.ToString().Trim('-');
    }

    private static readonly HashSet<string> UnitlessProperties = new(StringComparer.Ordinal)
    {
        "animationIterationCount", "borderImageOutset", "borderImageSlice", "borderImageWidth", "boxFlex", "columnCount",
        "fillOpacity", "flex", "flexGrow", "flexShrink", "fontWeight", "gridArea", "gridColumn", "gridColumnEnd",
        "gridColumnStart", "gridRow", "gridRowEnd", "gridRowStart", "lineHeight", "opacity", "order", "orphans",
        "scale", "strokeDasharray", "strokeDashoffset", "strokeMiterlimit", "strokeOpacity", "strokeWidth", "tabSize",
        "widows", "zIndex", "zoom"
    };
}

public sealed class CssStyleSheet
{
    internal CssStyleSheet(IReadOnlyDictionary<string, string> classes, string cssText)
    {
        Classes = classes;
        CssText = cssText;
    }

    public IReadOnlyDictionary<string, string> Classes { get; }
    public string CssText { get; }
    public string this[string name] => Classes[name];
}
