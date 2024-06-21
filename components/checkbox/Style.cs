using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class CheckboxToken : TokenWithCommonCls
    {
    }

    public partial class CheckboxToken
    {
        public string CheckboxCls
        {
            get => (string)_tokens["checkboxCls"];
            set => _tokens["checkboxCls"] = value;
        }

        public double CheckboxSize
        {
            get => (double)_tokens["checkboxSize"];
            set => _tokens["checkboxSize"] = value;
        }

    }

    public partial class Checkbox
    {
        public CSSInterpolation[] GenCheckboxStyle(CheckboxToken token)
        {
            var checkboxCls = token.CheckboxCls;
            var wrapperCls = @$"{checkboxCls}-wrapper";
            return new CSSInterpolation[]
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
                        BorderRadius = token.BorderRadiusSM,
                        AlignSelf = "center",
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
                    },
                    [$"{wrapperCls}-checked:not({wrapperCls}-disabled),{checkboxCls}-checked:not({checkboxCls}-disabled)"] = new CSSObject()
                    {
                        [$"&:hover {checkboxCls}-inner"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorPrimaryHover,
                            BorderColor = "transparent",
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
                                BackgroundColor = token.ColorBgContainer,
                                BorderColor = token.ColorBorder,
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

        public CSSInterpolation[] GetStyle(string prefixCls, TokenWithCommonCls token)
        {
            var checkboxToken = MergeToken(
                token,
                new CheckboxToken()
                {
                    CheckboxCls = @$".{prefixCls}",
                    CheckboxSize = token.ControlInteractiveSize,
                });
            return new CSSInterpolation[]
            {
                GenCheckboxStyle(checkboxToken),
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Checkbox",
                (token) =>
                {
                    var prefixCls = token.PrefixCls;
                    return new CSSInterpolation[]
                    {
                        GetStyle(prefixCls, token),
                    };
                });
        }

    }

}