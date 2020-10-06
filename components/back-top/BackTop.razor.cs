using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class BackTop : AntDomComponentBase
    {
        [Inject]
        public DomEventService DomEventService { get; set; }

        [Parameter]
        public string Icon { get; set; } = "vertical-align-top";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 回到顶部的目标控件
        /// </summary>
        [Parameter]
        public string TargetSelector { get; set; }

        protected ClassMapper BackTopContentClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper BackTopIconClassMapper { get; set; } = new ClassMapper();

        [Parameter]
        public EventCallback OnClick { get; set; }


        protected async Task OnClickHandle()
        {
            if (string.IsNullOrWhiteSpace(TargetSelector))
                await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop);
            else
                await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop, TargetSelector);


            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(null);
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }

        protected void SetClass()
        {
            string prefixCls = "ant-back-top";
            ClassMapper.Add(prefixCls);
            BackTopContentClassMapper.Add($"{prefixCls}-content");
            BackTopIconClassMapper.Add($"{prefixCls}-icon");
        }
    }
}
