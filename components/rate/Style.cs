using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class RateToken
    {
    }

    public partial class RateToken : TokenWithCommonCls
    {
        public string RateStarColor { get; set; }

        public int RateStarSize { get; set; }

        public CSSObject RateStarHoverScale { get; set; }

        public string DefaultColor { get; set; }

    }

    public partial class Rate
    {
        public Unknown_1 GenRateStarStyle(Unknown_4 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_5()
            {
                [$"{componentCls}-star"] = new Unknown_6()
                {
                    Position = "relative",
                    Display = "inline-block",
                    Color = "inherit",
                    Cursor = "pointer",
                    ["&:not(:last-child)"] = new Unknown_7()
                    {
                        MarginInlineEnd = token.MarginXS,
                    },
                    ["> div"] = new Unknown_8()
                    {
                        Transition = @$"all {token.MotionDurationMid}, outline 0s",
                        ["&:hover"] = new Unknown_9()
                        {
                            Transform = token.RateStarHoverScale,
                        },
                        ["&:focus"] = new Unknown_10()
                        {
                            Outline = 0,
                        },
                        ["&:focus-visible"] = new Unknown_11()
                        {
                            Outline = @$"{token.LineWidth}px dashed {token.RateStarColor}",
                            Transform = token.RateStarHoverScale,
                        },
                    },
                    ["&-first, &-second"] = new Unknown_12()
                    {
                        Color = token.DefaultColor,
                        Transition = @$"all {token.MotionDurationMid}",
                        UserSelect = "none",
                        [token.IconCls] = new Unknown_13()
                        {
                            VerticalAlign = "middle",
                        },
                    },
                    ["&-first"] = new Unknown_14()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineStart = 0,
                        Width = "50%",
                        Height = "100%",
                        Overflow = "hidden",
                        Opacity = 0,
                    },
                    [$"&-half {componentCls}-star-first, &-half {componentCls}-star-second"] = new Unknown_15()
                    {
                        Opacity = 1,
                    },
                    [$"&-half {componentCls}-star-first, &-full {componentCls}-star-second"] = new Unknown_16()
                    {
                        Color = "inherit",
                    },
                },
            };
        }

        public CSSObject GenRateRtlStyle(RateToken token)
        {
            return new CSSObject()
            {
                [$"&-rtl{token.ComponentCls}"] = new CSSObject()
                {
                    Direction = "rtl",
                },
            };
        }

        public Unknown_2 GenRateStyle(Unknown_17 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_18()
            {
                [componentCls] = new Unknown_19()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    Margin = 0,
                    Padding = 0,
                    Color = token.RateStarColor,
                    FontSize = token.RateStarSize,
                    LineHeight = "unset",
                    ListStyle = "none",
                    Outline = "none",
                    [$"&-disabled{componentCls} {componentCls}-star"] = new Unknown_20()
                    {
                        Cursor = "default",
                        ["> div:hover"] = new Unknown_21()
                        {
                            Transform = "scale(1)",
                        },
                    },
                    ["..."] = GenRateStarStyle(token),
                    [$"+ {componentCls}-text"] = new Unknown_22()
                    {
                        Display = "inline-block",
                        MarginInlineStart = token.MarginXS,
                        FontSize = token.FontSize,
                    },
                    ["..."] = GenRateRtlStyle(token)
                },
            };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_23 token)
        {
            var colorFillContent = token.ColorFillContent;
            var rateToken = MergeToken(
                token,
                new Unknown_24()
                {
                    RateStarColor = token.Yellow6,
                    RateStarSize = token.ControlHeightLG * 0.5,
                    RateStarHoverScale = "scale(1.1)",
                    DefaultColor = colorFillContent,
                });
            return new Unknown_25 { GenRateStyle(rateToken) };
        }

    }

}