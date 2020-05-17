using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class PaginationSimple : AntDomComponentBase
    {
        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public object Locale { get; set; } = false;

        [Parameter]
        public int Total { get; set; } = 0;

        [Parameter]
        public int PageIndex { get; set; } = 1;

        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter]
        public EventCallback<int> PageIndexChange { get; set; }

        [CascadingParameter]
        public Pagination Pagination { get; set; }

        private RenderFragment<PaginationItemRenderContext> _itemRender;

        private int _lastIndex = 0;
        private bool _isFirstIndex = false;
        private bool _isLastIndex = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _itemRender = Pagination?.ItemRender;
        }

        public void JumpToPageViaInput(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
            }
        }

        private void PrePage()
        {
            this.OnPageIndexChange(this.PageIndex - 1);
        }

        private void NextPage()
        {
            this.OnPageIndexChange(this.PageIndex + 1);
        }

        private void OnPageIndexChange(int index)
        {
            //this.pageIndexChange.next(index);
        }

        private void UpdateBindingValue()
        {
            //this.lastIndex = Math.ceil(this.total / this.pageSize);
            //this.isFirstIndex = this.pageIndex === 1;
            //this.isLastIndex = this.pageIndex === this.lastIndex;
        }
    }
}
