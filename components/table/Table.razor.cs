using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.HashCodes;
using AntDesign.Filters;
using AntDesign.JsInterop;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Reflection;
using AntDesign.Table.Internal;

#if NET5_0_OR_GREATER

using Microsoft.AspNetCore.Components.Web.Virtualization;

#endif

namespace AntDesign
{
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
#endif

    public partial class Table<TItem> : AntDomComponentBase, ITable, IEqualityComparer<TItem>, IAsyncDisposable
    {
        private static TItem _fieldModel = typeof(TItem).IsInterface ? DispatchProxy.Create<TItem, TItemProxy>()
            : !typeof(TItem).IsAbstract ? (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem))
            : default;

        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _preventRender = false;
        private bool _shouldRender = true;
        private int _parametersHashCode;

        [Parameter]
        public RerenderStrategy RerenderStrategy { get; set; } = RerenderStrategy.Always;

        [Parameter]
        public IEnumerable<TItem> DataSource
        {
            get => _dataSource;
            set
            {
                _waitingDataSourceReload = true;
                _dataSourceCount = value is IQueryable<TItem> ? 0 : value?.Count() ?? 0;
                _dataSource = value ?? Enumerable.Empty<TItem>();
                _fieldModel ??= _dataSource.FirstOrDefault();
            }
        }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter]
        public RenderFragment<RowData<TItem>> RowTemplate { get; set; }

        [Parameter]
        public RenderFragment<TItem> ColumnDefinitions { get; set; }

        [Parameter]
        public RenderFragment<TItem> HeaderTemplate { get; set; }

        [Parameter]
        public RenderFragment<RowData<TItem>> ExpandTemplate { get; set; }

        [Parameter]
        public bool DefaultExpandAllRows { get; set; }

        /// <summary>
        /// The max expand level when use DefaultExpandAllRows.
        /// This attribute is used to avoid endless loop when the tree records have circular reference.
        /// The default value is 4.
        /// </summary>
        [Parameter]
        public int DefaultExpandMaxLevel { get; set; } = 4;

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
        public TableSize Size { get; set; } = TableSize.Default;

        [Parameter]
        public TableLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Table;

        [Parameter]
        public bool Bordered { get; set; } = false;

        [Parameter]
        public string ScrollX { get; set; }

        [Parameter]
        public string ScrollY { get; set; }

        [Parameter]
        public string ScrollBarWidth { get => _scrollBarWidth; set => _scrollBarWidth = value; }

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
        public bool Responsive { get; set; }

        [Parameter]
        public RenderFragment EmptyTemplate { get; set; }

        [Parameter] public Func<TItem, object> RowKey { get; set; } = default!;

        /// <summary>
        /// Enable resizable column
        /// </summary>
        [Parameter] public bool Resizable { get; set; }

        [Parameter]
        public IFieldFilterTypeResolver FieldFilterTypeResolver { get; set; }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Whether to enable virtualization feature or not, only works for .NET 5 and higher
        /// </summary>
        [Parameter]
        public bool EnableVirtualization { get; set; }

