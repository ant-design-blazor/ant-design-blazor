using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalTemplate<TContentParams, TResult> : TemplateComponentBase<TContentParams>
    {
        [Parameter]
        public ModalRef<TResult> ModalRef { get; set; }


        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <returns></returns>
        protected async Task CloseAsync(TResult result)
        {
            await ModalRef.CloseAsync(result);
        }


        protected override void OnInitialized()
        {
            base.OnInitialized();
            ModalRef.OnOpen?.Invoke();
        }
    }
}
