using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntSubMenuBase : AntDomComponentBase
    {
        public int Level { get; set; } = 1;

        [CascadingParameter]
        protected AntMenu Menu { get; set; }

        internal IList<AntMenuItem> Items { get; set; } = new List<AntMenuItem>();

        [CascadingParameter]
        protected AntSubMenu ParentSubMenu { get; set; }

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
            this.isChildMenuSelected = this.Items.Any(x => x.nzSelected);

            string prefixName = Menu.isInDropDown ? "ant-dropdown-menu-submenu" : "ant-menu-submenu";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-disabled", () => nzDisabled)
                .If($"{prefixName}-open", () => this.nzOpen)
                .If($"{prefixName}-selected", () => isChildMenuSelected)
                .Add($"{prefixName}-{Menu.nzMode}")
                .If($"{prefixName}-active", () => isMouseHover && !nzDisabled)
                ;
        }

        protected override void OnInitialized()
        {
            if (this is AntSubMenu subMenu)
            {
                this.Menu?.SubMenus.Add(subMenu);
            }

            this.Level = this.ParentSubMenu?.Level + 1 ?? 1;

            this.open = nzOpen;
            this.SetClassMap();
            base.OnInitialized();
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
                this.nzOpen = !this.nzOpen;
                this.SetClassMap();
            }
        }

        public void SetOpenState(bool value)
        {
            this.nzOpen = value;
            this.open = value;
            StateHasChanged();
        }
    }
}