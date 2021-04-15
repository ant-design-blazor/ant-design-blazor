// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AntDesign.FilterExpression
{
    public class StringFilterExpression : IFilterExpression
    {
        public TableFilterCompareOperator GetDefaultCampareOperator()
        {
            return TableFilterCompareOperator.Contains;
        }

        public Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            return GetCaseInsensitiveСomparation(compareOperator, leftExpr, rightExpr);
        }

        private Expression GetCaseInsensitiveСomparation(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            MethodInfo miLower = typeof(string).GetMethod("ToLower", Array.Empty<Type>());
            Expression notNull = Expression.NotEqual(leftExpr, Expression.Constant(null));
            MethodCallExpression lowerLeftExpr = Expression.Call(leftExpr, miLower);
            MethodCallExpression lowerRightExpr = Expression.Call(rightExpr, miLower);

            switch (compareOperator)
            {
                case TableFilterCompareOperator.IsNull:
                    return Expression.Equal(leftExpr, rightExpr);
                case TableFilterCompareOperator.Equals:
                    return Expression.AndAlso(notNull, Expression.Equal(lowerLeftExpr, lowerRightExpr));
                case TableFilterCompareOperator.IsNotNull:
                    return Expression.NotEqual(leftExpr, rightExpr);
                case TableFilterCompareOperator.NotEquals:
                    return Expression.AndAlso(notNull, Expression.NotEqual(lowerLeftExpr,  lowerRightExpr));
                default:
                    string methodName = Enum.GetName(typeof(TableFilterCompareOperator), compareOperator);
                    MethodInfo mi = typeof(string).GetMethod(methodName, new[] { typeof(string) });
                    if (mi == null)
                        throw new MissingMethodException("There is no method - " + methodName);
                    return Expression.AndAlso(notNull, Expression.Call(lowerLeftExpr, mi, lowerRightExpr));
            }
        }
    }
}
