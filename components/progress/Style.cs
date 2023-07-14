using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class ProgressToken
    {
    }

    public partial class ProgressToken : TokenWithCommonCls
    {
        public int ProgressLineRadius { get; set; }

        public string ProgressInfoTextColor { get; set; }

        public string ProgressRemainingColor { get; set; }

        public string ProgressDefaultColor { get; set; }

        public int ProgressStepMinWidth { get; set; }

        public int ProgressStepMarginInlineEnd { get; set; }

        public string ProgressActiveMotionDuration { get; set; }

    }

    public partial class Progress
    {
        private Keyframes antProgressActive = new Keyframes("antProgressActive")
        {
            ["0%"] = new Keyframes()
            {
                Transform = "translateX(-100%) scaleX(0)",
                Opacity = 0.1f,
            },
            ["20%"] = new Keyframes()
            {
                Transform = "translateX(-100%) scaleX(0)",
                Opacity = 0.5f,
            },
            To = new Keyframes()
            {
                Transform = "translateX(0) scaleX(1)",
                Opacity = 0,
            },
        };

        public Unknown_1 GenBaseStyle(Unknown_4 token)
        {
            var progressCls = token.ComponentCls;
            var iconPrefixCls = token.IconCls;
            return new Unknown_5()
            {
                [progressCls] = new Unknown_6()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    ["&-rtl"] = new Unknown_7()
                    {
                        Direction = "rtl",
                    },
                    ["&-line"] = new Unknown_8()
                    {
                        Position = "relative",
                        Width = "100%",
                        FontSize = token.FontSize,
                        MarginInlineEnd = token.MarginXS,
                        MarginBottom = token.MarginXS,
                    },
                    [$"{progressCls}-outer"] = new Unknown_9()
                    {
                        Display = "inline-block",
                        Width = "100%",
                    },
                    [$"&{progressCls}-show-info"] = new Unknown_10()
                    {
                        [$"{progressCls}-outer"] = new Unknown_11()
                        {
                            MarginInlineEnd = @$"calc(-2em - {token.MarginXS}px)",
                            PaddingInlineEnd = @$"calc(2em + {token.PaddingXS}px)",
                        },
                    },
                    [$"{progressCls}-inner"] = new Unknown_12()
                    {
                        Position = "relative",
                        Display = "inline-block",
                        Width = "100%",
                        Overflow = "hidden",
                        VerticalAlign = "middle",
                        BackgroundColor = token.ProgressRemainingColor,
                        BorderRadius = token.ProgressLineRadius,
                    },
                    [$"{progressCls}-inner:not({progressCls}-circle-gradient)"] = new Unknown_13()
                    {
                        [$"{progressCls}-circle-path"] = new Unknown_14()
                        {
                            Stroke = token.ColorInfo,
                        },
                    },
                    [$"{progressCls}-success-bg, {progressCls}-bg"] = new Unknown_15()
                    {
                        Position = "relative",
                        BackgroundColor = token.ColorInfo,
                        BorderRadius = token.ProgressLineRadius,
                        Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseInOutCirc}",
                    },
                    [$"{progressCls}-success-bg"] = new Unknown_16()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        BackgroundColor = token.ColorSuccess,
                    },
                    [$"{progressCls}-text"] = new Unknown_17()
                    {
                        Display = "inline-block",
                        Width = "2em",
                        MarginInlineStart = token.MarginXS,
                        Color = token.ProgressInfoTextColor,
                        LineHeight = 1,
                        WhiteSpace = "nowrap",
                        TextAlign = "start",
                        VerticalAlign = "middle",
                        WordBreak = "normal",
                        [iconPrefixCls] = new Unknown_18()
                        {
                            FontSize = token.FontSize,
                        },
                    },
                    [$"&{progressCls}-status-active"] = new Unknown_19()
                    {
                        [$"{progressCls}-bg::before"] = new Unknown_20()
                        {
                            Position = "absolute",
                            Inset = 0,
                            BackgroundColor = token.ColorBgContainer,
                            BorderRadius = token.ProgressLineRadius,
                            Opacity = 0,
                            AnimationName = antProgressActive,
                            AnimationDuration = token.ProgressActiveMotionDuration,
                            AnimationTimingFunction = token.MotionEaseOutQuint,
                            AnimationIterationCount = "infinite",
                            Content = "\"\"",
                        },
                    },
                    [$"&{progressCls}-status-exception"] = new Unknown_21()
                    {
                        [$"{progressCls}-bg"] = new Unknown_22()
                        {
                            BackgroundColor = token.ColorError,
                        },
                        [$"{progressCls}-text"] = new Unknown_23()
                        {
                            Color = token.ColorError,
                        },
                    },
                    [$"&{progressCls}-status-exception {progressCls}-inner:not({progressCls}-circle-gradient)"] = new Unknown_24()
                    {
                        [$"{progressCls}-circle-path"] = new Unknown_25()
                        {
                            Stroke = token.ColorError,
                        },
                    },
                    [$"&{progressCls}-status-success"] = new Unknown_26()
                    {
                        [$"{progressCls}-bg"] = new Unknown_27()
                        {
                            BackgroundColor = token.ColorSuccess,
                        },
                        [$"{progressCls}-text"] = new Unknown_28()
                        {
                            Color = token.ColorSuccess,
                        },
                    },
                    [$"&{progressCls}-status-success {progressCls}-inner:not({progressCls}-circle-gradient)"] = new Unknown_29()
                    {
                        [$"{progressCls}-circle-path"] = new Unknown_30()
                        {
                            Stroke = token.ColorSuccess,
                        },
                    },
                },
            };
        }

        public Unknown_2 GenCircleStyle(Unknown_31 token)
        {
            var progressCls = token.ComponentCls;
            var iconPrefixCls = token.IconCls;
            return new Unknown_32()
            {
                [progressCls] = new Unknown_33()
                {
                    [$"{progressCls}-circle-trail"] = new Unknown_34()
                    {
                        Stroke = token.ProgressRemainingColor,
                    },
                    [$"&{progressCls}-circle {progressCls}-inner"] = new Unknown_35()
                    {
                        Position = "relative",
                        LineHeight = 1,
                        BackgroundColor = "transparent",
                    },
                    [$"&{progressCls}-circle {progressCls}-text"] = new Unknown_36()
                    {
                        Position = "absolute",
                        InsetBlockStart = "50%",
                        InsetInlineStart = 0,
                        Width = "100%",
                        Margin = 0,
                        Padding = 0,
                        Color = token.ColorText,
                        LineHeight = 1,
                        WhiteSpace = "normal",
                        TextAlign = "center",
                        Transform = "translateY(-50%)",
                        [iconPrefixCls] = new Unknown_37()
                        {
                            FontSize = @$"{token.FontSize / token.FontSizeSM}em",
                        },
                    },
                    [$"{progressCls}-circle&-status-exception"] = new Unknown_38()
                    {
                        [$"{progressCls}-text"] = new Unknown_39()
                        {
                            Color = token.ColorError,
                        },
                    },
                    [$"{progressCls}-circle&-status-success"] = new Unknown_40()
                    {
                        [$"{progressCls}-text"] = new Unknown_41()
                        {
                            Color = token.ColorSuccess,
                        },
                    },
                },
                [$"{progressCls}-inline-circle"] = new Unknown_42()
                {
                    LineHeight = 1,
                    [$"{progressCls}-inner"] = new Unknown_43()
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
                            BackgroundColor = token.ProgressRemainingColor,
                            Transition = @$"all {token.MotionDurationSlow}",
                            ["&-active"] = new CSSObject()
                            {
                                BackgroundColor = token.ColorInfo,
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

        public Unknown_3 GenComponentStyleHook(Unknown_44 token)
        {
            var progressStepMarginInlineEnd = token.MarginXXS / 2;
            var progressToken = MergeToken(
                token,
                new Unknown_45()
                {
                    ProgressLineRadius = 100,
                    ProgressInfoTextColor = token.ColorText,
                    ProgressDefaultColor = token.ColorInfo,
                    ProgressRemainingColor = token.ColorFillSecondary,
                    ProgressStepMarginInlineEnd = progressStepMarginInlineEnd,
                    ProgressStepMinWidth = progressStepMarginInlineEnd,
                    ProgressActiveMotionDuration = "2.4s",
                });
            return new Unknown_46
            {
                GenBaseStyle(progressToken),
                GenCircleStyle(progressToken),
                GenStepStyle(progressToken),
                GenSmallLine(progressToken)
            };
        }

    }

}