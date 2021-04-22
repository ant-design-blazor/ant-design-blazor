using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Core.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static bool IsBrowser(this IJSRuntime jsRuntime) => RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));

        public static async Task FocusAsync(this IJSRuntime jSRuntime, ElementReference target, bool preventScroll = false)
        {
#if NET5_0_OR_GREATER
            await target.FocusAsync();                    
#else
            try
            {
                await jSRuntime.InvokeVoidAsync(JSInteropConstants.Focus, target, preventScroll);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
#endif
        }

        public static ValueTask SetSelectionStartAsync(this IJSRuntime jSRuntime, ElementReference target, int selectionStart) =>
            jSRuntime.InvokeVoidAsync(JSInteropConstants.SetSelectionStart, target, selectionStart);

        public static ValueTask<bool> IsIE11(this IJSRuntime jSRuntime) => jSRuntime.InvokeAsync<bool>(JSInteropConstants.IsIE11);
    }
}
