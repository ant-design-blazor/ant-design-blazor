using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class SliderToken
    {
        public double ControlSize
        {
            get => (double)_tokens["controlSize"];
            set => _tokens["controlSize"] = value;
        }

        public double RailSize
        {
            get => (double)_tokens["railSize"];
            set => _tokens["railSize"] = value;
        }

        public double HandleSize
        {
            get => (double)_tokens["handleSize"];
            set => _tokens["handleSize"] = value;
        }

        public double HandleSizeHover
        {
            get => (double)_tokens["handleSizeHover"];
            set => _tokens["handleSizeHover"] = value;
        }

        public double HandleLineWidth
        {
            get => (double)_tokens["handleLineWidth"];
            set => _tokens["handleLineWidth"] = value;
        }

        public double HandleLineWidthHover
        {
            get => (double)_tokens["handleLineWidthHover"];
            set => _tokens["handleLineWidthHover"] = value;
        }

        public double DotSize
        {
            get => (double)_tokens["dotSize"];
            set => _tokens["dotSize"] = value;
        }

        public string RailBg
        {
            get => (string)_tokens["railBg"];
            set => _tokens["railBg"] = value;
        }

        public string RailHoverBg
        {
            get => (string)_tokens["railHoverBg"];
            set => _tokens["railHoverBg"] = value;
        }

        public string TrackBg
        {
            get => (string)_tokens["trackBg"];
            set => _tokens["trackBg"] = value;
        }

        public string TrackHoverBg
        {
            get => (string)_tokens["trackHoverBg"];
            set => _tokens["trackHoverBg"] = value;
        }

        public string HandleColor
        {
            get => (string)_tokens["handleColor"];
            set => _tokens["handleColor"] = value;
        }

        public string HandleActiveColor
        {
            get => (string)_tokens["handleActiveColor"];
            set => _tokens["handleActiveColor"] = value;
        }

        public string DotBorderColor
        {
            get => (string)_tokens["dotBorderColor"];
            set => _tokens["dotBorderColor"] = value;
        }

        public string DotActiveBorderColor
        {
            get => (string)_tokens["dotActiveBorderColor"];
            set => _tokens["dotActiveBorderColor"] = value;
        }

        public string TrackBgDisabled
        {
            get => (string)_tokens["trackBgDisabled"];
            set => _tokens["trackBgDisabled"] = value;
        }

    }

    public partial class SliderToken : TokenWithCommonCls
    {
        public double MarginFull
        {
            get => (double)_tokens["marginFull"];
            set => _tokens["marginFull"] = value;
        }

        public double MarginPart
        {
            get => (double)_tokens["marginPart"];
            set => _tokens["marginPart"] = value;
        }

        public double MarginPartWithMark
        {
            get => (double)_tokens["marginPartWithMark"];
            set => _tokens["marginPartWithMark"] = value;
        }

    }

    public partial class Slider<TValue>
    {
        public CSSObject GenBaseStyle(SliderToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var controlSize = token.ControlSize;
            var dotSize = token.DotSize;
            var marginFull = token.MarginFull;
            var marginPart = token.MarginPart;
            var colorFillContentHover = token.ColorFillContentHover;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Height = controlSize,
                    Margin = @$"{marginPart}px {marginFull}px",
                    Padding = 0,
                    Cursor = "pointer",
                    TouchAction = "none",
                    ["&-vertical"] = new CSSObject()
                    {
                        Margin = @$"{marginFull}px {marginPart}px",
                    },
                    [$"{componentCls}-rail"] = new CSSObject()
                    {
                        Position = "absolute",
                        BackgroundColor = token.RailBg,
                        BorderRadius = token.BorderRadiusXS,
                        Transition = @$"background-color {token.MotionDurationMid}",
                    },
                    [$"{componentCls}-track,{componentCls}-tracks"] = new CSSObject()
                    {
                        Position = "absolute",
                        Transition = @$"background-color {token.MotionDurationMid}",
                    },
                    [$"{componentCls}-track"] = new CSSObject()
                    {
                        BackgroundColor = token.TrackBg,
                        BorderRadius = token.BorderRadiusXS,
                    },
                    [$"{componentCls}-track-draggable"] = new CSSObject()
                    {
                        BoxSizing = "content-box",
                        BackgroundClip = "content-box",
                        Border = "solid rgba(0,0,0,0)",
                    },
                    ["&:hover"] = new CSSObject()
                    {
                        [$"{componentCls}-rail"] = new CSSObject()
                        {
                            BackgroundColor = token.RailHoverBg,
                        },
                        [$"{componentCls}-track"] = new CSSObject()
                        {
                            BackgroundColor = token.TrackHoverBg,
                        },
                        [$"{componentCls}-dot"] = new CSSObject()
                        {
                            BorderColor = colorFillContentHover,
                        },
                        [$"{componentCls}-handle::after"] = new CSSObject()
                        {
                            BoxShadow = @$"0 0 0 {token.HandleLineWidth}px {token.ColorPrimaryBorderHover}",
                        },
                        [$"{componentCls}-dot-active"] = new CSSObject()
                        {
                            BorderColor = token.DotActiveBorderColor,
                        },
                    },
                    [$"{componentCls}-handle"] = new CSSObject()
                    {
                        Position = "absolute",
                        Width = token.HandleSize,
                        Height = token.HandleSize,
                        Outline = "none",
                        ["&::before"] = new CSSObject()
                        {
                            Content = "\"\"",
                            Position = "absolute",
                            InsetInlineStart = -token.HandleLineWidth,
                            InsetBlockStart = -token.HandleLineWidth,
                            Width = token.HandleSize + token.HandleLineWidth * 2,
                            Height = token.HandleSize + token.HandleLineWidth * 2,
                            BackgroundColor = "transparent",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Content = "\"\"",
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineStart = 0,
                            Width = token.HandleSize,
                            Height = token.HandleSize,
                            BackgroundColor = token.ColorBgElevated,
                            BoxShadow = @$"0 0 0 {token.HandleLineWidth}px {token.HandleColor}",
                            BorderRadius = "50%",
                            Cursor = "pointer",
                            Transition = @$"
            inset-inline-start {token.MotionDurationMid},
            inset-block-start {token.MotionDurationMid},
            width {token.MotionDurationMid},
            height {token.MotionDurationMid},
            box-shadow {token.MotionDurationMid}
          ",
                        },
                        ["&:hover, &:active, &:focus"] = new CSSObject()
                        {
                            ["&::before"] = new CSSObject()
                            {
                                InsetInlineStart = -(
              (token.HandleSizeHover - token.HandleSize) / 2 +
              token.HandleLineWidthHover
            ),
                                InsetBlockStart = -(
              (token.HandleSizeHover - token.HandleSize) / 2 +
              token.HandleLineWidthHover
            ),
                                Width = token.HandleSizeHover + token.HandleLineWidthHover * 2,
                                Height = token.HandleSizeHover + token.HandleLineWidthHover * 2,
                            },
                            ["&::after"] = new CSSObject()
                            {
                                BoxShadow = @$"0 0 0 {token.HandleLineWidthHover}px {token.HandleActiveColor}",
                                Width = token.HandleSizeHover,
                                Height = token.HandleSizeHover,
                                InsetInlineStart = (token.HandleSize - token.HandleSizeHover) / 2,
                                InsetBlockStart = (token.HandleSize - token.HandleSizeHover) / 2,
                            },
                        },
                    },
                    [$"{componentCls}-mark"] = new CSSObject()
                    {
                        Position = "absolute",
                        FontSize = token.FontSize,
                    },
                    [$"{componentCls}-mark-text"] = new CSSObject()
                    {
                        Position = "absolute",
                        Display = "inline-block",
                        Color = token.ColorTextDescription,
                        TextAlign = "center",
                        WordBreak = "keep-all",
                        Cursor = "pointer",
                        UserSelect = "none",
                        ["&-active"] = new CSSObject()
                        {
                            Color = token.ColorText,
                        },
                    },
                    [$"{componentCls}-step"] = new CSSObject()
                    {
                        Position = "absolute",
                        Background = "transparent",
                        PointerEvents = "none",
                    },
                    [$"{componentCls}-dot"] = new CSSObject()
                    {
                        Position = "absolute",
                        Width = dotSize,
                        Height = dotSize,
                        BackgroundColor = token.ColorBgElevated,
                        Border = @$"{token.HandleLineWidth}px solid {token.DotBorderColor}",
                        BorderRadius = "50%",
                        Cursor = "pointer",
                        Transition = @$"border-color {token.MotionDurationSlow}",
                        PointerEvents = "auto",
                        ["&-active"] = new CSSObject()
                        {
                            BorderColor = token.DotActiveBorderColor,
                        },
                    },
                    [$"&{componentCls}-disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        [$"{componentCls}-rail"] = new CSSObject()
                        {
                            BackgroundColor = @$"{token.RailBg} !important",
                        },
                        [$"{componentCls}-track"] = new CSSObject()
                        {
                            BackgroundColor = @$"{token.TrackBgDisabled} !important",
                        },
                        [$"{componentCls}-dot"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgElevated,
                            BorderColor = token.TrackBgDisabled,
                            BoxShadow = "none",
                            Cursor = "not-allowed",
                        },
                        [$"{componentCls}-handle::after"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgElevated,
                            Cursor = "not-allowed",
                            Width = token.HandleSize,
                            Height = token.HandleSize,
                            BoxShadow = "",
            //                BoxShadow = @$"0 0 0 {token.HandleLineWidth}px {new TinyColor(token.ColorTextDisabled)
            //.onBackground(token.ColorBgContainer)
            //.toHexShortString()}",
                            InsetInlineStart = 0,
                            InsetBlockStart = 0,
                        },
                        [$"{componentCls}-mark-text,{componentCls}-dot"] = new CSSObject()
                        {
                            Cursor = @$"not-allowed !important",
                        },
                    },
                    [$"&-tooltip {antCls}-tooltip-inner"] = new CSSObject()
                    {
                        MinWidth = "unset",
                    },
                },
            };
        }

        public CSSObject GenDirectionStyle(SliderToken token, bool horizontal)
        {
            var componentCls = token.ComponentCls;
            var railSize = token.RailSize;
            var handleSize = token.HandleSize;
            var dotSize = token.DotSize;
            var railPadding = horizontal ? "paddingBlock" : "paddingInline";
            var full = horizontal ? "width" : "height";
            var part = horizontal ? "height" : "width";
            var handlePos = horizontal ? "insetBlockStart" : "insetInlineStart";
            var markInset = horizontal ? "top" : "insetInlineStart";
            var handlePosSize = (railSize * 3 - handleSize) / 2;
            var draggableBorderSize = (handleSize - railSize) / 2;
            var draggableBorder = horizontal
            ? new CSSObject()
            {
                BorderWidth = @$"{draggableBorderSize}px 0",
                Transform = @$"translateY(-{draggableBorderSize}px)",
            }
            : new CSSObject()
            {
                BorderWidth = @$"0 {draggableBorderSize}px",
                Transform = @$"translateX(-{draggableBorderSize}px)",
            };
            return new CSSObject()
            {
                [railPadding] = railSize,
                [part] = railSize * 3,
                [$"{componentCls}-rail"] = new CSSObject()
                {
                    [full] = "100%",
                    [part] = railSize,
                },
                [$"{componentCls}-track,{componentCls}-tracks"] = new CSSObject()
                {
                    [part] = railSize,
                },
                [$"{componentCls}-track-draggable"] = new CSSObject()
                {
                    ["..."] = draggableBorder,
                },
                [$"{componentCls}-handle"] = new CSSObject()
                {
                    [handlePos] = handlePosSize,
                },
                [$"{componentCls}-mark"] = new CSSObject()
                {
                    InsetInlineStart = 0,
                    Top = 0,
                    [markInset] = railSize * 3 + (horizontal ? 0 : token.MarginFull),
                    [full] = "100%",
                },
                [$"{componentCls}-step"] = new CSSObject()
                {
                    InsetInlineStart = 0,
                    Top = 0,
                    [markInset] = railSize,
                    [full] = "100%",
                    [part] = railSize,
                },
                [$"{componentCls}-dot"] = new CSSObject()
                {
                    Position = "absolute",
                    [handlePos] = (railSize - dotSize) / 2,
                },
            };
        }

        public CSSObject GenHorizontalStyle(SliderToken token)
        {
            var componentCls = token.ComponentCls;
            var marginPartWithMark = token.MarginPartWithMark;
            return new CSSObject()
            {
                [$"{componentCls}-horizontal"] = new CSSObject()
                {
                    ["..."] = GenDirectionStyle(token, true),
                    [$"&{componentCls}-with-marks"] = new CSSObject()
                    {
                        MarginBottom = marginPartWithMark,
                    },
                },
            };
        }

        public CSSObject GenVerticalStyle(SliderToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-vertical"] = new CSSObject()
                {
                    ["..."] = GenDirectionStyle(token, false),
                    Height = "100%",
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Slider",
                (token) =>
                {
                    var sliderToken = MergeToken(
                        token,
                        new SliderToken()
                        {
                            MarginPart = (token.ControlHeight - token.ControlSize) / 2,
                            MarginFull = token.ControlSize / 2,
                            MarginPartWithMark = token.ControlHeightLG - token.ControlSize,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(sliderToken),
                        GenHorizontalStyle(sliderToken),
                        GenVerticalStyle(sliderToken),
                    };
                },
                (token) =>
                {
                    var increaseHandleWidth = 1;
                    var controlSize = token.ControlHeightLG / 4;
                    var controlSizeHover = token.ControlHeightSM / 2;
                    var handleLineWidth = token.LineWidth + increaseHandleWidth;
                    var handleLineWidthHover = token.LineWidth + increaseHandleWidth * 3;
                    return new SliderToken()
                    {
                        ControlSize = controlSize,
                        RailSize = 4,
                        HandleSize = controlSize,
                        HandleSizeHover = controlSizeHover,
                        DotSize = 8,
                        HandleLineWidth = handleLineWidth,
                        HandleLineWidthHover = handleLineWidthHover,
                        RailBg = token.ColorFillTertiary,
                        RailHoverBg = token.ColorFillSecondary,
                        TrackBg = token.ColorPrimaryBorder,
                        TrackHoverBg = token.ColorPrimaryBorderHover,
                        HandleColor = token.ColorPrimaryBorder,
                        HandleActiveColor = token.ColorPrimary,
                        DotBorderColor = token.ColorBorderSecondary,
                        DotActiveBorderColor = token.ColorPrimaryBorder,
                        TrackBgDisabled = token.ColorBgContainerDisabled,
                    };
                });
        }

    }

}
