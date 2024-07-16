using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.InputStyle;

namespace AntDesign
{
    public partial class InputNumberToken : InputToken
    {
        public double ControlWidth
        {
            get => (double)_tokens["controlWidth"];
            set => _tokens["controlWidth"] = value;
        }

        public double HandleWidth
        {
            get => (double)_tokens["handleWidth"];
            set => _tokens["handleWidth"] = value;
        }

        public double HandleFontSize
        {
            get => (double)_tokens["handleFontSize"];
            set => _tokens["handleFontSize"] = value;
        }

        public string HandleVisible
        {
            get => (string)_tokens["handleVisible"];
            set => _tokens["handleVisible"] = value;
        }

        public string HandleBg
        {
            get => (string)_tokens["handleBg"];
            set => _tokens["handleBg"] = value;
        }

        public string HandleActiveBg
        {
            get => (string)_tokens["handleActiveBg"];
            set => _tokens["handleActiveBg"] = value;
        }

        public string HandleHoverColor
        {
            get => (string)_tokens["handleHoverColor"];
            set => _tokens["handleHoverColor"] = value;
        }

        public string HandleBorderColor
        {
            get => (string)_tokens["handleBorderColor"];
            set => _tokens["handleBorderColor"] = value;
        }

    }

    public partial class InputNumberToken
    {
    }

    public partial class InputNumberStyle
    {
        public static CSSObject GenRadiusStyle(InputNumberToken args, string size)
        {
            var componentCls = args.ComponentCls;
            var borderRadiusSM = args.BorderRadiusSM;
            var borderRadiusLG = args.BorderRadiusLG;
            var borderRadius = size == "lg" ? borderRadiusLG : borderRadiusSM;
            return new CSSObject()
            {
                [$"&-{size}"] = new CSSObject()
                {
                    [$"{componentCls}-handler-wrap"] = new CSSObject()
                    {
                        BorderStartEndRadius = borderRadius,
                        BorderEndEndRadius = borderRadius,
                    },
                    [$"{componentCls}-handler-up"] = new CSSObject()
                    {
                        BorderStartEndRadius = borderRadius,
                    },
                    [$"{componentCls}-handler-down"] = new CSSObject()
                    {
                        BorderEndEndRadius = borderRadius,
                    },
                },
            };
        }

