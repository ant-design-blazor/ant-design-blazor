using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class BreadcrumbToken : TokenWithCommonCls
    {
        public string BreadcrumbBaseColor { get; set; }

        public int BreadcrumbFontSize { get; set; }

        public int BreadcrumbIconFontSize { get; set; }

        public string BreadcrumbLinkColor { get; set; }

        public string BreadcrumbLinkColorHover { get; set; }

        public string BreadcrumbLastItemColor { get; set; }

        public int BreadcrumbSeparatorMargin { get; set; }

        public string BreadcrumbSeparatorColor { get; set; }

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
                    Color = token.BreadcrumbBaseColor,
                    FontSize = token.BreadcrumbFontSize,
                    [iconCls] = new CSSObject()
                    {
                        FontSize = token.BreadcrumbIconFontSize,
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
                        Color = token.BreadcrumbLinkColor,
                        Transition = @$"color {token.MotionDurationMid}",
                        Padding = @$"0 {token.PaddingXXS}px",
                        BorderRadius = token.BorderRadiusSM,
                        Height = token.LineHeight * token.FontSize,
                        Display = "inline-block",
                        MarginInline = -token.MarginXXS,
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.BreadcrumbLinkColorHover,
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        ["..."] = GenFocusStyle(token)
                    },
                    ["li:last-child"] = new CSSObject()
                    {
                        Color = token.BreadcrumbLastItemColor,
                    },
                    [$"{componentCls}-separator"] = new CSSObject()
                    {
                        MarginInline = token.BreadcrumbSeparatorMargin,
                        Color = token.BreadcrumbSeparatorColor,
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
                            Color = token.BreadcrumbLinkColorHover,
                            BackgroundColor = token.ColorBgTextHover,
                            ["a"] = new CSSObject()
                            {
                                Color = token.BreadcrumbLinkColorHover,
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

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var BreadcrumbToken = MergeToken(
                token,
                new BreadcrumbToken()
                {
                    BreadcrumbBaseColor = token.ColorTextDescription,
                    BreadcrumbFontSize = token.FontSize,
                    BreadcrumbIconFontSize = token.FontSize,
                    BreadcrumbLinkColor = token.ColorTextDescription,
                    BreadcrumbLinkColorHover = token.ColorText,
                    BreadcrumbLastItemColor = token.ColorText,
                    BreadcrumbSeparatorMargin = token.MarginXS,
                    BreadcrumbSeparatorColor = token.ColorTextDescription,
                });
            return new CSSInterpolation[] { GenBreadcrumbStyle(BreadcrumbToken) };
        }

    }

}