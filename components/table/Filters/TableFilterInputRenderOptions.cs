// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Filters;

public readonly record struct TableFilterInputRenderOptions(TableFilter Filter, Dictionary<string, object> Attributes, Expression InputRefExpression, Action<bool> FilterConfirm)
{
    public object Value { get => Filter.Value; set => Filter.Value = value; }
    public TableFilterCompareOperator FilterCompareOperator => Filter.FilterCompareOperator;

    public void Confirm()
    {
        FilterConfirm(false);
    }

    public object InputRef
    {
        get => InputRefExpression is MemberExpression memberExpression &&
                memberExpression.Member is FieldInfo fieldInfo &&
                memberExpression.Expression is ConstantExpression constantExpression &&
                constantExpression.Value is object constantValue ?
                fieldInfo.GetValue(constantValue) : null;
        set
        {
            if (InputRefExpression is MemberExpression memberExpression &&
                memberExpression.Member is FieldInfo fieldInfo &&
                memberExpression.Expression is ConstantExpression constantExpression &&
                constantExpression.Value is object constantValue)
                fieldInfo.SetValue(constantValue, value);
        }
    }
}
