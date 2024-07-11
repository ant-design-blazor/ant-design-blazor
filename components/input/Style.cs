using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.InputStyle;

namespace AntDesign
{
    public partial class SharedComponentToken : TokenWithCommonCls
    {
        public double PaddingInline
        {
            get => (double)_tokens["paddingInline"];
            set => _tokens["paddingInline"] = value;
        }

        public double PaddingInlineSM
        {
            get => (double)_tokens["paddingInlineSM"];
            set => _tokens["paddingInlineSM"] = value;
        }

        public double PaddingInlineLG
        {
            get => (double)_tokens["paddingInlineLG"];
            set => _tokens["paddingInlineLG"] = value;
        }

        public double PaddingBlock
        {
            get => (double)_tokens["paddingBlock"];
            set => _tokens["paddingBlock"] = value;
        }

        public double PaddingBlockSM
        {
            get => (double)_tokens["paddingBlockSM"];
            set => _tokens["paddingBlockSM"] = value;
        }

        public double PaddingBlockLG
        {
            get => (double)_tokens["paddingBlockLG"];
            set => _tokens["paddingBlockLG"] = value;
        }

        public string AddonBg
        {
            get => (string)_tokens["addonBg"];
            set => _tokens["addonBg"] = value;
        }

        public string HoverBorderColor
        {
            get => (string)_tokens["hoverBorderColor"];
            set => _tokens["hoverBorderColor"] = value;
        }

        public string ActiveBorderColor
        {
            get => (string)_tokens["activeBorderColor"];
            set => _tokens["activeBorderColor"] = value;
        }

        public string ActiveShadow
        {
            get => (string)_tokens["activeShadow"];
            set => _tokens["activeShadow"] = value;
        }

        public string ErrorActiveShadow
        {
            get => (string)_tokens["errorActiveShadow"];
            set => _tokens["errorActiveShadow"] = value;
        }

        public string WarningActiveShadow
        {
            get => (string)_tokens["warningActiveShadow"];
            set => _tokens["warningActiveShadow"] = value;
        }

        public string HoverBg
        {
            get => (string)_tokens["hoverBg"];
            set => _tokens["hoverBg"] = value;
        }

        public string ActiveBg
        {
            get => (string)_tokens["activeBg"];
            set => _tokens["activeBg"] = value;
        }

    }

    public partial class InputToken
    {
    }

    public partial class InputToken
    {
        public double InputAffixPadding
        {
            get => (double)_tokens["inputAffixPadding"];
            set => _tokens["inputAffixPadding"] = value;
        }

    }

    public partial class InputToken : SharedComponentToken
    {
    }

    public partial class InputStyle
    {
        public static CSSObject GenPlaceholderStyle(string color)
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

        public static CSSObject GenHoverStyle(InputToken token)
        {
            return new CSSObject()
            {
                BorderColor = token.HoverBorderColor,
                BackgroundColor = token.HoverBg,
            };
        }

        public static CSSObject GenActiveStyle(InputToken token)
        {
            return new CSSObject()
            {
                BorderColor = token.ActiveBorderColor,
                BoxShadow = token.ActiveShadow,
                Outline = 0,
                BackgroundColor = token.ActiveBg,
            };
        }

