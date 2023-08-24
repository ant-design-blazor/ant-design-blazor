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
        private bool _preventChangeRowDataWithSameData;
        private bool _preventRowDataSelectedChangedCallback;

        private void RowDataSelectedChanged(RowData<TItem> rowData, bool selected)
        {
            if (_preventRowDataSelectedChangedCallback) return;
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
            if (!_preventChangeRowDataWithSameData)
            {
                _preventRowDataSelectedChangedCallback = true;
                if (_allRowDataCache.ContainsKey(rowData.Data))
                {
                    foreach (var rowDataWithSameData in _allRowDataCache[rowData.Data])
                    {
                        rowDataWithSameData.Selected = selected;
                    }
                }
                _preventRowDataSelectedChangedCallback = false;
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
            _preventRowDataSelectedChangedCallback = true;
            _preventRowDataTriggerSelectedRowsChanged = true;
            _preventChangeRowDataWithSameData = true;
            foreach (var rowDataList in _allRowDataCache.Values)
            {
                foreach (var rowData in rowDataList)
                {
                    rowData.Selected = true;
                }
            }
            _preventRowDataSelectedChangedCallback = false;
            _preventRowDataTriggerSelectedRowsChanged = false;
            _preventChangeRowDataWithSameData = false;
            if (_selection != null)
            {
                _selection.StateHasChanged();
            }
            SelectionChanged();
        }

        public void UnselectAll()
        {
            _selectedRows.Clear();
            _preventRowDataTriggerSelectedRowsChanged = true;
            _preventChangeRowDataWithSameData = true;
            foreach (var rowDataList in _allRowDataCache.Values)
            {
                foreach (var rowData in rowDataList)
                {
                    rowData.Selected = false;
                }
            }
            _preventRowDataTriggerSelectedRowsChanged = false;
            _preventChangeRowDataWithSameData = false;
            if (_selection != null)
            {
                _selection.StateHasChanged();
            }
            SelectionChanged();
        }

        public void SetSelection(string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                UnselectAll();
                return;
            }

            if (_selection == null)
            {
                throw new InvalidOperationException("To use SetSelection method for a table, you should add a Selection component to the column definition.");
            }

            _preventRowDataTriggerSelectedRowsChanged = true;
            _preventChangeRowDataWithSameData = true;
            //_selection.RowSelections.ForEach(x => x.RowData.Selected = x.Key.IsIn(keys));
            _preventRowDataTriggerSelectedRowsChanged = false;
            _preventChangeRowDataWithSameData = false;
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
