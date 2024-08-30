// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Xunit;

namespace AntDesign.Tests.Core
{
    public class CssSizeLengthTests
    {
        public enum DataType
        {
            IntegerType,
            DoubleType,
            DecimalType,
            StringType
        };

        [Theory]
        [MemberData(nameof(Css_numeric_seeds))]
        public void Css_numeric(object value, DataType type, string expected, bool noUnit)
        {
            var result = type switch
            {
                DataType.IntegerType => noUnit ? new CssSizeLength((int)value, noUnit).ToString() : new CssSizeLength((int)value).ToString(),
                DataType.DoubleType => noUnit ? new CssSizeLength((double)value, noUnit).ToString() : new CssSizeLength((double)value).ToString(),
                DataType.DecimalType => noUnit ? new CssSizeLength((decimal)value, noUnit).ToString() : new CssSizeLength((decimal)value).ToString(),
                _ => throw new ArgumentException("Only integer/double/decimal allowed in this type of test.")
            };
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Css_numeric_seeds => new List<object[]>
        {
            new object[] { 1234567, DataType.IntegerType, "1234567px", false },
            new object[] { 1234567d, DataType.DoubleType, "1234567px", false },
            new object[] { 123456.7d, DataType.DoubleType, "123456.7px", false },
            new object[] { 12345.67d, DataType.DoubleType, "12345.67px", false },
            new object[] { 1234.567d, DataType.DoubleType, "1234.567px", false },
            new object[] { 1234567m, DataType.DecimalType, "1234567px", false },
            new object[] { 123456.7m, DataType.DecimalType, "123456.7px", false },
            new object[] { 12345.67m, DataType.DecimalType, "12345.67px", false },
            new object[] { 1234.567m, DataType.DecimalType, "1234.567px", false },

            new object[] { 1234567, DataType.IntegerType, "1234567", true },
            new object[] { 1234567d, DataType.DoubleType, "1234567", true },
            new object[] { 123456.7d, DataType.DoubleType, "123456.7", true },
            new object[] { 12345.67d, DataType.DoubleType, "12345.67", true },
            new object[] { 1234.567d, DataType.DoubleType, "1234.567", true },
            new object[] { 1234567m, DataType.DecimalType, "1234567", true },
            new object[] { 123456.7m, DataType.DecimalType, "123456.7", true },
            new object[] { 12345.67m, DataType.DecimalType, "12345.67", true },
            new object[] { 1234.567m, DataType.DecimalType, "1234.567", true },
        };

        [Theory]
        [MemberData(nameof(Css_implicit_numeric_seeds))]
        public void Css_implicit_numeric(object value, DataType type, string expected)
        {
            var result = type switch
            {
                DataType.IntegerType => ((CssSizeLength)(int)value).ToString(),
                DataType.DoubleType => ((CssSizeLength)(double)value).ToString(),
                DataType.DecimalType => ((CssSizeLength)(decimal)value).ToString(),
                _ => throw new ArgumentException("Only integer/double/decimal allowed in this type of test.")
            };
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Css_implicit_numeric_seeds => new List<object[]>
        {
            new object[] { 1234567, DataType.IntegerType, "1234567px"},
            new object[] { 1234567d, DataType.DoubleType, "1234567px"},
            new object[] { 123456.7d, DataType.DoubleType, "123456.7px"},
            new object[] { 12345.67d, DataType.DoubleType, "12345.67px"},
            new object[] { 1234.567d, DataType.DoubleType, "1234.567px"},
            new object[] { 1234567m, DataType.DecimalType, "1234567px"},
            new object[] { 123456.7m, DataType.DecimalType, "123456.7px"},
            new object[] { 12345.67m, DataType.DecimalType, "12345.67px"},
            new object[] { 1234.567m, DataType.DecimalType, "1234.567px"},
        };

        [Theory]
        [MemberData(nameof(Css_implicit_string_seeds))]
        public void Css_tryprase_string(string value, CssSizeLength expected, bool expectedParsed)
        {
            var parsedResult = CssSizeLength.TryParse(value, out var result);
            Assert.Equal(expectedParsed, parsedResult);
            if (parsedResult)
            {
                Assert.Equal(expected, result);
            }
        }

        public static IEnumerable<object[]> Css_implicit_string_seeds => new List<object[]>
        {
            new object[] { "1234567",         (CssSizeLength)"1234567px",        true  },
            new object[] { "1234567px",       (CssSizeLength)"1234567px",        true  },
            new object[] { "1234567rem",      (CssSizeLength)"1234567rem",       true  },
            new object[] { "1234567%",        (CssSizeLength)"1234567%",         true  },
            new object[] { "large",           (CssSizeLength)"large" ,           false },
            new object[] { "calc( 10px / 2 )",(CssSizeLength)"calc( 10px / 2 )", true  },
            new object[] { "1234567abc",      (CssSizeLength)"1234567abc",       false },
        };
    }
}
