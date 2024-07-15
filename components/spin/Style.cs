using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Keyframes = CssInCSharp.Keyframe;

namespace AntDesign
{
    public partial class SpinToken
    {
        public double ContentHeight
        {
            get => (double)_tokens["contentHeight"];
            set => _tokens["contentHeight"] = value;
        }

        public double DotSize
        {
            get => (double)_tokens["dotSize"];
            set => _tokens["dotSize"] = value;
        }

        public double DotSizeSM
        {
            get => (double)_tokens["dotSizeSM"];
            set => _tokens["dotSizeSM"] = value;
        }

        public double DotSizeLG
        {
            get => (double)_tokens["dotSizeLG"];
            set => _tokens["dotSizeLG"] = value;
        }

    }

    public partial class SpinToken : TokenWithCommonCls
    {
        public string SpinDotDefault
        {
            get => (string)_tokens["spinDotDefault"];
            set => _tokens["spinDotDefault"] = value;
        }

    }

    public partial class SpinStyle
    {
        private static Keyframes _antSpinMove = new Keyframes("antSpinMove",
            new CSSObject()
            {
                ["to"] = new CSSObject()
                {
                    Opacity = 1,
                },
            });

        private static Keyframes _antRotate = new Keyframes("antRotate",
            new CSSObject()
            {
                ["to"] = new CSSObject()
                {
                    Transform = "rotate(405deg)",
                },
            });

        public static double DotPadding(SpinToken token)
        {
            return (token.DotSize - token.FontSize) / 2 + 2;
        }

        public static CSSObject GenSpinStyle(SpinToken token)
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
                    [$"{token.ComponentCls}-text"] = new CSSObject()
                    {
                        FontSize = token.FontSize,
                        PaddingTop = DotPadding(token)
                    },
                    ["&-fullscreen"] = new CSSObject()
                    {
                        Position = "fixed",
                        Width = "100vw",
                        Height = "100vh",
                        BackgroundColor = token.ColorBgMask,
                        ZIndex = token.ZIndexPopupBase,
                        Inset = 0,
                        Display = "flex",
                        AlignItems = "center",
                        FlexDirection = "column",
                        JustifyContent = "center",
                        PointerEvents = "none",
                        Opacity = 0,
                        Visibility = "hidden",
                        Transition = @$"all {token.MotionDurationMid}",
                        ["&-show"] = new CSSObject()
                        {
                            Opacity = 1,
                            Visibility = "visible",
                        },
                        [$"{token.ComponentCls}-dot {token.ComponentCls}-dot-item"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorWhite,
                        },
                        [$"{token.ComponentCls}-text"] = new CSSObject()
                        {
                            Color = token.ColorTextLightSolid,
                        },
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
                                Margin = -token.DotSize / 2,
                            },
                            [$"{token.ComponentCls}-text"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = "50%",
                                Width = "100%",
                                TextShadow = @$"0 1px 2px {token.ColorBgContainer}",
                            },
                            [$"&{token.ComponentCls}-show-text {token.ComponentCls}-dot"] = new CSSObject()
                            {
                                MarginTop = -(token.DotSize / 2) - 10,
                            },
                            ["&-sm"] = new CSSObject()
                            {
                                [$"{token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    Margin = -token.DotSizeSM / 2,
                                },
                                [$"{token.ComponentCls}-text"] = new CSSObject()
                                {
                                    PaddingTop = (token.DotSizeSM - token.FontSize) / 2 + 2,
                                },
                                [$"&{token.ComponentCls}-show-text {token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    MarginTop = -(token.DotSizeSM / 2) - 10,
                                },
                            },
                            ["&-lg"] = new CSSObject()
                            {
                                [$"{token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    Margin = -(token.DotSizeLG / 2),
                                },
                                [$"{token.ComponentCls}-text"] = new CSSObject()
                                {
                                    PaddingTop = (token.DotSizeLG - token.FontSize) / 2 + 2,
                                },
                                [$"&{token.ComponentCls}-show-text {token.ComponentCls}-dot"] = new CSSObject()
                                {
                                    MarginTop = -(token.DotSizeLG / 2) - 10,
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
                            Opacity = 0.5,
                            UserSelect = "none",
                            PointerEvents = "none",
                            ["&::after"] = new CSSObject()
                            {
                                Opacity = 0.4,
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
                        FontSize = token.DotSize,
                        Width = "1em",
                        Height = "1em",
                        ["&-item"] = new CSSObject()
                        {
                            Position = "absolute",
                            Display = "block",
                            Width = (token.DotSize - token.MarginXXS / 2) / 2,
                            Height = (token.DotSize - token.MarginXXS / 2) / 2,
                            BackgroundColor = token.ColorPrimary,
                            BorderRadius = "100%",
                            Transform = "scale(0.75)",
                            TransformOrigin = "50% 50%",
                            Opacity = 0.3,
                            AnimationName = _antSpinMove,
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
                            AnimationName = _antRotate,
                            AnimationDuration = "1.2s",
                            AnimationIterationCount = "infinite",
                            AnimationTimingFunction = "linear",
                        },
                    },
                    [$"&-sm {token.ComponentCls}-dot"] = new CSSObject()
                    {
                        FontSize = token.DotSizeSM,
                        ["i"] = new CSSObject()
                        {
                            Width = (token.DotSizeSM - token.MarginXXS / 2) / 2,
                            Height = (token.DotSizeSM - token.MarginXXS / 2) / 2,
                        },
                    },
                    [$"&-lg {token.ComponentCls}-dot"] = new CSSObject()
                    {
                        FontSize = token.DotSizeLG,
                        ["i"] = new CSSObject()
                        {
                            Width = (token.DotSizeLG - token.MarginXXS) / 2,
                            Height = (token.DotSizeLG - token.MarginXXS) / 2,
                        },
                    },
                    [$"&{token.ComponentCls}-show-text {token.ComponentCls}-text"] = new CSSObject()
                    {
                        Display = "block",
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Spin",
                (token) =>
                {
                    var spinToken = MergeToken(
                        token,
                        new SpinToken()
                        {
                            SpinDotDefault = token.ColorTextDescription,
                        });
                    return new CSSInterpolation[]
                    {
                        GenSpinStyle(spinToken),
                    };
                },
                (token) =>
                {
                    return new SpinToken()
                    {
                        ContentHeight = 400,
                        DotSize = token.ControlHeightLG / 2,
                        DotSizeSM = token.ControlHeightLG * 0.35,
                        DotSizeLG = token.ControlHeight,
                    };
                });
        }

    }

}
