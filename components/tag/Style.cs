using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TagToken
    {
    }

    public partial class TagToken : TokenWithCommonCls
    {
        public int TagFontSize { get; set; }

        public React.CSSProperties TagLineHeight { get; set; }

        public string TagDefaultBg { get; set; }

        public string TagDefaultColor { get; set; }

        public int TagIconSize { get; set; }

        public int TagPaddingHorizontal { get; set; }

        public string TagBorderlessBg { get; set; }

    }

    public class CssVariableType
    {
    }

    public partial class Tag
    {
        public CSSInterpolation GenTagStatusStyle(TagToken token, 'success' | 'processing' | 'error' | 'warning' status, CssVariableType cssVariableType)
        {
            var capitalizedCssVariableType = Capitalize(cssVariableType);
            return new CSSInterpolation()
            {
                [$"{token.ComponentCls}-{status}"] = new CSSInterpolation()
                {
                    Color = token[@$"color{cssVariableType}"],
                    Background = token[@$"color{capitalizedCssVariableType}Bg"],
                    BorderColor = token[@$"color{capitalizedCssVariableType}Border"],
                },
            };
        }

        public Unknown_2 GenPresetStyle(TagToken token)
        {
            return GenPresetColor(
                token,
                (colorKey, args) => {
                    var textColor = args.TextColor;
                    var lightBorderColor = args.LightBorderColor;
                    var lightColor = args.LightColor;
                    var darkColor = args.DarkColor;
                    return new Unknown_4()
                    {
                        [$"{token.ComponentCls}-{colorKey}"] = new Unknown_5()
                        {
                            Color = textColor,
                            Background = lightColor,
                            BorderColor = lightBorderColor,
                            ["&-inverse"] = new Unknown_6()
                            {
                                Color = token.ColorTextLightSolid,
                                Background = darkColor,
                                BorderColor = darkColor,
                            },
                            [$"&{token.ComponentCls}-borderless"] = new Unknown_7()
                            {
                                BorderColor = "transparent",
                            },
                        },
                    };
                });
        }

        public CSSInterpolation GenBaseStyle(TagToken token)
        {
            var paddingXXS = token.PaddingXXS;
            var lineWidth = token.LineWidth;
            var tagPaddingHorizontal = token.TagPaddingHorizontal;
            var componentCls = token.ComponentCls;
            var paddingInline = tagPaddingHorizontal - lineWidth;
            var iconMarginInline = paddingXXS - lineWidth;
            return new CSSInterpolation()
            {
                [componentCls] = new CSSInterpolation()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    Height = "auto",
                    MarginInlineEnd = token.MarginXS,
                    PaddingInline = paddingInline,
                    FontSize = token.TagFontSize,
                    LineHeight = @$"{token.TagLineHeight}px",
                    WhiteSpace = "nowrap",
                    Background = token.TagDefaultBg,
                    Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                    BorderRadius = token.BorderRadiusSM,
                    Opacity = 1,
                    Transition = @$"all {token.MotionDurationMid}",
                    TextAlign = "start",
                    [$"&{componentCls}-rtl"] = new CSSInterpolation()
                    {
                        Direction = "rtl",
                    },
                    ["&, a, a:hover"] = new CSSInterpolation()
                    {
                        Color = token.TagDefaultColor,
                    },
                    [$"{componentCls}-close-icon"] = new CSSInterpolation()
                    {
                        MarginInlineStart = iconMarginInline,
                        Color = token.ColorTextDescription,
                        FontSize = token.TagIconSize,
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationMid}",
                        ["&:hover"] = new CSSInterpolation()
                        {
                            Color = token.ColorTextHeading,
                        },
                    },
                    [$"&{componentCls}-has-color"] = new CSSInterpolation()
                    {
                        BorderColor = "transparent",
                        [$"&, a, a:hover, {token.IconCls}-close, {token.IconCls}-close:hover"] = new CSSInterpolation()
                        {
                            Color = token.ColorTextLightSolid,
                        },
                    },
                    ["&-checkable"] = new CSSInterpolation()
                    {
                        BackgroundColor = "transparent",
                        BorderColor = "transparent",
                        Cursor = "pointer",
                        [$"&:not({componentCls}-checkable-checked):hover"] = new CSSInterpolation()
                        {
                            Color = token.ColorPrimary,
                            BackgroundColor = token.ColorFillSecondary,
                        },
                        ["&:active, &-checked"] = new CSSInterpolation()
                        {
                            Color = token.ColorTextLightSolid,
                        },
                        ["&-checked"] = new CSSInterpolation()
                        {
                            BackgroundColor = token.ColorPrimary,
                            ["&:hover"] = new CSSInterpolation()
                            {
                                BackgroundColor = token.ColorPrimaryHover,
                            },
                        },
                        ["&:active"] = new CSSInterpolation()
                        {
                            BackgroundColor = token.ColorPrimaryActive,
                        },
                    },
                    ["&-hidden"] = new CSSInterpolation()
                    {
                        Display = "none",
                    },
                    [$"> {token.IconCls} + span, > span + {token.IconCls}"] = new CSSInterpolation()
                    {
                        MarginInlineStart = paddingInline,
                    },
                },
                [$"{componentCls}-borderless"] = new CSSInterpolation()
                {
                    BorderColor = "transparent",
                    Background = token.TagBorderlessBg,
                },
            };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_8 token)
        {
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var lineWidth = token.LineWidth;
            var fontSizeIcon = token.FontSizeIcon;
            var tagHeight = Math.Round(fontSize * lineHeight);
            var tagFontSize = token.FontSizeSM;
            var tagLineHeight = tagHeight - lineWidth * 2;
            var tagDefaultBg = token.ColorFillQuaternary;
            var tagDefaultColor = token.ColorText;
            var tagToken = MergeToken(
                token,
                new Unknown_9()
                {
                    TagFontSize = tagFontSize,
                    TagLineHeight = tagLineHeight,
                    TagDefaultBg = tagDefaultBg,
                    TagDefaultColor = tagDefaultColor,
                    TagIconSize = fontSizeIcon - 2 * lineWidth,
                    TagPaddingHorizontal = 8,
                    TagBorderlessBg = token.ColorFillTertiary,
                });
            return new Unknown_10
            {
                GenBaseStyle(tagToken),
                GenPresetStyle(tagToken),
                GenTagStatusStyle(tagToken, "success", "Success"),
                GenTagStatusStyle(tagToken, "processing", "Info"),
                GenTagStatusStyle(tagToken, "error", "Error"),
                GenTagStatusStyle(tagToken, "warning", "Warning")
            };
        }

    }

}