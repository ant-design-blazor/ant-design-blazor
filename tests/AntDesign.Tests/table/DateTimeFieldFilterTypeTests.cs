// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using AntDesign.Filters;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class DateTimeFieldFilterTypeTests
    {
        private static readonly Expression<Func<DateTime?, DateTime?>> NullableField = value => value;

        [Theory]
        [InlineData("2026-09-05T12:34:56.789", "2026-09-05T12:34:56.500", true)]
        [InlineData("2026-09-05T12:34:56.789", "2026-09-06T00:00:00.000", false)]
        public void The_same_date_with_compares_only_dates(string left, string right, bool expected)
        {
            var filterType = new DateTimeFieldFilterType();
            var expression = filterType.GetFilterExpression(
                TableFilterCompareOperator.TheSameDateWith,
                Expression.Property(NullableField.Body, nameof(Nullable<DateTime>.Value)),
                Expression.Constant(DateTime.Parse(right)));

            var predicate = Expression.Lambda<Func<DateTime?, bool>>(
                Expression.AndAlso(Expression.NotEqual(NullableField.Body, Expression.Constant(null)), expression),
                NullableField.Parameters).Compile();

            Assert.Equal(expected, predicate(DateTime.Parse(left)));
        }

        [Theory]
        [InlineData("2026-09-05", "2026-09-01", "2026-09-10", true)]
        [InlineData("2026-09-11", "2026-09-01", "2026-09-10", false)]
        public void Between_compares_nullable_dates(string fieldValue, string start, string end, bool expected)
        {
            var filterType = new DateTimeFieldFilterType();
            var range = new[] { DateTime.Parse(start), DateTime.Parse(end) };
            var expression = filterType.GetFilterExpression(
                TableFilterCompareOperator.Between,
                NullableField.Body,
                Expression.Constant(range));

            var predicate = Expression.Lambda<Func<DateTime?, bool>>(expression, NullableField.Parameters).Compile();

            Assert.Equal(expected, predicate(DateTime.Parse(fieldValue)));
        }

        [Theory]
        [InlineData(TableFilterCompareOperator.IsNull, null, true)]
        [InlineData(TableFilterCompareOperator.IsNotNull, "2026-09-05", true)]
        [InlineData(TableFilterCompareOperator.NotEquals, null, true)]
        public void Nullable_operators_include_null_and_not_null_branches(TableFilterCompareOperator compareOperator, string? fieldValue, bool expected)
        {
            var filterType = new DateTimeFieldFilterType();
            var expression = filterType.GetFilterExpression(
                compareOperator,
                NullableField.Body,
                Expression.Constant((DateTime?)null, typeof(DateTime?)));

            var predicate = Expression.Lambda<Func<DateTime?, bool>>(expression, NullableField.Parameters).Compile();

            Assert.Equal(expected, predicate(fieldValue is null ? null : DateTime.Parse(fieldValue)));
        }
    }
}
