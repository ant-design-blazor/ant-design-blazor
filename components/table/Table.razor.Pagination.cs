using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem>
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
                SetPaginationClass();
            }
        }

        [Parameter]
        public int Total { get; set; }

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

        private int ActualTotal => Total > _total ? Total : _total;

        private int _total = 0;
        private string _paginationPosition = "bottomRight";
        private string _paginationClass;

        private void SetPaginationClass()
        {
            _paginationClass = $"ant-table-pagination ant-table-pagination-{Regex.Replace(_paginationPosition, "bottom|top", "").ToLowerInvariant()}";
        }

        private void HandlePageIndexChange(PaginationEventArgs args)
        {
            PageIndex = args.PageIndex;

            ReloadAndInvokeChange();

            PageIndexChanged.InvokeAsync(args.PageIndex);
            OnPageIndexChange.InvokeAsync(args);

            _selection.ChangeOnPaging();
        }

        private void HandlePageSizeChange(PaginationEventArgs args)
        {
            PageSize = args.PageSize;

            ReloadAndInvokeChange();

            PageSizeChanged.InvokeAsync(args.PageSize);
            OnPageSizeChange.InvokeAsync(args);

            _selection.ChangeOnPaging();
        }
    }
}
