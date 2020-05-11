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
        public bool Collapsed { get; set; }

        [Parameter] public IEnumerable<string> DefaultSelectedKeys { get; set; } = new List<string>();
        [Parameter] public IEnumerable<string> DefaultOpenKeys { get; set; } = new List<string>();

        private MenuMode _initialMode;
        internal MenuMode InternalMode { get; private set; }
        private bool _collapsed;

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

            if (OnMenuItemClicked.HasDelegate)
                OnMenuItemClicked.InvokeAsync(item);
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

            StateHasChanged();
        }

        private void SetClass()
        {
            ClassMapper.Add(PrefixCls)
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
                throw new ArgumentException($"{nameof(Menu)} in the {Mode} mode cannot be {nameof(Collapsed)}");

            _initialMode = Mode;
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
                this._collapsed = Collapsed;
            }
            Update(_collapsed);
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
            }
            else
            {
                InternalMode = _initialMode;
            }
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
