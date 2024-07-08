using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Zoom;
using static AntDesign.CollapseMotion;

namespace AntDesign
{
    public partial class FormToken
    {
        public string LabelRequiredMarkColor
        {
            get => (string)_tokens["labelRequiredMarkColor"];
            set => _tokens["labelRequiredMarkColor"] = value;
        }

        public string LabelColor
        {
            get => (string)_tokens["labelColor"];
            set => _tokens["labelColor"] = value;
        }

        public double LabelFontSize
        {
            get => (double)_tokens["labelFontSize"];
            set => _tokens["labelFontSize"] = value;
        }

        public double LabelHeight
        {
            get => (double)_tokens["labelHeight"];
            set => _tokens["labelHeight"] = value;
        }

        public double LabelColonMarginInlineStart
        {
            get => (double)_tokens["labelColonMarginInlineStart"];
            set => _tokens["labelColonMarginInlineStart"] = value;
        }

        public double LabelColonMarginInlineEnd
        {
            get => (double)_tokens["labelColonMarginInlineEnd"];
            set => _tokens["labelColonMarginInlineEnd"] = value;
        }

        public double ItemMarginBottom
        {
            get => (double)_tokens["itemMarginBottom"];
            set => _tokens["itemMarginBottom"] = value;
        }

        public string VerticalLabelPadding
        {
            get => (string)_tokens["verticalLabelPadding"];
            set => _tokens["verticalLabelPadding"] = value;
        }

        public double VerticalLabelMargin
        {
            get => (double)_tokens["verticalLabelMargin"];
            set => _tokens["verticalLabelMargin"] = value;
        }

    }

    public partial class FormToken : TokenWithCommonCls
    {
        public string FormItemCls
        {
            get => (string)_tokens["formItemCls"];
            set => _tokens["formItemCls"] = value;
        }

        public string RootPrefixCls
        {
            get => (string)_tokens["rootPrefixCls"];
            set => _tokens["rootPrefixCls"] = value;
        }

    }

    public class FormStyle
    {
        public static CSSObject ResetForm(FormToken token)
        {
            return new CSSObject()
            {
                ["legend"] = new CSSObject()
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
                ["label"] = new CSSObject()
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
                ["output"] = new CSSObject()
                {
                    Display = "block",
                    PaddingTop = 15,
                    Color = token.ColorText,
                    FontSize = token.FontSize,
                    LineHeight = token.LineHeight,
                },
            };
        }

        public static CSSObject GenFormSize(FormToken token, double height)
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

        public static CSSObject GenFormStyle(FormToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [token.ComponentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = ResetForm(token),
                    [$"{componentCls}-text"] = new CSSObject()
                    {
                        Display = "inline-block",
                        PaddingInlineEnd = token.PaddingSM,
                    },
                    ["&-small"] = new CSSObject()
                    {
                        ["..."] = GenFormSize(token, token.ControlHeightSM)
                    },
                    ["&-large"] = new CSSObject()
                    {
                        ["..."] = GenFormSize(token, token.ControlHeightLG)
                    },
                },
            };
        }

