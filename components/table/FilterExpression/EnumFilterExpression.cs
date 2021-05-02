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
    public class EnumFilterExpression : IFilterExpression
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
                case TableFilterCompareOperator.HasFlag:
                    {
                        MethodInfo mi = typeof(Enum).GetMethod(nameof(Enum.HasFlag));
                        if (leftExpr.Type.IsGenericType)
                        {
                            Expression notNull = Expression.NotEqual(leftExpr, Expression.Constant(null));
                            leftExpr = Expression.MakeMemberAccess(leftExpr, leftExpr.Type.GetMember("Value")[0]);
                            rightExpr = Expression.MakeMemberAccess(rightExpr, rightExpr.Type.GetMember("Value")[0]);
                            rightExpr = Expression.Convert(rightExpr, typeof(Enum));
                            return Expression.AndAlso(notNull, Expression.Call(leftExpr, mi, rightExpr));
                        }
                        rightExpr = Expression.Convert(rightExpr, typeof(Enum));
                        return Expression.Call(leftExpr, mi, rightExpr);
                    }
                case TableFilterCompareOperator.NotHasFlag:
                    {
                        MethodInfo mi = typeof(Enum).GetMethod(nameof(Enum.HasFlag));
                        if (leftExpr.Type.IsGenericType)
                        {
                            Expression notNull = Expression.NotEqual(leftExpr, Expression.Constant(null));
                            leftExpr = Expression.MakeMemberAccess(leftExpr, leftExpr.Type.GetMember("Value")[0]);
                            rightExpr = Expression.MakeMemberAccess(rightExpr, rightExpr.Type.GetMember("Value")[0]);
                            rightExpr = Expression.Convert(rightExpr, typeof(Enum));
                            return Expression.Equal(Expression.AndAlso(notNull, Expression.Call(leftExpr, mi, rightExpr)), Expression.Constant(false, typeof(bool)));
                        }
                        rightExpr = Expression.Convert(rightExpr, typeof(Enum));
                        return Expression.Equal(Expression.Call(leftExpr, mi, rightExpr), Expression.Constant(false, typeof(bool)));
                    }
            }
            throw new InvalidOperationException();
        }
    }
}
