using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

        /// <summary>
        /// How to select or deselect the current row by clicking on it
        /// </summary>
        [Parameter]
        public TableRowClickSelect RowClickSelect { get; set; } = TableRowClickSelect.Disabled;

        private ISelectionColumn _selection;
        private readonly HashSet<TItem> _selectedRows;
        private bool _preventRowDataTriggerSelectedRowsChanged;
        private RowData<TItem> _prevSelectedRow;

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
            selectItem.RowData.SetSelected(true, selectItem.Type == "radio" || selectItem.CheckStrictly);

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
                _outerSelectedRows = _selectedRows;
                SelectedRowsChanged.InvokeAsync(_selectedRows);
            }
        }

        private IEnumerable<TItem> SelectRow(RowData<TItem> row)
        {
            if (_prevSelectedRow == row)
            {
                _prevSelectedRow = null;
                return [];
            }
            else
            {
                _prevSelectedRow = row;
                return [row.Data];
            }
        }

        private IEnumerable<TItem> CheckRow(RowData<TItem> row)
        {
            if (_selectedRows.Contains(row.Data, this))
            {
                _prevSelectedRow = null;
                return _selectedRows.Except([row.Data], this);
            }
            else
            {
                _prevSelectedRow = row;
                return _selectedRows.Concat([row.Data]);
            }
        }

        private bool AddChildrenToSelectRows(RowData<TItem> currentRow, RowData<TItem> endRow, List<TItem> toSelectRows)
        {
            toSelectRows.Add(currentRow.Data);

            if (currentRow == endRow) return true;

            // Do not select invisible rows
            if (!currentRow.Expanded) return false;

            var children = currentRow.Children?.Values.ToList();
            if (children == null || children.Count == 0)
            {
                return false;
            }
            if (endRow.Parent == currentRow)
            {
                children = children.Where(x => x.RowIndex <= endRow.RowIndex).ToList();
            }
            foreach (var child in children)
            {
                if (AddChildrenToSelectRows(child, endRow, toSelectRows))
                    return true;
            }
            return false;
        }

        private bool AddSiblingsToSelectRows(RowData<TItem> currentRow, RowData<TItem> endRoot, RowData<TItem> endRow, List<TItem> toSelectRows)
        {
            List<RowData<TItem>> siblings;
            if (currentRow.Parent == null)
            {
                siblings = [.. _rootRowDataCache.Values];
            }
            else
            {
                siblings = currentRow.Parent.Children?.Values.ToList();
            }
            siblings = siblings?.Where(x => x.RowIndex > currentRow.RowIndex).ToList();
            if (endRoot.Parent == currentRow.Parent)
                siblings = siblings?.Where(x => x.RowIndex <= endRoot.RowIndex).ToList();
            if (siblings == null) return false;
            foreach (var child in siblings)
            {
                if (AddChildrenToSelectRows(child, endRow, toSelectRows)) return true;
            }
            return false;
        }

        void AddToSelectedRows(RowData<TItem> startRow, RowData<TItem> startRoot, RowData<TItem> endRoot, RowData<TItem> endRow, List<TItem> toSelectRows)
        {
            if (AddChildrenToSelectRows(startRow, endRow, toSelectRows)) return;
            RowData<TItem> currRow = startRow;
            while (currRow != null)
            {
                if (AddSiblingsToSelectRows(currRow, endRoot, endRow, toSelectRows)) return;
                if (currRow == startRoot) break;
                currRow = currRow.Parent;
            }
        }

        private IEnumerable<TItem> SelectRange(RowData<TItem> row)
        {
            List<TItem> toSelectRows = [];
            if (_prevSelectedRow.PageIndex == row.PageIndex)
            {
                RowData<TItem> startRow, endRow, startRoot, endRoot;
                var prevRoot = _prevSelectedRow;
                var newRoot = row;
                while (true)
                {
                    if (prevRoot.Parent == newRoot.Parent) break;
                    if (prevRoot.Level < newRoot.Level)
                    {
                        while (true)
                        {
                            newRoot = newRoot.Parent;
                            if (newRoot.Level == prevRoot.Level) break;
                        }
                    }
                    else if (prevRoot.Level > newRoot.Level)
                    {
                        while (true)
                        {
                            prevRoot = prevRoot.Parent;
                            if (newRoot.Level == prevRoot.Level) break;
                        }
                    }
                    else
                    {
                        prevRoot = prevRoot.Parent;
                        newRoot = newRoot.Parent;
                    }
                }
                if ((prevRoot.RowIndex < newRoot.RowIndex)
                    || ((prevRoot.RowIndex == newRoot.RowIndex) && (_prevSelectedRow == newRoot)))
                {
                    startRow = _prevSelectedRow;
                    endRow = row;
                    startRoot = prevRoot;
                    endRoot = newRoot;
                }
                else
                {
                    startRow = row;
                    endRow = _prevSelectedRow;
                    startRoot = newRoot;
                    endRoot = prevRoot;
                }
                AddToSelectedRows(startRow, startRoot, endRoot, endRow, toSelectRows);
            }
            return toSelectRows;
        }

        void DoRowSelect(MouseEventArgs e, RowData<TItem> row)
        {
            IEnumerable<TItem> toSelectRows;

            if (RowClickSelect == TableRowClickSelect.Multiple && e.ShiftKey && _prevSelectedRow != null)
            {
                toSelectRows = SelectRange(row);
            }
            else if (RowClickSelect == TableRowClickSelect.Multiple && e.CtrlKey)
            {
                toSelectRows = CheckRow(row);
            }
            else if (RowClickSelect == TableRowClickSelect.Single || _selection?.Type == "radio")
            {
                toSelectRows = SelectRow(row);
            }
            else
            {
                toSelectRows = CheckRow(row);
            }

            SetSelection(toSelectRows);
        }
    }
}