        public static CSSObject GenDisabledStyle(InputToken token)
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
                    ["..."] = GenHoverStyle(
                        MergeToken(
                            token,
                            new InputToken()
                            {
                                HoverBorderColor = token.ColorBorder,
                                HoverBg = token.ColorBgContainerDisabled,
                            }))
                },
            };
        }

        public static CSSObject GenInputLargeStyle(InputToken token)
        {
            var paddingBlockLG = token.PaddingBlockLG;
            var fontSizeLG = token.FontSizeLG;
            var lineHeightLG = token.LineHeightLG;
            var borderRadiusLG = token.BorderRadiusLG;
            var paddingInlineLG = token.PaddingInlineLG;
            return new CSSObject()
            {
                Padding = @$"{paddingBlockLG}px {paddingInlineLG}px",
                FontSize = fontSizeLG,
                LineHeight = lineHeightLG,
                BorderRadius = borderRadiusLG,
            };
        }

        public static CSSObject GenInputSmallStyle(InputToken token)
        {
            return new CSSObject()
            {
                Padding = @$"{token.PaddingBlockSM}px {token.PaddingInlineSM}px",
                BorderRadius = token.BorderRadiusSM,
            };
        }

        public static CSSObject GenStatusStyle(InputToken token, string parentCls)
        {
            var componentCls = token.ComponentCls;
            var colorError = token.ColorError;
            var colorWarning = token.ColorWarning;
            var errorActiveShadow = token.ErrorActiveShadow;
            var warningActiveShadow = token.WarningActiveShadow;
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
                    ["&:focus, &:focus-within"] = new CSSObject()
                    {
                        ["..."] = GenActiveStyle(
                            MergeToken(
                                token,
                                new InputToken()
                                {
                                    ActiveBorderColor = colorError,
                                    ActiveShadow = errorActiveShadow,
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
                    ["&:focus, &:focus-within"] = new CSSObject()
                    {
                        ["..."] = GenActiveStyle(
                            MergeToken(
                                token,
                                new InputToken()
                                {
                                    ActiveBorderColor = colorWarning,
                                    ActiveShadow = warningActiveShadow,
                                }))
                    },
                    [$"{componentCls}-prefix, {componentCls}-suffix"] = new CSSObject()
                    {
                        Color = colorWarning,
                    },
                },
            };
        }

        public static CSSObject GenBasicInputStyle(InputToken token)
        {
            return new CSSObject()
            {
                Position = "relative",
                Display = "inline-block",
                Width = "100%",
                MinWidth = 0,
                Padding = @$"{token.PaddingBlock}px {token.PaddingInline}px",
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
                ["&:focus, &:focus-within"] = new CSSObject()
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

        public static CSSObject GenInputGroupStyle(InputToken token)
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
                ["&[class*=\"col-\"]"] = new CSSObject()
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
                        Padding = @$"0 {token.PaddingInline}px",
                        Color = token.ColorText,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                        TextAlign = "center",
                        BackgroundColor = token.AddonBg,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        BorderRadius = token.BorderRadius,
                        Transition = @$"all {token.MotionDurationSlow}",
                        LineHeight = 1,
                        [$"{antCls}-select"] = new CSSObject()
                        {
                            Margin = @$"-{token.PaddingBlock + 1}px -{token.PaddingInline}px",
                            [$"&{antCls}-select-single:not({antCls}-select-customize-input):not({antCls}-pagination-size-changer)"] = new CSSObject()
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
                            Margin = @$"-9px -{token.PaddingInline}px",
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
                    [$"&>{componentCls}-affix-wrapper,&>{componentCls}-number-affix-wrapper,&>{antCls}-picker-range"] = new CSSObject()
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

        public static CSSObject GenInputStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var controlHeightSM = token.ControlHeightSM;
            var lineWidth = token.LineWidth;
            var fixedChromeColorHeight = 16;
            var colorSmallPadding = (controlHeightSM - lineWidth * 2 - fixedChromeColorHeight) / 2;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenBasicInputStyle(token),
                    ["..."] = GenStatusStyle(token, componentCls),
                    ["&[type=\"color\"]"] = new CSSObject()
                    {
                        Height = token.ControlHeight,
                        [$"&{componentCls}-lg"] = new CSSObject()
                        {
                            Height = token.ControlHeightLG,
                        },
                        [$"&{componentCls}-sm"] = new CSSObject()
                        {
                            Height = controlHeightSM,
                            PaddingTop = colorSmallPadding,
                            PaddingBottom = colorSmallPadding,
                        },
                    },
                    ["&[type=\"search\"]::-webkit-search-cancel-button, &[type=\"search\"]::-webkit-search-decoration"] = new CSSObject()
                    {
                        ["-webkit-appearance"] = "none",
                    },
                },
            };
        }

        public static CSSObject GenAllowClearStyle(InputToken token)
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

        public static CSSObject GenAffixStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var inputAffixPadding = token.InputAffixPadding;
            var colorTextDescription = token.ColorTextDescription;
            var motionDurationSlow = token.MotionDurationSlow;
            var colorIcon = token.ColorIcon;
            var colorIconHover = token.ColorIconHover;
            var iconCls = token.IconCls;
            return new CSSObject()
            {
                [$"{componentCls}-affix-wrapper"] = new CSSObject()
                {
                    ["..."] = GenBasicInputStyle(token),
                    Display = "inline-flex",
                    [$"&:not({componentCls}-affix-wrapper-disabled):hover"] = new CSSObject()
                    {
                        ZIndex = 1,
                        [$"{componentCls}-search-with-button &"] = new CSSObject()
                        {
                            ZIndex = 0,
                        },
                    },
                    ["&-focused, &:focus"] = new CSSObject()
                    {
                        ZIndex = 1,
                    },
                    ["&-disabled"] = new CSSObject()
                    {
                        [$"{componentCls}[disabled]"] = new CSSObject()
                        {
                            Background = "transparent",
                        },
                    },
                    [$"> input{componentCls}"] = new CSSObject()
                    {
                        Padding = 0,
                        FontSize = "inherit",
                        Border = "none",
                        BorderRadius = 0,
                        Outline = "none",
                        ["&::-ms-reveal"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        ["&:focus"] = new CSSObject()
                        {
                            BoxShadow = "none !important",
                        },
                    },
                    ["&::before"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = 0,
                        Visibility = "hidden",
                        Content = "'\\\\a0'",
                    },
                    [$"{componentCls}"] = new CSSObject()
                    {
                        ["&-prefix, &-suffix"] = new CSSObject()
                        {
                            Display = "flex",
                            Flex = "none",
                            AlignItems = "center",
                            ["> *:not(:last-child)"] = new CSSObject()
                            {
                                MarginInlineEnd = token.PaddingXS,
                            },
                        },
                        ["&-show-count-suffix"] = new CSSObject()
                        {
                            Color = colorTextDescription,
                        },
                        ["&-show-count-has-suffix"] = new CSSObject()
                        {
                            MarginInlineEnd = token.PaddingXXS,
                        },
                        ["&-prefix"] = new CSSObject()
                        {
                            MarginInlineEnd = inputAffixPadding,
                        },
                        ["&-suffix"] = new CSSObject()
                        {
                            MarginInlineStart = inputAffixPadding,
                        },
                    },
                    ["..."] = GenAllowClearStyle(token),
                    [$"{iconCls}{componentCls}-password-icon"] = new CSSObject()
                    {
                        Color = colorIcon,
                        Cursor = "pointer",
                        Transition = @$"all {motionDurationSlow}",
                        ["&:hover"] = new CSSObject()
                        {
                            Color = colorIconHover,
                        },
                    },
                    ["..."] = GenStatusStyle(token, $"{componentCls}-affix-wrapper")
                },
            };
        }

        public static CSSObject GenGroupStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var colorError = token.ColorError;
            var colorWarning = token.ColorWarning;
            var borderRadiusLG = token.BorderRadiusLG;
            var borderRadiusSM = token.BorderRadiusSM;
            return new CSSObject()
            {
                [$"{componentCls}-group"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenInputGroupStyle(token),
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    ["&-wrapper"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = "100%",
                        TextAlign = "start",
                        VerticalAlign = "top",
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
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
                        ["&-status-error"] = new CSSObject()
                        {
                            [$"{componentCls}-group-addon"] = new CSSObject()
                            {
                                Color = colorError,
                                BorderColor = colorError,
                            },
                        },
                        ["&-status-warning"] = new CSSObject()
                        {
                            [$"{componentCls}-group-addon"] = new CSSObject()
                            {
                                Color = colorWarning,
                                BorderColor = colorWarning,
                            },
                        },
                        ["&-disabled"] = new CSSObject()
                        {
                            [$"{componentCls}-group-addon"] = new CSSObject()
                            {
                                ["..."] = GenDisabledStyle(token)
                            },
                        },
                        [$"&:not({componentCls}-compact-first-item):not({componentCls}-compact-last-item){componentCls}-compact-item"] = new CSSObject()
                        {
                            [$"{componentCls}, {componentCls}-group-addon"] = new CSSObject()
                            {
                                BorderRadius = 0,
                            },
                        },
                        [$"&:not({componentCls}-compact-last-item){componentCls}-compact-first-item"] = new CSSObject()
                        {
                            [$"{componentCls}, {componentCls}-group-addon"] = new CSSObject()
                            {
                                BorderStartEndRadius = 0,
                                BorderEndEndRadius = 0,
                            },
                        },
                        [$"&:not({componentCls}-compact-first-item){componentCls}-compact-last-item"] = new CSSObject()
                        {
                            [$"{componentCls}, {componentCls}-group-addon"] = new CSSObject()
                            {
                                BorderStartStartRadius = 0,
                                BorderEndStartRadius = 0,
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenSearchInputStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var searchPrefixCls = @$"{componentCls}-search";
            return new CSSObject()
            {
                [searchPrefixCls] = new CSSObject()
                {
                    [$"{componentCls}"] = new CSSObject()
                    {
                        ["&:hover, &:focus"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                            [$"+ {componentCls}-group-addon {searchPrefixCls}-button:not({antCls}-btn-primary)"] = new CSSObject()
                            {
                                BorderInlineStartColor = token.ColorPrimaryHover,
                            },
                        },
                    },
                    [$"{componentCls}-affix-wrapper"] = new CSSObject()
                    {
                        BorderRadius = 0,
                    },
                    [$"{componentCls}-lg"] = new CSSObject()
                    {
                        LineHeight = token.LineHeightLG - 0.0002,
                    },
                    [$"> {componentCls}-group"] = new CSSObject()
                    {
                        [$"> {componentCls}-group-addon:last-child"] = new CSSObject()
                        {
                            InsetInlineStart = -1,
                            Padding = 0,
                            Border = 0,
                            [$"{searchPrefixCls}-button"] = new CSSObject()
                            {
                                PaddingTop = 0,
                                PaddingBottom = 0,
                                BorderStartStartRadius = 0,
                                BorderStartEndRadius = token.BorderRadius,
                                BorderEndEndRadius = token.BorderRadius,
                                BorderEndStartRadius = 0,
                                BoxShadow = "none",
                            },
                            [$"{searchPrefixCls}-button:not({antCls}-btn-primary)"] = new CSSObject()
                            {
                                Color = token.ColorTextDescription,
                                ["&:hover"] = new CSSObject()
                                {
                                    Color = token.ColorPrimaryHover,
                                },
                                ["&:active"] = new CSSObject()
                                {
                                    Color = token.ColorPrimaryActive,
                                },
                                [$"&{antCls}-btn-loading::before"] = new CSSObject()
                                {
                                    InsetInlineStart = 0,
                                    InsetInlineEnd = 0,
                                    InsetBlockStart = 0,
                                    InsetBlockEnd = 0,
                                },
                            },
                        },
                    },
                    [$"{searchPrefixCls}-button"] = new CSSObject()
                    {
                        Height = token.ControlHeight,
                        ["&:hover, &:focus"] = new CSSObject()
                        {
                            ZIndex = 1,
                        },
                    },
                    [$"&-large {searchPrefixCls}-button"] = new CSSObject()
                    {
                        Height = token.ControlHeightLG,
                    },
                    [$"&-small {searchPrefixCls}-button"] = new CSSObject()
                    {
                        Height = token.ControlHeightSM,
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"&{componentCls}-compact-item"] = new CSSObject()
                    {
                        [$"&:not({componentCls}-compact-last-item)"] = new CSSObject()
                        {
                            [$"{componentCls}-group-addon"] = new CSSObject()
                            {
                                [$"{componentCls}-search-button"] = new CSSObject()
                                {
                                    MarginInlineEnd = -token.LineWidth,
                                    BorderRadius = 0,
                                },
                            },
                        },
                        [$"&:not({componentCls}-compact-first-item)"] = new CSSObject()
                        {
                            [$"{componentCls},{componentCls}-affix-wrapper"] = new CSSObject()
                            {
                                BorderRadius = 0,
                            },
                        },
                        [$">{componentCls}-group-addon{componentCls}-search-button,>{componentCls},{componentCls}-affix-wrapper"] = new CSSObject()
                        {
                            ["&:hover,&:focus,&:active"] = new CSSObject()
                            {
                                ZIndex = 2,
                            },
                        },
                        [$"> {componentCls}-affix-wrapper-focused"] = new CSSObject()
                        {
                            ZIndex = 2,
                        },
                    },
                },
            };
        }

        public static CSSObject GenTextAreaStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            var paddingLG = token.PaddingLG;
            var textareaPrefixCls = @$"{componentCls}-textarea";
            return new CSSObject()
            {
                [textareaPrefixCls] = new CSSObject()
                {
                    Position = "relative",
                    ["&-show-count"] = new CSSObject()
                    {
                        [$"> {componentCls}"] = new CSSObject()
                        {
                            Height = "100%",
                        },
                        [$"{componentCls}-data-count"] = new CSSObject()
                        {
                            Position = "absolute",
                            Bottom = -token.FontSize * token.LineHeight,
                            InsetInlineEnd = 0,
                            Color = token.ColorTextDescription,
                            WhiteSpace = "nowrap",
                            PointerEvents = "none",
                        },
                    },
                    ["&-allow-clear"] = new CSSObject()
                    {
                        [$"> {componentCls}"] = new CSSObject()
                        {
                            PaddingInlineEnd = paddingLG,
                        },
                    },
                    [$"&-affix-wrapper{textareaPrefixCls}-has-feedback"] = new CSSObject()
                    {
                        [$"{componentCls}"] = new CSSObject()
                        {
                            PaddingInlineEnd = paddingLG,
                        },
                    },
                    [$"&-affix-wrapper{componentCls}-affix-wrapper"] = new CSSObject()
                    {
                        Padding = 0,
                        [$"> textarea{componentCls}"] = new CSSObject()
                        {
                            FontSize = "inherit",
                            Border = "none",
                            Outline = "none",
                            ["&:focus"] = new CSSObject()
                            {
                                BoxShadow = "none !important",
                            },
                        },
                        [$"{componentCls}-suffix"] = new CSSObject()
                        {
                            Margin = 0,
                            ["> *:not(:last-child)"] = new CSSObject()
                            {
                                MarginInline = 0,
                            },
                            [$"{componentCls}-clear-icon"] = new CSSObject()
                            {
                                Position = "absolute",
                                InsetInlineEnd = token.PaddingXS,
                                InsetBlockStart = token.PaddingXS,
                            },
                            [$"{textareaPrefixCls}-suffix"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = 0,
                                InsetInlineEnd = token.PaddingInline,
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

        public static CSSObject GenRangeStyle(InputToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-out-of-range"] = new CSSObject()
                {
                    [$"&, & input, & textarea, {componentCls}-show-count-suffix, {componentCls}-data-count"] = new CSSObject()
                    {
                        Color = token.ColorError,
                    },
                },
            };
        }

        public static InputToken InitInputToken(GlobalToken token)
        {
            return MergeToken(
                token,
                new InputToken()
                {
                    InputAffixPadding = token.PaddingXXS,
                });
        }

        public static SharedComponentToken InitComponentToken(GlobalToken token)
        {
            var controlHeight = token.ControlHeight;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var lineWidth = token.LineWidth;
            var controlHeightSM = token.ControlHeightSM;
            var controlHeightLG = token.ControlHeightLG;
            var fontSizeLG = token.FontSizeLG;
            var lineHeightLG = token.LineHeightLG;
            var paddingSM = token.PaddingSM;
            var controlPaddingHorizontalSM = token.ControlPaddingHorizontalSM;
            var controlPaddingHorizontal = token.ControlPaddingHorizontal;
            var colorFillAlter = token.ColorFillAlter;
            var colorPrimaryHover = token.ColorPrimaryHover;
            var colorPrimary = token.ColorPrimary;
            var controlOutlineWidth = token.ControlOutlineWidth;
            var controlOutline = token.ControlOutline;
            var colorErrorOutline = token.ColorErrorOutline;
            var colorWarningOutline = token.ColorWarningOutline;
            return new SharedComponentToken()
            {
                PaddingBlock = Math.Max(
                    Math.Round(((controlHeight - fontSize * lineHeight) / 2) * 10) / 10 - lineWidth,
                    0
                ),
                PaddingBlockSM = Math.Max(
                    Math.Round(((controlHeightSM - fontSize * lineHeight) / 2) * 10) / 10 - lineWidth,
                    0
                ),
                PaddingBlockLG = Math.Ceiling(((controlHeightLG - fontSizeLG * lineHeightLG) / 2) * 10) / 10 - lineWidth,
                PaddingInline = paddingSM - lineWidth,
                PaddingInlineSM = controlPaddingHorizontalSM - lineWidth,
                PaddingInlineLG = controlPaddingHorizontal - lineWidth,
                AddonBg = colorFillAlter,
                ActiveBorderColor = colorPrimary,
                HoverBorderColor = colorPrimaryHover,
                ActiveShadow = @$"0 0 0 {controlOutlineWidth}px {controlOutline}",
                ErrorActiveShadow = @$"0 0 0 {controlOutlineWidth}px {colorErrorOutline}",
                WarningActiveShadow = @$"0 0 0 {controlOutlineWidth}px {colorWarningOutline}",
                HoverBg = "",
                ActiveBg = "",
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Input",
                (token) =>
                {
                    var inputToken = MergeToken(
                        token,
                        InitInputToken(token));
                    return new CSSInterpolation[]
                    {
                        GenInputStyle(inputToken),
                        GenTextAreaStyle(inputToken),
                        GenAffixStyle(inputToken),
                        GenGroupStyle(inputToken),
                        GenSearchInputStyle(inputToken),
                        GenRangeStyle(inputToken),
                        GenCompactItemStyle(inputToken),
                    };
                },
                InitComponentToken);
        }
    }
}
