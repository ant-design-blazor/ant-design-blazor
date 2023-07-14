using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SliderToken
    {
        public int ControlSize { get; set; }

        public int RailSize { get; set; }

        public int HandleSize { get; set; }

        public int HandleSizeHover { get; set; }

        public int HandleLineWidth { get; set; }

        public int HandleLineWidthHover { get; set; }

        public int DotSize { get; set; }

    }

    public partial class SliderToken : TokenWithCommonCls
    {
        public int MarginFull { get; set; }

        public int MarginPart { get; set; }

        public int MarginPartWithMark { get; set; }

    }

    public partial class Slider
    {
        public Unknown_1 GenBaseStyle(Unknown_6 token)
        {
            var componentCls = token.ComponentCls;
            var controlSize = token.ControlSize;
            var dotSize = token.DotSize;
            var marginFull = token.MarginFull;
            var marginPart = token.MarginPart;
            var colorFillContentHover = token.ColorFillContentHover;
            return new Unknown_7()
            {
                [componentCls] = new Unknown_8()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Height = controlSize,
                    Margin = @$"{marginPart}px {marginFull}px",
                    Padding = 0,
                    Cursor = "pointer",
                    TouchAction = "none",
                    ["&-vertical"] = new Unknown_9()
                    {
                        Margin = @$"{marginFull}px {marginPart}px",
                    },
                    [$"{componentCls}-rail"] = new Unknown_10()
                    {
                        Position = "absolute",
                        BackgroundColor = token.ColorFillTertiary,
                        BorderRadius = token.BorderRadiusXS,
                        Transition = @$"background-color {token.MotionDurationMid}",
                    },
                    [$"{componentCls}-track"] = new Unknown_11()
                    {
                        Position = "absolute",
                        BackgroundColor = token.ColorPrimaryBorder,
                        BorderRadius = token.BorderRadiusXS,
                        Transition = @$"background-color {token.MotionDurationMid}",
                    },
                    ["&:hover"] = new Unknown_12()
                    {
                        [$"{componentCls}-rail"] = new Unknown_13()
                        {
                            BackgroundColor = token.ColorFillSecondary,
                        },
                        [$"{componentCls}-track"] = new Unknown_14()
                        {
                            BackgroundColor = token.ColorPrimaryBorderHover,
                        },
                        [$"{componentCls}-dot"] = new Unknown_15()
                        {
                            BorderColor = colorFillContentHover,
                        },
                        [$"{componentCls}-handle::after"] = new Unknown_16()
                        {
                            BoxShadow = @$"0 0 0 {token.HandleLineWidth}px {token.ColorPrimaryBorderHover}",
                        },
                        [$"{componentCls}-dot-active"] = new Unknown_17()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                    },
                    [$"{componentCls}-handle"] = new Unknown_18()
                    {
                        Position = "absolute",
                        Width = token.HandleSize,
                        Height = token.HandleSize,
                        Outline = "none",
                        [$"{componentCls}-dragging"] = new Unknown_19()
                        {
                            ZIndex = 1,
                        },
                        ["&::before"] = new Unknown_20()
                        {
                            Content = "\"\"",
                            Position = "absolute",
                            InsetInlineStart = -token.HandleLineWidth,
                            InsetBlockStart = -token.HandleLineWidth,
                            Width = token.HandleSize + token.HandleLineWidth * 2,
                            Height = token.HandleSize + token.HandleLineWidth * 2,
                            BackgroundColor = "transparent",
                        },
                        ["&::after"] = new Unknown_21()
                        {
                            Content = "\"\"",
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineStart = 0,
                            Width = token.HandleSize,
                            Height = token.HandleSize,
                            BackgroundColor = token.ColorBgElevated,
                            BoxShadow = @$"0 0 0 {token.HandleLineWidth}px {token.ColorPrimaryBorder}",
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
                        ["&:hover, &:active, &:focus"] = new Unknown_22()
                        {
                            ["&::before"] = new Unknown_23()
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
                            ["&::after"] = new Unknown_24()
                            {
                                BoxShadow = @$"0 0 0 {token.HandleLineWidthHover}px {token.ColorPrimary}",
                                Width = token.HandleSizeHover,
                                Height = token.HandleSizeHover,
                                InsetInlineStart = (token.HandleSize - token.HandleSizeHover) / 2,
                                InsetBlockStart = (token.HandleSize - token.HandleSizeHover) / 2,
                            },
                        },
                    },
                    [$"{componentCls}-mark"] = new Unknown_25()
                    {
                        Position = "absolute",
                        FontSize = token.FontSize,
                    },
                    [$"{componentCls}-mark-text"] = new Unknown_26()
                    {
                        Position = "absolute",
                        Display = "inline-block",
                        Color = token.ColorTextDescription,
                        TextAlign = "center",
                        WordBreak = "keep-all",
                        Cursor = "pointer",
                        UserSelect = "none",
                        ["&-active"] = new Unknown_27()
                        {
                            Color = token.ColorText,
                        },
                    },
                    [$"{componentCls}-step"] = new Unknown_28()
                    {
                        Position = "absolute",
                        Background = "transparent",
                        PointerEvents = "none",
                    },
                    [$"{componentCls}-dot"] = new Unknown_29()
                    {
                        Position = "absolute",
                        Width = dotSize,
                        Height = dotSize,
                        BackgroundColor = token.ColorBgElevated,
                        Border = @$"{token.HandleLineWidth}px solid {token.ColorBorderSecondary}",
                        BorderRadius = "50%",
                        Cursor = "pointer",
                        Transition = @$"border-color {token.MotionDurationSlow}",
                        PointerEvents = "auto",
                        ["&-active"] = new Unknown_30()
                        {
                            BorderColor = token.ColorPrimaryBorder,
                        },
                    },
                    [$"&{componentCls}-disabled"] = new Unknown_31()
                    {
                        Cursor = "not-allowed",
                        [$"{componentCls}-rail"] = new Unknown_32()
                        {
                            BackgroundColor = @$"{token.ColorFillSecondary} !important",
                        },
                        [$"{componentCls}-track"] = new Unknown_33()
                        {
                            BackgroundColor = @$"{token.ColorTextDisabled} !important",
                        },
                        [$"{componentCls}-dot"] = new Unknown_34()
                        {
                            BackgroundColor = token.ColorBgElevated,
                            BorderColor = token.ColorTextDisabled,
                            BoxShadow = "none",
                            Cursor = "not-allowed",
                        },
                        [$"{componentCls}-handle::after"] = new Unknown_35()
                        {
                            BackgroundColor = token.ColorBgElevated,
                            Cursor = "not-allowed",
                            Width = token.HandleSize,
                            Height = token.HandleSize,
                            BoxShadow = @$"0 0 0 {token.HandleLineWidth}px {new TinyColor(token.ColorTextDisabled)
            .onBackground(token.ColorBgContainer)
            .toHexShortString()}",
                            InsetInlineStart = 0,
                            InsetBlockStart = 0,
                        },
                        [$"{componentCls}-mark-text,{componentCls}-dot"] = new Unknown_36()
                        {
                            Cursor = @$"not-allowed !important",
                        },
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
            return new CSSObject()
            {
                [railPadding] = railSize,
                [part] = railSize * 3,
                [$"{componentCls}-rail"] = new CSSObject()
                {
                    [full] = "100%",
                    [part] = railSize,
                },
                [$"{componentCls}-track"] = new CSSObject()
                {
                    [part] = railSize,
                },
                [$"{componentCls}-handle"] = new CSSObject()
                {
                    [handlePos] = (railSize * 3 - handleSize) / 2,
                },
                [$"{componentCls}-mark"] = new CSSObject()
                {
                    InsetInlineStart = 0,
                    Top = 0,
                    [markInset] = handleSize,
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

        public Unknown_2 GenHorizontalStyle(Unknown_37 token)
        {
            var componentCls = token.ComponentCls;
            var marginPartWithMark = token.MarginPartWithMark;
            return new Unknown_38()
            {
                [$"{componentCls}-horizontal"] = new Unknown_39()
                {
                    ["..."] = GenDirectionStyle(token, true),
                    [$"&{componentCls}-with-marks"] = new Unknown_40()
                    {
                        MarginBottom = marginPartWithMark,
                    },
                },
            };
        }

        public Unknown_3 GenVerticalStyle(Unknown_41 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_42()
            {
                [$"{componentCls}-vertical"] = new Unknown_43()
                {
                    ["..."] = GenDirectionStyle(token, false),
                    Height = "100%",
                },
            };
        }

        public Unknown_4 GenComponentStyleHook(Unknown_44 token)
        {
            var sliderToken = MergeToken(
                token,
                new Unknown_45()
                {
                    MarginPart = (token.ControlHeight - token.ControlSize) / 2,
                    MarginFull = token.ControlSize / 2,
                    MarginPartWithMark = token.ControlHeightLG - token.ControlSize,
                });
            return new Unknown_46
            {
                GenBaseStyle(sliderToken),
                GenHorizontalStyle(sliderToken),
                GenVerticalStyle(sliderToken)
            };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_47 token)
        {
            var increaseHandleWidth = 1;
            var controlSize = token.ControlHeightLG / 4;
            var controlSizeHover = token.ControlHeightSM / 2;
            var handleLineWidth = token.LineWidth + increaseHandleWidth;
            var handleLineWidthHover = token.LineWidth + increaseHandleWidth * 3;
            return new Unknown_48()
            {
                ControlSize = controlSize,
                RailSize = 4,
                HandleSize = controlSize,
                HandleSizeHover = controlSizeHover,
                DotSize = 8,
                HandleLineWidth = handleLineWidth,
                HandleLineWidthHover = handleLineWidthHover,
            };
        }

    }

}