using System;
using System.Threading.Tasks;
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
            await this.JsInvokeAsync(JSInteropConstants.copy, text);
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
    }
}
