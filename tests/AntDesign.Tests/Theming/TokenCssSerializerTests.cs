using System.Collections.Generic;
using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming
{
    public sealed class TokenCssSerializerTests
    {
        [Fact]
        public void SerializesTokensInDeterministicCssVariableOrder()
        {
            var tokens = new List<DesignToken>
            {
                new() { Name = "borderRadius", CssVariable = "--ant-border-radius", Value = "6px" },
                new() { Name = "colorPrimary", CssVariable = "--ant-color-primary", Value = "#1677ff" },
                new() { Name = "controlHeight", CssVariable = "--ant-control-height", Value = "32px" },
            };

            var css = TokenCssSerializer.SerializeVariables(":where(.ant-theme-app)", tokens);

            Assert.Equal(":where(.ant-theme-app) {\n" +
                         "  --ant-border-radius: 6px;\n" +
                         "  --ant-color-primary: #1677ff;\n" +
                         "  --ant-control-height: 32px;\n" +
                         "}\n", css);
        }

        [Fact]
        public void PreservesTokenValueTextForExactReactComparison()
        {
            var token = new DesignToken
            {
                CssVariable = "--ant-button-primary-shadow",
                Value = "0 2px 0 rgba(5, 145, 255, 0.1)"
            };

            var css = TokenCssSerializer.SerializeVariables(".ant-theme-app", new[] { token });

            Assert.Equal(".ant-theme-app {\n" +
                         "  --ant-button-primary-shadow: 0 2px 0 rgba(5, 145, 255, 0.1);\n" +
                         "}\n", css);
        }
    }
}
