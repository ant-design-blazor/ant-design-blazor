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
    }
}
