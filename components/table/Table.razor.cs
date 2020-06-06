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

        public ColumnContext ColumnContext { get; set; } = new ColumnContext();

        public ISelectionColumn HeaderSelection { get; set; }

        private IEnumerable<TItem> _dataSource;

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

        void ITable.Refresh()
        {
            StateHasChanged();
        }

        private void SetClass()
        {
            string prefixCls = "ant-table";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-fixed-header", () => ScrollY != null)
                //.Add( "ant-table ant-table-ping-left ant-table-ping-right ")
                .If($"{prefixCls}-fixed-column {prefixCls}-scroll-horizontal", () => ColumnContext.Columns.Any(x => x.Fixed.IsIn("left", "right")))
                .If($"{prefixCls}-has-fix-left", () => ColumnContext.Columns.Any(x => x.Fixed == "left"))
                .If($"{prefixCls}-has-fix-right {prefixCls}-ping-right ", () => ColumnContext.Columns.Any(x => x.Fixed == "right"))
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClass();
        }
    }
}
