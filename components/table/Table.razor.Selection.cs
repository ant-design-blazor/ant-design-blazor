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
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private HashSet<TItem> _selectedRows = new();
        private bool _preventRowDataTriggerSelectedRowsChanged;

        private void RowDataSelectedChanged(RowData<TItem> rowData, bool selected)
        {
            if (!RowSelectable(rowData.Data))
            {
                rowData.SetSelected(!selected);
                return;
            }
            if (selected)
            {
                _selectedRows.Add(rowData.Data);
            }
            else
            {
                _selectedRows.Remove(rowData.Data);
            }
            if (!_preventRowDataTriggerSelectedRowsChanged)
            {
                SelectionChanged();
            }
        }

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        bool ITable.AllSelected => _selectedRows.Count != 0 && _selectedRows.Count == GetAllItemsByTopLevelItems(_showItems, true).Count();

        bool ITable.AnySelected => _selectedRows.Count > 0;

        public void SelectAll()
        {
            _selectedRows = GetAllItemsByTopLevelItems(_showItems, true).ToHashSet();
            foreach (RowData<TItem> rowData in _dataSourceCache.Values)
            {
                rowData.SetSelected(true);
            }

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        private void ClearSelectedRows()
        {
            _selectedRows.Clear();
            foreach (RowData<TItem> rowData in _dataSourceCache.Values)
            {
                rowData.SetSelected(false);
            }
        }

        public void UnselectAll()
        {
            ClearSelectedRows();

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        /// <summary>
        /// Please use <see cref="SetSelection(IEnumerable{TItem})"/> instead if possible,
        /// as this method won't correctly select items from invisible rows when virtualization is enabled.
        /// </summary>
        public void SetSelection(ICollection<string> keys)
        {
            if (_selection == null)
            {
                throw new InvalidOperationException("To use SetSelection method for a table, you should add a Selection component to the column definition.");
            }

            ClearSelectedRows();
            if (keys?.Count > 0)
            {
                _preventRowDataTriggerSelectedRowsChanged = true;
                _selection.RowSelections.ForEach(x => x.RowData.Selected = keys.Contains(x.Key));
                _preventRowDataTriggerSelectedRowsChanged = false;
            }

            _selection.StateHasChanged();
            SelectionChanged();
        }

        // Only select the given row (radio)
        void ITable.SetSelection(ISelectionColumn selectItem)
        {
            ClearSelectedRows();

            _preventRowDataTriggerSelectedRowsChanged = true;
            selectItem.RowData.Selected = true;
            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection.StateHasChanged();
            SelectionChanged();
        }

        private void SelectItem(TItem item)
        {
            if (!RowSelectable(item))
                return;

            _selectedRows.Add(item);
            if (_dataSourceCache.TryGetValue(item, out var rowData))
            {
                rowData.SetSelected(true);
            }
        }

        public void SetSelection(IEnumerable<TItem> items)
        {
            if (_selection == null)
            {
                throw new InvalidOperationException("To use SetSelection method for a table, you should add a Selection component to the column definition.");
            }

            ClearSelectedRows();
            items?.ForEach(SelectItem);

            _selection.StateHasChanged();
            SelectionChanged();
        }

        public void SetSelection(TItem item)
        {
            if (_selection == null)
            {
                throw new InvalidOperationException("To use SetSelection method for a table, you should add a Selection component to the column definition.");
            }

            ClearSelectedRows();
            if (item != null)
            {
                SelectItem(item);
            }

            _selection.StateHasChanged();
            SelectionChanged();
        }

        void ITable.SelectionChanged() => SelectionChanged();

        private void SelectionChanged()
        {
            if (SelectedRowsChanged.HasDelegate)
            {
                _preventRender = true;
                _outerSelectedRows = _selectedRows;
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }
    }
}
