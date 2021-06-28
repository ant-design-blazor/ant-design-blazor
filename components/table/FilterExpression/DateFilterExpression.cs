// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AntDesign.FilterExpression
{
    public class DateFilterExpression : IFilterExpression
    {
        public TableFilterCompareOperator GetDefaultCampareOperator()
        {
            return TableFilterCompareOperator.Equals;
        }
        public Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
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
            }
            throw new InvalidOperationException();
        }
    }
}