        public static CSSObject GenFormItemStyle(FormToken token)
        {
            var formItemCls = token.FormItemCls;
            var iconCls = token.IconCls;
            var componentCls = token.ComponentCls;
            var rootPrefixCls = token.RootPrefixCls;
            var labelRequiredMarkColor = token.LabelRequiredMarkColor;
            var labelColor = token.LabelColor;
            var labelFontSize = token.LabelFontSize;
            var labelHeight = token.LabelHeight;
            var labelColonMarginInlineStart = token.LabelColonMarginInlineStart;
            var labelColonMarginInlineEnd = token.LabelColonMarginInlineEnd;
            var itemMarginBottom = token.ItemMarginBottom;
            return new CSSObject()
            {
                [formItemCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    MarginBottom = itemMarginBottom,
                    VerticalAlign = "top",
                    ["&-with-help"] = new CSSObject()
                    {
                        Transition = "none",
                    },
                    [$"&-hidden,&-hidden.{rootPrefixCls}-row"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    ["&-has-warning"] = new CSSObject()
                    {
                        [$"{formItemCls}-split"] = new CSSObject()
                        {
                            Color = token.ColorError,
                        },
                    },
                    ["&-has-error"] = new CSSObject()
                    {
                        [$"{formItemCls}-split"] = new CSSObject()
                        {
                            Color = token.ColorWarning,
                        },
                    },
                    [$"{formItemCls}-label"] = new CSSObject()
                    {
                        FlexGrow = 0,
                        Overflow = "hidden",
                        WhiteSpace = "nowrap",
                        TextAlign = "end",
                        VerticalAlign = "middle",
                        ["&-left"] = new CSSObject()
                        {
                            TextAlign = "start",
                        },
                        ["&-wrap"] = new CSSObject()
                        {
                            Overflow = "unset",
                            LineHeight = @$"{token.LineHeight} - 0.25em",
                            WhiteSpace = "unset",
                        },
                        ["> label"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-flex",
                            AlignItems = "center",
                            MaxWidth = "100%",
                            Height = labelHeight,
                            Color = labelColor,
                            FontSize = labelFontSize,
                            [$"> {iconCls}"] = new CSSObject()
                            {
                                FontSize = token.FontSize,
                                VerticalAlign = "top",
                            },
                            [$"&{formItemCls}-required:not({formItemCls}-required-mark-optional)::before"] = new CSSObject()
                            {
                                Display = "inline-block",
                                MarginInlineEnd = token.MarginXXS,
                                Color = labelRequiredMarkColor,
                                FontSize = token.FontSize,
                                FontFamily = "SimSun, sans-serif",
                                LineHeight = 1,
                                Content = "\"*\"",
                                [$"{componentCls}-hide-required-mark &"] = new CSSObject()
                                {
                                    Display = "none",
                                },
                            },
                            [$"{formItemCls}-optional"] = new CSSObject()
                            {
                                Display = "inline-block",
                                MarginInlineStart = token.MarginXXS,
                                Color = token.ColorTextDescription,
                                [$"{componentCls}-hide-required-mark &"] = new CSSObject()
                                {
                                    Display = "none",
                                },
                            },
                            [$"{formItemCls}-tooltip"] = new CSSObject()
                            {
                                Color = token.ColorTextDescription,
                                Cursor = "help",
                                WritingMode = "horizontal-tb",
                                MarginInlineStart = token.MarginXXS,
                            },
                            ["&::after"] = new CSSObject()
                            {
                                Content = "\":\"",
                                Position = "relative",
                                MarginBlock = 0,
                                MarginInlineStart = labelColonMarginInlineStart,
                                MarginInlineEnd = labelColonMarginInlineEnd,
                            },
                            [$"&{formItemCls}-no-colon::after"] = new CSSObject()
                            {
                                Content = "\"\\\\a0\"",
                            },
                        },
                    },
                    [$"{formItemCls}-control"] = new CSSObject()
                    {
                        ["--ant-display"] = "flex",
                        FlexDirection = "column",
                        FlexGrow = 1,
                        [$"&:first-child:not([class^=\"'{rootPrefixCls}-col-'\"]):not([class*=\"'{rootPrefixCls}-col-'\"])"] = new CSSObject()
                        {
                            Width = "100%",
                        },
                        ["&-input"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "flex",
                            AlignItems = "center",
                            MinHeight = token.ControlHeight,
                            ["&-content"] = new CSSObject()
                            {
                                Flex = "auto",
                                MaxWidth = "100%",
                            },
                        },
                    },
                    [formItemCls] = new CSSObject()
                    {
                        ["&-explain, &-extra"] = new CSSObject()
                        {
                            Clear = "both",
                            Color = token.ColorTextDescription,
                            FontSize = token.FontSize,
                            LineHeight = token.LineHeight,
                        },
                        ["&-explain-connected"] = new CSSObject()
                        {
                            Width = "100%",
                        },
                        ["&-extra"] = new CSSObject()
                        {
                            MinHeight = token.ControlHeightSM,
                            Transition = @$"color {token.MotionDurationMid} {token.MotionEaseOut}",
                        },
                        ["&-explain"] = new CSSObject()
                        {
                            ["&-error"] = new CSSObject()
                            {
                                Color = token.ColorError,
                            },
                            ["&-warning"] = new CSSObject()
                            {
                                Color = token.ColorWarning,
                            },
                        },
                    },
                    [$"&-with-help {formItemCls}-explain"] = new CSSObject()
                    {
                        Height = "auto",
                        Opacity = 1,
                    },
                    [$"{formItemCls}-feedback-icon"] = new CSSObject()
                    {
                        FontSize = token.FontSize,
                        TextAlign = "center",
                        Visibility = "visible",
                        AnimationName = ZoomIn,
                        AnimationDuration = token.MotionDurationMid,
                        AnimationTimingFunction = token.MotionEaseOutBack,
                        PointerEvents = "none",
                        ["&-success"] = new CSSObject()
                        {
                            Color = token.ColorSuccess,
                        },
                        ["&-error"] = new CSSObject()
                        {
                            Color = token.ColorError,
                        },
                        ["&-warning"] = new CSSObject()
                        {
                            Color = token.ColorWarning,
                        },
                        ["&-validating"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                        },
                    },
                },
            };
        }

        public static CSSObject GenHorizontalStyle(FormToken token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            return new CSSObject()
            {
                [$"{componentCls}-horizontal"] = new CSSObject()
                {
                    [$"{formItemCls}-label"] = new CSSObject()
                    {
                        FlexGrow = 0,
                    },
                    [$"{formItemCls}-control"] = new CSSObject()
                    {
                        Flex = "1 1 0",
                        MinWidth = 0,
                    },
                    [$"{formItemCls}-label[class$='-24'], {formItemCls}-label[class*='-24 ']"] = new CSSObject()
                    {
                        [$"& + {formItemCls}-control"] = new CSSObject()
                        {
                            MinWidth = "unset",
                        },
                    },
                },
            };
        }

        public static CSSObject GenInlineStyle(FormToken token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            return new CSSObject()
            {
                [$"{componentCls}-inline"] = new CSSObject()
                {
                    Display = "flex",
                    FlexWrap = "wrap",
                    [formItemCls] = new CSSObject()
                    {
                        Flex = "none",
                        MarginInlineEnd = token.Margin,
                        MarginBottom = 0,
                        ["&-row"] = new CSSObject()
                        {
                            FlexWrap = "nowrap",
                        },
                        [$">{formItemCls}-label,>{formItemCls}-control"] = new CSSObject()
                        {
                            Display = "inline-block",
                            VerticalAlign = "top",
                        },
                        [$"> {formItemCls}-label"] = new CSSObject()
                        {
                            Flex = "none",
                        },
                        [$"{componentCls}-text"] = new CSSObject()
                        {
                            Display = "inline-block",
                        },
                        [$"{formItemCls}-has-feedback"] = new CSSObject()
                        {
                            Display = "inline-block",
                        },
                    },
                },
            };
        }

        public static CSSObject MakeVerticalLayoutLabel(FormToken token)
        {
            return new CSSObject()
            {
                Padding = token.VerticalLabelPadding,
                Margin = token.VerticalLabelMargin,
                WhiteSpace = "initial",
                TextAlign = "start",
                ["> label"] = new CSSObject()
                {
                    Margin = 0,
                    ["&::after"] = new CSSObject()
                    {
                        Visibility = "hidden",
                    },
                },
            };
        }

        public static CSSObject MakeVerticalLayout(FormToken token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            var rootPrefixCls = token.RootPrefixCls;
            return new CSSObject()
            {
                [$"{formItemCls} {formItemCls}-label"] = MakeVerticalLayoutLabel(token),
                [$"{componentCls}:not({componentCls}-inline)"] = new CSSObject()
                {
                    [formItemCls] = new CSSObject()
                    {
                        FlexWrap = "wrap",
                        [$"{formItemCls}-label, {formItemCls}-control"] = new CSSObject()
                        {
                            [$"&:not([class*=\" {rootPrefixCls}-col-xs\"])"] = new CSSObject()
                            {
                                Flex = "0 0 100%",
                                MaxWidth = "100%",
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenVerticalStyle(FormToken token)
        {
            var componentCls = token.ComponentCls;
            var formItemCls = token.FormItemCls;
            var rootPrefixCls = token.RootPrefixCls;
            return new CSSObject()
            {
                [$"{componentCls}-vertical"] = new CSSObject()
                {
                    [formItemCls] = new CSSObject()
                    {
                        ["&-row"] = new CSSObject()
                        {
                            FlexDirection = "column",
                        },
                        ["&-label > label"] = new CSSObject()
                        {
                            Height = "auto",
                        },
                        [$"{componentCls}-item-control"] = new CSSObject()
                        {
                            Width = "100%",
                        },
                    },
                },
                [$"{componentCls}-vertical{formItemCls}-label,.{rootPrefixCls}-col-24{formItemCls}-label,.{rootPrefixCls}-col-xl-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token),
                [$"@media (max-width: {token.ScreenXSMax}px)"] = new CSSInterpolation[]
                {
                    MakeVerticalLayout(token),
                    new CSSObject()
                    {
                        [componentCls] = new CSSObject()
                        {
                            [$".{rootPrefixCls}-col-xs-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                        },
                    },
                },
                [$"@media (max-width: {token.ScreenSMMax}px)"] = new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$".{rootPrefixCls}-col-sm-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                    },
                },
                [$"@media (max-width: {token.ScreenMDMax}px)"] = new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$".{rootPrefixCls}-col-md-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                    },
                },
                [$"@media (max-width: {token.ScreenLGMax}px)"] = new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$".{rootPrefixCls}-col-lg-24{formItemCls}-label"] = MakeVerticalLayoutLabel(token)
                    },
                },
            };
        }

        public static FormToken PrepareToken(FormToken token, string rootPrefixCls)
        {
            var formToken = MergeToken(
                token,
                new FormToken()
                {
                    FormItemCls = @$"{token.ComponentCls}-item",
                    RootPrefixCls = rootPrefixCls,
                });
            return formToken;
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Form",
                (token) =>
                {
                    var rootPrefixCls = token.RootPrefixCls;
                    var formToken = PrepareToken(token, rootPrefixCls);
                    return new CSSInterpolation[]
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
                },
                (token) =>
                {
                    return new FormToken()
                    {
                        LabelRequiredMarkColor = token.ColorError,
                        LabelColor = token.ColorTextHeading,
                        LabelFontSize = token.FontSize,
                        LabelHeight = token.ControlHeight,
                        LabelColonMarginInlineStart = token.MarginXXS / 2,
                        LabelColonMarginInlineEnd = token.MarginXS,
                        ItemMarginBottom = token.MarginLG,
                        VerticalLabelPadding = @$"0 0 {token.PaddingXS}px",
                        VerticalLabelMargin = 0,
                    };
                },
                new GenOptions()
                {
                    Order = -1000,
                });
        }

        public static CSSObject GenFormValidateMotionStyle(FormToken token)
        {
            var componentCls = token.ComponentCls;
            var helpCls = @$"{componentCls}-show-help";
            var helpItemCls = @$"{componentCls}-show-help-item";
            return new CSSObject()
            {
                [helpCls] = new CSSObject()
                {
                    Transition = @$"opacity {token.MotionDurationSlow} {token.MotionEaseInOut}",
                    ["&-appear, &-enter"] = new CSSObject()
                    {
                        Opacity = 0,
                        ["&-active"] = new CSSObject()
                        {
                            Opacity = 1,
                        },
                    },
                    ["&-leave"] = new CSSObject()
                    {
                        Opacity = 1,
                        ["&-active"] = new CSSObject()
                        {
                            Opacity = 0,
                        },
                    },
                    [helpItemCls] = new CSSObject()
                    {
                        Overflow = "hidden",
                        Transition = @$"height {token.MotionDurationSlow} {token.MotionEaseInOut},
                     opacity {token.MotionDurationSlow} {token.MotionEaseInOut},
                     transform {token.MotionDurationSlow} {token.MotionEaseInOut} !important",
                        [$"&{helpItemCls}-appear, &{helpItemCls}-enter"] = new CSSObject()
                        {
                            Transform = @$"translateY(-5px)",
                            Opacity = 0,
                            ["&-active"] = new CSSObject()
                            {
                                Transform = "translateY(0)",
                                Opacity = 1,
                            },
                        },
                        [$"&{helpItemCls}-leave-active"] = new CSSObject()
                        {
                            Transform = @$"translateY(-5px)",
                        },
                    },
                },
            };
        }

        public static CSSObject GenFallbackStyle(FormToken token)
        {
            var formItemCls = token.FormItemCls;
            return new CSSObject()
            {
                ["@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none)"] = new CSSObject()
                {
                    [$"{formItemCls}-control"] = new CSSObject()
                    {
                        Display = "flex",
                    },
                },
            };
        }

        //public UseComponentStyleResult ExportDefault()
        //{
        //    return GenSubStyleComponent(
        //        ["Form", "item-item"],
        //        (token, args) =>
        //        {
        //            var rootPrefixCls = args.RootPrefixCls;
        //            var formToken = PrepareToken(token, rootPrefixCls);
        //            return new Unknown14_1
        //            {
        //                GenFallbackStyle(formToken),
        //            };
        //        });
        //}

    }
}
