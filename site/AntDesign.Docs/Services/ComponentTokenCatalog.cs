using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.Theming;

namespace AntDesign.Docs.Services;

public static class ComponentTokenCatalog
{
    private static readonly IReadOnlyDictionary<string, GlobalTokenDefinition> GlobalTokens = GeneratedGlobalTokens.All
        .ToDictionary(token => token.Name, StringComparer.Ordinal);

    public static IReadOnlyList<GlobalTokenDefinition> GetGlobal(string component)
    {
        var key = NormalizeComponentKey(component);
        return GeneratedComponentGlobalTokens.All.TryGetValue(key, out var names)
            ? names.Where(GlobalTokens.ContainsKey).Select(name => GlobalTokens[name]).ToArray()
            : Array.Empty<GlobalTokenDefinition>();
    }

    public static IReadOnlyList<ComponentTokenDefinition> Get(string component)
    {
        if (string.IsNullOrWhiteSpace(component)) return Array.Empty<ComponentTokenDefinition>();

        var normalizedName = component.Replace("-", string.Empty).ToLowerInvariant();
        var normalizedComponent = normalizedName switch
        {
            "qrcode" => "qr-code",
            "autocomplete" => "select",
            "timepicker" => "date-picker",
            _ => component
        };

        return GeneratedComponentTokens.All
            .Where(token => token.Documented && !token.Reserved && string.Equals(token.Component, normalizedComponent, StringComparison.OrdinalIgnoreCase))
            .OrderBy(token => token.Name, StringComparer.Ordinal)
            .ToArray();
    }

    private static string NormalizeComponentKey(string component)
    {
        var key = (component ?? string.Empty).Replace("-", "").ToLowerInvariant();
        return key switch
        {
            "autocomplete" => "select",
            "timepicker" => "datepicker",
            _ => key
        };
    }
}
