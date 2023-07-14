using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class EmptyToken
    {
    }

    public partial class EmptyToken : TokenWithCommonCls
    {
        public string EmptyImgCls { get; set; }

        public int EmptyImgHeight { get; set; }

        public int EmptyImgHeightSM { get; set; }

        public int EmptyImgHeightMD { get; set; }

    }

    public partial class Empty
    {
        public CSSObject GenSharedEmptyStyle(Unknown_2 token)
        {
            var componentCls = token.ComponentCls;
            var margin = token.Margin;
            var marginXS = token.MarginXS;
            var marginXL = token.MarginXL;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    MarginInline = marginXS,
                    FontSize = fontSize,
                    LineHeight = lineHeight,
                    TextAlign = "center",
                    [$"{componentCls}-image"] = new CSSObject()
                    {
                        Height = token.EmptyImgHeight,
                        MarginBottom = marginXS,
                        Opacity = token.OpacityImage,
                        ["img"] = new CSSObject()
                        {
                            Height = "100%",
                        },
                        ["svg"] = new CSSObject()
                        {
                            MaxWidth = "100%",
                            Height = "100%",
                            Margin = "auto",
                        },
                    },
                    [$"{componentCls}-description"] = new CSSObject()
                    {
                        Color = token.ColorText,
                    },
                    [$"{componentCls}-footer"] = new CSSObject()
                    {
                        MarginTop = margin,
                    },
                    ["&-normal"] = new CSSObject()
                    {
                        MarginBlock = marginXL,
                        Color = token.ColorTextDisabled,
                        [$"{componentCls}-description"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                        },
                        [$"{componentCls}-image"] = new CSSObject()
                        {
                            Height = token.EmptyImgHeightMD,
                        },
                    },
                    ["&-small"] = new CSSObject()
                    {
                        MarginBlock = marginXS,
                        Color = token.ColorTextDisabled,
                        [$"{componentCls}-image"] = new CSSObject()
                        {
                            Height = token.EmptyImgHeightSM,
                        },
                    },
                },
            };
        }

        public Unknown_1 GenComponentStyleHook(Unknown_3 token)
        {
            var componentCls = token.ComponentCls;
            var controlHeightLG = token.ControlHeightLG;
            var emptyToken = MergeToken(
                token,
                new Unknown_4()
                {
                    EmptyImgCls = @$"{componentCls}-img",
                    EmptyImgHeight = controlHeightLG * 2.5,
                    EmptyImgHeightMD = controlHeightLG,
                    EmptyImgHeightSM = controlHeightLG * 0.875,
                });
            return new Unknown_5 { GenSharedEmptyStyle(emptyToken) };
        }

    }

}