using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class TableLocale
    {
        public string FilterTitle { get; set; }

        public string FilterConfirm { get; set; }

        public string FilterReset { get; set; }

        public string FilterEmptyText { get; set; }

        public string SelectAll { get; set; }

        public string SelectInvert { get; set; }

        public string SelectionAll { get; set; }

        public string SortTitle { get; set; }

        public string Expand { get; set; }

        public string Collapse { get; set; }

        public string TriggerDesc { get; set; }

        public string TriggerAsc { get; set; }

        public string CancelSort { get; set; }

        public FilterOptions FilterOptions { get; set; }
    }

    public class FilterOptions
    {
        public string True { get; set; }

        public string False { get; set; }

        public string And { get; set; }

        public string Or { get; set; }

        public new string Equals { get; set; }

        public string NotEquals { get; set; }

        public string Contains { get; set; }

        public string StartsWith { get; set; }

        public string EndsWith { get; set; }

        public string GreaterThan { get; set; }

        public string LessThan { get; set; }

        public string GreaterThanOrEquals { get; set; }

        public string LessThanOrEquals { get; set; }

        public string IsNull { get; set; }

        public string IsNotNull { get; set; }
    }
}
