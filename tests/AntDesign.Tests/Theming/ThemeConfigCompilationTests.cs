using System.Collections.Generic;
using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class ThemeConfigCompilationTests
    {
        [Fact]
        public void DefaultAndDarkAlgorithmsResolveGeneratedSeedAndHeadingDefaults()
        {
            var light = new ThemeConfig().GetResolvedTokenValues();
            Assert.Equal("#1677ff", light["colorPrimary"]);
            Assert.Equal("32", light["controlHeight"]);
            Assert.Equal("38", light["fontSizeHeading1"]);
            Assert.Equal("16", light["fontSizeHeading5"]);
            Assert.Equal("1.5", light["lineHeightHeading5"]);
            Assert.Equal("0.1s", light["motionDurationFast"]);
            Assert.Equal("#4096ff", light["colorPrimaryHover"]);
            Assert.Equal(light["colorError3"], light["colorErrorBgActive"]);

            var dark = new ThemeConfig { Algorithm = "dark" }.GetResolvedTokenValues();
            Assert.Equal("rgba(255, 255, 255, 0.85)", dark["colorText"]);
            Assert.Equal("#141414", dark["colorBgContainer"]);
        }

        [Fact]
        public void MergesTypedSeedAliasAndDynamicTokens()
        {
            var config = new ThemeConfig
            {
                Seed = new GeneratedSeedToken { ColorPrimary = "#1677ff", FontSize = 14 },
                Alias = new GeneratedAliasToken { ColorTextHeading = "#123456" }
            };
            config.Token["customAccent"] = "#abcdef";

            var values = config.GetResolvedTokenValues();

            Assert.Equal("#1677ff", values["colorPrimary"]);
            Assert.Equal("#123456", values["colorTextHeading"]);
            Assert.Equal("#abcdef", values["customAccent"]);
            Assert.Contains("colorPrimaryBg", values.Keys);
        }

        [Fact]
        public void MergesTypedComponentTokensIntoComponentCssValues()
        {
            var config = new ThemeConfig();
            config.SetComponentToken("Button", new ButtonComponentToken { PrimaryColor = "#1677ff" });

            var values = config.GetResolvedComponentValues();

            Assert.Equal("#1677ff", values["Button"]["primaryColor"]);
        }

        [Fact]
        public void MergesDynamicAvatarTokensIntoComponentCssValues()
        {
            var config = new ThemeConfig();
            config.Components["Avatar"] = new Dictionary<string, string>
            {
                ["containerSize"] = "60",
                ["groupOverlapping"] = "-10",
                ["groupBorderColor"] = "#eee"
            };

            var values = config.GetResolvedComponentValues();

            Assert.Equal("60", values["Avatar"]["containerSize"]);
            Assert.Equal("-10", values["Avatar"]["groupOverlapping"]);
            Assert.Equal("#eee", values["Avatar"]["groupBorderColor"]);
            Assert.Equal("60px", CssVariableValueFormatter.Format("Avatar-containerSize", values["Avatar"]["containerSize"]));
            Assert.Equal("-10px", CssVariableValueFormatter.Format("Avatar-groupOverlapping", values["Avatar"]["groupOverlapping"]));
        }

        [Fact]
        public void ChildThemeInheritsParentResolvedValuesAndOverridesExplicitTokens()
        {
            var parent = new ThemeConfig
            {
                Seed = new GeneratedSeedToken { ColorPrimary = "#1677ff" }
            };
            var child = new ThemeConfig
            {
                Seed = new GeneratedSeedToken { ColorSuccess = "#52c41a" }
            };

            var values = child.GetResolvedTokenValues(parent.GetResolvedTokenValues());

            Assert.Equal("#1677ff", values["colorPrimary"]);
            Assert.Equal("#52c41a", values["colorSuccess"]);
            Assert.Contains("colorPrimaryBg", values.Keys);
        }

        [Fact]
        public void FormatsRuntimeCssVariablesLikeReactCssInJs()
        {
            Assert.Equal("12px", CssVariableValueFormatter.Format("paddingSM", "12"));
            Assert.Equal("14px", CssVariableValueFormatter.Format("fontSize", "14"));
            Assert.Equal("1.57142857142857", CssVariableValueFormatter.Format("lineHeight", "1.57142857142857"));
            Assert.Equal("0", CssVariableValueFormatter.Format("padding", "0"));
            Assert.Equal("0.2s", CssVariableValueFormatter.Format("motionDurationFast", "0.2s"));
        }
    }
}
