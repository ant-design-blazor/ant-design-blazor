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
     
        public async Task UseCustomTheme(string cssLinkPath, string cssLinkId)
        {
            if (string.IsNullOrWhiteSpace(cssLinkId)) throw new Exception("未指定Css样式的 CssLinkId");
            if (string.IsNullOrWhiteSpace(cssLinkPath)) throw new Exception("未指定Css样式的 链接");

            var js = @$"document.getElementById(""{cssLinkId}"").setAttribute(""href"", ""{cssLinkPath}"");";
            await _jS.InvokeVoidAsync("eval", js);
        }

        public async Task UseTheme(GlobalThemeMode themeMode)
        {
            string css = "";
            switch (themeMode.Name.ToLower())
            {
                case "light":
                    css = "/_content/AntDesign/css/ant-design-blazor.css";
                    break;
                case "dark":
                    css = "/_content/AntDesign/css/ant-design-blazor.dark.css";
                    break;
                case "compact":
                    css = "/_content/AntDesign/css/ant-design-blazor.compact.css";
                    break;
                case "aliyun":
                    css = "/_content/AntDesign/css/ant-design-blazor.aliyun.css";
                    break;
            }

            var js = @$"Array.from(document.getElementsByTagName(""link"")).forEach((item) => {{ if (item.getAttribute(""href"").match(""_content/AntDesign/css/ant-design-blazor"") ) {{ item.setAttribute(""href"", ""{css}""); return; }} }})";
            await _jS.InvokeVoidAsync("eval", js);
        }

    }
}
