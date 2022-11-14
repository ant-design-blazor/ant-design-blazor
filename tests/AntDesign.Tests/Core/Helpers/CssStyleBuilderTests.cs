// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public sealed class CssStyleBuilderTests
    {
        [Fact]
        public void AddStyle()
        {
            var builder = new CssStyleBuilder();

            var cssStyle = builder
                .AddStyle("font-size", "10px")
                .AddStyle("color: red")
                .AddStyle("margin: 1px; ")
                .AddStyle("background-color: white;")
                .Build();

            Assert.Equal("font-size: 10px; color: red; margin: 1px; background-color: white; ", cssStyle);
        }
    }
}
