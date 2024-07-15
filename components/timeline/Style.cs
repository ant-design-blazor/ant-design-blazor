using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class TimelineToken
    {
        public string TailColor
        {
            get => (string)_tokens["tailColor"];
            set => _tokens["tailColor"] = value;
        }

        public double TailWidth
        {
            get => (double)_tokens["tailWidth"];
            set => _tokens["tailWidth"] = value;
        }

        public double DotBorderWidth
        {
            get => (double)_tokens["dotBorderWidth"];
            set => _tokens["dotBorderWidth"] = value;
        }

        public string DotBg
        {
            get => (string)_tokens["dotBg"];
            set => _tokens["dotBg"] = value;
        }

        public double ItemPaddingBottom
        {
            get => (double)_tokens["itemPaddingBottom"];
            set => _tokens["itemPaddingBottom"] = value;
        }

    }

    public partial class TimelineToken : TokenWithCommonCls
    {
        public double ItemHeadSize
        {
            get => (double)_tokens["itemHeadSize"];
            set => _tokens["itemHeadSize"] = value;
        }

        public double CustomHeadPaddingVertical
        {
            get => (double)_tokens["customHeadPaddingVertical"];
            set => _tokens["customHeadPaddingVertical"] = value;
        }

        public double PaddingInlineEnd
        {
            get => (double)_tokens["paddingInlineEnd"];
            set => _tokens["paddingInlineEnd"] = value;
        }

    }

    public partial class TimelineStyle
    {
        public static CSSObject GenTimelineStyle(TimelineToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Margin = 0,
                    Padding = 0,
                    ListStyle = "none",
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Position = "relative",
                        Margin = 0,
                        PaddingBottom = token.ItemPaddingBottom,
                        FontSize = token.FontSize,
                        ListStyle = "none",
                        ["&-tail"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlockStart = token.ItemHeadSize,
                            InsetInlineStart = (token.ItemHeadSize - token.TailWidth) / 2,
                            Height = @$"calc(100% - {token.ItemHeadSize}px)",
                            BorderInlineStart = @$"{token.TailWidth}px {token.LineType} {token.TailColor}",
                        },
                        ["&-pending"] = new CSSObject()
                        {
                            [$"{componentCls}-item-head"] = new CSSObject()
                            {
                                FontSize = token.FontSizeSM,
                                BackgroundColor = "transparent",
                            },
                            [$"{componentCls}-item-tail"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        ["&-head"] = new CSSObject()
                        {
                            Position = "absolute",
                            Width = token.ItemHeadSize,
                            Height = token.ItemHeadSize,
                            BackgroundColor = token.DotBg,
                            Border = @$"{token.DotBorderWidth}px {token.LineType} transparent",
                            BorderRadius = "50%",
                            ["&-blue"] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                                BorderColor = token.ColorPrimary,
                            },
                            ["&-red"] = new CSSObject()
                            {
                                Color = token.ColorError,
                                BorderColor = token.ColorError,
                            },
                            ["&-green"] = new CSSObject()
                            {
                                Color = token.ColorSuccess,
                                BorderColor = token.ColorSuccess,
                            },
                            ["&-gray"] = new CSSObject()
                            {
                                Color = token.ColorTextDisabled,
                                BorderColor = token.ColorTextDisabled,
                            },
                        },
                        ["&-head-custom"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlockStart = token.ItemHeadSize / 2,
                            InsetInlineStart = token.ItemHeadSize / 2,
                            Width = "auto",
                            Height = "auto",
                            MarginBlockStart = 0,
                            PaddingBlock = token.CustomHeadPaddingVertical,
                            LineHeight = 1,
                            TextAlign = "center",
                            Border = 0,
                            BorderRadius = 0,
                            Transform = @$"translate(-50%, -50%)",
                        },
                        ["&-content"] = new CSSObject()
                        {
                            Position = "relative",
                            InsetBlockStart = -(token.FontSize * token.LineHeight - token.FontSize) + token.LineWidth,
                            MarginInlineStart = token.Margin + token.ItemHeadSize,
                            MarginInlineEnd = 0,
                            MarginBlockStart = 0,
                            MarginBlockEnd = 0,
                            WordBreak = "break-word",
                        },
                        ["&-last"] = new CSSObject()
                        {
                            [$"> {componentCls}-item-tail"] = new CSSObject()
                            {
                                Display = "none",
                            },
                            [$"> {componentCls}-item-content"] = new CSSObject()
                            {
                                MinHeight = token.ControlHeightLG * 1.2,
                            },
                        },
                    },
                    [$"&{componentCls}-alternate,&{componentCls}-right,&{componentCls}-label"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            ["&-tail, &-head, &-head-custom"] = new CSSObject()
                            {
                                InsetInlineStart = "50%",
                            },
                            ["&-head"] = new CSSObject()
                            {
                                MarginInlineStart = @$"-{token.MarginXXS}px",
                                ["&-custom"] = new CSSObject()
                                {
                                    MarginInlineStart = token.TailWidth / 2,
                                },
                            },
                            ["&-left"] = new CSSObject()
                            {
                                [$"{componentCls}-item-content"] = new CSSObject()
                                {
                                    InsetInlineStart = @$"calc(50% - {token.MarginXXS}px)",
                                    Width = @$"calc(50% - {token.MarginSM}px)",
                                    TextAlign = "start",
                                },
                            },
                            ["&-right"] = new CSSObject()
                            {
                                [$"{componentCls}-item-content"] = new CSSObject()
                                {
                                    Width = @$"calc(50% - {token.MarginSM}px)",
                                    Margin = 0,
                                    TextAlign = "end",
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-right"] = new CSSObject()
                    {
                        [$"{componentCls}-item-right"] = new CSSObject()
                        {
                            [$"{componentCls}-item-tail,{componentCls}-item-head,{componentCls}-item-head-custom"] = new CSSObject()
                            {
                                InsetInlineStart = @$"calc(100% - {(token.ItemHeadSize + token.TailWidth) / 2}px)",
                            },
                            [$"{componentCls}-item-content"] = new CSSObject()
                            {
                                Width = @$"calc(100% - {token.ItemHeadSize + token.MarginXS}px)",
                            },
                        },
                    },
                    [@$"&{componentCls}-pending
  {componentCls}-item-last
  {componentCls}-item-tail"] = new CSSObject()
                    {
                        Display = "block",
                        Height = @$"calc(100% - {token.Margin}px)",
                        BorderInlineStart = @$"{token.TailWidth}px dotted {token.TailColor}",
                    },
                    [@$"&{componentCls}-reverse
  {componentCls}-item-last
  {componentCls}-item-tail"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    [$"&{componentCls}-reverse {componentCls}-item-pending"] = new CSSObject()
                    {
                        [$"{componentCls}-item-tail"] = new CSSObject()
                        {
                            InsetBlockStart = token.Margin,
                            Display = "block",
                            Height = @$"calc(100% - {token.Margin}px)",
                            BorderInlineStart = @$"{token.TailWidth}px dotted {token.TailColor}",
                        },
                        [$"{componentCls}-item-content"] = new CSSObject()
                        {
                            MinHeight = token.ControlHeightLG * 1.2,
                        },
                    },
                    [$"&{componentCls}-label"] = new CSSObject()
                    {
                        [$"{componentCls}-item-label"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlockStart = -(token.FontSize * token.LineHeight - token.FontSize) + token.TailWidth,
                            Width = @$"calc(50% - {token.MarginSM}px)",
                            TextAlign = "end",
                        },
                        [$"{componentCls}-item-right"] = new CSSObject()
                        {
                            [$"{componentCls}-item-label"] = new CSSObject()
                            {
                                InsetInlineStart = @$"calc(50% + {token.MarginSM}px)",
                                Width = @$"calc(50% - {token.MarginSM}px)",
                                TextAlign = "start",
                            },
                        },
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                        [$"{componentCls}-item-head-custom"] = new CSSObject()
                        {
                            Transform = @$"translate(50%, -50%)",
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Timeline",
                (token) =>
                {
                    var timeLineToken = MergeToken(
                        token,
                        new TimelineToken()
                        {
                            ItemHeadSize = 10,
                            CustomHeadPaddingVertical = token.PaddingXXS,
                            PaddingInlineEnd = 2,
                        });
                    return new CSSInterpolation[]
                    {
                        GenTimelineStyle(timeLineToken),
                    };
                },
                (token) =>
                {
                    return new TimelineToken()
                    {
                        TailColor = token.ColorSplit,
                        TailWidth = token.LineWidthBold,
                        DotBorderWidth = token.Wireframe ? token.LineWidthBold : token.LineWidth * 3,
                        DotBg = token.ColorBgContainer,
                        ItemPaddingBottom = token.Padding * 1.25,
                    };
                });
        }

    }

}
