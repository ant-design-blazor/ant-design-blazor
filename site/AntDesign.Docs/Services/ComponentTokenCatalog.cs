using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.Theming;

namespace AntDesign.Docs.Services;

public static class ComponentTokenCatalog
{
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
