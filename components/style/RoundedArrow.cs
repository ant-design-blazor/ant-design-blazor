// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CssInCSharp;

namespace AntDesign
{
    internal partial class GlobalStyle
    {
        public static CSSObject RoundedArrow(double width, double innerRadius, double outerRadius, string bgColor, string boxShadow)
        {
            var unitWidth = width / 2;
            var ax = 0;
            var ay = unitWidth;
            var bx = (outerRadius * 1) / Math.Sqrt(2);
            var by = unitWidth - outerRadius * (1 - 1 / Math.Sqrt(2));
            var cx = unitWidth - innerRadius * (1 / Math.Sqrt(2));
            var cy = outerRadius * (Math.Sqrt(2) - 1) + innerRadius * (1 / Math.Sqrt(2));
            var dx = 2 * unitWidth - cx;
            var dy = cy;
            var ex = 2 * unitWidth - bx;
            var ey = by;
            var fx = 2 * unitWidth - ax;
            var fy = ay;
            var shadowWidth = unitWidth * Math.Sqrt(2) + outerRadius * (Math.Sqrt(2) - 2);
            var polygonOffset = outerRadius * (Math.Sqrt(2) - 1);
            return new CSSObject
            {
                PointerEvents = "none",
                Width = width,
                Height = width,
                Overflow = "hidden",
                ["&::before"] = new CSSObject
                {
                    Position = "absolute",
                    Bottom = 0,
                    InsetInlineStart = 0,
                    Width = width,
                    Height = width / 2,
                    Background = bgColor,
                    ClipPath = $"polygon({polygonOffset}px 100%, 50% {polygonOffset}px,{2 * unitWidth - polygonOffset}px 100%, {polygonOffset}px 100%),path('M {ax} {ay} A {outerRadius} {outerRadius} 0 0 0 {bx} {by} L {cx} {cy} A {innerRadius} {innerRadius} 0 0 1 {dx} {dy} L {ex} {ey} A {outerRadius} {outerRadius} 0 0 0 {fx} {fy} Z')",
                    Content = "\"\""
                },
                ["&::after"] = new CSSObject
                {
                    Content = "\"\"",
                    Position = "absolute",
                    Width = shadowWidth,
                    Height = shadowWidth,
                    Bottom = 0,
                    InsetInline = 0,
                    Margin = "auto",
                    BorderRadius = $"0 0 {innerRadius}px 0",
                    Transform = "translateY(50%) rotate(-135deg)",
                    BoxShadow = boxShadow,
                    ZIndex = 0,
                    Background = "transparent",
                }
            };
        }
    }
}
