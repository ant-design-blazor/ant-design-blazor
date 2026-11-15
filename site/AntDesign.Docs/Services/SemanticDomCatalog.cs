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

    public static IReadOnlyList<SemanticDomNode> Get(string component) => Get(component, "zh-CN");

    public static IReadOnlyList<SemanticDomNode> Get(string component, string? language)
    {
        var normalizedComponent = string.Equals(component, "qrcode", StringComparison.OrdinalIgnoreCase)
            ? "qr-code"
            : component ?? string.Empty;
        if (!SemanticDomGeneratedCatalog.Nodes.TryGetValue(normalizedComponent, out var nodes))
        {
            return Array.Empty<SemanticDomNode>();
        }

        if (language?.StartsWith("en", StringComparison.OrdinalIgnoreCase) != true)
        {
            return nodes;
        }

        return nodes.Select(node => SemanticDomGeneratedCatalog.EnglishDescriptions.TryGetValue($"{normalizedComponent}.{node.Name}", out var description)
            ? node with { Description = description }
            : node).ToArray();
    }
}
