using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntBlazor
{
    public partial class AntMenuItemGroup : AntDomComponentBase
    {
        private static string _prefixCls = "ant-menu-item-group";

        protected override void OnInitialized()
        {
            ClassMapper.Add(_prefixCls);
        }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
