using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class EmptyToken
    {
    }

    public partial class EmptyToken : TokenWithCommonCls
    {
        public string EmptyImgCls
        {
            get => (string)_tokens["emptyImgCls"];
            set => _tokens["emptyImgCls"] = value;
        }

        public double EmptyImgHeight
        {
            get => (double)_tokens["emptyImgHeight"];
            set => _tokens["emptyImgHeight"] = value;
        }

        public double EmptyImgHeightSM
        {
            get => (double)_tokens["emptyImgHeightSM"];
            set => _tokens["emptyImgHeightSM"] = value;
        }

        public double EmptyImgHeightMD
        {
            get => (double)_tokens["emptyImgHeightMD"];
            set => _tokens["emptyImgHeightMD"] = value;
        }

    }

    public partial class EmptyStyle
    {
        public static CSSObject GenSharedEmptyStyle(EmptyToken token)
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

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Empty",
                (token) =>
                {
                    var componentCls = token.ComponentCls;
                    var controlHeightLG = token.ControlHeightLG;
                    var emptyToken = MergeToken(
                        token,
                        new EmptyToken()
                        {
                            EmptyImgCls = @$"{componentCls}-img",
                            EmptyImgHeight = controlHeightLG * 2.5,
                            EmptyImgHeightMD = controlHeightLG,
                            EmptyImgHeightSM = controlHeightLG * 0.875,
                        });
                    return new CSSInterpolation[]
                    {
                        GenSharedEmptyStyle(emptyToken),
                    };
                });
        }

    }

}
