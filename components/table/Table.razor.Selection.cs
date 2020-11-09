using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        [Parameter]
        public IEnumerable<TItem> SelectedRows
        {
            get => _selectedRows;
            set
            {
                if (value != null && value.Any())
                {
                    _dataSourceCache.Values.ForEach(x => x.Selected = x.Data.IsIn(value));
                }
                else if (_selectedRows != null)
                {
                    _dataSourceCache.Values.ForEach(x => x.Selected = false);
                }

                _selectedRows = value;

                StateHasChanged();
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private IEnumerable<TItem> _selectedRows;

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        public void SetSelection(string[] keys)
        {
            _selection.SetSelection(keys);
        }

        void ITable.SelectionChanged() => SelectionChanged();

        private void SelectionChanged()
        {
            foreach (var selection in _selection.RowSelections)
            {
                _dataSourceCache[selection.RowData.CacheKey].Selected = selection.Checked;
            }

            if (SelectedRowsChanged.HasDelegate)
            {
                _selectedRows = _dataSourceCache.Values.Where(x => x.Selected).Select(x => x.Data);
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }
    }
}
