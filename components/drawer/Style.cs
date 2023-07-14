using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class DrawerToken
    {
        public int ZIndexPopup { get; set; }

        public int FooterPaddingBlock { get; set; }

        public int FooterPaddingInline { get; set; }

    }

    public partial class DrawerToken : TokenWithCommonCls
    {
    }

    public partial class Drawer
    {
        public Unknown_1 GenDrawerStyle(DrawerToken token)
        {
            var componentCls = token.ComponentCls;
            var zIndexPopup = token.ZIndexPopup;
            var colorBgMask = token.ColorBgMask;
            var colorBgElevated = token.ColorBgElevated;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var padding = token.Padding;
            var paddingLG = token.PaddingLG;
            var fontSizeLG = token.FontSizeLG;
            var lineHeightLG = token.LineHeightLG;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorSplit = token.ColorSplit;
            var marginSM = token.MarginSM;
            var colorIcon = token.ColorIcon;
            var colorIconHover = token.ColorIconHover;
            var colorText = token.ColorText;
            var fontWeightStrong = token.FontWeightStrong;
            var footerPaddingBlock = token.FooterPaddingBlock;
            var footerPaddingInline = token.FooterPaddingInline;
            var wrapperCls = @$"{componentCls}-content-wrapper";
            return new Unknown_4()
            {
                [componentCls] = new Unknown_5()
                {
                    Position = "fixed",
                    Inset = 0,
                    ZIndex = zIndexPopup,
                    PointerEvents = "none",
                    ["&-pure"] = new Unknown_6()
                    {
                        Position = "relative",
                        Background = colorBgElevated,
                        [$"&{componentCls}-left"] = new Unknown_7()
                        {
                            BoxShadow = token.BoxShadowDrawerLeft,
                        },
                        [$"&{componentCls}-right"] = new Unknown_8()
                        {
                            BoxShadow = token.BoxShadowDrawerRight,
                        },
                        [$"&{componentCls}-top"] = new Unknown_9()
                        {
                            BoxShadow = token.BoxShadowDrawerUp,
                        },
                        [$"&{componentCls}-bottom"] = new Unknown_10()
                        {
                            BoxShadow = token.BoxShadowDrawerDown,
                        },
                    },
                    ["&-inline"] = new Unknown_11()
                    {
                        Position = "absolute",
                    },
                    [$"{componentCls}-mask"] = new Unknown_12()
                    {
                        Position = "absolute",
                        Inset = 0,
                        ZIndex = zIndexPopup,
                        Background = colorBgMask,
                        PointerEvents = "auto",
                    },
                    [wrapperCls] = new Unknown_13()
                    {
                        Position = "absolute",
                        ZIndex = zIndexPopup,
                        Transition = @$"all {motionDurationSlow}",
                        ["&-hidden"] = new Unknown_14()
                        {
                            Display = "none",
                        },
                    },
                    [$"&-left > {wrapperCls}"] = new Unknown_15()
                    {
                        Top = 0,
                        Bottom = 0,
                        Left = new Unknown_16()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        BoxShadow = token.BoxShadowDrawerLeft,
                    },
                    [$"&-right > {wrapperCls}"] = new Unknown_17()
                    {
                        Top = 0,
                        Right = new Unknown_18()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        Bottom = 0,
                        BoxShadow = token.BoxShadowDrawerRight,
                    },
                    [$"&-top > {wrapperCls}"] = new Unknown_19()
                    {
                        Top = 0,
                        InsetInline = 0,
                        BoxShadow = token.BoxShadowDrawerUp,
                    },
                    [$"&-bottom > {wrapperCls}"] = new Unknown_20()
                    {
                        Bottom = 0,
                        InsetInline = 0,
                        BoxShadow = token.BoxShadowDrawerDown,
                    },
                    [$"{componentCls}-content"] = new Unknown_21()
                    {
                        Width = "100%",
                        Height = "100%",
                        Overflow = "auto",
                        Background = colorBgElevated,
                        PointerEvents = "auto",
                    },
                    [$"{componentCls}-wrapper-body"] = new Unknown_22()
                    {
                        Display = "flex",
                        FlexDirection = "column",
                        Width = "100%",
                        Height = "100%",
                    },
                    [$"{componentCls}-header"] = new Unknown_23()
                    {
                        Display = "flex",
                        Flex = 0,
                        AlignItems = "center",
                        Padding = @$"{padding}px {paddingLG}px",
                        FontSize = fontSizeLG,
                        LineHeight = lineHeightLG,
                        BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                        ["&-title"] = new Unknown_24()
                        {
                            Display = "flex",
                            Flex = 1,
                            AlignItems = "center",
                            MinWidth = 0,
                            MinHeight = 0,
                        },
                    },
                    [$"{componentCls}-extra"] = new Unknown_25()
                    {
                        Flex = "none",
                    },
                    [$"{componentCls}-close"] = new Unknown_26()
                    {
                        Display = "inline-block",
                        MarginInlineEnd = marginSM,
                        Color = colorIcon,
                        FontWeight = fontWeightStrong,
                        FontSize = fontSizeLG,
                        FontStyle = "normal",
                        LineHeight = 1,
                        TextAlign = "center",
                        TextTransform = "none",
                        TextDecoration = "none",
                        Background = "transparent",
                        Border = 0,
                        Outline = 0,
                        Cursor = "pointer",
                        Transition = @$"color {motionDurationMid}",
                        TextRendering = "auto",
                        ["&:focus, &:hover"] = new Unknown_27()
                        {
                            Color = colorIconHover,
                            TextDecoration = "none",
                        },
                    },
                    [$"{componentCls}-title"] = new Unknown_28()
                    {
                        Flex = 1,
                        Margin = 0,
                        Color = colorText,
                        FontWeight = token.FontWeightStrong,
                        FontSize = fontSizeLG,
                        LineHeight = lineHeightLG,
                    },
                    [$"{componentCls}-body"] = new Unknown_29()
                    {
                        Flex = 1,
                        MinWidth = 0,
                        MinHeight = 0,
                        Padding = paddingLG,
                        Overflow = "auto",
                    },
                    [$"{componentCls}-footer"] = new Unknown_30()
                    {
                        FlexShrink = 0,
                        Padding = @$"{footerPaddingBlock}px {footerPaddingInline}px",
                        BorderTop = @$"{lineWidth}px {lineType} {colorSplit}",
                    },
                    ["&-rtl"] = new Unknown_31()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_32 token)
        {
            var drawerToken = MergeToken(
                token,
                new Unknown_33()
                {
                });
            return new Unknown_34
            {
                GenDrawerStyle(drawerToken),
                GenMotionStyle(drawerToken)
            };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_35 token)
        {
            return new Unknown_36()
            {
                ZIndexPopup = token.ZIndexPopupBase,
                FooterPaddingBlock = token.PaddingXS,
                FooterPaddingInline = token.Padding,
            };
        }

    }

}