using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
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

        [Parameter]
        public EventCallback<int> OnClick { get; set; }

        [CascadingParameter]
        public Pagination Pagination { get; set; }

        private RenderFragment<PaginationItemRenderContext> _itemRender;

        private PaginationItemRenderContext ItemRenderContext => new PaginationItemRenderContext() { Type = Type, Page = Index, DefaultRender = _renderItemTemplate };

        private string Key => $"{Type}-{Index}";

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

        protected override void OnParametersSet()
        {
            _title = this.Type switch
            {
                "page" => $"{this.Index}",
                "next" => Pagination.Locale.NextPage,
                "prev" => Pagination.Locale.PrevPage,
                "prev_5" => Pagination.Locale.Prev5,
                "next_5" => Pagination.Locale.Next5,
                _ => ""
            };

            base.OnParametersSet();
        }

        private void ClickItem(MouseEventArgs args)
        {
            if (Disabled)
                return;

            if (Type == "page")
            {
                this.GotoIndex.InvokeAsync(this.Index);
                this.OnClick.InvokeAsync(this.Index);
            }
            else
            {
                var index = Type switch
                {
                    "next" => 1,
                    "prev" => -1,
                    "prev_5" => -5,
                    "next_5" => 5,
                    _ => 0
                };

                this.DiffIndex.InvokeAsync(index);
                this.OnClick.InvokeAsync(index);
            }
        }
    }
}
