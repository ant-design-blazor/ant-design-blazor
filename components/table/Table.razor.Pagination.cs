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
        public bool HidePagination { get; set; }

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

        private IEnumerable<TItem> ShowItems => Total > 0 ? DataSource : DataSource.Skip((PageIndex - 1) * PageSize).Take(PageSize);

        private int ActualTotal => Total > 0 ? Total : _total;

        private int _total = 0;

        private string PaginationClass => $"ant-table-pagination ant-table-pagination-{Regex.Replace(PaginationPosition, "bottom|top", "").ToLowerInvariant()}";

        private void HandlePageIndexChange(PaginationEventArgs args)
        {
            PageIndexChanged.InvokeAsync(args.PageIndex);
            OnPageIndexChange.InvokeAsync(args);

            ChangeSelection(null);
        }

        private void HandlePageSizeChange(PaginationEventArgs args)
        {
            PageSizeChanged.InvokeAsync(args.PageSize);
            OnPageSizeChange.InvokeAsync(args);
        }
    }
}
