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
    public class GuidFilterExpression : IFilterExpression
    {
        public TableFilterCompareOperator GetDefaultCompareOperator()
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
                default:
                    throw new InvalidOperationException($"The compare operator {compareOperator} is not supported for Guid type.");
            }
        }
    }
}
