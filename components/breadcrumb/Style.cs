using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class BreadcrumbToken
    {
        public string ItemColor { get; set; }

        public int IconFontSize { get; set; }

        public string LinkColor { get; set; }

        public string LinkHoverColor { get; set; }

        public string LastItemColor { get; set; }

        public int SeparatorMargin { get; set; }

        public string SeparatorColor { get; set; }

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
                        Height = (int)token.LineHeight * token.FontSize,
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
                        Height = (int)token.LineHeight * token.FontSize,
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
            return GenComponentStyleHook("Breadcrumb", (token) =>
            {
                var breadcrumbToken = MergeToken(
                    token,
                    new BreadcrumbToken()
                    {
                        ItemColor = token.ColorTextDescription,
                        LastItemColor = token.ColorText,
                        IconFontSize = token.FontSize,
                        LinkColor = token.ColorTextDescription,
                        LinkHoverColor = token.ColorText,
                        SeparatorColor = token.ColorTextDescription,
                        SeparatorMargin = token.MarginXS,
                    });
                return new CSSInterpolation[] { GenBreadcrumbStyle(breadcrumbToken) };
            });
        }
    }
}
