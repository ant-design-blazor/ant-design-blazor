using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using Keyframes = CssInCs.Keyframe;

namespace AntDesign
{
    public partial class CheckboxToken : TokenWithCommonCls
    {
    }

    public partial class CheckboxToken
    {
        public string CheckboxCls { get; set; }

        public int CheckboxSize { get; set; }

    }

    public partial class Checkbox
    {
        private Keyframes antCheckboxEffect = new Keyframes("antCheckboxEffect")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(1)",
                Opacity = 0.5f,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(1.6)",
                Opacity = 0,
            },
        };

        public CSSObject[] GenCheckboxStyle(CheckboxToken token)
        {
            var checkboxCls = token.CheckboxCls;
            var wrapperCls = @$"{checkboxCls}-wrapper";
            return new CSSObject[]
            {
                new CSSObject()
                {
                    [$"{checkboxCls}-group"] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Display = "inline-flex",
                        FlexWrap = "wrap",
                        ColumnGap = token.MarginXS,
                        [$"> {token.AntCls}-row"] = new CSSObject()
                        {
                            Flex = 1,
                        },
                    },
                    [wrapperCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Display = "inline-flex",
                        AlignItems = "baseline",
                        Cursor = "pointer",
                        ["&:after"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = 0,
                            Overflow = "hidden",
                            Content = "'\\a0'",
                        },
                        [$"& + {wrapperCls}"] = new CSSObject()
                        {
                            MarginInlineStart = 0,
                        },
                        [$"&{wrapperCls}-in-form-item"] = new CSSObject()
                        {
                            ["input[type=\"checkbox\"]"] = new CSSObject()
                            {
                                Width = 14,
                                Height = 14,
                            },
                        },
                    },
                    [checkboxCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "relative",
                        WhiteSpace = "nowrap",
                        LineHeight = 1,
                        Cursor = "pointer",
                        AlignSelf = "start",
                        Transform = @$"translate(0, {
          (token.LineHeight * token.FontSize) / 2 - token.CheckboxSize / 2
        }px)",
                        [$"{checkboxCls}-input"] = new CSSObject()
                        {
                            Position = "absolute",
                            Inset = 0,
                            ZIndex = 1,
                            Cursor = "pointer",
                            Opacity = 0,
                            Margin = 0,
                            [$"&:focus-visible + {checkboxCls}-inner"] = new CSSObject()
                            {
                                ["..."] = GenFocusOutline(token)
                            },
                        },
                        [$"{checkboxCls}-inner"] = new CSSObject()
                        {
                            BoxSizing = "border-box",
                            Position = "relative",
                            Top = 0,
                            InsetInlineStart = 0,
                            Display = "block",
                            Width = token.CheckboxSize,
                            Height = token.CheckboxSize,
                            Direction = "ltr",
                            BackgroundColor = token.ColorBgContainer,
                            Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                            BorderRadius = token.BorderRadiusSM,
                            BorderCollapse = "separate",
                            Transition = @$"all {token.MotionDurationSlow}",
                            ["&:after"] = new CSSObject()
                            {
                                BoxSizing = "border-box",
                                Position = "absolute",
                                Top = "50%",
                                InsetInlineStart = "21.5%",
                                Display = "table",
                                Width = (token.CheckboxSize / 14) * 5,
                                Height = (token.CheckboxSize / 14) * 8,
                                Border = @$"{token.LineWidthBold}px solid {token.ColorWhite}",
                                BorderTop = 0,
                                BorderInlineStart = 0,
                                Transform = "rotate(45deg) scale(0) translate(-50%,-50%)",
                                Opacity = 0,
                                Content = "\"\"",
                                Transition = @$"all {token.MotionDurationFast} {token.MotionEaseInBack}, opacity {token.MotionDurationFast}",
                            },
                        },
                        ["& + span"] = new CSSObject()
                        {
                            PaddingInlineStart = token.PaddingXS,
                            PaddingInlineEnd = token.PaddingXS,
                        },
                    },
                },
                new CSSObject()
                {
                    [checkboxCls] = new CSSObject()
                    {
                        ["&-indeterminate"] = new CSSObject()
                        {
                            [$"{checkboxCls}-inner"] = new CSSObject()
                            {
                                ["&:after"] = new CSSObject()
                                {
                                    Top = "50%",
                                    InsetInlineStart = "50%",
                                    Width = token.FontSizeLG / 2,
                                    Height = token.FontSizeLG / 2,
                                    BackgroundColor = token.ColorPrimary,
                                    Border = 0,
                                    Transform = "translate(-50%, -50%) scale(1)",
                                    Opacity = 1,
                                    Content = "\"\"",
                                },
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{wrapperCls}:hover {checkboxCls}:after"] = new CSSObject()
                    {
                        Visibility = "visible",
                    },
                    [$"{wrapperCls}:not({wrapperCls}-disabled),{checkboxCls}:not({checkboxCls}-disabled)"] = new CSSObject()
                    {
                        [$"&:hover {checkboxCls}-inner"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                    },
                    [$"{wrapperCls}:not({wrapperCls}-disabled)"] = new CSSObject()
                    {
                        [$"&:hover {checkboxCls}-checked:not({checkboxCls}-disabled) {checkboxCls}-inner"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorPrimaryHover,
                            BorderColor = "transparent",
                        },
                        [$"&:hover {checkboxCls}-checked:not({checkboxCls}-disabled):after"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{checkboxCls}-checked"] = new CSSObject()
                    {
                        [$"{checkboxCls}-inner"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorPrimary,
                            BorderColor = token.ColorPrimary,
                            ["&:after"] = new CSSObject()
                            {
                                Opacity = 1,
                                Transform = "rotate(45deg) scale(1) translate(-50%,-50%)",
                                Transition = @$"all {token.MotionDurationMid} {token.MotionEaseOutBack} {token.MotionDurationFast}",
                            },
                        },
                        ["&:after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = 0,
                            Width = "100%",
                            Height = "100%",
                            BorderRadius = token.BorderRadiusSM,
                            Visibility = "hidden",
                            Border = @$"{token.LineWidthBold}px solid {token.ColorPrimary}",
                            AnimationName = antCheckboxEffect,
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = "ease-in-out",
                            AnimationFillMode = "backwards",
                            Content = "\"\"",
                            Transition = @$"all {token.MotionDurationSlow}",
                        },
                    },
                    [$"{wrapperCls}-checked:not({wrapperCls}-disabled),{checkboxCls}-checked:not({checkboxCls}-disabled)"] = new CSSObject()
                    {
                        [$"&:hover {checkboxCls}-inner"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorPrimaryHover,
                            BorderColor = "transparent",
                        },
                        [$"&:hover {checkboxCls}:after"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{wrapperCls}-disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                    },
                    [$"{checkboxCls}-disabled"] = new CSSObject()
                    {
                        [$"&, {checkboxCls}-input"] = new CSSObject()
                        {
                            Cursor = "not-allowed",
                            PointerEvents = "none",
                        },
                        [$"{checkboxCls}-inner"] = new CSSObject()
                        {
                            Background = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                            ["&:after"] = new CSSObject()
                            {
                                BorderColor = token.ColorTextDisabled,
                            },
                        },
                        ["&:after"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        ["& + span"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                        },
                        [$"&{checkboxCls}-indeterminate {checkboxCls}-inner::after"] = new CSSObject()
                        {
                            Background = token.ColorTextDisabled,
                        },
                    },
                },
            };
        }

        public CSSObject[] GetStyle(string prefixCls, GlobalToken token)
        {
            var checkboxToken = MergeToken(
                token,
                new CheckboxToken()
                {
                    CheckboxCls = @$".{prefixCls}",
                    CheckboxSize = token.ControlInteractiveSize,
                });
            return GenCheckboxStyle(checkboxToken);
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token, Unknown_52 args)
        {
            return new CSSInterpolation[] { GetStyle(prefixCls, token) };
        }

    }

}