using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

#pragma warning disable CA1822
#pragma warning disable CA1054
#pragma warning disable CA1062

namespace AntDesign
{
    public class IconService
    {
        private IJSRuntime _js;

        public IconService(IJSRuntime js)
        {
            _js = js;
        }

        public static string GetIconImg(string type, string theme)
        {
            var iconImg = IconStore.GetIcon(type, theme);

            if (string.IsNullOrEmpty(iconImg))
                return null;

            return iconImg;
        }

        public static string GetStyledSvg(string svgImg, string svgClass = null, string width = "1em", string height = "1em", string fill = "currentColor", int rotate = 0)
        {
            if (!string.IsNullOrEmpty(svgImg))
            {
                string svgStyle = $"focusable=\"false\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\" {(rotate == 0 ? "" : $"style=\"transform: rotate({rotate}deg);\"")}";
                if (!string.IsNullOrEmpty(svgClass))
                {
                    svgStyle += $" class=\"{svgClass}\"";
                }

                return svgImg.Insert(svgImg.IndexOf("<svg", StringComparison.Ordinal) + 4, $" {svgStyle} ");
            }

            return null;
        }

        public async ValueTask CreateFromIconfontCN(string scriptUrl)
        {
            if (string.IsNullOrEmpty(scriptUrl))
            {
                return;
            }

            await _js.InvokeVoidAsync(JSInteropConstants.CreateIconFromfontCN, scriptUrl);
        }

        public IDictionary<string, string[]> GetAllIcons()
        {
            return IconStore.GetAllIconNames();
        }

        public bool IconExists(string theme = "", string type = "")
        {
            var icon = IconStore.GetIcon(type, theme);

            return !string.IsNullOrEmpty(icon);
        }
    }
}
