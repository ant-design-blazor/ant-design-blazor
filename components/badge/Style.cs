using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Keyframes = CssInCSharp.Keyframe;

namespace AntDesign
{
    public partial class BadgeToken
    {
        public string IndicatorZIndex
        {
            get => (string)_tokens["indicatorZIndex"];
            set => _tokens["indicatorZIndex"] = value;
        }

        public double IndicatorHeight
        {
            get => (double)_tokens["indicatorHeight"];
            set => _tokens["indicatorHeight"] = value;
        }

        public double IndicatorHeightSM
        {
            get => (double)_tokens["indicatorHeightSM"];
            set => _tokens["indicatorHeightSM"] = value;
        }

        public double DotSize
        {
            get => (double)_tokens["dotSize"];
            set => _tokens["dotSize"] = value;
        }

        public double TextFontSize
        {
            get => (double)_tokens["textFontSize"];
            set => _tokens["textFontSize"] = value;
        }

        public double TextFontSizeSM
        {
            get => (double)_tokens["textFontSizeSM"];
            set => _tokens["textFontSizeSM"] = value;
        }

        public string TextFontWeight
        {
            get => (string)_tokens["textFontWeight"];
            set => _tokens["textFontWeight"] = value;
        }

        public double StatusSize
        {
            get => (double)_tokens["statusSize"];
            set => _tokens["statusSize"] = value;
        }

    }

    public partial class BadgeToken : TokenWithCommonCls
    {
        public double BadgeFontHeight
        {
            get => (double)_tokens["badgeFontHeight"];
            set => _tokens["badgeFontHeight"] = value;
        }

        public string BadgeTextColor
        {
            get => (string)_tokens["badgeTextColor"];
            set => _tokens["badgeTextColor"] = value;
        }

        public string BadgeColor
        {
            get => (string)_tokens["badgeColor"];
            set => _tokens["badgeColor"] = value;
        }

        public string BadgeColorHover
        {
            get => (string)_tokens["badgeColorHover"];
            set => _tokens["badgeColorHover"] = value;
        }

        public double BadgeShadowSize
        {
            get => (double)_tokens["badgeShadowSize"];
            set => _tokens["badgeShadowSize"] = value;
        }

        public string BadgeShadowColor
        {
            get => (string)_tokens["badgeShadowColor"];
            set => _tokens["badgeShadowColor"] = value;
        }

        public string BadgeProcessingDuration
        {
            get => (string)_tokens["badgeProcessingDuration"];
            set => _tokens["badgeProcessingDuration"] = value;
        }

        public double BadgeRibbonOffset
        {
            get => (double)_tokens["badgeRibbonOffset"];
            set => _tokens["badgeRibbonOffset"] = value;
        }

        public string BadgeRibbonCornerTransform
        {
            get => (string)_tokens["badgeRibbonCornerTransform"];
            set => _tokens["badgeRibbonCornerTransform"] = value;
        }

        public string BadgeRibbonCornerFilter
        {
            get => (string)_tokens["badgeRibbonCornerFilter"];
            set => _tokens["badgeRibbonCornerFilter"] = value;
        }

    }

