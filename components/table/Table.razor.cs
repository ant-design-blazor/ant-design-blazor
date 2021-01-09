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
        public Func<RowData<TItem>, bool> RowExpandable { get; set; } = _ => true;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> TreeChildren { get; set; } = _ => Enumerable.Empty<TItem>();

        [Parameter]
        public EventCallback<QueryModel<TItem>> OnChange { get; set; }

        [Parameter]
        public EventCallback<RowData<TItem>> OnRowClick { get; set; }

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

        [Inject]
        public DomEventService DomEventService { get; set; }

        public ColumnContext ColumnContext { get; set; }

        private IEnumerable<TItem> _showItems;

        private IEnumerable<TItem> _dataSource;

        private bool _waitingReload;
        private bool _waitingReloadAndInvokeChange;
        private bool _treeMode;

        private bool _hasFixLeft;
        private bool _hasFixRight;
        private bool _pingRight;
        private bool _pingLeft;

        private ElementReference _tableHeaderRef;
        private ElementReference _tableBodyRef;

        private bool ServerSide => _total > _dataSourceCount;

        bool ITable.TreeMode => _treeMode;

        int ITable.IndentSize => IndentSize;

        string ITable.ScrollX => ScrollX;
        string ITable.ScrollY => ScrollY;
        int ITable.ScrollBarWidth => ScrollBarWidth;

        public void ReloadData()
        {
            PageIndex = 1;

            FlushCache();

            this.Reload();
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

            foreach (var col in ColumnContext.Columns)
            {
                if (col is IFieldColumn fieldColumn && fieldColumn.Sortable)
                {
                    queryModel.AddSortModel(fieldColumn.SortModel);
                }
            }

            if (ServerSide)
            {
                _showItems = _dataSource;
            }
            else
            {
                if (_dataSource != null)
                {
                    var query = _dataSource.AsQueryable();
                    foreach (var sort in queryModel.SortModel)
                    {
                        query = sort.Sort(query);
                    }

                    query = query.Skip((PageIndex - 1) * PageSize).Take(PageSize);
                    queryModel.SetQueryableLambda(query);

                    _showItems = query;
                }
            }

            _treeMode = TreeChildren != null && (_showItems?.Any(x => TreeChildren(x).Any()) == true);

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
                .If($"{prefixCls}-ping-left", () => _pingLeft)
                .If($"{prefixCls}-ping-right", () => _pingRight)
                ;
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

            InitializePagination();

            FlushCache();

            ReloadAndInvokeChange();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (_waitingReloadAndInvokeChange)
            {
                _waitingReloadAndInvokeChange = false;
                _waitingReload = false;

                ReloadAndInvokeChange();
            }
            else if (_waitingReload)
            {
                _waitingReload = false;
                Reload();
            }

            if (!firstRender)
            {
                this.FinishLoadPage();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (ScrollX != null)
                {
                    await SetScrollPositionClassName();

                    DomEventService.AddEventListener("window", "resize", OnResize, false);
                    DomEventService.AddEventListener(_tableBodyRef, "scroll", OnScroll);
                }

                if (ScrollY != null && ScrollX != null)
                {
                    await JsInvokeAsync(JSInteropConstants.BindTableHeaderAndBodyScroll, _tableBodyRef, _tableHeaderRef);
                }
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

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

        protected override bool ShouldRender() => this._shouldRender;

        private static void ToggleExpandRow(RowData<TItem> rowData)
        {
            rowData.Expanded = !rowData.Expanded;
        }

        private void RowClick(RowData<TItem> item)
        {
            if (OnRowClick.HasDelegate)
            {
                OnRowClick.InvokeAsync(item);
            }
        }

        void ITable.HasFixLeft() => _hasFixLeft = true;

        void ITable.HasFixRight() => _hasFixRight = true;

        private async void OnResize(JsonElement _) => await SetScrollPositionClassName();

        private async void OnScroll(JsonElement _) => await SetScrollPositionClassName();

        private async Task SetScrollPositionClassName(bool clear = false)
        {
            var element = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _tableBodyRef);
            var scrollWidth = element.scrollWidth;
            var scrollLeft = element.scrollLeft;
            var clientWidth = element.clientWidth;

            var beforePingLeft = _pingLeft;
            var beforePingRight = _pingRight;

            if ((scrollWidth == clientWidth && scrollWidth != 0) || clear)
            {
                _pingLeft = false;
                _pingRight = false;
            }
            else if (scrollLeft == 0)
            {
                _pingLeft = false;
                _pingRight = true;
            }
            else if (scrollWidth == scrollLeft + clientWidth)
            {
                _pingRight = false;
                _pingLeft = true;
            }
            else
            {
                _pingLeft = true;
                _pingRight = true;
            }

            _shouldRender = beforePingLeft != _pingLeft || beforePingRight != _pingRight;
            if (!clear)
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>("window", "resize", OnResize);
            DomEventService.RemoveEventListerner<JsonElement>(_tableBodyRef, "scroll", OnScroll);

            base.Dispose(disposing);
        }

        public async ValueTask DisposeAsync()
        {
            await this.SetScrollPositionClassName(true);
            if (ScrollY != null && ScrollX != null)
            {
                await JsInvokeAsync(JSInteropConstants.UnbindTableHeaderAndBodyScroll, _tableBodyRef);
            }
        }
    }
}
