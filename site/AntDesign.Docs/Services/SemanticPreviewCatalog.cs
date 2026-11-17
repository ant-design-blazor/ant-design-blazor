using AntDesign.Docs.Components;
using System.Collections.Generic;
using System;

namespace AntDesign.Docs.Services;

public static class SemanticPreviewCatalog
{
    public static IReadOnlyList<SemanticPreviewRegistration> Get(string component)
    {
        var key = string.Equals(component, "qrcode", StringComparison.OrdinalIgnoreCase)
            ? "qr-code"
            : component ?? string.Empty;

        return GeneratedDemoIndex.SemanticPreviews.TryGetValue(key, out var previews)
            ? previews
            : Array.Empty<SemanticPreviewRegistration>();
    }
}
