using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem>
    {
        [Parameter]
        public bool ShowPagination { get; set; } = true;

        /// <summary>
        /// topLeft | topCenter | topRight |bottomLeft | bottomCenter | bottomRight
        /// </summary>
        [Parameter]
        public string PaginationPosition { get; set; } = "bottomRight";

        [Parameter]
        public int Total { get; set; }

        [Parameter]
        public EventCallback<int> TotalChanged { get; set; }

        [Parameter]
        public int PageIndex { get; set; }

        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }

        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public EventCallback<int> PageSizeChanged { get; set; }

        [Parameter]
        public bool ClientSide { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageIndexChange { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageSizeChange { get; set; }

        private IEnumerable<TItem> ShowItems => !ClientSide ? DataSource : DataSource.Skip(_pageIndex - 1 * _pageSize).Take(_pageSize);

        private int _pageSize = 10;
        private int _pageIndex = 1;

        private string PaginationClass => $"ant-table-pagination ant-table-pagination-{Regex.Replace(PaginationPosition, "bottom|top", "").ToLowerInvariant()}";

        private void HandlePageIndexChange(PaginationEventArgs args)
        {
            PageIndexChanged.InvokeAsync(args.PageIndex);
            OnPageIndexChange.InvokeAsync(args);
        }

        private void HandlePageSizeChange(PaginationEventArgs args)
        {
            PageSizeChanged.InvokeAsync(args.PageSize);
            OnPageSizeChange.InvokeAsync(args);
        }
    }
}
