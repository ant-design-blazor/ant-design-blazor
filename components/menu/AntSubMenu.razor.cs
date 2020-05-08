using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSubMenu : AntDomComponentBase
    {
        private const string PrefixCls = "ant-menu-submenu";

        [CascadingParameter]
        public AntMenu RootMenu { get; set; }

        [CascadingParameter]
        public AntSubMenu Parent { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public RenderFragment Children { get; set; }

        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnTitleClicked { get; set; }

        private ClassMapper SubMenuMapper { get; } = new ClassMapper();

        public bool IsOpen { get; private set; }

        private void SetClass()
        {
            ClassMapper.Add(PrefixCls)
                .Add($"{PrefixCls}-{RootMenu.InternalMode}")
                .If($"{PrefixCls}-open", () => IsOpen);

            SubMenuMapper.Add("ant-menu")
                .Add("ant-menu-sub")
                .Add($"ant-menu-{(RootMenu.InternalMode == AntMenuMode.Horizontal ? AntMenuMode.Vertical : RootMenu.InternalMode)}")
                .If($"ant-menu-submenu-popup", () => RootMenu.InternalMode != AntMenuMode.Inline)
                .If($"ant-menu-hidden", () => !IsOpen);
        }

        private async Task HandleOnTitleClick(MouseEventArgs args)
        {
            RootMenu.SelectSubmenu(this);
            if (OnTitleClicked.HasDelegate)
                await OnTitleClicked.InvokeAsync(args);
        }

        private void HandleMouseOver(MouseEventArgs args)
        {
            if (RootMenu.InternalMode == AntMenuMode.Inline)
                return;

            IsOpen = true;
        }

        private void HandleMouseOut(MouseEventArgs args)
        {
            if (RootMenu.InternalMode == AntMenuMode.Inline)
                return;

            IsOpen = false;
        }

        public async Task Collapse()
        {
            await Task.Delay(300);
            IsOpen = false;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            //if (string.IsNullOrWhiteSpace(Key))
            //    throw new ArgumentException($"Parameter {nameof(Key)} is required for a {nameof(AntSubMenu)}");

            RootMenu.Submenus.Add(this);
            if (RootMenu.DefaultOpenSubMenus.Contains(Key))
                IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Open()
        {
            IsOpen = true;
            if (Parent != null)
            {
                Parent.Open();
            }
        }
    }
}
