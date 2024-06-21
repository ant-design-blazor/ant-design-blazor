using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class BreadcrumbToken
    {
        public string ItemColor
        {
            get => (string)_tokens["itemColor"];
            set => _tokens["itemColor"] = value;
        }

        public double IconFontSize
        {
            get => (double)_tokens["iconFontSize"];
            set => _tokens["iconFontSize"] = value;
        }

        public string LinkColor
        {
            get => (string)_tokens["linkColor"];
            set => _tokens["linkColor"] = value;
        }

        public string LinkHoverColor
        {
            get => (string)_tokens["linkHoverColor"];
            set => _tokens["linkHoverColor"] = value;
        }

        public string LastItemColor
        {
            get => (string)_tokens["lastItemColor"];
            set => _tokens["lastItemColor"] = value;
        }

        public double SeparatorMargin
        {
            get => (double)_tokens["separatorMargin"];
            set => _tokens["separatorMargin"] = value;
        }

        public string SeparatorColor
        {
            get => (string)_tokens["separatorColor"];
            set => _tokens["separatorColor"] = value;
        }

    }

    public partial class BreadcrumbToken : TokenWithCommonCls
    {
    }

    public partial class Breadcrumb
    {
        public CSSObject GenBreadcrumbStyle(BreadcrumbToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Color = token.ItemColor,
                    FontSize = token.FontSize,
                    [iconCls] = new CSSObject()
                    {
                        FontSize = token.IconFontSize,
                    },
                    ["ol"] = new CSSObject()
                    {
                        Display = "flex",
                        FlexWrap = "wrap",
                        Margin = 0,
                        Padding = 0,
                        ListStyle = "none",
                    },
                    ["a"] = new CSSObject()
                    {
                        Color = token.LinkColor,
                        Transition = @$"color {token.MotionDurationMid}",
                        Padding = @$"0 {token.PaddingXXS}px",
                        BorderRadius = token.BorderRadiusSM,
                        Height = token.LineHeight * token.FontSize,
                        Display = "inline-block",
                        MarginInline = -token.MarginXXS,
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.LinkHoverColor,
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        ["..."] = GenFocusStyle(token)
                    },
                    ["li:last-child"] = new CSSObject()
                    {
                        Color = token.LastItemColor,
                    },
                    [$"{componentCls}-separator"] = new CSSObject()
                    {
                        MarginInline = token.SeparatorMargin,
                        Color = token.SeparatorColor,
                    },
                    [$"{componentCls}-link"] = new CSSObject()
                    {
                        [$">{iconCls}+span,>{iconCls}+a"] = new CSSObject()
                        {
                            MarginInlineStart = token.MarginXXS,
                        },
                    },
                    [$"{componentCls}-overlay-link"] = new CSSObject()
                    {
                        BorderRadius = token.BorderRadiusSM,
                        Height = token.LineHeight * token.FontSize,
                        Display = "inline-block",
                        Padding = @$"0 {token.PaddingXXS}px",
                        MarginInline = -token.MarginXXS,
                        [$"> {iconCls}"] = new CSSObject()
                        {
                            MarginInlineStart = token.MarginXXS,
                            FontSize = token.FontSizeIcon,
                        },
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.LinkHoverColor,
                            BackgroundColor = token.ColorBgTextHover,
                            ["a"] = new CSSObject()
                            {
                                Color = token.LinkHoverColor,
                            },
                        },
                        ["a"] = new CSSObject()
                        {
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = "transparent",
                            },
                        },
                    },
                    [$"&{token.ComponentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Breadcrumb",
                (token) =>
                {
                    var breadcrumbToken = MergeToken(
                        token,
                        new BreadcrumbToken()
                        {
                        });
                    return new CSSInterpolation[] { GenBreadcrumbStyle(breadcrumbToken) };
                },
                (token) =>
                {
                    return new BreadcrumbToken()
                    {
                        ItemColor = token.ColorTextDescription,
                        LastItemColor = token.ColorText,
                        IconFontSize = token.FontSize,
                        LinkColor = token.ColorTextDescription,
                        LinkHoverColor = token.ColorText,
                        SeparatorColor = token.ColorTextDescription,
                        SeparatorMargin = token.MarginXS,
                    };
                });
        }

    }

}