#endif

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        [Inject]
        private ILogger<Table<TItem>> Logger { get; set; }

        [Inject]
        private IFieldFilterTypeResolver InjectedFieldFilterTypeResolver { get; set; }

        public ColumnContext ColumnContext { get; set; }

        private IEnumerable<TItem> _showItems;

        private IEnumerable<TItem> _dataSource;

        private IList<SummaryRow> _summaryRows;

        private bool _hasInitialized;
        private bool _waitingDataSourceReload;
        private bool _waitingReloadAndInvokeChange;
        private bool _treeMode;
        private string _scrollBarWidth = "17px";

        private bool _hasFixLeft;
        private bool _hasFixRight;
        private int _treeExpandIconColumnIndex;

        private QueryModel _currentQueryModel;
        private readonly ClassMapper _wrapperClassMapper = new();
        private List<IGrouping<object, TItem>> _groups = [];

        private string TableLayoutStyle => TableLayout == null ? "" : $"table-layout: {TableLayout};";

        private ElementReference _tableHeaderRef;
        private ElementReference _tableBodyRef;
        private ElementReference _tableRef;

        private decimal _tableWidth;

        private bool _isVirtualizeEmpty;
        private bool _afterFirstRender;

        private bool ServerSide => _hasRemoteDataSourceAttribute ? RemoteDataSource : Total > _dataSourceCount;

        private bool IsEntityFrameworkCore => _dataSource is IQueryable<TItem> query && query.Provider.ToString().Contains("EntityFrameworkCore");

        private bool UseItemsProvider
        {
            get
            {
#if NET5_0_OR_GREATER
                return EnableVirtualization && (ServerSide || IsEntityFrameworkCore);
#else
                return false;
#endif
            }
        }

        bool ITable.TreeMode => _treeMode;
        int ITable.IndentSize => IndentSize;
        string ITable.ScrollX => ScrollX;
        string ITable.ScrollY => ScrollY;
        string ITable.ScrollBarWidth => _scrollBarWidth;
        int ITable.ExpandIconColumnIndex => ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
        int ITable.TreeExpandIconColumnIndex => _treeExpandIconColumnIndex;
        bool ITable.HasExpandTemplate => ExpandTemplate != null;
        bool ITable.HasHeaderTemplate => HeaderTemplate != null;
        bool ITable.HasRowTemplate => RowTemplate != null;

        void ITable.AddGroupColumn(IFieldColumn column) => AddGroupColumn(column);

        void ITable.RemoveGroupColumn(IFieldColumn column) => RemoveGroupColumn(column);

        TableLocale ITable.Locale => this.Locale;

        SortDirection[] ITable.SortDirections => SortDirections;

        public Table()
        {
            _dataSourceCache = new();
            _rootRowDataCache = new();
            _selectedRows = new(this);
        }

        private List<IFieldColumn> _groupedColumns = [];

        /// <summary>
        /// This method will be called when all columns have been set
        /// </summary>
        void ITable.OnColumnInitialized() => OnColumnInitialized();

        void ITable.OnExpandChange(RowData rowData)
        {
            if (OnExpand.HasDelegate)
            {
                OnExpand.InvokeAsync(rowData as RowData<TItem>);
            }
        }

        void ITable.AddSummaryRow(SummaryRow summaryRow)
        {
            _summaryRows ??= new List<SummaryRow>();
            _summaryRows.Add(summaryRow);
        }

        private void OnColumnInitialized()
        {
            if (_hasInitialized)
            {
                return;
            }

            ReloadAndInvokeChange();
            _hasInitialized = true;
        }

        public void ReloadData()
        {
            ResetData();

            PageIndex = 1;

            this.ReloadAndInvokeChange();
        }

        public void ReloadData(int? pageIndex, int? pageSize = null)
        {
            ResetData();

            ChangePageIndex(pageIndex ?? 1);
            ChangePageSize(pageSize ?? PageSize);

            this.ReloadAndInvokeChange();
        }

        public void ReloadData(QueryModel queryModel)
        {
            ResetData();

            if (queryModel is not null)
            {
                ChangePageIndex(queryModel.PageIndex);
                ChangePageSize(queryModel.PageSize);

                foreach (var sorter in queryModel.SortModel)
                {
                    var fieldColumn = ColumnContext.HeaderColumns[sorter.ColumnIndex] as IFieldColumn;
                    fieldColumn?.SetSortModel(sorter);
                }

                foreach (var filter in queryModel.FilterModel)
                {
                    var fieldColumn = ColumnContext.HeaderColumns[filter.ColumnIndex] as IFieldColumn;
                    fieldColumn?.SetFilterModel(filter);
                }

                this.ReloadAndInvokeChange();
            }
        }

        public void ResetData()
        {
            ChangePageIndex(1);
            ChangePageSize(PageSize);

            FlushCache();

            foreach (var col in ColumnContext.HeaderColumns)
            {
                if (col is IFieldColumn fieldColumn)
                {
                    if (fieldColumn.SortModel != null)
                    {
                        fieldColumn.ClearSorter();
                    }

                    if (fieldColumn.FilterModel != null)
                    {
                        fieldColumn.ClearFilters();
                    }
                }
            }
        }

        public QueryModel GetQueryModel() => BuildQueryModel().Clone() as QueryModel;

        private QueryModel<TItem> BuildQueryModel()
        {
            var queryModel = new QueryModel<TItem>(PageIndex, PageSize, _startIndex);

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

        void ITable.ColumnFilterChange()
        {
            ChangePageIndex(1);
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

            ChangePageIndex(1);
            ReloadAndInvokeChange();
        }

        private void ReloadAndInvokeChange()
        {
#if NET5_0_OR_GREATER
            if (UseItemsProvider)
            {
                StateHasChanged();
                return;
            }
#endif

            var queryModel = this.InternalReload();
            StateHasChanged();
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(queryModel);
            }
        }

        private async Task ReloadAndInvokeChangeAsync()
        {
            var queryModel = this.InternalReload();
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(queryModel);
            }
        }

        private QueryModel<TItem> InternalReload()
        {
            FlushCache(); // clear the selection state after pages was changed outside

            if (HidePagination && _dataSourceCount > 0)
            {
                _pageSize = _dataSourceCount;
            }

            var queryModel = BuildQueryModel();
            _currentQueryModel = queryModel;

            if (ServerSide)
            {
                _showItems = _dataSource ?? Enumerable.Empty<TItem>();
                _total = Total;
            }
            else
            {
                if (_dataSource != null)
                {
                    var query = queryModel.ExecuteQuery(_dataSource.AsQueryable());

                    _total = query.Count();
                    _showItems = queryModel.CurrentPagedRecords(query).ToList();
                }
                else
                {
                    _showItems = Enumerable.Empty<TItem>();
                    _total = 0;
                }

                if (_total != Total && TotalChanged.HasDelegate)
                {
                    TotalChanged.InvokeAsync(_total);
                }

                _shouldRender = true;
            }

            if (_groupedColumns.Count > 0)
            {
                GroupItems();
            }

            if (!_preventRender)
            {
                if (_outerSelectedRows != null)
                {
                    SetSelection(_outerSelectedRows);
                }
            }

            _treeMode = (TreeChildren != null && (_showItems?.Any(x => TreeChildren(x)?.Any() == true) == true)) || _groupedColumns.Count > 0;
            if (_treeMode)
            {
                _treeExpandIconColumnIndex = ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
            }

            return queryModel;
        }

