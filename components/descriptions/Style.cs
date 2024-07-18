using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class DescriptionsToken
    {
        public string LabelBg
        {
            get => (string)_tokens["labelBg"];
            set => _tokens["labelBg"] = value;
        }

        public string TitleColor
        {
            get => (string)_tokens["titleColor"];
            set => _tokens["titleColor"] = value;
        }

        public double TitleMarginBottom
        {
            get => (double)_tokens["titleMarginBottom"];
            set => _tokens["titleMarginBottom"] = value;
        }

        public double ItemPaddingBottom
        {
            get => (double)_tokens["itemPaddingBottom"];
            set => _tokens["itemPaddingBottom"] = value;
        }

        public double ColonMarginRight
        {
            get => (double)_tokens["colonMarginRight"];
            set => _tokens["colonMarginRight"] = value;
        }

        public double ColonMarginLeft
        {
            get => (double)_tokens["colonMarginLeft"];
            set => _tokens["colonMarginLeft"] = value;
        }

        public string ContentColor
        {
            get => (string)_tokens["contentColor"];
            set => _tokens["contentColor"] = value;
        }

        public string ExtraColor
        {
            get => (string)_tokens["extraColor"];
            set => _tokens["extraColor"] = value;
        }

    }

    public partial class DescriptionsToken : TokenWithCommonCls
    {
    }

    public partial class DescriptionsStyle
    {
        public static CSSObject GenBorderedStyle(DescriptionsToken token)
        {
            var componentCls = token.ComponentCls;
            var labelBg = token.LabelBg;
            return new CSSObject()
            {
                [$"&{componentCls}-bordered"] = new CSSObject()
                {
                    [$"> {componentCls}-view"] = new CSSObject()
                    {
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                        ["> table"] = new CSSObject()
                        {
                            TableLayout = "auto",
                            BorderCollapse = "collapse",
                        },
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            BorderBottom = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                            ["&:last-child"] = new CSSObject()
                            {
                                BorderBottom = "none",
                            },
                            [$"> {componentCls}-item-label, > {componentCls}-item-content"] = new CSSObject()
                            {
                                Padding = @$"{token.Padding}px {token.PaddingLG}px",
                                BorderInlineEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                                ["&:last-child"] = new CSSObject()
                                {
                                    BorderInlineEnd = "none",
                                },
                            },
                            [$"> {componentCls}-item-label"] = new CSSObject()
                            {
                                Color = token.ColorTextSecondary,
                                BackgroundColor = labelBg,
                                ["&::after"] = new CSSObject()
                                {
                                    Display = "none",
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-middle"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            [$"> {componentCls}-item-label, > {componentCls}-item-content"] = new CSSObject()
                            {
                                Padding = @$"{token.PaddingSM}px {token.PaddingLG}px",
                            },
                        },
                    },
                    [$"&{componentCls}-small"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            [$"> {componentCls}-item-label, > {componentCls}-item-content"] = new CSSObject()
                            {
                                Padding = @$"{token.PaddingXS}px {token.Padding}px",
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenDescriptionStyles(DescriptionsToken token)
        {
            var componentCls = token.ComponentCls;
            var extraColor = token.ExtraColor;
            var itemPaddingBottom = token.ItemPaddingBottom;
            var colonMarginRight = token.ColonMarginRight;
            var colonMarginLeft = token.ColonMarginLeft;
            var titleMarginBottom = token.TitleMarginBottom;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenBorderedStyle(token),
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"{componentCls}-header"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignItems = "center",
                        MarginBottom = titleMarginBottom,
                    },
                    [$"{componentCls}-title"] = new CSSObject()
                    {
                        ["..."] = TextEllipsis,
                        Flex = "auto",
                        Color = token.TitleColor,
                        FontWeight = token.FontWeightStrong,
                        FontSize = token.FontSizeLG,
                        LineHeight = token.LineHeightLG,
                    },
                    [$"{componentCls}-extra"] = new CSSObject()
                    {
                        MarginInlineStart = "auto",
                        Color = extraColor,
                        FontSize = token.FontSize,
                    },
                    [$"{componentCls}-view"] = new CSSObject()
                    {
                        Width = "100%",
                        BorderRadius = token.BorderRadiusLG,
                        ["table"] = new CSSObject()
                        {
                            Width = "100%",
                            TableLayout = "fixed",
                        },
                    },
                    [$"{componentCls}-row"] = new CSSObject()
                    {
                        ["> th, > td"] = new CSSObject()
                        {
                            PaddingBottom = itemPaddingBottom,
                        },
                        ["&:last-child"] = new CSSObject()
                        {
                            BorderBottom = "none",
                        },
                    },
                    [$"{componentCls}-item-label"] = new CSSObject()
                    {
                        Color = token.ColorTextTertiary,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                        LineHeight = token.LineHeight,
                        TextAlign = @$"start",
                        ["&::after"] = new CSSObject()
                        {
                            Content = "\":\"",
                            Position = "relative",
                            Top = -0.5f,
                            MarginInline = @$"{colonMarginLeft}px {colonMarginRight}px",
                        },
                        [$"&{componentCls}-item-no-colon::after"] = new CSSObject()
                        {
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-item-no-label"] = new CSSObject()
                    {
                        ["&::after"] = new CSSObject()
                        {
                            Margin = 0,
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-item-content"] = new CSSObject()
                    {
                        Display = "table-cell",
                        Flex = 1,
                        Color = token.ContentColor,
                        FontSize = token.FontSize,
                        LineHeight = token.LineHeight,
                        WordBreak = "break-word",
                        OverflowWrap = "break-word",
                    },
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        PaddingBottom = 0,
                        VerticalAlign = "top",
                        ["&-container"] = new CSSObject()
                        {
                            Display = "flex",
                            [$"{componentCls}-item-label"] = new CSSObject()
                            {
                                Display = "inline-flex",
                                AlignItems = "baseline",
                            },
                            [$"{componentCls}-item-content"] = new CSSObject()
                            {
                                Display = "inline-flex",
                                AlignItems = "baseline",
                            },
                        },
                    },
                    ["&-middle"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                PaddingBottom = token.PaddingSM,
                            },
                        },
                    },
                    ["&-small"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                PaddingBottom = token.PaddingXS,
                            },
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Descriptions",
                (token) =>
                {
                    var descriptionToken = MergeToken(
                        token,
                        new DescriptionsToken()
                        {
                        });
                    return new CSSInterpolation[] { GenDescriptionStyles(descriptionToken) };
                },
                (token) =>
                {
                    return new DescriptionsToken()
                    {
                        LabelBg = token.ColorFillAlter,
                        TitleColor = token.ColorText,
                        TitleMarginBottom = token.FontSizeSM * token.LineHeightSM,
                        ItemPaddingBottom = token.Padding,
                        ColonMarginRight = token.MarginXS,
                        ColonMarginLeft = token.MarginXXS / 2,
                        ContentColor = token.ColorText,
                        ExtraColor = token.ColorText,
                    };
                });
        }

    }

}
