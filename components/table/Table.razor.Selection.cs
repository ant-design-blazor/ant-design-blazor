using System.Linq;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private ISelectionColumn _selection;

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        public void SetSelection(string[] keys)
        {
            _selection.SetSelection(keys);
        }

        void ITable.SelectionChanged()
        {
            foreach (var selection in _selection.RowSelections)
            {
                _dataSourceCache[selection.RowIndex].Selected = selection.Checked;
            }

            if (SelectedRowsChanged.HasDelegate)
            {
                var list = _dataSourceCache.Values.Where(x => x.Selected).Select(x => x.Data);
                SelectedRowsChanged.InvokeAsync(list);
            }
        }
    }
}
