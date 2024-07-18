using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class CarouselToken : TokenWithCommonCls
    {
        public double DotWidth
        {
            get => (double)_tokens["dotWidth"];
            set => _tokens["dotWidth"] = value;
        }

        public double DotHeight
        {
            get => (double)_tokens["dotHeight"];
            set => _tokens["dotHeight"] = value;
        }

        public double DotWidthActive
        {
            get => (double)_tokens["dotWidthActive"];
            set => _tokens["dotWidthActive"] = value;
        }

        public double DotActiveWidth
        {
            get => (double)_tokens["dotActiveWidth"];
            set => _tokens["dotActiveWidth"] = value;
        }

    }

    public partial class CarouselToken
    {
        public double CarouselArrowSize
        {
            get => (double)_tokens["carouselArrowSize"];
            set => _tokens["carouselArrowSize"] = value;
        }

        public double CarouselDotOffset
        {
            get => (double)_tokens["carouselDotOffset"];
            set => _tokens["carouselDotOffset"] = value;
        }

        public double CarouselDotInline
        {
            get => (double)_tokens["carouselDotInline"];
            set => _tokens["carouselDotInline"] = value;
        }

    }

    public partial class CarouselStyle
    {
        public static CSSObject GenCarouselStyle(CarouselToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var carouselArrowSize = token.CarouselArrowSize;
            var carouselDotOffset = token.CarouselDotOffset;
            var marginXXS = token.MarginXXS;
            var arrowOffset = -carouselArrowSize * 1.25;
            var carouselDotMargin = marginXXS;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    [".slick-slider"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "block",
                        BoxSizing = "border-box",
                        TouchAction = "pan-y",
                        WebkitTouchCallout = "none",
                        WebkitTapHighlightColor = "transparent",
                        [".slick-track, .slick-list"] = new CSSObject()
                        {
                            Transform = "translate3d(0, 0, 0)",
                            TouchAction = "pan-y",
                        },
                    },
                    [".slick-list"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "block",
                        Margin = 0,
                        Padding = 0,
                        Overflow = "hidden",
                        ["&:focus"] = new CSSObject()
                        {
                            Outline = "none",
                        },
                        ["&.dragging"] = new CSSObject()
                        {
                            Cursor = "pointer",
                        },
                        [".slick-slide"] = new CSSObject()
                        {
                            PointerEvents = "none",
                            [$"input{antCls}-radio-input, input{antCls}-checkbox-input"] = new CSSObject()
                            {
                                Visibility = "hidden",
                            },
                            ["&.slick-active"] = new CSSObject()
                            {
                                PointerEvents = "auto",
                                [$"input{antCls}-radio-input, input{antCls}-checkbox-input"] = new CSSObject()
                                {
                                    Visibility = "visible",
                                },
                            },
                            ["> div > div"] = new CSSObject()
                            {
                                VerticalAlign = "bottom",
                            },
                        },
                    },
                    [".slick-track"] = new CSSObject()
                    {
                        Position = "relative",
                        Top = 0,
                        InsetInlineStart = 0,
                        Display = "block",
                        ["&::before, &::after"] = new CSSObject()
                        {
                            Display = "table",
                            Content = "\"\"",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Clear = "both",
                        },
                    },
                    [".slick-slide"] = new CSSObject()
                    {
                        Display = "none",
                        Float = "left",
                        Height = "100%",
                        MinHeight = 1,
                        ["img"] = new CSSObject()
                        {
                            Display = "block",
                        },
                        ["&.dragging img"] = new CSSObject()
                        {
                            PointerEvents = "none",
                        },
                    },
                    [".slick-initialized .slick-slide"] = new CSSObject()
                    {
                        Display = "block",
                    },
                    [".slick-vertical .slick-slide"] = new CSSObject()
                    {
                        Display = "block",
                        Height = "auto",
                    },
                    [".slick-arrow.slick-hidden"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    [".slick-prev, .slick-next"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = "50%",
                        Display = "block",
                        Width = carouselArrowSize,
                        Height = carouselArrowSize,
                        MarginTop = -carouselArrowSize / 2,
                        Padding = 0,
                        Color = "transparent",
                        FontSize = 0,
                        LineHeight = 0,
                        Background = "transparent",
                        Border = 0,
                        Outline = "none",
                        Cursor = "pointer",
                        ["&:hover, &:focus"] = new CSSObject()
                        {
                            Color = "transparent",
                            Background = "transparent",
                            Outline = "none",
                            ["&::before"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                        },
                        ["&.slick-disabled::before"] = new CSSObject()
                        {
                            Opacity = 0.25,
                        },
                    },
                    [".slick-prev"] = new CSSObject()
                    {
                        InsetInlineStart = arrowOffset,
                        ["&::before"] = new CSSObject()
                        {
                            Content = "\"←\"",
                        },
                    },
                    [".slick-next"] = new CSSObject()
                    {
                        InsetInlineEnd = arrowOffset,
                        ["&::before"] = new CSSObject()
                        {
                            Content = "\"→\"",
                        },
                    },
                    [".slick-dots"] = new CSSObject()
                    {
                        Position = "absolute",
                        InsetInlineEnd = 0,
                        Bottom = 0,
                        InsetInlineStart = 0,
                        ZIndex = 15,
                        Display = "flex !important",
                        JustifyContent = "center",
                        PaddingInlineStart = 0,
                        Margin = 0,
                        ListStyle = "none",
                        ["&-bottom"] = new CSSObject()
                        {
                            Bottom = carouselDotOffset,
                        },
                        ["&-top"] = new CSSObject()
                        {
                            Top = carouselDotOffset,
                            Bottom = "auto",
                        },
                        ["li"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-block",
                            Flex = "0 1 auto",
                            BoxSizing = "content-box",
                            Width = token.DotWidth,
                            Height = token.DotHeight,
                            MarginInline = carouselDotMargin,
                            Padding = 0,
                            TextAlign = "center",
                            TextIndent = -999,
                            VerticalAlign = "top",
                            Transition = @$"all {token.MotionDurationSlow}",
                            ["button"] = new CSSObject()
                            {
                                Position = "relative",
                                Display = "block",
                                Width = "100%",
                                Height = token.DotHeight,
                                Padding = 0,
                                Color = "transparent",
                                FontSize = 0,
                                Background = token.ColorBgContainer,
                                Border = 0,
                                BorderRadius = token.DotHeight,
                                Outline = "none",
                                Cursor = "pointer",
                                Opacity = 0.3,
                                Transition = @$"all {token.MotionDurationSlow}",
                                ["&: hover, &:focus"] = new CSSObject()
                                {
                                    Opacity = 0.75,
                                },
                                ["&::after"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Inset = -carouselDotMargin,
                                    Content = "\"\"",
                                },
                            },
                            ["&.slick-active"] = new CSSObject()
                            {
                                Width = token.DotActiveWidth,
                                ["& button"] = new CSSObject()
                                {
                                    Background = token.ColorBgContainer,
                                    Opacity = 1,
                                },
                                ["&: hover, &:focus"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenCarouselVerticalStyle(CarouselToken token)
        {
            var componentCls = token.ComponentCls;
            var carouselDotOffset = token.CarouselDotOffset;
            var marginXXS = token.MarginXXS;
            var reverseSizeOfDot = new CSSObject()
            {
                Width = token.DotHeight,
                Height = token.DotWidth,
            };
            return new CSSObject()
            {
                [$"{componentCls}-vertical"] = new CSSObject()
                {
                    [".slick-dots"] = new CSSObject()
                    {
                        Top = "50%",
                        Bottom = "auto",
                        FlexDirection = "column",
                        Width = token.DotHeight,
                        Height = "auto",
                        Margin = 0,
                        Transform = "translateY(-50%)",
                        ["&-left"] = new CSSObject()
                        {
                            InsetInlineEnd = "auto",
                            InsetInlineStart = carouselDotOffset,
                        },
                        ["&-right"] = new CSSObject()
                        {
                            InsetInlineEnd = carouselDotOffset,
                            InsetInlineStart = "auto",
                        },
                        ["li"] = new CSSObject()
                        {
                            ["..."] = reverseSizeOfDot,
                            Margin = @$"{marginXXS}px 0",
                            VerticalAlign = "baseline",
                            ["button"] = reverseSizeOfDot,
                            ["&.slick-active"] = new CSSObject()
                            {
                                ["..."] = reverseSizeOfDot,
                                ["button"] = reverseSizeOfDot,
                            },
                        },
                    },
                },
            };
        }

        public static CSSInterpolation[] GenCarouselRtlStyle(CarouselToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                        [".slick-dots"] = new CSSObject()
                        {
                            [$"{componentCls}-rtl&"] = new CSSObject()
                            {
                                FlexDirection = "row-reverse",
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-vertical"] = new CSSObject()
                    {
                        [".slick-dots"] = new CSSObject()
                        {
                            [$"{componentCls}-rtl&"] = new CSSObject()
                            {
                                FlexDirection = "column",
                            },
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Carousel",
                (token) =>
                {
                    var controlHeightLG = token.ControlHeightLG;
                    var controlHeightSM = token.ControlHeightSM;
                    var carouselToken = MergeToken(
                        token,
                        new CarouselToken()
                        {
                            CarouselArrowSize = controlHeightLG / 2,
                            CarouselDotOffset = controlHeightSM / 2,
                        });
                    return new CSSInterpolation[]
                    {
                        GenCarouselStyle(carouselToken),
                        GenCarouselVerticalStyle(carouselToken),
                        GenCarouselRtlStyle(carouselToken),
                    };
                },
                (token) =>
                {
                    var dotActiveWidth = 24;
                    return new CarouselToken()
                    {
                        DotWidth = 16,
                        DotHeight = 3,
                        DotWidthActive = dotActiveWidth,
                        DotActiveWidth = dotActiveWidth,
                    };
                },
                new GenOptions()
                {
                    DeprecatedTokens = new ()
                    {
                        new ("dotWidthActive", "dotActiveWidth")
                    }
                });
        }

    }

}
