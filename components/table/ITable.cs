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

        public TableLocale Locale { get; set; }

        internal void SelectionChanged();

        internal void Refresh();

        internal void ReloadAndInvokeChange();

        void SetSelection(string[] keys);

        internal int[] GetSelectedCacheKeys();

        void ReloadData();

        internal void HasFixLeft();

        internal void HasFixRight();

        internal void TableLayoutIsFixed();

        public int ExpandIconColumnIndex { get; set; }
    }
}
