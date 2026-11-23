// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using AntDesign.Filters;
using Xunit;

namespace AntDesign.Tests.Table;

public class DateFieldFilterTests
{
    [Fact]
    public void Between_compares_a_boxed_DataTable_date_to_the_range_endpoints()
    {
        var start = new DateTime(2026, 9, 8);
        var end = new DateTime(2026, 11, 15);
        using var table = new DataTable();
        table.Columns.Add("CreateAt", typeof(DateTime));
        table.Rows.Add(start.AddDays(-1));
        table.Rows.Add(start);
        table.Rows.Add(new DateTime(2026, 10, 12));
        table.Rows.Add(end);
        table.Rows.Add(end.AddDays(1));
        table.Rows.Add(DBNull.Value);

        Expression<Func<DataRow, object?>> field = row => row.Field<object>("CreateAt");
        var range = Expression.Constant(new DateTime?[] { start, end });
        var comparison = new DateTimeFieldFilterType().GetFilterExpression(
            TableFilterCompareOperator.Between, field.Body, range);
        var predicate = Expression.Lambda<Func<DataRow, bool>>(comparison, field.Parameters);

        var result = table.AsEnumerable().AsQueryable().Where(predicate).Select(row => row.Field<DateTime>("CreateAt"));
        Assert.Equal(new[] { start, new DateTime(2026, 10, 12), end }, result);
    }

    [Fact]
    public void Between_preserves_a_boxed_interval_for_a_custom_date_filter()
    {
        var start = new DateTime(2026, 9, 8);
        var end = new DateTime(2026, 11, 15);
        var contained = new[] { start.AddDays(1), end.AddDays(-1) };
        using var table = new DataTable();
        table.Columns.Add("Availability", typeof(DateTime[]));
        table.Rows.Add((object)contained);
        table.Rows.Add((object)new[] { start.AddDays(-1), end });

        Expression<Func<DataRow, object?>> field = row => row.Field<object>("Availability");
        var bounds = Expression.Constant(new[] { start, end });
        var comparison = new ContainedIntervalFilter().GetFilterExpression(
            TableFilterCompareOperator.Between, field.Body, bounds);
        var predicate = Expression.Lambda<Func<DataRow, bool>>(comparison, field.Parameters);

        var matchedRow = Assert.Single(table.AsEnumerable().AsQueryable().Where(predicate));
        Assert.Same(contained, matchedRow.Field<DateTime[]>("Availability"));
    }

    private sealed class ContainedIntervalFilter : DateTimeFieldFilterType<DateTime>
    {
        protected override Expression GetNonNullFilterExpression(
            TableFilterCompareOperator compareOperator, Expression left, Expression right)
        {
            if (compareOperator != TableFilterCompareOperator.Between)
            {
                return base.GetNonNullFilterExpression(compareOperator, left, right);
            }

            var start = Expression.Constant(0);
            var end = Expression.Constant(1);
            return Expression.AndAlso(
                Expression.GreaterThanOrEqual(Expression.ArrayIndex(left, start), Expression.ArrayIndex(right, start)),
                Expression.LessThanOrEqual(Expression.ArrayIndex(left, end), Expression.ArrayIndex(right, end)));
        }
    }
}
