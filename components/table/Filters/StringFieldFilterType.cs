// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters
{
    public class StringFieldFilterType : BaseFieldFilterType
    {
        private static readonly MethodInfo _stringToLower = typeof(string).GetMethod(nameof(string.ToLower), Array.Empty<Type>());

        public override TableFilterCompareOperator DefaultCompareOperator => TableFilterCompareOperator.Contains;

        public override RenderFragment<TableFilterInputRenderOptions> FilterInput { get; } =
            FilterInputs.Instance.GetInput<string>();

        private static IEnumerable<TableFilterCompareOperator> _supportedCompareOperators = new[]
        {
            TableFilterCompareOperator.Equals,
            TableFilterCompareOperator.NotEquals,
            TableFilterCompareOperator.Contains,
            TableFilterCompareOperator.StartsWith,
            TableFilterCompareOperator.EndsWith,
        };

        public StringFieldFilterType()
        {
            SupportedCompareOperators = _supportedCompareOperators;
        }

        public override Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            MethodCallExpression lowerLeftExpr = Expression.Call(leftExpr, _stringToLower);
            MethodCallExpression lowerRightExpr = Expression.Call(rightExpr, _stringToLower);

            return compareOperator switch
            {
                TableFilterCompareOperator.IsNull or TableFilterCompareOperator.IsNotNull => base.GetFilterExpression(
                    compareOperator, leftExpr, rightExpr),
                TableFilterCompareOperator.Contains => NotNullAnd(GetMethodExpression(nameof(string.Contains),
                    lowerLeftExpr, lowerRightExpr)),
                TableFilterCompareOperator.StartsWith => NotNullAnd(GetMethodExpression(nameof(string.StartsWith),
                    lowerLeftExpr, lowerRightExpr)),
                TableFilterCompareOperator.EndsWith => NotNullAnd(GetMethodExpression(nameof(string.EndsWith),
                    lowerLeftExpr, lowerRightExpr)),
                TableFilterCompareOperator.NotEquals => Expression.OrElse(
                    Expression.Equal(leftExpr, Expression.Constant(null)),
                    base.GetFilterExpression(compareOperator, lowerLeftExpr, lowerRightExpr)),
                _ => NotNullAnd(base.GetFilterExpression(compareOperator, lowerLeftExpr, lowerRightExpr))
            };

            Expression NotNullAnd(Expression innerExpression)
                => Expression.AndAlso(Expression.NotEqual(leftExpr, Expression.Constant(null)), innerExpression);
        }

        private static Expression GetMethodExpression(string methodName, Expression leftExpr, Expression rightExpr)
        {
            MethodInfo mi = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            if (mi == null)
                throw new MissingMethodException("There is no method - " + methodName);
            return Expression.Call(leftExpr, mi, rightExpr);
        }
    }
}
