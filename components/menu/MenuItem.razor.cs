// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class MenuItem : AntDomComponentBase
    {
        [CascadingParameter]
        internal Menu RootMenu { get; set; }

        [CascadingParameter]
        internal SubMenu ParentMenu { get; set; }

        /// <summary>
        /// Display title
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Unique ID of the menu item
        /// </summary>
        /// <default value="Uniquely Generated ID" />
        [Parameter]
        public string Key
        {
            get => _key ?? Id;
            set => _key = value;
        }

        /// <summary>
        /// Whether menu item is disabled
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Callback for when item is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Href route
        /// </summary>
        [Parameter]
        public string RouterLink { get; set; }

        /// <summary>
        /// Modifies the URL matching behavior for a NavLink
        /// </summary>
        /// <default value="NavLinkMatch.All" />
        [Parameter]
        public NavLinkMatch RouterMatch { get; set; } = NavLinkMatch.All;
        
        /// <summary>
        /// Allows specification of the HTML `target` attribute
        /// </summary>
        [Parameter]
        public MenuTarget? Target { get; set; }

        /// <summary>
        /// Title of the menu item
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Icon of the menu item
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Custom icon template, when Icon and IconTemplate are set at the same time, IconTemplate is preferred
        /// </summary>
        [Parameter]
        public RenderFragment IconTemplate { get; set; }

        internal bool IsSelected { get; private set; }
        internal bool FirstRun { get; set; } = true;
        private string _key;
        private Tooltip _tooltip;

        private bool TooltipDisabled => ParentMenu?.IsOpen == true || ParentMenu?._overlayVisible == true || RootMenu?.InlineCollapsed == false;

        private int Padding => RootMenu?.InternalMode == MenuMode.Inline ? ((ParentMenu?.Level ?? 0) + 1) * RootMenu?.InlineIndent ?? 0 : 0;

        private string PaddingStyle => Padding > 0 ? $"{(RTL ? "padding-right" : "padding-left")}:{Padding}px;" : "";

        // There is no need to render the tooltip if there is no inline mode. Tooltip will be only showing menu content if menu is collapsed to icon version && only for root menu
        private bool ShowTooltip => RootMenu?.Mode == MenuMode.Inline && ParentMenu is null && RootMenu?.ShowCollapsedTooltip == true;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu?.PrefixCls}-item";

            ClassMapper
                .Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-selected", () => IsSelected)
                .If($"{prefixCls}-disabled", () => Disabled);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClass();

            RootMenu?.AddMenuItem(this);

            if (RootMenu?.DefaultSelectedKeys.Contains(Key) == true)
                Select(false, true);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (RootMenu?.SelectedKey(Key) == true && !IsSelected)
                Select();

            _tooltip?.SetShouldRender(true);
        }

        protected override void Dispose(bool disposing)
        {
            RootMenu?.RemoveMenuItem(this);

            base.Dispose(disposing);
        }

        internal void UpdateStelected()
        {
            if (RootMenu?.SelectedKey(Key) == true)
            {
                if (!IsSelected) Select();
            }
            else if (IsSelected)
            {
                Deselect();
            }
        }

        private async Task HandleOnClick(MouseEventArgs args)
        {
            if (Disabled)
                return;
            // Hide overlay before handling OnClick to make sure the overlay is not on top of modal created in OnClick delegate
            if (RootMenu?.Selectable == true)
            {
                if (IsSelected && RootMenu?.Multiple == true)
                {
                    Deselect();
                }
                else
                {
                    RootMenu?.SelectItem(this);
                }
            }
            else
            {
                RootMenu?.HideOverlay();
            }

            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }

            if (RootMenu?.OnMenuItemClicked.HasDelegate == true)
            {
                await RootMenu.OnMenuItemClicked.InvokeAsync(this);
            }

            if (ParentMenu == null)
                return;

            if (RootMenu?.Mode != MenuMode.Inline)
            {
                await ParentMenu?.Collapse();
            }
        }

        internal void Select(bool skipParentSelection = false, bool isInitializing = false)
        {
            IsSelected = true;
            FirstRun = false;
            if (!skipParentSelection)
                ParentMenu?.Select(isInitializing);

            // It seems that the `StateHasChanged()` call in parent menu doesn't work correctly when the menuitem was wrapped by any other component than a menu.
            StateHasChanged();
        }

        internal void Deselect(bool sameParentAsSelected = false)
        {
            RootMenu?.HideOverlay();
            IsSelected = false;
            FirstRun = false;
            if (!sameParentAsSelected)
                ParentMenu?.Deselect();

            // It seems that the `StateHasChanged()` call in parent menu doesn't work correctly when the menuitem was wrapped by any other component than a menu.
            StateHasChanged();
        }
    }
}
