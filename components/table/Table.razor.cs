using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using OneOf;

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
        public OneOf<string, RenderFragment> Title { get; set; }
        
        [Parameter] 
        public OneOf<string, RenderFragment> Footer { get; set; }
        
        [Parameter] 
        public TableSize Size { get; set; }

        [Parameter] 
        public bool Bordered { get; set; } = false;

        [Parameter]
        public string ScrollX { get; set; }

        [Parameter]
        public string ScrollY { get; set; }

        [Parameter]
        public int ScrollBarWidth { get; set; } = 17;

        public ColumnContext ColumnContext { get; set; } = new ColumnContext();

        private IEnumerable<TItem> _dataSource;
        private ISelectionColumn _headerSelection;

        ISelectionColumn ITable.HeaderSelection
        {
            get => _headerSelection;
            set => _headerSelection = value;
        }

        void ITable.SelectionChanged(int[] checkedIndex)
        {
            if (SelectedRowsChanged.HasDelegate)
            {
                var list = new List<TItem>();
                foreach (var index in checkedIndex)
                {
                    list.Add(DataSource.ElementAt(index));
                }

                SelectedRowsChanged.InvokeAsync(list);
            }
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
                .If($"{prefixCls}-bordered", () => Bordered)
                .If($"{prefixCls}-small", () => Size == TableSize.Small)
                .If($"{prefixCls}-middle", () => Size == TableSize.Middle)
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

        private void ChangeSelection(int[] indexes)
        {
            if (indexes == null || !indexes.Any())
            {
                this._headerSelection.RowSelections.ForEach(x => x.Check(false));
                this._headerSelection.Check(false);
            }
            else
            {
                this._headerSelection.RowSelections.Where(x => !x.RowIndex.IsIn(indexes)).ForEach(x => x.Check(false));
                this._headerSelection.RowSelections.Where(x => x.RowIndex.IsIn(indexes)).ForEach(x => x.Check(true));
                this._headerSelection.Check(true);
            }
        }

        public void SetSelection(string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                this._headerSelection.RowSelections.ForEach(x => x.Check(false));
                this._headerSelection.Check(false);
            }
            else
            {
                this._headerSelection.RowSelections.Where(x => !x.Key.IsIn(keys)).ForEach(x => x.Check(false));
                this._headerSelection.RowSelections.Where(x => x.Key.IsIn(keys)).ForEach(x => x.Check(true));
                this._headerSelection.Check(keys.Any());
            }
        }
    }
}
