using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        [Parameter]
        public bool HidePagination { get; set; }

        /// <summary>
        /// topLeft | topCenter | topRight |bottomLeft | bottomCenter | bottomRight
        /// </summary>
        [Parameter]
        public string PaginationPosition
        {
            get => _paginationPosition;
            set
            {
                _paginationPosition = value;
                InitializePagination();
            }
        }

        [Parameter]
        public RenderFragment PaginationTemplate { get; set; }

        [Parameter]
        public int Total
        {
            get => _total > _dataSourceCount ? _total : _dataSourceCount;
            set
            {
                _total = value;
            }
        }

        [Parameter]
        public EventCallback<int> TotalChanged { get; set; }

        [Parameter]
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (_pageIndex != value)
                {
                    _pageIndex = value;
                    _waitingReloadAndInvokeChange = true;
                }
            }
        }

        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }

        [Parameter]
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    _waitingReloadAndInvokeChange = true;
                }
            }
        }

        [Parameter]
        public EventCallback<int> PageSizeChanged { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageIndexChange { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageSizeChange { get; set; }

        private int _total;
        private int _dataSourceCount;
        private string _paginationPosition = "bottomRight";
        private string _paginationClass;
        private int _pageIndex = 1;
        private int _pageSize = 10;

        private void InitializePagination()
        {
            _paginationClass = $"ant-table-pagination ant-table-pagination-{Regex.Replace(_paginationPosition, "bottom|top", "").ToLowerInvariant()}";
        }

        private async Task HandlePageChange(PaginationEventArgs args)
        {
            if (_pageIndex != args.Page)
            {
                await HandlePageIndexChange(args);
            }

            if (_pageSize != args.PageSize)
            {
                await HandlePageSizeChange(args);
            }
        }

        private async Task HandlePageIndexChange(PaginationEventArgs args)
        {
            _pageIndex = args.Page;

            if (PageIndexChanged.HasDelegate)
            {
                await PageIndexChanged.InvokeAsync(args.Page);
            }

            if (OnPageIndexChange.HasDelegate)
            {
                await OnPageIndexChange.InvokeAsync(args);
            }

            ReloadAndInvokeChange();

            StateHasChanged();
        }

        private async Task HandlePageSizeChange(PaginationEventArgs args)
        {
            _pageSize = args.PageSize;

            if (PageSizeChanged.HasDelegate)
            {
                await PageSizeChanged.InvokeAsync(args.PageSize);
            }

            if (OnPageSizeChange.HasDelegate)
            {
                await OnPageSizeChange.InvokeAsync(args);
            }

            ReloadAndInvokeChange();

            StateHasChanged();
        }
    }
}
