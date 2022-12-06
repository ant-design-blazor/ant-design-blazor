// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.HashCodes;
using AntDesign.JsInterop;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

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
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg", Columns = 1)]
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
#endif

    public partial class Table<TItem> : AntDomComponentBase, ITable, IAsyncDisposable
    {
        private static readonly TItem _fieldModel = (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem));
        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _preventRender = false;
        private bool _shouldRender = true;
        private int _parametersHashCode;

        /// <summary>
        /// Render mode of table. See <see cref="AntDesign.RenderMode"/> documentation for details.
        /// </summary>
        [Parameter]
        public RenderMode RenderMode { get; set; } = RenderMode.Always;

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
                _dataSourceCount = value?.Count() ?? 0;
                _dataSource = value ?? Enumerable.Empty<TItem>();
            }
        }

        /// <summary>
        /// Content of the table. Typically will contain <see cref="PropertyColumn{TItem, TProp}"/> and <see cref="ActionColumn"/> elements.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        /// <summary>
        /// Template for a row
        /// </summary>
        [Parameter]
        public RenderFragment<RowData<TItem>> RowTemplate { get; set; }

        [Parameter]
        public RenderFragment<TItem> ColumnDefinitions { get; set; }

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
        public Func<RowData<TItem>, bool> RowExpandable { get; set; } = _ => true;

        /// <summary>
        /// Function to determine if a specific row is selectable
        /// </summary>
        /// <default value="true for any rows" />
        [Parameter]
        public Func<TItem, bool> RowSelectable { get; set; } = _ => true;

        /// <summary>
        /// Children tree items
        /// </summary>
        /// <default value="Enumerable.Empty&lt;TItem&gt;()" />
        [Parameter]
        public Func<TItem, IEnumerable<TItem>> TreeChildren { get; set; } = _ => Enumerable.Empty<TItem>();

        /// <summary>
        /// Callback executed when paging, sorting, and filtering changes	
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
        /// Scroll bar width
        /// </summary>
        /// <default value="17px" />
        [Parameter]
        public string ScrollBarWidth { get => _scrollBarWidth; set => _scrollBarWidth = value; }

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
        public SortDirection[] SortDirections { get; set; } = SortDirection.Preset.Default;

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

        internal ColumnContext ColumnContext { get; set; }

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
        string ITable.ScrollBarWidth => _scrollBarWidth;
        int ITable.ExpandIconColumnIndex => ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
        int ITable.TreeExpandIconColumnIndex => _treeExpandIconColumnIndex;
        bool ITable.HasExpandTemplate => ExpandTemplate != null;
        bool ITable.HasHeaderTemplate => HeaderTemplate != null;
        bool ITable.HasRowTemplate => RowTemplate != null;

        TableLocale ITable.Locale => this.Locale;

        SortDirection[] ITable.SortDirections => SortDirections;

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

        /// <summary>
        /// Reload the data for the table, go to page 1
        /// </summary>
        public void ReloadData()
        {
            ResetData();

            PageIndex = 1;

            FlushCache();

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

            FlushCache();

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

                FlushCache();

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

        /// <summary>
        /// Get the query model for the table
        /// </summary>
        /// <returns></returns>
        public QueryModel GetQueryModel() => BuildQueryModel().Clone() as QueryModel;

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
            StateHasChanged();
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(queryModel);
            }
        }

        private QueryModel<TItem> InternalReload()
        {
            var queryModel = BuildQueryModel();
            _currentQueryModel = queryModel;

            if (ServerSide)
            {
                _showItems = _dataSource;
                _total = Total;
            }
            else
            {
                if (_dataSource != null)
                {
                    var query = queryModel.ExecuteQuery(_dataSource.AsQueryable());

                    _total = query.Count();
                    _showItems = queryModel.CurrentPagedRecords(query);
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

            if (HidePagination)
            {
                PageSize = _total;
            }

            if (!_preventRender)
            {
                if (_outerSelectedRows != null)
                {
                    _selectedRows = GetAllItemsByTopLevelItems(_showItems, true).Intersect(_outerSelectedRows).ToHashSet();
                    if (_selectedRows.Count != _outerSelectedRows.Count())
                    {
                        SelectedRowsChanged.InvokeAsync(_selectedRows);
                    }
                }
                else
                {
                    _selectedRows?.Clear();
                }

                var removedCacheItems = _dataSourceCache.Keys.Except(_showItems).ToArray();
                if (removedCacheItems.Length > 0)
                {
                    foreach (var item in removedCacheItems)
                    {
                        _dataSourceCache.Remove(item);
                    }
                    _allRowDataCache.Clear();
                }
            }

            _treeMode = TreeChildren != null && (_showItems?.Any(x => TreeChildren(x)?.Any() == true) == true);
            if (_treeMode)
            {
                _treeExpandIconColumnIndex = ExpandIconColumnIndex + (_selection != null && _selection.ColIndex <= ExpandIconColumnIndex ? 1 : 0);
            }

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
                .If($"{prefixCls}-has-scrollbar-width", () => ScrollBarWidth != null)
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

            InitializePagination();

            FlushCache();
        }

        private IEnumerable<TItem> GetAllItemsByTopLevelItems(IEnumerable<TItem> items, bool onlySelectable = false)
        {
            if (items?.Any() != true) return Array.Empty<TItem>();
            if (TreeChildren == null) return items;
            var result = GetAllDataItemsWithParent(items.Select(x => new DataItemWithParent<TItem>
            {
                Data = x,
                Parent = null
            })).Select(x => x.Data);
            if (onlySelectable) result = result.Where(x => RowSelectable(x));
            return result.ToHashSet();

            IEnumerable<DataItemWithParent<TItem>> GetAllDataItemsWithParent(IEnumerable<DataItemWithParent<TItem>> dataItems)
            {
                if (dataItems?.Any() != true) return Array.Empty<DataItemWithParent<TItem>>();
                if (TreeChildren == null) return dataItems ?? Array.Empty<DataItemWithParent<TItem>>();
                return dataItems.Union(
                    dataItems.SelectMany(
                        x1 =>
                        {
                            var ancestors = x1.GetAllAncestors().Select(x2 => x2.Data).ToHashSet();
                            return GetAllDataItemsWithParent(TreeChildren(x1.Data)?.Select(x2 => new DataItemWithParent<TItem>
                            {
                                Data = x2,
                                Parent = x1
                            }).Where(x2 => !ancestors.Contains(x2.Data) && x2.Data?.Equals(x1.Data) == false));
                        })
                    ).ToList();
            }
        }

        private class DataItemWithParent<T>
        {
            public T Data { get; set; }

            public DataItemWithParent<T> Parent { get; set; }

            public IEnumerable<DataItemWithParent<T>> GetAllAncestors()
            {
                var result = new HashSet<DataItemWithParent<T>>();
                var parent = Parent;
                while (parent != null)
                {
                    result.Add(parent);
                    parent = parent.Parent;
                }
                return result;
            }
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
            else if (this.RenderMode == RenderMode.ParametersHashCodeChanged)
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
                DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);

                if (ScrollY != null || ScrollX != null)
                {
                    await JsInvokeAsync(JSInteropConstants.BindTableScroll, _tableBodyRef, _tableRef, _tableHeaderRef, ScrollX != null, ScrollY != null);
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

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            base.Dispose(disposing);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!_isReloading)
                {
                    if (ScrollY != null || ScrollX != null)
                    {
                        await JsInvokeAsync(JSInteropConstants.UnbindTableScroll, _tableBodyRef);
                    }
                }
                DomEventListener?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError("AntDesign: an exception was thrown at Table `DisposeAsync` method.", ex);
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
    }
}
