using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SpinToken
    {
        public int ContentHeight { get; set; }

    }

    public partial class SpinToken : TokenWithCommonCls
    {
        public string SpinDotDefault { get; set; }

        public int SpinDotSize { get; set; }

        public int SpinDotSizeSM { get; set; }

        public int SpinDotSizeLG { get; set; }

    }

    public partial class Spin
    {
        private Keyframes antSpinMove = new Keyframes("antSpinMove")
        {
            To = new Keyframes()
            {
                Opacity = 1,
            },
        };

        private Keyframes antRotate = new Keyframes("antRotate")
        {
            To = new Keyframes()
            {
                Transform = "rotate(405deg)",
            },
        };

        public CSSObject GenSpinStyle(SpinToken token)
        {
            return new CSSObject()
            {
                [$"{token.ComponentCls}"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "absolute",
                    Display = "none",
                    Color = token.ColorPrimary,
                    FontSize = 0,
                    TextAlign = "center",
                    VerticalAlign = "middle",
                    Opacity = 0,
                    Transition = @$"transform {token.MotionDurationSlow} {token.MotionEaseInOutCirc}",
                    ["&-spinning"] = new CSSObject()
                    {
                        Position = "static",
                        Display = "inline-block",
                        Opacity = 1,
                    },
                    ["&-nested-loading"] = new CSSObject()
                    {
                        Position = "relative",
                        [$"> div > {token.ComponentCls}"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = 0,
                            ZIndex = 4,
                            Display = "block",
                            Width = "100%",
                            Height = "100%",
                            MaxHeight = token.ContentHeight,
                            [$"{token.ComponentCls}-dot"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = "50%",
                                InsetInlineStart = "50%",
                                Margin = -token.SpinDotSize / 2,
                            },
                            [$"{token.ComponentCls}-text"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = "50%",
                                Width = "100%",
                                PaddingTop = (token.SpinDotSize - token.FontSize) / 2 + 2,
                                TextShadow = @$"0 1px 2px {token.ColorBgContainer}",
                                FontSize = token.FontSize,
                            },
                            [$"&{token.ComponentCls}-show-text {token.ComponentCls}-dot"] = new CSSObject()
                            {
                                MarginTop = -(token.SpinDotSize / 2) - 10,
                            },
                            ["&-sm"] = new CSSObject()
                            {
                                [$"{token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    Margin = -token.SpinDotSizeSM / 2,
                                },
                                [$"{token.ComponentCls}-text"] = new CSSObject()
                                {
                                    PaddingTop = (token.SpinDotSizeSM - token.FontSize) / 2 + 2,
                                },
                                [$"&{token.ComponentCls}-show-text {token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    MarginTop = -(token.SpinDotSizeSM / 2) - 10,
                                },
                            },
                            ["&-lg"] = new CSSObject()
                            {
                                [$"{token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    Margin = -(token.SpinDotSizeLG / 2),
                                },
                                [$"{token.ComponentCls}-text"] = new CSSObject()
                                {
                                    PaddingTop = (token.SpinDotSizeLG - token.FontSize) / 2 + 2,
                                },
                                [$"&{token.ComponentCls}-show-text {token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    MarginTop = -(token.SpinDotSizeLG / 2) - 10,
                                },
                            },
                        },
                        [$"{token.ComponentCls}-container"] = new CSSObject()
                        {
                            Position = "relative",
                            Transition = @$"opacity {token.MotionDurationSlow}",
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = 0,
                                InsetInlineEnd = 0,
                                Bottom = 0,
                                InsetInlineStart = 0,
                                ZIndex = 10,
                                Width = "100%",
                                Height = "100%",
                                Background = token.ColorBgContainer,
                                Opacity = 0,
                                Transition = @$"all {token.MotionDurationSlow}",
                                Content = "\"\"",
                                PointerEvents = "none",
                            },
                        },
                        [$"{token.ComponentCls}-blur"] = new CSSObject()
                        {
                            Clear = "both",
                            Opacity = 0.5f,
                            UserSelect = "none",
                            PointerEvents = "none",
                            ["&::after"] = new CSSObject()
                            {
                                Opacity = 0.4f,
                                PointerEvents = "auto",
                            },
                        },
                    },
                    ["&-tip"] = new CSSObject()
                    {
                        Color = token.SpinDotDefault,
                    },
                    [$"{token.ComponentCls}-dot"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "inline-block",
                        FontSize = token.SpinDotSize,
                        Width = "1em",
                        Height = "1em",
                        ["&-item"] = new CSSObject()
                        {
                            Position = "absolute",
                            Display = "block",
                            Width = (token.SpinDotSize - token.MarginXXS / 2) / 2,
                            Height = (token.SpinDotSize - token.MarginXXS / 2) / 2,
                            BackgroundColor = token.ColorPrimary,
                            BorderRadius = "100%",
                            Transform = "scale(0.75)",
                            TransformOrigin = "50% 50%",
                            Opacity = 0.3f,
                            AnimationName = antSpinMove,
                            AnimationDuration = "1s",
                            AnimationIterationCount = "infinite",
                            AnimationTimingFunction = "linear",
                            AnimationDirection = "alternate",
                            ["&:nth-child(1)"] = new CSSObject()
                            {
                                Top = 0,
                                InsetInlineStart = 0,
                            },
                            ["&:nth-child(2)"] = new CSSObject()
                            {
                                Top = 0,
                                InsetInlineEnd = 0,
                                AnimationDelay = "0.4s",
                            },
                            ["&:nth-child(3)"] = new CSSObject()
                            {
                                InsetInlineEnd = 0,
                                Bottom = 0,
                                AnimationDelay = "0.8s",
                            },
                            ["&:nth-child(4)"] = new CSSObject()
                            {
                                Bottom = 0,
                                InsetInlineStart = 0,
                                AnimationDelay = "1.2s",
                            },
                        },
                        ["&-spin"] = new CSSObject()
                        {
                            Transform = "rotate(45deg)",
                            AnimationName = antRotate,
                            AnimationDuration = "1.2s",
                            AnimationIterationCount = "infinite",
                            AnimationTimingFunction = "linear",
                        },
                    },
                    [$"&-sm {token.ComponentCls}-dot"] = new CSSObject()
                    {
                        FontSize = token.SpinDotSizeSM,
                        I = new CSSObject()
                        {
                            Width = (token.SpinDotSizeSM - token.MarginXXS / 2) / 2,
                            Height = (token.SpinDotSizeSM - token.MarginXXS / 2) / 2,
                        },
                    },
                    [$"&-lg {token.ComponentCls}-dot"] = new CSSObject()
                    {
                        FontSize = token.SpinDotSizeLG,
                        I = new CSSObject()
                        {
                            Width = (token.SpinDotSizeLG - token.MarginXXS) / 2,
                            Height = (token.SpinDotSizeLG - token.MarginXXS) / 2,
                        },
                    },
                    [$"&{token.ComponentCls}-show-text {token.ComponentCls}-text"] = new CSSObject()
                    {
                        Display = "block",
                    },
                },
            };
        }

        public Unknown_1 GenComponentStyleHook(Unknown_2 token)
        {
            var spinToken = MergeToken(
                token,
                new Unknown_3()
                {
                    SpinDotDefault = token.ColorTextDescription,
                    SpinDotSize = token.ControlHeightLG / 2,
                    SpinDotSizeSM = token.ControlHeightLG * 0.35,
                    SpinDotSizeLG = token.ControlHeight,
                });
            return new Unknown_4 { GenSpinStyle(spinToken) };
        }

    }

}