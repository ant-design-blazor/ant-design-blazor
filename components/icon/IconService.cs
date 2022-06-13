using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                string svgStyle = $"focusable=\"false\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\" {(rotate == 0 ? $"style=\"pointer-events: none;\"" : $"style=\"pointer-events: none;transform: rotate({rotate}deg);\"")}";
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

        public async Task<string> GetTwotoneSvgIcon(string svgImg, string twotoneColor)
        {
            svgImg = svgImg.Replace("fill=\"#333\"", "fill=\"primaryColor\"")
                .Replace("fill=\"#E6E6E6\"", "fill=\"secondaryColor\"")
                .Replace("fill=\"#D9D9D9\"", "fill=\"secondaryColor\"")
                .Replace("fill=\"#D8D8D8\"", "fill=\"secondaryColor\"");
            
            var secondaryColors = await _js.InvokeAsync<string[]>(JSInteropConstants.GenerateColor, twotoneColor);

            var document = XDocument.Load(new StringReader(svgImg));
            var svg = document.Root;
            foreach (var path in svg.Nodes().OfType<XElement>())
            {
                if (path.Attribute("fill")?.Value == "secondaryColor")
                {
                    path.SetAttributeValue("fill", secondaryColors[0]);
                }
                else
                {
                    path.SetAttributeValue("fill", twotoneColor);
                }
            }

            return svg.ToString();
        }
    }
}
