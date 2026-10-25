// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AntDesign.Icons;
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
            if (!string.IsNullOrEmpty(icon.IconFont))
            {
                var rotation = icon.Rotate == 0 ? string.Empty : $" style=\"transform: rotate({icon.Rotate}deg);\"";
                return $"<svg width=\"{WebUtility.HtmlEncode(icon.Width)}\" height=\"{WebUtility.HtmlEncode(icon.Height)}\" fill=\"{WebUtility.HtmlEncode(icon.Fill)}\" aria-hidden=\"true\" focusable=\"false\" class=\"{(icon.Spin ? "anticon-spin" : string.Empty)}\"{rotation}><use xlink:href=\"#{WebUtility.HtmlEncode(icon.IconFont)}\"></use></svg>";
            }

            if (string.IsNullOrEmpty(icon.Type)) return null;

            string svgClass = icon.Spin || icon.Type == "loading" ? "anticon-spin" : null;

            var svg = icon.Type.EndsWith("</svg>", StringComparison.OrdinalIgnoreCase) ? icon.Type :
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

        /// <summary>Loads iconfont resources in React's order, sharing requests across icon instances.</summary>
        public async ValueTask CreateFromIconfontCN(string[] scriptUrls)
        {
            if (scriptUrls == null || scriptUrls.Length == 0) return;
            await _js.InvokeVoidAsync(JSInteropConstants.CreateIconFromfontCN, (object)scriptUrls);
        }

        public static IDictionary<IconThemeType, string[]> GetAllIcons()
            => IconStore.AllIconsByTheme.Value;

        public static bool IconExists(IconThemeType iconTheme = IconThemeType.Outline, string iconName = "")
        {
            var icon = IconStore.GetIcon(iconName, iconTheme);

            return !string.IsNullOrEmpty(icon);
        }

        /// <summary>Computes the first light palette color without JavaScript.</summary>
        public Task<string> GetSecondaryColor(string primaryColor)
            => Task.FromResult(Colors.ColorPalette.Generate(string.IsNullOrWhiteSpace(primaryColor) ? "#1677ff" : primaryColor)[0]);

        private static string GetTwoToneIconSvg(string iconSvg, string primaryColor, string secondaryColor)
        {
            // Only replace the catalog's color slots; preserve nested groups,
            // explicit fill="none", and all unrelated SVG attributes.
            var primary = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(primaryColor) ? "#1677ff" : primaryColor);
            var secondary = WebUtility.HtmlEncode(secondaryColor);
            return iconSvg.Replace("fill=\"#333\"", $"fill=\"{primary}\"")
                .Replace("fill=\"#E6E6E6\"", $"fill=\"{secondary}\"")
                .Replace("fill=\"#D9D9D9\"", $"fill=\"{secondary}\"")
                .Replace("fill=\"#D8D8D8\"", $"fill=\"{secondary}\"");
        }
    }
}
