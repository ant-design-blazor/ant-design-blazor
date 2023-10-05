// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public class EnumHelperTests
    {
        [Theory]
        [MemberData(nameof(Combine_seeds))]
        public void Combine(IEnumerable<TestColor> colors, TestColor? expectedColor)
        {
            var result = EnumHelper<TestColor>.Combine(colors);

            Assert.Equal(expectedColor, result);
        }

        public static IEnumerable<object[]> Combine_seeds => new List<object[]>
        {
            new object[] { null, null },
            new object[] { Array.Empty<TestColor>(), null },
            new object[] { new TestColor[] { TestColor.Red }, TestColor.Red },
            new object[] { new TestColor[] { TestColor.Red, TestColor.Yellow } , TestColor.Red | TestColor.Yellow },
            new object[] { new TestColor[] { TestColor.Red, TestColor.Yellow, TestColor.Green }, TestColor.Red | TestColor.Yellow | TestColor.Green },
        };

        [Theory]
        [MemberData(nameof(Split_seeds))]
        public void Split(object? value, IEnumerable<TestColor> expectedList)
        {
            var list = EnumHelper<TestColor>.Split(value);

            Assert.True(list.SequenceEqual(expectedList));
        }

        public static IEnumerable<object[]> Split_seeds => new List<object[]>
        {
            new object[] { null, Array.Empty<TestColor>() },
            new object[] { (int)TestColor.Red, new TestColor[] { TestColor.Red } },
            new object[] { "Red", new TestColor[] { TestColor.Red } },
            new object[] { "Red,Green", new TestColor[] { TestColor.Red, TestColor.Green } },
        };

        [Fact]
        public void GetValueList()
        {
            var list = EnumHelper<TestColor>.GetValueList();

            Assert.Equal(TestColor.Red, list.ElementAt(0));
            Assert.Equal(TestColor.Yellow, list.ElementAt(1));
            Assert.Equal(TestColor.Green, list.ElementAt(2));
        }

        [Fact]
        public void GetValueLabelList()
        {
            var dict = EnumHelper<TestColor>
                .GetValueLabelList()
                .ToDictionary(p => p.Value, p => p.Label);

            Assert.Equal(3, dict.Count);
            Assert.Equal("Hong", dict[TestColor.Red]);
            Assert.Equal("Yellow", dict[TestColor.Yellow]);
            Assert.Equal("Green", dict[TestColor.Green]);
        }

        [Theory]
        [InlineData(TestColor.Red, "Hong")]
        [InlineData(TestColor.Green, "Green")]
        public void GetDisplayName(TestColor color, string expectedName)
        {
            var name = EnumHelper<TestColor>.GetDisplayName(color);

            Assert.Equal(expectedName, name);
        }

        public enum TestColor
        {
            [Display(Name = "Hong")]
            Red = 0b001,

            Yellow = 0b010,
            Green = 0b100
        }
    }
}
