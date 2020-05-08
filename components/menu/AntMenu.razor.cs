using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntBlazor
{
    public partial class AntMenu : AntDomComponentBase
    {
        private const string PrefixCls = "ant-menu";

        [CascadingParameter]
        public AntSider Parent { get; set; }

        [Parameter]
        public AntMenuTheme Theme { get; set; } = AntMenuTheme.Light;

        [Parameter]
        public AntMenuMode Mode { get; set; } = AntMenuMode.Inline;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<AntSubMenu> OnSubmenuClicked { get; set; }

        [Parameter]
        public EventCallback<AntMenuItem> OnMenuItemClicked { get; set; }

        [Parameter]
        public IEnumerable<string> DefaultOpenSubMenus { get; set; } = new List<string>();

        [Parameter]
        public IEnumerable<string> DefaultSelectedItems { get; set; } = new List<string>();

        [Parameter]
        public bool Accordion { get; set; }

        [Parameter]
        public bool Selectable { get; set; } = true;

        [Parameter]
        public bool Collapsed { get; set; }

        private AntMenuMode _initialMode;
        internal AntMenuMode InternalMode { get; private set; }
        private bool _collapsed;

        public List<AntSubMenu> Submenus { get; set; } = new List<AntSubMenu>();
        public List<AntMenuItem> MenuItems { get; set; } = new List<AntMenuItem>();

        public void SelectItem(AntMenuItem item)
        {
            foreach (AntMenuItem menuitem in MenuItems.Where(x => x != item))
            {
                menuitem.Deselect();
            }

            if (item.IsSelected)
            {
                item.Deselect();
            }
            else
            {
                item.Select();
            }

            if (OnMenuItemClicked.HasDelegate)
                OnMenuItemClicked.InvokeAsync(item);

            StateHasChanged();
        }

        public void SelectSubmenu(AntSubMenu menu)
        {
            if (Accordion)
            {
                foreach (AntSubMenu item in Submenus.Where(x => x != menu && x != menu.Parent))
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

            if (InternalMode != AntMenuMode.Inline && _collapsed)
                throw new ArgumentException($"{nameof(AntMenu)} in the {Mode} mode cannot be {nameof(Collapsed)}");

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
                InternalMode = AntMenuMode.Vertical;
                foreach (AntSubMenu item in Submenus)
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
