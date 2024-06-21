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
    public class EnumFieldFilterType<T> : BaseFieldFilterType
    {
        private static readonly MethodInfo _enumHasFlag = typeof(Enum).GetMethod(nameof(Enum.HasFlag));

        public override RenderFragment<TableFilterInputRenderOptions> FilterInput { get; } = FilterInputs.Instance.GetEnumInput<T>();

        private static IEnumerable<TableFilterCompareOperator> _supportedCompareOperators = new[]
        {
            TableFilterCompareOperator.Equals,
            TableFilterCompareOperator.NotEquals,
            TableFilterCompareOperator.Contains,
            TableFilterCompareOperator.NotContains,
        };

        public EnumFieldFilterType()
        {
            SupportedCompareOperators = _supportedCompareOperators;
        }

        public override Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression fieldValueExpr, Expression filterValueExpr)
        {
            Expression ContainsExpression()
            {
                filterValueExpr = Expression.Convert(filterValueExpr, typeof(Enum));
                return Expression.Call(fieldValueExpr, _enumHasFlag, filterValueExpr);
            }

            Expression GetCompareExpr()
            {
                return compareOperator switch
                {
                    TableFilterCompareOperator.Contains => ContainsExpression(),
                    TableFilterCompareOperator.NotContains => Expression.Not(ContainsExpression()),
                    _ => base.GetFilterExpression(compareOperator, fieldValueExpr, filterValueExpr)
                };
            }

            if (THelper.IsTypeNullable(fieldValueExpr.Type))
            {
                Expression notNull = Expression.NotEqual(fieldValueExpr, Expression.Constant(null));
                fieldValueExpr = Expression.MakeMemberAccess(fieldValueExpr, fieldValueExpr.Type.GetMember(nameof(Nullable<int>.Value))[0]);
                return Expression.AndAlso(notNull, GetCompareExpr());
            }

            return GetCompareExpr();
        }
    }
}
