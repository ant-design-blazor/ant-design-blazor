using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntBlazor
{
    public partial class SubMenu : AntDomComponentBase
    {
        private const string PrefixCls = "ant-menu-submenu";

        [CascadingParameter]
        public Menu RootMenu { get; set; }

        [CascadingParameter]
        public SubMenu Parent { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Key
        {
            get => _key ?? Id;
            set => _key = value;
        }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnTitleClicked { get; set; }

        private ClassMapper SubMenuMapper { get; } = new ClassMapper();

        public bool IsOpen { get; private set; }

        private string _key;

        private Dropdown _dropdown;

        private void SetClass()
        {
            ClassMapper
                .Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-{RootMenu.InternalMode}")
                .If($"{PrefixCls}-disabled", () => Disabled)
                .If($"{PrefixCls}-open", () => RootMenu.InternalMode == MenuMode.Inline && IsOpen)
                ;

            SubMenuMapper
                .Clear()
                .Add("ant-menu")
                .Add("ant-menu-sub")
                .Add($"ant-menu-{(RootMenu.InternalMode == MenuMode.Horizontal ? MenuMode.Vertical : RootMenu.InternalMode)}")
                .If($"ant-menu-submenu-popup", () => RootMenu.InternalMode != MenuMode.Inline)
                .If($"ant-menu-hidden", () => RootMenu.InternalMode == MenuMode.Inline && !IsOpen)
                ;
        }

        private async Task HandleOnTitleClick(MouseEventArgs args)
        {
            RootMenu.SelectSubmenu(this);
            if (OnTitleClicked.HasDelegate)
                await OnTitleClicked.InvokeAsync(args);
        }

        public async Task Collapse()
        {
            await Task.Delay(300);
            IsOpen = false;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();

            RootMenu.Submenus.Add(this);

            if (RootMenu.DefaultOpenKeys.Contains(Key))
                IsOpen = true;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClass();

            if (RootMenu.OpenKeys.Contains(Key))
                IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Open()
        {
            if (Disabled)
            {
                return;
            }

            IsOpen = true;
            if (Parent != null)
            {
                Parent.Open();
            }
        }

        public Dropdown GetDropdown()
        {
            return _dropdown;
        }

        private async Task OnDropdownVisibleChange(bool visible)
        {
            await UpdateParentOverlayState(visible);
        }

        private async Task UpdateParentOverlayState(bool visible)
        {
            if (Parent == null)
            {
                return;
            }

            Dropdown parentDropdown = Parent.GetDropdown();

            if (parentDropdown == null)
            {
                return;
            }

            parentDropdown.GetDropdownOverlay().UpdateChildState(visible);

            if (!visible)
            {
                await parentDropdown.Hide();
            }
        }
    }
}
