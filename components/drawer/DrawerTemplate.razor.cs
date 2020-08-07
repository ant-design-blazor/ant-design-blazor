using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerTemplate<TContentParams, TResult> : TemplateComponentBase<TContentParams>
    {
        [Parameter] public DrawerRef<TResult> DrawerRef { get; set; }


        /// <summary>
        /// 关闭抽屉
        /// </summary>
        /// <returns></returns>
        protected async Task CloseAsync(TResult result)
        {
            await DrawerRef.CloseAsync(result);
        }


        protected override void OnInitialized()
        {
            base.OnInitialized();
            DrawerRef.OnOpen?.Invoke();
        }
    }
}
