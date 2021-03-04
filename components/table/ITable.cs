using AntDesign.TableModels;

namespace AntDesign
{
    public interface ITable
    {
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

        public TableLocale Locale { get; set; }

        internal void SelectionChanged();

        internal void Refresh();

        internal void ReloadAndInvokeChange();

        void SetSelection(string[] keys);

        internal int[] GetSelectedCacheKeys();

        void ReloadData();

        QueryModel GetQueryModel();

        internal void HasFixLeft();

        internal void HasFixRight();

        internal void TableLayoutIsFixed();

        internal void ColumnSorterChange(IFieldColumn column);

        internal bool RowExpandable(RowData rowData);
    }
}
