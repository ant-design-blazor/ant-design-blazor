// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public static async Task BlurAsyc(this IJSRuntime jSRuntime, ElementReference target)
        {
            try
            {
                await jSRuntime.InvokeVoidAsync(JSInteropConstants.Blur, target);
            }
            catch
            {
                throw;
            }
        }

        public static async Task FocusAsync(this IJSRuntime jSRuntime, ElementReference target, bool preventScroll = false)
        {
#if NET6_0_OR_GREATER
            await target.FocusAsync(preventScroll);
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

        public static async Task FocusAsync(this IJSRuntime jSRuntime, ElementReference target, FocusBehavior behavior, bool preventScroll = false)
        {
            try
            {
                await jSRuntime.InvokeVoidAsync(JSInteropConstants.Focus, target, preventScroll, behavior);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static ValueTask SetSelectionStartAsync(this IJSRuntime jSRuntime, ElementReference target, int selectionStart) =>
            jSRuntime.InvokeVoidAsync(JSInteropConstants.SetSelectionStart, target, selectionStart);

        public static ValueTask<bool> IsResizeObserverSupported(this IJSRuntime jSRuntime) => jSRuntime.InvokeAsync<bool>(JSInteropConstants.ObserverConstants.Resize.IsResizeObserverSupported);
    }
}
