using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class SwitchToken
    {
        public double TrackHeight
        {
            get => (double)_tokens["trackHeight"];
            set => _tokens["trackHeight"] = value;
        }

        public double TrackHeightSM
        {
            get => (double)_tokens["trackHeightSM"];
            set => _tokens["trackHeightSM"] = value;
        }

        public double TrackMinWidth
        {
            get => (double)_tokens["trackMinWidth"];
            set => _tokens["trackMinWidth"] = value;
        }

        public double TrackMinWidthSM
        {
            get => (double)_tokens["trackMinWidthSM"];
            set => _tokens["trackMinWidthSM"] = value;
        }

        public double TrackPadding
        {
            get => (double)_tokens["trackPadding"];
            set => _tokens["trackPadding"] = value;
        }

        public string HandleBg
        {
            get => (string)_tokens["handleBg"];
            set => _tokens["handleBg"] = value;
        }

        public string HandleShadow
        {
            get => (string)_tokens["handleShadow"];
            set => _tokens["handleShadow"] = value;
        }

        public double HandleSize
        {
            get => (double)_tokens["handleSize"];
            set => _tokens["handleSize"] = value;
        }

        public double HandleSizeSM
        {
            get => (double)_tokens["handleSizeSM"];
            set => _tokens["handleSizeSM"] = value;
        }

        public double InnerMinMargin
        {
            get => (double)_tokens["innerMinMargin"];
            set => _tokens["innerMinMargin"] = value;
        }

        public double InnerMaxMargin
        {
            get => (double)_tokens["innerMaxMargin"];
            set => _tokens["innerMaxMargin"] = value;
        }

        public double InnerMinMarginSM
        {
            get => (double)_tokens["innerMinMarginSM"];
            set => _tokens["innerMinMarginSM"] = value;
        }

        public double InnerMaxMarginSM
        {
            get => (double)_tokens["innerMaxMarginSM"];
            set => _tokens["innerMaxMarginSM"] = value;
        }

    }

    public partial class SwitchToken : TokenWithCommonCls
    {
        public string SwitchDuration
        {
            get => (string)_tokens["switchDuration"];
            set => _tokens["switchDuration"] = value;
        }

        public string SwitchColor
        {
            get => (string)_tokens["switchColor"];
            set => _tokens["switchColor"] = value;
        }

        public double SwitchDisabledOpacity
        {
            get => (double)_tokens["switchDisabledOpacity"];
            set => _tokens["switchDisabledOpacity"] = value;
        }

        public double SwitchLoadingIconSize
        {
            get => (double)_tokens["switchLoadingIconSize"];
            set => _tokens["switchLoadingIconSize"] = value;
        }

        public string SwitchLoadingIconColor
        {
            get => (string)_tokens["switchLoadingIconColor"];
            set => _tokens["switchLoadingIconColor"] = value;
        }

        public string SwitchHandleActiveInset
        {
            get => (string)_tokens["switchHandleActiveInset"];
            set => _tokens["switchHandleActiveInset"] = value;
        }

    }

    public partial class Switch
    {
        public CSSObject GenSwitchSmallStyle(SwitchToken token)
        {
            var componentCls = token.ComponentCls;
            var trackHeightSM = token.TrackHeightSM;
            var trackPadding = token.TrackPadding;
            var trackMinWidthSM = token.TrackMinWidthSM;
            var innerMinMarginSM = token.InnerMinMarginSM;
            var innerMaxMarginSM = token.InnerMaxMarginSM;
            var handleSizeSM = token.HandleSizeSM;
            var switchInnerCls = @$"{componentCls}-inner";
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"&{componentCls}-small"] = new CSSObject()
                    {
                        MinWidth = trackMinWidthSM,
                        Height = trackHeightSM,
                        LineHeight = @$"{trackHeightSM}px",
                        [$"{componentCls}-inner"] = new CSSObject()
                        {
                            PaddingInlineStart = innerMaxMarginSM,
                            PaddingInlineEnd = innerMinMarginSM,
                            [$"{switchInnerCls}-checked"] = new CSSObject()
                            {
                                MarginInlineStart = @$"calc(-100% + {handleSizeSM + trackPadding * 2}px - {
              innerMaxMarginSM * 2
            }px)",
                                MarginInlineEnd = @$"calc(100% - {handleSizeSM + trackPadding * 2}px + {
              innerMaxMarginSM * 2
            }px)",
                            },
                            [$"{switchInnerCls}-unchecked"] = new CSSObject()
                            {
                                MarginTop = -trackHeightSM,
                                MarginInlineStart = 0,
                                MarginInlineEnd = 0,
                            },
                        },
                        [$"{componentCls}-handle"] = new CSSObject()
                        {
                            Width = handleSizeSM,
                            Height = handleSizeSM,
                        },
                        [$"{componentCls}-loading-icon"] = new CSSObject()
                        {
                            Top = (handleSizeSM - token.SwitchLoadingIconSize) / 2,
                            FontSize = token.SwitchLoadingIconSize,
                        },
                        [$"&{componentCls}-checked"] = new CSSObject()
                        {
                            [$"{componentCls}-inner"] = new CSSObject()
                            {
                                PaddingInlineStart = innerMinMarginSM,
                                PaddingInlineEnd = innerMaxMarginSM,
                                [$"{switchInnerCls}-checked"] = new CSSObject()
                                {
                                    MarginInlineStart = 0,
                                    MarginInlineEnd = 0,
                                },
                                [$"{switchInnerCls}-unchecked"] = new CSSObject()
                                {
                                    MarginInlineStart = @$"calc(100% - {handleSizeSM + trackPadding * 2}px + {
                innerMaxMarginSM * 2
              }px)",
                                    MarginInlineEnd = @$"calc(-100% + {handleSizeSM + trackPadding * 2}px - {
                innerMaxMarginSM * 2
              }px)",
                                },
                            },
                            [$"{componentCls}-handle"] = new CSSObject()
                            {
                                InsetInlineStart = @$"calc(100% - {handleSizeSM + trackPadding}px)",
                            },
                        },
                        [$"&:not({componentCls}-disabled):active"] = new CSSObject()
                        {
                            [$"&:not({componentCls}-checked) {switchInnerCls}"] = new CSSObject()
                            {
                                [$"{switchInnerCls}-unchecked"] = new CSSObject()
                                {
                                    MarginInlineStart = token.MarginXXS / 2,
                                    MarginInlineEnd = -token.MarginXXS / 2,
                                },
                            },
                            [$"&{componentCls}-checked {switchInnerCls}"] = new CSSObject()
                            {
                                [$"{switchInnerCls}-checked"] = new CSSObject()
                                {
                                    MarginInlineStart = -token.MarginXXS / 2,
                                    MarginInlineEnd = token.MarginXXS / 2,
                                },
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenSwitchLoadingStyle(SwitchToken token)
        {
            var componentCls = token.ComponentCls;
            var handleSize = token.HandleSize;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"{componentCls}-loading-icon{token.IconCls}"] = new CSSObject()
                    {
                        Position = "relative",
                        Top = (handleSize - token.FontSize) / 2,
                        Color = token.SwitchLoadingIconColor,
                        VerticalAlign = "top",
                    },
                    [$"&{componentCls}-checked {componentCls}-loading-icon"] = new CSSObject()
                    {
                        Color = token.SwitchColor,
                    },
                },
            };
        }

        public CSSObject GenSwitchHandleStyle(SwitchToken token)
        {
            var componentCls = token.ComponentCls;
            var motion = token.Motion;
            var trackPadding = token.TrackPadding;
            var handleBg = token.HandleBg;
            var handleShadow = token.HandleShadow;
            var handleSize = token.HandleSize;
            var switchHandleCls = @$"{componentCls}-handle";
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [switchHandleCls] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = trackPadding,
                        InsetInlineStart = trackPadding,
                        Width = handleSize,
                        Height = handleSize,
                        Transition = @$"all {token.SwitchDuration} ease-in-out",
                        ["&::before"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineEnd = 0,
                            Bottom = 0,
                            InsetInlineStart = 0,
                            BackgroundColor = handleBg,
                            BorderRadius = handleSize / 2,
                            BoxShadow = handleShadow,
                            Transition = @$"all {token.SwitchDuration} ease-in-out",
                            Content = "\"\"",
                        },
                    },
                    [$"&{componentCls}-checked {switchHandleCls}"] = new CSSObject()
                    {
                        InsetInlineStart = @$"calc(100% - {handleSize + trackPadding}px)",
                    },
                    [$"&:not({componentCls}-disabled):active"] = (motion
                    ? new CSSObject()
                    {
                        [$"{switchHandleCls}::before"] = new CSSObject()
                        {
                            InsetInlineEnd = token.SwitchHandleActiveInset,
                            InsetInlineStart = 0,
                        },
                        [$"&{componentCls}-checked {switchHandleCls}::before"] = new CSSObject()
                        {
                            InsetInlineEnd = 0,
                            InsetInlineStart = token.SwitchHandleActiveInset,
                        },
                    }
                    : new CSSObject()
                    {
                    })
                },
            };
        }

        public CSSObject GenSwitchInnerStyle(SwitchToken token)
        {
            var componentCls = token.ComponentCls;
            var trackHeight = token.TrackHeight;
            var trackPadding = token.TrackPadding;
            var innerMinMargin = token.InnerMinMargin;
            var innerMaxMargin = token.InnerMaxMargin;
            var handleSize = token.HandleSize;
            var switchInnerCls = @$"{componentCls}-inner";
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [switchInnerCls] = new CSSObject()
                    {
                        Display = "block",
                        Overflow = "hidden",
                        BorderRadius = 100,
                        Height = "100%",
                        PaddingInlineStart = innerMaxMargin,
                        PaddingInlineEnd = innerMinMargin,
                        Transition = @$"padding-inline-start {token.SwitchDuration} ease-in-out, padding-inline-end {token.SwitchDuration} ease-in-out",
                        [$"{switchInnerCls}-checked, {switchInnerCls}-unchecked"] = new CSSObject()
                        {
                            Display = "block",
                            Color = token.ColorTextLightSolid,
                            FontSize = token.FontSizeSM,
                            Transition = @$"margin-inline-start {token.SwitchDuration} ease-in-out, margin-inline-end {token.SwitchDuration} ease-in-out",
                            PointerEvents = "none",
                        },
                        [$"{switchInnerCls}-checked"] = new CSSObject()
                        {
                            MarginInlineStart = @$"calc(-100% + {handleSize + trackPadding * 2}px - {
            innerMaxMargin * 2
          }px)",
                            MarginInlineEnd = @$"calc(100% - {handleSize + trackPadding * 2}px + {
            innerMaxMargin * 2
          }px)",
                        },
                        [$"{switchInnerCls}-unchecked"] = new CSSObject()
                        {
                            MarginTop = -trackHeight,
                            MarginInlineStart = 0,
                            MarginInlineEnd = 0,
                        },
                    },
                    [$"&{componentCls}-checked {switchInnerCls}"] = new CSSObject()
                    {
                        PaddingInlineStart = innerMinMargin,
                        PaddingInlineEnd = innerMaxMargin,
                        [$"{switchInnerCls}-checked"] = new CSSObject()
                        {
                            MarginInlineStart = 0,
                            MarginInlineEnd = 0,
                        },
                        [$"{switchInnerCls}-unchecked"] = new CSSObject()
                        {
                            MarginInlineStart = @$"calc(100% - {handleSize + trackPadding * 2}px + {
            innerMaxMargin * 2
          }px)",
                            MarginInlineEnd = @$"calc(-100% + {handleSize + trackPadding * 2}px - {
            innerMaxMargin * 2
          }px)",
                        },
                    },
                    [$"&:not({componentCls}-disabled):active"] = new CSSObject()
                    {
                        [$"&:not({componentCls}-checked) {switchInnerCls}"] = new CSSObject()
                        {
                            [$"{switchInnerCls}-unchecked"] = new CSSObject()
                            {
                                MarginInlineStart = trackPadding * 2,
                                MarginInlineEnd = -trackPadding * 2,
                            },
                        },
                        [$"&{componentCls}-checked {switchInnerCls}"] = new CSSObject()
                        {
                            [$"{switchInnerCls}-checked"] = new CSSObject()
                            {
                                MarginInlineStart = -trackPadding * 2,
                                MarginInlineEnd = trackPadding * 2,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenSwitchStyle(SwitchToken token)
        {
            var componentCls = token.ComponentCls;
            var trackHeight = token.TrackHeight;
            var trackMinWidth = token.TrackMinWidth;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    BoxSizing = "border-box",
                    MinWidth = trackMinWidth,
                    Height = trackHeight,
                    LineHeight = @$"{trackHeight}px",
                    VerticalAlign = "middle",
                    Background = token.ColorTextQuaternary,
                    Border = "0",
                    BorderRadius = 100,
                    Cursor = "pointer",
                    Transition = @$"all {token.MotionDurationMid}",
                    UserSelect = "none",
                    [$"&:hover:not({componentCls}-disabled)"] = new CSSObject()
                    {
                        Background = token.ColorTextTertiary,
                    },
                    ["..."] = GenFocusStyle(token),
                    [$"&{componentCls}-checked"] = new CSSObject()
                    {
                        Background = token.SwitchColor,
                        [$"&:hover:not({componentCls}-disabled)"] = new CSSObject()
                        {
                            Background = token.ColorPrimaryHover,
                        },
                    },
                    [$"&{componentCls}-loading, &{componentCls}-disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        Opacity = token.SwitchDisabledOpacity,
                        ["*"] = new CSSObject()
                        {
                            BoxShadow = "none",
                            Cursor = "not-allowed",
                        },
                    },
                    [$"&{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Switch",
                (token) =>
                {
                    var switchToken = MergeToken(
                        token,
                        new SwitchToken()
                        {
                            SwitchDuration = token.MotionDurationMid,
                            SwitchColor = token.ColorPrimary,
                            SwitchDisabledOpacity = token.OpacityLoading,
                            SwitchLoadingIconSize = token.FontSizeIcon * 0.75,
                            SwitchLoadingIconColor = @$"rgba(0, 0, 0, {token.OpacityLoading})",
                            SwitchHandleActiveInset = "-30%",
                        });
                    return new CSSInterpolation[]
                    {
                        GenSwitchStyle(switchToken),
                        GenSwitchInnerStyle(switchToken),
                        GenSwitchHandleStyle(switchToken),
                        GenSwitchLoadingStyle(switchToken),
                        GenSwitchSmallStyle(switchToken),
                    };
                },
                (token) =>
                {
                    var fontSize = token.FontSize;
                    var lineHeight = token.LineHeight;
                    var controlHeight = token.ControlHeight;
                    var colorWhite = token.ColorWhite;
                    var height = fontSize * lineHeight;
                    var heightSM = controlHeight / 2;
                    var padding = 2;
                    var handleSize = height - padding * 2;
                    var handleSizeSM = heightSM - padding * 2;
                    return new SwitchToken()
                    {
                        TrackHeight = height,
                        TrackHeightSM = heightSM,
                        TrackMinWidth = handleSize * 2 + padding * 4,
                        TrackMinWidthSM = handleSizeSM * 2 + padding * 2,
                        TrackPadding = padding,
                        HandleBg = colorWhite,
                        HandleSize = handleSize,
                        HandleSizeSM = handleSizeSM,
                        // HandleShadow = @$"0 2px 4px 0 {new TinyColor("#00230b").setAlpha(0.2).toRgbString()}",
                        HandleShadow = "",
                        InnerMinMargin = handleSize / 2,
                        InnerMaxMargin = handleSize + padding + padding * 2,
                        InnerMinMarginSM = handleSizeSM / 2,
                        InnerMaxMarginSM = handleSizeSM + padding + padding * 2,
                    };
                });
        }

    }

}
