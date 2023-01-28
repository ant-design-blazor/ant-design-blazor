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
        public Menu RootMenu { get; set; }

        [CascadingParameter]
        public SubMenu ParentMenu { get; set; }

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

        private bool TooltipDisabled => ParentMenu?.IsOpen == true || ParentMenu?._overlayVisible == true || RootMenu?.InlineCollapsed == false;

        private int PaddingLeft => RootMenu.InternalMode == MenuMode.Inline ? ((ParentMenu?.Level ?? 0) + 1) * RootMenu?.InlineIndent ?? 0 : 0;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-item";

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

            RootMenu.MenuItems.Add(this);

            if (RootMenu.DefaultSelectedKeys.Contains(Key))
                Select(false, true);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (RootMenu.SelectedKeys.Contains(Key) && !IsSelected)
                Select();
        }

        internal void UpdateStelected()
        {
            if (RootMenu.SelectedKeys.Contains(Key))
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

            if (!RootMenu.Selectable)
                return;

            if (IsSelected && RootMenu?.Multiple == true)
            {
                Deselect();
            }
            else
            {
                RootMenu.SelectItem(this);
            }

            if (ParentMenu == null)
                return;

            if (RootMenu.Mode != MenuMode.Inline)
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
        }

        public void Deselect(bool sameParentAsSelected = false)
        {
            IsSelected = false;
            FirstRun = false;
            if (!sameParentAsSelected)
                ParentMenu?.Deselect();
        }
    }
}
