using System.Linq;
using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class PreviewThemeCatalogTests
    {
        [Fact]
        public void ContainsAllReactPreviewThemesInStableOrder()
        {
            Assert.Equal(13, GeneratedPreviewThemes.All.Count);
            Assert.Equal(new[] { "light", "mui", "shadcn", "bootstrap", "cartoon", "dark", "illustration", "glass", "geek", "lark", "blossom", "v4", "serene" }, GeneratedPreviewThemes.All.Select(theme => theme.Key));
            Assert.Equal("dark", GeneratedPreviewThemes.All.Single(theme => theme.Key == "dark").Algorithm);
            Assert.All(GeneratedPreviewThemes.All, theme => Assert.False(string.IsNullOrWhiteSpace(theme.ReactSource)));
            var light = GeneratedPreviewThemes.All.Single(theme => theme.Key == "light");
            Assert.Equal("#f5f8ff", light.Components["Layout"]["bodyBg"]);
            Assert.Equal("#1677FF", light.Components["Progress"]["defaultColor"]);
            var dark = GeneratedPreviewThemes.All.Single(theme => theme.Key == "dark");
            Assert.Equal("#050505", dark.Components["Layout"]["bodyBg"]);
            Assert.Equal("rgba(255, 255, 255, 0.68)", dark.Components["Menu"]["darkItemColor"]);
            var mui = GeneratedPreviewThemes.All.Single(theme => theme.Key == "mui");
            Assert.Equal("#1976d2", mui.Tokens["colorPrimary"]);
            Assert.Equal("#fff", mui.Components["Button"]["primaryColor"]);
            Assert.Contains("defaultTheme.components", GeneratedPreviewThemes.All.Single(theme => theme.Key == "v4").Unresolved);

            var config = PreviewThemeConfigFactory.Create("mui");
            Assert.Equal("mui", config.Key);
            Assert.Equal("#1976d2", config.Token.ColorPrimary);
            Assert.Equal("#fff", config.Components["Button"]["primaryColor"]);
        }
    }
}
