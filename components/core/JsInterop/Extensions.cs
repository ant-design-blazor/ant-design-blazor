using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AntBlazor
{
    public static class Extensions
    {
        public static async Task InitCsInstance<T>(this IJSRuntime jsRuntime,
            DotNetObjectReference<T> dotNetObjectReference,
            string instanceName)
            where T : class
        {
            await jsRuntime.InvokeVoidAsync(JSInteropConstants.initCsInstance,
                dotNetObjectReference,
                instanceName);
        }

        public static async Task DelCsInstance(this IJSRuntime jsRuntime, string instanceName)
        {
            await jsRuntime.InvokeVoidAsync(JSInteropConstants.delCsInstance,
                instanceName);
        }

        public static async Task AppendHtml(this IJSRuntime jsRuntime, string html, string selector = null)
        {
            await jsRuntime.InvokeVoidAsync(JSInteropConstants.appendHtml,
                html, selector);
        }

        public static async Task RemoveElement(this IJSRuntime jsRuntime, string selector)
        {
            await jsRuntime.InvokeVoidAsync(JSInteropConstants.removeElement,
                selector);
        }

    }
}
