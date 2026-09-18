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
    }
}
