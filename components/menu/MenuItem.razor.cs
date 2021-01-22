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
        public NavLinkMatch RouterMatch { get; set; }

        [Parameter]
        public string Title { get; set; }

        internal bool IsSelected { get; private set; }
        internal bool FirstRun { get; set; } = true;
        private string _key;

        private int PaddingLeft => RootMenu.InternalMode == MenuMode.Inline ? ((ParentMenu?.Level ?? 0) + 1) * 24 : 0;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-item";

            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-selected", () => IsSelected)
                .If($"{prefixCls}-disabled", () => Disabled);
        }

        protected override void Dispose(bool disposing)
        {
            RootMenu.MenuItems.Remove(this);
            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClass();

            RootMenu.MenuItems.Add(this);

            if (RootMenu.DefaultSelectedKeys.Contains(Key))
                Select();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (RootMenu.SelectedKeys.Contains(Key) && !IsSelected)
                Select();
        }

        public async Task HandleOnClick(MouseEventArgs args)
        {
            if (Disabled)
                return;

            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(args);

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

        public void Select()
        {
            IsSelected = true;
            FirstRun = false;
            ParentMenu?.Select();
        }

        public void Deselect()
        {
            IsSelected = false;
            FirstRun = false;
            ParentMenu?.Deselect();
        }
    }
}
