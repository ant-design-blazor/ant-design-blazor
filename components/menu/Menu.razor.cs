using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntBlazor
{
    public partial class Menu : AntDomComponentBase
    {
        private const string PrefixCls = "ant-menu";

        [CascadingParameter]
        public AntSider Parent { get; set; }

        [Parameter]
        public MenuTheme Theme { get; set; } = MenuTheme.Light;

        [Parameter]
        public MenuMode Mode { get; set; } = MenuMode.Inline;

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
        public bool InlineCollapsed { get; set; }

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

        internal MenuMode InternalMode { get; private set; }
        private bool _collapsed;
        private string[] _openKeys;
        private string[] _selectedKeys;

        public List<SubMenu> Submenus { get; set; } = new List<SubMenu>();
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public void SelectItem(MenuItem item)
        {
            foreach (MenuItem menuitem in MenuItems.Where(x => x != item))
            {
                menuitem.Deselect();
            }

            if (!item.IsSelected)
            {
                item.Select();
            }
            else
            {
                item.Deselect();
            }

            StateHasChanged();

            if (OnMenuItemClicked.HasDelegate)
                OnMenuItemClicked.InvokeAsync(item);

            _selectedKeys = MenuItems.Where(x => x.IsSelected).Select(x => x.Key).ToArray();
            if (SelectedKeysChanged.HasDelegate)
                SelectedKeysChanged.InvokeAsync(_selectedKeys);
        }

        public void SelectSubmenu(SubMenu menu)
        {
            if (Accordion)
            {
                foreach (SubMenu item in Submenus.Where(x => x != menu && x != menu.Parent))
                {
                    item.Close();
                }
            }

            if (menu.IsOpen)
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
                .Add($"{PrefixCls}-{Theme}")
                .Add($"{PrefixCls}-{InternalMode}")
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
                Parent.OnCollapsed += Update;
            }

            SetClass();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Parent == null)
            {
                this._collapsed = InlineCollapsed;
            }
            Update(_collapsed);
            SetClass();
        }

        public void Update(bool collapsed)
        {
            this._collapsed = collapsed;
            if (collapsed)
            {
                InternalMode = MenuMode.Vertical;
                foreach (SubMenu item in Submenus)
                {
                    item.Close();
                }

                HandleOpenChange(Array.Empty<string>());
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

        public void Dispose()
        {
            if (Parent != null)
            {
                Parent.OnCollapsed -= Update;
            }
        }
    }
}
