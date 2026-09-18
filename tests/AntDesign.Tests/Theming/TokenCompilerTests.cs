using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void DefaultAlgorithmDerivesMapTokensFromSeedTokens()
        {
            var result = TokenCompiler.Compile(new[]
            {
                new SeedToken { Name = "controlHeight", Value = "32" },
                new SeedToken { Name = "sizeUnit", Value = "4" },
                new SeedToken { Name = "sizeStep", Value = "4" },
                new SeedToken { Name = "colorTextBase", Value = "#000000" },
                new SeedToken { Name = "colorBgBase", Value = "#ffffff" }
            }, ":root");

            Assert.Equal("24", result.Resolved.Single(token => token.Name == "controlHeightSM").Value);
            Assert.Equal("16", result.Resolved.Single(token => token.Name == "size").Value);
            Assert.Equal("rgba(0, 0, 0, 0.88)", result.Resolved.Single(token => token.Name == "colorText").Value);
        }

        [Fact]
        public void DefaultAlgorithmMatchesAntDesignPrimaryPalette()
        {
            var result = TokenCompiler.Compile(new[] { new SeedToken { Name = "colorPrimary", Value = "#1677ff" } }, ":root");
            var palette = Enumerable.Range(1, 10).Select(index => result.Resolved.Single(token => token.Name == $"colorPrimary{index}").Value).ToArray();

            Assert.Equal(new[] { "#e6f4ff", "#bae0ff", "#91caff", "#69b1ff", "#4096ff", "#1677ff", "#0958d9", "#003eb3", "#002c8c", "#001d66" }, palette);
        }

        [Fact]
        public void DarkAlgorithmMatchesOfficialAntDesignColorsCases()
        {
            var result = TokenCompiler.Compile(new[] { new SeedToken { Name = "colorPrimary", Value = "#1890ff" } }, ":root", "dark", new DarkThemeAlgorithm());
            var palette = Enumerable.Range(1, 10).Select(index => result.Resolved.Single(token => token.Name == $"colorPrimary{index}").Value).ToArray();

            Assert.Equal(new[] { "#111d2c", "#112a45", "#15395b", "#164c7e", "#1765ad", "#177ddc", "#3c9ae8", "#65b7f3", "#8dcff8", "#b7e3fa" }, palette);
        }
    }
}
