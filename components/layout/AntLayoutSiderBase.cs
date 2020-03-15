using System.Collections;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntLayoutSiderBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public string width { get; set; } = "200";

        /// <summary>
        ///  'light' | 'dark'
        /// </summary>
        [Parameter]
        public string theme { get; set; } = "dark";

        [Parameter]
        public int collapsedWidth { get; set; } = 80;

        /// <summary>
        /// "xs" | "sm" | "md" | "lg" | "xl" | "xxl"
        /// </summary>
        [Parameter]
        public string breakpoint { get; set; }

        [Parameter]
        public RenderFragment zeroTrigger { get; set; }

        [Parameter]
        public bool reverseArrow { get; set; } = false;

        [Parameter]
        public bool collapsible { get; set; } = false;

        [Parameter]
        public bool collapsed { get; set; } = false;

        [Parameter]
        public RenderFragment trigger { get; set; }

        [CascadingParameter]
        public AntLayout Layout { get; set; }

        [Parameter]
        public EventCallback<bool> onCollapsedChange { get; set; }

        [Inject] private DomEventService domEventService { get; set; }

        protected string widthSetting => this.collapsed ? $"{this.collapsedWidth}px" : StyleHelper.ToCssPixel(width);

        private string flexSetting => $"0 0 {widthSetting}";

        protected string style =>
            $@"flex:{flexSetting};
                    max-width:{widthSetting};
                    min-width:{widthSetting};
                    width:{widthSetting};
           ";

        protected bool below { get; set; }

        private Hashtable dimensionMap = new Hashtable()
        {
            ["xs"] = "480px",
            ["sm"] = "576px",
            ["md"] = "768px",
            ["lg"] = "992px",
            ["xl"] = "1200px",
            ["xxl"] = "1600px"
        };

        public AntLayoutSiderBase()
        {
            ClassMapper.Add("ant-layout-sider")
                .If("ant-layout-sider-zero-width", () => collapsed && collapsedWidth == 0)
                .If("ant-layout-sider-light", () => theme == "light")
                .If("ant-layout-sider-collapsed", () => collapsed)
                ;
        }

        protected override async Task OnInitializedAsync()
        {
            domEventService.AddEventListener<object>("window", "resize", async _ => await watchMatchMedia());
            await base.OnInitializedAsync();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await watchMatchMedia();
            await base.OnFirstAfterRenderAsync();
        }

        public async Task toggleCollapse()
        {
            this.collapsed = !this.collapsed;
            await onCollapsedChange.InvokeAsync(collapsed);
        }

        public async Task watchMatchMedia()
        {
            if (string.IsNullOrEmpty(breakpoint))
                return;

            var matchBelow = await JsInvokeAsync<bool>(JSInteropConstants.matchMedia, $"(max-width: {dimensionMap[breakpoint]})");
            this.below = matchBelow;
            this.collapsed = matchBelow;
            await this.onCollapsedChange.InvokeAsync(matchBelow);
        }
    }
}