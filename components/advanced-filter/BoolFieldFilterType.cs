// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters
{
    public class BoolFieldFilterType : BaseFieldFilterType
    {
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput { get; } =
            FilterInputs.Instance.GetBoolInput();

        private static readonly IEnumerable<TableFilterCompareOperator> _supportedCompareOperators = new[]
        {
            TableFilterCompareOperator.Equals,
            TableFilterCompareOperator.NotEquals,
            TableFilterCompareOperator.IsNull,
            TableFilterCompareOperator.IsNotNull,
        };

        public BoolFieldFilterType()
        {
            SupportedCompareOperators = _supportedCompareOperators;
        }

        public override Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr)
        {
            if (compareOperator == TableFilterCompareOperator.IsNull || compareOperator == TableFilterCompareOperator.IsNotNull)
            {
                return base.GetFilterExpression(compareOperator, leftExpr, Expression.Constant(null));
            }

            rightExpr = Expression.Convert(rightExpr, leftExpr.Type);
            return base.GetFilterExpression(compareOperator, leftExpr, rightExpr);
        }
    }
}
