using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DrawerTemplate<TConfig> : TemplateComponentBase<TConfig>
    {
        [Parameter]
        public DrawerConfig DrawerConfig { get; set; }

        /// <summary>
        /// close this drawer
        /// </summary>
        /// <returns></returns>
        protected async Task HandleClose()
        {
            await DrawerConfig.HandleOnClose();
        }
    }
}
