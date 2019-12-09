using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntMenuBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public int nzInlineIndent { get; set; } = 24;

        [Parameter]
        public string nzTheme { get; set; } = "light";//'light' | 'dark' = 'light';

        [Parameter]
        public NzDirectionVHIType nzMode { get; set; } = NzDirectionVHIType.vertical;

        [Parameter]
        public bool nzInDropDown { get; set; } = false;

        [Parameter]
        public bool nzInlineCollapsed { get; set; } = false;

        [Parameter]
        public bool nzSelectable { get; set; } //= !this.nzMenuService.isInDropDown;

        [Parameter]
        public EventCallback<AntMenuItem> nzClick { get; set; }

        public bool isInDropDown { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        private void SetClassMap()
        {
            string prefixName = isInDropDown ? "ant-dropdown-menu" : "ant-menu";
            ClassMapper.Add(prefixName)
                .Add($"{prefixName}-root")
                .Add($"{prefixName}-{nzTheme}")
                .Add($"{prefixName}-{nzMode}")
                .If($"{prefixName}-inline-collapsed", () => nzInlineCollapsed);
        }
    }
}