using System;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Pagination : AntDomComponentBase
    {
        [Parameter]
        public EventCallback<int> PageSizeChanged { get; set; }

        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageIndexChange { get; set; }

        [Parameter]
        public EventCallback<PaginationEventArgs> OnPageSizeChange { get; set; }

        [Parameter]
        public OneOf<Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext>> ShowTotal { get; set; }

        [Parameter]
        public int Current { get; set; }

        [Parameter]
        public EventCallback<int> CurrentChanged { get; set; }

        /// <summary>
        /// 'default' | 'small'
        /// </summary>
        [Parameter]
        public string Size { get; set; } = "default";

        [Parameter]
        public int[] PageSizeOptions { get; set; } = { 10, 20, 30, 40 };

        [Parameter] public RenderFragment<PaginationItemRenderContext> ItemRender { get; set; } = null;

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool ShowSizeChanger { get; set; }

        [Parameter]
        public bool HideOnSinglePage { get; set; }

        [Parameter]
        public bool ShowQuickJumper { get; set; }

        [Parameter]
        public bool Simple { get; set; }

        [Parameter]
        public bool Responsive { get; set; }

        [Parameter]
        public int Total
        {
            get => _total;
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnTotalChange(value);
                }
            }
        }

        [Parameter]
        public int PageIndex { get; set; } = 1;

        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter] public int DefaultCurrent { get; set; } = 1;

        [Parameter] public int DefaultPageSize { get; set; } = 10;

        [Parameter] public PaginationLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Pagination;

        private bool _showPagination = true;

        private string _size = "default";

        private int _total = 0;

        private void SetClass()
        {
            var clsPrefix = "ant-pagination";
            ClassMapper
                .Add(clsPrefix)
                .If($"{clsPrefix}-simple", () => Simple)
                .If($"{clsPrefix}-disabled", () => Disabled)
                .If($"mini", () => !Simple && Size == "small")
                ;
        }

        protected override void OnInitialized()
        {
            SetClass();
            this.PageIndex = Current > 0 ? Current : DefaultCurrent;
            this.PageSize = DefaultPageSize;
        }

        protected override void OnParametersSet()
        {
            if (this.PageSize <= 0)
            {
                this.PageSize = DefaultPageSize;
            }
        }

        private static int ValidatePageIndex(int value, int lastIndex)
        {
            if (value > lastIndex)
            {
                return lastIndex;
            }
            else if (value < 1)
            {
                return 1;
            }
            else
            {
                return value;
            }
        }

        private void HandlePageIndexChange(int index)
        {
            var lastIndex = GetLastIndex(this.Total, this.PageSize);
            var validIndex = ValidatePageIndex(index, lastIndex);
            if (validIndex != this.PageIndex && !this.Disabled)
            {
                this.PageIndex = validIndex;
                this.PageIndexChanged.InvokeAsync(this.PageIndex);
                this.CurrentChanged.InvokeAsync(this.PageIndex);
                this.OnPageIndexChange.InvokeAsync(new PaginationEventArgs()
                {
                    PageCount = lastIndex,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    Total = Total
                });
            }
        }

        private void HandlePageSizeChange(int size)
        {
            this.PageSize = size;
            this.PageSizeChanged.InvokeAsync(size);
            var lastIndex = GetLastIndex(this.Total, this.PageSize);
            if (this.PageIndex > lastIndex)
            {
                this.HandlePageIndexChange(lastIndex);
            }
            else
            {
                this.OnPageSizeChange.InvokeAsync(new PaginationEventArgs()
                {
                    PageCount = lastIndex,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    Total = Total
                });
            }
        }

        private void OnTotalChange(int total)
        {
            var lastIndex = GetLastIndex(total, this.PageSize);
            if (this.PageIndex > lastIndex)
            {
                this.HandlePageIndexChange(lastIndex);
            }
        }

        private static int GetLastIndex(int total, int pageSize)
        {
            return (total - 1) / pageSize + 1;
        }
    }

    public class PaginationTotalContext
    {
        public int Total { get; set; }

        public (int, int) Range { get; set; }
    }

    public class PaginationItemRenderContext
    {
        /// <summary>
        ///  'page' | 'prev' | 'next' | 'prev_5' | 'next_5'
        /// </summary>
        public string Type { get; set; }

        public int Page { get; set; }

        public RenderFragment<PaginationItemRenderContext> DefaultRender { get; set; }
    }
}
