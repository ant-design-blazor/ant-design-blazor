using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class RadioToken
    {
        public double RadioSize
        {
            get => (double)_tokens["radioSize"];
            set => _tokens["radioSize"] = value;
        }

        public double DotSize
        {
            get => (double)_tokens["dotSize"];
            set => _tokens["dotSize"] = value;
        }

        public string DotColorDisabled
        {
            get => (string)_tokens["dotColorDisabled"];
            set => _tokens["dotColorDisabled"] = value;
        }

        public string ButtonBg
        {
            get => (string)_tokens["buttonBg"];
            set => _tokens["buttonBg"] = value;
        }

        public string ButtonCheckedBg
        {
            get => (string)_tokens["buttonCheckedBg"];
            set => _tokens["buttonCheckedBg"] = value;
        }

        public string ButtonColor
        {
            get => (string)_tokens["buttonColor"];
            set => _tokens["buttonColor"] = value;
        }

        public double ButtonPaddingInline
        {
            get => (double)_tokens["buttonPaddingInline"];
            set => _tokens["buttonPaddingInline"] = value;
        }

        public string ButtonCheckedBgDisabled
        {
            get => (string)_tokens["buttonCheckedBgDisabled"];
            set => _tokens["buttonCheckedBgDisabled"] = value;
        }

        public string ButtonCheckedColorDisabled
        {
            get => (string)_tokens["buttonCheckedColorDisabled"];
            set => _tokens["buttonCheckedColorDisabled"] = value;
        }

        public string ButtonSolidCheckedColor
        {
            get => (string)_tokens["buttonSolidCheckedColor"];
            set => _tokens["buttonSolidCheckedColor"] = value;
        }

        public string ButtonSolidCheckedBg
        {
            get => (string)_tokens["buttonSolidCheckedBg"];
            set => _tokens["buttonSolidCheckedBg"] = value;
        }

        public string ButtonSolidCheckedHoverBg
        {
            get => (string)_tokens["buttonSolidCheckedHoverBg"];
            set => _tokens["buttonSolidCheckedHoverBg"] = value;
        }

        public string ButtonSolidCheckedActiveBg
        {
            get => (string)_tokens["buttonSolidCheckedActiveBg"];
            set => _tokens["buttonSolidCheckedActiveBg"] = value;
        }

        public double WrapperMarginInlineEnd
        {
            get => (double)_tokens["wrapperMarginInlineEnd"];
            set => _tokens["wrapperMarginInlineEnd"] = value;
        }

    }

    public partial class RadioToken : TokenWithCommonCls
    {
        public double RadioDotDisabledSize
        {
            get => (double)_tokens["radioDotDisabledSize"];
            set => _tokens["radioDotDisabledSize"] = value;
        }

        public string RadioFocusShadow
        {
            get => (string)_tokens["radioFocusShadow"];
            set => _tokens["radioFocusShadow"] = value;
        }

        public string RadioButtonFocusShadow
        {
            get => (string)_tokens["radioButtonFocusShadow"];
            set => _tokens["radioButtonFocusShadow"] = value;
        }

    }

    public partial class RadioStyle
    {
        public static CSSObject GetGroupRadioStyle(RadioToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var groupPrefixCls = @$"{componentCls}-group";
            return new CSSObject()
            {
                [groupPrefixCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    FontSize = 0,
                    [$"&{groupPrefixCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"{antCls}-badge {antCls}-badge-count"] = new CSSObject()
                    {
                        ZIndex = 1,
                    },
                    [$"> {antCls}-badge:not(:first-child) > {antCls}-button-wrapper"] = new CSSObject()
                    {
                        BorderInlineStart = "none",
                    },
                },
            };
        }

        public static CSSObject GetRadioBasicStyle(RadioToken token)
        {
            var componentCls = token.ComponentCls;
            var wrapperMarginInlineEnd = token.WrapperMarginInlineEnd;
            var colorPrimary = token.ColorPrimary;
            var radioSize = token.RadioSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOutCirc = token.MotionEaseInOutCirc;
            var colorBgContainer = token.ColorBgContainer;
            var colorBorder = token.ColorBorder;
            var lineWidth = token.LineWidth;
            var dotSize = token.DotSize;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            var colorTextDisabled = token.ColorTextDisabled;
            var paddingXS = token.PaddingXS;
            var dotColorDisabled = token.DotColorDisabled;
            var lineType = token.LineType;
            var radioDotDisabledSize = token.RadioDotDisabledSize;
            var wireframe = token.Wireframe;
            var colorWhite = token.ColorWhite;
            var radioInnerPrefixCls = @$"{componentCls}-inner";
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-flex",
                    AlignItems = "baseline",
                    MarginInlineStart = 0,
                    MarginInlineEnd = wrapperMarginInlineEnd,
                    Cursor = "pointer",
                    [$"&{componentCls}-wrapper-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    ["&-disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        Color = token.ColorTextDisabled,
                    },
                    ["&::after"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = 0,
                        Overflow = "hidden",
                        Content = "'\\\\a0'",
                    },
                    [$"{componentCls}-checked::after"] = new CSSObject()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        Width = "100%",
                        Height = "100%",
                        Border = @$"{lineWidth}px {lineType} {colorPrimary}",
                        BorderRadius = "50%",
                        Visibility = "hidden",
                        Content = "\"\"",
                    },
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "relative",
                        Display = "inline-block",
                        Outline = "none",
                        Cursor = "pointer",
                        AlignSelf = "center",
                        BorderRadius = "50%",
                    },
                    [$"{componentCls}-wrapper:hover&,&:hover{radioInnerPrefixCls}"] = new CSSObject()
                    {
                        BorderColor = colorPrimary,
                    },
                    [$"{componentCls}-input:focus-visible + {radioInnerPrefixCls}"] = new CSSObject()
                    {
                        ["..."] = GenFocusOutline(token)
                    },
                    [$"{componentCls}:hover::after, {componentCls}-wrapper:hover &::after"] = new CSSObject()
                    {
                        Visibility = "visible",
                    },
                    [$"{componentCls}-inner"] = new CSSObject()
                    {
                        ["&::after"] = new CSSObject()
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
                            BackgroundColor = wireframe ? colorPrimary : colorWhite,
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
                        BackgroundColor = colorBgContainer,
                        BorderColor = colorBorder,
                        BorderStyle = "solid",
                        BorderWidth = lineWidth,
                        BorderRadius = "50%",
                        Transition = @$"all {motionDurationMid}",
                    },
                    [$"{componentCls}-input"] = new CSSObject()
                    {
                        Position = "absolute",
                        Inset = 0,
                        ZIndex = 1,
                        Cursor = "pointer",
                        Opacity = 0,
                    },
                    [$"{componentCls}-checked"] = new CSSObject()
                    {
                        [radioInnerPrefixCls] = new CSSObject()
                        {
                            BorderColor = colorPrimary,
                            BackgroundColor = wireframe ? colorBgContainer : colorPrimary,
                            ["&::after"] = new CSSObject()
                            {
                                Transform = @$"scale({dotSize / radioSize})",
                                Opacity = 1,
                                Transition = @$"all {motionDurationSlow} {motionEaseInOutCirc}",
                            },
                        },
                    },
                    [$"{componentCls}-disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        [radioInnerPrefixCls] = new CSSObject()
                        {
                            BackgroundColor = colorBgContainerDisabled,
                            BorderColor = colorBorder,
                            Cursor = "not-allowed",
                            ["&::after"] = new CSSObject()
                            {
                                BackgroundColor = dotColorDisabled,
                            },
                        },
                        [$"{componentCls}-input"] = new CSSObject()
                        {
                            Cursor = "not-allowed",
                        },
                        [$"{componentCls}-disabled + span"] = new CSSObject()
                        {
                            Color = colorTextDisabled,
                            Cursor = "not-allowed",
                        },
                        [$"&{componentCls}-checked"] = new CSSObject()
                        {
                            [radioInnerPrefixCls] = new CSSObject()
                            {
                                ["&::after"] = new CSSObject()
                                {
                                    Transform = @$"scale({radioDotDisabledSize / radioSize})",
                                },
                            },
                        },
                    },
                    [$"span{componentCls} + *"] = new CSSObject()
                    {
                        PaddingInlineStart = paddingXS,
                        PaddingInlineEnd = paddingXS,
                    },
                },
            };
        }

        public static CSSObject GetRadioButtonStyle(RadioToken token)
        {
            var buttonColor = token.ButtonColor;
            var controlHeight = token.ControlHeight;
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorBorder = token.ColorBorder;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var buttonPaddingInline = token.ButtonPaddingInline;
            var fontSize = token.FontSize;
            var buttonBg = token.ButtonBg;
            var fontSizeLG = token.FontSizeLG;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var paddingXS = token.PaddingXS;
            var borderRadius = token.BorderRadius;
            var borderRadiusSM = token.BorderRadiusSM;
            var borderRadiusLG = token.BorderRadiusLG;
            var buttonCheckedBg = token.ButtonCheckedBg;
            var buttonSolidCheckedColor = token.ButtonSolidCheckedColor;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            var buttonCheckedBgDisabled = token.ButtonCheckedBgDisabled;
            var buttonCheckedColorDisabled = token.ButtonCheckedColorDisabled;
            var colorPrimary = token.ColorPrimary;
            var colorPrimaryHover = token.ColorPrimaryHover;
            var colorPrimaryActive = token.ColorPrimaryActive;
            var buttonSolidCheckedBg = token.ButtonSolidCheckedBg;
            var buttonSolidCheckedHoverBg = token.ButtonSolidCheckedHoverBg;
            var buttonSolidCheckedActiveBg = token.ButtonSolidCheckedActiveBg;
            return new CSSObject()
            {
                [$"{componentCls}-button-wrapper"] = new CSSObject()
                {
                    Position = "relative",
                    Display = "inline-block",
                    Height = controlHeight,
                    Margin = 0,
                    PaddingInline = buttonPaddingInline,
                    PaddingBlock = 0,
                    Color = buttonColor,
                    FontSize = fontSize,
                    LineHeight = @$"{controlHeight - lineWidth * 2}px",
                    Background = buttonBg,
                    Border = @$"{lineWidth}px {lineType} {colorBorder}",
                    BorderBlockStartWidth = lineWidth + 0.02,
                    BorderInlineStartWidth = 0,
                    BorderInlineEndWidth = lineWidth,
                    Cursor = "pointer",
                    Transition = new string[]
                    {
                        $"color {motionDurationMid}",
                        $"background {motionDurationMid}",
                        $"box-shadow {motionDurationMid}"
                    }.Join(","),
                    ["a"] = new CSSObject()
                    {
                        Color = buttonColor,
                    },
                    [$"> {componentCls}-button"] = new CSSObject()
                    {
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        ZIndex = -1,
                        Width = "100%",
                        Height = "100%",
                    },
                    ["&:not(:first-child)"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
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
                    ["&:first-child"] = new CSSObject()
                    {
                        BorderInlineStart = @$"{lineWidth}px {lineType} {colorBorder}",
                        BorderStartStartRadius = borderRadius,
                        BorderEndStartRadius = borderRadius,
                    },
                    ["&:last-child"] = new CSSObject()
                    {
                        BorderStartEndRadius = borderRadius,
                        BorderEndEndRadius = borderRadius,
                    },
                    ["&:first-child:last-child"] = new CSSObject()
                    {
                        BorderRadius = borderRadius,
                    },
                    [$"{componentCls}-group-large &"] = new CSSObject()
                    {
                        Height = controlHeightLG,
                        FontSize = fontSizeLG,
                        LineHeight = @$"{controlHeightLG - lineWidth * 2}px",
                        ["&:first-child"] = new CSSObject()
                        {
                            BorderStartStartRadius = borderRadiusLG,
                            BorderEndStartRadius = borderRadiusLG,
                        },
                        ["&:last-child"] = new CSSObject()
                        {
                            BorderStartEndRadius = borderRadiusLG,
                            BorderEndEndRadius = borderRadiusLG,
                        },
                    },
                    [$"{componentCls}-group-small &"] = new CSSObject()
                    {
                        Height = controlHeightSM,
                        PaddingInline = paddingXS - lineWidth,
                        PaddingBlock = 0,
                        LineHeight = @$"{controlHeightSM - lineWidth * 2}px",
                        ["&:first-child"] = new CSSObject()
                        {
                            BorderStartStartRadius = borderRadiusSM,
                            BorderEndStartRadius = borderRadiusSM,
                        },
                        ["&:last-child"] = new CSSObject()
                        {
                            BorderStartEndRadius = borderRadiusSM,
                            BorderEndEndRadius = borderRadiusSM,
                        },
                    },
                    ["&:hover"] = new CSSObject()
                    {
                        Position = "relative",
                        Color = colorPrimary,
                    },
                    ["&:has(:focus-visible)"] = new CSSObject()
                    {
                        ["..."] = GenFocusOutline(token)
                    },
                    [$"{componentCls}-inner, input[type=\"checkbox\"], input[type=\"radio\"]"] = new CSSObject()
                    {
                        Width = 0,
                        Height = 0,
                        Opacity = 0,
                        PointerEvents = "none",
                    },
                    [$"&-checked:not({componentCls}-button-wrapper-disabled)"] = new CSSObject()
                    {
                        ZIndex = 1,
                        Color = colorPrimary,
                        Background = buttonCheckedBg,
                        BorderColor = colorPrimary,
                        ["&::before"] = new CSSObject()
                        {
                            BackgroundColor = colorPrimary,
                        },
                        ["&:first-child"] = new CSSObject()
                        {
                            BorderColor = colorPrimary,
                        },
                        ["&:hover"] = new CSSObject()
                        {
                            Color = colorPrimaryHover,
                            BorderColor = colorPrimaryHover,
                            ["&::before"] = new CSSObject()
                            {
                                BackgroundColor = colorPrimaryHover,
                            },
                        },
                        ["&:active"] = new CSSObject()
                        {
                            Color = colorPrimaryActive,
                            BorderColor = colorPrimaryActive,
                            ["&::before"] = new CSSObject()
                            {
                                BackgroundColor = colorPrimaryActive,
                            },
                        },
                    },
                    [$"{componentCls}-group-solid &-checked:not({componentCls}-button-wrapper-disabled)"] = new CSSObject()
                    {
                        Color = buttonSolidCheckedColor,
                        Background = buttonSolidCheckedBg,
                        BorderColor = buttonSolidCheckedBg,
                        ["&:hover"] = new CSSObject()
                        {
                            Color = buttonSolidCheckedColor,
                            Background = buttonSolidCheckedHoverBg,
                            BorderColor = buttonSolidCheckedHoverBg,
                        },
                        ["&:active"] = new CSSObject()
                        {
                            Color = buttonSolidCheckedColor,
                            Background = buttonSolidCheckedActiveBg,
                            BorderColor = buttonSolidCheckedActiveBg,
                        },
                    },
                    ["&-disabled"] = new CSSObject()
                    {
                        Color = colorTextDisabled,
                        BackgroundColor = colorBgContainerDisabled,
                        BorderColor = colorBorder,
                        Cursor = "not-allowed",
                        ["&:first-child, &:hover"] = new CSSObject()
                        {
                            Color = colorTextDisabled,
                            BackgroundColor = colorBgContainerDisabled,
                            BorderColor = colorBorder,
                        },
                    },
                    [$"&-disabled{componentCls}-button-wrapper-checked"] = new CSSObject()
                    {
                        Color = buttonCheckedColorDisabled,
                        BackgroundColor = buttonCheckedBgDisabled,
                        BorderColor = colorBorder,
                        BoxShadow = "none",
                    },
                },
            };
        }

        public static double GetDotSize(double radioSize)
        {
            var dotPadding = 4;
            return radioSize - dotPadding * 2;
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Radio",
                (token) =>
                {
                    var controlOutline = token.ControlOutline;
                    var controlOutlineWidth = token.ControlOutlineWidth;
                    var radioSize = token.RadioSize;
                    var radioFocusShadow = @$"0 0 0 {controlOutlineWidth}px {controlOutline}";
                    var radioButtonFocusShadow = radioFocusShadow;
                    var radioDotDisabledSize = GetDotSize(radioSize);
                    var radioToken = MergeToken(
                        token,
                        new RadioToken()
                        {
                            RadioDotDisabledSize = radioDotDisabledSize,
                            RadioFocusShadow = radioFocusShadow,
                            RadioButtonFocusShadow = radioButtonFocusShadow,
                        });
                    return new CSSInterpolation[]
                    {
                        GetGroupRadioStyle(radioToken),
                        GetRadioBasicStyle(radioToken),
                        GetRadioButtonStyle(radioToken),
                    };
                },
                (token) =>
                {
                    var wireframe = token.Wireframe;
                    var padding = token.Padding;
                    var marginXS = token.MarginXS;
                    var lineWidth = token.LineWidth;
                    var fontSizeLG = token.FontSizeLG;
                    var colorText = token.ColorText;
                    var colorBgContainer = token.ColorBgContainer;
                    var colorTextDisabled = token.ColorTextDisabled;
                    var controlItemBgActiveDisabled = token.ControlItemBgActiveDisabled;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorPrimary = token.ColorPrimary;
                    var colorPrimaryHover = token.ColorPrimaryHover;
                    var colorPrimaryActive = token.ColorPrimaryActive;
                    var dotPadding = 4;
                    var radioSize = fontSizeLG;
                    var radioDotSize = wireframe ? GetDotSize(radioSize) : radioSize - (dotPadding + lineWidth) * 2;
                    return new RadioToken()
                    {
                        RadioSize = radioSize,
                        DotSize = radioDotSize,
                        DotColorDisabled = colorTextDisabled,
                        ButtonSolidCheckedColor = colorTextLightSolid,
                        ButtonSolidCheckedBg = colorPrimary,
                        ButtonSolidCheckedHoverBg = colorPrimaryHover,
                        ButtonSolidCheckedActiveBg = colorPrimaryActive,
                        ButtonBg = colorBgContainer,
                        ButtonCheckedBg = colorBgContainer,
                        ButtonColor = colorText,
                        ButtonCheckedBgDisabled = controlItemBgActiveDisabled,
                        ButtonCheckedColorDisabled = colorTextDisabled,
                        ButtonPaddingInline = padding - lineWidth,
                        WrapperMarginInlineEnd = marginXS,
                    };
                });
        }

    }

}
