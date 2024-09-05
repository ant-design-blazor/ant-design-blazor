// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET5_0_OR_GREATER
using System.Text.Json.Serialization;
#endif

namespace AntDesign
{
    /// <summary>
    /// Filter item definition
    /// </summary>
    public class TableFilter
    {
        /// <summary>
        /// The display text of the filter item
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The value of the filter item
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Whether the filter item is selected
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// The compare operators supported by the filter item
        /// </summary>
        public TableFilterCompareOperator FilterCompareOperator { get; internal set; }

        /// <summary>
        /// The condition of the filter item
        /// </summary>
        public TableFilterCondition FilterCondition { get; internal set; }

        internal void SelectValue(bool selected)
        {
            this.Selected = selected;
        }

        public TableFilter()
        {
        }
#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public TableFilter(string text, object value, bool selected, TableFilterCompareOperator filterCompareOperator, TableFilterCondition filterCondition)
        {
            this.Text = text;
            this.Value = value;
            this.Selected = selected;
            this.FilterCompareOperator = filterCompareOperator;
            this.FilterCondition = filterCondition;
        }
    }

    public class TableFilter<TValue> : TableFilter
    {
        public TableFilter()
        {
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public TableFilter(string text, object value, bool selected, TableFilterCompareOperator filterCompareOperator, TableFilterCondition filterCondition) : base(text, value, selected, filterCompareOperator, filterCondition)
        {
        }

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
        IsNotNull = 12,
        NotContains = 13,
        TheSameDateWith = 14,
        Between = 15
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
