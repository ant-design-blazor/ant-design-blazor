using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class ResultToken
    {
        public int TitleFontSize { get; set; }

        public int SubtitleFontSize { get; set; }

        public int IconFontSize { get; set; }

        public CSSProperties ExtraMargin { get; set; }

    }

    public partial class ResultToken : TokenWithCommonCls
    {
        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public string ResultInfoIconColor { get; set; }

        public string ResultSuccessIconColor { get; set; }

        public string ResultWarningIconColor { get; set; }

        public string ResultErrorIconColor { get; set; }

    }

    public partial class Result
    {
        public CSSObject GenBaseStyle(Unknown_6 token)
        {
            var componentCls = token.ComponentCls;
            var lineHeightHeading3 = token.LineHeightHeading3;
            var iconCls = token.IconCls;
            var padding = token.Padding;
            var paddingXL = token.PaddingXL;
            var paddingXS = token.PaddingXS;
            var paddingLG = token.PaddingLG;
            var marginXS = token.MarginXS;
            var lineHeight = token.LineHeight;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Padding = @$"{paddingLG * 2}px {paddingXL}px",
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
                [$"{componentCls} {componentCls}-image"] = new CSSObject()
                {
                    Width = token.ImageWidth,
                    Height = token.ImageHeight,
                    Margin = "auto",
                },
                [$"{componentCls} {componentCls}-icon"] = new CSSObject()
                {
                    MarginBottom = paddingLG,
                    TextAlign = "center",
                    [$"& > {iconCls}"] = new CSSObject()
                    {
                        FontSize = token.IconFontSize,
                    },
                },
                [$"{componentCls} {componentCls}-title"] = new CSSObject()
                {
                    Color = token.ColorTextHeading,
                    FontSize = token.TitleFontSize,
                    LineHeight = lineHeightHeading3,
                    MarginBlock = marginXS,
                    TextAlign = "center",
                },
                [$"{componentCls} {componentCls}-subtitle"] = new CSSObject()
                {
                    Color = token.ColorTextDescription,
                    FontSize = token.SubtitleFontSize,
                    LineHeight = lineHeight,
                    TextAlign = "center",
                },
                [$"{componentCls} {componentCls}-content"] = new CSSObject()
                {
                    MarginTop = paddingLG,
                    Padding = @$"{paddingLG}px {padding * 2.5}px",
                    BackgroundColor = token.ColorFillAlter,
                },
                [$"{componentCls} {componentCls}-extra"] = new CSSObject()
                {
                    Margin = token.ExtraMargin,
                    TextAlign = "center",
                    ["& > *"] = new CSSObject()
                    {
                        MarginInlineEnd = paddingXS,
                        ["&:last-child"] = new CSSObject()
                        {
                            MarginInlineEnd = 0,
                        },
                    },
                },
            };
        }

        public Unknown_1 GenStatusIconStyle(Unknown_7 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            return new Unknown_8()
            {
                [$"{componentCls}-success {componentCls}-icon > {iconCls}"] = new Unknown_9()
                {
                    Color = token.ResultSuccessIconColor,
                },
                [$"{componentCls}-error {componentCls}-icon > {iconCls}"] = new Unknown_10()
                {
                    Color = token.ResultErrorIconColor,
                },
                [$"{componentCls}-info {componentCls}-icon > {iconCls}"] = new Unknown_11()
                {
                    Color = token.ResultInfoIconColor,
                },
                [$"{componentCls}-warning {componentCls}-icon > {iconCls}"] = new Unknown_12()
                {
                    Color = token.ResultWarningIconColor,
                },
            };
        }

        public Unknown_2 GenResultStyle(Unknown_13 token)
        {
            return new Unknown_14
            {
                GenBaseStyle(token),
                GenStatusIconStyle(token)
            };
        }

        public Unknown_3 GetStyle(Unknown_15 token)
        {
            return GenResultStyle(token);
        }

        public Unknown_4 GenComponentStyleHook(Unknown_16 token)
        {
            var resultInfoIconColor = token.ColorInfo;
            var resultErrorIconColor = token.ColorError;
            var resultSuccessIconColor = token.ColorSuccess;
            var resultWarningIconColor = token.ColorWarning;
            var resultToken = MergeToken(
                token,
                new Unknown_17()
                {
                    ResultInfoIconColor = resultInfoIconColor,
                    ResultErrorIconColor = resultErrorIconColor,
                    ResultSuccessIconColor = resultSuccessIconColor,
                    ResultWarningIconColor = resultWarningIconColor,
                    ImageWidth = 250,
                    ImageHeight = 295,
                });
            return new Unknown_18 { GetStyle(resultToken) };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_19 token)
        {
            return new Unknown_20()
            {
                TitleFontSize = token.FontSizeHeading3,
                SubtitleFontSize = token.FontSize,
                IconFontSize = token.FontSizeHeading3 * 3,
                ExtraMargin = @$"{token.PaddingLG}px 0 0 0",
            };
        }

    }

}