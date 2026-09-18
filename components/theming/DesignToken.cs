using System;
using System.Collections.Generic;

namespace AntDesign.Theming
{
    public sealed class GlobalToken
    {
        public IDictionary<string, string> Values { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    public enum DesignTokenKind
    {
        Seed,
        Alias,
        Component,
        Semantic
    }

    public class DesignToken
    {
        public string Name { get; set; }
        public DesignTokenKind Kind { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string CssVariable { get; set; }
        public string Component { get; set; }
        public string Source { get; set; }

        public DesignToken()
        {
            Name = string.Empty;
            Type = string.Empty;
            Value = string.Empty;
            CssVariable = string.Empty;
            Source = string.Empty;
        }
    }

    public sealed class SeedToken : DesignToken { public SeedToken() { Kind = DesignTokenKind.Seed; } }
    public sealed class AliasToken : DesignToken { public AliasToken() { Kind = DesignTokenKind.Alias; } }
    public sealed class ComponentToken : DesignToken { public ComponentToken() { Kind = DesignTokenKind.Component; } }
    public sealed class SemanticToken : DesignToken { public SemanticToken() { Kind = DesignTokenKind.Semantic; } }

    public sealed class ResolvedToken
    {
        public string Name { get; }
        public string Value { get; }
        public string Source { get; }

        public ResolvedToken(string name, string value, string source = "")
        {
            Name = name ?? string.Empty;
            Value = value ?? string.Empty;
            Source = source ?? string.Empty;
        }
    }

    public sealed class CssVarToken
    {
        public string Name { get; }
        public string Variable { get; }
        public string Reference { get; }

        public CssVarToken(string name, string variable)
        {
            Name = name ?? string.Empty;
            Variable = variable ?? string.Empty;
            Reference = $"var({Variable})";
        }
    }

    public sealed class StyleDeclaration
    {
        public string Property { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public sealed class StyleIrEntry
    {
        public string Component { get; set; } = string.Empty;
        public string Layer { get; set; } = "static";
        public string Selector { get; set; } = string.Empty;
        public System.Collections.Generic.List<StyleDeclaration> Declarations { get; set; } = new();
        public System.Collections.Generic.List<string> AtRules { get; set; } = new();
        public string Source { get; set; } = string.Empty;
    }
}
