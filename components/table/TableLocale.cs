using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class TableLocale
    {
        public string FilterTitle { get; set; } = "Filter menu";

        public string FilterConfirm { get; set; } = "OK";

        public string FilterReset { get; set; } = "Reset";

        public string FilterEmptyText { get; set; } = "";

        public string SelectAll { get; set; } = "Select current page";

        public string SelectInvert { get; set; } = "Invert current page";

        public string SelectionAll { get; set; } = "Select all data";

        public string SortTitle { get; set; } = "Sort";

        public string Expand { get; set; } = "Expand row";

        public string Collapse { get; set; } = "Collapse row";

        public string TriggerDesc { get; set; } = "Click sort by descend";

        public string TriggerAsc { get; set; } = "Click sort by ascend";

        public string CancelSort { get; set; } = "Click to cancel sort";

        public FilterOptionsLocale FilterOptions { get; set; } = new();
    }

    public class FilterOptionsLocale
    {
        public string True { get; set; } = "True";

        public string False { get; set; } = "False";

        public string And { get; set; } = "And";

        public string Or { get; set; } = "Or";

        public new string Equals { get; set; } = "Equal";

        public string NotEquals { get; set; } = "Not Equal";

        public string Contains { get; set; } = "Contains";

        public string NotContains { get; set; } = "Not Contains";

        public string StartsWith { get; set; } = "Start With";

        public string EndsWith { get; set; } = "End With";

        public string GreaterThan { get; set; } = "Greater Than";

        public string LessThan { get; set; } = "Less Than";

        public string GreaterThanOrEquals { get; set; } = "Greater Than Or Equals";

        public string LessThanOrEquals { get; set; } = "Less Than Or Equals";

        public string IsNull { get; set; } = "Is Null";

        public string IsNotNull { get; set; } = "Is Not Null";

        public string TheSameDateWith { get; set; } = "The Same Date With";

        public string Operator(TableFilterCompareOperator compareOperator)
            => compareOperator switch
            {
                TableFilterCompareOperator.Equals => Equals,
                TableFilterCompareOperator.Contains => Contains,
                TableFilterCompareOperator.StartsWith => StartsWith,
                TableFilterCompareOperator.EndsWith => EndsWith,
                TableFilterCompareOperator.GreaterThan => GreaterThan,
                TableFilterCompareOperator.LessThan => LessThan,
                TableFilterCompareOperator.GreaterThanOrEquals => GreaterThanOrEquals,
                TableFilterCompareOperator.LessThanOrEquals => LessThanOrEquals,
                TableFilterCompareOperator.NotEquals => NotEquals,
                TableFilterCompareOperator.IsNull => IsNull,
                TableFilterCompareOperator.IsNotNull => IsNotNull,
                TableFilterCompareOperator.NotContains => NotContains,
                TableFilterCompareOperator.TheSameDateWith => TheSameDateWith,
                _ => throw new ArgumentOutOfRangeException(nameof(compareOperator), compareOperator, null)
            };
    }
}
