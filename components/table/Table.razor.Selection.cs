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
                _outerSelectedRows = value;
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private readonly HashSet<TItem> _selectedRows;
        private bool _preventRowDataTriggerSelectedRowsChanged;

        internal void DataItemSelectedChanged(TableDataItem<TItem> dataItem, bool selected)
        {
            //if (!RowSelectable(dataItem.Data))
            //{
            //    dataItem.SetSelected(!selected, triggersSelectedChanged: false);
            //    return;
            //}
            if (selected)
            {
                _selectedRows.Add(dataItem.Data);
            }
            else
            {
                _selectedRows.Remove(dataItem.Data);
            }
            if (!_preventRowDataTriggerSelectedRowsChanged)
            {
                SelectionChanged();
                _selection?.StateHasChanged();
            }
        }

        ISelectionColumn ITable.Selection
        {
            get => _selection;
            set => _selection = value;
        }

        bool ITable.AllSelected => _selection.RowSelections.All(x => x.Disabled || x.Selected);

        bool ITable.AnySelected => _selection.RowSelections.Any(x => !x.Disabled && x.Selected);

        public void SelectAll()
        {
            foreach (var select in _selection.RowSelections)
            {
                if (select.Disabled)
                    continue;

                select.RowData.TableDataItem.SetSelected(true);
                _selectedRows.Add(((RowData<TItem>)select.RowData).Data);
            }

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        private void ClearSelectedRows()
        {
            //_selectedRows.Clear();
            foreach (TableDataItem<TItem> dataItem in _dataSourceCache.Values)
            {
                dataItem.SetSelected(false);
                _selectedRows.Remove(dataItem.Data);
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
            ClearSelectedRows();
            if (keys?.Count > 0)
            {
                _preventRowDataTriggerSelectedRowsChanged = true;
                _selection?.RowSelections.ForEach(x => x.RowData.Selected = keys.Contains(x.Key));
                _preventRowDataTriggerSelectedRowsChanged = false;
            }

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        // Only select the given row (for radio selection)
        void ITable.SetSelection(ISelectionColumn selectItem)
        {
            ClearSelectedRows();

            _preventRowDataTriggerSelectedRowsChanged = true;
            selectItem.RowData.Selected = true;
            _preventRowDataTriggerSelectedRowsChanged = false;

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        private void SelectItem(TItem item)
        {
            _selectedRows.Add(item);
            if (_dataSourceCache.TryGetValue(item, out var rowData))
            {
                rowData.SetSelected(true);
            }
        }

        public void SetSelection(IEnumerable<TItem> items)
        {
            if (ReferenceEquals(items, _selectedRows))
                return;

            if (items is not null and not IReadOnlyCollection<TItem>)
                // Ensure that the given enumerable doesn't change when we clear the current collection
                // (which would happen when the given enumerable is based on _selectedRows with linq methods)
                items = items.ToArray();

            ClearSelectedRows();
            items?.ForEach(SelectItem);

            _selection?.StateHasChanged();
            SelectionChanged();
        }

        public void SetSelection(TItem item)
        {
            ClearSelectedRows();
            if (item != null)
            {
                SelectItem(item);
            }

            _selection?.StateHasChanged();
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
