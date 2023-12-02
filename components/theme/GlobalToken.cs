// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public record TokenHash(string TokenKey, string HashId);

    public partial class GlobalToken
    {
        private readonly Dictionary<string, object> _tokens = new();
        private TokenHash _tokenHash;
#if DEBUG
        private const string HashPrefix = "css-dev-only-do-not-override";
#else
        private const string HashPrefix = "css";
#endif
        /*
         * todo: using AddAntDesign() method instead.
         * eg:
         *  service.AddAntDesign(option =>
         *  {
         *      option.UseDefaultTheme();  // useDefaultTheme
         *  });
         *
         * note:
         *  style cache using tokenKey + tokenValue as cacheKey,
         *  need to keep the order of properties.
         */
        public GlobalToken()
        {
            Blue = "#1677ff";
            Purple = "#722ED1";
            Cyan = "#13C2C2";
            Green = "#52C41A";
            Magenta = "#EB2F96";
            Pink = "#eb2f96";
            Red = "#F5222D";
            Orange = "#FA8C16";
            Yellow = "#FADB14";
            Volcano = "#FA541C";
            Geekblue = "#2F54EB";
            Gold = "#FAAD14";
            Lime = "#A0D911";
            // Color
            ColorPrimary = "#1677ff";
            ColorSuccess = "#52c41a";
            ColorWarning = "#faad14";
            ColorError = "#ff4d4f";
            ColorInfo = "#1677ff";
            ColorLink = "#1677ff";
            ColorTextBase = "#000";
            ColorBgBase = "#fff";
            // Font
            FontFamily = "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial,\n'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol',\n'Noto Color Emoji'";
            FontFamilyCode = "'SFMono-Regular', Consolas, 'Liberation Mono', Menlo, Courier, monospace";
            FontSize = 14;
            // Line
            LineWidth = 1;
            LineType = "solid";
            // Motion
            MotionUnit = 0.1;
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

            // color
            _tokens.Add("blue-1", "#e6f4ff");
            _tokens.Add("blue1", "#e6f4ff");
            _tokens.Add("blue-2", "#bae0ff");
            _tokens.Add("blue2", "#bae0ff");
            _tokens.Add("blue-3", "#91caff");
            _tokens.Add("blue3", "#91caff");
            _tokens.Add("blue-4", "#69b1ff");
            _tokens.Add("blue4", "#69b1ff");
            _tokens.Add("blue-5", "#4096ff");
            _tokens.Add("blue5", "#4096ff");
            _tokens.Add("blue-6", "#1677ff");
            _tokens.Add("blue6", "#1677ff");
            _tokens.Add("blue-7", "#0958d9");
            _tokens.Add("blue7", "#0958d9");
            _tokens.Add("blue-8", "#003eb3");
            _tokens.Add("blue8", "#003eb3");
            _tokens.Add("blue-9", "#002c8c");
            _tokens.Add("blue9", "#002c8c");
            _tokens.Add("blue-10", "#001d66");
            _tokens.Add("blue10", "#001d66");
            _tokens.Add("purple-1", "#f9f0ff");
            _tokens.Add("purple1", "#f9f0ff");
            _tokens.Add("purple-2", "#efdbff");
            _tokens.Add("purple2", "#efdbff");
            _tokens.Add("purple-3", "#d3adf7");
            _tokens.Add("purple3", "#d3adf7");
            _tokens.Add("purple-4", "#b37feb");
            _tokens.Add("purple4", "#b37feb");
            _tokens.Add("purple-5", "#9254de");
            _tokens.Add("purple5", "#9254de");
            _tokens.Add("purple-6", "#722ed1");
            _tokens.Add("purple6", "#722ed1");
            _tokens.Add("purple-7", "#531dab");
            _tokens.Add("purple7", "#531dab");
            _tokens.Add("purple-8", "#391085");
            _tokens.Add("purple8", "#391085");
            _tokens.Add("purple-9", "#22075e");
            _tokens.Add("purple9", "#22075e");
            _tokens.Add("purple-10", "#120338");
            _tokens.Add("purple10", "#120338");
            _tokens.Add("cyan-1", "#e6fffb");
            _tokens.Add("cyan1", "#e6fffb");
            _tokens.Add("cyan-2", "#b5f5ec");
            _tokens.Add("cyan2", "#b5f5ec");
            _tokens.Add("cyan-3", "#87e8de");
            _tokens.Add("cyan3", "#87e8de");
            _tokens.Add("cyan-4", "#5cdbd3");
            _tokens.Add("cyan4", "#5cdbd3");
            _tokens.Add("cyan-5", "#36cfc9");
            _tokens.Add("cyan5", "#36cfc9");
            _tokens.Add("cyan-6", "#13c2c2");
            _tokens.Add("cyan6", "#13c2c2");
            _tokens.Add("cyan-7", "#08979c");
            _tokens.Add("cyan7", "#08979c");
            _tokens.Add("cyan-8", "#006d75");
            _tokens.Add("cyan8", "#006d75");
            _tokens.Add("cyan-9", "#00474f");
            _tokens.Add("cyan9", "#00474f");
            _tokens.Add("cyan-10", "#002329");
            _tokens.Add("cyan10", "#002329");
            _tokens.Add("green-1", "#f6ffed");
            _tokens.Add("green1", "#f6ffed");
            _tokens.Add("green-2", "#d9f7be");
            _tokens.Add("green2", "#d9f7be");
            _tokens.Add("green-3", "#b7eb8f");
            _tokens.Add("green3", "#b7eb8f");
            _tokens.Add("green-4", "#95de64");
            _tokens.Add("green4", "#95de64");
            _tokens.Add("green-5", "#73d13d");
            _tokens.Add("green5", "#73d13d");
            _tokens.Add("green-6", "#52c41a");
            _tokens.Add("green6", "#52c41a");
            _tokens.Add("green-7", "#389e0d");
            _tokens.Add("green7", "#389e0d");
            _tokens.Add("green-8", "#237804");
            _tokens.Add("green8", "#237804");
            _tokens.Add("green-9", "#135200");
            _tokens.Add("green9", "#135200");
            _tokens.Add("green-10", "#092b00");
            _tokens.Add("green10", "#092b00");
            _tokens.Add("magenta-1", "#fff0f6");
            _tokens.Add("magenta1", "#fff0f6");
            _tokens.Add("magenta-2", "#ffd6e7");
            _tokens.Add("magenta2", "#ffd6e7");
            _tokens.Add("magenta-3", "#ffadd2");
            _tokens.Add("magenta3", "#ffadd2");
            _tokens.Add("magenta-4", "#ff85c0");
            _tokens.Add("magenta4", "#ff85c0");
            _tokens.Add("magenta-5", "#f759ab");
            _tokens.Add("magenta5", "#f759ab");
            _tokens.Add("magenta-6", "#eb2f96");
            _tokens.Add("magenta6", "#eb2f96");
            _tokens.Add("magenta-7", "#c41d7f");
            _tokens.Add("magenta7", "#c41d7f");
            _tokens.Add("magenta-8", "#9e1068");
            _tokens.Add("magenta8", "#9e1068");
            _tokens.Add("magenta-9", "#780650");
            _tokens.Add("magenta9", "#780650");
            _tokens.Add("magenta-10", "#520339");
            _tokens.Add("magenta10", "#520339");
            _tokens.Add("pink-1", "#fff0f6");
            _tokens.Add("pink1", "#fff0f6");
            _tokens.Add("pink-2", "#ffd6e7");
            _tokens.Add("pink2", "#ffd6e7");
            _tokens.Add("pink-3", "#ffadd2");
            _tokens.Add("pink3", "#ffadd2");
            _tokens.Add("pink-4", "#ff85c0");
            _tokens.Add("pink4", "#ff85c0");
            _tokens.Add("pink-5", "#f759ab");
            _tokens.Add("pink5", "#f759ab");
            _tokens.Add("pink-6", "#eb2f96");
            _tokens.Add("pink6", "#eb2f96");
            _tokens.Add("pink-7", "#c41d7f");
            _tokens.Add("pink7", "#c41d7f");
            _tokens.Add("pink-8", "#9e1068");
            _tokens.Add("pink8", "#9e1068");
            _tokens.Add("pink-9", "#780650");
            _tokens.Add("pink9", "#780650");
            _tokens.Add("pink-10", "#520339");
            _tokens.Add("pink10", "#520339");
            _tokens.Add("red-1", "#fff1f0");
            _tokens.Add("red1", "#fff1f0");
            _tokens.Add("red-2", "#ffccc7");
            _tokens.Add("red2", "#ffccc7");
            _tokens.Add("red-3", "#ffa39e");
            _tokens.Add("red3", "#ffa39e");
            _tokens.Add("red-4", "#ff7875");
            _tokens.Add("red4", "#ff7875");
            _tokens.Add("red-5", "#ff4d4f");
            _tokens.Add("red5", "#ff4d4f");
            _tokens.Add("red-6", "#f5222d");
            _tokens.Add("red6", "#f5222d");
            _tokens.Add("red-7", "#cf1322");
            _tokens.Add("red7", "#cf1322");
            _tokens.Add("red-8", "#a8071a");
            _tokens.Add("red8", "#a8071a");
            _tokens.Add("red-9", "#820014");
            _tokens.Add("red9", "#820014");
            _tokens.Add("red-10", "#5c0011");
            _tokens.Add("red10", "#5c0011");
            _tokens.Add("orange-1", "#fff7e6");
            _tokens.Add("orange1", "#fff7e6");
            _tokens.Add("orange-2", "#ffe7ba");
            _tokens.Add("orange2", "#ffe7ba");
            _tokens.Add("orange-3", "#ffd591");
            _tokens.Add("orange3", "#ffd591");
            _tokens.Add("orange-4", "#ffc069");
            _tokens.Add("orange4", "#ffc069");
            _tokens.Add("orange-5", "#ffa940");
            _tokens.Add("orange5", "#ffa940");
            _tokens.Add("orange-6", "#fa8c16");
            _tokens.Add("orange6", "#fa8c16");
            _tokens.Add("orange-7", "#d46b08");
            _tokens.Add("orange7", "#d46b08");
            _tokens.Add("orange-8", "#ad4e00");
            _tokens.Add("orange8", "#ad4e00");
            _tokens.Add("orange-9", "#873800");
            _tokens.Add("orange9", "#873800");
            _tokens.Add("orange-10", "#612500");
            _tokens.Add("orange10", "#612500");
            _tokens.Add("yellow-1", "#feffe6");
            _tokens.Add("yellow1", "#feffe6");
            _tokens.Add("yellow-2", "#ffffb8");
            _tokens.Add("yellow2", "#ffffb8");
            _tokens.Add("yellow-3", "#fffb8f");
            _tokens.Add("yellow3", "#fffb8f");
            _tokens.Add("yellow-4", "#fff566");
            _tokens.Add("yellow4", "#fff566");
            _tokens.Add("yellow-5", "#ffec3d");
            _tokens.Add("yellow5", "#ffec3d");
            _tokens.Add("yellow-6", "#fadb14");
            _tokens.Add("yellow6", "#fadb14");
            _tokens.Add("yellow-7", "#d4b106");
            _tokens.Add("yellow7", "#d4b106");
            _tokens.Add("yellow-8", "#ad8b00");
            _tokens.Add("yellow8", "#ad8b00");
            _tokens.Add("yellow-9", "#876800");
            _tokens.Add("yellow9", "#876800");
            _tokens.Add("yellow-10", "#614700");
            _tokens.Add("yellow10", "#614700");
            _tokens.Add("volcano-1", "#fff2e8");
            _tokens.Add("volcano1", "#fff2e8");
            _tokens.Add("volcano-2", "#ffd8bf");
            _tokens.Add("volcano2", "#ffd8bf");
            _tokens.Add("volcano-3", "#ffbb96");
            _tokens.Add("volcano3", "#ffbb96");
            _tokens.Add("volcano-4", "#ff9c6e");
            _tokens.Add("volcano4", "#ff9c6e");
            _tokens.Add("volcano-5", "#ff7a45");
            _tokens.Add("volcano5", "#ff7a45");
            _tokens.Add("volcano-6", "#fa541c");
            _tokens.Add("volcano6", "#fa541c");
            _tokens.Add("volcano-7", "#d4380d");
            _tokens.Add("volcano7", "#d4380d");
            _tokens.Add("volcano-8", "#ad2102");
            _tokens.Add("volcano8", "#ad2102");
            _tokens.Add("volcano-9", "#871400");
            _tokens.Add("volcano9", "#871400");
            _tokens.Add("volcano-10", "#610b00");
            _tokens.Add("volcano10", "#610b00");
            _tokens.Add("geekblue-1", "#f0f5ff");
            _tokens.Add("geekblue1", "#f0f5ff");
            _tokens.Add("geekblue-2", "#d6e4ff");
            _tokens.Add("geekblue2", "#d6e4ff");
            _tokens.Add("geekblue-3", "#adc6ff");
            _tokens.Add("geekblue3", "#adc6ff");
            _tokens.Add("geekblue-4", "#85a5ff");
            _tokens.Add("geekblue4", "#85a5ff");
            _tokens.Add("geekblue-5", "#597ef7");
            _tokens.Add("geekblue5", "#597ef7");
            _tokens.Add("geekblue-6", "#2f54eb");
            _tokens.Add("geekblue6", "#2f54eb");
            _tokens.Add("geekblue-7", "#1d39c4");
            _tokens.Add("geekblue7", "#1d39c4");
            _tokens.Add("geekblue-8", "#10239e");
            _tokens.Add("geekblue8", "#10239e");
            _tokens.Add("geekblue-9", "#061178");
            _tokens.Add("geekblue9", "#061178");
            _tokens.Add("geekblue-10", "#030852");
            _tokens.Add("geekblue10", "#030852");
            _tokens.Add("gold-1", "#fffbe6");
            _tokens.Add("gold1", "#fffbe6");
            _tokens.Add("gold-2", "#fff1b8");
            _tokens.Add("gold2", "#fff1b8");
            _tokens.Add("gold-3", "#ffe58f");
            _tokens.Add("gold3", "#ffe58f");
            _tokens.Add("gold-4", "#ffd666");
            _tokens.Add("gold4", "#ffd666");
            _tokens.Add("gold-5", "#ffc53d");
            _tokens.Add("gold5", "#ffc53d");
            _tokens.Add("gold-6", "#faad14");
            _tokens.Add("gold6", "#faad14");
            _tokens.Add("gold-7", "#d48806");
            _tokens.Add("gold7", "#d48806");
            _tokens.Add("gold-8", "#ad6800");
            _tokens.Add("gold8", "#ad6800");
            _tokens.Add("gold-9", "#874d00");
            _tokens.Add("gold9", "#874d00");
            _tokens.Add("gold-10", "#613400");
            _tokens.Add("gold10", "#613400");
            _tokens.Add("lime-1", "#fcffe6");
            _tokens.Add("lime1", "#fcffe6");
            _tokens.Add("lime-2", "#f4ffb8");
            _tokens.Add("lime2", "#f4ffb8");
            _tokens.Add("lime-3", "#eaff8f");
            _tokens.Add("lime3", "#eaff8f");
            _tokens.Add("lime-4", "#d3f261");
            _tokens.Add("lime4", "#d3f261");
            _tokens.Add("lime-5", "#bae637");
            _tokens.Add("lime5", "#bae637");
            _tokens.Add("lime-6", "#a0d911");
            _tokens.Add("lime6", "#a0d911");
            _tokens.Add("lime-7", "#7cb305");
            _tokens.Add("lime7", "#7cb305");
            _tokens.Add("lime-8", "#5b8c00");
            _tokens.Add("lime8", "#5b8c00");
            _tokens.Add("lime-9", "#3f6600");
            _tokens.Add("lime9", "#3f6600");
            _tokens.Add("lime-10", "#254000");
            _tokens.Add("lime10", "#254000");
            // 
            ColorText = "rgba(0, 0, 0, 0.88)";
            ColorTextSecondary = "rgba(0, 0, 0, 0.65)";
            ColorTextTertiary = "rgba(0, 0, 0, 0.45)";
            ColorTextQuaternary = "rgba(0, 0, 0, 0.25)";
            ColorFill = "rgba(0, 0, 0, 0.15)";
            ColorFillSecondary = "rgba(0, 0, 0, 0.06)";
            ColorFillTertiary = "rgba(0, 0, 0, 0.04)";
            ColorFillQuaternary = "rgba(0, 0, 0, 0.02)";
            ColorBgLayout = "#f5f5f5";
            ColorBgContainer = "#ffffff";
            ColorBgElevated = "#ffffff";
            ColorBgSpotlight = "rgba(0, 0, 0, 0.85)";
            ColorBgBlur = "transparent";
            ColorBorder = "#d9d9d9";
            ColorBorderSecondary = "#f0f0f0";
            ColorPrimaryBg = "#e6f4ff";
            ColorPrimaryBgHover = "#bae0ff";
            ColorPrimaryBorder = "#91caff";
            ColorPrimaryBorderHover = "#69b1ff";
            ColorPrimaryHover = "#4096ff";
            ColorPrimaryActive = "#0958d9";
            ColorPrimaryTextHover = "#4096ff";
            ColorPrimaryText = "#1677ff";
            ColorPrimaryTextActive = "#0958d9";
            ColorSuccessBg = "#f6ffed";
            ColorSuccessBgHover = "#d9f7be";
            ColorSuccessBorder = "#b7eb8f";
            ColorSuccessBorderHover = "#95de64";
            ColorSuccessHover = "#95de64";
            ColorSuccessActive = "#389e0d";
            ColorSuccessTextHover = "#73d13d";
            ColorSuccessText = "#52c41a";
            ColorSuccessTextActive = "#389e0d";
            ColorErrorBg = "#fff2f0";
            ColorErrorBgHover = "#fff1f0";
            ColorErrorBorder = "#ffccc7";
            ColorErrorBorderHover = "#ffa39e";
            ColorErrorHover = "#ff7875";
            ColorErrorActive = "#d9363e";
            ColorErrorTextHover = "#ff7875";
            ColorErrorText = "#ff4d4f";
            ColorErrorTextActive = "#d9363e";
            ColorWarningBg = "#fffbe6";
            ColorWarningBgHover = "#fff1b8";
            ColorWarningBorder = "#ffe58f";
            ColorWarningBorderHover = "#ffd666";
            ColorWarningHover = "#ffd666";
            ColorWarningActive = "#d48806";
            ColorWarningTextHover = "#ffc53d";
            ColorWarningText = "#faad14";
            ColorWarningTextActive = "#d48806";
            ColorInfoBg = "#e6f4ff";
            ColorInfoBgHover = "#bae0ff";
            ColorInfoBorder = "#91caff";
            ColorInfoBorderHover = "#69b1ff";
            ColorInfoHover = "#69b1ff";
            ColorInfoActive = "#0958d9";
            ColorInfoTextHover = "#4096ff";
            ColorInfoText = "#1677ff";
            ColorInfoTextActive = "#0958d9";
            ColorLinkHover = "#69b1ff";
            ColorLinkActive = "#0958d9";
            ColorBgMask = "rgba(0, 0, 0, 0.45)";
            ColorWhite = "#fff";
            FontSizeSM = 12;
            FontSizeLG = 16;
            FontSizeXL = 20;
            FontSizeHeading1 = 38;
            FontSizeHeading2 = 30;
            FontSizeHeading3 = 24;
            FontSizeHeading4 = 20;
            FontSizeHeading5 = 16;
            LineHeight = 1.5714285714285714;
            LineHeightLG = 1.5;
            LineHeightSM = 1.6666666666666667;
            LineHeightHeading1 = 1.2105263157894737;
            LineHeightHeading2 = 1.2666666666666666;
            LineHeightHeading3 = 1.3333333333333333;
            LineHeightHeading4 = 1.4;
            LineHeightHeading5 = 1.5;
            SizeXXL = 48;
            SizeXL = 32;
            SizeLG = 24;
            SizeMD = 20;
            SizeMS = 16;
            Size = 16;
            SizeSM = 12;
            SizeXS = 8;
            SizeXXS = 4;
            ControlHeightSM = 24;
            ControlHeightXS = 16;
            ControlHeightLG = 40;
            MotionDurationFast = "0.1s";
            MotionDurationMid = "0.2s";
            MotionDurationSlow = "0.3s";
            LineWidthBold = 2;
            BorderRadiusXS = 2;
            BorderRadiusSM = 4;
            BorderRadiusLG = 8;
            BorderRadiusOuter = 4;
            ColorFillContent = "rgba(0, 0, 0, 0.06)";
            ColorFillContentHover = "rgba(0, 0, 0, 0.15)";
            ColorFillAlter = "rgba(0, 0, 0, 0.02)";
            ColorBgContainerDisabled = "rgba(0, 0, 0, 0.04)";
            ColorBorderBg = "#ffffff";
            ColorSplit = "rgba(5, 5, 5, 0.06)";
            ColorTextPlaceholder = "rgba(0, 0, 0, 0.25)";
            ColorTextDisabled = "rgba(0, 0, 0, 0.25)";
            ColorTextHeading = "rgba(0, 0, 0, 0.88)";
            ColorTextLabel = "rgba(0, 0, 0, 0.65)";
            ColorTextDescription = "rgba(0, 0, 0, 0.45)";
            ColorTextLightSolid = "#fff";
            ColorHighlight = "#ff4d4f";
            ColorBgTextHover = "rgba(0, 0, 0, 0.06)";
            ColorBgTextActive = "rgba(0, 0, 0, 0.15)";
            ColorIcon = "rgba(0, 0, 0, 0.45)";
            ColorIconHover = "rgba(0, 0, 0, 0.88)";
            ColorErrorOutline = "rgba(255, 38, 5, 0.06)";
            ColorWarningOutline = "rgba(255, 215, 5, 0.1)";
            FontSizeIcon = 12;
            LineWidthFocus = 4;
            ControlOutlineWidth = 2;
            ControlInteractiveSize = 16;
            ControlItemBgHover = "rgba(0, 0, 0, 0.04)";
            ControlItemBgActive = "#e6f4ff";
            ControlItemBgActiveHover = "#bae0ff";
            ControlItemBgActiveDisabled = "rgba(0, 0, 0, 0.15)";
            ControlTmpOutline = "rgba(0, 0, 0, 0.02)";
            ControlOutline = "rgba(5, 145, 255, 0.1)";
            FontWeightStrong = 600;
            OpacityLoading = 0.65f;
            LinkDecoration = "none";
            LinkHoverDecoration = "none";
            LinkFocusDecoration = "none";
            ControlPaddingHorizontal = 12;
            ControlPaddingHorizontalSM = 8;
            PaddingXXS = 4;
            PaddingXS = 8;
            PaddingSM = 12;
            Padding = 16;
            PaddingMD = 20;
            PaddingLG = 24;
            PaddingXL = 32;
            PaddingContentHorizontalLG = 24;
            PaddingContentVerticalLG = 16;
            PaddingContentHorizontal = 16;
            PaddingContentVertical = 12;
            PaddingContentHorizontalSM = 16;
            PaddingContentVerticalSM = 8;
            MarginXXS = 4;
            MarginXS = 8;
            MarginSM = 12;
            Margin = 16;
            MarginMD = 20;
            MarginLG = 24;
            MarginXL = 32;
            MarginXXL = 48;
            BoxShadow = "\n      0 6px 16px 0 rgba(0, 0, 0, 0.08),\n      0 3px 6px -4px rgba(0, 0, 0, 0.12),\n      0 9px 28px 8px rgba(0, 0, 0, 0.05)\n    ";
            BoxShadowSecondary = "\n      0 6px 16px 0 rgba(0, 0, 0, 0.08),\n      0 3px 6px -4px rgba(0, 0, 0, 0.12),\n      0 9px 28px 8px rgba(0, 0, 0, 0.05)\n    ";
            BoxShadowTertiary = "\n      0 1px 2px 0 rgba(0, 0, 0, 0.03),\n      0 1px 6px -1px rgba(0, 0, 0, 0.02),\n      0 2px 4px 0 rgba(0, 0, 0, 0.02)\n    ";
            ScreenXS = 480;
            ScreenXSMin = 480;
            ScreenXSMax = 575;
            ScreenSM = 576;
            ScreenSMMin = 576;
            ScreenSMMax = 767;
            ScreenMD = 768;
            ScreenMDMin = 768;
            ScreenMDMax = 991;
            ScreenLG = 992;
            ScreenLGMin = 992;
            ScreenLGMax = 1199;
            ScreenXL = 1200;
            ScreenXLMin = 1200;
            ScreenXLMax = 1599;
            ScreenXXL = 1600;
            ScreenXXLMin = 1600;
            BoxShadowPopoverArrow = "2px 2px 5px rgba(0, 0, 0, 0.05)";
            BoxShadowCard = "\n      0 1px 2px -2px rgba(0, 0, 0, 0.16),\n      0 3px 6px 0 rgba(0, 0, 0, 0.12),\n      0 5px 12px 4px rgba(0, 0, 0, 0.09)\n    ";
            BoxShadowDrawerRight = "\n      -6px 0 16px 0 rgba(0, 0, 0, 0.08),\n      -3px 0 6px -4px rgba(0, 0, 0, 0.12),\n      -9px 0 28px 8px rgba(0, 0, 0, 0.05)\n    ";
            BoxShadowDrawerLeft = "\n      6px 0 16px 0 rgba(0, 0, 0, 0.08),\n      3px 0 6px -4px rgba(0, 0, 0, 0.12),\n      9px 0 28px 8px rgba(0, 0, 0, 0.05)\n    ";
            BoxShadowDrawerUp = "\n      0 6px 16px 0 rgba(0, 0, 0, 0.08),\n      0 3px 6px -4px rgba(0, 0, 0, 0.12),\n      0 9px 28px 8px rgba(0, 0, 0, 0.05)\n    ";
            BoxShadowDrawerDown = "\n      0 -6px 16px 0 rgba(0, 0, 0, 0.08),\n      0 -3px 6px -4px rgba(0, 0, 0, 0.12),\n      0 -9px 28px 8px rgba(0, 0, 0, 0.05)\n    ";
            BoxShadowTabsOverflowLeft = "inset 10px 0 8px -8px rgba(0, 0, 0, 0.08)";
            BoxShadowTabsOverflowRight = "inset -10px 0 8px -8px rgba(0, 0, 0, 0.08)";
            BoxShadowTabsOverflowTop = "inset 0 10px 8px -8px rgba(0, 0, 0, 0.08)";
            BoxShadowTabsOverflowBottom = "inset 0 -10px 8px -8px rgba(0, 0, 0, 0.08)";
        }

        public TokenHash GetTokenHash(string version = "5.11.4", bool hashed = true)
        {
            if (_tokenHash == null)
            {
                var hashFlag = hashed ? "true" : "";
                var salt = $"{version}-{hashFlag}";
                var tokenKey = TokenToKey(_tokens, salt);
                var hashId = $"{HashPrefix}-{Hash(tokenKey)}";
                _tokenHash = new TokenHash(tokenKey, hashId);
            }

            return _tokenHash;
        }

        public string GetHashId()
        {
            return GetTokenHash().HashId;
        }

        public string TokenToKey(Dictionary<string, object> token, string salt)
        {
            return Hash($"{salt}_{FlattenToken(token)}");
        }

        private string FlattenToken(Dictionary<string, object> token)
        {
            var sb = new StringBuilder();
            foreach (var item in token)
            {
                sb.Append(item.Key);
                if (item.Value is bool)
                {
                    sb.Append(item.Value.ToString().ToLower());
                }
                else
                {
                    sb.Append(item.Value);
                }
            }
            return sb.ToString();
        }

        private string Hash(string str)
        {
            long h = 0;
            int k = 0;
            var i = 0;
            var len = str.Length;
            for (; len >= 4; ++i, len -= 4)
            {
                k = (str.CharCodeAt(i) & 0xff) |
                    ((str.CharCodeAt(++i) & 0xff) << 8) |
                    ((str.CharCodeAt(++i) & 0xff) << 16) |
                    ((str.CharCodeAt(++i) & 0xff) << 24);

                k = (k & 0xffff) * 0x5bd1e995 + (((k >>> 16) * 0xe995) << 16);
                k ^= k >>> 24;
                h = ((k & 0xffff) * 0x5bd1e995 + (((k >>> 16) * 0xe995) << 16)) ^
                    ((h & 0xffff) * 0x5bd1e995 + (((h >>> 16) * 0xe995) << 16));
            }
            switch (len)
            {
                case 3:
                    h ^= (str.CharCodeAt(i + 2) & 0xff) << 16;
                    goto case 2;
                case 2:
                    h ^= (str.CharCodeAt(i + 1) & 0xff) << 8;
                    goto case 1;
                case 1:
                    h ^= str.CharCodeAt(i) & 0xff;
                    h = ((h & 0xffff) * 0x5bd1e995) + (((int)(h >>> 16) * 0xe995) << 16);
                    break;
            }
            h ^= (int)h >>> 13;
            h = ((h & 0xffff) * 0x5bd1e995) + (((int)(h >>> 16) * 0xe995) << 16);
            var val = ((int)(h ^ ((int)h >>> 15)));
            return Convert.ToInt64(Convert.ToString((val >>> 0), toBase: 2), 2).ToString(36);
        }
    }

    public static class StringEntensions
    {
        public static int CharCodeAt(this string str, int index)
        {
            return str[index];
        }
    }

    public static class IntEntensions
    {
        private const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static string ToString(this long value, int radix = 36)
        {
            var clistarr = CharList.ToCharArray();
            var result = new Stack<char>();
            while (value != 0)
            {
                result.Push(clistarr[value % radix]);
                value /= radix;
            }
            return new string(result.ToArray());
        }
    }
}
