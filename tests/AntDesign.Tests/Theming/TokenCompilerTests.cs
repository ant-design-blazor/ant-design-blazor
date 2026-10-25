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

            Assert.Equal("#1677ff", result.Resolved.Single(token => token.Name == "colorPrimary").Value);
            Assert.Contains(result.CssVariables, token => token.Variable == "--ant-color-primary");
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
        public void DefaultAlgorithmDerivesFontRadiusAndMotionMaps()
        {
            var result = TokenCompiler.Compile(new[]
            {
                new SeedToken { Name = "fontSize", Value = "14" },
                new SeedToken { Name = "borderRadius", Value = "6" },
                new SeedToken { Name = "motionBase", Value = "0.1" },
                new SeedToken { Name = "motionUnit", Value = "0.1" },
                new SeedToken { Name = "lineWidth", Value = "1" }
            }, ":root");

            Assert.Equal("12", result.Resolved.Single(token => token.Name == "fontSizeSM").Value);
            Assert.Equal("16", result.Resolved.Single(token => token.Name == "fontSizeLG").Value);
            Assert.Equal("2", result.Resolved.Single(token => token.Name == "borderRadiusXS").Value);
            Assert.Equal("8", result.Resolved.Single(token => token.Name == "borderRadiusLG").Value);
            Assert.Equal("0.2s", result.Resolved.Single(token => token.Name == "motionDurationFast").Value);
            Assert.Equal("2", result.Resolved.Single(token => token.Name == "lineWidthBold").Value);
        }

        [Fact]
        public void DefaultAlgorithmDerivesSemanticColorAndAliasTokens()
        {
            var result = TokenCompiler.Compile(new[]
            {
                new SeedToken { Name = "colorPrimary", Value = "#1677ff" },
                new SeedToken { Name = "colorTextBase", Value = "#000000" },
                new SeedToken { Name = "colorBgBase", Value = "#ffffff" }
            }, ":root");

            Assert.Equal("#e6f4ff", result.Resolved.Single(token => token.Name == "colorPrimaryBg").Value);
            Assert.Equal("#1677ff", result.Resolved.Single(token => token.Name == "colorPrimary").Value);
            Assert.Equal("rgba(0, 0, 0, 0.65)", result.Resolved.Single(token => token.Name == "colorTextSecondary").Value);
            Assert.Equal("rgba(0, 0, 0, 0.06)", result.Resolved.Single(token => token.Name == "colorFillSecondary").Value);
            Assert.Equal("#ffffff", result.Resolved.Single(token => token.Name == "colorBgContainer").Value);
            Assert.Equal("rgba(0, 0, 0, 0.06)", result.Resolved.Single(token => token.Name == "colorFillContent").Value);
            Assert.Equal("#f0f0f0", result.Resolved.Single(token => token.Name == "colorBorderSecondary").Value);
            Assert.Equal("rgba(5, 5, 5, 0.06)", result.Resolved.Single(token => token.Name == "colorSplit").Value);
        }

        [Fact]
        public void DefaultAlgorithmDerivesResponsiveAndShadowAliases()
        {
            var result = TokenCompiler.Compile(new[]
            {
                new SeedToken { Name = "colorShadow", Value = "#000000" },
                new SeedToken { Name = "motion", Value = "false" }
            }, ":root");

            Assert.Equal("480", result.Resolved.Single(token => token.Name == "screenXS").Value);
            Assert.Equal("575", result.Resolved.Single(token => token.Name == "screenXSMax").Value);
            Assert.Equal("0s", result.Resolved.Single(token => token.Name == "motionDurationFast").Value);
            Assert.Contains("rgba(0, 0, 0, 0.08)", result.Resolved.Single(token => token.Name == "boxShadow").Value);
            Assert.Contains("drop-shadow", result.Resolved.Single(token => token.Name == "dropShadowPopover").Value);
        }

        [Fact]
        public void CompactAlgorithmUsesCompactSizeAndHeightRules()
        {
            var result = TokenCompiler.Compile(new[]
            {
                new SeedToken { Name = "fontSize", Value = "14" },
                new SeedToken { Name = "fontSizeSM", Value = "12" },
                new SeedToken { Name = "sizeUnit", Value = "4" },
                new SeedToken { Name = "sizeStep", Value = "4" },
                new SeedToken { Name = "controlHeight", Value = "32" }
            }, ":root", "compact", new CompactThemeAlgorithm());

            Assert.Equal("12", result.Resolved.Single(token => token.Name == "fontSize").Value);
            Assert.Equal("28", result.Resolved.Single(token => token.Name == "controlHeight").Value);
            Assert.Equal("21", result.Resolved.Single(token => token.Name == "controlHeightSM").Value);
            Assert.Equal("14", result.Resolved.Single(token => token.Name == "controlHeightXS").Value);
            Assert.Equal("35", result.Resolved.Single(token => token.Name == "controlHeightLG").Value);
            Assert.Equal("8", result.Resolved.Single(token => token.Name == "size").Value);
            Assert.Equal("16", result.Resolved.Single(token => token.Name == "sizeLG").Value);
        }

        [Theory]
        [InlineData("default", typeof(DefaultThemeAlgorithm))]
        [InlineData("dark", typeof(DarkThemeAlgorithm))]
        [InlineData("compact", typeof(CompactThemeAlgorithm))]
        public void ThemeAlgorithmFactoryResolvesAllStandardAlgorithms(string name, System.Type expectedType)
        {
            Assert.IsType(expectedType, ThemeAlgorithmFactory.Create(name));
        }

        [Fact]
        public void DarkAlgorithmResolvesDarkSemanticNeutralsAndColorPalette()
        {
            var values = new DarkThemeAlgorithm().Resolve(new[]
            {
                new SeedToken { Name = "colorPrimary", Value = "#1890ff" },
                new SeedToken { Name = "colorTextBase", Value = "#ffffff" },
                new SeedToken { Name = "colorBgBase", Value = "#000000" }
            }).ToDictionary(token => token.Name, token => token.Value);

            Assert.Equal("#177ddc", values["colorPrimary6"]);
            Assert.Equal("rgba(255, 255, 255, 0.85)", values["colorText"]);
            Assert.Equal("#141414", values["colorBgContainer"]);
            Assert.Equal("#424242", values["colorBorder"]);
            Assert.Equal(values["colorPrimaryBorder"], values["colorPrimaryBg"]);
            Assert.True(values.ContainsKey("blue10"));
        }

        [Fact]
        public void DefaultAlgorithmMatchesAntDesignPrimaryPalette()
        {
            var palette = AntDesign.Colors.ColorPalette.Generate("#1677ff");

            Assert.Equal(new[] { "#e6f4ff", "#bae0ff", "#91caff", "#69b1ff", "#4096ff", "#1677ff", "#0958d9", "#003eb3", "#002c8c", "#001d66" }, palette);
        }

        [Fact]
        public void DarkAlgorithmMatchesOfficialAntDesignColorsCases()
        {
            var palette = AntDesign.Colors.ColorPalette.Generate("#1890ff", true, "#141414");

            Assert.Equal(new[] { "#111d2c", "#112a45", "#15395b", "#164c7e", "#1765ad", "#177ddc", "#3c9ae8", "#65b7f3", "#8dcff8", "#b7e3fa" }, palette);
        }
        [Fact]
        public void MatchesOfficialPresetPaletteExportCases()
        {
            Assert.Equal(new[] { "red", "volcano", "orange", "gold", "yellow", "lime", "green", "cyan", "blue", "geekblue", "purple", "magenta", "grey" }, PresetColorPalettes.All.Keys);
            Assert.Equal("#1677ff", PresetColorPalettes.Primary("blue"));
            Assert.Equal(new[] { "#e6f4ff", "#bae0ff", "#91caff", "#69b1ff", "#4096ff", "#1677ff", "#0958d9", "#003eb3", "#002c8c", "#001d66" }, PresetColorPalettes.All["blue"]);
            Assert.All(PresetColorPalettes.All.Values, palette => Assert.Equal(10, palette.Length));
            Assert.All(PresetColorPalettes.Dark.Values, palette => Assert.Equal(10, palette.Length));
        }

        [Fact]
        public void HandlesOfficialGreyBoundaryCase()
        {
            Assert.NotEmpty(AntDesign.Colors.ColorPalette.Generate("#666666"));
            Assert.NotEmpty(AntDesign.Colors.ColorPalette.Generate("#666666", true, "#141414"));
        }
    }
}
