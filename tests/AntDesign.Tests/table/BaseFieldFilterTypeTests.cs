// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using AntDesign.Filters;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class BaseFieldFilterTypeTests
    {
        private sealed class TestFilterType : BaseFieldFilterType
        {
            public override RenderFragment<TableFilterInputRenderOptions> FilterInput => null!;
        }

        private static readonly Expression<Func<int, int>> IntField = value => value;
        private static readonly Expression<Func<object?, object?>> ObjectField = value => value;

        [Theory]
        [InlineData(TableFilterCompareOperator.Equals, 2, 2, true)]
        [InlineData(TableFilterCompareOperator.NotEquals, 2, 3, true)]
        [InlineData(TableFilterCompareOperator.GreaterThan, 3, 2, true)]
        [InlineData(TableFilterCompareOperator.LessThan, 1, 2, true)]
        [InlineData(TableFilterCompareOperator.GreaterThanOrEquals, 2, 2, true)]
        [InlineData(TableFilterCompareOperator.LessThanOrEquals, 2, 2, true)]
        public void Base_filter_builds_comparison_expressions(TableFilterCompareOperator compareOperator, int fieldValue, int filterValue, bool expected)
        {
            var filterType = new TestFilterType();
            var expression = filterType.GetFilterExpression(compareOperator, IntField.Body, Expression.Constant(filterValue));
            var predicate = Expression.Lambda<Func<int, bool>>(expression, IntField.Parameters).Compile();

            Assert.Equal(expected, predicate(fieldValue));
        }

        [Fact]
        public void Base_filter_converts_object_expressions_to_the_actual_field_type()
        {
            var filterType = new TestFilterType();
            var expression = filterType.GetFilterExpression(
                TableFilterCompareOperator.Equals,
                ObjectField.Body,
                Expression.Constant(2));

            Assert.Equal(typeof(int), ((BinaryExpression)expression).Left.Type);
            Assert.Equal(typeof(int), ((BinaryExpression)expression).Right.Type);
        }

        [Fact]
        public void Base_filter_rejects_unsupported_operators()
        {
            var filterType = new TestFilterType();

            Assert.Throws<NotSupportedException>(() => filterType.GetFilterExpression(
                TableFilterCompareOperator.Contains,
                IntField.Body,
                Expression.Constant(2)));
        }
    }
}
