// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public class StyleHelperTests
    {
        [Theory]
        [InlineData("10", "10px")]
        [InlineData("10px", "10px")]
        public void ToCssPixel_String(string value, string expectedCss)
        {
            var css = StyleHelper.ToCssPixel(value);

            Assert.Equal(expectedCss, css);
        }

        [Fact]
        public void ToCssPixel_Int32()
        {
            var css = StyleHelper.ToCssPixel(10);

            Assert.Equal("10px", css);
        }
    }
}
