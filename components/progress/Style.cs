using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Keyframes = CssInCSharp.Keyframe;

namespace AntDesign
{
    public partial class ProgressToken
    {
        public string DefaultColor
        {
            get => (string)_tokens["defaultColor"];
            set => _tokens["defaultColor"] = value;
        }

        public string RemainingColor
        {
            get => (string)_tokens["remainingColor"];
            set => _tokens["remainingColor"] = value;
        }

        public string CircleTextColor
        {
            get => (string)_tokens["circleTextColor"];
            set => _tokens["circleTextColor"] = value;
        }

        public double LineBorderRadius
        {
            get => (double)_tokens["lineBorderRadius"];
            set => _tokens["lineBorderRadius"] = value;
        }

        public string CircleTextFontSize
        {
            get => (string)_tokens["circleTextFontSize"];
            set => _tokens["circleTextFontSize"] = value;
        }

    }

    public partial class ProgressToken : TokenWithCommonCls
    {
        public double ProgressStepMinWidth
        {
            get => (double)_tokens["progressStepMinWidth"];
            set => _tokens["progressStepMinWidth"] = value;
        }

        public double ProgressStepMarginInlineEnd
        {
            get => (double)_tokens["progressStepMarginInlineEnd"];
            set => _tokens["progressStepMarginInlineEnd"] = value;
        }

        public string ProgressActiveMotionDuration
        {
            get => (string)_tokens["progressActiveMotionDuration"];
            set => _tokens["progressActiveMotionDuration"] = value;
        }

    }

