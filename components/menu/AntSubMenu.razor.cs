using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntSubMenu : AntDomComponentBase
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
        public string MenuClassName { get; set; }

        [Parameter]
        public int? PaddingLeft { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public bool Open { get; set; } = false;

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public EventCallback<bool> OpenChange { get; set; }

        protected ClassMapper TitleDivClass { get; } = new ClassMapper();

        protected ClassMapper PopupClassMapper { get; } = new ClassMapper();

        protected ClassMapper PopupUlClassMapper { get; } = new ClassMapper();

        private string _placement = "rightTop";

        private bool _isChildMenuSelected = false;
        protected bool IsMouseHover { get; set; } = false;
        private bool _hasOpened;

        private Element _element = new Element();

        private void SetClassMap()
        {
            this._isChildMenuSelected = this.Items.Any(x => x.Selected);

            string prefixName = Menu.IsInDropDown ? "ant-dropdown-menu-submenu" : "ant-menu-submenu";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-disabled", () => Disabled)
                .If($"{prefixName}-open", () => this.Open)
                .If($"{prefixName}-selected", () => _isChildMenuSelected)
                .Add($"{prefixName}-{Menu.Mode}")
                .If($"{prefixName}-active", () => IsMouseHover && !Disabled)
                ;
        }

        protected override void OnInitialized()
        {
            if (this is AntSubMenu subMenu)
            {
                this.Menu?.SubMenus.Add(subMenu);
            }

            this.Level = this.ParentSubMenu?.Level + 1 ?? 1;

            this._hasOpened = Open;
            this.SetClassMap();
            base.OnInitialized();

            PopupClassMapper.Clear()
                .If("ant-menu-light", () => Menu.Theme == "light")
                .If("ant-menu-dark", () => Menu.Theme == "dark")
                .If("ant-menu-submenu-placement-bottomLeft", () => Menu.Mode == AntDirectionVHIType.horizontal)
                .If("ant-menu-submenu-placement-rightTop",
                    () => Menu.Mode == AntDirectionVHIType.vertical && _placement == "rightTop")
                .If("ant-menu-submenu-placement-leftTop",
                    () => Menu.Mode == AntDirectionVHIType.vertical && _placement == "leftTop")
                ;

            PopupUlClassMapper.Clear()
                .If("ant-dropdown-menu", () => Menu.IsInDropDown)
                .If("ant-menu", () => !Menu.IsInDropDown)
                .If("ant-dropdown-menu-vertical", () => Menu.IsInDropDown)
                .If("ant-menu-vertical", () => !Menu.IsInDropDown)
                .If("ant-dropdown-menu-sub", () => Menu.IsInDropDown)
                .If("ant-menu-sub", () => !Menu.IsInDropDown)
                ;

            TitleDivClass
                .If("ant-dropdown-menu-submenu-title", () => Menu.IsInDropDown)
                .If("ant-menu-submenu-title", () => !Menu.IsInDropDown)
                ;
        }

        protected async Task SetMouseEnterState(bool value)
        {
            if (IsMouseHover != value)
            {
                this.IsMouseHover = value;
                this.SetClassMap();

                this._element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Ref);
            }
        }

        protected async Task ClickSubMenuTitle()
        {
            if (Menu.Mode == AntDirectionVHIType.inline && !Menu.IsInDropDown && !this.Disabled)
            {
                this.Open = !this.Open;
                await OpenChange.InvokeAsync(this.Open);
                this.SetClassMap();
            }
        }

        public async Task SetOpenState(bool value)
        {
            this.Open = value;
            this._hasOpened = value;
            await OpenChange.InvokeAsync(this.Open);
            StateHasChanged();
        }
    }
}
