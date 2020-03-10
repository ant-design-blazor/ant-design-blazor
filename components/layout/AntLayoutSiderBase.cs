using System.Collections;
using System.Threading.Tasks;
using AntBlazor.core;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntLayoutSiderBase : AntDomComponentBase
    {
        private string _currentWidth, _customWidth = "256";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string adWidth { get => _currentWidth; set { _currentWidth = _customWidth = value; } }

        /// <summary>
        /// use to fix the user has long content, but they don't want to scroll all body.
        /// </summary>
        [Parameter]
        public bool longBodyMode { get; set; } = false;

        [Parameter]
        public string adTheme { get; set; } = "dark";

        [Parameter]
        public int adCollapsedWidth { get; set; } = 80;

        /// <summary>
        /// "xs" | "sm" | "md" | "lg" | "xl" | "xxl"
        /// </summary>
        [Parameter]
        public string adBreakpoint { get; set; }

        [Parameter]
        public RenderFragment adZeroTrigger { get; set; }

        [Parameter]
        public bool adReverseArrow { get; set; } = false;

        [Parameter]
        public bool adCollapsible { get; set; } = false;

        [Parameter]
        public bool adCollapsed { get; set; } = false;

        [Parameter]
        public RenderFragment adTrigger { get; set; }

        [CascadingParameter]
        public AntLayout Layout { get; set; }

        [Parameter]
        public EventCallback<bool> onCollapsedChange { get; set; }

        [Inject] private DomEventService domEventService { get; set; }

        protected string widthSetting => this.adCollapsed ? $"{this.adCollapsedWidth}px" : StyleHelper.ToCssPixel(adWidth);

        private string flexSetting => $"0 0 {widthSetting}";

        protected string style =>
                    $@"flex:{flexSetting};
                    max-width:{widthSetting};
                    min-width:{widthSetting};
                    width:{widthSetting};
                    {(longBodyMode ? ";overflow:auto;height:100vh !important;position:fixed;left:0;" : "")}";

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
                .If("ant-layout-sider-zero-width", () => adCollapsed && adCollapsedWidth == 0)
                .If("ant-layout-sider-light", () => adTheme == "light")
                .If("ant-layout-sider-collapsed", () => adCollapsed)
                ;
        }

        protected override async Task OnInitializedAsync()
        {
            domEventService.onResize += async () => await watchMatchMedia();
            domEventService.RegisterResizeListener();
            await base.OnInitializedAsync();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await watchMatchMedia();
            await base.OnFirstAfterRenderAsync();
        }

        public async Task toggleCollapse()
        {
            this.adCollapsed = !this.adCollapsed;
            await onCollapsedChange.InvokeAsync(adCollapsed);
        }

        public async Task watchMatchMedia()
        {
            if (string.IsNullOrEmpty(adBreakpoint))
                return;

            var matchBelow = await JsInvokeAsync<bool>(JSInteropConstants.antMatchMedia, $"(max-width: {dimensionMap[adBreakpoint]})");
            this.below = matchBelow;
            this.adCollapsed = matchBelow;
            if (matchBelow)
            {
                this._currentWidth = "100%";
            }
            else
            {
                this._currentWidth = this._customWidth;
            }
            await this.onCollapsedChange.InvokeAsync(matchBelow);
        }
    }
}