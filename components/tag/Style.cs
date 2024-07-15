using System;
using CssInCSharp;
using CssInCSharp.Colors;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class TagToken
    {
        public string DefaultBg
        {
            get => (string)_tokens["defaultBg"];
            set => _tokens["defaultBg"] = value;
        }

        public string DefaultColor
        {
            get => (string)_tokens["defaultColor"];
            set => _tokens["defaultColor"] = value;
        }

    }

    public partial class TagToken : TokenWithCommonCls
    {
        public double TagFontSize
        {
            get => (double)_tokens["tagFontSize"];
            set => _tokens["tagFontSize"] = value;
        }

        public string TagLineHeight
        {
            get => (string)_tokens["tagLineHeight"];
            set => _tokens["tagLineHeight"] = value;
        }

        public double TagIconSize
        {
            get => (double)_tokens["tagIconSize"];
            set => _tokens["tagIconSize"] = value;
        }

        public double TagPaddingHorizontal
        {
            get => (double)_tokens["tagPaddingHorizontal"];
            set => _tokens["tagPaddingHorizontal"] = value;
        }

        public string TagBorderlessBg
        {
            get => (string)_tokens["tagBorderlessBg"];
            set => _tokens["tagBorderlessBg"] = value;
        }

    }

    public partial class TagStyle
    {
        public static CSSObject GenBaseStyle(TagToken token)
        {
            var paddingXXS = token.PaddingXXS;
            var lineWidth = token.LineWidth;
            var tagPaddingHorizontal = token.TagPaddingHorizontal;
            var componentCls = token.ComponentCls;
            var paddingInline = tagPaddingHorizontal - lineWidth;
            var iconMarginInline = paddingXXS - lineWidth;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    Height = "auto",
                    MarginInlineEnd = token.MarginXS,
                    PaddingInline = paddingInline,
                    FontSize = token.TagFontSize,
                    LineHeight = token.TagLineHeight,
                    WhiteSpace = "nowrap",
                    Background = token.DefaultBg,
                    Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                    BorderRadius = token.BorderRadiusSM,
                    Opacity = 1,
                    Transition = @$"all {token.MotionDurationMid}",
                    TextAlign = "start",
                    Position = "relative",
                    [$"&{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    ["&, a, a:hover"] = new CSSObject()
                    {
                        Color = token.DefaultColor,
                    },
                    [$"{componentCls}-close-icon"] = new CSSObject()
                    {
                        MarginInlineStart = iconMarginInline,
                        Color = token.ColorTextDescription,
                        FontSize = token.TagIconSize,
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationMid}",
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.ColorTextHeading,
                        },
                    },
                    [$"&{componentCls}-has-color"] = new CSSObject()
                    {
                        BorderColor = "transparent",
                        [$"&, a, a:hover, {token.IconCls}-close, {token.IconCls}-close:hover"] = new CSSObject()
                        {
                            Color = token.ColorTextLightSolid,
                        },
                    },
                    ["&-checkable"] = new CSSObject()
                    {
                        BackgroundColor = "transparent",
                        BorderColor = "transparent",
                        Cursor = "pointer",
                        [$"&:not({componentCls}-checkable-checked):hover"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                            BackgroundColor = token.ColorFillSecondary,
                        },
                        ["&:active, &-checked"] = new CSSObject()
                        {
                            Color = token.ColorTextLightSolid,
                        },
                        ["&-checked"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorPrimary,
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = token.ColorPrimaryHover,
                            },
                        },
                        ["&:active"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorPrimaryActive,
                        },
                    },
                    ["&-hidden"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    [$"> {token.IconCls} + span, > span + {token.IconCls}"] = new CSSObject()
                    {
                        MarginInlineStart = paddingInline,
                    },
                },
                [$"{componentCls}-borderless"] = new CSSObject()
                {
                    BorderColor = "transparent",
                    Background = token.TagBorderlessBg,
                },
            };
        }

        public static TagToken PrepareToken(TagToken token)
        {
            var lineWidth = token.LineWidth;
            var fontSizeIcon = token.FontSizeIcon;
            var tagFontSize = token.FontSizeSM;
            var tagLineHeight = @$"{token.LineHeightSM * tagFontSize}px";
            var tagToken = MergeToken(
                token,
                new TagToken()
                {
                    TagFontSize = tagFontSize,
                    TagLineHeight = tagLineHeight,
                    TagIconSize = fontSizeIcon - 2 * lineWidth,
                    TagPaddingHorizontal = 8,
                    TagBorderlessBg = token.ColorFillTertiary,
                });
            return tagToken;
        }

        public static TagToken PrepareCommonToken(GlobalToken token)
        {
            return new TagToken()
            {
                DefaultBg = new TinyColor(token.ColorFillQuaternary).OnBackground(token.ColorBgContainer).ToHexString(),
                DefaultColor = token.ColorText,
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Tag",
                (token) =>
                {
                    var tagToken = PrepareToken(token);
                    return GenBaseStyle(tagToken);
                },
                PrepareCommonToken);
        }

    }

}
