// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
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

            var svg = icon.Type.EndsWith("</svg>", StringComparison.OrdinalIgnoreCase) ? icon.Type :
                GetIconImg(icon.Type.ToLowerInvariant(), icon.Theme);

            if (string.IsNullOrEmpty(svg))
            {
                return null;
            }

            var svgStyle = icon.Rotate == 0 ? string.Empty : $"transform: rotate({icon.Rotate}deg);";
            var svgAttributes = new Dictionary<string, string>
            {
                ["focusable"] = "false",
                ["width"] = icon.Width,
                ["height"] = icon.Height,
                ["fill"] = icon.Fill,
                ["aria-hidden"] = "true"
            };
            if (!icon.Type.EndsWith("</svg>", StringComparison.OrdinalIgnoreCase))
                svgAttributes["data-icon"] = icon.Type.ToLowerInvariant();
            if (!string.IsNullOrEmpty(svgStyle))
                svgAttributes["style"] = svgStyle;
            var iconSvg = SetSvgRootAttributes(svg, svgAttributes);

            if (icon.Theme == IconThemeType.TwoTone
                && (!string.IsNullOrWhiteSpace(icon.TwoToneColor) || !string.IsNullOrWhiteSpace(icon.TwoToneSecondaryColor)))
            {
                return GetTwoToneIconSvg(iconSvg, icon.TwoToneColor, icon.SecondaryColor);
            }

            return iconSvg;
        }

        private static string SetSvgRootAttributes(string svg, IDictionary<string, string> attributes)
        {
            var svgStart = svg.IndexOf("<svg", StringComparison.OrdinalIgnoreCase);
            if (svgStart < 0) return svg;
            var svgEnd = svg.IndexOf('>', svgStart + 4);
            if (svgEnd < 0) return svg;

            var root = svg.Substring(svgStart, svgEnd - svgStart + 1);
            foreach (var attribute in attributes)
            {
                var value = WebUtility.HtmlEncode(attribute.Value ?? string.Empty);
                var pattern = $"(?<![\\w:-]){Regex.Escape(attribute.Key)}\\s*=\\s*(?:\"[^\"]*\"|'[^']*')";
                var matcher = new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                var replacement = $"{attribute.Key}=\"{value}\"";
                root = matcher.IsMatch(root)
                    ? matcher.Replace(root, _ => replacement, 1)
                    : root.Insert(root.Length - 1, $" {replacement}");
            }

            return svg.Substring(0, svgStart) + root + svg.Substring(svgEnd + 1);
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
            var result = iconSvg;
            if (!string.IsNullOrWhiteSpace(primaryColor))
            {
                var primary = WebUtility.HtmlEncode(primaryColor);
                result = result.Replace("fill=\"#333\"", $"fill=\"{primary}\"");
            }

            if (!string.IsNullOrWhiteSpace(secondaryColor))
            {
                var secondary = WebUtility.HtmlEncode(secondaryColor);
                result = result.Replace("fill=\"#E6E6E6\"", $"fill=\"{secondary}\"")
                    .Replace("fill=\"#D9D9D9\"", $"fill=\"{secondary}\"")
                    .Replace("fill=\"#D8D8D8\"", $"fill=\"{secondary}\"");
            }

            return result;
        }
    }
}
