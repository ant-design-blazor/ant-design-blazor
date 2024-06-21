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
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Parameter]
        public string RouterLink { get; set; }

        [Parameter]
        public NavLinkMatch RouterMatch { get; set; } = NavLinkMatch.All;

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [Parameter]
        public RenderFragment IconTemplate { get; set; }

        internal bool IsSelected { get; private set; }
        internal bool FirstRun { get; set; } = true;
        private string _key;

        private bool TooltipDisabled => ParentMenu?.IsOpen == true || ParentMenu?._overlayVisible == true || RootMenu?.InlineCollapsed == false;

        private int Padding => RootMenu?.InternalMode == MenuMode.Inline ? ((ParentMenu?.Level ?? 0) + 1) * RootMenu?.InlineIndent ?? 0 : 0;

        private string PaddingStyle => Padding > 0 ? $"{(RTL ? "padding-right" : "padding-left")}:{Padding}px;" : "";

        // There is no need to render the tooltip if there is no inline mode. Tooltip will be only showing menu content if menu is collapsed to icon version && only for root menu
        private bool ShowTooltip => RootMenu?.Mode == MenuMode.Inline && ParentMenu is null && RootMenu?.InlineCollapsed == true && RootMenu?.ShowCollapsedTooltip == true;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu?.PrefixCls}-item";

            ClassMapper
                .Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-selected", () => IsSelected)
                .If($"{prefixCls}-disabled", () => Disabled);
        }

        protected override void Dispose(bool disposing)
        {
            RootMenu?.MenuItems?.Remove(this);

            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClass();

            RootMenu?.MenuItems.Add(this);

            if (RootMenu?.DefaultSelectedKeys.Contains(Key) == true)
                Select(false, true);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (RootMenu?.SelectedKeys.Contains(Key) == true && !IsSelected)
                Select();
        }

        internal void UpdateStelected()
        {
            if (RootMenu?.SelectedKeys.Contains(Key) == true)
            {
                if (!IsSelected) Select();
            }
            else if (IsSelected)
            {
                Deselect();
            }
        }

        public async Task HandleOnClick(MouseEventArgs args)
        {
            if (Disabled)
                return;

            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }

            if (RootMenu?.OnMenuItemClicked.HasDelegate == true)
            {
                await RootMenu.OnMenuItemClicked.InvokeAsync(this);
            }

            if (RootMenu?.Selectable != true)
                return;

            if (IsSelected && RootMenu?.Multiple == true)
            {
                Deselect();
            }
            else
            {
                RootMenu?.SelectItem(this);
            }

            if (ParentMenu == null)
                return;

            if (RootMenu?.Mode != MenuMode.Inline)
            {
                await ParentMenu?.Collapse();
            }
        }

        public void Select(bool skipParentSelection = false, bool isInitializing = false)
        {
            IsSelected = true;
            FirstRun = false;
            if (!skipParentSelection)
                ParentMenu?.Select(isInitializing);

            // It seems that the `StateHasChanged()` call in parent menu doesn't work correctly when the menuitem was wrapped by any other component than a menu.
            StateHasChanged();
        }

        public void Deselect(bool sameParentAsSelected = false)
        {
            IsSelected = false;
            FirstRun = false;
            if (!sameParentAsSelected)
                ParentMenu?.Deselect();

            // It seems that the `StateHasChanged()` call in parent menu doesn't work correctly when the menuitem was wrapped by any other component than a menu.
            StateHasChanged();
        }
    }
}
