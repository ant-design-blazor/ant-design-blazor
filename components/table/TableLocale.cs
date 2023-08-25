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

        public string Operator(TableFilterCompareOperator compareOperator)
            => compareOperator switch
            {
                TableFilterCompareOperator.Equals              => "Equal",
                TableFilterCompareOperator.Contains            => "Contains",
                TableFilterCompareOperator.StartsWith          => "Start With",
                TableFilterCompareOperator.EndsWith            => "End With",
                TableFilterCompareOperator.GreaterThan         => "Greater Than",
                TableFilterCompareOperator.LessThan            => "Less Than",
                TableFilterCompareOperator.GreaterThanOrEquals => "Greater Than Or Equals",
                TableFilterCompareOperator.LessThanOrEquals    => "Less Than Or Equals",
                TableFilterCompareOperator.NotEquals           => "Not Equal",
                TableFilterCompareOperator.IsNull              => "Is Null",
                TableFilterCompareOperator.IsNotNull           => "Is Not Null",
                TableFilterCompareOperator.NotContains         => "Not Contains",
                TableFilterCompareOperator.TheSameDateWith     => "The Same Date With",
                _                                              => throw new ArgumentOutOfRangeException(nameof(compareOperator), compareOperator, null)
            };
    }
}
