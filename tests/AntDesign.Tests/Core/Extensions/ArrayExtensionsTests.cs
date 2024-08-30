// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.Core.Extensions;
using Xunit;

namespace AntDesign.Tests.Core.Extensions
{
    public class ArrayExtensionsTests
    {
        [Theory]
        [MemberData(nameof(Array_seeds))]
        public void ShiftLeftTest(string[] seeds, int offset, string[] expected)
        {
            var actual = seeds.Scroll(offset);

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> Array_seeds => new List<object[]>
        {
            new object[]{ new[] { "A", "B", "C", "D" }, 1, new[]{ "B","C","D","A" } },
            new object[]{ new[] { "A", "B", "C", "D" }, 0, new[]{ "A", "B","C", "D"} },
            new object[]{ new[] { "A", "B", "C", "D" }, -1, new[]{ "D", "A", "B","C", } },
        };
    }
}
