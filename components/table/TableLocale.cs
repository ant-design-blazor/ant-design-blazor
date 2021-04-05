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


        public string TrueFilterOption { get; set; } = "True";

        public string FalseFilterOption { get; set; } = "False";

        public string AndFilterCondition { get; set; } = "And";

        public string OrFilterCondition { get; set; } = "Or";

        public string EqualCompareOperator { get; set; } = "Equal";

        public string NotEqualCompareOperator { get; set; } = "Not Equal";

        public string ContainsCompareOperator { get; set; } = "Contains";

        public string StartWithCompareOperator { get; set; } = "Start With";

        public string EndWithCompareOperator { get; set; } = "End With";

        public string GreaterThanCompareOperator { get; set; } = "Greater Than";

        public string LessThanCompareOperator { get; set; } = "Less Than";

        public string GreaterThanOrEqualCompareOperator { get; set; } = "Greater Than Or Equals";

        public string LessThanOrEqualCompareOperator { get; set; } = "Less Than Or Equals";

        public string IsNullCompareOperator { get; set; } = "Is Null";

        public string IsNotNullCompareOperator { get; set; } = "Is Not Null";
    }
}
