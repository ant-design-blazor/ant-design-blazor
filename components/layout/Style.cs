using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class LayoutToken
    {
        public string ColorBgHeader
        {
            get => (string)_tokens["colorBgHeader"];
            set => _tokens["colorBgHeader"] = value;
        }

        public string ColorBgBody
        {
            get => (string)_tokens["colorBgBody"];
            set => _tokens["colorBgBody"] = value;
        }

        public string ColorBgTrigger
        {
            get => (string)_tokens["colorBgTrigger"];
            set => _tokens["colorBgTrigger"] = value;
        }

        public string BodyBg
        {
            get => (string)_tokens["bodyBg"];
            set => _tokens["bodyBg"] = value;
        }

        public string HeaderBg
        {
            get => (string)_tokens["headerBg"];
            set => _tokens["headerBg"] = value;
        }

        public double HeaderHeight
        {
            get => (double)_tokens["headerHeight"];
            set => _tokens["headerHeight"] = value;
        }

        public string HeaderPadding
        {
            get => (string)_tokens["headerPadding"];
            set => _tokens["headerPadding"] = value;
        }

        public string HeaderColor
        {
            get => (string)_tokens["headerColor"];
            set => _tokens["headerColor"] = value;
        }

        public string FooterPadding
        {
            get => (string)_tokens["footerPadding"];
            set => _tokens["footerPadding"] = value;
        }

        public string FooterBg
        {
            get => (string)_tokens["footerBg"];
            set => _tokens["footerBg"] = value;
        }

        public string SiderBg
        {
            get => (string)_tokens["siderBg"];
            set => _tokens["siderBg"] = value;
        }

        public double TriggerHeight
        {
            get => (double)_tokens["triggerHeight"];
            set => _tokens["triggerHeight"] = value;
        }

        public string TriggerBg
        {
            get => (string)_tokens["triggerBg"];
            set => _tokens["triggerBg"] = value;
        }

        public string TriggerColor
        {
            get => (string)_tokens["triggerColor"];
            set => _tokens["triggerColor"] = value;
        }

        public double ZeroTriggerWidth
        {
            get => (double)_tokens["zeroTriggerWidth"];
            set => _tokens["zeroTriggerWidth"] = value;
        }

        public double ZeroTriggerHeight
        {
            get => (double)_tokens["zeroTriggerHeight"];
            set => _tokens["zeroTriggerHeight"] = value;
        }

        public string LightSiderBg
        {
            get => (string)_tokens["lightSiderBg"];
            set => _tokens["lightSiderBg"] = value;
        }

        public string LightTriggerBg
        {
            get => (string)_tokens["lightTriggerBg"];
            set => _tokens["lightTriggerBg"] = value;
        }

        public string LightTriggerColor
        {
            get => (string)_tokens["lightTriggerColor"];
            set => _tokens["lightTriggerColor"] = value;
        }

    }

    public partial class LayoutToken : TokenWithCommonCls
    {
    }

    public partial class LayoutStyle
    {
        public static CSSObject GenLayoutStyle(LayoutToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var colorText = token.ColorText;
            var triggerColor = token.TriggerColor;
            var footerBg = token.FooterBg;
            var triggerBg = token.TriggerBg;
            var headerHeight = token.HeaderHeight;
            var headerPadding = token.HeaderPadding;
            var headerColor = token.HeaderColor;
            var footerPadding = token.FooterPadding;
            var triggerHeight = token.TriggerHeight;
            var zeroTriggerHeight = token.ZeroTriggerHeight;
            var zeroTriggerWidth = token.ZeroTriggerWidth;
            var motionDurationMid = token.MotionDurationMid;
            var motionDurationSlow = token.MotionDurationSlow;
            var fontSize = token.FontSize;
            var borderRadius = token.BorderRadius;
            var bodyBg = token.BodyBg;
            var headerBg = token.HeaderBg;
            var siderBg = token.SiderBg;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Display = "flex",
                    Flex = "auto",
                    FlexDirection = "column",
                    MinHeight = 0,
                    Background = bodyBg,
                    ["&, *"] = new CSSObject()
                    {
                        BoxSizing = "border-box",
                    },
                    [$"&{componentCls}-has-sider"] = new CSSObject()
                    {
                        FlexDirection = "row",
                        [$"> {componentCls}, > {componentCls}-content"] = new CSSObject()
                        {
                            Width = 0,
                        },
                    },
                    [$"{componentCls}-header, &{componentCls}-footer"] = new CSSObject()
                    {
                        Flex = "0 0 auto",
                    },
                    [$"{componentCls}-sider"] = new CSSObject()
                    {
                        Position = "relative",
                        MinWidth = 0,
                        Background = siderBg,
                        Transition = @$"all {motionDurationMid}, background 0s",
                        ["&-children"] = new CSSObject()
                        {
                            Height = "100%",
                            MarginTop = -0.1,
                            PaddingTop = 0.1,
                            [$"{antCls}-menu{antCls}-menu-inline-collapsed"] = new CSSObject()
                            {
                                Width = "auto",
                            },
                        },
                        ["&-has-trigger"] = new CSSObject()
                        {
                            PaddingBottom = triggerHeight,
                        },
                        ["&-right"] = new CSSObject()
                        {
                            Order = 1,
                        },
                        ["&-trigger"] = new CSSObject()
                        {
                            Position = "fixed",
                            Bottom = 0,
                            ZIndex = 1,
                            Height = triggerHeight,
                            Color = triggerColor,
                            LineHeight = @$"{triggerHeight}px",
                            TextAlign = "center",
                            Background = triggerBg,
                            Cursor = "pointer",
                            Transition = @$"all {motionDurationMid}",
                        },
                        ["&-zero-width"] = new CSSObject()
                        {
                            ["> *"] = new CSSObject()
                            {
                                Overflow = "hidden",
                            },
                            ["&-trigger"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = headerHeight,
                                InsetInlineEnd = -zeroTriggerWidth,
                                ZIndex = 1,
                                Width = zeroTriggerWidth,
                                Height = zeroTriggerHeight,
                                Color = triggerColor,
                                FontSize = token.FontSizeXL,
                                Display = "flex",
                                AlignItems = "center",
                                JustifyContent = "center",
                                Background = siderBg,
                                BorderStartStartRadius = 0,
                                BorderStartEndRadius = borderRadius,
                                BorderEndEndRadius = borderRadius,
                                BorderEndStartRadius = 0,
                                Cursor = "pointer",
                                Transition = @$"background {motionDurationSlow} ease",
                                ["&::after"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Inset = 0,
                                    Background = "transparent",
                                    Transition = @$"all {motionDurationSlow}",
                                    Content = "\"\"",
                                },
                                ["&:hover::after"] = new CSSObject()
                                {
                                    Background = @$"rgba(255, 255, 255, 0.2)",
                                },
                                ["&-right"] = new CSSObject()
                                {
                                    InsetInlineStart = -zeroTriggerWidth,
                                    BorderStartStartRadius = borderRadius,
                                    BorderStartEndRadius = 0,
                                    BorderEndEndRadius = 0,
                                    BorderEndStartRadius = borderRadius,
                                },
                            },
                        },
                    },
                    ["..."] = GenLayoutLightStyle(token),
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
                [$"{componentCls}-header"] = new CSSObject()
                {
                    Height = headerHeight,
                    Padding = headerPadding,
                    Color = headerColor,
                    LineHeight = @$"{headerHeight}px",
                    Background = headerBg,
                    [$"{antCls}-menu"] = new CSSObject()
                    {
                        LineHeight = "inherit",
                    },
                },
                [$"{componentCls}-footer"] = new CSSObject()
                {
                    Padding = footerPadding,
                    Color = colorText,
                    FontSize = fontSize,
                    Background = footerBg,
                },
                [$"{componentCls}-content"] = new CSSObject()
                {
                    Flex = "auto",
                    MinHeight = 0,
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Layout",
                (token) =>
                {
                    return new CSSInterpolation[]
                    {
                        GenLayoutStyle(token),
                    };
                },
                (token) =>
                {
                    var colorBgLayout = token.ColorBgLayout;
                    var controlHeight = token.ControlHeight;
                    var controlHeightLG = token.ControlHeightLG;
                    var colorText = token.ColorText;
                    var controlHeightSM = token.ControlHeightSM;
                    var marginXXS = token.MarginXXS;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorBgContainer = token.ColorBgContainer;
                    var paddingInline = controlHeightLG * 1.25;
                    return new LayoutToken()
                    {
                        ColorBgHeader = "#001529",
                        ColorBgBody = colorBgLayout,
                        ColorBgTrigger = "#002140",
                        BodyBg = colorBgLayout,
                        HeaderBg = "#001529",
                        HeaderHeight = controlHeight * 2,
                        HeaderPadding = @$"0 {paddingInline}px",
                        HeaderColor = colorText,
                        FooterPadding = @$"{controlHeightSM}px {paddingInline}px",
                        FooterBg = colorBgLayout,
                        SiderBg = "#001529",
                        TriggerHeight = controlHeightLG + marginXXS * 2,
                        TriggerBg = "#002140",
                        TriggerColor = colorTextLightSolid,
                        ZeroTriggerWidth = controlHeightLG,
                        ZeroTriggerHeight = controlHeightLG,
                        LightSiderBg = colorBgContainer,
                        LightTriggerBg = colorBgContainer,
                        LightTriggerColor = colorText,
                    };
                },
                new GenOptions()
                {
                    DeprecatedTokens = new ()
                    {
                        ("colorBgBody", "bodyBg"),
                        ("colorBgHeader", "headerBg"),
                        ("colorBgTrigger", "triggerBg"),
                    }
                });
        }

        public static CSSObject GenLayoutLightStyle(LayoutToken token)
        {
            var componentCls = token.ComponentCls;
            var bodyBg = token.BodyBg;
            var lightSiderBg = token.LightSiderBg;
            var lightTriggerBg = token.LightTriggerBg;
            var lightTriggerColor = token.LightTriggerColor;
            return new CSSObject()
            {
                [$"{componentCls}-sider-light"] = new CSSObject()
                {
                    Background = lightSiderBg,
                    [$"{componentCls}-sider-trigger"] = new CSSObject()
                    {
                        Color = lightTriggerColor,
                        Background = lightTriggerBg,
                    },
                    [$"{componentCls}-sider-zero-width-trigger"] = new CSSObject()
                    {
                        Color = lightTriggerColor,
                        Background = lightTriggerBg,
                        Border = @$"1px solid {bodyBg}",
                        BorderInlineStart = 0,
                    },
                },
            };
        }

    }

}
