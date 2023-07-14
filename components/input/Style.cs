using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class InputToken : TokenWithCommonCls
    {
        public int InputAffixPadding { get; set; }

        public int InputPaddingVertical { get; set; }

        public int InputPaddingVerticalLG { get; set; }

        public int InputPaddingVerticalSM { get; set; }

        public int InputPaddingHorizontal { get; set; }

        public int InputPaddingHorizontalLG { get; set; }

        public int InputPaddingHorizontalSM { get; set; }

        public string InputBorderHoverColor { get; set; }

        public string InputBorderActiveColor { get; set; }

    }

    public partial class Input
    {
        public CSSObject GenPlaceholderStyle(string color)
        {
            return new CSSObject()
            {
                ["&::-moz-placeholder"] = new CSSObject()
                {
                    Opacity = 1,
                },
                ["&::placeholder"] = new CSSObject()
                {
                    Color = color,
                    UserSelect = "none",
                },
                ["&:placeholder-shown"] = new CSSObject()
                {
                    TextOverflow = "ellipsis",
                },
            };
        }

        public CSSObject GenHoverStyle(InputToken token)
        {
            return new CSSObject()
            {
                BorderColor = token.InputBorderHoverColor,
                BorderInlineEndWidth = token.LineWidth,
            };
        }

        public Unknown_1 GenActiveStyle(InputToken token)
        {
            return new Unknown_8()
            {
                BorderColor = token.InputBorderHoverColor,
                BoxShadow = @$"0 0 0 {token.ControlOutlineWidth}px {token.ControlOutline}",
                BorderInlineEndWidth = token.LineWidth,
                Outline = 0,
            };
        }

        public CSSObject GenDisabledStyle(InputToken token)
        {
            return new CSSObject()
            {
                Color = token.ColorTextDisabled,
                BackgroundColor = token.ColorBgContainerDisabled,
                BorderColor = token.ColorBorder,
                BoxShadow = "none",
                Cursor = "not-allowed",
                Opacity = 1,
                ["&:hover"] = new CSSObject()
                {
                    ["..."] = GenHoverStyle(mergeToken<InputToken>(token, { inputBorderHoverColor: token.ColorBorder }))
                },
            };
        }

        public CSSObject GenInputLargeStyle(InputToken token)
        {
            var inputPaddingVerticalLG = token.InputPaddingVerticalLG;
            var fontSizeLG = token.FontSizeLG;
            var lineHeightLG = token.LineHeightLG;
            var borderRadiusLG = token.BorderRadiusLG;
            var inputPaddingHorizontalLG = token.InputPaddingHorizontalLG;
            return new CSSObject()
            {
                Padding = @$"{inputPaddingVerticalLG}px {inputPaddingHorizontalLG}px",
                FontSize = fontSizeLG,
                LineHeight = lineHeightLG,
                BorderRadius = borderRadiusLG,
            };
        }

        public CSSObject GenInputSmallStyle(InputToken token)
        {
            return new CSSObject()
            {
                Padding = @$"{token.InputPaddingVerticalSM}px {token.ControlPaddingHorizontalSM - 1}px",
                BorderRadius = token.BorderRadiusSM,
            };
        }

        public CSSObject GenStatusStyle(InputToken token, string parentCls)
        {
            var componentCls = token.ComponentCls;
            var colorError = token.ColorError;
            var colorWarning = token.ColorWarning;
            var colorErrorOutline = token.ColorErrorOutline;
            var colorWarningOutline = token.ColorWarningOutline;
            var colorErrorBorderHover = token.ColorErrorBorderHover;
            var colorWarningBorderHover = token.ColorWarningBorderHover;
            return new CSSObject()
            {
                [$"&-status-error:not({parentCls}-disabled):not({parentCls}-borderless){parentCls}"] = new CSSObject()
                {
                    BorderColor = colorError,
                    ["&:hover"] = new CSSObject()
                    {
                        BorderColor = colorErrorBorderHover,
                    },
                    ["&:focus, &-focused"] = new CSSObject()
                    {
                        ["..."] = GenActiveStyle(mergeToken<InputToken>(token, {
            inputBorderActiveColor: colorError,
            inputBorderHoverColor: colorError,
            controlOutline: colorErrorOutline,
          }))
                    },
                    [$"{componentCls}-prefix, {componentCls}-suffix"] = new CSSObject()
                    {
                        Color = colorError,
                    },
                },
                [$"&-status-warning:not({parentCls}-disabled):not({parentCls}-borderless){parentCls}"] = new CSSObject()
                {
                    BorderColor = colorWarning,
                    ["&:hover"] = new CSSObject()
                    {
                        BorderColor = colorWarningBorderHover,
                    },
                    ["&:focus, &-focused"] = new CSSObject()
                    {
                        ["..."] = GenActiveStyle(mergeToken<InputToken>(token, {
            inputBorderActiveColor: colorWarning,
            inputBorderHoverColor: colorWarning,
            controlOutline: colorWarningOutline,
          }))
                    },
                    [$"{componentCls}-prefix, {componentCls}-suffix"] = new CSSObject()
                    {
                        Color = colorWarning,
                    },
                },
            };
        }

        public CSSObject GenBasicInputStyle(InputToken token)
        {
            return new CSSObject()
            {
                Position = "relative",
                Display = "inline-block",
                Width = "100%",
                MinWidth = 0,
                Padding = @$"{token.InputPaddingVertical}px {token.InputPaddingHorizontal}px",
                Color = token.ColorText,
                FontSize = token.FontSize,
                LineHeight = token.LineHeight,
                BackgroundColor = token.ColorBgContainer,
                BackgroundImage = "none",
                BorderWidth = token.LineWidth,
                BorderStyle = token.LineType,
                BorderColor = token.ColorBorder,
                BorderRadius = token.BorderRadius,
                Transition = @$"all {token.MotionDurationMid}",
                ["..."] = GenPlaceholderStyle(token.ColorTextPlaceholder),
                ["&:hover"] = new CSSObject()
                {
                    ["..."] = GenHoverStyle(token)
                },
                ["&:focus, &-focused"] = new CSSObject()
                {
                    ["..."] = GenActiveStyle(token)
                },
                ["&-disabled, &[disabled]"] = new CSSObject()
                {
                    ["..."] = GenDisabledStyle(token)
                },
                ["&-borderless"] = new CSSObject()
                {
                    ["&, &:hover, &:focus, &-focused, &-disabled, &[disabled]"] = new CSSObject()
                    {
                        BackgroundColor = "transparent",
                        Border = "none",
                        BoxShadow = "none",
                    },
                },
                ["textarea&"] = new CSSObject()
                {
                    MaxWidth = "100%",
                    Height = "auto",
                    MinHeight = token.ControlHeight,
                    LineHeight = token.LineHeight,
                    VerticalAlign = "bottom",
                    Transition = @$"all {token.MotionDurationSlow}, height 0s",
                    Resize = "vertical",
                },
                ["&-lg"] = new CSSObject()
                {
                    ["..."] = GenInputLargeStyle(token)
                },
                ["&-sm"] = new CSSObject()
                {
                    ["..."] = GenInputSmallStyle(token)
                },
                ["&-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
                ["&-textarea-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
            };
        }

        public CSSObject GenInputGroupStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            return new CSSObject()
            {
                Position = "relative",
                Display = "table",
                Width = "100%",
                BorderCollapse = "separate",
                BorderSpacing = 0,
                ["&[class*="col-"]"] = new CSSObject()
                {
                    PaddingInlineEnd = token.PaddingXS,
                    ["&:last-child"] = new CSSObject()
                    {
                        PaddingInlineEnd = 0,
                    },
                },
                [$"&-lg {componentCls}, &-lg > {componentCls}-group-addon"] = new CSSObject()
                {
                    ["..."] = GenInputLargeStyle(token)
                },
                [$"&-sm {componentCls}, &-sm > {componentCls}-group-addon"] = new CSSObject()
                {
                    ["..."] = GenInputSmallStyle(token)
                },
                [$"&-lg {antCls}-select-single {antCls}-select-selector"] = new CSSObject()
                {
                    Height = token.ControlHeightLG,
                },
                [$"&-sm {antCls}-select-single {antCls}-select-selector"] = new CSSObject()
                {
                    Height = token.ControlHeightSM,
                },
                [$"> {componentCls}"] = new CSSObject()
                {
                    Display = "table-cell",
                    ["&:not(:first-child):not(:last-child)"] = new CSSObject()
                    {
                        BorderRadius = 0,
                    },
                },
                [$"{componentCls}-group"] = new CSSObject()
                {
                    ["&-addon, &-wrap"] = new CSSObject()
                    {
                        Display = "table-cell",
                        Width = 1,
                        WhiteSpace = "nowrap",
                        VerticalAlign = "middle",
                        ["&:not(:first-child):not(:last-child)"] = new CSSObject()
                        {
                            BorderRadius = 0,
                        },
                    },
                    ["&-wrap > *"] = new CSSObject()
                    {
                        Display = "block !important",
                    },
                    ["&-addon"] = new CSSObject()
                    {
                        Position = "relative",
                        Padding = @$"0 {token.InputPaddingHorizontal}px",
                        Color = token.ColorText,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                        TextAlign = "center",
                        BackgroundColor = token.ColorFillAlter,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        BorderRadius = token.BorderRadius,
                        Transition = @$"all {token.MotionDurationSlow}",
                        LineHeight = 1,
                        [$"{antCls}-select"] = new CSSObject()
                        {
                            Margin = @$"-{token.InputPaddingVertical + 1}px -{token.InputPaddingHorizontal}px",
                            [$"&{antCls}-select-single:not({antCls}-select-customize-input)"] = new CSSObject()
                            {
                                [$"{antCls}-select-selector"] = new CSSObject()
                                {
                                    BackgroundColor = "inherit",
                                    Border = @$"{token.LineWidth}px {token.LineType} transparent",
                                    BoxShadow = "none",
                                },
                            },
                            ["&-open, &-focused"] = new CSSObject()
                            {
                                [$"{antCls}-select-selector"] = new CSSObject()
                                {
                                    Color = token.ColorPrimary,
                                },
                            },
                        },
                        [$"{antCls}-cascader-picker"] = new CSSObject()
                        {
                            Margin = @$"-9px -{token.InputPaddingHorizontal}px",
                            BackgroundColor = "transparent",
                            [$"{antCls}-cascader-input"] = new CSSObject()
                            {
                                TextAlign = "start",
                                Border = 0,
                                BoxShadow = "none",
                            },
                        },
                    },
                    ["&-addon:first-child"] = new CSSObject()
                    {
                        BorderInlineEnd = 0,
                    },
                    ["&-addon:last-child"] = new CSSObject()
                    {
                        BorderInlineStart = 0,
                    },
                },
                [$"{componentCls}"] = new CSSObject()
                {
                    Width = "100%",
                    MarginBottom = 0,
                    TextAlign = "inherit",
                    ["&:focus"] = new CSSObject()
                    {
                        ZIndex = 1,
                        BorderInlineEndWidth = 1,
                    },
                    ["&:hover"] = new CSSObject()
                    {
                        ZIndex = 1,
                        BorderInlineEndWidth = 1,
                        [$"{componentCls}-search-with-button &"] = new CSSObject()
                        {
                            ZIndex = 0,
                        },
                    },
                },
                [$"> {componentCls}:first-child, {componentCls}-group-addon:first-child"] = new CSSObject()
                {
                    BorderStartEndRadius = 0,
                    BorderEndEndRadius = 0,
                    [$"{antCls}-select {antCls}-select-selector"] = new CSSObject()
                    {
                        BorderStartEndRadius = 0,
                        BorderEndEndRadius = 0,
                    },
                },
                [$"> {componentCls}-affix-wrapper"] = new CSSObject()
                {
                    [$"&:not(:first-child) {componentCls}"] = new CSSObject()
                    {
                        BorderStartStartRadius = 0,
                        BorderEndStartRadius = 0,
                    },
                    [$"&:not(:last-child) {componentCls}"] = new CSSObject()
                    {
                        BorderStartEndRadius = 0,
                        BorderEndEndRadius = 0,
                    },
                },
                [$"> {componentCls}:last-child, {componentCls}-group-addon:last-child"] = new CSSObject()
                {
                    BorderStartStartRadius = 0,
                    BorderEndStartRadius = 0,
                    [$"{antCls}-select {antCls}-select-selector"] = new CSSObject()
                    {
                        BorderStartStartRadius = 0,
                        BorderEndStartRadius = 0,
                    },
                },
                [$"{componentCls}-affix-wrapper"] = new CSSObject()
                {
                    ["&:not(:last-child)"] = new CSSObject()
                    {
                        BorderStartEndRadius = 0,
                        BorderEndEndRadius = 0,
                        [$"{componentCls}-search &"] = new CSSObject()
                        {
                            BorderStartStartRadius = token.BorderRadius,
                            BorderEndStartRadius = token.BorderRadius,
                        },
                    },
                    [$"&:not(:first-child), {componentCls}-search &:not(:first-child)"] = new CSSObject()
                    {
                        BorderStartStartRadius = 0,
                        BorderEndStartRadius = 0,
                    },
                },
                [$"&{componentCls}-group-compact"] = new CSSObject()
                {
                    Display = "block",
                    ["..."] = ClearFix(),
                    [$"{componentCls}-group-addon, {componentCls}-group-wrap, > {componentCls}"] = new CSSObject()
                    {
                        ["&:not(:first-child):not(:last-child)"] = new CSSObject()
                        {
                            BorderInlineEndWidth = token.LineWidth,
                            ["&:hover"] = new CSSObject()
                            {
                                ZIndex = 1,
                            },
                            ["&:focus"] = new CSSObject()
                            {
                                ZIndex = 1,
                            },
                        },
                    },
                    ["& > *"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Float = "none",
                        VerticalAlign = "top",
                        BorderRadius = 0,
                    },
                    [$"& > {componentCls}-affix-wrapper"] = new CSSObject()
                    {
                        Display = "inline-flex",
                    },
                    [$"& > {antCls}-picker-range"] = new CSSObject()
                    {
                        Display = "inline-flex",
                    },
                    ["& > *:not(:last-child)"] = new CSSObject()
                    {
                        MarginInlineEnd = -token.LineWidth,
                        BorderInlineEndWidth = token.LineWidth,
                    },
                    [$"{componentCls}"] = new CSSObject()
                    {
                        Float = "none",
                    },
                    [$"&>{antCls}-select>{antCls}-select-selector,&>{antCls}-select-auto-complete{componentCls},&>{antCls}-cascader-picker{componentCls},&>{componentCls}-group-wrapper{componentCls}"] = new CSSObject()
                    {
                        BorderInlineEndWidth = token.LineWidth,
                        BorderRadius = 0,
                        ["&:hover"] = new CSSObject()
                        {
                            ZIndex = 1,
                        },
                        ["&:focus"] = new CSSObject()
                        {
                            ZIndex = 1,
                        },
                    },
                    [$"& > {antCls}-select-focused"] = new CSSObject()
                    {
                        ZIndex = 1,
                    },
                    [$"& > {antCls}-select > {antCls}-select-arrow"] = new CSSObject()
                    {
                        ZIndex = 1,
                    },
                    [$"&>*:first-child,&>{antCls}-select:first-child>{antCls}-select-selector,&>{antCls}-select-auto-complete:first-child{componentCls},&>{antCls}-cascader-picker:first-child{componentCls}"] = new CSSObject()
                    {
                        BorderStartStartRadius = token.BorderRadius,
                        BorderEndStartRadius = token.BorderRadius,
                    },
                    [$"&>*:last-child,&>{antCls}-select:last-child>{antCls}-select-selector,&>{antCls}-cascader-picker:last-child{componentCls},&>{antCls}-cascader-picker-focused:last-child{componentCls}"] = new CSSObject()
                    {
                        BorderInlineEndWidth = token.LineWidth,
                        BorderStartEndRadius = token.BorderRadius,
                        BorderEndEndRadius = token.BorderRadius,
                    },
                    [$"& > {antCls}-select-auto-complete {componentCls}"] = new CSSObject()
                    {
                        VerticalAlign = "top",
                    },
                    [$"{componentCls}-group-wrapper + {componentCls}-group-wrapper"] = new CSSObject()
                    {
                        MarginInlineStart = -token.LineWidth,
                        [$"{componentCls}-affix-wrapper"] = new CSSObject()
                        {
                            BorderRadius = 0,
                        },
                    },
                    [$"{componentCls}-group-wrapper:not(:last-child)"] = new CSSObject()
                    {
                        [$"&{componentCls}-search > {componentCls}-group"] = new CSSObject()
                        {
                            [$"& > {componentCls}-group-addon > {componentCls}-search-button"] = new CSSObject()
                            {
                                BorderRadius = 0,
                            },
                            [$"& > {componentCls}"] = new CSSObject()
                            {
                                BorderStartStartRadius = token.BorderRadius,
                                BorderStartEndRadius = 0,
                                BorderEndEndRadius = 0,
                                BorderEndStartRadius = token.BorderRadius,
                            },
                        },
                    },
                },
            };
        }

        public Unknown_2 GenInputStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var controlHeightSM = token.ControlHeightSM;
            var lineWidth = token.LineWidth;
            var FIXED_CHROME_COLOR_HEIGHT = 16;
            var colorSmallPadding = (controlHeightSM - lineWidth * 2 - FIXED_CHROME_COLOR_HEIGHT) / 2;
            return new Unknown_9()
            {
                [componentCls] = new Unknown_10()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenBasicInputStyle(token),
                    ["..."] = GenStatusStyle(token, componentCls),
                    ["&[type=\"color\"]"] = new Unknown_11()
                    {
                        Height = token.ControlHeight,
                        [$"&{componentCls}-lg"] = new Unknown_12()
                        {
                            Height = token.ControlHeightLG,
                        },
                        [$"&{componentCls}-sm"] = new Unknown_13()
                        {
                            Height = controlHeightSM,
                            PaddingTop = colorSmallPadding,
                            PaddingBottom = colorSmallPadding,
                        },
                    },
                    ["&[type=\"search\"]::-webkit-search-cancel-button, &[type=\"search\"]::-webkit-search-decoration"] = new Unknown_14()
                    {
                        ["-webkit-appearance"] = "none",
                    },
                },
            };
        }

        public CSSObject GenAllowClearStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-clear-icon"] = new CSSObject()
                {
                    Margin = 0,
                    Color = token.ColorTextQuaternary,
                    FontSize = token.FontSizeIcon,
                    VerticalAlign = -1,
                    Cursor = "pointer",
                    Transition = @$"color {token.MotionDurationSlow}",
                    ["&:hover"] = new CSSObject()
                    {
                        Color = token.ColorTextTertiary,
                    },
                    ["&:active"] = new CSSObject()
                    {
                        Color = token.ColorText,
                    },
                    ["&-hidden"] = new CSSObject()
                    {
                        Visibility = "hidden",
                    },
                    ["&-has-suffix"] = new CSSObject()
                    {
                        Margin = @$"0 {token.InputAffixPadding}px",
                    },
                },
            };
        }

        public Unknown_3 GenAffixStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var inputAffixPadding = token.InputAffixPadding;
            var colorTextDescription = token.ColorTextDescription;
            var motionDurationSlow = token.MotionDurationSlow;
            var colorIcon = token.ColorIcon;
            var colorIconHover = token.ColorIconHover;
            var iconCls = token.IconCls;
            return new Unknown_15()
            {
                [$"{componentCls}-affix-wrapper"] = new Unknown_16()
                {
                    ["..."] = GenBasicInputStyle(token),
                    Display = "inline-flex",
                    [$"&:not({componentCls}-affix-wrapper-disabled):hover"] = new Unknown_17()
                    {
                        ["..."] = GenHoverStyle(token),
                        ZIndex = 1,
                        [$"{componentCls}-search-with-button &"] = new Unknown_18()
                        {
                            ZIndex = 0,
                        },
                    },
                    ["&-focused, &:focus"] = new Unknown_19()
                    {
                        ZIndex = 1,
                    },
                    ["&-disabled"] = new Unknown_20()
                    {
                        [$"{componentCls}[disabled]"] = new Unknown_21()
                        {
                            Background = "transparent",
                        },
                    },
                    [$"> input{componentCls}"] = new Unknown_22()
                    {
                        Padding = 0,
                        FontSize = "inherit",
                        Border = "none",
                        BorderRadius = 0,
                        Outline = "none",
                        ["&::-ms-reveal"] = new Unknown_23()
                        {
                            Display = "none",
                        },
                        ["&:focus"] = new Unknown_24()
                        {
                            BoxShadow = "none !important",
                        },
                    },
                    ["&::before"] = new Unknown_25()
                    {
                        Width = 0,
                        Visibility = "hidden",
                        Content = '"\\a0"',
                    },
                    [$"{componentCls}"] = new Unknown_26()
                    {
                        ["&-prefix, &-suffix"] = new Unknown_27()
                        {
                            Display = "flex",
                            Flex = "none",
                            AlignItems = "center",
                            ["> *:not(:last-child)"] = new Unknown_28()
                            {
                                MarginInlineEnd = token.PaddingXS,
                            },
                        },
                        ["&-show-count-suffix"] = new Unknown_29()
                        {
                            Color = colorTextDescription,
                        },
                        ["&-show-count-has-suffix"] = new Unknown_30()
                        {
                            MarginInlineEnd = token.PaddingXXS,
                        },
                        ["&-prefix"] = new Unknown_31()
                        {
                            MarginInlineEnd = inputAffixPadding,
                        },
                        ["&-suffix"] = new Unknown_32()
                        {
                            MarginInlineStart = inputAffixPadding,
                        },
                    },
                    ["..."] = GenAllowClearStyle(token),
                    [$"{iconCls}{componentCls}-password-icon"] = new Unknown_33()
                    {
                        Color = colorIcon,
                        Cursor = "pointer",
                        Transition = @$"all {motionDurationSlow}",
                        ["&:hover"] = new Unknown_34()
                        {
                            Color = colorIconHover,
                        },
                    },
                    ["..."] = GenStatusStyle(token, $"{componentCls}-affix-wrapper")
                },
            };
        }

        public Unknown_4 GenGroupStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var colorError = token.ColorError;
            var colorWarning = token.ColorWarning;
            var borderRadiusLG = token.BorderRadiusLG;
            var borderRadiusSM = token.BorderRadiusSM;
            return new Unknown_35()
            {
                [$"{componentCls}-group"] = new Unknown_36()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenInputGroupStyle(token),
                    ["&-rtl"] = new Unknown_37()
                    {
                        Direction = "rtl",
                    },
                    ["&-wrapper"] = new Unknown_38()
                    {
                        Display = "inline-block",
                        Width = "100%",
                        TextAlign = "start",
                        VerticalAlign = "top",
                        ["&-rtl"] = new Unknown_39()
                        {
                            Direction = "rtl",
                        },
                        ["&-lg"] = new Unknown_40()
                        {
                            [$"{componentCls}-group-addon"] = new Unknown_41()
                            {
                                BorderRadius = borderRadiusLG,
                            },
                        },
                        ["&-sm"] = new Unknown_42()
                        {
                            [$"{componentCls}-group-addon"] = new Unknown_43()
                            {
                                BorderRadius = borderRadiusSM,
                            },
                        },
                        ["&-status-error"] = new Unknown_44()
                        {
                            [$"{componentCls}-group-addon"] = new Unknown_45()
                            {
                                Color = colorError,
                                BorderColor = colorError,
                            },
                        },
                        ["&-status-warning"] = new Unknown_46()
                        {
                            [$"{componentCls}-group-addon"] = new Unknown_47()
                            {
                                Color = colorWarning,
                                BorderColor = colorWarning,
                            },
                        },
                        ["&-disabled"] = new Unknown_48()
                        {
                            [$"{componentCls}-group-addon"] = new Unknown_49()
                            {
                                ["..."] = GenDisabledStyle(token)
                            },
                        },
                        [$"&:not({componentCls}-compact-first-item):not({componentCls}-compact-last-item){componentCls}-compact-item"] = new Unknown_50()
                        {
                            [$"{componentCls}, {componentCls}-group-addon"] = new Unknown_51()
                            {
                                BorderRadius = 0,
                            },
                        },
                        [$"&:not({componentCls}-compact-last-item){componentCls}-compact-first-item"] = new Unknown_52()
                        {
                            [$"{componentCls}, {componentCls}-group-addon"] = new Unknown_53()
                            {
                                BorderStartEndRadius = 0,
                                BorderEndEndRadius = 0,
                            },
                        },
                        [$"&:not({componentCls}-compact-first-item){componentCls}-compact-last-item"] = new Unknown_54()
                        {
                            [$"{componentCls}, {componentCls}-group-addon"] = new Unknown_55()
                            {
                                BorderStartStartRadius = 0,
                                BorderEndStartRadius = 0,
                            },
                        },
                    },
                },
            };
        }

        public Unknown_5 GenSearchInputStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var searchPrefixCls = @$"{componentCls}-search";
            return new Unknown_56()
            {
                [searchPrefixCls] = new Unknown_57()
                {
                    [$"{componentCls}"] = new Unknown_58()
                    {
                        ["&:hover, &:focus"] = new Unknown_59()
                        {
                            BorderColor = token.ColorPrimaryHover,
                            [$"+ {componentCls}-group-addon {searchPrefixCls}-button:not({antCls}-btn-primary)"] = new Unknown_60()
                            {
                                BorderInlineStartColor = token.ColorPrimaryHover,
                            },
                        },
                    },
                    [$"{componentCls}-affix-wrapper"] = new Unknown_61()
                    {
                        BorderRadius = 0,
                    },
                    [$"{componentCls}-lg"] = new Unknown_62()
                    {
                        LineHeight = token.LineHeightLG - 0.0002,
                    },
                    [$"> {componentCls}-group"] = new Unknown_63()
                    {
                        [$"> {componentCls}-group-addon:last-child"] = new Unknown_64()
                        {
                            InsetInlineStart = -1,
                            Padding = 0,
                            Border = 0,
                            [$"{searchPrefixCls}-button"] = new Unknown_65()
                            {
                                PaddingTop = 0,
                                PaddingBottom = 0,
                                BorderStartStartRadius = 0,
                                BorderStartEndRadius = token.BorderRadius,
                                BorderEndEndRadius = token.BorderRadius,
                                BorderEndStartRadius = 0,
                            },
                            [$"{searchPrefixCls}-button:not({antCls}-btn-primary)"] = new Unknown_66()
                            {
                                Color = token.ColorTextDescription,
                                ["&:hover"] = new Unknown_67()
                                {
                                    Color = token.ColorPrimaryHover,
                                },
                                ["&:active"] = new Unknown_68()
                                {
                                    Color = token.ColorPrimaryActive,
                                },
                                [$"&{antCls}-btn-loading::before"] = new Unknown_69()
                                {
                                    InsetInlineStart = 0,
                                    InsetInlineEnd = 0,
                                    InsetBlockStart = 0,
                                    InsetBlockEnd = 0,
                                },
                            },
                        },
                    },
                    [$"{searchPrefixCls}-button"] = new Unknown_70()
                    {
                        Height = token.ControlHeight,
                        ["&:hover, &:focus"] = new Unknown_71()
                        {
                            ZIndex = 1,
                        },
                    },
                    [$"&-large {searchPrefixCls}-button"] = new Unknown_72()
                    {
                        Height = token.ControlHeightLG,
                    },
                    [$"&-small {searchPrefixCls}-button"] = new Unknown_73()
                    {
                        Height = token.ControlHeightSM,
                    },
                    ["&-rtl"] = new Unknown_74()
                    {
                        Direction = "rtl",
                    },
                    [$"&{componentCls}-compact-item"] = new Unknown_75()
                    {
                        [$"&:not({componentCls}-compact-last-item)"] = new Unknown_76()
                        {
                            [$"{componentCls}-group-addon"] = new Unknown_77()
                            {
                                [$"{componentCls}-search-button"] = new Unknown_78()
                                {
                                    MarginInlineEnd = -token.LineWidth,
                                    BorderRadius = 0,
                                },
                            },
                        },
                        [$"&:not({componentCls}-compact-first-item)"] = new Unknown_79()
                        {
                            [$"{componentCls},{componentCls}-affix-wrapper"] = new Unknown_80()
                            {
                                BorderRadius = 0,
                            },
                        },
                        [$">{componentCls}-group-addon{componentCls}-search-button,>{componentCls},{componentCls}-affix-wrapper"] = new Unknown_81()
                        {
                            ["&:hover,&:focus,&:active"] = new Unknown_82()
                            {
                                ZIndex = 2,
                            },
                        },
                        [$"> {componentCls}-affix-wrapper-focused"] = new Unknown_83()
                        {
                            ZIndex = 2,
                        },
                    },
                },
            };
        }

        public InputToken<T> InitInputToken(T token)
        {
            return MergeToken(
                token,
                new Unknown_84()
                {
                    InputAffixPadding = token.PaddingXXS,
                    InputPaddingVertical = Math.Max(Math.Round(((token.ControlHeight - token.FontSize * token.LineHeight) / 2) * 10) / 10 -
        token.LineWidth, 3),
                    InputPaddingVerticalLG = Math.Ceil(((token.ControlHeightLG - token.FontSizeLG * token.LineHeightLG) / 2) * 10) / 10 -
      token.LineWidth,
                    InputPaddingVerticalSM = Math.Max(Math.Round(((token.ControlHeightSM - token.FontSize * token.LineHeight) / 2) * 10) / 10 -
        token.LineWidth, 0),
                    InputPaddingHorizontal = token.PaddingSM - token.LineWidth,
                    InputPaddingHorizontalSM = token.PaddingXS - token.LineWidth,
                    InputPaddingHorizontalLG = token.ControlPaddingHorizontal - token.LineWidth,
                    InputBorderHoverColor = token.ColorPrimaryHover,
                    InputBorderActiveColor = token.ColorPrimaryHover,
                });
        }

        public Unknown_6 GenTextAreaStyle(Unknown_85 token)
        {
            var componentCls = token.ComponentCls;
            var paddingLG = token.PaddingLG;
            var textareaPrefixCls = @$"{componentCls}-textarea";
            return new Unknown_86()
            {
                [textareaPrefixCls] = new Unknown_87()
                {
                    Position = "relative",
                    ["&-show-count"] = new Unknown_88()
                    {
                        [$"> {componentCls}"] = new Unknown_89()
                        {
                            Height = "100%",
                        },
                        [$"{componentCls}-data-count"] = new Unknown_90()
                        {
                            Position = "absolute",
                            Bottom = -token.FontSize * token.LineHeight,
                            InsetInlineEnd = 0,
                            Color = token.ColorTextDescription,
                            WhiteSpace = "nowrap",
                            PointerEvents = "none",
                        },
                    },
                    ["&-allow-clear"] = new Unknown_91()
                    {
                        [$"> {componentCls}"] = new Unknown_92()
                        {
                            PaddingInlineEnd = paddingLG,
                        },
                    },
                    [$"&-affix-wrapper{textareaPrefixCls}-has-feedback"] = new Unknown_93()
                    {
                        [$"{componentCls}"] = new Unknown_94()
                        {
                            PaddingInlineEnd = paddingLG,
                        },
                    },
                    [$"&-affix-wrapper{componentCls}-affix-wrapper"] = new Unknown_95()
                    {
                        Padding = 0,
                        [$"> textarea{componentCls}"] = new Unknown_96()
                        {
                            FontSize = "inherit",
                            Border = "none",
                            Outline = "none",
                            ["&:focus"] = new Unknown_97()
                            {
                                BoxShadow = "none !important",
                            },
                        },
                        [$"{componentCls}-suffix"] = new Unknown_98()
                        {
                            Margin = 0,
                            ["> *:not(:last-child)"] = new Unknown_99()
                            {
                                MarginInline = 0,
                            },
                            [$"{componentCls}-clear-icon"] = new Unknown_100()
                            {
                                Position = "absolute",
                                InsetInlineEnd = token.PaddingXS,
                                InsetBlockStart = token.PaddingXS,
                            },
                            [$"{textareaPrefixCls}-suffix"] = new Unknown_101()
                            {
                                Position = "absolute",
                                Top = 0,
                                InsetInlineEnd = token.InputPaddingHorizontal,
                                Bottom = 0,
                                ZIndex = 1,
                                Display = "inline-flex",
                                AlignItems = "center",
                                Margin = "auto",
                                PointerEvents = "none",
                            },
                        },
                    },
                },
            };
        }

        public Unknown_7 GenComponentStyleHook(Unknown_102 token)
        {
            var inputToken = InitInputToken(token);
            return new Unknown_103
            {
                GenInputStyle(inputToken),
                GenTextAreaStyle(inputToken),
                GenAffixStyle(inputToken),
                GenGroupStyle(inputToken),
                GenSearchInputStyle(inputToken),
                GenCompactItemStyle(inputToken)
            };
        }

    }

}