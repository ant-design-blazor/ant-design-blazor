// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters
{
    public interface IFieldFilterType
    {
        TableFilterCompareOperator DefaultCompareOperator { get; }
        RenderFragment<TableFilterInputRenderOptions> FilterInput { get; }
        IEnumerable<TableFilterCompareOperator> SupportedCompareOperators { get; set; }

        Expression GetFilterExpression(TableFilterCompareOperator compareOperator, Expression leftExpr, Expression rightExpr);
    }
}
