﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AntDesign.TableModels;
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

        public PaginationModel Pagination { get; private set; }

        private int _total = 0;
        private int _dataSourceCount = 0;
        private string _paginationPosition = "bottomRight";
        private string _paginationClass;

        private void InitializePagination()
        {
            Pagination ??= new PaginationModel()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Total = Total
            };
            _paginationClass = $"ant-table-pagination ant-table-pagination-{Regex.Replace(_paginationPosition, "bottom|top", "").ToLowerInvariant()}";
        }

        private void HandlePageIndexChange(PaginationEventArgs args)
        {
            PageIndex = args.PageIndex;
            Pagination.PageIndex = args.PageIndex;

            ReloadAndInvokeChange();

            PageIndexChanged.InvokeAsync(args.PageIndex);
            OnPageIndexChange.InvokeAsync(args);
        }

        private void HandlePageSizeChange(PaginationEventArgs args)
        {
            PageSize = args.PageSize;
            Pagination.PageSize = args.PageSize;

            ReloadAndInvokeChange();

            PageSizeChanged.InvokeAsync(args.PageSize);
            OnPageSizeChange.InvokeAsync(args);
        }
    }
}
