using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : AntDomComponentBase, ITable
    {
        private static readonly TItem _fieldModel = (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem));

        [Parameter]
        public IEnumerable<TItem> DataSource
        {
            get => _dataSource;
            set
            {
                _total = value.Count();
                _dataSource = value;
            }
        }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter]
        public IEnumerable<TItem> SelectedRows { get; set; } = Array.Empty<TItem>();

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public string ScrollX { get; set; }

        [Parameter]
        public string ScrollY { get; set; }

        private readonly IList<ITableColumn> _columns = new List<ITableColumn>();

        public IRowSelection HeaderSelection { get; set; }

        private IEnumerable<TItem> _dataSource;

        public void AddColumn(ITableColumn column)
        {
            _columns.Add(column);
            StateHasChanged();
        }

        void ITable.OnSelectionChanged(int[] checkedIndex)
        {
            var list = new List<TItem>();
            foreach (var index in checkedIndex)
            {
                list.Add(DataSource.ElementAt(index));
            }

            SelectedRows = list.ToArray();
            SelectedRowsChanged.InvokeAsync(SelectedRows);
        }
    }
}
