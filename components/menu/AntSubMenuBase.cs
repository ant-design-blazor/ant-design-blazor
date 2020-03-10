using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
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
        public string menuClassName { get; set; }

        [Parameter]
        public int? paddingLeft { get; set; }

        [Parameter]
        public string title { get; set; }

        [Parameter]
        public string icon { get; set; }

        [Parameter]
        public bool open { get; set; } = false;

        [Parameter]
        public bool disabled { get; set; } = false;

        [Parameter]
        public EventCallback<bool> openChange { get; set; }

        protected ClassMapper TitleDivClass { get; } = new ClassMapper();

        protected ClassMapper PopupClassMapper { get; } = new ClassMapper();

        protected ClassMapper PopupUlClassMapper { get; } = new ClassMapper();

        private string placement = "rightTop";
        private int triggerWidth;
        private string expandState = "collapsed";
        private string[] overlayPositions = new[] { "" };
        private bool isChildMenuSelected = false;
        protected bool isMouseHover { get; set; } = false;
        private bool _hasOpened;

        protected Element element = new Element();

        private void SetClassMap()
        {
            this.isChildMenuSelected = this.Items.Any(x => x.selected);

            string prefixName = Menu.isInDropDown ? "ant-dropdown-menu-submenu" : "ant-menu-submenu";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-disabled", () => disabled)
                .If($"{prefixName}-open", () => this.open)
                .If($"{prefixName}-selected", () => isChildMenuSelected)
                .Add($"{prefixName}-{Menu.mode}")
                .If($"{prefixName}-active", () => isMouseHover && !disabled)
                ;
        }

        protected override void OnInitialized()
        {
            if (this is AntSubMenu subMenu)
            {
                this.Menu?.SubMenus.Add(subMenu);
            }

            this.Level = this.ParentSubMenu?.Level + 1 ?? 1;

            this._hasOpened = open;
            this.SetClassMap();
            base.OnInitialized();

            PopupClassMapper.Clear()
                .If("ant-menu-light", () => Menu.theme == "light")
                .If("ant-menu-dark", () => Menu.theme == "dark")
                .If("ant-menu-submenu-placement-bottomLeft", () => Menu.mode == AntDirectionVHIType.horizontal)
                .If("ant-menu-submenu-placement-rightTop",
                    () => Menu.mode == AntDirectionVHIType.vertical && placement == "rightTop")
                .If("ant-menu-submenu-placement-leftTop",
                    () => Menu.mode == AntDirectionVHIType.vertical && placement == "leftTop")
                ;

            PopupUlClassMapper.Clear()
                .If("ant-dropdown-menu", () => Menu.isInDropDown)
                .If("ant-menu", () => !Menu.isInDropDown)
                .If("ant-dropdown-menu-vertical", () => Menu.isInDropDown)
                .If("ant-menu-vertical", () => !Menu.isInDropDown)
                .If("ant-dropdown-menu-sub", () => Menu.isInDropDown)
                .If("ant-menu-sub", () => !Menu.isInDropDown)
                ;

            TitleDivClass
                .If("ant-dropdown-menu-submenu-title", () => Menu.isInDropDown)
                .If("ant-menu-submenu-title", () => !Menu.isInDropDown)
                ;
        }

        protected async Task SetMouseEnterState(bool value)
        {
            if (isMouseHover != value)
            {
                this.isMouseHover = value;
                this.SetClassMap();

                this.element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Ref);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            this.open = this._hasOpened;
        }

        protected async Task ClickSubMenuTitle()
        {
            if (Menu.mode == AntDirectionVHIType.inline && !Menu.isInDropDown && !this.disabled)
            {
                this.open = !this.open;
                this._hasOpened = this.open;
                await openChange.InvokeAsync(this.open);
                this.SetClassMap();
            }
        }

        public async Task SetOpenState(bool value)
        {
            this.open = value;
            this._hasOpened = value;
            await openChange.InvokeAsync(this.open);
            StateHasChanged();
        }
    }
}