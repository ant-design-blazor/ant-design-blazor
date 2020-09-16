using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerTemplate<TComponentOptions, TResult> : TemplateComponentBase<TComponentOptions>
    {
        [Parameter]
        public DrawerRef<TResult> DrawerRef { get; set; }

        /// <summary>
        /// Close the drawer
        /// </summary>
        /// <returns></returns>
        protected async Task CloseAsync(TResult result = default)
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
