// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AntDesign
{
    public class TableFilter
    {
        public string Text { get; set; }

        public object Value { get; set; }

        public bool Selected { get; set; }

        internal TableFilterCompareOperator FilterCompareOperator { get; set; }

        internal TableFilterCondition FilterCondition { get; set; }

        internal void SelectValue(bool selected)
        {
            this.Selected = selected;
        }
    }

    public class TableFilter<TValue> : TableFilter
    {
        new public TValue Value { get => (TValue)(base.Value ?? default(TValue)); set => base.Value = value; }
    }

    public enum TableFilterCompareOperator
    {
        Equals = 1,
        Contains = 2,
        StartsWith = 3,
        EndsWith = 4,
        GreaterThan = 5,
        LessThan = 6,
        GreaterThanOrEquals = 7,
        LessThanOrEquals = 8,
        Condition = 9,
        NotEquals = 10,
        IsNull = 11,
        IsNotNull = 12
    }

    public enum TableFilterCondition
    {
        And = 1,
        Or = 2
    }

    public enum TableFilterType
    {
        List = 1,
        FieldType = 2
    }
}
