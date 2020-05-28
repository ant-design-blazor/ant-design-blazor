using System;
using System.Collections;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Sider : AntDomComponentBase
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
        public Layout Layout { get; set; }

        [Parameter]
        public EventCallback<bool> OnCollapse { get; set; }

        [Parameter]
        public EventCallback<bool> OnBreakpoint { get; set; }

        [Inject]
        private DomEventService DomEventService { get; set; }

        protected string WidthSetting => this.Collapsed ? $"{this.CollapsedWidth}px" : StyleHelper.ToCssPixel(Width);

        private string FlexSetting => $"0 0 {WidthSetting}";

        public event Action<bool> OnCollapsed;

        private string InternalStyle =>
            $@"flex:{FlexSetting};
               max-width:{WidthSetting};
               min-width:{WidthSetting};
               width:{WidthSetting};
           ";

        private bool Below { get; set; }

        private readonly Hashtable _dimensionMap = new Hashtable()
        {
            ["xs"] = "480px",
            ["sm"] = "576px",
            ["md"] = "768px",
            ["lg"] = "992px",
            ["xl"] = "1200px",
            ["xxl"] = "1600px"
        };

        bool IsZeroTrigger => this.Collapsible && this.SiderTrigger != null && this.CollapsedWidth == 0 && ((this.Breakpoint != null && this.Below) || this.Breakpoint == null);

        bool IsSiderTrigger => this.Collapsible && this.SiderTrigger != null && this.CollapsedWidth != 0;

        RenderFragment SiderTrigger => Trigger ?? defaultTrigger(this);

        private void SetClass()
        {
            var prefixCls = "ant-layout-sider";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-zero-width", () => Collapsed && CollapsedWidth == 0)
                .If($"{prefixCls}-light", () => Theme == "light")
                .If($"{prefixCls}-collapsed", () => Collapsed)
                .If($"{prefixCls}-has-trigger", () => IsSiderTrigger)
                ;
        }

        protected override async Task OnInitializedAsync()
        {
            Layout?.HasSider();
            SetClass();
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
            await OnCollapse.InvokeAsync(Collapsed);
            OnCollapsed?.Invoke(this.Collapsed);
        }

        public async Task WatchMatchMedia()
        {
            if (string.IsNullOrEmpty(Breakpoint))
                return;

            bool matchBelow = await JsInvokeAsync<bool>(JSInteropConstants.matchMedia, $"(max-width: {_dimensionMap[Breakpoint]})");
            this.Below = matchBelow;
            this.Collapsed = matchBelow;
            await this.OnCollapse.InvokeAsync(matchBelow);
            OnCollapsed?.Invoke(this.Collapsed);
            await OnBreakpoint.InvokeAsync(matchBelow);
        }
    }
}
