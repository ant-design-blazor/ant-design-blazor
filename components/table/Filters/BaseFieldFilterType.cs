// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters;

public abstract class BaseFieldFilterType : IFieldFilterType
{
    public virtual TableFilterCompareOperator DefaultCompareOperator => TableFilterCompareOperator.Equals;

    public abstract RenderFragment<TableFilterInputRenderOptions> FilterInput { get; }

    public virtual bool SupportsCompareOperator(TableFilterCompareOperator compareOperator)
        => compareOperator is TableFilterCompareOperator.Equals or TableFilterCompareOperator.NotEquals;

    public virtual Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr,
        Expression rightExpr)
        => compareOperator switch
        {
            TableFilterCompareOperator.Equals => Expression.Equal(leftExpr, rightExpr),
            TableFilterCompareOperator.NotEquals => Expression.NotEqual(leftExpr, rightExpr),
            TableFilterCompareOperator.GreaterThan => Expression.GreaterThan(leftExpr, rightExpr),
            TableFilterCompareOperator.LessThan => Expression.LessThan(leftExpr, rightExpr),
            TableFilterCompareOperator.GreaterThanOrEquals => Expression.GreaterThanOrEqual(leftExpr, rightExpr),
            TableFilterCompareOperator.LessThanOrEquals => Expression.LessThanOrEqual(leftExpr, rightExpr),
            TableFilterCompareOperator.IsNull => Expression.Equal(leftExpr, rightExpr),
            TableFilterCompareOperator.IsNotNull => Expression.NotEqual(leftExpr, rightExpr),
            _ => throw new ArgumentOutOfRangeException(nameof(compareOperator), compareOperator, null)
        };
}
