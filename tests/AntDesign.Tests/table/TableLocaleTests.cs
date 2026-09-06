// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class TableLocaleTests
    {
        [Theory]
        [InlineData(TableFilterCompareOperator.Equals, "Equal")]
        [InlineData(TableFilterCompareOperator.NotEquals, "Not Equal")]
        [InlineData(TableFilterCompareOperator.Contains, "Contains")]
        [InlineData(TableFilterCompareOperator.NotContains, "Not Contains")]
        [InlineData(TableFilterCompareOperator.StartsWith, "Start With")]
        [InlineData(TableFilterCompareOperator.EndsWith, "End With")]
        [InlineData(TableFilterCompareOperator.GreaterThan, "Greater Than")]
        [InlineData(TableFilterCompareOperator.LessThan, "Less Than")]
        [InlineData(TableFilterCompareOperator.GreaterThanOrEquals, "Greater Than Or Equals")]
        [InlineData(TableFilterCompareOperator.LessThanOrEquals, "Less Than Or Equals")]
        [InlineData(TableFilterCompareOperator.IsNull, "Is Null")]
        [InlineData(TableFilterCompareOperator.IsNotNull, "Is Not Null")]
        [InlineData(TableFilterCompareOperator.TheSameDateWith, "The Same Date With")]
        [InlineData(TableFilterCompareOperator.Between, "Between")]
        public void Operator_returns_localized_label(TableFilterCompareOperator compareOperator, string expected)
        {
            var locale = new FilterOptionsLocale();

            Assert.Equal(expected, locale.Operator(compareOperator));
        }

        [Fact]
        public void Operator_rejects_unknown_values()
        {
            var locale = new FilterOptionsLocale();

            Assert.Throws<ArgumentOutOfRangeException>(() => locale.Operator((TableFilterCompareOperator)999));
        }
    }
}
