// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters
{
    public abstract class DateFieldFilterType : BaseFieldFilterType
    {
        private static IEnumerable<TableFilterCompareOperator> _supportedCompareOperators = new[]
        {
            TableFilterCompareOperator.Equals,
            TableFilterCompareOperator.NotEquals,
            TableFilterCompareOperator.GreaterThan,
            TableFilterCompareOperator.LessThan,
            TableFilterCompareOperator.GreaterThanOrEquals,
            TableFilterCompareOperator.LessThanOrEquals
        };

        public DateFieldFilterType()
        {
            SupportedCompareOperators = _supportedCompareOperators;
        }

        protected virtual Expression GetNonNullFilterExpression(TableFilterCompareOperator compareOperator,
            Expression leftExpr, Expression rightExpr)
            => base.GetFilterExpression(compareOperator, leftExpr, rightExpr);

        public override sealed Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            switch (compareOperator)
            {
                case TableFilterCompareOperator.IsNull:
                    return Expression.Equal(leftExpr, rightExpr);

                case TableFilterCompareOperator.IsNotNull:
                    return Expression.NotEqual(leftExpr, rightExpr);
            }

            if (THelper.IsTypeNullable(leftExpr.Type))
            {
                Expression notNull = Expression.NotEqual(leftExpr, Expression.Constant(null));
                Expression isNull = Expression.Equal(leftExpr, Expression.Constant(null));
                leftExpr = Expression.Property(leftExpr, nameof(Nullable<DateTime>.Value));
                rightExpr = Expression.Property(rightExpr, nameof(Nullable<DateTime>.Value));

                return compareOperator switch
                {
                    TableFilterCompareOperator.NotEquals => Expression.OrElse(isNull, GetNonNullFilterExpression(compareOperator, leftExpr, rightExpr)),
                    _ => Expression.AndAlso(notNull, GetNonNullFilterExpression(compareOperator, leftExpr, rightExpr))
                };
            }

            return GetNonNullFilterExpression(compareOperator, leftExpr, rightExpr);
        }
    }

    public class DateTimeFieldFilterType : DateFieldFilterType
    {
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput => FilterInputs.Instance.DateTimeInput;

        protected override Expression GetNonNullFilterExpression(TableFilterCompareOperator compareOperator,
            Expression leftExpr, Expression rightExpr)
        {
            if (compareOperator != TableFilterCompareOperator.TheSameDateWith)
            {
                leftExpr = RemoveMilliseconds(leftExpr);
            }

            return compareOperator switch
            {
                TableFilterCompareOperator.TheSameDateWith => Expression.Equal(
                    Expression.Property(leftExpr, nameof(DateTime.Date)),
                    Expression.Property(rightExpr, nameof(DateTime.Date))),
                _ => base.GetNonNullFilterExpression(compareOperator, leftExpr, rightExpr)
            };
        }

        private static Expression RemoveMilliseconds(Expression dateTimeExpression)
        {
            return Expression.Call(dateTimeExpression, typeof(DateTime).GetMethod(nameof(DateTime.AddMilliseconds))!,
                Expression.Convert(
                    Expression.Subtract(Expression.Constant(0),
                        Expression.MakeMemberAccess(dateTimeExpression,
                            typeof(DateTime).GetMember(nameof(DateTime.Millisecond)).First())), typeof(double)));
        }
    }
}
