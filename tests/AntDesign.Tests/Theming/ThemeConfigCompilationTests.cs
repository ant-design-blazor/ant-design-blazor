using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class ThemeConfigCompilationTests
    {
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
    }
}
