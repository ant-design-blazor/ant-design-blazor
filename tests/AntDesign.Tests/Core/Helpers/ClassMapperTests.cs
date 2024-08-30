// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public class ClassMapperTests
    {
        [Fact]
        public void Add()
        {
            var mapper = new ClassMapper()
                .Add("A")
                .Get(() => "B");

            var str = mapper.ToString();

            Assert.Equal("A B", str);
        }

        [Theory]
        [InlineData(false, "Test")]
        [InlineData(true, "Test A B")]
        public void AddIf(bool select, string expected)
        {
            var mapper = new ClassMapper("Test")
                .If("A", () => select)
                .GetIf(() => "B", () => select);

            var str = mapper.ToString();

            Assert.Equal(expected, str);
        }

        [Fact]
        public void Clear()
        {
            var mapper = new ClassMapper("Test")
                .Add("A")
                .Get(() => "B")
                .Clear();

            var str = mapper.ToString();

            Assert.Equal("Test", str);
        }
    }
}
