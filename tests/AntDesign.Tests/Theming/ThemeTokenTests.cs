using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class ThemeTokenTests
    {
        [Fact]
        public void SupportsStaticPropertiesAndDynamicIndexerValues()
        {
            var token = new ThemeToken
            {
                ColorPrimary = "#1677ff",
                BorderRadius = "6px"
            };
            token["customAccent"] = "#722ed1";

            Assert.Equal("#1677ff", token["colorPrimary"]);
            Assert.Equal("6px", token.BorderRadius);
            Assert.Equal("#722ed1", token.Values["customAccent"]);
        }

        [Fact]
        public void ExposesGeneratedSeedMapAndAliasShortcuts()
        {
            var token = new ThemeToken
            {
                ColorPrimary = "#1677ff",
                FontSize = "14px",
                ColorPrimaryBg = "#e6f4ff",
                MotionDurationFast = "0.1s",
                BoxShadowCard = "0 1px 2px #000"
            };

            Assert.Equal("#1677ff", token["colorPrimary"]);
            Assert.Equal("14px", token["fontSize"]);
            Assert.Equal("#e6f4ff", token["colorPrimaryBg"]);
            Assert.Equal("0.1s", token["motionDurationFast"]);
            Assert.Equal("0 1px 2px #000", token["boxShadowCard"]);
        }
    }
}
