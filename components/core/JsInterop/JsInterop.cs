using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntBlazor.JsInterop
{
    public class JsInterop
    {
        private IJSRuntime _jsRuntime;

        public JsInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<Element> GetDom(ElementReference element)
        {
            return _jsRuntime.InvokeAsync<Element>("GetDomInfo", element);
        }
    }
}