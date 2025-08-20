// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        internal static string GetIconImg(string type, IconThemeType theme)
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
                : icon.Type.EndsWith("</svg>", StringComparison.OrdinalIgnoreCase) ? icon.Type :
                GetIconImg(icon.Type.ToLowerInvariant(), icon.Theme);

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

            if (icon.Theme == IconThemeType.TwoTone)
            {
                return GetTwoToneIconSvg(iconSvg, icon.TwoToneColor, icon.SecondaryColor);
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

        public static IDictionary<IconThemeType, string[]> GetAllIcons()
            => IconStore.AllIconsByTheme.Value;

        public static bool IconExists(IconThemeType iconTheme = IconThemeType.Outline, string iconName = "")
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
