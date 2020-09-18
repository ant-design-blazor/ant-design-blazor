using System;
using System.Collections.Generic;
using AntDesign.Core.Helpers;
using Xunit;

namespace AntDesign.Tests.Core
{
    public class FormatterTests
    {
        [Theory]
        [MemberData(nameof(Format_values_seeds))]
        public void Format_values<T>(T value, string format, string expected)
        {
            var result = Formatter<T>.Format(value, format);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Format_values_seeds => new List<object[]>
        {
            new object[] { 1234567, "###,###", "1,234,567" },
            new object[] { DateTime.Parse("2020-09-06 12:10:10"), "yyyy-MM-dd hh:mm:ss", "2020-09-06 12:10:10" },
            new object[] { 1.32111F, "N02", "1.32" },
            new object[] { TimeSpan.FromDays(1), "d 'd'ay", "1 day" },
            new object[] { TimeSpan.FromMinutes(1), " mm 分", " 01 分" }
        };
    }
}
