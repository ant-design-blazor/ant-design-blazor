using System.Collections.Generic;
using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class TokenCompilerTests
    {
        [Fact]
        public void CompilesTypedTokensIntoResolvedAndCssVariableViews()
        {
            var result = TokenCompiler.Compile(new List<DesignToken>
            {
                new ComponentToken { Name = "primaryShadow", Value = "0 2px 0 rgba(5, 145, 255, 0.1)", Source = "button/token.ts" },
                new SeedToken { Name = "colorPrimary", Value = "#1677ff", Source = "seed.ts" },
                new AliasToken { Name = "borderRadius", Value = "6px", Source = "alias.ts" },
                new SemanticToken { Name = "colorText", Value = "rgba(0,0,0,.88)", Source = "semantic.ts" }
            }, ":where(.ant-theme-app)");

            Assert.Equal("#1677ff", result.Resolved[1].Value);
            Assert.Equal("--ant-color-primary", result.CssVariables[1].Variable);
            Assert.Contains("--ant-primary-shadow: 0 2px 0 rgba(5, 145, 255, 0.1);", result.Css);
            Assert.Equal("token-values", result.Manifest.Layer);
            Assert.StartsWith("token-values:default:", result.Manifest.CacheKey);
        }

        [Fact]
        public void KeepsThemeAlgorithmAndScopeInManifest()
        {
            var result = TokenCompiler.Compile(new[] { new SeedToken { Name = "colorPrimary", Value = "#1668dc" } }, ":where(.ant-theme-app[data-theme='dark'])", "dark", new DarkThemeAlgorithm());

            Assert.Equal("dark", result.Manifest.Theme);
            Assert.Equal("dark", result.Manifest.Algorithm);
            Assert.Contains("data-theme='dark'", result.Manifest.Scope);
        }
    }
}