#if NET5_0_OR_GREATER
        private async ValueTask<ItemsProviderResult<RowData<TItem>>> ItemsProvider(ItemsProviderRequest request)
        {
            _startIndex = request.StartIndex;
            if (_total > 0)
            {
                PageSize = Math.Min(request.Count, _total - _startIndex);
            }
            else
            {
                PageSize = request.Count;
            }

            IEnumerable<TItem> items = Array.Empty<TItem>();

            if (_dataSource is IQueryable<TItem> query)
            {
                _total = query.Count();
                items = query.Skip(_startIndex).Take(PageSize).ToArray();
            }
            else
            {
                await ReloadAndInvokeChangeAsync();
                items = _dataSource;
            }

            if (_startIndex == 0 && _total == 0)
            {
                _isVirtualizeEmpty = true;
            }

            return new ItemsProviderResult<RowData<TItem>>(items.Select((data, index) => GetRowData(data, index, 0)), _total);
        }
#endif

        public void GroupItems()
        {
            if (_groupedColumns.Count == 0)
            {
                _groups = [];
                StateHasChanged();
                return;
            }

            var queryModel = BuildQueryModel();
            var query = queryModel.ExecuteQuery(_dataSource.AsQueryable());

            foreach (var column in _groupedColumns)
            {
                var grouping = column.Group(queryModel.CurrentPagedRecords(query));
                _groups = [.. grouping];
            }

            StateHasChanged();
        }

        public void AddGroupColumn(IFieldColumn column)
        {
            this._groupedColumns.Add(column);
            GroupItems();
        }

        public void RemoveGroupColumn(IFieldColumn column)
        {
            this._groupedColumns.Remove(column);
            GroupItems();
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
                .If($"{prefixCls}-has-scrollbar-width", () => ScrollBarWidth != null)
                //.If($"{prefixCls}-ping-left", () => _pingLeft)
                //.If($"{prefixCls}-ping-right", () => _pingRight)
                .If($"{prefixCls}-rtl", () => RTL)
                .If($"{prefixCls}-resizable", () => Resizable)
                ;

            _wrapperClassMapper
                .Add($"{prefixCls}-wrapper")
                .If($"{prefixCls}-responsive", () => Responsive) // Not implemented in ant design
                .If($"{prefixCls}-wrapper-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ColumnDefinitions != null)
            {
                ChildContent = ColumnDefinitions;
            }

            this.ColumnContext = new ColumnContext(this);

            SetClass();

            if (ScrollX != null || ScrollY != null)
            {
                TableLayout = "fixed";
            }

#if NET5_0_OR_GREATER
            if (UseItemsProvider)
            {
                HidePagination = true;
            }
#endif

            InitializePagination();

            FieldFilterTypeResolver ??= InjectedFieldFilterTypeResolver;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_waitingReloadAndInvokeChange)
            {
                _waitingReloadAndInvokeChange = false;

                if (_hasInitialized)
                {
                    _waitingDataSourceReload = false;

                    ReloadAndInvokeChange();
                }
            }
            else if (_waitingDataSourceReload)
            {
                _waitingDataSourceReload = false;
                if (_hasInitialized)
                {
                    InternalReload();
                }
            }

            if (_preventRender)
            {
                _shouldRender = false;
                _preventRender = false;
            }
            else if (this.RerenderStrategy == RerenderStrategy.ParametersHashCodeChanged)
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _afterFirstRender = true;
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);

                if (ScrollY != null || ScrollX != null || Resizable)
                {
                    await JsInvokeAsync(JSInteropConstants.BindTableScroll, _tableBodyRef, _tableRef, _tableHeaderRef, ScrollX != null, ScrollY != null, Resizable);
                }

                // To handle the case where JS is called asynchronously and does not render when there is a fixed header or are any fixed columns.
                if (_hasInitialized && !_shouldRender)
                {
                    _shouldRender = true;
                    StateHasChanged();
                    return;
                }

                // To handle the case where a dynamic table does not render columns until the data is requested
                if (!ColumnContext.HeaderColumns.Any() && !_hasInitialized)
                {
                    OnColumnInitialized();
                    return;
                }
            }

            if (!firstRender)
            {
                this.FinishLoadPage();
            }

            _shouldRender = false;
        }

        protected override bool ShouldRender()
        {
            // Do not render until initialisation is complete.
            this._shouldRender = this._shouldRender && _hasInitialized;

            return this._shouldRender;
        }

        void ITable.HasFixLeft() => _hasFixLeft = true;

        void ITable.HasFixRight() => _hasFixRight = true;

        void ITable.TableLayoutIsFixed()
        {
            TableLayout = "fixed";
            StateHasChanged();
        }

        private void OnResize(DomRect domRect)
        {
            if (_tableWidth == domRect.Width)
            {
                return;
            }

            _tableWidth = domRect.Width;
            _shouldRender = true;
            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();

            base.Dispose(disposing);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            try
            {
                if (_afterFirstRender && !_isReloading)
                {
                    if (ScrollY != null || ScrollX != null)
                    {
                        await JsInvokeAsync(JSInteropConstants.UnbindTableScroll, _tableBodyRef);
                    }
                }
                DomEventListener?.Dispose();
            }
#if NET6_0_OR_GREATER
            catch (JSDisconnectedException) { }
#endif
            catch (Exception ex)
            {
                Logger.LogError(ex, "AntDesign: an exception was thrown at Table `DisposeAsync` method.");
            }
        }

        bool ITable.RowExpandable(RowData rowData)
        {
            return RowExpandable(rowData as RowData<TItem>);
        }

        private IEnumerable<TItem> SortFilterChildren(IEnumerable<TItem> children)
        {
            if (_currentQueryModel == null || ServerSide)
            {
                return children;
            }

            var query = children.AsQueryable();
            foreach (var sort in _currentQueryModel.SortModel.OrderBy(x => x.Priority))
            {
                query = sort.SortList(query);
            }

            foreach (var filter in _currentQueryModel.FilterModel)
            {
                query = filter.FilterList(query);
            }

            return query;
        }

        /// <summary>
        /// Indicates that a page is being refreshed
        /// </summary>
        private bool _isReloading;

        private void Reloading(JsonElement jsonElement)
        {
            _isReloading = true;
        }

        bool IEqualityComparer<TItem>.Equals(TItem x, TItem y)
        {
            if (RowKey == null)
                RowKey = data => data;

            return RowKey(x).Equals(RowKey(y));
        }

        int IEqualityComparer<TItem>.GetHashCode(TItem obj) => GetHashCode(obj);

        private int GetHashCode(TItem obj)
        {
            if (RowKey == null)
                RowKey = data => data;

            return RowKey(obj).GetHashCode();
        }
    }
}
