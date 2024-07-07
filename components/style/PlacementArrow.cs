// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;

namespace AntDesign
{
    public class ArrowPlacement
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Top { get; set; }
        public bool Bottom { get; set; }
    }

    public class PlacementArrowOptions
    {
        public string ColorBg { get; set; }
        public string ShowArrowCls { get; set; }
        public double ContentRadius { get; set; }
        public bool LimitVerticalRadius { get; set; }
        public double ArrowDistance { get; set; }
        public ArrowPlacement ArrowPlacement { get; set; }
    }

    public class ArrowOffsetOptions
    {
        public double ContentRadius { get; set; }
        public bool LimitVerticalRadius { get; set; }
    }

    public class ArrowOffset
    {
        public double DropdownArrowOffset { get; set; }
        public double DropdownArrowOffsetVertical { get; set; }
    }

    public class PlacementArrow
    {
        public const double MAX_VERTICAL_CONTENT_RADIUS = 8;

        public static ArrowOffset GetArrowOffset(ArrowOffsetOptions options)
        {
            return new ArrowOffset
            {

            };
        }

        public static CSSObject GetArrowStyle(TokenWithCommonCls token, PlacementArrowOptions options)
        {
            return new CSSObject();
        }
    }
}
