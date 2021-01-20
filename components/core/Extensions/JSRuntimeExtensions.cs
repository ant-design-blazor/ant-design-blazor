using System.Runtime.InteropServices;
using Microsoft.JSInterop;

namespace AntDesign.Core.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static bool IsBrowser(this IJSRuntime jsRuntime) => RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));
    }
}
