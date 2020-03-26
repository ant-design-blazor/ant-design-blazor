using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntBlazor
{
    public class AntBackTopBase : AntDomComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        //[Parameter]
        //public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

        protected async Task OnClick(MouseEventArgs args)
        {
            await JsInvokeAsync(JSInteropConstants.backTop, "BodyConatainer");
        }

        //protected async Task OnScroll(EventArgs args)
        //{
        //    await JsInvokeAsync(JSInteropConstants.getDomInfo, Ref);
        //}
        protected override void OnInitialized()
        {
            SetClass();

            base.OnInitialized();
        }
        protected void SetClass()
        {
            string prefixCls = "ant-backtop";
            ClassMapper.Clear()
                .If($"{prefixCls}-wrapper", () => !RadioButton)
                .If($"{prefixCls}-button-wrapper", () => RadioButton);
        }
    }
}
