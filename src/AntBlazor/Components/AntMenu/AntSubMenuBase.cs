using System.Collections;
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

        [Inject]
        public JsInterop.JsInterop JsInterop { get; set; }

        protected ClassMapper TitleDivClass { get; } = new ClassMapper();

        protected ClassMapper PopupClassMapper { get; } = new ClassMapper();

        protected ClassMapper PopupUlClassMapper { get; } = new ClassMapper();

        private string placement = "rightTop";
        private int triggerWidth;
        private string expandState = "collapsed";
        private string[] overlayPositions = new[] { "" };
        private bool isChildMenuSelected = false;
        protected bool isMouseHover { get; set; } = false;
        private bool open;

        protected Element element;

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

            PopupClassMapper.Clear()
                .If("ant-menu-light", () => Menu.nzTheme == "light")
                .If("ant-menu-dark", () => Menu.nzTheme == "dark")
                .If("ant-menu-submenu-placement-bottomLeft", () => Menu.nzMode == NzDirectionVHIType.horizontal)
                .If("ant-menu-submenu-placement-rightTop",
                    () => Menu.nzMode == NzDirectionVHIType.vertical && placement == "rightTop")
                .If("ant-menu-submenu-placement-leftTop",
                    () => Menu.nzMode == NzDirectionVHIType.vertical && placement == "leftTop")
                ;

            PopupUlClassMapper.Clear()
                .If("ant-dropdown-menu", () => Menu.isInDropDown)
                .If("ant-menu", () => !Menu.isInDropDown)
                .If("ant-dropdown-menu-vertical", () => Menu.isInDropDown)
                .If("ant-menu-vertical", () => !Menu.isInDropDown)
                .If("ant-dropdown-menu-sub", () => Menu.isInDropDown)
                .If("ant-menu-sub", () => !Menu.isInDropDown)
                ;
        }

        protected void SetMouseEnterState(bool value)
        {
            if (isMouseHover != value)
            {
                this.isMouseHover = value;
                this.SetClassMap();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            this.element = await JsInterop.GetDom(Ref);
        }

        protected async Task ClickSubMenuTitle()
        {
            if (Menu.nzMode == NzDirectionVHIType.inline && !Menu.isInDropDown && !this.nzDisabled)
            {
                this.nzOpen = !this.nzOpen;
                await nzOpenChange.InvokeAsync(this.nzOpen);
                this.SetClassMap();
            }
        }

        public async Task SetOpenState(bool value)
        {
            this.nzOpen = value;
            this.open = value;
            await nzOpenChange.InvokeAsync(this.nzOpen);
            StateHasChanged();
        }
    }
}