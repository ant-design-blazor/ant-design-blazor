using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntCollapse : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected ClassMapper ContentBoxClassMapper { get; set; } = new ClassMapper();


        protected override async Task OnInitializedAsync()
        {
            string prefixCls = "ant-collapse";

            ClassMapper.Add($"{prefixCls}").Add($"{prefixCls}-icon-position-left");//添加默认样式

            await base.OnInitializedAsync();
        }
    }
}
