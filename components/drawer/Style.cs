using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class DrawerToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public double FooterPaddingBlock
        {
            get => (double)_tokens["footerPaddingBlock"];
            set => _tokens["footerPaddingBlock"] = value;
        }

        public double FooterPaddingInline
        {
            get => (double)_tokens["footerPaddingInline"];
            set => _tokens["footerPaddingInline"] = value;
        }

    }

    public partial class DrawerToken : TokenWithCommonCls
    {
    }

    public partial class Drawer
    {
        public CSSObject GenDrawerStyle(DrawerToken token)
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
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Position = "fixed",
                    Inset = 0,
                    ZIndex = zIndexPopup,
                    PointerEvents = "none",
                    ["&-pure"] = new CSSObject()
                    {
                        Position = "relative",
                        Background = colorBgElevated,
                        [$"&{componentCls}-left"] = new CSSObject()
                        {
                            BoxShadow = token.BoxShadowDrawerLeft,
                        },
                        [$"&{componentCls}-right"] = new CSSObject()
                        {
                            BoxShadow = token.BoxShadowDrawerRight,
                        },
                        [$"&{componentCls}-top"] = new CSSObject()
                        {
                            BoxShadow = token.BoxShadowDrawerUp,
                        },
                        [$"&{componentCls}-bottom"] = new CSSObject()
                        {
                            BoxShadow = token.BoxShadowDrawerDown,
                        },
                    },
                    ["&-inline"] = new CSSObject()
                    {
                        Position = "absolute",
                    },
                    [$"{componentCls}-mask"] = new CSSObject()
                    {
                        Position = "absolute",
                        Inset = 0,
                        ZIndex = zIndexPopup,
                        Background = colorBgMask,
                        PointerEvents = "auto",
                    },
                    [wrapperCls] = new CSSObject()
                    {
                        Position = "absolute",
                        ZIndex = zIndexPopup,
                        MaxWidth = "100vw",
                        Transition = @$"all {motionDurationSlow}",
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                    [$"&-left > {wrapperCls}"] = new CSSObject()
                    {
                        Top = 0,
                        Bottom = 0,
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        BoxShadow = token.BoxShadowDrawerLeft,
                    },
                    [$"&-right > {wrapperCls}"] = new CSSObject()
                    {
                        Top = 0,
                        Right = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        Bottom = 0,
                        BoxShadow = token.BoxShadowDrawerRight,
                    },
                    [$"&-top > {wrapperCls}"] = new CSSObject()
                    {
                        Top = 0,
                        InsetInline = 0,
                        BoxShadow = token.BoxShadowDrawerUp,
                    },
                    [$"&-bottom > {wrapperCls}"] = new CSSObject()
                    {
                        Bottom = 0,
                        InsetInline = 0,
                        BoxShadow = token.BoxShadowDrawerDown,
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Width = "100%",
                        Height = "100%",
                        Overflow = "auto",
                        Background = colorBgElevated,
                        PointerEvents = "auto",
                    },
                    [$"{componentCls}-wrapper-body"] = new CSSObject()
                    {
                        Display = "flex",
                        FlexDirection = "column",
                        Width = "100%",
                        Height = "100%",
                    },
                    [$"{componentCls}-header"] = new CSSObject()
                    {
                        Display = "flex",
                        Flex = 0,
                        AlignItems = "center",
                        Padding = @$"{padding}px {paddingLG}px",
                        FontSize = fontSizeLG,
                        LineHeight = lineHeightLG,
                        BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                        ["&-title"] = new CSSObject()
                        {
                            Display = "flex",
                            Flex = 1,
                            AlignItems = "center",
                            MinWidth = 0,
                            MinHeight = 0,
                        },
                    },
                    [$"{componentCls}-extra"] = new CSSObject()
                    {
                        Flex = "none",
                    },
                    [$"{componentCls}-close"] = new CSSObject()
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
                        ["&:focus, &:hover"] = new CSSObject()
                        {
                            Color = colorIconHover,
                            TextDecoration = "none",
                        },
                    },
                    [$"{componentCls}-title"] = new CSSObject()
                    {
                        Flex = 1,
                        Margin = 0,
                        Color = colorText,
                        FontWeight = token.FontWeightStrong,
                        FontSize = fontSizeLG,
                        LineHeight = lineHeightLG,
                    },
                    [$"{componentCls}-body"] = new CSSObject()
                    {
                        Flex = 1,
                        MinWidth = 0,
                        MinHeight = 0,
                        Padding = paddingLG,
                        Overflow = "auto",
                    },
                    [$"{componentCls}-footer"] = new CSSObject()
                    {
                        FlexShrink = 0,
                        Padding = @$"{footerPaddingBlock}px {footerPaddingInline}px",
                        BorderTop = @$"{lineWidth}px {lineType} {colorSplit}",
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Drawer",
                (token) =>
                {
                    var drawerToken = MergeToken(
                        token,
                        new DrawerToken()
                        {
                        });
                    return new CSSInterpolation[]
                    {
                        GenDrawerStyle(drawerToken),
                        GenMotionStyle(drawerToken)
                    };
                },
                (token) =>
                {
                    return new DrawerToken()
                    {
                        ZIndexPopup = token.ZIndexPopupBase,
                        FooterPaddingBlock = token.PaddingXS,
                        FooterPaddingInline = token.Padding,
                    };
                });
        }

        public CSSObject GenMotionStyle(DrawerToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var sharedPanelMotion = new CSSObject()
            {
                ["&-enter, &-appear, &-leave"] = new CSSObject()
                {
                    ["&-start"] = new CSSObject()
                    {
                        Transition = "none",
                    },
                    ["&-active"] = new CSSObject()
                    {
                        Transition = @$"all {motionDurationSlow}",
                    },
                },
            };
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"{componentCls}-mask-motion"] = new CSSObject()
                    {
                        ["&-enter, &-appear, &-leave"] = new CSSObject()
                        {
                            ["&-active"] = new CSSObject()
                            {
                                Transition = @$"all {motionDurationSlow}",
                            },
                        },
                        ["&-enter, &-appear"] = new CSSObject()
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
                    },
                    [$"{componentCls}-panel-motion"] = new CSSObject()
                    {
                        ["&-left"] = new CSSInterpolation[]
                        {
                            sharedPanelMotion,
                            new CSSObject()
                            {
                                ["&-enter, &-appear"] = new CSSObject()
                                {
                                    ["&-start"] = new CSSObject()
                                    {
                                        Transform = "translateX(-100%) !important",
                                    },
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateX(0)",
                                    },
                                },
                                ["&-leave"] = new CSSObject()
                                {
                                    Transform = "translateX(0)",
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateX(-100%)",
                                    },
                                },
                            },
                        },
                        ["&-right"] = new CSSInterpolation[]
                        {
                            sharedPanelMotion,
                            new CSSObject()
                            {
                                ["&-enter, &-appear"] = new CSSObject()
                                {
                                    ["&-start"] = new CSSObject()
                                    {
                                        Transform = "translateX(100%) !important",
                                    },
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateX(0)",
                                    },
                                },
                                ["&-leave"] = new CSSObject()
                                {
                                    Transform = "translateX(0)",
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateX(100%)",
                                    },
                                },
                            },
                        },
                        ["&-top"] = new CSSInterpolation[]
                        {
                            sharedPanelMotion,
                            new CSSObject()
                            {
                                ["&-enter, &-appear"] = new CSSObject()
                                {
                                    ["&-start"] = new CSSObject()
                                    {
                                        Transform = "translateY(-100%) !important",
                                    },
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateY(0)",
                                    },
                                },
                                ["&-leave"] = new CSSObject()
                                {
                                    Transform = "translateY(0)",
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateY(-100%)",
                                    },
                                },
                            },
                        },
                        ["&-bottom"] = new CSSInterpolation[]
                        {
                            sharedPanelMotion,
                            new CSSObject()
                            {
                                ["&-enter, &-appear"] = new CSSObject()
                                {
                                    ["&-start"] = new CSSObject()
                                    {
                                        Transform = "translateY(100%) !important",
                                    },
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateY(0)",
                                    },
                                },
                                ["&-leave"] = new CSSObject()
                                {
                                    Transform = "translateY(0)",
                                    ["&-active"] = new CSSObject()
                                    {
                                        Transform = "translateY(100%)",
                                    },
                                },
                            },
                        }
                    },
                },
            };
        }

    }

}