using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class ResultToken
    {
        public double TitleFontSize
        {
            get => (double)_tokens["titleFontSize"];
            set => _tokens["titleFontSize"] = value;
        }

        public double SubtitleFontSize
        {
            get => (double)_tokens["subtitleFontSize"];
            set => _tokens["subtitleFontSize"] = value;
        }

        public double IconFontSize
        {
            get => (double)_tokens["iconFontSize"];
            set => _tokens["iconFontSize"] = value;
        }

        public string ExtraMargin
        {
            get => (string)_tokens["extraMargin"];
            set => _tokens["extraMargin"] = value;
        }

    }

    public partial class ResultToken : TokenWithCommonCls
    {
        public double ImageWidth
        {
            get => (double)_tokens["imageWidth"];
            set => _tokens["imageWidth"] = value;
        }

        public double ImageHeight
        {
            get => (double)_tokens["imageHeight"];
            set => _tokens["imageHeight"] = value;
        }

        public string ResultInfoIconColor
        {
            get => (string)_tokens["resultInfoIconColor"];
            set => _tokens["resultInfoIconColor"] = value;
        }

        public string ResultSuccessIconColor
        {
            get => (string)_tokens["resultSuccessIconColor"];
            set => _tokens["resultSuccessIconColor"] = value;
        }

        public string ResultWarningIconColor
        {
            get => (string)_tokens["resultWarningIconColor"];
            set => _tokens["resultWarningIconColor"] = value;
        }

        public string ResultErrorIconColor
        {
            get => (string)_tokens["resultErrorIconColor"];
            set => _tokens["resultErrorIconColor"] = value;
        }

    }

    public partial class Result
    {
        public CSSObject GenBaseStyle(ResultToken token)
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

        public CSSObject GenStatusIconStyle(ResultToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            return new CSSObject()
            {
                [$"{componentCls}-success {componentCls}-icon > {iconCls}"] = new CSSObject()
                {
                    Color = token.ResultSuccessIconColor,
                },
                [$"{componentCls}-error {componentCls}-icon > {iconCls}"] = new CSSObject()
                {
                    Color = token.ResultErrorIconColor,
                },
                [$"{componentCls}-info {componentCls}-icon > {iconCls}"] = new CSSObject()
                {
                    Color = token.ResultInfoIconColor,
                },
                [$"{componentCls}-warning {componentCls}-icon > {iconCls}"] = new CSSObject()
                {
                    Color = token.ResultWarningIconColor,
                },
            };
        }

        public CSSInterpolation[] GenResultStyle(ResultToken token)
        {
            return new CSSInterpolation[]
            {
                GenBaseStyle(token),
                GenStatusIconStyle(token),
            };
        }

        public CSSInterpolation[] GetStyle(ResultToken token)
        {
            return GenResultStyle(token);
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Result",
                (token) =>
                {
                    var resultInfoIconColor = token.ColorInfo;
                    var resultErrorIconColor = token.ColorError;
                    var resultSuccessIconColor = token.ColorSuccess;
                    var resultWarningIconColor = token.ColorWarning;
                    var resultToken = MergeToken(
                        token,
                        new ResultToken()
                        {
                            ResultInfoIconColor = resultInfoIconColor,
                            ResultErrorIconColor = resultErrorIconColor,
                            ResultSuccessIconColor = resultSuccessIconColor,
                            ResultWarningIconColor = resultWarningIconColor,
                            ImageWidth = 250,
                            ImageHeight = 295,
                        });
                    return new CSSInterpolation[]
                    {
                        GetStyle(resultToken),
                    };
                },
                (token) =>
                {
                    return new ResultToken()
                    {
                        TitleFontSize = token.FontSizeHeading3,
                        SubtitleFontSize = token.FontSize,
                        IconFontSize = token.FontSizeHeading3 * 3,
                        ExtraMargin = @$"{token.PaddingLG}px 0 0 0",
                    };
                });
        }

    }

}