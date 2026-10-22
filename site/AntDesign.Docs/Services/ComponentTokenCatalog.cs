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
        var key = (component ?? string.Empty).Replace("-", "").ToLowerInvariant();
        return GeneratedComponentGlobalTokens.All.TryGetValue(key, out var names)
            ? names.Where(GlobalTokens.ContainsKey).Select(name => GlobalTokens[name]).ToArray()
            : Array.Empty<GlobalTokenDefinition>();
    }

    public static IReadOnlyList<ComponentTokenDefinition> Get(string component)
    {
        if (string.IsNullOrWhiteSpace(component)) return Array.Empty<ComponentTokenDefinition>();

        var normalizedComponent = string.Equals(component, "qrcode", StringComparison.OrdinalIgnoreCase)
            ? "qr-code"
            : component;

        return GeneratedComponentTokens.All
            .Where(token => token.Documented && !token.Reserved && string.Equals(token.Component, normalizedComponent, StringComparison.OrdinalIgnoreCase))
            .OrderBy(token => token.Name, StringComparer.Ordinal)
            .ToArray();
    }
}
