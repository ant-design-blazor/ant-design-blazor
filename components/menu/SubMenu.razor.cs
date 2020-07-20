using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
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
        public bool IsOpen { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnTitleClicked { get; set; }

        internal int Level => RootMenu.InternalMode == MenuMode.Inline ? (Parent?.Level ?? 0) + 1 : 0;

        private int PaddingLeft => Level * 24;

        private ClassMapper SubMenuMapper { get; } = new ClassMapper();

        private bool _isSelected;

        private string _key;

        private string _popupMinWidthStyle = "";
        private OverlayTrigger _overlayTrigger;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-submenu";

            ClassMapper
                    .Clear()
                    .Add(prefixCls)
                    .Add($"{prefixCls}-{RootMenu?.InternalMode}")
                    .If($"{prefixCls}-disabled", () => Disabled)
                    .If($"{prefixCls}-selected", () => _isSelected)
                    .If($"{prefixCls}-open", () => RootMenu?.InternalMode == MenuMode.Inline && IsOpen)
                    ;

            SubMenuMapper
                .Clear()
                .Add(RootMenu.PrefixCls)
                .Add($"{RootMenu.PrefixCls}-sub")
                .Add($"{RootMenu.PrefixCls}-{RootMenu.Theme}")
                .Add($"{RootMenu.PrefixCls}-{(RootMenu.InternalMode == MenuMode.Horizontal ? MenuMode.Vertical : RootMenu.InternalMode)}")
                //.If($"{RootMenu.PrefixCls}-submenu-popup", () => RootMenu.InternalMode != MenuMode.Inline)
                .If($"{RootMenu.PrefixCls}-hidden", () => RootMenu.InternalMode == MenuMode.Inline && !IsOpen)
                ;

            if (RootMenu.InternalMode != MenuMode.Inline && _overlayTrigger != null)
            {
                Overlay overlay = _overlayTrigger.GetOverlayComponent();

                ClassMapper
                    .If($"{prefixCls}-selected", () => overlay.IsPopup());

                SubMenuMapper
                    .If($"{RootMenu.PrefixCls}-hidden", () => overlay.IsHiding() == false && overlay.IsPopup() == false)
                    .If($"zoom-big zoom-big-enter zoom-big-enter-active", () => RootMenu.Mode == MenuMode.Vertical && overlay.IsPopup() && !overlay.IsHiding())
                    .If($"zoom-big zoom-big-leave zoom-big-leave-active", () => RootMenu.Mode == MenuMode.Vertical && overlay.IsHiding())
                    .If($"slide-up slide-up-enter slide-up-enter-active", () => RootMenu.Mode == MenuMode.Horizontal && overlay.IsPopup() && !overlay.IsHiding())
                    .If($"slide-up slide-up-leave slide-up-leave-active", () => RootMenu.Mode == MenuMode.Horizontal && overlay.IsHiding())
                    ;
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
            if (RootMenu.InternalMode == MenuMode.Inline)
            {
                await Task.Delay(300);
            }
            else
            {
                await _overlayTrigger.Hide(true);
            }
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (RootMenu.InternalMode != MenuMode.Inline && _overlayTrigger != null)
            {
                var domInfo = await _overlayTrigger.GetTriggerDomInfo();
                _popupMinWidthStyle = $"min-width: {domInfo.clientWidth}px";
            }

            await base.OnAfterRenderAsync(firstRender);
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

        public void Select()
        {
            Parent?.Select();
            _isSelected = true;
        }

        public void Deselect()
        {
            Parent?.Deselect();
            _isSelected = false;
        }
    }
}