    public partial class Badge
    {
        private Keyframes _antStatusProcessing = new Keyframes("antStatusProcessing")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(0.8)",
                Opacity = 0.5f,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(2.4)",
                Opacity = 0,
            },
        };

        private Keyframes _antZoomBadgeIn = new Keyframes("antZoomBadgeIn")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(0) translate(50%, -50%)",
                Opacity = 0,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(1) translate(50%, -50%)",
            },
        };

        private Keyframes _antZoomBadgeOut = new Keyframes("antZoomBadgeOut")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(1) translate(50%, -50%)",
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(0) translate(50%, -50%)",
                Opacity = 0,
            },
        };

        private Keyframes _antNoWrapperZoomBadgeIn = new Keyframes("antNoWrapperZoomBadgeIn")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(0)",
                Opacity = 0,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(1)",
            },
        };

        private Keyframes _antNoWrapperZoomBadgeOut = new Keyframes("antNoWrapperZoomBadgeOut")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(1)",
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(0)",
                Opacity = 0,
            },
        };

        private Keyframes _antBadgeLoadingCircle = new Keyframes("antBadgeLoadingCircle")
        {
            ["0%"] = new CSSObject()
            {
                TransformOrigin = "50%",
            },
            ["100%"] = new CSSObject()
            {
                Transform = "translate(50%, -50%) rotate(360deg)",
                TransformOrigin = "50%",
            },
        };

        public CSSObject GenSharedBadgeStyle(BadgeToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var antCls = token.AntCls;
            var badgeShadowSize = token.BadgeShadowSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var textFontSize = token.TextFontSize;
            var textFontSizeSM = token.TextFontSizeSM;
            var statusSize = token.StatusSize;
            var dotSize = token.DotSize;
            var textFontWeight = token.TextFontWeight;
            var indicatorHeight = token.IndicatorHeight;
            var indicatorHeightSM = token.IndicatorHeightSM;
            var marginXS = token.MarginXS;
            var numberPrefixCls = @$"{antCls}-scroll-number";
            var colorPreset = GenPresetColor(
                token,
                (colorKey, args) =>
                {
                    var darkColor = args.DarkColor;
                    return new CSSObject()
                    {
                        [$"&{componentCls} {componentCls}-color-{colorKey}"] = new CSSObject()
                        {
                            Background = darkColor,
                            [$"&:not({componentCls}-count)"] = new CSSObject()
                            {
                                Color = darkColor,
                            },
                        },
                    };
                });
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    Width = "fit-content",
                    LineHeight = 1,
                    [$"{componentCls}-count"] = new CSSObject()
                    {
                        ZIndex = token.IndicatorZIndex,
                        MinWidth = indicatorHeight,
                        Height = indicatorHeight,
                        Color = token.BadgeTextColor,
                        FontWeight = textFontWeight,
                        FontSize = textFontSize,
                        LineHeight = @$"{indicatorHeight}px",
                        WhiteSpace = "nowrap",
                        TextAlign = "center",
                        Background = token.BadgeColor,
                        BorderRadius = indicatorHeight / 2,
                        BoxShadow = @$"0 0 0 {badgeShadowSize}px {token.BadgeShadowColor}",
                        Transition = @$"background {token.MotionDurationMid}",
                        ["a"] = new CSSObject()
                        {
                            Color = token.BadgeTextColor,
                        },
                        ["a:hover"] = new CSSObject()
                        {
                            Color = token.BadgeTextColor,
                        },
                        ["a:hover &"] = new CSSObject()
                        {
                            Background = token.BadgeColorHover,
                        },
                    },
                    [$"{componentCls}-count-sm"] = new CSSObject()
                    {
                        MinWidth = indicatorHeightSM,
                        Height = indicatorHeightSM,
                        FontSize = textFontSizeSM,
                        LineHeight = @$"{indicatorHeightSM}px",
                        BorderRadius = indicatorHeightSM / 2,
                    },
                    [$"{componentCls}-multiple-words"] = new CSSObject()
                    {
                        Padding = @$"0 {token.PaddingXS}px",
                        ["bdi"] = new CSSObject()
                        {
                            UnicodeBidi = "plaintext",
                        },
                    },
                    [$"{componentCls}-dot"] = new CSSObject()
                    {
                        ZIndex = token.IndicatorZIndex,
                        Width = dotSize,
                        MinWidth = dotSize,
                        Height = dotSize,
                        Background = token.BadgeColor,
                        BorderRadius = "100%",
                        BoxShadow = @$"0 0 0 {badgeShadowSize}px {token.BadgeShadowColor}",
                    },
                    [$"{componentCls}-dot{numberPrefixCls}"] = new CSSObject()
                    {
                        Transition = @$"background {motionDurationSlow}",
                    },
                    [$"{componentCls}-count, {componentCls}-dot, {numberPrefixCls}-custom-component"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = 0,
                        Transform = "translate(50%, -50%)",
                        TransformOrigin = "100% 0%",
                        [$"&{iconCls}-spin"] = new CSSObject()
                        {
                            AnimationName = _antBadgeLoadingCircle,
                            AnimationDuration = "1s",
                            AnimationIterationCount = "infinite",
                            AnimationTimingFunction = "linear",
                        },
                    },
                    [$"&{componentCls}-status"] = new CSSObject()
                    {
                        LineHeight = "inherit",
                        VerticalAlign = "baseline",
                        [$"{componentCls}-status-dot"] = new CSSObject()
                        {
                            Position = "relative",
                            Top = -1,
                            Display = "inline-block",
                            Width = statusSize,
                            Height = statusSize,
                            VerticalAlign = "middle",
                            BorderRadius = "50%",
                        },
                        [$"{componentCls}-status-success"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorSuccess,
                        },
                        [$"{componentCls}-status-processing"] = new CSSObject()
                        {
                            Overflow = "visible",
                            Color = token.ColorPrimary,
                            BackgroundColor = token.ColorPrimary,
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = 0,
                                InsetInlineStart = 0,
                                Width = "100%",
                                Height = "100%",
                                BorderWidth = badgeShadowSize,
                                BorderStyle = "solid",
                                BorderColor = "inherit",
                                BorderRadius = "50%",
                                AnimationName = _antStatusProcessing,
                                AnimationDuration = token.BadgeProcessingDuration,
                                AnimationIterationCount = "infinite",
                                AnimationTimingFunction = "ease-in-out",
                                Content = "\"\"",
                            },
                        },
                        [$"{componentCls}-status-default"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorTextPlaceholder,
                        },
                        [$"{componentCls}-status-error"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorError,
                        },
                        [$"{componentCls}-status-warning"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorWarning,
                        },
                        [$"{componentCls}-status-text"] = new CSSObject()
                        {
                            MarginInlineStart = marginXS,
                            Color = token.ColorText,
                            FontSize = token.FontSize,
                        },
                    },
                    ["..."] = colorPreset,
                    [$"{componentCls}-zoom-appear, {componentCls}-zoom-enter"] = new CSSObject()
                    {
                        AnimationName = _antZoomBadgeIn,
                        AnimationDuration = token.MotionDurationSlow,
                        AnimationTimingFunction = token.MotionEaseOutBack,
                        AnimationFillMode = "both",
                    },
                    [$"{componentCls}-zoom-leave"] = new CSSObject()
                    {
                        AnimationName = _antZoomBadgeOut,
                        AnimationDuration = token.MotionDurationSlow,
                        AnimationTimingFunction = token.MotionEaseOutBack,
                        AnimationFillMode = "both",
                    },
                    [$"&{componentCls}-not-a-wrapper"] = new CSSObject()
                    {
                        [$"{componentCls}-zoom-appear, {componentCls}-zoom-enter"] = new CSSObject()
                        {
                            AnimationName = _antNoWrapperZoomBadgeIn,
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = token.MotionEaseOutBack,
                        },
                        [$"{componentCls}-zoom-leave"] = new CSSObject()
                        {
                            AnimationName = _antNoWrapperZoomBadgeOut,
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = token.MotionEaseOutBack,
                        },
                        [$"&:not({componentCls}-status)"] = new CSSObject()
                        {
                            VerticalAlign = "middle",
                        },
                        [$"{numberPrefixCls}-custom-component, {componentCls}-count"] = new CSSObject()
                        {
                            Transform = "none",
                        },
                        [$"{numberPrefixCls}-custom-component, {numberPrefixCls}"] = new CSSObject()
                        {
                            Position = "relative",
                            Top = "auto",
                            Display = "block",
                            TransformOrigin = "50% 50%",
                        },
                    },
                    [$"{numberPrefixCls}"] = new CSSObject()
                    {
                        Overflow = "hidden",
                        [$"{numberPrefixCls}-only"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-block",
                            Height = indicatorHeight,
                            Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseOutBack}",
                            WebkitTransformStyle = "preserve-3d",
                            WebkitBackfaceVisibility = "hidden",
                            [$"> p{numberPrefixCls}-only-unit"] = new CSSObject()
                            {
                                Height = indicatorHeight,
                                Margin = 0,
                                WebkitTransformStyle = "preserve-3d",
                                WebkitBackfaceVisibility = "hidden",
                            },
                        },
                        [$"{numberPrefixCls}-symbol"] = new CSSObject()
                        {
                            VerticalAlign = "top",
                        },
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                        [$"{componentCls}-count, {componentCls}-dot, {numberPrefixCls}-custom-component"] = new CSSObject()
                        {
                            Transform = "translate(-50%, -50%)",
                        },
                    },
                },
            };
        }

        public BadgeToken PrepareToken(BadgeToken token)
        {
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var lineWidth = token.LineWidth;
            var marginXS = token.MarginXS;
            var colorBorderBg = token.ColorBorderBg;
            var badgeFontHeight = Math.Round(fontSize * lineHeight);
            var badgeShadowSize = lineWidth;
            var badgeTextColor = token.ColorBgContainer;
            var badgeColor = token.ColorError;
            var badgeColorHover = token.ColorErrorHover;
            var badgeToken = MergeToken(
                token,
                new BadgeToken()
                {
                    BadgeFontHeight = badgeFontHeight,
                    BadgeShadowSize = badgeShadowSize,
                    BadgeTextColor = badgeTextColor,
                    BadgeColor = badgeColor,
                    BadgeColorHover = badgeColorHover,
                    BadgeShadowColor = colorBorderBg,
                    BadgeProcessingDuration = "1.2s",
                    BadgeRibbonOffset = marginXS,
                    BadgeRibbonCornerTransform = "scaleY(0.75)",
                    BadgeRibbonCornerFilter = @$"brightness(75%)",
                });
            return badgeToken;
        }

        public BadgeToken PrepareComponentToken(GlobalToken token)
        {
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var fontSizeSM = token.FontSizeSM;
            var lineWidth = token.LineWidth;
            return new BadgeToken()
            {
                IndicatorZIndex = "auto",
                IndicatorHeight = Math.Round(fontSize * lineHeight) - 2 * lineWidth,
                IndicatorHeightSM = fontSize,
                DotSize = fontSizeSM / 2,
                TextFontSize = fontSizeSM,
                TextFontSizeSM = fontSizeSM,
                TextFontWeight = "normal",
                StatusSize = fontSizeSM / 2,
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Badge",
                (token) =>
                {
                    var badgeToken = PrepareToken(token);
                    return new CSSInterpolation[] { GenSharedBadgeStyle(badgeToken) };
                },
                PrepareComponentToken);
        }

    }

}