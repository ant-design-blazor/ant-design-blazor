using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign.Docs.Services;

public sealed record SemanticDomNode(
    string Name,
    string Selector,
    string Description,
    string? Version = null);

public static class SemanticDomCatalog
{
    public static IReadOnlyCollection<string> Components => SemanticDomGeneratedCatalog.Nodes.Keys.ToArray();

    public static IReadOnlyList<SemanticDomNode> Get(string component)
    {
        var normalizedComponent = string.Equals(component, "qrcode", StringComparison.OrdinalIgnoreCase)
            ? "qr-code"
            : component ?? string.Empty;
        return SemanticDomGeneratedCatalog.Nodes.TryGetValue(normalizedComponent, out var nodes)
            ? nodes
            : Array.Empty<SemanticDomNode>();
    }
}
