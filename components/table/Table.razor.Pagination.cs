// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : ITable
    {
        /// <summary>
        /// Whether to hide pagination or not
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
        /// Currently visible page
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