        public static CSSInterpolation[] GenInputNumberStyles(InputNumberToken token)
        {
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorBorder = token.ColorBorder;
            var borderRadius = token.BorderRadius;
            var fontSizeLG = token.FontSizeLG;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var colorError = token.ColorError;
            var paddingInlineSM = token.PaddingInlineSM;
            var colorTextDescription = token.ColorTextDescription;
            var motionDurationMid = token.MotionDurationMid;
            var handleHoverColor = token.HandleHoverColor;
            var paddingInline = token.PaddingInline;
            var paddingBlock = token.PaddingBlock;
            var handleBg = token.HandleBg;
            var handleActiveBg = token.HandleActiveBg;
            var colorTextDisabled = token.ColorTextDisabled;
            var borderRadiusSM = token.BorderRadiusSM;
            var borderRadiusLG = token.BorderRadiusLG;
            var controlWidth = token.ControlWidth;
            var handleVisible = token.HandleVisible;
            var handleBorderColor = token.HandleBorderColor;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        ["..."] = GenBasicInputStyle(token),
                        ["..."] = GenStatusStyle(token, componentCls),
                        Display = "inline-block",
                        Width = controlWidth,
                        Margin = 0,
                        Padding = 0,
                        Border = @$"{lineWidth}px {lineType} {colorBorder}",
                        BorderRadius = borderRadius,
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                            [$"{componentCls}-input"] = new CSSObject()
                            {
                                Direction = "rtl",
                            },
                        },
                        ["&-lg"] = new CSSObject()
                        {
                            Padding = 0,
                            FontSize = fontSizeLG,
                            BorderRadius = borderRadiusLG,
                            [$"input{componentCls}-input"] = new CSSObject()
                            {
                                Height = controlHeightLG - 2 * lineWidth,
                            },
                        },
                        ["&-sm"] = new CSSObject()
                        {
                            Padding = 0,
                            BorderRadius = borderRadiusSM,
                            [$"input{componentCls}-input"] = new CSSObject()
                            {
                                Height = controlHeightSM - 2 * lineWidth,
                                Padding = @$"0 {paddingInlineSM}px",
                            },
                        },
                        ["&-out-of-range"] = new CSSObject()
                        {
                            [$"{componentCls}-input-wrap"] = new CSSObject()
                            {
                                ["input"] = new CSSObject()
                                {
                                    Color = colorError,
                                },
                            },
                        },
                        ["&-group"] = new CSSObject()
                        {
                            ["..."] = ResetComponent(token),
                            ["..."] = GenInputGroupStyle(token),
                            ["&-wrapper"] = new CSSObject()
                            {
                                Display = "inline-block",
                                TextAlign = "start",
                                VerticalAlign = "top",
                                [$"{componentCls}-affix-wrapper"] = new CSSObject()
                                {
                                    Width = "100%",
                                },
                                ["&-lg"] = new CSSObject()
                                {
                                    [$"{componentCls}-group-addon"] = new CSSObject()
                                    {
                                        BorderRadius = borderRadiusLG,
                                        FontSize = token.FontSizeLG,
                                    },
                                },
                                ["&-sm"] = new CSSObject()
                                {
                                    [$"{componentCls}-group-addon"] = new CSSObject()
                                    {
                                        BorderRadius = borderRadiusSM,
                                    },
                                },
                                [$"{componentCls}-wrapper-disabled > {componentCls}-group-addon"] = new CSSObject()
                                {
                                    ["..."] = GenDisabledStyle(token)
                                },
                            },
                        },
                        [$"&-disabled {componentCls}-input"] = new CSSObject()
                        {
                            Cursor = "not-allowed",
                        },
                        [componentCls] = new CSSObject()
                        {
                            ["&-input"] = new CSSObject()
                            {
                                ["..."] = ResetComponent(token),
                                Width = "100%",
                                Padding = @$"{paddingBlock}px {paddingInline}px",
                                TextAlign = "start",
                                BackgroundColor = "transparent",
                                Border = 0,
                                BorderRadius = borderRadius,
                                Outline = 0,
                                Transition = @$"all {motionDurationMid} linear",
                                Appearance = "textfield",
                                FontSize = "inherit",
                                ["..."] = GenPlaceholderStyle(token.ColorTextPlaceholder),
                                ["&[type=\"number\"]::-webkit-inner-spin-button, &[type=\"number\"]::-webkit-outer-spin-button"] = new CSSObject()
                                {
                                    Margin = 0,
                                    WebkitAppearance = "none",
                                    Appearance = "none",
                                },
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$"&:hover {componentCls}-handler-wrap, &-focused {componentCls}-handler-wrap"] = new CSSObject()
                        {
                            Opacity = 1,
                        },
                        [$"{componentCls}-handler-wrap"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineEnd = 0,
                            Width = token.HandleWidth,
                            Height = "100%",
                            Background = handleBg,
                            BorderStartStartRadius = 0,
                            BorderStartEndRadius = borderRadius,
                            BorderEndEndRadius = borderRadius,
                            BorderEndStartRadius = 0,
                            Opacity = handleVisible == "true" ? 1 : 0,
                            Display = "flex",
                            FlexDirection = "column",
                            AlignItems = "stretch",
                            Transition = @$"opacity {motionDurationMid} linear {motionDurationMid}",
                            [$"{componentCls}-handler"] = new CSSObject()
                            {
                                Display = "flex",
                                AlignItems = "center",
                                JustifyContent = "center",
                                Flex = "auto",
                                Height = "40%",
                                [$"{componentCls}-handler-up-inner,{componentCls}-handler-down-inner"] = new CSSObject()
                                {
                                    MarginInlineEnd = 0,
                                    FontSize = token.HandleFontSize,
                                },
                            },
                        },
                        [$"{componentCls}-handler"] = new CSSObject()
                        {
                            Height = "50%",
                            Overflow = "hidden",
                            Color = colorTextDescription,
                            FontWeight = "bold",
                            LineHeight = 0,
                            TextAlign = "center",
                            Cursor = "pointer",
                            BorderInlineStart = @$"{lineWidth}px {lineType} {handleBorderColor}",
                            Transition = @$"all {motionDurationMid} linear",
                            ["&:active"] = new CSSObject()
                            {
                                Background = handleActiveBg,
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                Height = @$"60%",
                                [$"{componentCls}-handler-up-inner,{componentCls}-handler-down-inner"] = new CSSObject()
                                {
                                    Color = handleHoverColor,
                                },
                            },
                            ["&-up-inner, &-down-inner"] = new CSSObject()
                            {
                                ["..."] = ResetIcon(),
                                Color = colorTextDescription,
                                Transition = @$"all {motionDurationMid} linear",
                                UserSelect = "none",
                            },
                        },
                        [$"{componentCls}-handler-up"] = new CSSObject()
                        {
                            BorderStartEndRadius = borderRadius,
                        },
                        [$"{componentCls}-handler-down"] = new CSSObject()
                        {
                            BorderBlockStart = @$"{lineWidth}px {lineType} {handleBorderColor}",
                            BorderEndEndRadius = borderRadius,
                        },
                        ["..."] = GenRadiusStyle(token, "lg"),
                        ["..."] = GenRadiusStyle(token, "sm"),
                        ["&-disabled, &-readonly"] = new CSSObject()
                        {
                            [$"{componentCls}-handler-wrap"] = new CSSObject()
                            {
                                Display = "none",
                            },
                            [$"{componentCls}-input"] = new CSSObject()
                            {
                                Color = "inherit",
                            },
                        },
                        [$"{componentCls}-handler-up-disabled,{componentCls}-handler-down-disabled"] = new CSSObject()
                        {
                            Cursor = "not-allowed",
                        },
                        [@$"
                            {componentCls}-handler-up-disabled:hover &-handler-up-inner,
                            {componentCls}-handler-down-disabled:hover &-handler-down-inner"] = new CSSObject()
                        {
                            Color = colorTextDisabled,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-borderless"] = new CSSObject()
                    {
                        BorderColor = "transparent",
                        BoxShadow = "none",
                        [$"{componentCls}-handler-down"] = new CSSObject()
                        {
                            BorderBlockStartWidth = 0,
                        },
                    },
                },
            };
        }

