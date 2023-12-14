// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;

namespace AntDesign
{
    public class PlacementArrowOptions
    {
        public string ColorBg { get; set; }
    }

    public class PlacementArrow
    {
        public static CSSObject GetArrowStyle(TokenWithCommonCls token, PlacementArrowOptions options)
        {
            return new CSSObject();
        }
    }
}
