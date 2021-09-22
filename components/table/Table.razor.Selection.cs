using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private IEnumerable<TItem> _outerSelectedRows;

        [Parameter]
        public IEnumerable<TItem> SelectedRows
        {
            get => _selectedRows;
            set
            {
                _outerSelectedRows = value;
                if (_dataSourceCache is null)
                {
                    _dataSourceCache = new Dictionary<int, RowData<TItem>>();
                    _selectedRows = _dataSourceCache.Values.Where(x => x.Selected).Select(x => x.Data);
                }
                if (value == _selectedRows) return;
                if (value != null && value.Any())
                {
                    _dataSourceCache.Values.ForEach(x => x.SetSelected(x.Data.IsIn(value)));
                }
                else
                {
                    _dataSourceCache.Values.ForEach(x => x.SetSelected(false));
                }
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private IEnumerable<TItem> _selectedRows;

        private void RowDataSelectedChanged(RowData rowData, bool selected)
        {
            if (SelectedRowsChanged.HasDelegate && _outerSelectedRows != _selectedRows)
            {
                _preventRender = true;
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
            if (keys == null || !keys.Any())
            {
                _dataSourceCache.Values.ForEach(x => x.Selected = false);
                if (_selection != null)
                {
                    _selection.StateHasChanged();
                }
                SelectionChanged();
                return;
            }

            if (_selection == null)
            {
                throw new InvalidOperationException("To use SetSelection method for a table, you should add a Selection component to the column definition.");
            }
            else
            {
                _selection.RowSelections.ForEach(x => x.RowData.Selected = x.Key.IsIn(keys));
                _selection.StateHasChanged();
                SelectionChanged();
            }
        }

        void ITable.SelectionChanged() => SelectionChanged();

        private void SelectionChanged()
        {
            if (SelectedRowsChanged.HasDelegate)
            {
                _preventRender = true;
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }
    }
}
