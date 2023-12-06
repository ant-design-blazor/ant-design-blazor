using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class AlertToken : TokenWithCommonCls
    {
        public string DefaultPadding { get; set; }

        public string WithDescriptionPadding { get; set; }

        public int WithDescriptionIconSize { get; set; }

    }

    public partial class AlertToken
    {
    }

    public partial class Alert
    {
        public CSSObject GenAlertTypeStyle(string bgColor, string borderColor, string iconColor, AlertToken token, string alertCls)
        {
            return new CSSObject()
            {
                BackgroundColor = bgColor,
                Border = @$"{token.LineWidth}px {token.LineType} {borderColor}",
                [$"{alertCls}-icon"] = new CSSObject()
                {
                    Color = iconColor,
                },
            };
        }

        public CSSObject GenBaseStyle(AlertToken token)
        {
            var componentCls = token.ComponentCls;
            var duration = token.MotionDurationSlow;
            var marginXS = token.MarginXS;
            var marginSM = token.MarginSM;
            var fontSize = token.FontSize;
            var fontSizeLG = token.FontSizeLG;
            var lineHeight = token.LineHeight;
            var borderRadius = token.BorderRadiusLG;
            var motionEaseInOutCirc = token.MotionEaseInOutCirc;
            var withDescriptionIconSize = token.WithDescriptionIconSize;
            var colorText = token.ColorText;
            var colorTextHeading = token.ColorTextHeading;
            var withDescriptionPadding = token.WithDescriptionPadding;
            var defaultPadding = token.DefaultPadding;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "flex",
                    AlignItems = "center",
                    Padding = defaultPadding,
                    WordWrap = "break-word",
                    BorderRadius = borderRadius,
                    [$"&{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Flex = 1,
                        MinWidth = 0,
                    },
                    [$"{componentCls}-icon"] = new CSSObject()
                    {
                        MarginInlineEnd = marginXS,
                        LineHeight = 0,
                    },
                    ["&-description"] = new CSSObject()
                    {
                        Display = "none",
                        FontSize = fontSize,
                        LineHeight = lineHeight,
                    },
                    ["&-message"] = new CSSObject()
                    {
                        Color = colorTextHeading,
                    },
                    [$"&{componentCls}-motion-leave"] = new CSSObject()
                    {
                        Overflow = "hidden",
                        Opacity = 1,
                        Transition = @$"max-height {duration} {motionEaseInOutCirc}, opacity {duration} {motionEaseInOutCirc},
        padding-top {duration} {motionEaseInOutCirc}, padding-bottom {duration} {motionEaseInOutCirc},
        margin-bottom {duration} {motionEaseInOutCirc}",
                    },
                    [$"&{componentCls}-motion-leave-active"] = new CSSObject()
                    {
                        MaxHeight = 0,
                        MarginBottom = "0 !important",
                        PaddingTop = 0,
                        PaddingBottom = 0,
                        Opacity = 0,
                    },
                },
                [$"{componentCls}-with-description"] = new CSSObject()
                {
                    AlignItems = "flex-start",
                    Padding = withDescriptionPadding,
                    [$"{componentCls}-icon"] = new CSSObject()
                    {
                        MarginInlineEnd = marginSM,
                        FontSize = withDescriptionIconSize,
                        LineHeight = 0,
                    },
                    [$"{componentCls}-message"] = new CSSObject()
                    {
                        Display = "block",
                        MarginBottom = marginXS,
                        Color = colorTextHeading,
                        FontSize = fontSizeLG,
                    },
                    [$"{componentCls}-description"] = new CSSObject()
                    {
                        Display = "block",
                        Color = colorText,
                    },
                },
                [$"{componentCls}-banner"] = new CSSObject()
                {
                    MarginBottom = 0,
                    Border = "0 !important",
                    BorderRadius = 0,
                },
            };
        }

        public CSSObject GenTypeStyle(AlertToken token)
        {
            var componentCls = token.ComponentCls;
            var colorSuccess = token.ColorSuccess;
            var colorSuccessBorder = token.ColorSuccessBorder;
            var colorSuccessBg = token.ColorSuccessBg;
            var colorWarning = token.ColorWarning;
            var colorWarningBorder = token.ColorWarningBorder;
            var colorWarningBg = token.ColorWarningBg;
            var colorError = token.ColorError;
            var colorErrorBorder = token.ColorErrorBorder;
            var colorErrorBg = token.ColorErrorBg;
            var colorInfo = token.ColorInfo;
            var colorInfoBorder = token.ColorInfoBorder;
            var colorInfoBg = token.ColorInfoBg;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-success"] = GenAlertTypeStyle(colorSuccessBg, colorSuccessBorder, colorSuccess, token, componentCls),
                    ["&-info"] = GenAlertTypeStyle(colorInfoBg, colorInfoBorder, colorInfo, token, componentCls),
                    ["&-warning"] = GenAlertTypeStyle(colorWarningBg, colorWarningBorder, colorWarning, token, componentCls),
                    ["&-error"] = new CSSObject()
                    {
                        ["..."] = GenAlertTypeStyle(colorErrorBg, colorErrorBorder, colorError, token, componentCls),
                        [$"{componentCls}-description > pre"] = new CSSObject()
                        {
                            Margin = 0,
                            Padding = 0,
                        },
                    },
                },
            };
        }

        public CSSObject GenActionStyle(AlertToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var motionDurationMid = token.MotionDurationMid;
            var marginXS = token.MarginXS;
            var fontSizeIcon = token.FontSizeIcon;
            var colorIcon = token.ColorIcon;
            var colorIconHover = token.ColorIconHover;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-action"] = new CSSObject()
                    {
                        MarginInlineStart = marginXS,
                    },
                    [$"{componentCls}-close-icon"] = new CSSObject()
                    {
                        MarginInlineStart = marginXS,
                        Padding = 0,
                        Overflow = "hidden",
                        FontSize = fontSizeIcon,
                        LineHeight = @$"{fontSizeIcon}px",
                        BackgroundColor = "transparent",
                        Border = "none",
                        Outline = "none",
                        Cursor = "pointer",
                        [$"{iconCls}-close"] = new CSSObject()
                        {
                            Color = colorIcon,
                            Transition = @$"color {motionDurationMid}",
                            ["&:hover"] = new CSSObject()
                            {
                                Color = colorIconHover,
                            },
                        },
                    },
                    ["&-close-text"] = new CSSObject()
                    {
                        Color = colorIcon,
                        Transition = @$"color {motionDurationMid}",
                        ["&:hover"] = new CSSObject()
                        {
                            Color = colorIconHover,
                        },
                    },
                },
            };
        }

        public CSSInterpolation GenAlertStyle(AlertToken token)
        {
            return new CSSInterpolation[]
            {
                GenBaseStyle(token),
                GenTypeStyle(token),
                GenActionStyle(token)
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Alert",
                (token) =>
                {
                    return new CSSInterpolation[] { GenAlertStyle(token) };
                },
                (token) =>
                {
                    var paddingHorizontal = 12;
                    return new AlertToken()
                    {
                        WithDescriptionIconSize = token.FontSizeHeading3,
                        DefaultPadding = @$"{token.PaddingContentVerticalSM}px {paddingHorizontal}px",
                        WithDescriptionPadding = @$"{token.PaddingMD}px {token.PaddingContentHorizontalLG}px",
                    };
                });
        }

    }

}