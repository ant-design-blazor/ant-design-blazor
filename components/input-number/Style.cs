using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class InputNumberToken
    {
        public int ControlWidth { get; set; }

        public int HandleWidth { get; set; }

        public int HandleFontSize { get; set; }

        public true | "auto" HandleVisible { get; set; }

    }

    public partial class InputNumberToken : TokenWithCommonCls
    {
    }

    public partial class InputNumber
    {
        public Unknown_1 GenInputNumberStyles(InputNumberToken token)
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
            var inputPaddingHorizontalSM = token.InputPaddingHorizontalSM;
            var colorTextDescription = token.ColorTextDescription;
            var motionDurationMid = token.MotionDurationMid;
            var colorPrimary = token.ColorPrimary;
            var controlHeight = token.ControlHeight;
            var inputPaddingHorizontal = token.InputPaddingHorizontal;
            var colorBgContainer = token.ColorBgContainer;
            var colorTextDisabled = token.ColorTextDisabled;
            var borderRadiusSM = token.BorderRadiusSM;
            var borderRadiusLG = token.BorderRadiusLG;
            var controlWidth = token.ControlWidth;
            var handleVisible = token.HandleVisible;
            return new Unknown_5
            {
                new Unknown_6()
                {
                    [componentCls] = new Unknown_7()
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
                        ["&-rtl"] = new Unknown_8()
                        {
                            Direction = "rtl",
                            [$"{componentCls}-input"] = new Unknown_9()
                            {
                                Direction = "rtl",
                            },
                        },
                        ["&-lg"] = new Unknown_10()
                        {
                            Padding = 0,
                            FontSize = fontSizeLG,
                            BorderRadius = borderRadiusLG,
                            [$"input{componentCls}-input"] = new Unknown_11()
                            {
                                Height = controlHeightLG - 2 * lineWidth,
                            },
                        },
                        ["&-sm"] = new Unknown_12()
                        {
                            Padding = 0,
                            BorderRadius = borderRadiusSM,
                            [$"input{componentCls}-input"] = new Unknown_13()
                            {
                                Height = controlHeightSM - 2 * lineWidth,
                                Padding = @$"0 {inputPaddingHorizontalSM}px",
                            },
                        },
                        ["&:hover"] = new Unknown_14()
                        {
                            ["..."] = GenHoverStyle(token)
                        },
                        ["&-focused"] = new Unknown_15()
                        {
                            ["..."] = GenActiveStyle(token)
                        },
                        ["&-disabled"] = new Unknown_16()
                        {
                            ["..."] = GenDisabledStyle(token),
                            [$"{componentCls}-input"] = new Unknown_17()
                            {
                                Cursor = "not-allowed",
                            },
                        },
                        ["&-out-of-range"] = new Unknown_18()
                        {
                            [$"{componentCls}-input-wrap"] = new Unknown_19()
                            {
                                Input = new Unknown_20()
                                {
                                    Color = colorError,
                                },
                            },
                        },
                        ["&-group"] = new Unknown_21()
                        {
                            ["..."] = ResetComponent(token),
                            ["..."] = GenInputGroupStyle(token),
                            ["&-wrapper"] = new Unknown_22()
                            {
                                Display = "inline-block",
                                TextAlign = "start",
                                VerticalAlign = "top",
                                [$"{componentCls}-affix-wrapper"] = new Unknown_23()
                                {
                                    Width = "100%",
                                },
                                ["&-lg"] = new Unknown_24()
                                {
                                    [$"{componentCls}-group-addon"] = new Unknown_25()
                                    {
                                        BorderRadius = borderRadiusLG,
                                    },
                                },
                                ["&-sm"] = new Unknown_26()
                                {
                                    [$"{componentCls}-group-addon"] = new Unknown_27()
                                    {
                                        BorderRadius = borderRadiusSM,
                                    },
                                },
                            },
                        },
                        [componentCls] = new Unknown_28()
                        {
                            ["&-input"] = new Unknown_29()
                            {
                                ["..."] = ResetComponent(token),
                                Width = "100%",
                                Height = controlHeight - 2 * lineWidth,
                                Padding = @$"0 {inputPaddingHorizontal}px",
                                TextAlign = "start",
                                BackgroundColor = "transparent",
                                Border = 0,
                                BorderRadius = borderRadius,
                                Outline = 0,
                                Transition = @$"all {motionDurationMid} linear",
                                Appearance = "textfield",
                                FontSize = "inherit",
                                VerticalAlign = "top",
                                ["..."] = GenPlaceholderStyle(token.ColorTextPlaceholder),
                                ["&[type=\"number\"]::-webkit-inner-spin-button, &[type=\"number\"]::-webkit-outer-spin-button"] = new Unknown_30()
                                {
                                    Margin = 0,
                                    WebkitAppearance = "none",
                                    Appearance = "none",
                                },
                            },
                        },
                    },
                },
                new Unknown_31()
                {
                    [componentCls] = new Unknown_32()
                    {
                        [$"&:hover {componentCls}-handler-wrap, &-focused {componentCls}-handler-wrap"] = new Unknown_33()
                        {
                            Opacity = 1,
                        },
                        [$"{componentCls}-handler-wrap"] = new Unknown_34()
                        {
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineEnd = 0,
                            Width = token.HandleWidth,
                            Height = "100%",
                            Background = colorBgContainer,
                            BorderStartStartRadius = 0,
                            BorderStartEndRadius = borderRadius,
                            BorderEndEndRadius = borderRadius,
                            BorderEndStartRadius = 0,
                            Opacity = handleVisible === true ? 1 : 0,
                            Display = "flex",
                            FlexDirection = "column",
                            AlignItems = "stretch",
                            Transition = @$"opacity {motionDurationMid} linear {motionDurationMid}",
                            [$"{componentCls}-handler"] = new Unknown_35()
                            {
                                Display = "flex",
                                AlignItems = "center",
                                JustifyContent = "center",
                                Flex = "auto",
                                Height = "40%",
                                [$"{componentCls}-handler-up-inner,{componentCls}-handler-down-inner"] = new Unknown_36()
                                {
                                    MarginInlineEnd = 0,
                                    FontSize = token.HandleFontSize,
                                },
                            },
                        },
                        [$"{componentCls}-handler"] = new Unknown_37()
                        {
                            Height = "50%",
                            Overflow = "hidden",
                            Color = colorTextDescription,
                            FontWeight = "bold",
                            LineHeight = 0,
                            TextAlign = "center",
                            Cursor = "pointer",
                            BorderInlineStart = @$"{lineWidth}px {lineType} {colorBorder}",
                            Transition = @$"all {motionDurationMid} linear",
                            ["&:active"] = new Unknown_38()
                            {
                                Background = token.ColorFillAlter,
                            },
                            ["&:hover"] = new Unknown_39()
                            {
                                Height = @$"60%",
                                [$"{componentCls}-handler-up-inner,{componentCls}-handler-down-inner"] = new Unknown_40()
                                {
                                    Color = colorPrimary,
                                },
                            },
                            ["&-up-inner, &-down-inner"] = new Unknown_41()
                            {
                                ["..."] = ResetIcon(),
                                Color = colorTextDescription,
                                Transition = @$"all {motionDurationMid} linear",
                                UserSelect = "none",
                            },
                        },
                        [$"{componentCls}-handler-up"] = new Unknown_42()
                        {
                            BorderStartEndRadius = borderRadius,
                        },
                        [$"{componentCls}-handler-down"] = new Unknown_43()
                        {
                            BorderBlockStart = @$"{lineWidth}px {lineType} {colorBorder}",
                            BorderEndEndRadius = borderRadius,
                        },
                        ["&-disabled, &-readonly"] = new Unknown_44()
                        {
                            [$"{componentCls}-handler-wrap"] = new Unknown_45()
                            {
                                Display = "none",
                            },
                            [$"{componentCls}-input"] = new Unknown_46()
                            {
                                Color = "inherit",
                            },
                        },
                        [$"{componentCls}-handler-up-disabled,{componentCls}-handler-down-disabled"] = new Unknown_47()
                        {
                            Cursor = "not-allowed",
                        },
                        [$"{componentCls}-handler-up-disabled:hover&-handler-up-inner,{componentCls}-handler-down-disabled:hover&-handler-down-inner"] = new Unknown_48()
                        {
                            Color = colorTextDisabled,
                        },
                    },
                },
                new Unknown_49()
                {
                    [$"{componentCls}-borderless"] = new Unknown_50()
                    {
                        BorderColor = "transparent",
                        BoxShadow = "none",
                        [$"{componentCls}-handler-down"] = new Unknown_51()
                        {
                            BorderBlockStartWidth = 0,
                        },
                    },
                },
            };
        }

        public Unknown_2 GenAffixWrapperStyles(InputNumberToken token)
        {
            var componentCls = token.ComponentCls;
            var inputPaddingHorizontal = token.InputPaddingHorizontal;
            var inputAffixPadding = token.InputAffixPadding;
            var controlWidth = token.ControlWidth;
            var borderRadiusLG = token.BorderRadiusLG;
            var borderRadiusSM = token.BorderRadiusSM;
            return new Unknown_52()
            {
                [$"{componentCls}-affix-wrapper"] = new Unknown_53()
                {
                    ["..."] = GenBasicInputStyle(token),
                    ["..."] = GenStatusStyle(token, $"{componentCls}-affix-wrapper"),
                    Position = "relative",
                    Display = "inline-flex",
                    Width = controlWidth,
                    Padding = 0,
                    PaddingInlineStart = inputPaddingHorizontal,
                    ["&-lg"] = new Unknown_54()
                    {
                        BorderRadius = borderRadiusLG,
                    },
                    ["&-sm"] = new Unknown_55()
                    {
                        BorderRadius = borderRadiusSM,
                    },
                    [$"&:not({componentCls}-affix-wrapper-disabled):hover"] = new Unknown_56()
                    {
                        ["..."] = GenHoverStyle(token),
                        ZIndex = 1,
                    },
                    ["&-focused, &:focus"] = new Unknown_57()
                    {
                        ZIndex = 1,
                    },
                    ["&-disabled"] = new Unknown_58()
                    {
                        [$"{componentCls}[disabled]"] = new Unknown_59()
                        {
                            Background = "transparent",
                        },
                    },
                    [$"> div{componentCls}"] = new Unknown_60()
                    {
                        Width = "100%",
                        Border = "none",
                        Outline = "none",
                        [$"&{componentCls}-focused"] = new Unknown_61()
                        {
                            BoxShadow = "none !important",
                        },
                    },
                    [$"input{componentCls}-input"] = new Unknown_62()
                    {
                        Padding = 0,
                    },
                    ["&::before"] = new Unknown_63()
                    {
                        Width = 0,
                        Visibility = "hidden",
                        Content = '"\\a0"',
                    },
                    [$"{componentCls}-handler-wrap"] = new Unknown_64()
                    {
                        ZIndex = 2,
                    },
                    [componentCls] = new Unknown_65()
                    {
                        ["&-prefix, &-suffix"] = new Unknown_66()
                        {
                            Display = "flex",
                            Flex = "none",
                            AlignItems = "center",
                            PointerEvents = "none",
                        },
                        ["&-prefix"] = new Unknown_67()
                        {
                            MarginInlineEnd = inputAffixPadding,
                        },
                        ["&-suffix"] = new Unknown_68()
                        {
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineEnd = 0,
                            ZIndex = 1,
                            Height = "100%",
                            MarginInlineEnd = inputPaddingHorizontal,
                            MarginInlineStart = inputAffixPadding,
                        },
                    },
                },
            };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_69 token)
        {
            var inputNumberToken = InitInputToken(token);
            return new Unknown_70
            {
                GenInputNumberStyles(inputNumberToken),
                GenAffixWrapperStyles(inputNumberToken),
                GenCompactItemStyle(inputNumberToken)
            };
        }

        public Unknown_4 GenComponentStyleHook(Unknown_71 token)
        {
            return new Unknown_72()
            {
                ControlWidth = 90,
                HandleWidth = token.ControlHeightSM - token.LineWidth * 2,
                HandleFontSize = token.FontSize / 2,
                HandleVisible = "auto",
            };
        }

    }

}