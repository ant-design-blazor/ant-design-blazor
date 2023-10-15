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

        private static IEnumerable<TableFilterCompareOperator> _supportedCompareOperators = new[]
        {
            TableFilterCompareOperator.Equals,
            TableFilterCompareOperator.NotEquals,
            TableFilterCompareOperator.GreaterThan,
            TableFilterCompareOperator.LessThan,
            TableFilterCompareOperator.GreaterThanOrEquals,
            TableFilterCompareOperator.LessThanOrEquals
        };

        public NumberFieldFilterType()
        {
            SupportedCompareOperators = _supportedCompareOperators;
        }

        public override Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            return base.GetFilterExpression(compareOperator, leftExpr, Expression.Convert(rightExpr, leftExpr.Type));
        }
    }
}
