// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using AntDesign.Filters;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class EnumFieldFilterTypeTests
    {
        [Flags]
        public enum TestPermission
        {
            None = 0,
            Read = 1,
            Write = 2,
            Execute = 4,
        }

        private static readonly Expression<Func<TestPermission, TestPermission>> Field = value => value;
        private static readonly Expression<Func<TestPermission?, TestPermission?>> NullableField = value => value;

        [Theory]
        [InlineData(TableFilterCompareOperator.Contains, TestPermission.Read, TestPermission.Read | TestPermission.Write, true)]
        [InlineData(TableFilterCompareOperator.NotContains, TestPermission.Execute, TestPermission.Read | TestPermission.Write, true)]
        [InlineData(TableFilterCompareOperator.Equals, TestPermission.Read, TestPermission.Read, true)]
        [InlineData(TableFilterCompareOperator.NotEquals, TestPermission.Read, TestPermission.Write, true)]
        public void Enum_filter_builds_flag_and_equality_predicates(TableFilterCompareOperator compareOperator, TestPermission filterValue, TestPermission fieldValue, bool expected)
        {
            var filterType = new EnumFieldFilterType<TestPermission>();
            var expression = filterType.GetFilterExpression(compareOperator, Field.Body, Expression.Constant(filterValue, typeof(TestPermission)));
            var predicate = Expression.Lambda<Func<TestPermission, bool>>(expression, Field.Parameters).Compile();

            Assert.Equal(expected, predicate(fieldValue));
        }

        [Fact]
        public void Enum_filter_wraps_nullable_fields_before_comparing()
        {
            var filterType = new EnumFieldFilterType<TestPermission>();
            var expression = filterType.GetFilterExpression(
                TableFilterCompareOperator.Equals,
                NullableField.Body,
                Expression.Constant(TestPermission.Read));
            var predicate = Expression.Lambda<Func<TestPermission?, bool>>(expression, NullableField.Parameters).Compile();

            Assert.True(predicate(TestPermission.Read));
            Assert.False(predicate(null));
            Assert.False(predicate(TestPermission.Write));
        }
    }
}
