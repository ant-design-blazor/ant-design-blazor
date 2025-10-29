// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>A versatile menu for navigation.</para>

    <h2>When To Use</h2>

    <para>
    Navigation is an important part of any website, as a good navigation setup allows users to move around the site quickly and efficiently. 
    Ant Design offers top and side navigation options. 
    Top navigation provides all the categories and functions of the website. 
    Side navigation provides the multi-level structure of the website.
    </para>

    <para>See Layouts for more layouts with navigation.</para>
    </summary>
    <seealso cref="MenuItem" />
    <seealso cref="SubMenu" />
    <seealso cref="MenuItemGroup" />
    <seealso cref="MenuDivider" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/alicdn/3XZcjGpvK/Menu.svg", Columns = 1, Title = "Menu", SubTitle = "导航菜单")]
    public partial class Menu : AntDomComponentBase
    {
        [CascadingParameter(Name = "PrefixCls")]
        internal string PrefixCls { get; set; } = "ant-menu";

        [CascadingParameter]
        private Sider Parent { get; set; }

        [CascadingParameter(Name = "Overlay")]
        private Overlay Overlay { get; set; }

        /// <summary>
        /// Color theme of the menu
        /// </summary>
        /// <default value="MenuTheme.Light" />
        [Parameter]
        public MenuTheme Theme { get; set; } = MenuTheme.Light;

        internal MenuMode? InitialMode { get; private set; }

        /// <summary>
        /// Type of menu
        /// </summary>
        /// <default value="MenuMode.Vertical" />
        [Parameter]
        public MenuMode Mode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    UpdateMode();
                }
            }
        }

        /// <summary>
        /// Content of menu. Should contain MenuItem elements.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Callback when submenu is clicked
        /// </summary>
        [Parameter]
        public EventCallback<SubMenu> OnSubmenuClicked { get; set; }

        /// <summary>
        /// Callback when a main menu item is clicked
        /// </summary>
        [Parameter]
        public EventCallback<MenuItem> OnMenuItemClicked { get; set; }

        /// <summary>
        /// Accordion mode. When true only one submenu can be open at a time.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Accordion { get; set; }

        /// <summary>
        /// Allows selecting menu items. When it is false the menu item is not selected after OnClick.
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Selectable { get; set; } = true;

        /// <summary>
        /// Allows selection of multiple items	
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Multiple { get; set; }

        /// <summary>
        /// Specifies the collapsed status when menu is inline mode	
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool InlineCollapsed
        {
            get => Parent?.Collapsed ?? _inlineCollapsed;
            set
            {
                if (_inlineCollapsed != value)
                {
                    _inlineCollapsed = value;
                    UpdateMode();
                }
            }
        }

        /// <summary>
        /// Indent (in pixels) of inline menu items on each level	
        /// </summary>
        /// <default value="24" />
        [Parameter]
        public int InlineIndent { get; set; } = 24;

        /// <summary>
        /// Close dropdown when clicking an item
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool AutoCloseDropdown { get; set; } = true;

        /// <summary>
        /// Array with the keys of default selected menu items	
        /// </summary>
        [Parameter]
        public IEnumerable<string> DefaultSelectedKeys { get; set; } = new List<string>();

        /// <summary>
        /// Array with the keys of default opened sub menus	
        /// </summary>
        [Parameter]
        public IEnumerable<string> DefaultOpenKeys { get; set; } = new List<string>();

        private string[] _openKeysCopy = Array.Empty<string>();

        /// <summary>
        /// Array with the keys of currently opened sub-menus
        /// </summary>
        [Parameter]
        public string[] OpenKeys
        {
            get => _openKeys ?? Array.Empty<string>();
            set
            {
                if (!_openKeysCopy.SequenceEqual(value))
                {
                    _openKeys = value;
                    _openKeysCopy = value.ToArray();
                    HandleOpenKeySet();
                }
            }
        }

        /// <summary>
        /// Callback when the open sub menus change. Passed the array of open keys.
        /// </summary>
        [Parameter]
        public EventCallback<string[]> OpenKeysChanged { get; set; }

        /// <summary>
        /// Callback when the open sub menus change. Passed the array of open keys.
        /// </summary>
        [Parameter]
        public EventCallback<string[]> OnOpenChange { get; set; }

        /// <summary>
        /// Array with the keys of currently selected menu items, set empty array to clear selection instead of null.
        /// </summary>
        /// <default value="[]" />
        [Parameter]
        public string[] SelectedKeys { get; set; }

        /// <summary>
        /// Callback when the selected items change. Passed array of open keys.
        /// </summary>
        [Parameter]
        public EventCallback<string[]> SelectedKeysChanged { get; set; }

        /// <summary>
        /// Which action can trigger submenu open/close	
        /// </summary>
        /// <default value="Trigger.Hover" />
        [Parameter]
        public Trigger TriggerSubMenuAction { get; set; } = Trigger.Hover;

        /// <summary>
        /// Show tooltip on collapsed menu
        /// </summary>
        [Parameter]
        public bool ShowCollapsedTooltip { get; set; } = true;

        /// <summary>
        /// Enable or disable animation
        /// </summary>
        [Parameter]
        public bool Animation { get; set; }

        [Inject] private MenuService MenusService { get; set; }

        internal MenuMode InternalMode { get; private set; }

        private string[] _openKeys;
        private string[] _selectedKeys = [];
        private bool _inlineCollapsed;
        private MenuMode _mode = MenuMode.Vertical;

        private List<SubMenu> _submenus = [];
        private List<MenuItem> _menuItems = [];

        public override Task SetParametersAsync(ParameterView parameters)
        {
            // if outside doesn't bind this parameter, it would be set to null
            if (parameters.IsParameterChanged(nameof(SelectedKeys), SelectedKeys, out var newSelectedKeys) && newSelectedKeys is not null)
            {
                _selectedKeys = newSelectedKeys;
                _menuItems.ForEach(x => x.UpdateStelected());
            }

            return base.SetParametersAsync(parameters);
        }

        internal void HideOverlay()
        {
            if (Overlay != null && AutoCloseDropdown)
            {
                _ = Overlay.Hide(true);
            }
        }

        internal void SelectItem(MenuItem item)
        {
            HideOverlay();

            if (item == null || item.IsSelected)
            {
                return;
            }

            var selectedKeys = new List<string>();
            bool skipParentSelection = false;
            if (!Multiple)
            {
                foreach (MenuItem menuitem in _menuItems.Where(x => x != item))
                {
                    if (item.RouterLink != null && menuitem.RouterLink != null && menuitem.RouterLink.Equals(item.RouterLink) && !menuitem.IsSelected)
                    {
                        menuitem.Select();
                        selectedKeys.Add(menuitem.Key);
                    }
                    else if (menuitem.IsSelected || menuitem.FirstRun)
                    {
                        if (!menuitem.FirstRun)
                            skipParentSelection = item.ParentMenu?.Key == menuitem.ParentMenu?.Key;
                        menuitem.Deselect(skipParentSelection);
                    }
                }
            }

            if (!item.IsSelected)
            {
                item.Select(skipParentSelection);
            }
            selectedKeys.Add(item.Key);
            _selectedKeys = selectedKeys.ToArray();

            StateHasChanged();

            if (SelectedKeysChanged.HasDelegate)
                SelectedKeysChanged.InvokeAsync(_selectedKeys);
        }

        internal void SelectSubmenu(SubMenu menu, bool isTitleClick = false)
        {
            if (menu == null)
            {
                return;
            }

            if (Accordion)
            {
                foreach (SubMenu item in _submenus.Where(x => x != menu && x != menu.Parent))
                {
                    item.Close();
                }
            }

            if (isTitleClick && menu.IsOpen)
            {
                menu.Close();
            }
            else
            {
                menu.Open();
            }

            if (OnSubmenuClicked.HasDelegate)
                OnSubmenuClicked.InvokeAsync(menu);

            var openKeys = _submenus.Where(x => x.IsOpen).Select(x => x.Key).ToArray();
            HandleOpenChange(openKeys);

            StateHasChanged();
        }

        private void SetClass()
        {
            ClassMapper
                .Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-root")
                .If($"{PrefixCls}-dark", () => Theme == MenuTheme.Dark)
                .If($"{PrefixCls}-light", () => Theme == MenuTheme.Light)
                .If($"{PrefixCls}-inline", () => InternalMode == MenuMode.Inline)
                .If($"{PrefixCls}-vertical", () => InternalMode == MenuMode.Vertical)
                .If($"{PrefixCls}-horizontal", () => InternalMode == MenuMode.Horizontal)
                .If($"{PrefixCls}-inline-collapsed", () => _inlineCollapsed)
                .If($"{PrefixCls}-unselectable", () => !Selectable)
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Mode != MenuMode.Inline && _inlineCollapsed)
                throw new ArgumentException($"{nameof(Menu)} in the {Mode} mode cannot be {nameof(InlineCollapsed)}");

            InternalMode = Mode;
            InitialMode = Mode;
            Parent?.AddMenu(this);

            OpenKeys = DefaultOpenKeys?.ToArray() ?? OpenKeys;

            SetClass();
        }

        internal void AddSubmenu(SubMenu menu)
        {
            _submenus.Add(menu);
        }

        internal void AddMenuItem(MenuItem item)
        {
            _menuItems.Add(item);
            MenusService.SetMenuItem(item);
        }

        internal void RemoveSubmenu(SubMenu menu)
        {
            _submenus.Remove(menu);
        }

        internal void RemoveMenuItem(MenuItem item)
        {
            _menuItems.Remove(item);
        }

        internal void CollapseUpdated(bool collapsed)
        {
            _inlineCollapsed = collapsed;

            UpdateMode();
        }

        private void UpdateMode()
        {
            if (_inlineCollapsed)
            {
                InternalMode = MenuMode.Vertical;
                foreach (SubMenu item in _submenus)
                {
                    item.Close();
                }
            }
            else
            {
                InternalMode = Mode;
            }

            StateHasChanged();
        }

        private void HandleOpenChange(string[] openKeys)
        {
            this._openKeys = openKeys;

            if (OnOpenChange.HasDelegate)
                OnOpenChange.InvokeAsync(openKeys);

            if (OpenKeysChanged.HasDelegate)
                OpenKeysChanged.InvokeAsync(openKeys);
        }

        private void HandleOpenKeySet()
        {
            foreach (SubMenu item in _submenus.Where(x => x.Key.IsIn(this.OpenKeys)))
            {
                item.Open();
            }

            foreach (SubMenu item in _submenus.Where(x => !x.Key.IsIn(this.OpenKeys)))
            {
                item.Close();
            }

            StateHasChanged();
        }

        internal void MarkStateHasChanged()
        {
            if (!IsDisposed)
            {
                StateHasChanged();
            }
        }

        internal bool SelectedKey(string key)
        {
            return _selectedKeys.Contains(key);
        }
    }
}
