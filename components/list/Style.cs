using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class ListToken
    {
        public int ContentWidth { get; set; }

    }

    public partial class ListToken : TokenWithCommonCls
    {
        public string ListBorderedCls { get; set; }

        public int MinHeight { get; set; }

        public string ListItemPaddingLG { get; set; }

        public string ListItemPaddingSM { get; set; }

        public string ListItemPadding { get; set; }

    }

    public partial class List
    {
        public CSSObject GenBorderedStyle(ListToken token)
        {
            var listBorderedCls = token.ListBorderedCls;
            var componentCls = token.ComponentCls;
            var paddingLG = token.PaddingLG;
            var margin = token.Margin;
            var padding = token.Padding;
            var listItemPaddingSM = token.ListItemPaddingSM;
            var marginLG = token.MarginLG;
            var borderRadiusLG = token.BorderRadiusLG;
            return new CSSObject()
            {
                [$"{listBorderedCls}"] = new CSSObject()
                {
                    Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                    BorderRadius = borderRadiusLG,
                    [$"{componentCls}-header,{componentCls}-footer,{componentCls}-item"] = new CSSObject()
                    {
                        PaddingInline = paddingLG,
                    },
                    [$"{componentCls}-pagination"] = new CSSObject()
                    {
                        Margin = @$"{margin}px {marginLG}px",
                    },
                },
                [$"{listBorderedCls}{componentCls}-sm"] = new CSSObject()
                {
                    [$"{componentCls}-item,{componentCls}-header,{componentCls}-footer"] = new CSSObject()
                    {
                        Padding = listItemPaddingSM,
                    },
                },
                [$"{listBorderedCls}{componentCls}-lg"] = new CSSObject()
                {
                    [$"{componentCls}-item,{componentCls}-header,{componentCls}-footer"] = new CSSObject()
                    {
                        Padding = @$"{padding}px {paddingLG}px",
                    },
                },
            };
        }

        public CSSObject GenResponsiveStyle(ListToken token)
        {
            var componentCls = token.ComponentCls;
            var screenSM = token.ScreenSM;
            var screenMD = token.ScreenMD;
            var marginLG = token.MarginLG;
            var marginSM = token.MarginSM;
            var margin = token.Margin;
            return new CSSObject()
            {
                [$"@media screen and (max-width:{screenMD})"] = new CSSObject()
                {
                    [$"{componentCls}"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            [$"{componentCls}-item-action"] = new CSSObject()
                            {
                                MarginInlineStart = marginLG,
                            },
                        },
                    },
                    [$"{componentCls}-vertical"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            [$"{componentCls}-item-extra"] = new CSSObject()
                            {
                                MarginInlineStart = marginLG,
                            },
                        },
                    },
                },
                [$"@media screen and (max-width: {screenSM})"] = new CSSObject()
                {
                    [$"{componentCls}"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            FlexWrap = "wrap",
                            [$"{componentCls}-action"] = new CSSObject()
                            {
                                MarginInlineStart = marginSM,
                            },
                        },
                    },
                    [$"{componentCls}-vertical"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            FlexWrap = "wrap-reverse",
                            [$"{componentCls}-item-main"] = new CSSObject()
                            {
                                MinWidth = token.ContentWidth,
                            },
                            [$"{componentCls}-item-extra"] = new CSSObject()
                            {
                                Margin = @$"auto auto {margin}px",
                            },
                        },
                    },
                },
            };
        }

        public Unknown_1 GenBaseStyle(Unknown_3 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var controlHeight = token.ControlHeight;
            var minHeight = token.MinHeight;
            var paddingSM = token.PaddingSM;
            var marginLG = token.MarginLG;
            var padding = token.Padding;
            var listItemPadding = token.ListItemPadding;
            var colorPrimary = token.ColorPrimary;
            var listItemPaddingSM = token.ListItemPaddingSM;
            var listItemPaddingLG = token.ListItemPaddingLG;
            var paddingXS = token.PaddingXS;
            var margin = token.Margin;
            var colorText = token.ColorText;
            var colorTextDescription = token.ColorTextDescription;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineWidth = token.LineWidth;
            var alignCls = new Unknown_4()
            {
            };
            return new Unknown_5()
            {
                [$"{componentCls}"] = new Unknown_6()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    ["*"] = new Unknown_7()
                    {
                        Outline = "none",
                    },
                    [$"{componentCls}-header, {componentCls}-footer"] = new Unknown_8()
                    {
                        Background = "transparent",
                        PaddingBlock = paddingSM,
                    },
                    [$"{componentCls}-pagination"] = new Unknown_9()
                    {
                        MarginBlockStart = marginLG,
                        ["..."] = alignCls,
                        [$"{antCls}-pagination-options"] = new Unknown_10()
                        {
                            TextAlign = "start",
                        },
                    },
                    [$"{componentCls}-spin"] = new Unknown_11()
                    {
                        MinHeight = minHeight,
                        TextAlign = "center",
                    },
                    [$"{componentCls}-items"] = new Unknown_12()
                    {
                        Margin = 0,
                        Padding = 0,
                        ListStyle = "none",
                    },
                    [$"{componentCls}-item"] = new Unknown_13()
                    {
                        Display = "flex",
                        AlignItems = "center",
                        JustifyContent = "space-between",
                        Padding = listItemPadding,
                        Color = colorText,
                        [$"{componentCls}-item-meta"] = new Unknown_14()
                        {
                            Display = "flex",
                            Flex = 1,
                            AlignItems = "flex-start",
                            MaxWidth = "100%",
                            [$"{componentCls}-item-meta-avatar"] = new Unknown_15()
                            {
                                MarginInlineEnd = padding,
                            },
                            [$"{componentCls}-item-meta-content"] = new Unknown_16()
                            {
                                Flex = "1 0",
                                Width = 0,
                                Color = colorText,
                            },
                            [$"{componentCls}-item-meta-title"] = new Unknown_17()
                            {
                                Margin = @$"0 0 {token.MarginXXS}px 0",
                                Color = colorText,
                                FontSize = token.FontSize,
                                LineHeight = token.LineHeight,
                                ["> a"] = new Unknown_18()
                                {
                                    Color = colorText,
                                    Transition = @$"all {motionDurationSlow}",
                                    ["&:hover"] = new Unknown_19()
                                    {
                                        Color = colorPrimary,
                                    },
                                },
                            },
                            [$"{componentCls}-item-meta-description"] = new Unknown_20()
                            {
                                Color = colorTextDescription,
                                FontSize = token.FontSize,
                                LineHeight = token.LineHeight,
                            },
                        },
                        [$"{componentCls}-item-action"] = new Unknown_21()
                        {
                            Flex = "0 0 auto",
                            MarginInlineStart = token.MarginXXL,
                            Padding = 0,
                            FontSize = 0,
                            ListStyle = "none",
                            ["& > li"] = new Unknown_22()
                            {
                                Position = "relative",
                                Display = "inline-block",
                                Padding = @$"0 {paddingXS}px",
                                Color = colorTextDescription,
                                FontSize = token.FontSize,
                                LineHeight = token.LineHeight,
                                TextAlign = "center",
                                ["&:first-child"] = new Unknown_23()
                                {
                                    PaddingInlineStart = 0,
                                },
                            },
                            [$"{componentCls}-item-action-split"] = new Unknown_24()
                            {
                                Position = "absolute",
                                InsetBlockStart = "50%",
                                InsetInlineEnd = 0,
                                Width = lineWidth,
                                Height = Math.Ceil(token.FontSize * token.LineHeight) - token.MarginXXS * 2,
                                Transform = "translateY(-50%)",
                                BackgroundColor = token.ColorSplit,
                            },
                        },
                    },
                    [$"{componentCls}-empty"] = new Unknown_25()
                    {
                        Padding = @$"{padding}px 0",
                        Color = colorTextDescription,
                        FontSize = token.FontSizeSM,
                        TextAlign = "center",
                    },
                    [$"{componentCls}-empty-text"] = new Unknown_26()
                    {
                        Padding = padding,
                        Color = token.ColorTextDisabled,
                        FontSize = token.FontSize,
                        TextAlign = "center",
                    },
                    [$"{componentCls}-item-no-flex"] = new Unknown_27()
                    {
                        Display = "block",
                    },
                },
                [$"{componentCls}-grid {antCls}-col > {componentCls}-item"] = new Unknown_28()
                {
                    Display = "block",
                    MaxWidth = "100%",
                    MarginBlockEnd = margin,
                    PaddingBlock = 0,
                    BorderBlockEnd = "none",
                },
                [$"{componentCls}-vertical {componentCls}-item"] = new Unknown_29()
                {
                    AlignItems = "initial",
                    [$"{componentCls}-item-main"] = new Unknown_30()
                    {
                        Display = "block",
                        Flex = 1,
                    },
                    [$"{componentCls}-item-extra"] = new Unknown_31()
                    {
                        MarginInlineStart = marginLG,
                    },
                    [$"{componentCls}-item-meta"] = new Unknown_32()
                    {
                        MarginBlockEnd = padding,
                        [$"{componentCls}-item-meta-title"] = new Unknown_33()
                        {
                            MarginBlockStart = 0,
                            MarginBlockEnd = paddingSM,
                            Color = colorText,
                            FontSize = token.FontSizeLG,
                            LineHeight = token.LineHeightLG,
                        },
                    },
                    [$"{componentCls}-item-action"] = new Unknown_34()
                    {
                        MarginBlockStart = padding,
                        MarginInlineStart = "auto",
                        ["> li"] = new Unknown_35()
                        {
                            Padding = @$"0 {padding}px",
                            ["&:first-child"] = new Unknown_36()
                            {
                                PaddingInlineStart = 0,
                            },
                        },
                    },
                },
                [$"{componentCls}-split {componentCls}-item"] = new Unknown_37()
                {
                    BorderBlockEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                    ["&:last-child"] = new Unknown_38()
                    {
                        BorderBlockEnd = "none",
                    },
                },
                [$"{componentCls}-split {componentCls}-header"] = new Unknown_39()
                {
                    BorderBlockEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                },
                [$"{componentCls}-split{componentCls}-empty {componentCls}-footer"] = new Unknown_40()
                {
                    BorderTop = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                },
                [$"{componentCls}-loading {componentCls}-spin-nested-loading"] = new Unknown_41()
                {
                    MinHeight = controlHeight,
                },
                [$"{componentCls}-split{componentCls}-something-after-last-item {antCls}-spin-container > {componentCls}-items > {componentCls}-item:last-child"] = new Unknown_42()
                {
                    BorderBlockEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                },
                [$"{componentCls}-lg {componentCls}-item"] = new Unknown_43()
                {
                    Padding = listItemPaddingLG,
                },
                [$"{componentCls}-sm {componentCls}-item"] = new Unknown_44()
                {
                    Padding = listItemPaddingSM,
                },
                [$"{componentCls}:not({componentCls}-vertical)"] = new Unknown_45()
                {
                    [$"{componentCls}-item-no-flex"] = new Unknown_46()
                    {
                        [$"{componentCls}-item-action"] = new Unknown_47()
                        {
                            Float = "right",
                        },
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_48 token)
        {
            var listToken = MergeToken(
                token,
                new Unknown_49()
                {
                    ListBorderedCls = @$"{token.ComponentCls}-bordered",
                    MinHeight = token.ControlHeightLG,
                    ListItemPadding = @$"{token.PaddingContentVertical}px 0",
                    ListItemPaddingSM = @$"{token.PaddingContentVerticalSM}px {token.PaddingContentHorizontal}px",
                    ListItemPaddingLG = @$"{token.PaddingContentVerticalLG}px {token.PaddingContentHorizontalLG}px",
                });
            return new Unknown_50
            {
                GenBaseStyle(listToken),
                GenBorderedStyle(listToken),
                GenResponsiveStyle(listToken)
            };
        }

    }

}