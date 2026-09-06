// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using AntDesign.Filters;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class StringFieldFilterTypeTests
    {
        private static readonly Expression<Func<string?, string?>> Field = value => value;

        [Theory]
        [InlineData(TableFilterCompareOperator.Contains, "ELP", "help", true)]
        [InlineData(TableFilterCompareOperator.NotContains, "elp", "help", false)]
        [InlineData(TableFilterCompareOperator.StartsWith, "HE", "help", true)]
        [InlineData(TableFilterCompareOperator.EndsWith, "LP", "help", true)]
        [InlineData(TableFilterCompareOperator.NotEquals, "HELP", "help", false)]
        [InlineData(TableFilterCompareOperator.NotEquals, "help", null, true)]
        public void GetFilterExpression_builds_case_insensitive_string_predicates(TableFilterCompareOperator compareOperator, string? filterValue, string? fieldValue, bool expected)
        {
            var filterType = new StringFieldFilterType();
            var expression = filterType.GetFilterExpression(compareOperator, Field.Body, Expression.Constant(filterValue, typeof(string)));
            var predicate = Expression.Lambda<Func<string?, bool>>(expression, Field.Parameters).Compile();

            Assert.Equal(expected, predicate(fieldValue));
        }

        [Theory]
        [InlineData(TableFilterCompareOperator.IsNull, null, true)]
        [InlineData(TableFilterCompareOperator.IsNotNull, "help", true)]
        public void GetFilterExpression_delegates_null_operators(TableFilterCompareOperator compareOperator, string? fieldValue, bool expected)
        {
            var filterType = new StringFieldFilterType();
            var expression = filterType.GetFilterExpression(compareOperator, Field.Body, Expression.Constant(null, typeof(string)));
            var predicate = Expression.Lambda<Func<string?, bool>>(expression, Field.Parameters).Compile();

            Assert.Equal(expected, predicate(fieldValue));
        }
    }
}
