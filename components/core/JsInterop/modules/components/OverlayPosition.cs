// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace AntDesign.Core.JsInterop.Modules.Components
{
    public class OverlayPosition
    {
        public decimal? Top { get; set; }
        public string TopCss => $"top: " + GetAsString(Top);
        public decimal? Bottom { get; set; }
        public string BottomCss => $"bottom: " + GetAsString(Bottom);
        public decimal? Left { get; set; }
        public string LeftCss => $"left: " + GetAsString(Left);
        public decimal? Right { get; set; }
        public string RightCss => $"right: " + GetAsString(Right);
        public string PositionCss => ZIndexCss + LeftCss + RightCss + TopCss + BottomCss;

        public int ZIndex { get; set; }
        public string ZIndexCss => $"z-index:{ZIndex};";

        public Placement Placement { get; set; }

        private string GetAsString(decimal? value)
        {
            if (value is null)
                return "unset;";
            else
                return string.Format(CultureInfo.InvariantCulture, "{0}px;", value);
        }
    }
}
