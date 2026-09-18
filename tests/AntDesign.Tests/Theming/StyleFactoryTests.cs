using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class StyleFactoryTests
    {
        [Fact]
        public void ExposesResolvedTokenAndCssVariableViews()
        {
            var theme = new ThemeContext(new ThemeConfig { Token = { ["colorPrimary"] = "#1677ff" } });
            var factory = new StyleFactory();
            var result = factory.Create("button", "root", theme, context => new StyleDefinition
            {
                Selector = ".ant-btn",
                Declarations = { new StyleDeclaration { Property = "color", Value = context.CssVar("colorPrimary") } }
            });

            Assert.Equal("#1677ff", theme.Token.Get("colorPrimary"));
            Assert.Equal("var(--ant-color-primary)", theme.Token.CssVar("colorPrimary"));
            Assert.Contains("var(--ant-color-primary)", result.Css);
        }

        [Fact]
        public void CachesEquivalentDefinitionsAndMergesClassesDeterministically()
        {
            var theme = new ThemeContext(new ThemeConfig());
            var factory = new StyleFactory();
            StyleDefinition Definition(StyleContext _) => new() { Selector = ".x", Declarations = { new StyleDeclaration { Property = "display", Value = "block" } } };
            var first = factory.Create("button", "root", theme, Definition);
            var second = factory.Create("button", "root", theme, Definition);

            Assert.Same(first, second);
            Assert.Equal("a b c", factory.Cx("a b", "b", "c"));
        }

        [Fact]
        public void UsesTypedAndInheritedThemeSnapshots()
        {
            var parent = new ThemeContext(new ThemeConfig
            {
                Seed = new GeneratedSeedToken { ColorPrimary = "#1677ff" }
            });
            var child = new ThemeContext(new ThemeConfig
            {
                Token = { ColorSuccess = "#52c41a" }
            }, parent);

            Assert.Equal("#1677ff", child.Token.Get("colorPrimary"));
            Assert.Equal("#52c41a", child.Token.Get("colorSuccess"));
            Assert.Equal("var(--ant-color-primary)", child.Token.CssVar("colorPrimary"));
            Assert.Equal(":where(.ant-theme[data-theme='default'])", child.ScopeSelector);
        }
    }
}
