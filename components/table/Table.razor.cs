using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AntDesign.TableModels;
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
                _total = value?.Count() ?? 0;
                _dataSource = value ?? Enumerable.Empty<TItem>();
                StateHasChanged();
            }
        }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter]
        public IEnumerable<TItem> SelectedRows { get; set; }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedRowsChanged { get; set; }

        [Parameter]
        public EventCallback<QueryModel<TItem>> OnChange { get; set; }

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

        private IEnumerable<TItem> _showItems;

        private IEnumerable<TItem> _dataSource;

        public void ReloadData()
        {
            this.Reload();
        }

        void ITable.Refresh()
        {
            StateHasChanged();
        }

        void ITable.ReloadAndInvokeChange()
        {
            ReloadAndInvokeChange();
        }

        private void ReloadAndInvokeChange()
        {
            var queryModel = this.Reload();
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(queryModel);
            }
        }

        private QueryModel<TItem> Reload()
        {
            var queryModel = new QueryModel<TItem>(PageIndex, PageSize);

            if (Total > _total)
            {
                _showItems = _dataSource;
            }
            else
            {
                var query = _dataSource.AsQueryable();

                foreach (var col in ColumnContext.Columns)
                {
                    if (col is IFieldColumn fieldColumn && fieldColumn.Sortable)
                    {
                        query = fieldColumn.SortModel.Sort(query);
                        queryModel.AddSortModel(fieldColumn.SortModel);
                    }
                }

                query = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
                queryModel.SetQueryableLambda(query);

                _showItems = query;
            }

            StateHasChanged();

            return queryModel;
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
            SetPaginationClass();

            ReloadAndInvokeChange();
        }
    }
}
