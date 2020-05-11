using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class MenuItem : AntDomComponentBase
    {
        [CascadingParameter] public Menu RootMenu { get; set; }
        [CascadingParameter] public SubMenu ParentMenu { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Key { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClicked { get; set; }

        public bool IsSelected { get; private set; }

        private void SetClass()
        {
            string prefixCls = "ant-menu-item";

            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-selected", () => IsSelected)
                .If($"{prefixCls}-disabled", () => Disabled);
        }

        protected override void OnInitialized()
        {
            SetClass();

            base.OnInitialized();

            RootMenu.MenuItems.Add(this);
        }

        public async Task HandleOnClick(MouseEventArgs args)
        {
            if (!RootMenu.Selectable)
                return;

            RootMenu.SelectItem(this);

            if (OnClicked.HasDelegate)
                await OnClicked.InvokeAsync(args);

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
        }

        public void Deselect()
        {
            IsSelected = false;
        }
    }
}
