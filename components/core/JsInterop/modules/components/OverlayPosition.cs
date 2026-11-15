// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace AntDesign.Core.JsInterop.Modules.Components
{
    public class OverlayPosition
    {
        public decimal? Top { get; set; }
        public string TopCss => UseTop ? $"top: " + GetAsString(Top) : string.Empty;
        public decimal? Bottom { get; set; }
        public string BottomCss => UseBottom ? $"bottom: " + GetAsString(Bottom) : string.Empty;
        public decimal? Left { get; set; }
        public string LeftCss => UseLeft ? $"left: " + GetAsString(Left) : string.Empty;
        public decimal? Right { get; set; }
        public string RightCss => UseRight ? $"right: " + GetAsString(Right) : string.Empty;
        public string PositionCss => ZIndexCss + FixedCss + LeftCss + RightCss + TopCss + BottomCss;
        public string PositionCssWithoutZIndex => FixedCss + LeftCss + RightCss + TopCss + BottomCss;

        public bool PositionFixed { get; set; }
        private string FixedCss => PositionFixed ? "position:fixed;" : string.Empty;
        public bool UseTop { get; set; }
        public bool UseBottom { get; set; }
        public bool UseLeft { get; set; }
        public bool UseRight { get; set; }

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
