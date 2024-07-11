// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class CalcColor
    {
        public string LightColor { get; set; }
        public string LightBorderColor { get; set; }
        public string DarkColor { get; set; }
        public string TextColor { get; set; }
    }

    internal static class Theme
    {
        public static T MergeToken<T>(TokenWithCommonCls token, T value) where T : IToken
        {
            value.Merge(token);
            return value;
        }

        public static T MergeToken<T>(GlobalToken token, T value) where T : IToken
        {
            value.Merge(token);
            return value;
        }

        public static T MergeToken<T>(params IToken[] tokens) where T : IToken, new()
        {
            var token = new T();
            foreach (var item in tokens)
            {
                token.Merge(item);
            }
            return token;
        }

        public static CSSObject GenPresetColor(GlobalToken token, Func<string, CalcColor, CSSObject> genCss)
        {
            var css = new CSSObject();
            var colorKeys = Enum.GetNames(typeof(PresetColor)).Select(x => x.ToLower());
            foreach (var colorKey in colorKeys)
            {
                var lightColor = token[$"{colorKey}1"].To<string>();
                var lightBorderColor = token[$"{colorKey}3"].To<string>();
                var darkColor = token[$"{colorKey}6"].To<string>();
                var textColor = token[$"{colorKey}7"].To<string>();
                css["..."] = genCss(colorKey, new CalcColor()
                {
                    LightColor = lightColor,
                    LightBorderColor = lightBorderColor,
                    DarkColor = darkColor,
                    TextColor = textColor
                });
            }
            return css;
        }

        public static string Join(this IEnumerable<string> arr, string separator)
        {
            return string.Join(separator, arr);
        }

        public static T To<T>(this object property)
        {
            if (property == default)
            {
                return default(T);
            }
            return (T)property;
        }
    }
}
