using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class PaginationItem : AntDomComponentBase
    {
        /// <summary>
        /// 'page' | 'prev' | 'next' | 'prev_5' | 'next_5'
        /// </summary>
        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Active { get; set; }

        [Parameter]
        public int Index { get; set; }

        [Parameter]
        public EventCallback<int> DiffIndex { get; set; }

        [Parameter]
        public EventCallback<int> GotoIndex { get; set; }

        [CascadingParameter]
        public Pagination Pagination { get; set; }

        private RenderFragment<PaginationItemRenderContext> _itemRender;

        private PaginationItemRenderContext ItemRenderContext => new PaginationItemRenderContext() { Implicit = Type, Page = Index };

        private string _title;

        private void SetClassMap()
        {
            var cls = "ant-pagination";

            ClassMapper
                .If($"{cls}-prev", () => Type == "prev")
                .If($"{cls}-next", () => Type == "next")
                .If($"{cls}-item", () => Type == "page")
                .If($"{cls}-jump-prev", () => Type == "prev_5")
                .If($"{cls}-jump-prev-custom-icon", () => Type == "prev_5")
                .If($"{cls}-jump-next", () => Type == "next_5")
                .If($"{cls}-jump-next-custom-icon", () => Type == "next_5")
                .If($"{cls}-disabled", () => Disabled)
                .If($"{cls}-item-active", () => Active)
                ;
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            this._itemRender = Pagination?.ItemRender;

            base.OnInitialized();
        }

        private void ClickItem(MouseEventArgs args)
        {
        }
    }
}
