// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class AdvancedFilterLocale : TableLocale
    {
        private static readonly TableLocale DefaultTableLocale = new();

        public new FilterOptionsLocale FilterOptions { get; set; }

        public string MatchingPrefix { get; set; } = "Matching";

        public string MatchingSuffix { get; set; } = "of the conditions";

        public string AddCondition { get; set; } = "Add Condition";

        public string AddGroup { get; set; } = "Add Group";

        public string ClearAll { get; set; } = "Clear All";

        public string SelectField { get; set; } = "Select field";

        public static AdvancedFilterLocale FromTableLocale(TableLocale tableLocale)
        {
            var locale = new AdvancedFilterLocale
            {
                FilterTitle = tableLocale.FilterTitle,
                FilterConfirm = tableLocale.FilterConfirm,
                FilterReset = tableLocale.FilterReset,
                FilterEmptyText = tableLocale.FilterEmptyText,
                SelectAll = tableLocale.SelectAll,
                SelectInvert = tableLocale.SelectInvert,
                SelectionAll = tableLocale.SelectionAll,
                SortTitle = tableLocale.SortTitle,
                Expand = tableLocale.Expand,
                Collapse = tableLocale.Collapse,
                TriggerDesc = tableLocale.TriggerDesc,
                TriggerAsc = tableLocale.TriggerAsc,
                CancelSort = tableLocale.CancelSort,
                FilterOptions = tableLocale.FilterOptions,
            };

            return locale;
        }

        internal void UseTableLocaleFallback(TableLocale tableLocale)
        {
            if (tableLocale == null)
                return;

            if (FilterTitle == DefaultTableLocale.FilterTitle)
                FilterTitle = tableLocale.FilterTitle;

            if (FilterConfirm == DefaultTableLocale.FilterConfirm)
                FilterConfirm = tableLocale.FilterConfirm;

            if (FilterReset == DefaultTableLocale.FilterReset)
                FilterReset = tableLocale.FilterReset;

            if (FilterEmptyText == DefaultTableLocale.FilterEmptyText)
                FilterEmptyText = tableLocale.FilterEmptyText;

            if (SelectAll == DefaultTableLocale.SelectAll)
                SelectAll = tableLocale.SelectAll;

            if (SelectInvert == DefaultTableLocale.SelectInvert)
                SelectInvert = tableLocale.SelectInvert;

            if (SelectionAll == DefaultTableLocale.SelectionAll)
                SelectionAll = tableLocale.SelectionAll;

            if (SortTitle == DefaultTableLocale.SortTitle)
                SortTitle = tableLocale.SortTitle;

            if (Expand == DefaultTableLocale.Expand)
                Expand = tableLocale.Expand;

            if (Collapse == DefaultTableLocale.Collapse)
                Collapse = tableLocale.Collapse;

            if (TriggerDesc == DefaultTableLocale.TriggerDesc)
                TriggerDesc = tableLocale.TriggerDesc;

            if (TriggerAsc == DefaultTableLocale.TriggerAsc)
                TriggerAsc = tableLocale.TriggerAsc;

            if (CancelSort == DefaultTableLocale.CancelSort)
                CancelSort = tableLocale.CancelSort;

            if (FilterOptions == null)
                FilterOptions = tableLocale.FilterOptions;
        }

    }
}
