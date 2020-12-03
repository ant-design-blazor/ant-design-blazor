using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.JsInterop
{
    public class InteropService
    {
        private readonly IJSRuntime _js;

        public InteropService(IJSRuntime js)
        {
            _js = js;
        }

        public async ValueTask Copy(string text)
        {
            await this.JsInvokeAsync(JSInteropConstants.Copy, text);
        }

        private async Task<T> JsInvokeAsync<T>(string code, params object[] args)
        {
            try
            {
                return await _js.InvokeAsync<T>(code, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task JsInvokeAsync(string code, params object[] args)
        {
            try
            {
                await _js.InvokeVoidAsync(code, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region 

        public async Task<Element> GetDomInfo(ElementReference elemRef)
        {
            return await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, elemRef);
        }

        public async Task<Window> GetWindow()
        {
            return await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
        }

        #endregion
    }
}
