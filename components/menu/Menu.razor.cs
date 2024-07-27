using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Menu : AntDomComponentBase
    {
        [CascadingParameter(Name = "PrefixCls")]
        internal string PrefixCls { get; set; } = "ant-menu";

        [CascadingParameter]
        private Sider Parent { get; set; }

        [CascadingParameter(Name = "Overlay")]
        private Overlay Overlay { get; set; }

        [Parameter]
        public MenuTheme Theme { get; set; } = MenuTheme.Light;

        internal MenuMode? InitialMode { get; private set; }

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
        public bool Multiple { get; set; }

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

        [Parameter]
        public int InlineIndent { get; set; } = 24;

        [Parameter]
        public bool AutoCloseDropdown { get; set; } = true;

        [Parameter]
        public IEnumerable<string> DefaultSelectedKeys { get; set; } = new List<string>();

        [Parameter]
        public IEnumerable<string> DefaultOpenKeys { get; set; } = new List<string>();

        private string[] _openKeysCopy = Array.Empty<string>();

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
        public Trigger TriggerSubMenuAction { get; set; } = Trigger.Hover;

        [Parameter]
        public bool ShowCollapsedTooltip { get; set; } = true;

        [Parameter]
        public bool Animation { get; set; }

        [Inject] private MenuService MenusService { get; set; }

        internal MenuMode InternalMode { get; private set; }

        private string[] _openKeys;
        private string[] _selectedKeys;
        private bool _inlineCollapsed;
        private MenuMode _mode = MenuMode.Vertical;

        private List<SubMenu> _submenus = [];
        private List<MenuItem> _menuItems = [];

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.IsParameterChanged(nameof(SelectedKeys), SelectedKeys))
            {
                MenuItems.ForEach(x => x.UpdateStelected());
            }

            return base.SetParametersAsync(parameters);
        }

        public void SelectItem(MenuItem item)
        {
            if (Overlay != null && AutoCloseDropdown)
            {
                Overlay.Hide(true);
            }

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

        public void SelectSubmenu(SubMenu menu, bool isTitleClick = false)
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
                .If($"{PrefixCls}-{MenuTheme.Dark}", () => Theme == MenuTheme.Dark)
                .If($"{PrefixCls}-{MenuTheme.Light}", () => Theme == MenuTheme.Light)
                .If($"{PrefixCls}-{MenuMode.Inline}", () => InternalMode == MenuMode.Inline)
                .If($"{PrefixCls}-{MenuMode.Vertical}", () => InternalMode == MenuMode.Vertical)
                .If($"{PrefixCls}-{MenuMode.Horizontal}", () => InternalMode == MenuMode.Horizontal)
                .If($"{PrefixCls}-inline-collapsed", () => InlineCollapsed)
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

        protected override Task OnFirstAfterRenderAsync()
        {
            MenusService.SetMenuItems(_menuItems);
            return base.OnFirstAfterRenderAsync();
        }

        internal void AddSubmenu(SubMenu menu)
        {
            _submenus.Add(menu);
        }

        internal void AddMenuItem(MenuItem item)
        {
            _menuItems.Add(item);
        }

        internal void RemoveSubmenu(SubMenu menu)
        {
            _submenus.Remove(menu);
        }

        internal void RemoveMenuItem(MenuItem item)
        {
            _menuItems.Remove(item);
        }

        public void CollapseUpdated(bool collapsed)
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
    }
}
