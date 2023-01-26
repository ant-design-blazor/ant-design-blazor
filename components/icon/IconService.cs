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

        internal static string GetIconImg(string type, string theme)
        {
            var iconImg = IconStore.GetIcon(type, theme);

            if (string.IsNullOrEmpty(iconImg))
                return null;

            return iconImg;
        }

        internal static string GetStyledSvg(Icon icon)
        {
            string svgClass = icon.Spin || icon.Type == "loading" ? "anticon-spin" : null;

            var svg = !string.IsNullOrEmpty(icon.IconFont) ?
                $"<svg><use xlink:href=#{icon.IconFont} /></svg>"
                : GetIconImg(icon.Type.ToLowerInvariant(), icon.Theme.ToLowerInvariant());

            if (string.IsNullOrEmpty(svg))
            {
                return null;
            }

            string svgStyle = $"focusable=\"false\" width=\"{icon.Width}\" height=\"{icon.Height}\" fill=\"{icon.Fill}\" {(icon.Rotate == 0 ? $"style=\"pointer-events: none;\"" : $"style=\"pointer-events: none;transform: rotate({icon.Rotate}deg);\"")}";
            if (!string.IsNullOrEmpty(svgClass))
            {
                svgStyle += $" class=\"{svgClass}\"";
            }

            var iconSvg = svg.Insert(svg.IndexOf("<svg", StringComparison.Ordinal) + 4, $" {svgStyle} ");

            if (icon.Theme == IconThemeType.Twotone)
            {
                return GetTwoToneIconSvg(iconSvg, icon.TwotoneColor, icon.SecondaryColor);
            }

            return iconSvg;
        }

        public async ValueTask CreateFromIconfontCN(string scriptUrl)
        {
            if (string.IsNullOrEmpty(scriptUrl))
            {
                return;
            }

            await _js.InvokeVoidAsync(JSInteropConstants.CreateIconFromfontCN, scriptUrl);
        }

        public static IDictionary<string, string[]> GetAllIcons()
            => IconStore.AllIconsByTheme.Value;

        public static bool IconExists(string iconTheme = "", string iconName = "")
        {
            var icon = IconStore.GetIcon(iconName, iconTheme);

            return !string.IsNullOrEmpty(icon);
        }

        public async Task<string> GetSecondaryColor(string primaryColor)
        {
            var secondaryColors = await _js.InvokeAsync<string[]>(JSInteropConstants.GenerateColor, primaryColor);
            return secondaryColors[0];
        }

        private static string GetTwoToneIconSvg(string iconSvg, string primaryColor, string secondaryColor)
        {
            if (string.IsNullOrWhiteSpace(secondaryColor))
            {
                return iconSvg.Replace("fill=\"#333\"", $"fill=\"{primaryColor}\"");
            }

            iconSvg = iconSvg.Replace("fill=\"#333\"", "fill=\"primaryColor\"")
           .Replace("fill=\"#E6E6E6\"", "fill=\"secondaryColor\"")
           .Replace("fill=\"#D9D9D9\"", "fill=\"secondaryColor\"")
           .Replace("fill=\"#D8D8D8\"", "fill=\"secondaryColor\"");

            var document = XDocument.Load(new StringReader(iconSvg));
            var svgRoot = document.Root;
            foreach (var path in svgRoot.Nodes().OfType<XElement>())
            {
                if (path.Attribute("fill")?.Value == "secondaryColor")
                {
                    path.SetAttributeValue("fill", secondaryColor);
                }
                else
                {
                    path.SetAttributeValue("fill", primaryColor);
                }
            }

            return svgRoot.ToString();
        }
    }
}
