using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class SubMenu : AntDomComponentBase
    {
        [CascadingParameter]
        public Menu RootMenu { get; set; }

        [CascadingParameter]
        public SubMenu Parent { get; set; }

        /// <summary>
        /// Menu placement
        /// </summary>
        [Parameter]
        public Placement? Placement
        {
            get { return _placement?.Placement; }
            set
            {
                if (value is null)
                {
                    _placement = null;
                }
                else
                {
                    _placement = PlacementType.Create(value.Value);
                }
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// SubMenus or SubMenu items
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Unique ID of the SubMenu
        /// </summary>
        /// <default value="Uniquely Generated ID" />
        [Parameter]
        public string Key
        {
            get => _key ?? Id;
            set => _key = value;
        }

        /// <summary>
        /// Whether SubMenu is disabled
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Open state of the SubMenu
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool IsOpen { get; set; }

        /// <summary>
        /// Callback executed when the SubMenu title is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnTitleClick { get; set; }

        internal int Level => RootMenu?.InternalMode == MenuMode.Inline ? (Parent?.Level ?? 0) + 1 : 0;

        private int PaddingLeft => Level * RootMenu?.InlineIndent ?? 0;

        private ClassMapper SubMenuMapper { get; } = new ClassMapper();

        private bool _isSelected;

        private string _key;

        private string _popupMinWidthStyle = "";
        private OverlayTrigger _overlayTrigger;

        internal bool _overlayVisible;
        private PlacementType? _placement;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-submenu";

            ClassMapper
                    .Clear()
                    .Add(prefixCls)
                    .Get(() => $"{prefixCls}-{RootMenu?.InternalMode}")
                    .If($"{prefixCls}-disabled", () => Disabled)
                    .If($"{prefixCls}-selected", () => _isSelected)
                    .If($"{prefixCls}-open", () =>
                    {
                        var eval = RootMenu?.InternalMode == MenuMode.Inline && IsOpen;
                        return eval;
                    })
                    ;

            SubMenuMapper
                .Clear()
                .Add(RootMenu?.PrefixCls)
                .Add($"{RootMenu?.PrefixCls}-sub")
                .Get(() => $"{RootMenu?.PrefixCls}-{RootMenu?.Theme}")
                .Get(() => $"{RootMenu?.PrefixCls}-{(RootMenu?.InternalMode == MenuMode.Horizontal ? MenuMode.Vertical : RootMenu?.InternalMode)}")
                //.If($"{RootMenu.PrefixCls}-submenu-popup", () => RootMenu.InternalMode != MenuMode.Inline)
                .If($"{RootMenu?.PrefixCls}-hidden", () => RootMenu?.InternalMode == MenuMode.Inline && !IsOpen)
                ;

            if (RootMenu?.InternalMode != MenuMode.Inline && _overlayTrigger != null)
            {
                Overlay overlay = _overlayTrigger.GetOverlayComponent();

                ClassMapper
                    .If($"{prefixCls}-selected", () => overlay != null && overlay.IsPopup());

                SubMenuMapper
                    .If($"ant-zoom-big ant-zoom-big-enter ant-zoom-big-enter-active", () => overlay != null && RootMenu?.Mode == MenuMode.Vertical && overlay.IsPopup() && !overlay.IsHiding())
                    .If($"ant-zoom-big ant-zoom-big-leave ant-zoom-big-leave-active", () => overlay != null && RootMenu?.Mode == MenuMode.Vertical && overlay.IsHiding())
                    .If($"ant-slide-up ant-slide-up-enter ant-slide-up-enter-active", () => overlay != null && RootMenu?.Mode == MenuMode.Horizontal && overlay.IsPopup() && !overlay.IsHiding())
                    .If($"ant-slide-up ant-slide-up-leave ant-slide-up-leave-active", () => overlay != null && RootMenu?.Mode == MenuMode.Horizontal && overlay.IsHiding())
                    ;
            }
        }

        private async Task HandleOnTitleClick(MouseEventArgs args)
        {
            RootMenu?.SelectSubmenu(this, true);
            if (OnTitleClick.HasDelegate)
                await OnTitleClick.InvokeAsync(args);
        }

        public async Task Collapse()
        {
            if (RootMenu?.InternalMode == MenuMode.Inline)
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

            RootMenu?.Submenus.Add(this);

            if (RootMenu.DefaultOpenKeys.Contains(Key))
                IsOpen = true;

            _overlayVisible = IsOpen;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!RootMenu.InlineCollapsed && RootMenu.OpenKeys.Contains(Key))
            {
                if (RootMenu.InitialMode != RootMenu.Mode)
                {
                    IsOpen = false;
                }
                else
                {
                    IsOpen = true;
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (RootMenu.InternalMode != MenuMode.Inline && _overlayTrigger != null && IsOpen)
            {
                var domInfo = await _overlayTrigger.GetTriggerDomInfo();
                _popupMinWidthStyle = $"min-width: {domInfo.ClientWidth}px";
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
            Parent?.Open();
        }

        public OverlayTrigger GetOverlayTrigger()
        {
            return _overlayTrigger;
        }

        private void OnOverlayVisibleChange(bool visible)
        {
            _overlayVisible = visible;
        }

        private void OnOverlayHiding(bool _)
        {
        }

        public void Select(bool isInitializing = false)
        {
            Parent?.Select();
            _isSelected = true;
            if (isInitializing)
            {
                StateHasChanged();
            }
        }

        public void Deselect()
        {
            Parent?.Deselect();
            _isSelected = false;
        }
    }
}
