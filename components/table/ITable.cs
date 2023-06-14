using System;
using System.Collections.Generic;
using AntDesign.TableModels;

namespace AntDesign
{
    public interface ITable
    {
        void ReloadData();

        void ReloadData(int? pageIndex, int? pageSize = null);

        void ReloadData(QueryModel queryModel);

        void ResetData();

        QueryModel GetQueryModel();

        void SetSelection(string[] keys);

        void SelectAll();

        void UnselectAll();

        internal TableLocale Locale { get; }

        internal ISelectionColumn Selection { get; set; }

        internal bool TreeMode { get; }

        internal int IndentSize { get; }

        internal string ScrollX { get; }

        internal string ScrollY { get; }

        internal string ScrollBarWidth { get; }

        internal int ExpandIconColumnIndex { get; }

        internal int TreeExpandIconColumnIndex { get; }

        internal bool HasExpandTemplate { get; }

        internal SortDirection[] SortDirections { get; }

        internal bool AllSelected { get; }

        internal bool AnySelected { get; }

        internal bool HasHeaderTemplate { get; }

        internal bool HasRowTemplate { get; }

        internal Type ItemType { get; }

        internal void SelectionChanged();

        internal void OnExpandChange(RowData rowData);

        internal void Refresh();

        internal void ReloadAndInvokeChange();

        internal void HasFixLeft();

        internal void HasFixRight();

        internal void TableLayoutIsFixed();

        internal void ColumnSorterChange(IFieldColumn column);

        internal bool RowExpandable(RowData rowData);

        internal void AddSummaryRow(SummaryRow summaryRow);

        internal void OnColumnInitialized();
    }
}
