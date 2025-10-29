// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        private IEnumerable<TItem> _outerSelectedRows;

        /// <summary>
        /// Rows that are selected across pages
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

        /// <summary>
        /// Callback executed when the selected rows change
        /// </summary>
        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        /// <summary>
        /// Callback executed when the SelectAll button is clicked. <br/>
        /// This is useful for selecting all rows when the table is virtualized or not only shown on current page.
        /// <para>
        /// The argument is true when selecting all rows, false when unselecting all rows.
        /// </para>
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnSelectAll { get; set; }

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

            if (OnSelectAll.HasDelegate)
            {
                OnSelectAll.InvokeAsync(true);
            }

            foreach (var select in _rootRowDataCache.Values)
            {
                if (select.DataItem.Disabled)
                    continue;

                select.SetSelected(true, _selection.CheckStrictly);
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

            if (OnSelectAll.HasDelegate)
            {
                OnSelectAll.InvokeAsync(false);
            }

            foreach (var select in _rootRowDataCache.Values)
            {
                if (select.DataItem.Disabled)
                    continue;

                select.SetSelected(false, _selection.CheckStrictly);
            }

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
            selectItem.RowData.SetSelected(true, selectItem.Type == SelectionType.Radio || selectItem.CheckStrictly);

            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        void ITable.UnSelectSelection(ISelectionColumn selectItem)
        {
            _preventRowDataTriggerSelectedRowsChanged = true;

            ClearSelectedRows();
            selectItem.RowData.SetSelected(false, selectItem.Type == SelectionType.Radio || selectItem.CheckStrictly);

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
        /// Scroll the item into view
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task ScrollItemIntoView(TItem item)
        {
            if (_rootRowDataCache.TryGetValue(GetHashCode(item), out var rowData))
            {
                await JsInvokeAsync(JSInteropConstants.ScrollTo, rowData.RowElementRef);
            }
        }


        /// <summary>
        /// Set all selected items
        /// </summary>
        /// <param name="items"></param>
        public void SetSelection(IEnumerable<TItem> items)
        {
            UpdateSelection(items);
            SelectionChanged();
        }

        private void UpdateSelection(IEnumerable<TItem> items)
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
                _outerSelectedRows = _selectedRows;
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }
    }
}
