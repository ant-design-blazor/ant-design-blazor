using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class CssLayerCompilerTests
    {
        [Fact]
        public void KeepsStaticAndTokenValuesInSeparateArtifacts()
        {
            var staticLayer = CssLayerCompiler.CompileStatic(new[]
            {
                new StyleIrEntry
                {
                    Component = "Button",
                    Selector = ".ant-btn",
                    Declarations = { new StyleDeclaration { Property = "color", Value = "var(--ant-color-text)" } }
                }
            });
            var tokenLayer = CssLayerCompiler.CompileTokenValues(new[]
            {
                new DesignToken { CssVariable = "--ant-color-text", Value = "rgba(0, 0, 0, 0.88)" }
            });

            Assert.DoesNotContain("--ant-color-text:", staticLayer.Css);
            Assert.Contains("var(--ant-color-text)", staticLayer.Css);
            Assert.Contains("--ant-color-text: rgba(0, 0, 0, 0.88);", tokenLayer.Css);
            Assert.NotEqual(staticLayer.Digest, tokenLayer.Digest);
            Assert.Contains("static:default", staticLayer.CacheKey);
            Assert.Contains("tokenvalues:default", tokenLayer.CacheKey);
        }

        [Fact]
        public void ComposesLayersInStaticTokenRuntimeOrder()
        {
            var result = CssLayerCompiler.Compose(
                CssLayerCompiler.CompileStatic(new[] { Entry(".static", "a", "static") }),
                CssLayerCompiler.CompileTokenValues(new[] { new DesignToken { CssVariable = "--ant-a", Value = "1" } }),
                CssLayerCompiler.CompileRuntimeOverrides(new[] { Entry(".runtime", "b", "runtime-overrides") }));

            Assert.True(result.IndexOf(".static", System.StringComparison.Ordinal) < result.IndexOf("--ant-a", System.StringComparison.Ordinal));
            Assert.True(result.IndexOf("--ant-a", System.StringComparison.Ordinal) < result.IndexOf(".runtime", System.StringComparison.Ordinal));
        }

        [Fact]
        public void RejectsTokenDeclarationsInStaticLayer()
        {
            Assert.Throws<System.ArgumentException>(() => CssLayerCompiler.CompileStatic(new[]
            {
                new StyleIrEntry { Component = "Button", Selector = ".ant-btn", Declarations = { new StyleDeclaration { Property = "--ant-color-text", Value = "red" } } }
            }));
        }

        private static StyleIrEntry Entry(string selector, string value, string layer)
        {
            return new StyleIrEntry
            {
                Layer = layer,
                Selector = selector,
                Declarations = { new StyleDeclaration { Property = "content", Value = value } }
            };
        }
    }
}
