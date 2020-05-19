using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
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
        public EventCallback<MouseEventArgs> OnClicked { get; set; }

        public bool IsSelected { get; private set; }
        private string _key;

        private void SetClass()
        {
            string prefixCls = $"{RootMenu.PrefixCls}-item";

            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-selected", () => IsSelected)
                .If($"{prefixCls}-disabled", () => Disabled);
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

            if (RootMenu.SelectedKeys.Contains(Key))
                Select();
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
            ParentMenu?.Select();
        }

        public void Deselect()
        {
            IsSelected = false;
            ParentMenu?.Deselect();
        }
    }
}
