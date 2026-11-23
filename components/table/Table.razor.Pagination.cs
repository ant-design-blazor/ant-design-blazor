// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        /// <summary>Props collection key for the table's top pagination.</summary>
        public const string TopPaginationId = "ant-table-top-pagination";

        /// <summary>Props collection key for the table's bottom pagination.</summary>
        public const string BottomPaginationId = "ant-table-bottom-pagination";

        /// <summary>
        /// Whether to hide pagination or not.
        /// <para> Note: When the HidePagination is set to true, PageSize should not be set, so all the data will be displayed. Otherwise, if PageSize is set, the number of rows per page will be displayed according to PageSize. This requires the user to handle the logic of the pagging themselves. </para>
        /// </summary>
        [Parameter]
        public bool HidePagination { get; set; }

        /// <summary>
        /// Position of the pagination. Valid values: topLeft, topCenter, topRight, bottomLeft, bottomCenter, bottomRight
        /// </summary>
        /// <default value="bottomRight"/>
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

        /// <summary>
        /// Controls pagination placement independently above and below the table. Supported values include
        /// topStart, topCenter, topEnd, bottomStart, bottomCenter, bottomEnd, and none.
        /// </summary>
        [Parameter]
        public string[] PaginationPlacement { get; set; }

        /// <summary>
        /// Custom rendering for pagination
        /// </summary>
        [Parameter]
        public RenderFragment<(int PageSize, int PageIndex, int Total, string PaginationClass, EventCallback<PaginationEventArgs> HandlePageChange)> PaginationTemplate { get; set; }

        /// <summary>
        /// Total records
        /// </summary>
        [Parameter]
        public int Total { get; set; }

        /// <summary>
        /// Callback executed when total records changes
        /// </summary>
        [Parameter]
        public EventCallback<int> TotalChanged { get; set; }

        /// <summary>
        /// Currently visible page. The first page is 1. If it is set to less than 1, the <see cref="OnChange"/> callback won't be invoked.
        /// </summary>
        /// <default value="1"/>
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

        /// <summary>
        /// Callback executed when currently visible page changes
        /// </summary>
        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }

        /// <summary>
        /// Number of records per page
        /// </summary>
        /// <default value="10"/>
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

        /// <summary>
        /// Callback executed when page size changes
        /// </summary>
        [Parameter]
        public EventCallback<int> PageSizeChanged { get; set; }

        /// <inheritdoc cref="PageIndexChanged"/>
        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageIndexChange { get; set; }

        /// <inheritdoc cref="PageSizeChanged"/>
        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageSizeChange { get; set; }

        private int _total;
        private int _dataSourceCount;
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private int _startIndex = 0;
        private string _paginationPosition = "bottomRight";
        private bool _paginationPositionSet;

        private bool ShowTopPagination => !HidePagination && _total > 0 && GetPaginationPlacement(true) != null;

        private bool ShowBottomPagination => !HidePagination && _total > 0 && GetPaginationPlacement(false) != null;

        private string GetPaginationPlacement(bool top)
        {
            if (PaginationPlacement == null)
            {
                return PaginationPosition.IndexOf(top ? "top" : "bottom", StringComparison.OrdinalIgnoreCase) >= 0
                    ? PaginationPosition
                    : null;
            }

            var direction = top ? "top" : "bottom";
            var placement = PaginationPlacement.FirstOrDefault(item => item != null && item.IndexOf(direction, StringComparison.OrdinalIgnoreCase) >= 0);
            if (placement != null)
            {
                return placement;
            }

            if (top || (PaginationPlacement.Length > 0 && PaginationPlacement.All(item => string.Equals(item, "none", StringComparison.OrdinalIgnoreCase))))
            {
                return null;
            }

            return PaginationPlacement.Any(item => item != null && (item.IndexOf("top", StringComparison.OrdinalIgnoreCase) >= 0 || item.IndexOf("bottom", StringComparison.OrdinalIgnoreCase) >= 0))
                ? null
                : "bottomEnd";
        }

        private string GetPaginationClass(bool top)
        {
            var placement = GetPaginationPlacement(top) ?? PaginationPosition;
            var alignment = PaginationPlacement == null && !_paginationPositionSet && RTL
                ? "start"
                : Regex.Replace(placement, "bottom|top", "", RegexOptions.IgnoreCase).ToLowerInvariant() switch
            {
                "left" or "start" => "start",
                "center" => "center",
                _ => "end"
            };

            return $"ant-table-pagination ant-table-pagination-{alignment}";
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
                RequestScrollToFirstRow();
                ReloadAndInvokeChange();

                StateHasChanged();
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
