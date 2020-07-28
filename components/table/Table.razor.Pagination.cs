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
        public int PageIndex { get; set; } = 1;

        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }

        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter]
        public EventCallback<int> PageSizeChanged { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageIndexChange { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageSizeChange { get; set; }

        private int _total = 0;
        private int _dataSourceCount = 0;
        private string _paginationPosition = "bottomRight";
        private string _paginationClass;

        private void InitializePagination()
        {
            _paginationClass = $"ant-table-pagination ant-table-pagination-{Regex.Replace(_paginationPosition, "bottom|top", "").ToLowerInvariant()}";
        }

        private async Task HandlePageIndexChange(PaginationEventArgs args)
        {
            PageIndex = args.PageIndex;

            await PageIndexChanged.InvokeAsync(args.PageIndex);
            await OnPageIndexChange.InvokeAsync(args);

            ReloadAndInvokeChange();

            StateHasChanged();
        }

        private void HandlePageSizeChange(PaginationEventArgs args)
        {
            PageSize = args.PageSize;

            ReloadAndInvokeChange();

            PageSizeChanged.InvokeAsync(args.PageSize);
            OnPageSizeChange.InvokeAsync(args);
        }
    }
}
