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
            TableFilterCompareOperator.LessThanOrEquals,
            TableFilterCompareOperator.Between
        };

        public DateFieldFilterType()
        {
            SupportedCompareOperators = _supportedCompareOperators;
        }

        protected virtual Expression GetNonNullFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            if (compareOperator == TableFilterCompareOperator.Between)
            {
                return GetScalarBetweenExpression(leftExpr, rightExpr);
            }

            return base.GetFilterExpression(compareOperator, leftExpr, rightExpr);
        }

        internal static Expression GetScalarBetweenExpression(Expression leftExpr, Expression rightExpr,
            Func<Expression, Expression> prepareValue = null)
        {
            Expression notNull = null;

            // The shared filter conversion uses the array type. Only the built-in comparison expects a scalar field.
            if (leftExpr is UnaryExpression { NodeType: ExpressionType.Convert } conversion
                && conversion.Operand.Type == typeof(object) && leftExpr.Type == rightExpr.Type && rightExpr.Type.IsArray)
            {
                leftExpr = Expression.Convert(conversion.Operand, rightExpr.Type.GetElementType());
                if (THelper.IsTypeNullable(leftExpr.Type))
                {
                    notNull = Expression.NotEqual(leftExpr, Expression.Constant(null));
                    leftExpr = Expression.Property(leftExpr, nameof(Nullable<DateTime>.Value));
                }
            }

            leftExpr = prepareValue?.Invoke(leftExpr) ?? leftExpr;
            var lowerBound = GetRangeEndpoint(rightExpr, 0);
            var upperBound = GetRangeEndpoint(rightExpr, 1);
            var between = Expression.AndAlso(
                Expression.GreaterThanOrEqual(leftExpr, lowerBound),
                Expression.LessThanOrEqual(leftExpr, upperBound));

            return notNull is null ? between : Expression.AndAlso(notNull, between);
        }

        private static Expression GetRangeEndpoint(Expression range, int index)
        {
            var endpoint = Expression.ArrayIndex(range, Expression.Constant(index));
            return THelper.IsTypeNullable(endpoint.Type)
                ? Expression.Property(endpoint, nameof(Nullable<DateTime>.Value))
                : endpoint;
        }

        public override sealed Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            leftExpr = ConvertToActualTypeIfNecessary(leftExpr, rightExpr);
            rightExpr = ConvertToActualTypeIfNecessary(rightExpr, rightExpr);

            switch (compareOperator)
            {
                case TableFilterCompareOperator.IsNull:
                    return Expression.Equal(leftExpr, rightExpr);

                case TableFilterCompareOperator.IsNotNull:
                    return Expression.NotEqual(leftExpr, rightExpr);
            }

            if (THelper.IsTypeNullable(rightExpr.Type))
            {
                rightExpr = Expression.Property(rightExpr, nameof(Nullable<DateTime>.Value));
            }

            if (THelper.IsTypeNullable(leftExpr.Type))
            {
                Expression notNull = Expression.NotEqual(leftExpr, Expression.Constant(null));
                Expression isNull = Expression.Equal(leftExpr, Expression.Constant(null));
                leftExpr = Expression.Property(leftExpr, nameof(Nullable<DateTime>.Value));

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
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput => filter =>
        {
            InputAttributes.TryAdd("ShowTime", (OneOf.OneOf<bool, string>)(filter.FilterCompareOperator != TableFilterCompareOperator.TheSameDateWith));
            return FilterInputs.Instance.GetDatePicker<DateTime?>()(filter);
        };

        public DateTimeFieldFilterType()
        {
            SupportedCompareOperators = [.. base.SupportedCompareOperators, TableFilterCompareOperator.TheSameDateWith];
        }

        protected override Expression GetNonNullFilterExpression(TableFilterCompareOperator compareOperator,
            Expression leftExpr, Expression rightExpr)
        {
            return compareOperator switch
            {
                TableFilterCompareOperator.Between => GetScalarBetweenExpression(leftExpr, rightExpr, RemoveMilliseconds),
                TableFilterCompareOperator.TheSameDateWith => Expression.Equal(
                    Expression.Property(leftExpr, nameof(DateTime.Date)),
                    Expression.Property(rightExpr, nameof(DateTime.Date))),
                _ => base.GetNonNullFilterExpression(compareOperator, RemoveMilliseconds(leftExpr), rightExpr)
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

    public class DateTimeFieldFilterType<TData> : DateFieldFilterType
    {
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput => FilterInputs.Instance.GetDatePicker<TData>();
    }

    public class TimeOnlyFieldFilterType<TData> : DateFieldFilterType
    {
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput => FilterInputs.Instance.GetDatePicker<TData>();

        public TimeOnlyFieldFilterType()
        {
            InputAttributes.Add(nameof(DatePicker<TData>.Picker), DatePickerType.Time);
        }
    }
}
