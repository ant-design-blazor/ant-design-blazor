using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class FormToken : TokenWithCommonCls
    {
        public string FormItemCls { get; set; }

        public string RootPrefixCls { get; set; }

    }

    public partial class Form
    {
        public CSSObject ResetForm(AliasToken token)
        {
            return new CSSObject()
            {
                Legend = new CSSObject()
                {
                    Display = "block",
                    Width = "100%",
                    MarginBottom = token.MarginLG,
                    Padding = 0,
                    Color = token.ColorTextDescription,
                    FontSize = token.FontSizeLG,
                    LineHeight = "inherit",
                    Border = 0,
                    BorderBottom = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                },
                Label = new CSSObject()
                {
                    FontSize = token.FontSize,
                },
                ["input[type=\"search\"]"] = new CSSObject()
                {
                    BoxSizing = "border-box",
                },
                ["input[type=\"radio\"], input[type=\"checkbox\"]"] = new CSSObject()
                {
                    LineHeight = "normal",
                },
                ["input[type=\"file\"]"] = new CSSObject()
                {
                    Display = "block",
                },
                ["input[type=\"range\"]"] = new CSSObject()
                {
                    Display = "block",
                    Width = "100%",
                },
                ["select[multiple], select[size]"] = new CSSObject()
                {
                    Height = "auto",
                },
                ["input[type=\"file\"]:focus,input[type=\"radio\"]:focus,input[type=\"checkbox\"]:focus"] = new CSSObject()
                {
                    Outline = 0,
                    BoxShadow = @$"0 0 0 {token.ControlOutlineWidth}px {token.ControlOutline}",
                },
                Output = new CSSObject()
                {
                    Display = "block",
                    PaddingTop = 15,
                    Color = token.ColorText,
                    FontSize = token.FontSize,
                    LineHeight = token.LineHeight,
                },
            };
        }

        public CSSObject GenFormSize(FormToken token, int height)
        {
            var formItemCls = token.FormItemCls;
            return new CSSObject()
            {
                [formItemCls] = new CSSObject()
                {
                    [$"{formItemCls}-label > label"] = new CSSObject()
                    {
                        Height = height,
                    },
                    [$"{formItemCls}-control-input"] = new CSSObject()
                    {
                        MinHeight = height,
                    },
                },
            };
        }

        public Unknown_1 GenFormStyle(Unknown_8 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_9()
            {
                [token.ComponentCls] = new Unknown_10()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = ResetForm(token),
                    [$"{componentCls}-text"] = new Unknown_11()
                    {
                        Display = "inline-block",
                        PaddingInlineEnd = token.PaddingSM,
                    },
                    ["&-small"] = new Unknown_12()
                    {
                        ["..."] = GenFormSize(token, token.ControlHeightSM)
                    },
                    ["&-large"] = new Unknown_13()
                    {
                        ["..."] = GenFormSize(token, token.ControlHeightLG)
                    },
                },
            };
        }

        public Unknown_2 GenFormItemStyle(Unknown_14 token)
        {
            var formItemCls = token.FormItemCls;
            var iconCls = token.IconCls;
            var componentCls = token.ComponentCls;
            var rootPrefixCls = token.RootPrefixCls;
            return new Unknown_15()
            {
                [formItemCls] = new Unknown_16()
                {
                    ["..."] = ResetComponent(token),
                    MarginBottom = token.MarginLG,
                    VerticalAlign = "top",
                    ["&-with-help"] = new Unknown_17()
                    {
                        Transition = "none",
                    },
                    [$"&-hidden,&-hidden.{rootPrefixCls}-row"] = new Unknown_18()
                    {
                        Display = "none",
                    },
                    ["&-has-warning"] = new Unknown_19()
                    {
                        [$"{formItemCls}-split"] = new Unknown_20()
                        {
                            Color = token.ColorError,
                        },
                    },
                    ["&-has-error"] = new Unknown_21()
                    {
                        [$"{formItemCls}-split"] = new Unknown_22()
                        {
                            Color = token.ColorWarning,
                        },
                    },
                    [$"{formItemCls}-label"] = new Unknown_23()
                    {
                        Display = "inline-block",
                        FlexGrow = 0,
                        Overflow = "hidden",
                        WhiteSpace = "nowrap",
                        TextAlign = "end",
                        VerticalAlign = "middle",
                        ["&-left"] = new Unknown_24()
                        {
                            TextAlign = "start",
                        },
                        ["&-wrap"] = new Unknown_25()
                        {
                            Overflow = "unset",
                            LineHeight = @$"{token.LineHeight} - 0.25em",
                            WhiteSpace = "unset",
                        },
                        ["> label"] = new Unknown_26()
                        {
                            Position = "relative",
                            Display = "inline-flex",
                            AlignItems = "center",
                            MaxWidth = "100%",
                            Height = token.ControlHeight,
                            Color = token.ColorTextHeading,
                            FontSize = token.FontSize,
                            [$"> {iconCls}"] = new Unknown_27()
                            {
                                FontSize = token.FontSize,
                                VerticalAlign = "top",
                            },
                            [$"&{formItemCls}-required:not({formItemCls}-required-mark-optional)::before"] = new Unknown_28()
                            {
                                Display = "inline-block",
                                MarginInlineEnd = token.MarginXXS,
                                Color = token.ColorError,
                                FontSize = token.FontSize,
                                FontFamily = "SimSun, sans-serif",
                                LineHeight = 1,
                                Content = "\"*\"",
                                [$"{componentCls}-hide-required-mark &"] = new Unknown_29()
                                {
                                    Display = "none",
                                },
                            },
                            [$"{formItemCls}-optional"] = new Unknown_30()
                            {
                                Display = "inline-block",
                                MarginInlineStart = token.MarginXXS,
                                Color = token.ColorTextDescription,
                                [$"{componentCls}-hide-required-mark &"] = new Unknown_31()
                                {
                                    Display = "none",
                                },
                            },
                            [$"{formItemCls}-tooltip"] = new Unknown_32()
                            {
                                Color = token.ColorTextDescription,
                                Cursor = "help",
                                WritingMode = "horizontal-tb",
                                MarginInlineStart = token.MarginXXS,
                            },
                            ["&::after"] = new Unknown_33()
                            {
                                Content = "\":\"",
                                Position = "relative",
                                MarginBlock = 0,
                                MarginInlineStart = token.MarginXXS / 2,
                                MarginInlineEnd = token.MarginXS,
                            },
                            [$"&{formItemCls}-no-colon::after"] = new Unknown_34()
                            {
                                Content = "\" \"",
                            },
                        },
                    },
                    [$"{formItemCls}-control"] = new Unknown_35()
                    {
                        Display = "flex",
                        FlexDirection = "column",
                        FlexGrow = 1,
                        [$"&:first-child:not([class^=""{rootPrefixCls}-col-""]):not([class*="" {rootPrefixCls}-col-""])"] = new Unknown_36()
                        {
                            Width = "100%",
                        },
                        ["&-input"] = new Unknown_37()
                        {
                            Position = "relative",
                            Display = "flex",
                            AlignItems = "center",
                            MinHeight = token.ControlHeight,
                            ["&-content"] = new Unknown_38()
                            {
                                Flex = "auto",
                                MaxWidth = "100%",
                            },
                        },
                    },
                    [formItemCls] = new Unknown_39()
                    {
                        ["&-explain, &-extra"] = new Unknown_40()
                        {
                            Clear = "both",
                            Color = token.ColorTextDescription,
                            FontSize = token.FontSize,
                            LineHeight = token.LineHeight,
                        },
                        ["&-explain-connected"] = new Unknown_41()
                        {
                            Width = "100%",
                        },
                        ["&-extra"] = new Unknown_42()
                        {
                            MinHeight = token.ControlHeightSM,
                            Transition = @$"color {token.MotionDurationMid} {token.MotionEaseOut}",
                        },
                        ["&-explain"] = new Unknown_43()
                        {
                            ["&-error"] = new Unknown_44()
                            {
                                Color = token.ColorError,
                            },
                            ["&-warning"] = new Unknown_45()
                            {
                                Color = token.ColorWarning,
                            },
                        },
                    },
                    [$"&-with-help {formItemCls}-explain"] = new Unknown_46()
                    {
                        Height = "auto",
                        Opacity = 1,
                    },
                    [$"{formItemCls}-feedback-icon"] = new Unknown_47()
                    {
                        FontSize = token.FontSize,
                        TextAlign = "center",
                        Visibility = "visible",
                        AnimationName = zoomIn,
                        AnimationDuration = token.MotionDurationMid,
                        AnimationTimingFunction = token.MotionEaseOutBack,
                        PointerEvents = "none",
                        ["&-success"] = new Unknown_48()
                        {
                            Color = token.ColorSuccess,
                        },
                        ["&-error"] = new Unknown_49()
                        {
                            Color = token.ColorError,
                        },
                        ["&-warning"] = new Unknown_50()
                        {
                            Color = token.ColorWarning,
                        },
                        ["&-validating"] = new Unknown_51()
                        {
                            Color = token.ColorPrimary,
                        },
                    },
                },
            };
        }

        public Unknown_3 GenHorizontalStyle(Unknown_52 token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            var rootPrefixCls = token.RootPrefixCls;
            return new Unknown_53()
            {
                [$"{componentCls}-horizontal"] = new Unknown_54()
                {
                    [$"{formItemCls}-label"] = new Unknown_55()
                    {
                        FlexGrow = 0,
                    },
                    [$"{formItemCls}-control"] = new Unknown_56()
                    {
                        Flex = "1 1 0",
                        MinWidth = 0,
                    },
                    [$"{formItemCls}-label.{rootPrefixCls}-col-24 + {formItemCls}-control"] = new Unknown_57()
                    {
                        MinWidth = "unset",
                    },
                },
            };
        }

        public Unknown_4 GenInlineStyle(Unknown_58 token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            return new Unknown_59()
            {
                [$"{componentCls}-inline"] = new Unknown_60()
                {
                    Display = "flex",
                    FlexWrap = "wrap",
                    [formItemCls] = new Unknown_61()
                    {
                        Flex = "none",
                        MarginInlineEnd = token.Margin,
                        MarginBottom = 0,
                        ["&-row"] = new Unknown_62()
                        {
                            FlexWrap = "nowrap",
                        },
                        ["&-with-help"] = new Unknown_63()
                        {
                            MarginBottom = token.MarginLG,
                        },
                        [$">{formItemCls}-label,>{formItemCls}-control"] = new Unknown_64()
                        {
                            Display = "inline-block",
                            VerticalAlign = "top",
                        },
                        [$"> {formItemCls}-label"] = new Unknown_65()
                        {
                            Flex = "none",
                        },
                        [$"{componentCls}-text"] = new Unknown_66()
                        {
                            Display = "inline-block",
                        },
                        [$"{formItemCls}-has-feedback"] = new Unknown_67()
                        {
                            Display = "inline-block",
                        },
                    },
                },
            };
        }

        public CSSObject MakeVerticalLayoutLabel(FormToken token)
        {
            return new CSSObject()
            {
                Margin = 0,
                Padding = @$"0 0 {token.PaddingXS}px",
                WhiteSpace = "initial",
                TextAlign = "start",
                ["> label"] = new CSSObject()
                {
                    Margin = 0,
                    ["&::after"] = new CSSObject()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public CSSObject MakeVerticalLayout(FormToken token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            return new CSSObject()
            {
                [$"{formItemCls} {formItemCls}-label"] = MakeVerticalLayoutLabel(token),
                [componentCls] = new CSSObject()
                {
                    [formItemCls] = new CSSObject()
                    {
                        FlexWrap = "wrap",
                        [$"{formItemCls}-label,{formItemCls}-control"] = new CSSObject()
                        {
                            Flex = "0 0 100%",
                            MaxWidth = "100%",
                        },
                    },
                },
            };
        }

        public Unknown_5 GenVerticalStyle(Unknown_68 token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            var rootPrefixCls = token.RootPrefixCls;
            return new Unknown_69()
            {
                [$"{componentCls}-vertical"] = new Unknown_70()
                {
                    [formItemCls] = new Unknown_71()
                    {
                        ["&-row"] = new Unknown_72()
                        {
                            FlexDirection = "column",
                        },
                        ["&-label > label"] = new Unknown_73()
                        {
                            Height = "auto",
                        },
                        [$"{componentCls}-item-control"] = new Unknown_74()
                        {
                            Width = "100%",
                        },
                    },
                },
                [$"{componentCls}-vertical{formItemCls}-label,.{rootPrefixCls}-col-24{formItemCls}-label,.{rootPrefixCls}-col-xl-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token),
                [$"@media (max-width: {token.ScreenXSMax}px)"] = new Unknown_75
                {
                    MakeVerticalLayout(token),
                    new Unknown_76()
                    {
                        [componentCls] = new Unknown_77()
                        {
                            [$".{rootPrefixCls}-col-xs-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                        },
                    },
                }
                [$"@media (max-width: {token.ScreenSMMax}px)"] = new Unknown_78()
                {
                    [componentCls] = new Unknown_79()
                    {
                        [$".{rootPrefixCls}-col-sm-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                    },
                },
                [$"@media (max-width: {token.ScreenMDMax}px)"] = new Unknown_80()
                {
                    [componentCls] = new Unknown_81()
                    {
                        [$".{rootPrefixCls}-col-md-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                    },
                },
                [$"@media (max-width: {token.ScreenLGMax}px)"] = new Unknown_82()
                {
                    [componentCls] = new Unknown_83()
                    {
                        [$".{rootPrefixCls}-col-lg-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                    },
                },
            };
        }

        public Unknown_6 GenComponentStyleHook(Unknown_84 token, Unknown_85 args)
        {
            var formToken = MergeToken(
                token,
                new Unknown_86()
                {
                    FormItemCls = @$"{token.ComponentCls}-item",
                    RootPrefixCls = rootPrefixCls,
                });
            return new Unknown_87
            {
                GenFormStyle(formToken),
                GenFormItemStyle(formToken),
                GenFormValidateMotionStyle(formToken),
                GenHorizontalStyle(formToken),
                GenInlineStyle(formToken),
                GenVerticalStyle(formToken),
                GenCollapseMotion(formToken),
                ZoomIn
            };
        }

        public Unknown_7 GenFormValidateMotionStyle(Unknown_88 token)
        {
            var componentCls = token.ComponentCls;
            var helpCls = @$"{componentCls}-show-help";
            var helpItemCls = @$"{componentCls}-show-help-item";
            return new Unknown_89()
            {
                [helpCls] = new Unknown_90()
                {
                    Transition = @$"opacity {token.MotionDurationSlow} {token.MotionEaseInOut}",
                    ["&-appear, &-enter"] = new Unknown_91()
                    {
                        Opacity = 0,
                        ["&-active"] = new Unknown_92()
                        {
                            Opacity = 1,
                        },
                    },
                    ["&-leave"] = new Unknown_93()
                    {
                        Opacity = 1,
                        ["&-active"] = new Unknown_94()
                        {
                            Opacity = 0,
                        },
                    },
                    [helpItemCls] = new Unknown_95()
                    {
                        Overflow = "hidden",
                        Transition = @$"height {token.MotionDurationSlow} {token.MotionEaseInOut},
                     opacity {token.MotionDurationSlow} {token.MotionEaseInOut},
                     transform {token.MotionDurationSlow} {token.MotionEaseInOut} !important",
                        [$"&{helpItemCls}-appear, &{helpItemCls}-enter"] = new Unknown_96()
                        {
                            Transform = @$"translateY(-5px)",
                            Opacity = 0,
                            ["&-active"] = new Unknown_97()
                            {
                                Transform = "translateY(0)",
                                Opacity = 1,
                            },
                        },
                        [$"&{helpItemCls}-leave-active"] = new Unknown_98()
                        {
                            Transform = @$"translateY(-5px)",
                        },
                    },
                },
            };
        }

    }

}