using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntLayoutSiderBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string adWidth { get; set; }

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

        [Parameter]
        public EventCallback<bool> onCollapsedChange { get; set; }

        protected string widthSetting => this.adCollapsed ? $"{this.adCollapsedWidth}px" : StyleHelper.ToCssPixel(adWidth);

        private string flexSetting => $"0 0 ${widthSetting}";

        protected string style =>
                    $@"flex:{flexSetting}
                    max-width:{widthSetting};
                    min-width:{widthSetting};
                    width:{widthSetting};";

        public AntLayoutSiderBase()
        {
            ClassMapper.Add("ant-layout-sider")
                .If("ant-layout-sider-zero-width", () => adCollapsed && adCollapsedWidth == 0)
                .If("ant-layout-sider-light", () => adTheme == "light")
                .If("ant-layout-sider-collapsed", () => adCollapsed)
                ;
        }

        public async Task toggleCollapse()
        {
            this.adCollapsed = !this.adCollapsed;
            await onCollapsedChange.InvokeAsync(adCollapsed);
        }
    }
}