using System.Collections;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntSider : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] 
        public string Width { get; set; } = "200";

        /// <summary>
        ///  'light' | 'dark'
        /// </summary>
        [Parameter]
        public string Theme { get; set; } = "dark";

        [Parameter]
        public int CollapsedWidth { get; set; } = 80;

        /// <summary>
        /// "xs" | "sm" | "md" | "lg" | "xl" | "xxl"
        /// </summary>
        [Parameter]
        public string Breakpoint { get; set; }

        [Parameter]
        public RenderFragment ZeroTrigger { get; set; }

        [Parameter]
        public bool ReverseArrow { get; set; } = false;

        [Parameter]
        public bool Collapsible { get; set; } = false;

        [Parameter]
        public bool Collapsed { get; set; } = false;

        [Parameter]
        public RenderFragment Trigger { get; set; }

        [CascadingParameter]
        public AntLayout Layout { get; set; }

        [Parameter]
        public EventCallback<bool> OnCollapsedChange { get; set; }

        [Inject] 
        private DomEventService DomEventService { get; set; }

        protected string WidthSetting => this.Collapsed ? $"{this.CollapsedWidth}px" : StyleHelper.ToCssPixel(Width);

        private string FlexSetting => $"0 0 {WidthSetting}";

        private string Style =>
            $@"flex:{FlexSetting};
               max-width:{WidthSetting};
               min-width:{WidthSetting};
               width:{WidthSetting};
           ";

        private bool Below { get; set; }

        private Hashtable _dimensionMap = new Hashtable()
        {
            ["xs"] = "480px",
            ["sm"] = "576px",
            ["md"] = "768px",
            ["lg"] = "992px",
            ["xl"] = "1200px",
            ["xxl"] = "1600px"
        };

        public AntSider()
        {
            ClassMapper.Add("ant-layout-sider")
                .If("ant-layout-sider-zero-width", () => Collapsed && CollapsedWidth == 0)
                .If("ant-layout-sider-light", () => Theme == "light")
                .If("ant-layout-sider-collapsed", () => Collapsed)
                ;
        }

        protected override async Task OnInitializedAsync()
        {
            DomEventService.AddEventListener<object>("window", "resize", async _ => await WatchMatchMedia());
            await base.OnInitializedAsync();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await WatchMatchMedia();
            await base.OnFirstAfterRenderAsync();
        }

        public async Task ToggleCollapse()
        {
            this.Collapsed = !this.Collapsed;
            await OnCollapsedChange.InvokeAsync(Collapsed);
        }

        public async Task WatchMatchMedia()
        {
            if (string.IsNullOrEmpty(Breakpoint))
                return;

            var matchBelow = await JsInvokeAsync<bool>(JSInteropConstants.matchMedia, $"(max-width: {_dimensionMap[Breakpoint]})");
            this.Below = matchBelow;
            this.Collapsed = matchBelow;
            await this.OnCollapsedChange.InvokeAsync(matchBelow);
        }
    }
}
