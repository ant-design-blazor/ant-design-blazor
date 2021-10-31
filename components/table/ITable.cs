using AntDesign.TableModels;

namespace AntDesign
{
    public interface ITable
    {
        void ReloadData();

        void ReloadData(int? pageIndex, int? pageSize = null);

        QueryModel GetQueryModel();

        void SetSelection(string[] keys);

        internal TableLocale Locale { get; }

        internal ISelectionColumn Selection { get; set; }

        internal bool TreeMode { get; }

        internal int IndentSize { get; }

        internal string ScrollX { get; }

        internal string ScrollY { get; }

        internal int ScrollBarWidth { get; }

        internal int ExpandIconColumnIndex { get; }

        internal int TreeExpandIconColumnIndex { get; }

        internal bool HasExpandTemplate { get; }

        internal SortDirection[] SortDirections { get; }

        internal void SelectionChanged();

        internal void OnExpandChange(int cacheKey);

        internal void Refresh();

        internal void ReloadAndInvokeChange();

        internal int[] GetSelectedCacheKeys();

        internal void HasFixLeft();

        internal void HasFixRight();

        internal void TableLayoutIsFixed();

        internal void ColumnSorterChange(IFieldColumn column);

        internal bool RowExpandable(RowData rowData);

        internal void AddSummaryRow(SummaryRow summaryRow);

        internal void OnColumnInitialized();
    }
}
