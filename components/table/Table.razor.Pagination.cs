using System;
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
                _paginationPositionSet = true;
            }
        }

        [Parameter]
        public RenderFragment<(int PageSize, int PageIndex, int Total, string PaginationClass, EventCallback<PaginationEventArgs> HandlePageChange)> PaginationTemplate { get; set; }

        [Parameter]
        public int Total { get; set; }

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
        private ClassMapper _paginationClass = new ClassMapper();
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private int _startIndex = 0;
        private string _paginationPosition = "bottomRight";
        private bool _paginationPositionSet;

        private void InitializePagination()
        {
            _paginationClass
                .Add($"ant-table-pagination")
                .GetIf(() => $"ant-table-pagination-{Regex.Replace(_paginationPosition, "bottom|top", "").ToLowerInvariant()}", () => _paginationPositionSet)
                .If("ant-table-pagination-left", () => RTL && !_paginationPositionSet);
        }

        private async Task HandlePageChange(PaginationEventArgs args)
        {
            bool shouldInvokeChange = false;

            if (_pageSize != args.PageSize)
            {
                shouldInvokeChange = true;
                await HandlePageSizeChange(args);
            }

            if (_pageIndex != args.Page)
            {
                shouldInvokeChange = true;
                await HandlePageIndexChange(args);
            }

            if (shouldInvokeChange)
            {
                ReloadAndInvokeChange();

                StateHasChanged();
            }
        }

        private async Task HandlePageIndexChange(PaginationEventArgs args)
        {
            FlushCache();

            _pageIndex = args.Page;

            if (PageIndexChanged.HasDelegate)
            {
                await PageIndexChanged.InvokeAsync(args.Page);
            }

            if (OnPageIndexChange.HasDelegate)
            {
                await OnPageIndexChange.InvokeAsync(args);
            }
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
        }

        private void ChangePageSize(int pageSize)
        {
            pageSize = Math.Max(1, pageSize);

            if (_pageSize == pageSize)
            {
                return;
            }

            _pageSize = pageSize;

            if (PageSizeChanged.HasDelegate)
            {
                PageSizeChanged.InvokeAsync(_pageSize);
            }
        }

        private void ChangePageIndex(int pageIndex)
        {
            pageIndex = Math.Max(1, pageIndex);

            if (_pageIndex == pageIndex)
            {
                return;
            }

            _pageIndex = pageIndex;

            if (PageIndexChanged.HasDelegate)
            {
                PageIndexChanged.InvokeAsync(_pageIndex);
            }
        }
    }
}
