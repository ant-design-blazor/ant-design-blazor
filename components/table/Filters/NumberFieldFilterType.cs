// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters
{
    public class NumberFieldFilterType<T> : BaseFieldFilterType where T : struct
    {
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput { get; } =
            FilterInputs.Instance.GetNumberInput<T>();

        public override IEnumerable<TableFilterCompareOperator> GetSupportedCompareOperators()
        {
            foreach (TableFilterCompareOperator baseCompareOperator in base.GetSupportedCompareOperators())
                yield return baseCompareOperator;

            yield return TableFilterCompareOperator.GreaterThan;
            yield return TableFilterCompareOperator.LessThan;
            yield return TableFilterCompareOperator.GreaterThanOrEquals;
            yield return TableFilterCompareOperator.LessThanOrEquals;
        }

        public override Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            return base.GetFilterExpression(compareOperator, leftExpr, Expression.Convert(rightExpr, leftExpr.Type));
        }
    }
}
