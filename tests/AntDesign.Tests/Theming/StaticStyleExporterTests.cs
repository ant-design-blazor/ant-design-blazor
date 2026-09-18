using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class StaticStyleExporterTests
    {
        [Fact]
        public void ExportsTheSameTokenCssForSsrAndRuntimeConfig()
        {
            var theme = new ThemeConfig
            {
                Key = "dark",
                Seed = new GeneratedSeedToken { ColorPrimary = "#1677ff" }
            };
            var exporter = new StaticStyleExporter();

            var result = exporter.Export(new StaticStyleRequest { Theme = theme, IncludeStyleTag = true, Nonce = "nonce-1" });
            var runtime = TokenCssSerializer.SerializeVariables(result.Scope, new[]
            {
                new DesignToken { Name = "colorPrimary", Value = theme.GetResolvedTokenValues()["colorPrimary"], CssVariable = "--ant-color-primary" }
            });

            Assert.Contains("--ant-color-primary:", result.Css);
            Assert.Contains("nonce=\"nonce-1\"", result.StyleTag);
            Assert.NotEqual(string.Empty, result.Digest);
            Assert.Contains(result.Scope, runtime);
        }

        [Fact]
        public void DigestAndCacheKeyChangeWhenStaticCssChanges()
        {
            var exporter = new StaticStyleExporter();
            var theme = new ThemeConfig();
            var first = exporter.Export(new StaticStyleRequest { Theme = theme, StaticCss = ".a{color:red}" });
            var second = exporter.Export(new StaticStyleRequest { Theme = theme, StaticCss = ".a{color:blue}" });

            Assert.NotEqual(first.Digest, second.Digest);
            Assert.NotEqual(first.CacheKey, second.CacheKey);
        }
    }
}
