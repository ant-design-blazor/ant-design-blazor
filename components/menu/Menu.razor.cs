using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Menu : AntDomComponentBase
    {
        [CascadingParameter(Name = "PrefixCls")]
        public string PrefixCls { get; set; } = "ant-menu";

        [CascadingParameter]
        public Sider Parent { get; set; }

        [CascadingParameter(Name = "Overlay")]
        private Overlay Overlay { get; set; }

        [Parameter]
        public MenuTheme Theme { get; set; } = MenuTheme.Light;

        [Parameter]
        public MenuMode Mode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    CollapseUpdated(_collapsed);
                }
            }
        }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<SubMenu> OnSubmenuClicked { get; set; }

        [Parameter]
        public EventCallback<MenuItem> OnMenuItemClicked { get; set; }

        [Parameter]
        public bool Accordion { get; set; }

        [Parameter]
        public bool Selectable { get; set; } = true;

        [Parameter]
        public bool InlineCollapsed
        {
            get => _inlineCollapsed;
            set
            {
                if (_inlineCollapsed != value)
                {
                    _inlineCollapsed = value;
                    if (Parent == null)
                    {
                        this._collapsed = _inlineCollapsed;
                    }

                    CollapseUpdated(_collapsed);
                }
            }
        }

        [Parameter]
        public bool AutoCloseDropdown { get; set; } = true;

        [Parameter]
        public IEnumerable<string> DefaultSelectedKeys { get; set; } = new List<string>();

        [Parameter]
        public IEnumerable<string> DefaultOpenKeys { get; set; } = new List<string>();

        [Parameter]
        public string[] OpenKeys
        {
            get => _openKeys ?? Array.Empty<string>();
            set
            {
                _openKeys = value;
                HandleOpenKeySet();
            }
        }

        [Parameter]
        public EventCallback<string[]> OpenKeysChanged { get; set; }

        [Parameter]
        public EventCallback<string[]> OnOpenChange { get; set; }

        [Parameter]
        public string[] SelectedKeys
        {
            get => _selectedKeys ?? Array.Empty<string>();
            set
            {
                _selectedKeys = value;
            }
        }

        [Parameter]
        public EventCallback<string[]> SelectedKeysChanged { get; set; }

        [Parameter]
        public bool IgnoreSelectionAfterClick { get; set; } = false;

        internal MenuMode InternalMode { get; private set; }
        private bool _collapsed;
        private string[] _openKeys;
        private string[] _selectedKeys;
        private bool _inlineCollapsed;
        private MenuMode _mode = MenuMode.Vertical;

        public List<SubMenu> Submenus { get; set; } = new List<SubMenu>();
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public void SelectItem(MenuItem item)
        {
            if (item == null)
            {
                return;
            }

            foreach (MenuItem menuitem in MenuItems.Where(x => x != item))
            {
                menuitem.Deselect();
            }

            if (!item.IsSelected)
            {
                item.Select();
            }

            StateHasChanged();

            if (OnMenuItemClicked.HasDelegate)
                OnMenuItemClicked.InvokeAsync(item);

            _selectedKeys = MenuItems.Where(x => x.IsSelected).Select(x => x.Key).ToArray();
            if (SelectedKeysChanged.HasDelegate)
                SelectedKeysChanged.InvokeAsync(_selectedKeys);

            if (Overlay != null && AutoCloseDropdown)
            {
                Overlay.Hide(true);
            }
        }

        public void SelectSubmenu(SubMenu menu, bool isTitleClick = false)
        {
            if (menu == null)
            {
                return;
            }

            if (Accordion)
            {
                foreach (SubMenu item in Submenus.Where(x => x != menu && x != menu.Parent))
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

            var openKeys = Submenus.Where(x => x.IsOpen).Select(x => x.Key).ToArray();
            HandleOpenChange(openKeys);

            StateHasChanged();
        }

        private void SetClass()
        {
            ClassMapper
                .Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-root")
                .If($"{PrefixCls}-{MenuTheme.Dark}", () => Theme == MenuTheme.Dark)
                .If($"{PrefixCls}-{MenuTheme.Light}", () => Theme == MenuTheme.Light)
                .If($"{PrefixCls}-{MenuMode.Inline}", () => InternalMode == MenuMode.Inline)
                .If($"{PrefixCls}-{MenuMode.Vertical}", () => InternalMode == MenuMode.Vertical)
                .If($"{PrefixCls}-{MenuMode.Horizontal}", () => InternalMode == MenuMode.Horizontal)
                .If($"{PrefixCls}-inline-collapsed", () => _collapsed)
                .If($"{PrefixCls}-unselectable", () => !Selectable);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (InternalMode != MenuMode.Inline && _collapsed)
                throw new ArgumentException($"{nameof(Menu)} in the {Mode} mode cannot be {nameof(InlineCollapsed)}");

            InternalMode = Mode;
            if (Parent != null)
            {
                Parent.OnCollapsed += CollapseUpdated;
                CollapseUpdated(Parent.Collapsed);
            }

            SetClass();
        }

        public void CollapseUpdated(bool collapsed)
        {
            this._collapsed = collapsed;
            if (collapsed)
            {
                InternalMode = MenuMode.Vertical;
                foreach (SubMenu item in Submenus)
                {
                    item.Close();
                }
            }
            else
            {
                InternalMode = Mode;
            }
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
            foreach (SubMenu item in Submenus.Where(x => x.Key.IsIn(this.OpenKeys)))
            {
                item.Open();
            }

            foreach (SubMenu item in Submenus.Where(x => !x.Key.IsIn(this.OpenKeys)))
            {
                item.Close();
            }

            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            if (Parent != null)
            {
                Parent.OnCollapsed -= CollapseUpdated;
            }

            base.Dispose(disposing);
        }

        internal void MarkStateHasChanged()
        {
            if (!IsDisposed)
            {
                StateHasChanged();
            }
        }
    }
}
