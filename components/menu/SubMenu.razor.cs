using System;
using System.Linq;
using System.Threading.Tasks;
using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntBlazor
{
    public partial class SubMenu : AntDomComponentBase
    {
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

        private OverlayTrigger _overlayTrigger;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-submenu";

            ClassMapper
                    .Clear()
                    .Add(prefixCls)
                    .Add($"{prefixCls}-{RootMenu.InternalMode}")
                    .If($"{prefixCls}-disabled", () => Disabled)
                    .If($"{prefixCls}-open", () => RootMenu.InternalMode == MenuMode.Inline && IsOpen)
                    ;

            SubMenuMapper
                .Clear()
                .Add("ant-menu")
                .Add("ant-menu-sub")
                .Add($"ant-menu-{(RootMenu.InternalMode == MenuMode.Horizontal ? MenuMode.Vertical : RootMenu.InternalMode)}")
                //.If($"ant-menu-submenu-popup", () => RootMenu.InternalMode != MenuMode.Inline)
                .If($"ant-menu-hidden", () => RootMenu.InternalMode == MenuMode.Inline && !IsOpen)
                ;

            if (_overlayTrigger != null)
            {
                Overlay overlay = _overlayTrigger.GetOverlayComponent();

                SubMenuMapper
                    .If($"ant-menu-hidden", () => overlay.IsHiding() == false && overlay.IsPopup() == false)
                    .If($"zoom-big zoom-big-enter zoom-big-enter-active", () => overlay.IsPopup() && !overlay.IsHiding())
                    .If($"zoom-big zoom-big-leave zoom-big-leave-active", () => overlay.IsHiding());
            }
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

        public OverlayTrigger GetOverlayTrigger()
        {
            return _overlayTrigger;
        }

        private void OnOverlayVisibleChange(bool visible)
        {
            if (visible)
            {
                SetClass();
            }
        }

        private void OnOverlayHiding(bool hiding)
        {
            SetClass();
        }
    }
}
