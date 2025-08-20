// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Core.Services;
using AntDesign.Core.HashCodes;
using AntDesign.Core.Reflection;
using AntDesign.Filters;
using AntDesign.Internal;
using AntDesign.JsInterop;
using AntDesign.Table.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

#if NET6_0_OR_GREATER
using Microsoft.JSInterop;
#endif

#if NET5_0_OR_GREATER

using Microsoft.AspNetCore.Components.Web.Virtualization;

#endif

namespace AntDesign
{
    /**
    <summary>
    <para>Displays rows of data.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>To display a collection of structured data.</item>
        <item>To sort, search, paginate, filter data.</item>
    </list>
    </summary>
    <seealso cref="PropertyColumn{TItem, TProp}"/>
    <seealso cref="ActionColumn"/>
    <seealso cref="Selection"/>
    <seealso cref="QueryModel{TItem}" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg", Columns = 1, Title = "Table", SubTitle = "表格")]
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
#endif

#if NETCOREAPP3_1_OR_GREATER
    public partial class Table<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TItem> : AntDomComponentBase, ITable, IEqualityComparer<TItem>, IAsyncDisposable
#else
    public partial class Table<TItem> : AntDomComponentBase, ITable, IEqualityComparer<TItem>, IAsyncDisposable
#endif
    {
        private static TItem _fieldModel = typeof(TItem).IsInterface ? DispatchProxy.Create<TItem, TItemProxy>()
            : !typeof(TItem).IsAbstract ? ExpressionActivator<TItem>.CreateInstance()
            ?? (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem))
            : default;

        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _preventRender = false;
        private bool _shouldRender = true;
        private int _parametersHashCode;

        /// <summary>
        /// Enable or disable automatic column index assignments.
        /// Should be disabled if complex column structure is used and index assigned via ColIndex parameter.
        /// </summary>
        [Parameter]
        [PublicApi("1.1.0")]
        public bool AutoColIndexes { get; set; } = true;

        /// <summary>
        /// Render mode of table. See <see cref="AntDesign.RerenderStrategy"/> documentation for details.
        /// </summary>
        [Parameter]
        public RerenderStrategy RerenderStrategy { get; set; } = RerenderStrategy.Always;

        /// <summary>
        /// Data to display in table
        /// </summary>
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

        /// <summary>
        /// Content of the table. Typically will contain <see cref="PropertyColumn{TItem, TProp}"/> and <see cref="ActionColumn"/> elements.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        /// <summary>
        /// Template for the header of grouping blocks
        /// </summary>
        [Parameter]
        public RenderFragment<GroupResult<TItem>> GroupTitleTemplate { get; set; }

        /// <summary>
        /// Template for the footer of grouping blocks
        /// </summary>
        [Parameter]
        public RenderFragment<GroupResult<TItem>> GroupFooterTemplate { get; set; }

        /// <summary>
        /// Template for Rows
        /// </summary>
        [Parameter]
        public RenderFragment<RowData<TItem>> RowTemplate { get; set; }

        /// <summary>
        /// Template for column definitions
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ColumnDefinitions { get; set; }

        /// <summary>
        /// Template for the header
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> HeaderTemplate { get; set; }

        /// <summary>
        /// Template use for what to display when a row is expanded
        /// </summary>
        [Parameter]
        public RenderFragment<RowData<TItem>> ExpandTemplate { get; set; }

        /// <summary>
        /// Initially, whether to expand all rows
        /// </summary>
        [Parameter]
        public bool DefaultExpandAllRows { get; set; }

        /// <summary>
        /// The max expand level when use DefaultExpandAllRows.
        /// This attribute is used to avoid endless loop when the tree records have circular reference.
        /// The default value is 4.
        /// </summary>
        [Parameter]
        public int DefaultExpandMaxLevel { get; set; } = 4;

        /// <summary>
        /// Function to determine if a specific row is expandable
        /// </summary>
        /// <default value="true for any rows" />
        [Parameter]
        public Func<RowData<TItem>, bool> RowExpandable { get; set; }

        /// <summary>
        /// Children tree items
        /// </summary>
        [Parameter]
        public Func<TItem, IEnumerable<TItem>> TreeChildren { get; set; }

        /// <summary>
        /// Callback executed when table initialized, paging, sorting, and filtering changes.
        /// </summary>
        [Parameter]
        public EventCallback<QueryModel<TItem>> OnChange { get; set; }

        /// <summary>
        /// Set row attributes
        /// </summary>
        [Parameter]
        public Func<RowData<TItem>, Dictionary<string, object>> OnRow { get; set; }

        /// <summary>
        /// Set header row attributes
        /// </summary>
        [Parameter]
        public Func<Dictionary<string, object>> OnHeaderRow { get; set; }

        /// <summary>
        /// Is the table loading
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// Table title text
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Table title content
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Footer text
        /// </summary>
        [Parameter]
        public string Footer { get; set; }

        /// <summary>
        /// Footer content
        /// </summary>
        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        /// <summary>
        /// Table size
        /// </summary>
        [Parameter]
        public TableSize Size { get; set; } = TableSize.Default;

        /// <summary>
        /// Default copywriting settings, currently including sorting, filtering, and empty data copywriting
        /// </summary>
        [Parameter]
        public TableLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Table;

        /// <summary>
        /// Bordered table or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Bordered { get; set; } = false;

        /// <summary>
        /// Striped table or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        [PublicApi("1.1.0")]
        public bool Striped { get; set; } = false;

        /// <summary>
        /// Set horizontal scrolling, can also be used to specify the width of the scrolling area, can be set as pixel value, percentage
        /// </summary>
        [Parameter]
        public string ScrollX { get; set; }

        /// <summary>
        /// Set the vertical scroll, can also be used to specify the height of the scrolling area, can be set as a pixel value
        /// </summary>
        [Parameter]
        public string ScrollY { get; set; }


        /// <summary>
        /// Automatically raise the table height to full screen display
        /// </summary>
        [Parameter]
        public bool AutoHeight { get; set; }


        /// <summary>
        /// Scroll bar width
        /// </summary>
        /// <default value="17px" />
        [Parameter]
        public string ScrollBarWidth { get; set; }

        /// <summary>
        /// When displaying tree data, the width of each level of indentation, in px
        /// </summary>
        /// <default value="15" />
        [Parameter]
        public int IndentSize { get; set; } = 15;

        /// <summary>
        /// Index of the column where the custom expand icon is located
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int ExpandIconColumnIndex { get; set; }

        /// <summary>
        /// Function to determine the class name of a specific row
        /// </summary>
        [Parameter]
        public Func<RowData<TItem>, string> RowClassName { get; set; } = _ => "";

        /// <summary>
        /// Function to determine the class name of a specific row when expanded
        /// </summary>
        [Parameter]
        public Func<RowData<TItem>, string> ExpandedRowClassName { get; set; } = _ => "";

        /// <summary>
        /// Callback executed when row expands
        /// </summary>
        [Parameter]
        public EventCallback<RowData<TItem>> OnExpand { get; set; }

        /// <summary>
        /// Supported sorting methods, covering sortDirections in Table
        /// </summary>
        [Parameter]
        public SortDirection[] SortDirections { get; set; } = [SortDirection.Ascending, SortDirection.Descending, SortDirection.None];

        /// <summary>
        /// The table-layout attribute of the table element, set to fixed means that the content will not affect the layout of the column
        /// </summary>
        [Parameter]
        public string TableLayout { get; set; }

        /// <summary>
        /// Callback executed when a row is clicked
        /// </summary>
        [Parameter]
        public EventCallback<RowData<TItem>> OnRowClick { get; set; }

        private bool _remoteDataSource;
        private bool _hasRemoteDataSourceAttribute;

        /// <summary>
        /// If the datasource is remote or not for more complex use cases
        /// </summary>
        /// <default value="false" />
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

        /// <summary>
        /// When set to true and the screen width is less than 960px, the table would switch to small-screen mode.
        /// In small-screen mode, only certain features are currently supported, and mis-styling will occur in tables with some features such as group, expanded columns, tree data, summary cell, etc.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Responsive { get; set; }

        /// <summary>
        /// Customize the empty template when the table is empty
        /// </summary>
        [Parameter]
        public RenderFragment EmptyTemplate { get; set; }

        /// <summary>
        /// Specify the identifier of each row. Default is the hash code of the row item.
        /// </summary>
        [Parameter] public Func<TItem, object> RowKey { get; set; } = default!;

        /// <summary>
        /// Enable resizable column
        /// </summary>
        [Parameter] public bool Resizable { get; set; }

        /// <summary>
        /// Set the field filter type resolver
        /// </summary>
        [Parameter]
        public IFieldFilterTypeResolver FieldFilterTypeResolver { get; set; }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Whether to enable virtualization feature or not, only works for .NET 5 and higher
        /// </summary>
        [Parameter]
        public bool EnableVirtualization { get; set; }

#endif
        [Parameter]
        public int? StickyOffsetHeader { get; set; }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        [Inject]
        private ILogger<Table<TItem>> Logger { get; set; }

        [Inject]
        private IFieldFilterTypeResolver InjectedFieldFilterTypeResolver { get; set; }

        [Inject]
        private ClientDimensionService ClientDimensionService { get; set; }

        protected ColumnContext ColumnContext { get; set; }

        private IEnumerable<TItem> _showItems;

        private IEnumerable<TItem> _dataSource;

        private IList<SummaryRow> _summaryRows;

        private bool _hasInitialized;
        private bool _waitingDataSourceReload;
        private bool _waitingReloadAndInvokeChange;
        private bool _treeMode;
        private string _scrollBarWidth;
        private bool _hasFixLeft;
        private bool _hasFixRight;
        private int _treeExpandIconColumnIndex;

        private QueryModel _currentQueryModel;
        private readonly ClassMapper _wrapperClassMapper = new();
        private List<GroupResult<TItem>> _groups = [];

        private string TableLayoutStyle => TableLayout == null ? "" : $"table-layout: {TableLayout};";

        private string StickyHolderStyle => StickyOffsetHeader.HasValue ? $"top: {StickyOffsetHeader.Value}px;" : "";

        private ElementReference _wrapperRef;
        private ElementReference _tableHeaderRef;
        private ElementReference _tableBodyRef;
        private ElementReference _tableRef;

        private decimal _tableWidth;

        private bool _isVirtualizeEmpty;
        private bool _afterFirstRender;
        private bool _isRebuilding;
        private RenderFragment<TItem> _childContent;

        private HashSet<string> _outsideParameters;
        private bool ServerSide => _hasRemoteDataSourceAttribute ? RemoteDataSource : Total > _dataSourceCount;
        private bool IsSticky => StickyOffsetHeader.HasValue;
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
        string ITable.ScrollBarWidth => _scrollBarWidth ?? "15px";
        int ITable.ExpandIconColumnIndex => ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
        int ITable.TreeExpandIconColumnIndex => _treeExpandIconColumnIndex;
        bool ITable.HasExpandTemplate => ExpandTemplate != null;
        bool ITable.HasHeaderTemplate => HeaderTemplate != null;
        bool ITable.HasRowTemplate => RowTemplate != null;
        bool ITable.ServerSide => ServerSide;
        bool ITable.IsSticky => IsSticky;

        void ITable.AddGroupColumn(IFieldColumn column) => AddGroupColumn(column);

        void ITable.RemoveGroupColumn(IFieldColumn column) => RemoveGroupColumn(column);

        TableLocale ITable.Locale => this.Locale;

        RenderFragment<RowData> ITable.GroupTitleTemplate => rowData =>
        {
            var groupResult = ((RowData<TItem>)rowData).GroupResult;
            if (GroupTitleTemplate == null)
            {
                return builder =>
                {
                    builder.AddContent(0, groupResult.Key);
                };
            }
            return builder =>
            {
                builder.AddContent(0, GroupTitleTemplate(groupResult));
            };
        };

        SortDirection[] ITable.SortDirections => SortDirections;

        public Table()
        {
            _dataSourceCache = new();
            _rootRowDataCache = new();
            _selectedRows = new(this);
            _showItemHashs = new(this);
        }

        private List<IFieldColumn> _groupedColumns = [];

        /// <summary>
        /// This method will be called when all columns have been set
        /// </summary>
        void ITable.OnColumnInitialized() => OnColumnInitialized();

        bool ITable.RebuildColumns(bool add) => RebuildColumns(add);

        void ITable.OnExpandChange(RowData rowData)
        {
            _preventRender = true;
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

            _hasInitialized = true;
            ReloadAndInvokeChange();
        }

        /// <summary>
        /// Reload the data for the table, go to page 1
        /// </summary>
        public void ReloadData()
        {
            ResetData();

            ChangePageIndex(1);

            this.ReloadAndInvokeChange();
        }

        /// <summary>
        /// Reload the data for the table and go to specific page at page size
        /// </summary>
        /// <param name="pageIndex">Page to load after reload. Defaults to 1.</param>
        /// <param name="pageSize">Page size to use after reload. Defaults to the current value of <see cref="PageSize"/></param>
        public void ReloadData(int? pageIndex, int? pageSize = null)
        {
            ResetData();

            ChangePageIndex(pageIndex ?? 1);
            ChangePageSize(pageSize ?? PageSize);

            this.ReloadAndInvokeChange();
        }

        /// <summary>
        /// Reload the table's data from the provided query model
        /// </summary>
        /// <param name="queryModel"></param>
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

        /// <summary>
        /// Reset the table to its default view. Goes to page 1, default page size and clears sorts and filters.
        /// </summary>
        public void ResetData()
        {
            ChangePageIndex(1);
            ChangePageSize(PageSize);

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

        /// <summary>
        /// Get the query model for the table
        /// </summary>
        /// <returns></returns>
        public QueryModel GetQueryModel() => BuildQueryModel().Clone() as QueryModel;

        private QueryModel<TItem> BuildQueryModel()
        {
            var queryModel = new QueryModel<TItem>(_pageIndex, _pageSize, _startIndex);

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

            if (_fieldModel is null)
            {
                StateHasChanged();
                return;
            }

            var queryModel = this.InternalReload();
            _selection?.OnDataSourceChange();

            StateHasChanged();

            if (OnChange.HasDelegate && _pageIndex > 0)
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
            if (HidePagination && !_outsideParameters.Contains(nameof(PageSize)) && _dataSourceCount > 0)
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

            FlushCache();

            if (_groupedColumns.Count > 0)
            {
                GroupItems();
            }

            if (!_preventRender)
            {
                if (_outerSelectedRows != null)
                {
                    UpdateSelection(_outerSelectedRows);
                }
            }

            _treeMode = (TreeChildren != null && (_showItems?.Any(x => TreeChildren(x)?.Any() == true) == true || OnExpand.HasDelegate)) || _groupedColumns.Count > 0;
            if (_treeMode)
            {
                _treeExpandIconColumnIndex = ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
            }

            return queryModel;
        }

        /// <summary>
        /// Call this method after data source has changed to refresh the state of the table.
        /// </summary>
        /// Make the method protected to allow derived classes to call it.
        protected void InvokeDataSourceHasChanged()
        {
            if (_hasInitialized)
            {
                InternalReload();
            }
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

        private void GroupItems()
        {
            if (_groupedColumns.Count == 0)
            {
                _groups = [];
                StateHasChanged();
                return;
            }

            var selectedKeys = _groupedColumns.Select(x => x.GetGroupByExpression<TItem>()).ToArray();
            _groups = DynamicGroupByHelper.DynamicGroupBy(_showItems, selectedKeys);
        }

        internal void AddGroupColumn(IFieldColumn column)
        {
            this._groupedColumns.Add(column);
        }

        internal void RemoveGroupColumn(IFieldColumn column)
        {
            this._groupedColumns.RemoveAll(x => x.ColIndex == column.ColIndex);
        }

        private void SetClass()
        {
            string prefixCls = "ant-table";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-fixed-header", () => ScrollY != null)
                .If($"{prefixCls}-bordered", () => Bordered)
                .If($"{prefixCls}-striped", () => Striped)
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

            _scrollBarWidth = ScrollBarWidth;

#if NET5_0_OR_GREATER
            if (UseItemsProvider)
            {
                HidePagination = true;
            }
#endif
            InitializePagination();

            FieldFilterTypeResolver ??= InjectedFieldFilterTypeResolver;
        }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            _outsideParameters ??= [.. parameters.ToDictionary().Keys];

            await base.SetParametersAsync(parameters);

            if (AutoHeight)
            {
                ScrollY = "0px";
            }

            if (ScrollX != null || ScrollY != null)
            {
                TableLayout ??= "fixed";
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
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
            else if (_isRebuilding)
            {
                // because the ChildContent was clear inside, it would be set again after parent component is re-rendered
                // avoid re-render during rebuilding 
                _shouldRender = false;
            }
            else
            {
                _shouldRender = true;
            }

            if (!this._shouldRender)
            {
                return;
            }

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
                InvokeDataSourceHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _afterFirstRender = true;
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);

                if (ScrollY != null || ScrollX != null || Resizable || AutoHeight)
                {
                    await JsInvokeAsync(JSInteropConstants.BindTableScroll, _wrapperRef, _tableBodyRef, _tableRef, _tableHeaderRef, ScrollX != null, ScrollY != null, Resizable, AutoHeight);
                }

                if (ScrollY != null && ScrollY != null && _scrollBarWidth == null)
                {
                    var scrollBarSize = await ClientDimensionService.GetScrollBarSizeAsync();
                    _scrollBarWidth = $"{scrollBarSize}px";
                    ColumnContext.HeaderColumns.LastOrDefault()?.UpdateFixedStyle();
                }

                // To handle the case where JS is called asynchronously and does not render when there is a fixed header or are any fixed columns.
                if (_hasInitialized && !_shouldRender)
                {
                    _shouldRender = true;
                    StateHasChanged();
                    return;
                }

                // To handle the case where a dynamic table does not render columns until the data is requested
                if ((!ColumnContext.HeaderColumns.Any() || _fieldModel is null) && !_hasInitialized)
                {
                    OnColumnInitialized();
                    return;
                }
            }

            if (!firstRender)
            {
                this.FinishLoadPage();
            }

            if (_isRebuilding)
            {
                // call from Rerender, ChildContent is empty at this time,
                // so we need to rerender again for rebuild the columns
                _shouldRender = true;
                ChildContent = _childContent;
                StateHasChanged();

                // set the flag to false after calling StateHasChanged because we need to re-render when _hasInitialized is false
                _isRebuilding = false;
                return;
            }

            _shouldRender = false;
        }

        protected override bool ShouldRender()
        {
            // Do not render until initialisation is complete.
            this._shouldRender = this._shouldRender && (_hasInitialized || _isRebuilding);

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
                    if (ScrollY != null || ScrollX != null || Resizable || AutoHeight)
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

        bool ITable.RowExpandable(RowData rowData) => InternalRowExpandable(rowData as RowData<TItem>);

        private bool InternalRowExpandable(RowData<TItem> rowData)
        {
            if (RowExpandable != null)
            {
                return RowExpandable.Invoke(rowData);
            }

            if (_treeMode && TreeChildren != null)
            {
                return TreeChildren(rowData.Data)?.Any() == true || OnExpand.HasDelegate;
            }

            if (ExpandTemplate != null)
            {
                return true;
            }

            return OnExpand.HasDelegate;
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

            if (x is null && y is null)
                return true;

            return RowKey(x).Equals(RowKey(y));
        }

        int IEqualityComparer<TItem>.GetHashCode(TItem obj) => GetHashCode(obj);

        private int GetHashCode(TItem obj)
        {
            if (RowKey == null)
                RowKey = data => data;

            return RowKey(obj).GetHashCode();
        }

        /// <summary>
        /// For each column change, it needs to rerender four times
        /// <br/> 1. re-render once for recognize there is any column changed after calling at <see cref="OnParametersSet"/>, trigger render the empty ChildContent.
        /// <br/> 2. re-render once for empty ChildContent after calling at <see cref="ITable.RebuildColumns" />, then trigger rendering for rebuild the origin content. 
        /// <br/> 3. re-render for rebuilding columns after calling at <see cref="OnAfterRenderAsync(bool)"/>, and then trigger rendering for load data after the columns are ready.
        /// <br/> 4. re-render for reload data after calling at <see cref="OnColumnInitialized" />
        /// </summary>
        /// <param name="add">Whether a column is added/removed</param>
        /// <remarks>
        /// lifecycle process: columns was changed -> render#1(true) -> column add/dispose -> call rebuild(call render#2) -> render#2(true) -> OnAfterRenderAsync#2 (call render#3) -> render#3(true)
        /// -> OnColumnInitialized call render#4 -> OnAfterRenderAsync#4 -> OnAfterRenderAsync#3 -> OnAfterRenderAsync#1 (the last 2 steps are duplicated and useless)
        /// </remarks>
        /// <returns>Whether to start rebuilding</returns>
        protected virtual bool RebuildColumns(bool add)
        {
            // avoid rerender again before initialized (beacuse when we render the empty ChildContent, it will be called by Dispose)
            if (add && !_hasInitialized) return false;
            // avoid rerender again when the column are cleared
            if (!add && ColumnContext.Columns.Count == 0) return false;

            if (!_afterFirstRender) return false;
            if (IsDisposed || _isReloading) return false;

            _childContent = ChildContent;

            ChildContent = c => c => { };
            ColumnContext = new ColumnContext(this);
            _hasInitialized = false;
            _isRebuilding = true;

            StateHasChanged();

            return true;
        }
    }
}
