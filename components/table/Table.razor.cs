﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.HashCodes;
using AntDesign.JsInterop;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : AntDomComponentBase, ITable, IAsyncDisposable
    {
        private static readonly TItem _fieldModel = (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem));
        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _shouldRender = true;
        private int _parametersHashCode;

        [Parameter]
        public RenderMode RenderMode { get; set; } = RenderMode.Always;

        [Parameter]
        public IEnumerable<TItem> DataSource
        {
            get => _dataSource;
            set
            {
                _waitingReload = true;
                _dataSourceCount = value?.Count() ?? 0;
                _dataSource = value ?? Enumerable.Empty<TItem>();
            }
        }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; }

        [Parameter]
        public RenderFragment<RowData<TItem>> ExpandTemplate { get; set; }

        [Parameter]
        public bool DefaultExpandAllRows { get; set; }

        [Parameter]
        public Func<RowData<TItem>, bool> RowExpandable { get; set; } = _ => true;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> TreeChildren { get; set; } = _ => Enumerable.Empty<TItem>();

        [Parameter]
        public EventCallback<QueryModel<TItem>> OnChange { get; set; }

        [Parameter]
        public Func<RowData<TItem>, Dictionary<string, object>> OnRow { get; set; }

        [Parameter]
        public Func<Dictionary<string, object>> OnHeaderRow { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public string Footer { get; set; }

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public TableSize Size { get; set; }

        [Parameter]
        public TableLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Table;

        [Parameter]
        public bool Bordered { get; set; } = false;

        [Parameter]
        public string ScrollX { get; set; }

        [Parameter]
        public string ScrollY { get; set; }

        [Parameter]
        public int ScrollBarWidth { get; set; } = 17;

        [Parameter]
        public int IndentSize { get; set; } = 15;

        [Parameter]
        public int ExpandIconColumnIndex { get; set; }

        [Parameter]
        public Func<RowData<TItem>, string> RowClassName { get; set; } = _ => "";

        [Parameter]
        public Func<RowData<TItem>, string> ExpandedRowClassName { get; set; } = _ => "";

        [Parameter]
        public EventCallback<RowData<TItem>> OnExpand { get; set; }

        [Parameter]
        public SortDirection[] SortDirections { get; set; } = SortDirection.Preset.Default;

        [Parameter]
        public string TableLayout { get; set; }

        [Parameter]
        public EventCallback<RowData<TItem>> OnRowClick { get; set; }

        private bool _remoteDataSource;
        private bool _hasRemoteDataSourceAttribute;

        [Parameter]
        public bool RemoteDataSource
        {
            get => _remoteDataSource;
            set
            {
                _remoteDataSource = value;
                _hasRemoteDataSourceAttribute = true;
            }
        }

        [Parameter]
        public bool Responsive { get; set; } = true;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        public ColumnContext ColumnContext { get; set; }

        private IEnumerable<TItem> _showItems;

        private IEnumerable<TItem> _dataSource;

        private IList<SummaryRow> _summaryRows;

        private bool _hasFirstLoad;
        private bool _waitingReload;
        private bool _waitingReloadAndInvokeChange;
        private bool _treeMode;

        private bool _hasFixLeft;
        private bool _hasFixRight;
        private int _treeExpandIconColumnIndex;
        private readonly ClassMapper _wrapperClassMapper = new ClassMapper();
        private string TableLayoutStyle => TableLayout == null ? "" : $"table-layout: {TableLayout};";

        private ElementReference _tableHeaderRef;
        private ElementReference _tableBodyRef;
        private ElementReference _tableRef;

        private bool ServerSide => _hasRemoteDataSourceAttribute ? RemoteDataSource : Total > _dataSourceCount;

        bool ITable.TreeMode => _treeMode;
        int ITable.IndentSize => IndentSize;
        string ITable.ScrollX => ScrollX;
        string ITable.ScrollY => ScrollY;
        int ITable.ScrollBarWidth => ScrollBarWidth;
        int ITable.ExpandIconColumnIndex => ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
        int ITable.TreeExpandIconColumnIndex => _treeExpandIconColumnIndex;
        bool ITable.HasExpandTemplate => ExpandTemplate != null;
        TableLocale ITable.Locale => this.Locale;

        SortDirection[] ITable.SortDirections => SortDirections;

        void ITable.OnExpandChange(int cacheKey)
        {
            if (OnExpand.HasDelegate && _dataSourceCache.TryGetValue(cacheKey, out var currentRowData))
            {
                OnExpand.InvokeAsync(currentRowData);
            }
        }

        void ITable.AddSummaryRow(SummaryRow summaryRow)
        {
            _summaryRows ??= new List<SummaryRow>();
            _summaryRows.Add(summaryRow);
        }

        public void ReloadData()
        {
            PageIndex = 1;

            FlushCache();

            this.InternalReload();
        }

        public QueryModel GetQueryModel() => BuildQueryModel();

        private QueryModel<TItem> BuildQueryModel()
        {
            var queryModel = new QueryModel<TItem>(PageIndex, PageSize);

            foreach (var col in ColumnContext.HeaderColumns)
            {
                if (col is IFieldColumn fieldColumn)
                {
                    if (fieldColumn.SortModel != null)
                    {
                        queryModel.AddSortModel(fieldColumn.SortModel);
                    }

                    if (fieldColumn.FilterModel != null)
                    {
                        queryModel.AddFilterModel(fieldColumn.FilterModel);
                    }
                }
            }

            return queryModel;
        }

        void ITable.Refresh()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        void ITable.ReloadAndInvokeChange()
        {
            ReloadAndInvokeChange();
        }

        void ITable.ColumnSorterChange(IFieldColumn column)
        {
            foreach (var col in ColumnContext.HeaderColumns)
            {
                if (col.ColIndex != column.ColIndex && col is IFieldColumn fieldCol && fieldCol.SorterMultiple <= 0 && fieldCol.Sortable)
                {
                    fieldCol.ClearSorter();
                }
            }

            ReloadAndInvokeChange();
        }

        private void ReloadAndInvokeChange()
        {
            var queryModel = this.InternalReload();
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(queryModel);
            }
        }

        private QueryModel<TItem> InternalReload()
        {
            var queryModel = BuildQueryModel();

            if (ServerSide)
            {
                _showItems = _dataSource;
                _total = Total;
            }
            else
            {
                if (_dataSource != null)
                {
                    var query = _dataSource.AsQueryable();
                    foreach (var sort in queryModel.SortModel.OrderBy(x => x.Priority))
                    {
                        query = sort.SortList(query);
                    }

                    foreach (var filter in queryModel.FilterModel)
                    {
                        query = filter.FilterList(query);
                    }

                    _total = query.Count();

                    query = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
                    queryModel.SetQueryableLambda(query);

                    _showItems = query;
                    if (_total != Total)
                    {
                        if (TotalChanged.HasDelegate) TotalChanged.InvokeAsync(_total);
                    }
                }
                else
                {
                    _showItems = Enumerable.Empty<TItem>();
                    _total = 0;
                    if (_total != Total)
                    {
                        if (TotalChanged.HasDelegate) TotalChanged.InvokeAsync(_total);
                    }
                }

                _shouldRender = true;
            }

            _treeMode = TreeChildren != null && (_showItems?.Any(x => TreeChildren(x)?.Any() == true) == true);
            if (_treeMode)
            {
                _treeExpandIconColumnIndex = ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
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
                .If($"{prefixCls}-fixed-column {prefixCls}-scroll-horizontal", () => ScrollX != null)
                .If($"{prefixCls}-has-fix-left", () => _hasFixLeft)
                .If($"{prefixCls}-has-fix-right", () => _hasFixRight)
                //.If($"{prefixCls}-ping-left", () => _pingLeft)
                //.If($"{prefixCls}-ping-right", () => _pingRight)
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            _wrapperClassMapper
                .Add($"{prefixCls}-wrapper")
                .If($"{prefixCls}-responsive", () => Responsive) // Not implemented in ant design
                .If($"{prefixCls}-wrapper-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (RowTemplate != null)
            {
                ChildContent = RowTemplate;
            }

            this.ColumnContext = new ColumnContext(this);

            SetClass();

            if (ScrollX != null || ScrollY != null)
            {
                TableLayout = "fixed";
            }

            InitializePagination();

            FlushCache();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);

                if (ScrollY != null || ScrollX != null)
                {
                    await JsInvokeAsync(JSInteropConstants.BindTableScroll, _tableBodyRef, _tableRef, _tableHeaderRef, ScrollX != null, ScrollY != null);
                }

                _hasFirstLoad = true;
            }

            if (!firstRender)
            {
                this.FinishLoadPage();
            }

            _shouldRender = false;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_waitingReloadAndInvokeChange)
            {
                _waitingReloadAndInvokeChange = false;
                _waitingReload = false;

                if (_hasFirstLoad)
                {
                    ReloadAndInvokeChange();
                }
            }
            else if (_waitingReload)
            {
                _waitingReload = false;

                if (_hasFirstLoad)
                {
                    InternalReload();
                }
            }

            if (this.RenderMode == RenderMode.ParametersHashCodeChanged)
            {
                var hashCode = this.GetParametersHashCode();
                this._shouldRender = this._parametersHashCode != hashCode;
                this._parametersHashCode = hashCode;
            }
            else
            {
                this._shouldRender = true;
            }
        }

        protected override bool ShouldRender()
        {
            return this._shouldRender;
        }

        void ITable.HasFixLeft() => _hasFixLeft = true;

        void ITable.HasFixRight() => _hasFixRight = true;

        void ITable.TableLayoutIsFixed()
        {
            TableLayout = "fixed";
            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.Dispose();
            base.Dispose(disposing);
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isReloading)
            {
                if (ScrollY != null || ScrollX != null)
                {
                    await JsInvokeAsync(JSInteropConstants.UnbindTableScroll, _tableBodyRef);
                }
            }
            DomEventListener.Dispose();
        }

        bool ITable.RowExpandable(RowData rowData)
        {
            return RowExpandable(rowData as RowData<TItem>);
        }

        /// <summary>
        /// Indicates that a page is being refreshed
        /// </summary>
        private bool _isReloading;

        private void Reloading(JsonElement jsonElement)
        {
            _isReloading = true;
        }
    }
}
