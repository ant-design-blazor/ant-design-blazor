using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;
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
                _dataSourceCache ??= new Dictionary<int, RowData<TItem>>();

                if (value != null && value.Any())
                {
                    _dataSourceCache.Values.ForEach(x => x.Selected = x.Data.IsIn(value));
                }
                else if (_selectedRows != null)
                {
                    _dataSourceCache.Values.ForEach(x => x.Selected = false);
                }

                _selectedRows = _dataSourceCache.Values.Where(x => x.Selected).Select(x => x.Data);
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private IEnumerable<TItem> _selectedRows;

        private void RowDataSelectedChanged(RowData rowData, bool selected)
        {
            _selectedRows = _dataSourceCache.Values.Where(x => x.Selected).Select(x => x.Data);
            if (SelectedRowsChanged.HasDelegate)
            {
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        public void SetSelection(string[] keys)
        {
            if (_selection == null)
            {
                if (keys == null || !keys.Any())
                {
                    _dataSourceCache.Values.ForEach(x => x.Selected = false);
                    StateHasChanged();
                }
                else
                {
                    throw new NotSupportedException("To use SetSelection method for a table, you should add a Selection component to the column definition.");
                }
            }
            else
            {
                _selection.SetSelection(keys);
            }
        }

        void ITable.SelectionChanged() => SelectionChanged();

        private void SelectionChanged()
        {
            if (SelectedRowsChanged.HasDelegate)
            {
                _selectedRows = _dataSourceCache.Values.Where(x => x.Selected).Select(x => x.Data);
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
            else
            {
                StateHasChanged();
            }
        }
    }
}
