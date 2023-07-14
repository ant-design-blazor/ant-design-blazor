// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CssInCs;

namespace AntDesign
{
    internal class GlobalStyle
    {
        public static CSSObject TextEllipsis = new()
        {
            Overflow = "hidden",
            WhiteSpace = "nowrap",
            TextOverflow = "ellipsis"
        };

        public static CSSObject ResetComponent(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject ResetIcon()
        {
            return new CSSObject();
        }

        public static CSSObject ClearFix()
        {
            return new CSSObject();
        }

        public static CSSObject GenLinkStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenCommonStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenFocusOutline(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenFocusStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenCompactItemStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenCompactItemVerticalStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenPanelStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GetCheckboxStyle(string key, GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenCollapseMotion(GlobalToken token)
        {
            return new CSSObject();
        }
    }
}
