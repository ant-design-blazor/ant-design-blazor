// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;
using System;

namespace AntDesign
{
    public class CalcColor
    {
        public string LightColor { get; set; }
        public string LightBorderColor { get; set; }
        public string DarkColor { get; set; }
        public string TextColor { get; set; }
    }

    internal class Theme
    {
        public static T MergeToken<T>(TokenWithCommonCls token, T value) where T : TokenWithCommonCls
        {
            value.Merge(token);
            return value;
        }

        public static CSSObject GenPresetColor(GlobalToken token, Func<PresetColor, CalcColor, CSSObject> func)
        {
            return new CSSObject();
        }
    }
}
