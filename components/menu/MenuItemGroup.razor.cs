using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class MenuItemGroup : AntDomComponentBase
    {
        [CascadingParameter]
        public Menu RootMenu { get; set; }

        protected override void OnInitialized()
        {
            ClassMapper.Add($"{RootMenu.PrefixCls}-item-group");
        }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Key { get; set; }
    }
}
