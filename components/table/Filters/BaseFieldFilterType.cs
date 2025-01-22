// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters;

public abstract class BaseFieldFilterType : IFieldFilterType
{
    public virtual TableFilterCompareOperator DefaultCompareOperator => TableFilterCompareOperator.Equals;

    public abstract RenderFragment<TableFilterInputRenderOptions> FilterInput { get; }

    public virtual IEnumerable<TableFilterCompareOperator> SupportedCompareOperators { get; set; } = _supportedCompareOperators;

    public virtual Dictionary<string, object> InputAttributes { get; set; } = [];

    private static IEnumerable<TableFilterCompareOperator> _supportedCompareOperators = new[]
    {
        TableFilterCompareOperator.Equals,
        TableFilterCompareOperator.NotEquals,
    };

    protected static Expression ConvertToActualTypeIfNecessary(Expression expr, Expression referenceExpr)
    {
        if (expr.Type == typeof(object))
        {
            if (referenceExpr is UnaryExpression unaryExpr && unaryExpr.Operand.Type != typeof(object))
            {
                // Assuming the actual type is known and can be determined at runtime
                var actualType = unaryExpr.Operand.Type;
                return Expression.Convert(expr, actualType);
            }
            else if (referenceExpr.Type != typeof(object))
            {
                var actualType = referenceExpr.Type;
                return Expression.Convert(expr, actualType);
            }
        }
        return expr;
    }

    public virtual Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
    {
        // Convert left and right expressions to their actual types if necessary
        leftExpr = ConvertToActualTypeIfNecessary(leftExpr, rightExpr);
        rightExpr = ConvertToActualTypeIfNecessary(rightExpr, rightExpr);

        return compareOperator switch
        {
            TableFilterCompareOperator.Equals => Expression.Equal(leftExpr, rightExpr),
            TableFilterCompareOperator.NotEquals => Expression.NotEqual(leftExpr, rightExpr),
            TableFilterCompareOperator.GreaterThan => Expression.GreaterThan(leftExpr, rightExpr),
            TableFilterCompareOperator.LessThan => Expression.LessThan(leftExpr, rightExpr),
            TableFilterCompareOperator.GreaterThanOrEquals => Expression.GreaterThanOrEqual(leftExpr, rightExpr),
            TableFilterCompareOperator.LessThanOrEquals => Expression.LessThanOrEqual(leftExpr, rightExpr),
            TableFilterCompareOperator.IsNull => Expression.Equal(leftExpr, rightExpr),
            TableFilterCompareOperator.IsNotNull => Expression.NotEqual(leftExpr, rightExpr),
            _ => throw new NotSupportedException($"{nameof(TableFilterCompareOperator)} {compareOperator} is not supported by {GetType().Name}!")
        };
    }
}
