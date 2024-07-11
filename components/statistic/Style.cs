using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class StatisticToken
    {
        public double TitleFontSize
        {
            get => (double)_tokens["titleFontSize"];
            set => _tokens["titleFontSize"] = value;
        }

        public double ContentFontSize
        {
            get => (double)_tokens["contentFontSize"];
            set => _tokens["contentFontSize"] = value;
        }

    }

    public partial class StatisticToken : TokenWithCommonCls
    {
    }

    public partial class StatisticStyle
    {
        public static CSSObject GenStatisticStyle(StatisticToken token)
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

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Statistic",
                (token) =>
                {
                    var statisticToken = MergeToken(
                        token,
                        new StatisticToken()
                        {
                        });
                    return new CSSInterpolation[]
                    {
                        GenStatisticStyle(statisticToken),
                    };
                },
                (token) =>
                {
                    var fontSizeHeading3 = token.FontSizeHeading3;
                    var fontSize = token.FontSize;
                    return new StatisticToken()
                    {
                        TitleFontSize = fontSize,
                        ContentFontSize = fontSizeHeading3,
                    };
                });
        }

    }

}
