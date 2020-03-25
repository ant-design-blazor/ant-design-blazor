using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

        protected async Task OnClick(MouseEventArgs args)
        {
            //JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, Ref);
        }


    }
}
