// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign.FilterExpression
{
    public class DateFilterExpression : IFilterExpression
    {
        public TableFilterCompareOperator GetDefaultCompareOperator()
        {
            return TableFilterCompareOperator.Equals;
        }
        public Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            leftExpr = RemoveMilliseconds(leftExpr);
            rightExpr = RemoveMilliseconds(rightExpr);
            switch (compareOperator)
            {
                case TableFilterCompareOperator.IsNull:
                case TableFilterCompareOperator.Equals:
                    return Expression.Equal(leftExpr, rightExpr);
                case TableFilterCompareOperator.IsNotNull:
                case TableFilterCompareOperator.NotEquals:
                    return Expression.NotEqual(leftExpr, rightExpr);
                case TableFilterCompareOperator.GreaterThan:
                    return Expression.GreaterThan(leftExpr, rightExpr);
                case TableFilterCompareOperator.GreaterThanOrEquals:
                    return Expression.GreaterThanOrEqual(leftExpr, rightExpr);
                case TableFilterCompareOperator.LessThan:
                    return Expression.LessThan(leftExpr, rightExpr);
                case TableFilterCompareOperator.LessThanOrEquals:
                    return Expression.LessThanOrEqual(leftExpr, rightExpr);
                case TableFilterCompareOperator.TheSameDateWith:
                    return Expression.Equal(Expression.Property(leftExpr, "Date"),
                        Expression.Property(rightExpr, "Date"));
            }
            throw new InvalidOperationException();
        }

        private static Expression RemoveMilliseconds(Expression dateTimeExpression)
        {
            return Expression.Call(dateTimeExpression, typeof(DateTime).GetMethod("AddMilliseconds")!,
                Expression.Convert(
                    Expression.Subtract(Expression.Constant(0),
                        Expression.MakeMemberAccess(dateTimeExpression,
                            typeof(DateTime).GetMember("Millisecond").First())), typeof(double)));
        }
    }
}
