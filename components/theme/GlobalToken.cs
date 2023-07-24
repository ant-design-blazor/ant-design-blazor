// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public partial class GlobalToken
    {
        public GlobalToken()
        {
            // Color
            ColorPrimary = "#1677ff";
            ColorSuccess = "#52c41a";
            ColorWarning = "#faad14";
            ColorError = "#ff4d4f";
            ColorInfo = "#1677ff";
            ColorTextBase = "";
            ColorBgBase = "";
            // Font
            FontFamily = "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial,'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol','Noto Color Emoji'";
            FontFamilyCode = "'SFMono-Regular', Consolas, 'Liberation Mono', Menlo, Courier, monospace";
            FontSize = 14;
            // Line
            LineWidth = 1;
            LineType = "solid";
            // Motion
            MotionUnit = 0.1f;
            MotionBase = 0;
            MotionEaseOutCirc = "cubic-bezier(0.08, 0.82, 0.17, 1)";
            MotionEaseInOutCirc = "cubic-bezier(0.78, 0.14, 0.15, 0.86)";
            MotionEaseOut = "cubic-bezier(0.215, 0.61, 0.355, 1)";
            MotionEaseInOut = "cubic-bezier(0.645, 0.045, 0.355, 1)";
            MotionEaseOutBack = "cubic-bezier(0.12, 0.4, 0.29, 1.46)";
            MotionEaseInBack = "cubic-bezier(0.71, -0.46, 0.88, 0.6)";
            MotionEaseInQuint = "cubic-bezier(0.755, 0.05, 0.855, 0.06)";
            MotionEaseOutQuint = "cubic-bezier(0.23, 1, 0.32, 1)";
            // Radius
            BorderRadius = 6;

            // Size
            SizeUnit = 4;
            SizeStep = 4;
            SizePopupArrow = 16;

            // Control Base
            ControlHeight = 32;

            // zIndex
            ZIndexBase = 0;
            ZIndexPopupBase = 1000;

            // Image
            OpacityImage = 1;

            // Wireframe
            Wireframe = false;

            // Motion
            Motion = true;
        }
    }
}