    public partial class Progress
    {
        public Keyframes GenAntProgressActive(bool isRtl = false)
        {
            var direction = isRtl ? "100%" : "-100%";
            return new Keyframes($"antProgress{(isRtl ? "RTL" : "LTR")}Active",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        Transform = @$"translateX({direction}) scaleX(0)",
                        Opacity = 0.1f,
                    },
                    ["20%"] = new CSSObject()
                    {
                        Transform = @$"translateX({direction}) scaleX(0)",
                        Opacity = 0.5f,
                    },
                    ["to"] = new CSSObject()
                    {
                        Transform = "translateX(0) scaleX(1)",
                        Opacity = 0,
                    },
                });
        }

        public CSSObject GenBaseStyle(ProgressToken token)
        {
            var progressCls = token.ComponentCls;
            var iconPrefixCls = token.IconCls;
            return new CSSObject()
            {
                [progressCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    ["&-line"] = new CSSObject()
                    {
                        Position = "relative",
                        Width = "100%",
                        FontSize = token.FontSize,
                        MarginInlineEnd = token.MarginXS,
                        MarginBottom = token.MarginXS,
                    },
                    [$"{progressCls}-outer"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = "100%",
                    },
                    [$"&{progressCls}-show-info"] = new CSSObject()
                    {
                        [$"{progressCls}-outer"] = new CSSObject()
                        {
                            MarginInlineEnd = @$"calc(-2em - {token.MarginXS}px)",
                            PaddingInlineEnd = @$"calc(2em + {token.PaddingXS}px)",
                        },
                    },
                    [$"{progressCls}-inner"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "inline-block",
                        Width = "100%",
                        Overflow = "hidden",
                        VerticalAlign = "middle",
                        BackgroundColor = token.RemainingColor,
                        BorderRadius = token.LineBorderRadius,
                    },
                    [$"{progressCls}-inner:not({progressCls}-circle-gradient)"] = new CSSObject()
                    {
                        [$"{progressCls}-circle-path"] = new CSSObject()
                        {
                            ["stroke"] = token.DefaultColor,
                        },
                    },
                    [$"{progressCls}-success-bg, {progressCls}-bg"] = new CSSObject()
                    {
                        Position = "relative",
                        BackgroundColor = token.DefaultColor,
                        BorderRadius = token.LineBorderRadius,
                        Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseInOutCirc}",
                    },
                    [$"{progressCls}-success-bg"] = new CSSObject()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        BackgroundColor = token.ColorSuccess,
                    },
                    [$"{progressCls}-text"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = "2em",
                        MarginInlineStart = token.MarginXS,
                        Color = token.ColorText,
                        LineHeight = 1,
                        WhiteSpace = "nowrap",
                        TextAlign = "start",
                        VerticalAlign = "middle",
                        WordBreak = "normal",
                        [iconPrefixCls] = new CSSObject()
                        {
                            FontSize = token.FontSize,
                        },
                    },
                    [$"&{progressCls}-status-active"] = new CSSObject()
                    {
                        [$"{progressCls}-bg::before"] = new CSSObject()
                        {
                            Position = "absolute",
                            Inset = 0,
                            BackgroundColor = token.ColorBgContainer,
                            BorderRadius = token.LineBorderRadius,
                            Opacity = 0,
                            AnimationName = GenAntProgressActive(),
                            AnimationDuration = token.ProgressActiveMotionDuration,
                            AnimationTimingFunction = token.MotionEaseOutQuint,
                            AnimationIterationCount = "infinite",
                            Content = "\"\"",
                        },
                    },
                    [$"&{progressCls}-rtl{progressCls}-status-active"] = new CSSObject()
                    {
                        [$"{progressCls}-bg::before"] = new CSSObject()
                        {
                            AnimationName = GenAntProgressActive(true)
                        },
                    },
                    [$"&{progressCls}-status-exception"] = new CSSObject()
                    {
                        [$"{progressCls}-bg"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorError,
                        },
                        [$"{progressCls}-text"] = new CSSObject()
                        {
                            Color = token.ColorError,
                        },
                    },
                    [$"&{progressCls}-status-exception {progressCls}-inner:not({progressCls}-circle-gradient)"] = new CSSObject()
                    {
                        [$"{progressCls}-circle-path"] = new CSSObject()
                        {
                            ["stroke"] = token.ColorError,
                        },
                    },
                    [$"&{progressCls}-status-success"] = new CSSObject()
                    {
                        [$"{progressCls}-bg"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorSuccess,
                        },
                        [$"{progressCls}-text"] = new CSSObject()
                        {
                            Color = token.ColorSuccess,
                        },
                    },
                    [$"&{progressCls}-status-success {progressCls}-inner:not({progressCls}-circle-gradient)"] = new CSSObject()
                    {
                        [$"{progressCls}-circle-path"] = new CSSObject()
                        {
                            ["stroke"] = token.ColorSuccess,
                        },
                    },
                },
            };
        }

        public CSSObject GenCircleStyle(ProgressToken token)
        {
            var progressCls = token.ComponentCls;
            var iconPrefixCls = token.IconCls;
            return new CSSObject()
            {
                [progressCls] = new CSSObject()
                {
                    [$"{progressCls}-circle-trail"] = new CSSObject()
                    {
                        ["stroke"] = token.RemainingColor,
                    },
                    [$"&{progressCls}-circle {progressCls}-inner"] = new CSSObject()
                    {
                        Position = "relative",
                        LineHeight = 1,
                        BackgroundColor = "transparent",
                    },
                    [$"&{progressCls}-circle {progressCls}-text"] = new CSSObject()
                    {
                        Position = "absolute",
                        InsetBlockStart = "50%",
                        InsetInlineStart = 0,
                        Width = "100%",
                        Margin = 0,
                        Padding = 0,
                        Color = token.CircleTextColor,
                        FontSize = token.CircleTextFontSize,
                        LineHeight = 1,
                        WhiteSpace = "normal",
                        TextAlign = "center",
                        Transform = "translateY(-50%)",
                        [iconPrefixCls] = new CSSObject()
                        {
                            FontSize = @$"{token.FontSize / token.FontSizeSM}em",
                        },
                    },
                    [$"{progressCls}-circle&-status-exception"] = new CSSObject()
                    {
                        [$"{progressCls}-text"] = new CSSObject()
                        {
                            Color = token.ColorError,
                        },
                    },
                    [$"{progressCls}-circle&-status-success"] = new CSSObject()
                    {
                        [$"{progressCls}-text"] = new CSSObject()
                        {
                            Color = token.ColorSuccess,
                        },
                    },
                },
                [$"{progressCls}-inline-circle"] = new CSSObject()
                {
                    LineHeight = 1,
                    [$"{progressCls}-inner"] = new CSSObject()
                    {
                        VerticalAlign = "bottom",
                    },
                },
            };
        }

        public CSSObject GenStepStyle(ProgressToken token)
        {
            var progressCls = token.ComponentCls;
            return new CSSObject()
            {
                [progressCls] = new CSSObject()
                {
                    [$"{progressCls}-steps"] = new CSSObject()
                    {
                        Display = "inline-block",
                        ["&-outer"] = new CSSObject()
                        {
                            Display = "flex",
                            FlexDirection = "row",
                            AlignItems = "center",
                        },
                        ["&-item"] = new CSSObject()
                        {
                            FlexShrink = 0,
                            MinWidth = token.ProgressStepMinWidth,
                            MarginInlineEnd = token.ProgressStepMarginInlineEnd,
                            BackgroundColor = token.RemainingColor,
                            Transition = @$"all {token.MotionDurationSlow}",
                            ["&-active"] = new CSSObject()
                            {
                                BackgroundColor = token.DefaultColor,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenSmallLine(ProgressToken token)
        {
            var progressCls = token.ComponentCls;
            var iconPrefixCls = token.IconCls;
            return new CSSObject()
            {
                [progressCls] = new CSSObject()
                {
                    [$"{progressCls}-small&-line, {progressCls}-small&-line {progressCls}-text {iconPrefixCls}"] = new CSSObject()
                    {
                        FontSize = token.FontSizeSM,
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Progress",
                (token) =>
                {
                    var progressStepMarginInlineEnd = token.MarginXXS / 2;
                    var progressToken = MergeToken(
                        token,
                        new ProgressToken()
                        {
                            ProgressStepMarginInlineEnd = progressStepMarginInlineEnd,
                            ProgressStepMinWidth = progressStepMarginInlineEnd,
                            ProgressActiveMotionDuration = "2.4s",
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(progressToken),
                        GenCircleStyle(progressToken),
                        GenStepStyle(progressToken),
                        GenSmallLine(progressToken),
                    };
                },
                (token) =>
                {
                    return new ProgressToken()
                    {
                        CircleTextColor = token.ColorText,
                        DefaultColor = token.ColorInfo,
                        RemainingColor = token.ColorFillSecondary,
                        LineBorderRadius = 100,
                        CircleTextFontSize = "1em",
                    };
                });
        }

    }

}