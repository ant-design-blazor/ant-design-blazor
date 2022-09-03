using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Breadcrumb : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Not currently used. Planned for future development.
        /// </summary>
        [Parameter]
        public bool AutoGenerate { get; set; } = false;

        [Parameter]
        public string Separator { get; set; } = "/";

        /// <summary>
        /// Not currently used. Planned for future development.
        /// </summary>
        [Parameter]
        public string RouteLabel { get; set; } = "breadcrumb";

        protected override void OnInitialized()
        {
            var prefixCls = "ant-breadcrumb";

            ClassMapper
                .Add(prefixCls)
                .If($"{prefixCls}-rtl", () => RTL);

            base.OnInitialized();
        }
    }
}
