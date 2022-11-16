using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AntDesign
{
    public class GlobalThemeService
    {
        private IJSRuntime _jS;
        public GlobalThemeService(IJSRuntime js)
        {
            _jS = js;
        }

        public async Task UseCustomTheme(string cssLinkPath, string cssLinkId = "antcss")
        {
            if (string.IsNullOrWhiteSpace(cssLinkId)) throw new Exception("未指定Css样式的 CssLinkId");
            if (string.IsNullOrWhiteSpace(cssLinkPath)) throw new Exception("未指定Css样式的 链接");

            var js = @$"document.getElementById(""{cssLinkId}"").setAttribute(""href"", ""{cssLinkPath}"");";
            await _jS.InvokeVoidAsync("eval", js);
        }

        public async Task UseTheme(GlobalThemeMode themeMode, string cssLinkId = "antcss")
        {
            switch (themeMode.Name.ToLower())
            {
                case "light":
                    await UseCustomTheme("/_content/AntDesign/css/ant-design-blazor.css", cssLinkId);
                    break;
                case "dark":
                    await UseCustomTheme("/_content/AntDesign/css/ant-design-blazor.dark.css", cssLinkId);
                    break;
                case "compact":
                    await UseCustomTheme("/_content/AntDesign/css/ant-design-blazor.compact.css", cssLinkId);
                    break;
                case "aliyun":
                    await UseCustomTheme("/_content/AntDesign/css/ant-design-blazor.aliyun.css", cssLinkId);
                    break;
            }
        }

    }
}
