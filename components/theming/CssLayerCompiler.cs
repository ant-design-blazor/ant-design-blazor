using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AntDesign.Theming
{
    public enum CssStyleLayer
    {
        Static,
        TokenValues,
        RuntimeOverrides
    }

    public sealed class CssLayerArtifact
    {
        public CssStyleLayer Layer { get; set; }
        public string Theme { get; set; } = "default";
        public string Scope { get; set; } = ":root";
        public string Css { get; set; } = string.Empty;
        public string Digest { get; set; } = string.Empty;
        public string CacheKey { get; set; } = string.Empty;
        public string ManagedFile { get; set; } = string.Empty;
    }

    public static class CssLayerCompiler
    {
        public static CssLayerArtifact CompileStatic(
            IEnumerable<StyleIrEntry> entries,
            string theme = "default",
            string scope = ":root",
            string managedFile = "static.css")
        {
            return CompileStyleLayer(CssStyleLayer.Static, entries, theme, scope, managedFile);
        }

        public static CssLayerArtifact CompileRuntimeOverrides(
            IEnumerable<StyleIrEntry> entries,
            string theme = "default",
            string scope = ":root",
            string managedFile = "runtime-overrides.css")
        {
            return CompileStyleLayer(CssStyleLayer.RuntimeOverrides, entries, theme, scope, managedFile);
        }

        public static CssLayerArtifact CompileTokenValues(
            IEnumerable<DesignToken> tokens,
            string theme = "default",
            string scope = ":root",
            string managedFile = "token-values.css")
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            var css = TokenCssSerializer.SerializeVariables(scope, tokens);
            return CreateArtifact(CssStyleLayer.TokenValues, theme, scope, css, managedFile);
        }

        public static string Compose(
            CssLayerArtifact staticLayer,
            CssLayerArtifact tokenValuesLayer,
            CssLayerArtifact runtimeOverridesLayer = null)
        {
            if (staticLayer == null) throw new ArgumentNullException(nameof(staticLayer));
            if (tokenValuesLayer == null) throw new ArgumentNullException(nameof(tokenValuesLayer));
            EnsureLayer(staticLayer, CssStyleLayer.Static);
            EnsureLayer(tokenValuesLayer, CssStyleLayer.TokenValues);

            var builder = new StringBuilder();
            Append(builder, staticLayer.Css);
            Append(builder, tokenValuesLayer.Css);
            if (runtimeOverridesLayer != null)
            {
                EnsureLayer(runtimeOverridesLayer, CssStyleLayer.RuntimeOverrides);
                Append(builder, runtimeOverridesLayer.Css);
            }
            return builder.ToString();
        }

        private static CssLayerArtifact CompileStyleLayer(
            CssStyleLayer layer,
            IEnumerable<StyleIrEntry> entries,
            string theme,
            string scope,
            string managedFile)
        {
            if (entries == null) throw new ArgumentNullException(nameof(entries));
            var builder = new StringBuilder();
            foreach (var entry in entries)
            {
                if (entry == null) throw new ArgumentException("Style entries cannot contain null.", nameof(entries));
                if (!string.Equals(entry.Layer, layer == CssStyleLayer.RuntimeOverrides ? "runtime-overrides" : "static", StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Entry '{entry.Component}' does not belong to the {layer} layer.", nameof(entries));
                }
                if (layer == CssStyleLayer.Static && entry.Declarations.Any(x => x.Property.StartsWith("--ant-", StringComparison.Ordinal)))
                {
                    throw new ArgumentException("Static CSS cannot contain Design Token variable declarations.", nameof(entries));
                }
                AppendStyleEntry(builder, entry);
            }
            return CreateArtifact(layer, theme, scope, builder.ToString(), managedFile);
        }

        private static void AppendStyleEntry(StringBuilder builder, StyleIrEntry entry)
        {
            var css = new StringBuilder();
            css.Append(entry.Selector).Append(" {\n");
            foreach (var declaration in entry.Declarations)
            {
                css.Append("  ").Append(declaration.Property).Append(": ").Append(declaration.Value).Append(";\n");
            }
            css.Append("}\n");
            var text = css.ToString();
            foreach (var atRule in entry.AtRules.AsEnumerable().Reverse())
            {
                text = atRule + " {\n" + Indent(text) + "}\n";
            }
            builder.Append(text);
        }

        private static string Indent(string value)
        {
            return string.Join("\n", value.TrimEnd('\n').Split('\n').Select(line => "  " + line)) + "\n";
        }

        private static CssLayerArtifact CreateArtifact(CssStyleLayer layer, string theme, string scope, string css, string managedFile)
        {
            var digest = Sha256(css);
            return new CssLayerArtifact
            {
                Layer = layer,
                Theme = theme ?? "default",
                Scope = scope ?? ":root",
                Css = css,
                Digest = digest,
                CacheKey = $"{layer.ToString().ToLowerInvariant()}:{theme ?? "default"}:{scope ?? ":root"}:{digest}",
                ManagedFile = managedFile ?? string.Empty
            };
        }

        private static string Sha256(string value)
        {
            using var hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(value)))
                .Replace("-", string.Empty)
                .ToLowerInvariant();
        }

        private static void Append(StringBuilder builder, string css)
        {
            if (string.IsNullOrEmpty(css)) return;
            builder.Append(css);
            if (!css.EndsWith("\n", StringComparison.Ordinal)) builder.Append('\n');
        }

        private static void EnsureLayer(CssLayerArtifact artifact, CssStyleLayer expected)
        {
            if (artifact.Layer != expected) throw new ArgumentException($"Expected {expected} layer.");
        }
    }
}
