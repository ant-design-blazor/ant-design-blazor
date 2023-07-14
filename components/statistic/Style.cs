using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class StatisticToken
    {
        public int TitleFontSize { get; set; }

        public int ContentFontSize { get; set; }

    }

    public partial class StatisticToken : TokenWithCommonCls
    {
    }

    public partial class Statistic
    {
        public CSSObject GenStatisticStyle(StatisticToken token)
        {
            var componentCls = token.ComponentCls;
            var marginXXS = token.MarginXXS;
            var padding = token.Padding;
            var colorTextDescription = token.ColorTextDescription;
            var titleFontSize = token.TitleFontSize;
            var colorTextHeading = token.ColorTextHeading;
            var contentFontSize = token.ContentFontSize;
            var fontFamily = token.FontFamily;
            return new CSSObject()
            {
                [$"{componentCls}"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    [$"{componentCls}-title"] = new CSSObject()
                    {
                        MarginBottom = marginXXS,
                        Color = colorTextDescription,
                        FontSize = titleFontSize,
                    },
                    [$"{componentCls}-skeleton"] = new CSSObject()
                    {
                        PaddingTop = padding,
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Color = colorTextHeading,
                        FontSize = contentFontSize,
                        FontFamily = fontFamily,
                        [$"{componentCls}-content-value"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Direction = "ltr",
                        },
                        [$"{componentCls}-content-prefix, {componentCls}-content-suffix"] = new CSSObject()
                        {
                            Display = "inline-block",
                        },
                        [$"{componentCls}-content-prefix"] = new CSSObject()
                        {
                            MarginInlineEnd = marginXXS,
                        },
                        [$"{componentCls}-content-suffix"] = new CSSObject()
                        {
                            MarginInlineStart = marginXXS,
                        },
                    },
                },
            };
        }

        public Unknown_1 GenComponentStyleHook(Unknown_3 token)
        {
            var statisticToken = MergeToken(
                token,
                new Unknown_4()
                {
                });
            return new Unknown_5 { GenStatisticStyle(statisticToken) };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_6 token)
        {
            var fontSizeHeading3 = token.FontSizeHeading3;
            var fontSize = token.FontSize;
            return new Unknown_7()
            {
                TitleFontSize = fontSize,
                ContentFontSize = fontSizeHeading3,
            };
        }

    }

}