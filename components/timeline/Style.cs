using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TimelineToken
    {
    }

    public partial class TimelineToken : TokenWithCommonCls
    {
        public int TimeLineItemPaddingBottom { get; set; }

        public int TimeLineItemHeadSize { get; set; }

        public int TimeLineItemCustomHeadPaddingVertical { get; set; }

        public int TimeLineItemTailWidth { get; set; }

        public int TimeLinePaddingInlineEnd { get; set; }

        public int TimeLineHeadBorderWidth { get; set; }

    }

    public partial class Timeline
    {
        public Unknown_1 GenTimelineStyle(Unknown_3 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_4()
            {
                [componentCls] = new Unknown_5()
                {
                    ["..."] = ResetComponent(token),
                    Margin = 0,
                    Padding = 0,
                    ListStyle = "none",
                    [$"{componentCls}-item"] = new Unknown_6()
                    {
                        Position = "relative",
                        Margin = 0,
                        PaddingBottom = token.TimeLineItemPaddingBottom,
                        FontSize = token.FontSize,
                        ListStyle = "none",
                        ["&-tail"] = new Unknown_7()
                        {
                            Position = "absolute",
                            InsetBlockStart = token.TimeLineItemHeadSize,
                            InsetInlineStart = (token.TimeLineItemHeadSize - token.TimeLineItemTailWidth) / 2,
                            Height = @$"calc(100% - {token.TimeLineItemHeadSize}px)",
                            BorderInlineStart = @$"{token.TimeLineItemTailWidth}px {token.LineType} {token.ColorSplit}",
                        },
                        ["&-pending"] = new Unknown_8()
                        {
                            [$"{componentCls}-item-head"] = new Unknown_9()
                            {
                                FontSize = token.FontSizeSM,
                                BackgroundColor = "transparent",
                            },
                            [$"{componentCls}-item-tail"] = new Unknown_10()
                            {
                                Display = "none",
                            },
                        },
                        ["&-head"] = new Unknown_11()
                        {
                            Position = "absolute",
                            Width = token.TimeLineItemHeadSize,
                            Height = token.TimeLineItemHeadSize,
                            BackgroundColor = token.ColorBgContainer,
                            Border = @$"{token.TimeLineHeadBorderWidth}px {token.LineType} transparent",
                            BorderRadius = "50%",
                            ["&-blue"] = new Unknown_12()
                            {
                                Color = token.ColorPrimary,
                                BorderColor = token.ColorPrimary,
                            },
                            ["&-red"] = new Unknown_13()
                            {
                                Color = token.ColorError,
                                BorderColor = token.ColorError,
                            },
                            ["&-green"] = new Unknown_14()
                            {
                                Color = token.ColorSuccess,
                                BorderColor = token.ColorSuccess,
                            },
                            ["&-gray"] = new Unknown_15()
                            {
                                Color = token.ColorTextDisabled,
                                BorderColor = token.ColorTextDisabled,
                            },
                        },
                        ["&-head-custom"] = new Unknown_16()
                        {
                            Position = "absolute",
                            InsetBlockStart = token.TimeLineItemHeadSize / 2,
                            InsetInlineStart = token.TimeLineItemHeadSize / 2,
                            Width = "auto",
                            Height = "auto",
                            MarginBlockStart = 0,
                            PaddingBlock = token.TimeLineItemCustomHeadPaddingVertical,
                            LineHeight = 1,
                            TextAlign = "center",
                            Border = 0,
                            BorderRadius = 0,
                            Transform = @$"translate(-50%, -50%)",
                        },
                        ["&-content"] = new Unknown_17()
                        {
                            Position = "relative",
                            InsetBlockStart = -(token.FontSize * token.LineHeight - token.FontSize) + token.LineWidth,
                            MarginInlineStart = token.Margin + token.TimeLineItemHeadSize,
                            MarginInlineEnd = 0,
                            MarginBlockStart = 0,
                            MarginBlockEnd = 0,
                            WordBreak = "break-word",
                        },
                        ["&-last"] = new Unknown_18()
                        {
                            [$"> {componentCls}-item-tail"] = new Unknown_19()
                            {
                                Display = "none",
                            },
                            [$"> {componentCls}-item-content"] = new Unknown_20()
                            {
                                MinHeight = token.ControlHeightLG * 1.2,
                            },
                        },
                    },
                    [$"&{componentCls}-alternate,&{componentCls}-right,&{componentCls}-label"] = new Unknown_21()
                    {
                        [$"{componentCls}-item"] = new Unknown_22()
                        {
                            ["&-tail, &-head, &-head-custom"] = new Unknown_23()
                            {
                                InsetInlineStart = "50%",
                            },
                            ["&-head"] = new Unknown_24()
                            {
                                MarginInlineStart = @$"-{token.MarginXXS}px",
                                ["&-custom"] = new Unknown_25()
                                {
                                    MarginInlineStart = token.TimeLineItemTailWidth / 2,
                                },
                            },
                            ["&-left"] = new Unknown_26()
                            {
                                [$"{componentCls}-item-content"] = new Unknown_27()
                                {
                                    InsetInlineStart = @$"calc(50% - {token.MarginXXS}px)",
                                    Width = @$"calc(50% - {token.MarginSM}px)",
                                    TextAlign = "start",
                                },
                            },
                            ["&-right"] = new Unknown_28()
                            {
                                [$"{componentCls}-item-content"] = new Unknown_29()
                                {
                                    Width = @$"calc(50% - {token.MarginSM}px)",
                                    Margin = 0,
                                    TextAlign = "end",
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-right"] = new Unknown_30()
                    {
                        [$"{componentCls}-item-right"] = new Unknown_31()
                        {
                            [$"{componentCls}-item-tail,{componentCls}-item-head,{componentCls}-item-head-custom"] = new Unknown_32()
                            {
                                InsetInlineStart = @$"calc(100% - {
              (token.TimeLineItemHeadSize + token.TimeLineItemTailWidth) / 2
            }px)",
                            },
                            [$"{componentCls}-item-content"] = new Unknown_33()
                            {
                                Width = @$"calc(100% - {token.TimeLineItemHeadSize + token.MarginXS}px)",
                            },
                        },
                    },
                    [$"&{componentCls}-pending{componentCls}-item-last{componentCls}-item-tail"] = new Unknown_34()
                    {
                        Display = "block",
                        Height = @$"calc(100% - {token.Margin}px)",
                        BorderInlineStart = @$"{token.TimeLineItemTailWidth}px dotted {token.ColorSplit}",
                    },
                    [$"&{componentCls}-reverse{componentCls}-item-last{componentCls}-item-tail"] = new Unknown_35()
                    {
                        Display = "none",
                    },
                    [$"&{componentCls}-reverse {componentCls}-item-pending"] = new Unknown_36()
                    {
                        [$"{componentCls}-item-tail"] = new Unknown_37()
                        {
                            InsetBlockStart = token.Margin,
                            Display = "block",
                            Height = @$"calc(100% - {token.Margin}px)",
                            BorderInlineStart = @$"{token.TimeLineItemTailWidth}px dotted {token.ColorSplit}",
                        },
                        [$"{componentCls}-item-content"] = new Unknown_38()
                        {
                            MinHeight = token.ControlHeightLG * 1.2,
                        },
                    },
                    [$"&{componentCls}-label"] = new Unknown_39()
                    {
                        [$"{componentCls}-item-label"] = new Unknown_40()
                        {
                            Position = "absolute",
                            InsetBlockStart = -(token.FontSize * token.LineHeight - token.FontSize) + token.TimeLineItemTailWidth,
                            Width = @$"calc(50% - {token.MarginSM}px)",
                            TextAlign = "end",
                        },
                        [$"{componentCls}-item-right"] = new Unknown_41()
                        {
                            [$"{componentCls}-item-label"] = new Unknown_42()
                            {
                                InsetInlineStart = @$"calc(50% + {token.MarginSM}px)",
                                Width = @$"calc(50% - {token.MarginSM}px)",
                                TextAlign = "start",
                            },
                        },
                    },
                    ["&-rtl"] = new Unknown_43()
                    {
                        Direction = "rtl",
                        [$"{componentCls}-item-head-custom"] = new Unknown_44()
                        {
                            Transform = @$"translate(50%, -50%)",
                        },
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_45 token)
        {
            var timeLineToken = MergeToken(
                token,
                new Unknown_46()
                {
                    TimeLineItemPaddingBottom = token.Padding * 1.25,
                    TimeLineItemHeadSize = 10,
                    TimeLineItemCustomHeadPaddingVertical = token.PaddingXXS,
                    TimeLinePaddingInlineEnd = 2,
                    TimeLineItemTailWidth = token.LineWidthBold,
                    TimeLineHeadBorderWidth = token.Wireframe ? token.LineWidthBold : token.LineWidth * 3,
                });
            return new Unknown_47 { GenTimelineStyle(timeLineToken) };
        }

    }

}