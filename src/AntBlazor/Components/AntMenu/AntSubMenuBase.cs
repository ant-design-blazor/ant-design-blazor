using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntSubMenuBase : AntDomComponentBase
    {
        public int Level { get; set; } = 1;

        [CascadingParameter]
        protected AntMenu Menu { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string nzMenuClassName { get; set; }

        [Parameter]
        public int? nzPaddingLeft { get; set; }

        [Parameter]
        public string nzTitle { get; set; }

        [Parameter]
        public string nzIcon { get; set; }

        [Parameter]
        public bool nzOpen { get; set; } = false;

        [Parameter]
        public bool nzDisabled { get; set; } = false;

        [Parameter]
        public EventCallback<bool> nzOpenChange { get; set; }

        protected ClassMapper TitleDivClass { get; } = new ClassMapper();

        private string placement = "rightTop";
        private int triggerWidth;
        private string expandState = "collapsed";
        private string[] overlayPositions = new[] { "" };
        private bool isChildMenuSelected = false;
        private bool isMouseHover = false;
        private bool open;

        private void SetClassMap()
        {
            string prefixName = Menu.isInDropDown ? "ant-dropdown-menu-submenu" : "ant-menu-submenu";
            ClassMapper.Add(prefixName)
                .If($"{prefixName}-disabled", () => nzDisabled)
                .If($"{prefixName}-open", () => nzOpen)
                .If($"{prefixName}-selected", () => isChildMenuSelected)
                .Add($"{prefixName}-{Menu.nzMode}")
                .If($"{prefixName}-active", () => isMouseHover && !nzDisabled)
                ;
        }

        protected override void OnInitialized()
        {
            this.SetClassMap();
            this.open = nzOpen;
            base.OnInitializedAsync();
        }

        protected void setMouseEnterState(bool value)
        {
            this.isMouseHover = value;
            this.SetClassMap();
        }

        protected void clickSubMenuTitle()
        {
            if (Menu.nzMode == NzDirectionVHIType.inline && !Menu.isInDropDown && !this.nzDisabled)
            {
                open = !this.nzOpen;
            }
        }
    }
}