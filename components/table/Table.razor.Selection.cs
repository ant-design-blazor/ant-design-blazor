using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private IEnumerable<TItem> _outerSelectedRows;

        /// <summary>
        /// Selected rows across pages
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> SelectedRows
        {
            get => _selectedRows;
            set
            {
                _outerSelectedRows = value ?? [];
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private readonly HashSet<TItem> _selectedRows;
        private bool _preventRowDataTriggerSelectedRowsChanged;

        internal void DataItemSelectedChanged(TableDataItem<TItem> dataItem, bool selected)
        {
            if (selected)
            {
                _selectedRows.Add(dataItem.Data);
            }
            else
            {
                _selectedRows.Remove(dataItem.Data);
            }

            SelectionChanged();
            _selection?.StateHasChanged();
        }

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        bool ITable.AllSelected => _selectedRows.Any() && _dataSourceCache.Values.All(x => x.Disabled || x.Selected);

        bool ITable.AnySelected => _dataSourceCache.Values.Any(x => !x.Disabled && x.Selected);

        /// <summary>
        /// Select all rows of current page
        /// </summary>
        public void SelectAll()
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            foreach (var select in _dataSourceCache.Values)
            {
                if (select.Disabled)
                    continue;

                select.SetSelected(true);
                _selectedRows.Add(select.Data);
            }
            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        /// <summary>
        /// Unselect all rows of current page
        /// </summary>
        public void UnselectAll()
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            ClearSelectedRows();

            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        /// <summary>
        /// Please use <see cref="SetSelection(IEnumerable{TItem})"/> instead if possible,
        /// as this method won't correctly select items from invisible rows when virtualization is enabled.
        /// </summary>
        public void SetSelection(ICollection<string> keys)
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            ClearSelectedRows();
            if (keys?.Count > 0)
            {
                _selection?.RowSelections.ForEach(x => x.RowData.SetSelected(keys.Contains(x.Key), x.CheckStrictly));
            }

            _preventRowDataTriggerSelectedRowsChanged = false;
            _selection?.StateHasChanged();
            SelectionChanged();
        }

        // Only select the given row (for radio selection)
        void ITable.SetSelection(ISelectionColumn selectItem)
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            ClearSelectedRows();
            selectItem.RowData.SetSelected(true, selectItem.CheckStrictly);

            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        private void SelectItem(TItem item)
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            _selectedRows.Add(item);
            if (_dataSourceCache.TryGetValue(GetHashCode(item), out var rowData))
            {
                rowData.SetSelected(true);
            }

            _preventRowDataTriggerSelectedRowsChanged = false;
        }


        /// <summary>
        /// Set all selected items
        /// </summary>
        /// <param name="items"></param>
        public void SetSelection(IEnumerable<TItem> items)
        {
            if (items.SequenceEqual(_selectedRows, this))
                return;

            if (items is not null and not IReadOnlyCollection<TItem>)
                // Ensure that the given enumerable doesn't change when we clear the current collection
                // (which would happen when the given enumerable is based on _selectedRows with linq methods)
                items = items.ToArray();

            _preventRowDataTriggerSelectedRowsChanged = true;

            ClearAllSelectedRows();
            items?.ForEach(SelectItem);

            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        /// <summary>
        /// Select one item
        /// </summary>
        /// <param name="item"></param>
        public void SetSelection(TItem item)
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            ClearSelectedRows();
            if (item != null)
            {
                SelectItem(item);
            }
            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        /// <summary>
        /// clear current pages' selected rows
        /// </summary>
        private void ClearSelectedRows()
        {
            foreach (TableDataItem<TItem> dataItem in _dataSourceCache.Values)
            {
                dataItem.SetSelected(false);
                _selectedRows.Remove(dataItem.Data);
            }
        }

        private void ClearAllSelectedRows()
        {
            foreach (TableDataItem<TItem> dataItem in _dataSourceCache.Values)
            {
                dataItem.SetSelected(false);
            }
            _selectedRows.Clear();
        }

        private void SelectionChanged()
        {
            if (SelectedRowsChanged.HasDelegate && !_preventRowDataTriggerSelectedRowsChanged)
            {
                _preventRender = true;
                _outerSelectedRows = _selectedRows;
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }
    }
}