        public static CSSObject GenAffixWrapperStyles(InputNumberToken token)
        {
            var componentCls = token.ComponentCls;
            var paddingBlock = token.PaddingBlock;
            var paddingInline = token.PaddingInline;
            var inputAffixPadding = token.InputAffixPadding;
            var controlWidth = token.ControlWidth;
            var borderRadiusLG = token.BorderRadiusLG;
            var borderRadiusSM = token.BorderRadiusSM;
            return new CSSObject()
            {
                [$"{componentCls}-affix-wrapper"] = new CSSObject()
                {
                    ["..."] = GenBasicInputStyle(token),
                    ["..."] = GenStatusStyle(token, $"{componentCls}-affix-wrapper"),
                    Position = "relative",
                    Display = "inline-flex",
                    Width = controlWidth,
                    Padding = 0,
                    PaddingInlineStart = paddingInline,
                    ["&-lg"] = new CSSObject()
                    {
                        BorderRadius = borderRadiusLG,
                    },
                    ["&-sm"] = new CSSObject()
                    {
                        BorderRadius = borderRadiusSM,
                    },
                    [$"&:not({componentCls}-affix-wrapper-disabled):hover"] = new CSSObject()
                    {
                        ZIndex = 1,
                    },
                    ["&-focused, &:focus"] = new CSSObject()
                    {
                        ZIndex = 1,
                    },
                    [$"&-disabled > {componentCls}-disabled"] = new CSSObject()
                    {
                        Background = "transparent",
                    },
                    [$"> div{componentCls}"] = new CSSObject()
                    {
                        Width = "100%",
                        Border = "none",
                        Outline = "none",
                        [$"&{componentCls}-focused"] = new CSSObject()
                        {
                            BoxShadow = "none !important",
                        },
                    },
                    [$"input{componentCls}-input"] = new CSSObject()
                    {
                        Padding = @$"{paddingBlock}px 0",
                    },
                    ["&::before"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = 0,
                        Visibility = "hidden",
                        Content = "\"\\a0\"",
                    },
                    [$"{componentCls}-handler-wrap"] = new CSSObject()
                    {
                        ZIndex = 2,
                    },
                    [componentCls] = new CSSObject()
                    {
                        ["&-prefix, &-suffix"] = new CSSObject()
                        {
                            Display = "flex",
                            Flex = "none",
                            AlignItems = "center",
                            PointerEvents = "none",
                        },
                        ["&-prefix"] = new CSSObject()
                        {
                            MarginInlineEnd = inputAffixPadding,
                        },
                        ["&-suffix"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineEnd = 0,
                            ZIndex = 1,
                            Height = "100%",
                            MarginInlineEnd = paddingInline,
                            MarginInlineStart = inputAffixPadding,
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "InputNumber",
                (token) =>
                {
                    var inputToken = new InputNumberToken();
                    inputToken.Merge(InitInputToken(token));
                    var inputNumberToken = MergeToken<InputNumberToken>(
                        token,
                        inputToken);
                    return new CSSInterpolation[]
                    {
                        GenInputNumberStyles(inputNumberToken),
                        GenAffixWrapperStyles(inputNumberToken),
                        GenCompactItemStyle(inputNumberToken),
                    };
                },
                (token) =>
                {
                    return new InputNumberToken()
                    {
                        ["..."] = InitComponentToken(token),
                        ControlWidth = 90,
                        HandleWidth = token.ControlHeightSM - token.LineWidth * 2,
                        HandleFontSize = token.FontSize / 2,
                        HandleVisible = "auto",
                        HandleActiveBg = token.ColorFillAlter,
                        HandleBg = token.ColorBgContainer,
                        HandleHoverColor = token.ColorPrimary,
                        HandleBorderColor = token.ColorBorder,
                    };
                });
        }

    }

}
