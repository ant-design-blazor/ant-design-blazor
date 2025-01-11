// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
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
        /// class name of the popup
        /// </summary>
        [Parameter]
        public string PopupClassName { get; set; }

        /// <summary>
        /// Title 
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title template
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

        private int Padding => Level * RootMenu?.InlineIndent ?? 0;

        private string PaddingStyle => Padding > 0 ? $"{(RTL ? "padding-right" : "padding-left")}:{Padding}px;" : "";

        private ClassMapper SubMenuMapper { get; } = new ClassMapper();

        private bool _isSelected;

        private string _key;

        private string _popupMinWidthStyle = "";
        private OverlayTrigger _overlayTrigger;

        internal bool _overlayVisible;
        private PlacementType? _placement;

        private ElementReference _warpperRef;
        private decimal _warpperHight;
        private string _warpperStyle;

        private bool _isActive = false;
        private bool _isInactive = true;
        private bool _isHidden = true;
        private bool _isCollapseEnterPrepare;
        private bool _isCollapseEnterStart;
        private bool _isCollapseEnterActive;

        private bool _isCollapseLeavePrepare;
        private bool _isCollapseLeaveStart;
        private bool _isCollapseLeaveActive;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-submenu";

            ClassMapper
                .Add(prefixCls)
                .GetIf(() => $"{prefixCls}-vertical", () => RootMenu?.InternalMode != MenuMode.Inline)
                .GetIf(() => $"{prefixCls}-inline", () => RootMenu?.InternalMode == MenuMode.Inline)
                .If($"{prefixCls}-disabled", () => Disabled)
                .If($"{prefixCls}-selected", () => _isSelected)
                .If($"{prefixCls}-rtl", () => RTL)
                .If($"{prefixCls}-open", () => RootMenu?.InternalMode == MenuMode.Inline && IsOpen)
                ;

            SubMenuMapper
                .Add(RootMenu?.PrefixCls)
                .Add($"{RootMenu?.PrefixCls}-sub")
                .GetIf(() => $"{RootMenu?.PrefixCls}-light", () => RootMenu?.Theme == MenuTheme.Light)
                .GetIf(() => $"{RootMenu?.PrefixCls}-dark", () => RootMenu?.Theme == MenuTheme.Dark)
                .GetIf(() => $"{RootMenu?.PrefixCls}-vertical", () => RootMenu?.InternalMode != MenuMode.Inline)
                .GetIf(() => $"{RootMenu?.PrefixCls}-inline", () => RootMenu?.InternalMode == MenuMode.Inline)
                //.If($"{RootMenu.PrefixCls}-submenu-popup", () => RootMenu.InternalMode != MenuMode.Inline)
                .If($"{RootMenu?.PrefixCls}-hidden", () => RootMenu?.InternalMode == MenuMode.Inline && !IsOpen && _isHidden)
                .If($"{RootMenu?.PrefixCls}-rtl", () => RTL)
                .If("ant-motion-collapse-enter ant-motion-collapse-enter-prepare ant-motion-collapse", () => _isCollapseEnterPrepare)
                .If("ant-motion-collapse-enter ant-motion-collapse-enter-start ant-motion-collapse", () => _isCollapseEnterStart)
                .If("ant-motion-collapse-enter ant-motion-collapse-enter-active ant-motion-collapse", () => _isCollapseEnterActive)
                .If("ant-motion-collapse-leave ant-motion-collapse-leave-prepare ant-motion-collapse", () => _isCollapseLeavePrepare)
                .If("ant-motion-collapse-leave ant-motion-collapse-leave-start ant-motion-collapse", () => _isCollapseLeaveStart)
                .If("ant-motion-collapse-leave ant-motion-collapse-leave-active ant-motion-collapse", () => _isCollapseLeaveActive)
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

        internal async Task Collapse()
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

            RootMenu?.AddSubmenu(this);

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

        protected override void Dispose(bool disposing)
        {
            RootMenu?.RemoveSubmenu(this);
            base.Dispose(disposing);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && RootMenu.InternalMode != MenuMode.Inline && _overlayTrigger != null)
            {
                var domInfo = await _overlayTrigger.GetTriggerDomInfo();
                _popupMinWidthStyle = $"min-width: {domInfo.ClientWidth}px";
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        internal void Close()
        {
            IsOpen = false;

            if (RootMenu.Animation)
            {
                HandleCollapse();
            }
        }

        internal void Open()
        {
            if (Disabled)
            {
                return;
            }

            IsOpen = true;

            if (RootMenu.Animation)
            {
                HandleExpand();
            }

            Parent?.Open();
        }

        private void OnOverlayVisibleChange(bool visible)
        {
            _overlayVisible = visible;
        }

        private void OnOverlayHiding(bool _)
        {
        }

        internal void Select(bool isInitializing = false)
        {
            Parent?.Select();
            _isSelected = true;
            if (isInitializing)
            {
                StateHasChanged();
            }
        }

        internal void Deselect()
        {
            Parent?.Deselect();
            _isSelected = false;
        }

        private async Task UpdateHeight()
        {
            var rect = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _warpperRef);
            _warpperHight = rect.ScrollHeight;
        }

        private void HandleExpand()
        {
            _isActive = true;
            _isInactive = false;
            _isHidden = false;
            _isCollapseEnterPrepare = true;

            CallAfterRender(async () =>
            {
                await UpdateHeight();

                _isCollapseEnterPrepare = false;
                _isCollapseEnterStart = true;
                _warpperStyle = "height: 0px; opacity: 0;";

                CallAfterRender(async () =>
                {
                    _isCollapseEnterStart = false;
                    _isCollapseEnterActive = true;
                    StateHasChanged();
                    await Task.Delay(100);

                    _warpperStyle = $"height: {_warpperHight}px; opacity: 1;";
                    StateHasChanged();
                    await Task.Delay(450);

                    _isCollapseEnterActive = false;
                    _warpperStyle = "";
                    StateHasChanged();
                });

                StateHasChanged();
            });
        }

        private void HandleCollapse()
        {
            _isActive = false;
            _isInactive = true;
            _isCollapseLeavePrepare = true;

            CallAfterRender(async () =>
            {
                _isCollapseLeavePrepare = false;
                _isCollapseLeaveStart = true;
                _warpperStyle = $"height: {_warpperHight}px;";

                CallAfterRender(async () =>
                {
                    await Task.Delay(100);
                    _isCollapseLeaveStart = false;
                    _isCollapseLeaveActive = true;

                    _warpperStyle = "height: 0px; opacity: 0;";//still active
                    StateHasChanged();

                    await Task.Delay(450);
                    _isHidden = true; // still height 0
                    _warpperStyle = "";
                    _isCollapseLeaveActive = false;
                    StateHasChanged();
                });

                StateHasChanged();
                await Task.Yield();
            });

            StateHasChanged();
        }
    }
}
