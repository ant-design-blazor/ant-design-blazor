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
                //_selectedRows = _dataSource.Intersect(value).ToList();
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        private ISelectionColumn _selection;
        private HashSet<TItem> _selectedRows = new();
        private bool _preventRowDataTriggerSelectedRowsChanged;

        private void RowDataSelectedChanged(RowData rowData, bool selected)
        {
            if (selected)
            {
                _selectedRows.Add((rowData as RowData<TItem>).Data);
            }
            else
            {
                _selectedRows.Remove((rowData as RowData<TItem>).Data);
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

        bool ITable.AllSelected => _selectedRows.Count == GetAllDataItems(_showItems).Count();

        bool ITable.AnySelected => _selectedRows.Count > 0;

        public void SelectAll()
        {
            _preventRowDataTriggerSelectedRowsChanged = true;
            _selection.RowSelections.ForEach(x => x.RowData.Selected = true);
            _preventRowDataTriggerSelectedRowsChanged = false;
            _selectedRows = GetAllDataItems(_showItems).ToHashSet();
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
            _selection.RowSelections.ForEach(x => x.RowData.Selected = false);
            _preventRowDataTriggerSelectedRowsChanged = false;
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

            _selectedRows.Clear();
            _preventRowDataTriggerSelectedRowsChanged = true;
            _selection.RowSelections.ForEach(x => x.RowData.Selected = x.Key.IsIn(keys));
            _preventRowDataTriggerSelectedRowsChanged = false;
            _selection.StateHasChanged();
            SelectionChanged();
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
