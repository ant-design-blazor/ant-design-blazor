using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class LayoutToken
    {
        public string ColorBgHeader { get; set; }

        public string ColorBgBody { get; set; }

        public string ColorBgTrigger { get; set; }

    }

    public partial class LayoutToken : TokenWithCommonCls
    {
        public int LayoutHeaderHeight { get; set; }

        public int LayoutHeaderPaddingInline { get; set; }

        public string LayoutHeaderColor { get; set; }

        public string LayoutFooterPadding { get; set; }

        public int LayoutTriggerHeight { get; set; }

        public int LayoutZeroTriggerSize { get; set; }

    }

    public partial class Layout
    {
        public Unknown_1 GenLayoutStyle(Unknown_5 token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var colorText = token.ColorText;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var colorBgHeader = token.ColorBgHeader;
            var colorBgBody = token.ColorBgBody;
            var colorBgTrigger = token.ColorBgTrigger;
            var layoutHeaderHeight = token.LayoutHeaderHeight;
            var layoutHeaderPaddingInline = token.LayoutHeaderPaddingInline;
            var layoutHeaderColor = token.LayoutHeaderColor;
            var layoutFooterPadding = token.LayoutFooterPadding;
            var layoutTriggerHeight = token.LayoutTriggerHeight;
            var layoutZeroTriggerSize = token.LayoutZeroTriggerSize;
            var motionDurationMid = token.MotionDurationMid;
            var motionDurationSlow = token.MotionDurationSlow;
            var fontSize = token.FontSize;
            var borderRadius = token.BorderRadius;
            return new Unknown_6()
            {
                [componentCls] = new Unknown_7()
                {
                    Display = "flex",
                    Flex = "auto",
                    FlexDirection = "column",
                    MinHeight = 0,
                    Background = colorBgBody,
                    ["&, *"] = new Unknown_8()
                    {
                        BoxSizing = "border-box",
                    },
                    [$"&{componentCls}-has-sider"] = new Unknown_9()
                    {
                        FlexDirection = "row",
                        [$"> {componentCls}, > {componentCls}-content"] = new Unknown_10()
                        {
                            Width = 0,
                        },
                    },
                    [$"{componentCls}-header, &{componentCls}-footer"] = new Unknown_11()
                    {
                        Flex = "0 0 auto",
                    },
                    [$"{componentCls}-sider"] = new Unknown_12()
                    {
                        Position = "relative",
                        MinWidth = 0,
                        Background = colorBgHeader,
                        Transition = @$"all {motionDurationMid}, background 0s",
                        ["&-children"] = new Unknown_13()
                        {
                            Height = "100%",
                            MarginTop = -0.1f,
                            PaddingTop = 0.1f,
                            [$"{antCls}-menu{antCls}-menu-inline-collapsed"] = new Unknown_14()
                            {
                                Width = "auto",
                            },
                        },
                        ["&-has-trigger"] = new Unknown_15()
                        {
                            PaddingBottom = layoutTriggerHeight,
                        },
                        ["&-right"] = new Unknown_16()
                        {
                            Order = 1,
                        },
                        ["&-trigger"] = new Unknown_17()
                        {
                            Position = "fixed",
                            Bottom = 0,
                            ZIndex = 1,
                            Height = layoutTriggerHeight,
                            Color = colorTextLightSolid,
                            LineHeight = @$"{layoutTriggerHeight}px",
                            TextAlign = "center",
                            Background = colorBgTrigger,
                            Cursor = "pointer",
                            Transition = @$"all {motionDurationMid}",
                        },
                        ["&-zero-width"] = new Unknown_18()
                        {
                            ["> *"] = new Unknown_19()
                            {
                                Overflow = "hidden",
                            },
                            ["&-trigger"] = new Unknown_20()
                            {
                                Position = "absolute",
                                Top = layoutHeaderHeight,
                                InsetInlineEnd = -layoutZeroTriggerSize,
                                ZIndex = 1,
                                Width = layoutZeroTriggerSize,
                                Height = layoutZeroTriggerSize,
                                Color = colorTextLightSolid,
                                FontSize = token.FontSizeXL,
                                Display = "flex",
                                AlignItems = "center",
                                JustifyContent = "center",
                                Background = colorBgHeader,
                                BorderStartStartRadius = 0,
                                BorderStartEndRadius = borderRadius,
                                BorderEndEndRadius = borderRadius,
                                BorderEndStartRadius = 0,
                                Cursor = "pointer",
                                Transition = @$"background {motionDurationSlow} ease",
                                ["&::after"] = new Unknown_21()
                                {
                                    Position = "absolute",
                                    Inset = 0,
                                    Background = "transparent",
                                    Transition = @$"all {motionDurationSlow}",
                                    Content = "\"\"",
                                },
                                ["&:hover::after"] = new Unknown_22()
                                {
                                    Background = @$"rgba(255, 255, 255, 0.2)",
                                },
                                ["&-right"] = new Unknown_23()
                                {
                                    InsetInlineStart = -layoutZeroTriggerSize,
                                    BorderStartStartRadius = borderRadius,
                                    BorderStartEndRadius = 0,
                                    BorderEndEndRadius = 0,
                                    BorderEndStartRadius = borderRadius,
                                },
                            },
                        },
                    },
                    ["..."] = GenLayoutLightStyle(token),
                    ["&-rtl"] = new Unknown_24()
                    {
                        Direction = "rtl",
                    },
                },
                [$"{componentCls}-header"] = new Unknown_25()
                {
                    Height = layoutHeaderHeight,
                    PaddingInline = layoutHeaderPaddingInline,
                    Color = layoutHeaderColor,
                    LineHeight = @$"{layoutHeaderHeight}px",
                    Background = colorBgHeader,
                    [$"{antCls}-menu"] = new Unknown_26()
                    {
                        LineHeight = "inherit",
                    },
                },
                [$"{componentCls}-footer"] = new Unknown_27()
                {
                    Padding = layoutFooterPadding,
                    Color = colorText,
                    FontSize = fontSize,
                    Background = colorBgBody,
                },
                [$"{componentCls}-content"] = new Unknown_28()
                {
                    Flex = "auto",
                    MinHeight = 0,
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_29 token)
        {
            var colorText = token.ColorText;
            var controlHeightSM = token.ControlHeightSM;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var marginXXS = token.MarginXXS;
            var layoutHeaderPaddingInline = controlHeightLG * 1.25;
            var layoutToken = MergeToken(
                token,
                new Unknown_30()
                {
                    LayoutHeaderHeight = controlHeight * 2,
                    LayoutHeaderPaddingInline = layoutHeaderPaddingInline,
                    LayoutHeaderColor = colorText,
                    LayoutFooterPadding = @$"{controlHeightSM}px {layoutHeaderPaddingInline}px",
                    LayoutTriggerHeight = controlHeightLG + marginXXS * 2,
                    LayoutZeroTriggerSize = controlHeightLG,
                });
            return new Unknown_31 { GenLayoutStyle(layoutToken) };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_32 token)
        {
            var colorBgLayout = token.ColorBgLayout;
            return new Unknown_33()
            {
                ColorBgHeader = "#001529",
                ColorBgBody = colorBgLayout,
                ColorBgTrigger = "#002140",
            };
        }

        public Unknown_4 GenLayoutLightStyle(Unknown_34 token)
        {
            var componentCls = token.ComponentCls;
            var colorBgContainer = token.ColorBgContainer;
            var colorBgBody = token.ColorBgBody;
            var colorText = token.ColorText;
            return new Unknown_35()
            {
                [$"{componentCls}-sider-light"] = new Unknown_36()
                {
                    Background = colorBgContainer,
                    [$"{componentCls}-sider-trigger"] = new Unknown_37()
                    {
                        Color = colorText,
                        Background = colorBgContainer,
                    },
                    [$"{componentCls}-sider-zero-width-trigger"] = new Unknown_38()
                    {
                        Color = colorText,
                        Background = colorBgContainer,
                        Border = @$"1px solid {colorBgBody}",
                        BorderInlineStart = 0,
                    },
                },
            };
        }

    }

}