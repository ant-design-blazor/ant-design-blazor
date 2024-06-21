using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class RateToken
    {
        public string StarColor
        {
            get => (string)_tokens["starColor"];
            set => _tokens["starColor"] = value;
        }

        public double StarSize
        {
            get => (double)_tokens["starSize"];
            set => _tokens["starSize"] = value;
        }

        public string StarHoverScale
        {
            get => (string)_tokens["starHoverScale"];
            set => _tokens["starHoverScale"] = value;
        }

        public string StarBg
        {
            get => (string)_tokens["starBg"];
            set => _tokens["starBg"] = value;
        }

    }

    public partial class RateToken : TokenWithCommonCls
    {
    }

    public partial class Rate
    {
        public CSSObject GenRateStarStyle(RateToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-star"] = new CSSObject()
                {
                    Position = "relative",
                    Display = "inline-block",
                    Color = "inherit",
                    Cursor = "pointer",
                    ["&:not(:last-child)"] = new CSSObject()
                    {
                        MarginInlineEnd = token.MarginXS,
                    },
                    ["> div"] = new CSSObject()
                    {
                        Transition = @$"all {token.MotionDurationMid}, outline 0s",
                        ["&:hover"] = new CSSObject()
                        {
                            Transform = token.StarHoverScale,
                        },
                        ["&:focus"] = new CSSObject()
                        {
                            Outline = 0,
                        },
                        ["&:focus-visible"] = new CSSObject()
                        {
                            Outline = @$"{token.LineWidth}px dashed {token.StarColor}",
                            Transform = token.StarHoverScale,
                        },
                    },
                    ["&-first, &-second"] = new CSSObject()
                    {
                        Color = token.StarBg,
                        Transition = @$"all {token.MotionDurationMid}",
                        UserSelect = "none",
                    },
                    ["&-first"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineStart = 0,
                        Width = "50%",
                        Height = "100%",
                        Overflow = "hidden",
                        Opacity = 0,
                    },
                    [$"&-half {componentCls}-star-first, &-half {componentCls}-star-second"] = new CSSObject()
                    {
                        Opacity = 1,
                    },
                    [$"&-half {componentCls}-star-first, &-full {componentCls}-star-second"] = new CSSObject()
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

        public CSSObject GenRateStyle(RateToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    Margin = 0,
                    Padding = 0,
                    Color = token.StarColor,
                    FontSize = token.StarSize,
                    LineHeight = 1,
                    ListStyle = "none",
                    Outline = "none",
                    [$"&-disabled{componentCls} {componentCls}-star"] = new CSSObject()
                    {
                        Cursor = "default",
                        ["> div:hover"] = new CSSObject()
                        {
                            Transform = "scale(1)",
                        },
                    },
                    ["..."] = GenRateStarStyle(token),
                    ["..."] = GenRateRtlStyle(token)
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Rate",
                (token) =>
                {
                    var rateToken = MergeToken(
                        token,
                        new RateToken()
                        {
                        });
                    return new CSSInterpolation[]
                    {
                        GenRateStyle(rateToken),
                    };
                },
                (token) =>
                {
                    return new RateToken()
                    {
                        StarColor = token["yellow6"].ToString(),
                        StarSize = token.ControlHeightLG * 0.5,
                        StarHoverScale = "scale(1.1)",
                        StarBg = token.ColorFillContent,
                    };
                });
        }

    }

}