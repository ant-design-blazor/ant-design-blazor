using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class RadioToken
    {
    }

    public partial class RadioToken : TokenWithCommonCls
    {
        public string RadioFocusShadow { get; set; }

        public string RadioButtonFocusShadow { get; set; }

        public int RadioSize { get; set; }

        public int RadioTop { get; set; }

        public int RadioDotSize { get; set; }

        public int RadioDotDisabledSize { get; set; }

        public string RadioCheckedColor { get; set; }

        public string RadioDotDisabledColor { get; set; }

        public string RadioSolidCheckedColor { get; set; }

        public string RadioButtonBg { get; set; }

        public string RadioButtonCheckedBg { get; set; }

        public string RadioButtonColor { get; set; }

        public string RadioButtonHoverColor { get; set; }

        public string RadioButtonActiveColor { get; set; }

        public int RadioButtonPaddingHorizontal { get; set; }

        public string RadioDisabledButtonCheckedBg { get; set; }

        public string RadioDisabledButtonCheckedColor { get; set; }

        public int RadioWrapperMarginRight { get; set; }

    }

    public partial class Radio
    {
        private Keyframes antRadioEffect = new Keyframes("antRadioEffect")
        {
            ["0%"] = new Keyframes()
            {
                Transform = "scale(1)",
                Opacity = 0.5f,
            },
            ["100%"] = new Keyframes()
            {
                Transform = "scale(1.6)",
                Opacity = 0,
            },
        };

        public Unknown_1 GetGroupRadioStyle(Unknown_5 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var groupPrefixCls = @$"{componentCls}-group";
            return new Unknown_6()
            {
                [groupPrefixCls] = new Unknown_7()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    FontSize = 0,
                    [$"&{groupPrefixCls}-rtl"] = new Unknown_8()
                    {
                        Direction = "rtl",
                    },
                    [$"{antCls}-badge {antCls}-badge-count"] = new Unknown_9()
                    {
                        ZIndex = 1,
                    },
                    [$"> {antCls}-badge:not(:first-child) > {antCls}-button-wrapper"] = new Unknown_10()
                    {
                        BorderInlineStart = "none",
                    },
                },
            };
        }

        public Unknown_2 GetRadioBasicStyle(Unknown_11 token)
        {
            var componentCls = token.ComponentCls;
            var radioWrapperMarginRight = token.RadioWrapperMarginRight;
            var radioCheckedColor = token.RadioCheckedColor;
            var radioSize = token.RadioSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var motionEaseInOutCirc = token.MotionEaseInOutCirc;
            var radioButtonBg = token.RadioButtonBg;
            var colorBorder = token.ColorBorder;
            var lineWidth = token.LineWidth;
            var radioDotSize = token.RadioDotSize;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            var colorTextDisabled = token.ColorTextDisabled;
            var paddingXS = token.PaddingXS;
            var radioDotDisabledColor = token.RadioDotDisabledColor;
            var lineType = token.LineType;
            var radioDotDisabledSize = token.RadioDotDisabledSize;
            var wireframe = token.Wireframe;
            var colorWhite = token.ColorWhite;
            var radioInnerPrefixCls = @$"{componentCls}-inner";
            return new Unknown_12()
            {
                [$"{componentCls}-wrapper"] = new Unknown_13()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-flex",
                    AlignItems = "baseline",
                    MarginInlineStart = 0,
                    MarginInlineEnd = radioWrapperMarginRight,
                    Cursor = "pointer",
                    [$"&{componentCls}-wrapper-rtl"] = new Unknown_14()
                    {
                        Direction = "rtl",
                    },
                    ["&-disabled"] = new Unknown_15()
                    {
                        Cursor = "not-allowed",
                        Color = token.ColorTextDisabled,
                    },
                    ["&::after"] = new Unknown_16()
                    {
                        Display = "inline-block",
                        Width = 0,
                        Overflow = "hidden",
                        Content = '"\\a0"',
                    },
                    [$"{componentCls}-checked::after"] = new Unknown_17()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        Width = "100%",
                        Height = "100%",
                        Border = @$"{lineWidth}px {lineType} {radioCheckedColor}",
                        BorderRadius = "50%",
                        Visibility = "hidden",
                        AnimationName = antRadioEffect,
                        AnimationDuration = motionDurationSlow,
                        AnimationTimingFunction = motionEaseInOut,
                        AnimationFillMode = "both",
                        Content = "\"\"",
                    },
                    [componentCls] = new Unknown_18()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "relative",
                        Display = "inline-block",
                        Outline = "none",
                        Cursor = "pointer",
                        AlignSelf = "center",
                    },
                    [$"{componentCls}-wrapper:hover&,&:hover{radioInnerPrefixCls}"] = new Unknown_19()
                    {
                        BorderColor = radioCheckedColor,
                    },
                    [$"{componentCls}-input:focus-visible + {radioInnerPrefixCls}"] = new Unknown_20()
                    {
                        ["..."] = GenFocusOutline(token)
                    },
                    [$"{componentCls}:hover::after, {componentCls}-wrapper:hover &::after"] = new Unknown_21()
                    {
                        Visibility = "visible",
                    },
                    [$"{componentCls}-inner"] = new Unknown_22()
                    {
                        ["&::after"] = new Unknown_23()
                        {
                            BoxSizing = "border-box",
                            Position = "absolute",
                            InsetBlockStart = "50%",
                            InsetInlineStart = "50%",
                            Display = "block",
                            Width = radioSize,
                            Height = radioSize,
                            MarginBlockStart = radioSize / -2,
                            MarginInlineStart = radioSize / -2,
                            BackgroundColor = wireframe ? radioCheckedColor : colorWhite,
                            BorderBlockStart = 0,
                            BorderInlineStart = 0,
                            BorderRadius = radioSize,
                            Transform = "scale(0)",
                            Opacity = 0,
                            Transition = @$"all {motionDurationSlow} {motionEaseInOutCirc}",
                            Content = "\"\"",
                        },
                        BoxSizing = "border-box",
                        Position = "relative",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        Display = "block",
                        Width = radioSize,
                        Height = radioSize,
                        BackgroundColor = radioButtonBg,
                        BorderColor = colorBorder,
                        BorderStyle = "solid",
                        BorderWidth = lineWidth,
                        BorderRadius = "50%",
                        Transition = @$"all {motionDurationMid}",
                    },
                    [$"{componentCls}-input"] = new Unknown_24()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineEnd = 0,
                        InsetBlockEnd = 0,
                        InsetInlineStart = 0,
                        ZIndex = 1,
                        Cursor = "pointer",
                        Opacity = 0,
                    },
                    [$"{componentCls}-checked"] = new Unknown_25()
                    {
                        [radioInnerPrefixCls] = new Unknown_26()
                        {
                            BorderColor = radioCheckedColor,
                            BackgroundColor = wireframe ? radioButtonBg : radioCheckedColor,
                            ["&::after"] = new Unknown_27()
                            {
                                Transform = @$"scale({radioDotSize / radioSize})",
                                Opacity = 1,
                                Transition = @$"all {motionDurationSlow} {motionEaseInOutCirc}",
                            },
                        },
                    },
                    [$"{componentCls}-disabled"] = new Unknown_28()
                    {
                        Cursor = "not-allowed",
                        [radioInnerPrefixCls] = new Unknown_29()
                        {
                            BackgroundColor = colorBgContainerDisabled,
                            BorderColor = colorBorder,
                            Cursor = "not-allowed",
                            ["&::after"] = new Unknown_30()
                            {
                                BackgroundColor = radioDotDisabledColor,
                            },
                        },
                        [$"{componentCls}-input"] = new Unknown_31()
                        {
                            Cursor = "not-allowed",
                        },
                        [$"{componentCls}-disabled + span"] = new Unknown_32()
                        {
                            Color = colorTextDisabled,
                            Cursor = "not-allowed",
                        },
                        [$"&{componentCls}-checked"] = new Unknown_33()
                        {
                            [radioInnerPrefixCls] = new Unknown_34()
                            {
                                ["&::after"] = new Unknown_35()
                                {
                                    Transform = @$"scale({radioDotDisabledSize / radioSize})",
                                },
                            },
                        },
                    },
                    [$"span{componentCls} + *"] = new Unknown_36()
                    {
                        PaddingInlineStart = paddingXS,
                        PaddingInlineEnd = paddingXS,
                    },
                },
            };
        }

        public Unknown_3 GetRadioButtonStyle(Unknown_37 token)
        {
            var radioButtonColor = token.RadioButtonColor;
            var controlHeight = token.ControlHeight;
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorBorder = token.ColorBorder;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var radioButtonPaddingHorizontal = token.RadioButtonPaddingHorizontal;
            var fontSize = token.FontSize;
            var radioButtonBg = token.RadioButtonBg;
            var fontSizeLG = token.FontSizeLG;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var paddingXS = token.PaddingXS;
            var borderRadius = token.BorderRadius;
            var borderRadiusSM = token.BorderRadiusSM;
            var borderRadiusLG = token.BorderRadiusLG;
            var radioCheckedColor = token.RadioCheckedColor;
            var radioButtonCheckedBg = token.RadioButtonCheckedBg;
            var radioButtonHoverColor = token.RadioButtonHoverColor;
            var radioButtonActiveColor = token.RadioButtonActiveColor;
            var radioSolidCheckedColor = token.RadioSolidCheckedColor;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            var radioDisabledButtonCheckedColor = token.RadioDisabledButtonCheckedColor;
            var radioDisabledButtonCheckedBg = token.RadioDisabledButtonCheckedBg;
            return new Unknown_38()
            {
                [$"{componentCls}-button-wrapper"] = new Unknown_39()
                {
                    Position = "relative",
                    Display = "inline-block",
                    Height = controlHeight,
                    Margin = 0,
                    PaddingInline = radioButtonPaddingHorizontal,
                    PaddingBlock = 0,
                    Color = radioButtonColor,
                    FontSize = fontSize,
                    LineHeight = @$"{controlHeight - lineWidth * 2}px",
                    Background = radioButtonBg,
                    Border = @$"{lineWidth}px {lineType} {colorBorder}",
                    BorderBlockStartWidth = lineWidth + 0.02,
                    BorderInlineStartWidth = 0,
                    BorderInlineEndWidth = lineWidth,
                    Cursor = "pointer",
                    Transition = [
        `color ${motionDurationMid}`,
        `background ${motionDurationMid}`,
        `border-color ${motionDurationMid}`,
        `box-shadow ${motionDurationMid}`,
      ].join(","),
                    ["a"] = new Unknown_40()
                    {
                        Color = radioButtonColor,
                    },
                    [$"> {componentCls}-button"] = new Unknown_41()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        ZIndex = -1,
                        Width = "100%",
                        Height = "100%",
                    },
                    ["&:not(:first-child)"] = new Unknown_42()
                    {
                        ["&::before"] = new Unknown_43()
                        {
                            Position = "absolute",
                            InsetBlockStart = -lineWidth,
                            InsetInlineStart = -lineWidth,
                            Display = "block",
                            BoxSizing = "content-box",
                            Width = 1,
                            Height = "100%",
                            PaddingBlock = lineWidth,
                            PaddingInline = 0,
                            BackgroundColor = colorBorder,
                            Transition = @$"background-color {motionDurationSlow}",
                            Content = "\"\"",
                        },
                    },
                    ["&:first-child"] = new Unknown_44()
                    {
                        BorderInlineStart = @$"{lineWidth}px {lineType} {colorBorder}",
                        BorderStartStartRadius = borderRadius,
                        BorderEndStartRadius = borderRadius,
                    },
                    ["&:last-child"] = new Unknown_45()
                    {
                        BorderStartEndRadius = borderRadius,
                        BorderEndEndRadius = borderRadius,
                    },
                    ["&:first-child:last-child"] = new Unknown_46()
                    {
                        BorderRadius = borderRadius,
                    },
                    [$"{componentCls}-group-large &"] = new Unknown_47()
                    {
                        Height = controlHeightLG,
                        FontSize = fontSizeLG,
                        LineHeight = @$"{controlHeightLG - lineWidth * 2}px",
                        ["&:first-child"] = new Unknown_48()
                        {
                            BorderStartStartRadius = borderRadiusLG,
                            BorderEndStartRadius = borderRadiusLG,
                        },
                        ["&:last-child"] = new Unknown_49()
                        {
                            BorderStartEndRadius = borderRadiusLG,
                            BorderEndEndRadius = borderRadiusLG,
                        },
                    },
                    [$"{componentCls}-group-small &"] = new Unknown_50()
                    {
                        Height = controlHeightSM,
                        PaddingInline = paddingXS - lineWidth,
                        PaddingBlock = 0,
                        LineHeight = @$"{controlHeightSM - lineWidth * 2}px",
                        ["&:first-child"] = new Unknown_51()
                        {
                            BorderStartStartRadius = borderRadiusSM,
                            BorderEndStartRadius = borderRadiusSM,
                        },
                        ["&:last-child"] = new Unknown_52()
                        {
                            BorderStartEndRadius = borderRadiusSM,
                            BorderEndEndRadius = borderRadiusSM,
                        },
                    },
                    ["&:hover"] = new Unknown_53()
                    {
                        Position = "relative",
                        Color = radioCheckedColor,
                    },
                    ["&:has(:focus-visible)"] = new Unknown_54()
                    {
                        ["..."] = GenFocusOutline(token)
                    },
                    [$"{componentCls}-inner, input[type=\"checkbox\"], input[type=\"radio\"]"] = new Unknown_55()
                    {
                        Width = 0,
                        Height = 0,
                        Opacity = 0,
                        PointerEvents = "none",
                    },
                    [$"&-checked:not({componentCls}-button-wrapper-disabled)"] = new Unknown_56()
                    {
                        ZIndex = 1,
                        Color = radioCheckedColor,
                        Background = radioButtonCheckedBg,
                        BorderColor = radioCheckedColor,
                        ["&::before"] = new Unknown_57()
                        {
                            BackgroundColor = radioCheckedColor,
                        },
                        ["&:first-child"] = new Unknown_58()
                        {
                            BorderColor = radioCheckedColor,
                        },
                        ["&:hover"] = new Unknown_59()
                        {
                            Color = radioButtonHoverColor,
                            BorderColor = radioButtonHoverColor,
                            ["&::before"] = new Unknown_60()
                            {
                                BackgroundColor = radioButtonHoverColor,
                            },
                        },
                        ["&:active"] = new Unknown_61()
                        {
                            Color = radioButtonActiveColor,
                            BorderColor = radioButtonActiveColor,
                            ["&::before"] = new Unknown_62()
                            {
                                BackgroundColor = radioButtonActiveColor,
                            },
                        },
                    },
                    [$"{componentCls}-group-solid &-checked:not({componentCls}-button-wrapper-disabled)"] = new Unknown_63()
                    {
                        Color = radioSolidCheckedColor,
                        Background = radioCheckedColor,
                        BorderColor = radioCheckedColor,
                        ["&:hover"] = new Unknown_64()
                        {
                            Color = radioSolidCheckedColor,
                            Background = radioButtonHoverColor,
                            BorderColor = radioButtonHoverColor,
                        },
                        ["&:active"] = new Unknown_65()
                        {
                            Color = radioSolidCheckedColor,
                            Background = radioButtonActiveColor,
                            BorderColor = radioButtonActiveColor,
                        },
                    },
                    ["&-disabled"] = new Unknown_66()
                    {
                        Color = colorTextDisabled,
                        BackgroundColor = colorBgContainerDisabled,
                        BorderColor = colorBorder,
                        Cursor = "not-allowed",
                        ["&:first-child, &:hover"] = new Unknown_67()
                        {
                            Color = colorTextDisabled,
                            BackgroundColor = colorBgContainerDisabled,
                            BorderColor = colorBorder,
                        },
                    },
                    [$"&-disabled{componentCls}-button-wrapper-checked"] = new Unknown_68()
                    {
                        Color = radioDisabledButtonCheckedColor,
                        BackgroundColor = radioDisabledButtonCheckedBg,
                        BorderColor = colorBorder,
                        BoxShadow = "none",
                    },
                },
            };
        }

        public Unknown_4 GenComponentStyleHook(Unknown_69 token)
        {
            var padding = token.Padding;
            var lineWidth = token.LineWidth;
            var controlItemBgActiveDisabled = token.ControlItemBgActiveDisabled;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorBgContainer = token.ColorBgContainer;
            var fontSizeLG = token.FontSizeLG;
            var controlOutline = token.ControlOutline;
            var colorPrimaryHover = token.ColorPrimaryHover;
            var colorPrimaryActive = token.ColorPrimaryActive;
            var colorText = token.ColorText;
            var colorPrimary = token.ColorPrimary;
            var marginXS = token.MarginXS;
            var controlOutlineWidth = token.ControlOutlineWidth;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var wireframe = token.Wireframe;
            var radioFocusShadow = @$"0 0 0 {controlOutlineWidth}px {controlOutline}";
            var radioButtonFocusShadow = radioFocusShadow;
            var radioSize = fontSizeLG;
            var dotPadding = 4;
            var radioDotDisabledSize = radioSize - dotPadding * 2;
            var radioDotSize = wireframe ? radioDotDisabledSize : radioSize - (dotPadding + lineWidth) * 2;
            var radioCheckedColor = colorPrimary;
            var radioButtonColor = colorText;
            var radioButtonHoverColor = colorPrimaryHover;
            var radioButtonActiveColor = colorPrimaryActive;
            var radioButtonPaddingHorizontal = padding - lineWidth;
            var radioDisabledButtonCheckedColor = colorTextDisabled;
            var radioWrapperMarginRight = marginXS;
            var radioToken = MergeToken(
                token,
                new Unknown_70()
                {
                    RadioFocusShadow = radioFocusShadow,
                    RadioButtonFocusShadow = radioButtonFocusShadow,
                    RadioSize = radioSize,
                    RadioDotSize = radioDotSize,
                    RadioDotDisabledSize = radioDotDisabledSize,
                    RadioCheckedColor = radioCheckedColor,
                    RadioDotDisabledColor = colorTextDisabled,
                    RadioSolidCheckedColor = colorTextLightSolid,
                    RadioButtonBg = colorBgContainer,
                    RadioButtonCheckedBg = colorBgContainer,
                    RadioButtonColor = radioButtonColor,
                    RadioButtonHoverColor = radioButtonHoverColor,
                    RadioButtonActiveColor = radioButtonActiveColor,
                    RadioButtonPaddingHorizontal = radioButtonPaddingHorizontal,
                    RadioDisabledButtonCheckedBg = controlItemBgActiveDisabled,
                    RadioDisabledButtonCheckedColor = radioDisabledButtonCheckedColor,
                    RadioWrapperMarginRight = radioWrapperMarginRight,
                });
            return new Unknown_71
            {
                GetGroupRadioStyle(radioToken),
                GetRadioBasicStyle(radioToken),
                GetRadioButtonStyle(radioToken)
            };
        }

    }

}