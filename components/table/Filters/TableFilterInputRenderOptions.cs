// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Filters;

public readonly record struct TableFilterInputRenderOptions(TableFilter Filter, string PopupContainerSelector, string Format)
{
    public object Value { get => Filter.Value; set => Filter.Value = value; }
    public TableFilterCompareOperator FilterCompareOperator => Filter.FilterCompareOperator;
}
