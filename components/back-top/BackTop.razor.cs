using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace AntDesign
{
    public partial class BackTop : AntDomComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        protected ClassMapper BackTopClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper BackTopContentClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper BackTopIconClassMapper { get; set; } = new ClassMapper();

        protected async Task OnClick()
        {
            await JsInvokeAsync<DomRect>(JSInteropConstants.backTop, "BodyContainer");
        }

        protected override void OnInitialized()
        {
            SetClass();

            base.OnInitialized();
        }

        protected void SetClass()
        {
            string prefixCls = "ant-back-top";
            BackTopClassMapper.Add(prefixCls);
            BackTopContentClassMapper.Add($"{prefixCls}-content");
            BackTopIconClassMapper.Add($"{prefixCls}-icon");
        }
    }
}
