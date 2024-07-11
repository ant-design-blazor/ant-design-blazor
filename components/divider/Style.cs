using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class DividerToken
    {
        public string TextPaddingInline
        {
            get => (string)_tokens["textPaddingInline"];
            set => _tokens["textPaddingInline"] = value;
        }

        public double OrientationMargin
        {
            get => (double)_tokens["orientationMargin"];
            set => _tokens["orientationMargin"] = value;
        }

        public double VerticalMarginInline
        {
            get => (double)_tokens["verticalMarginInline"];
            set => _tokens["verticalMarginInline"] = value;
        }

    }

    public partial class DividerToken : TokenWithCommonCls
    {
        public double SizePaddingEdgeHorizontal
        {
            get => (double)_tokens["sizePaddingEdgeHorizontal"];
            set => _tokens["sizePaddingEdgeHorizontal"] = value;
        }

        public double DividerHorizontalWithTextGutterMargin
        {
            get => (double)_tokens["dividerHorizontalWithTextGutterMargin"];
            set => _tokens["dividerHorizontalWithTextGutterMargin"] = value;
        }

        public double DividerHorizontalGutterMargin
        {
            get => (double)_tokens["dividerHorizontalGutterMargin"];
            set => _tokens["dividerHorizontalGutterMargin"] = value;
        }

    }

    public partial class DividerStyle
    {
        public static CSSObject GenSharedDividerStyle(DividerToken token)
        {
            var componentCls = token.ComponentCls;
            var sizePaddingEdgeHorizontal = token.SizePaddingEdgeHorizontal;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var textPaddingInline = token.TextPaddingInline;
            var orientationMargin = token.OrientationMargin;
            var verticalMarginInline = token.VerticalMarginInline;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    BorderBlockStart = @$"{lineWidth}px solid {colorSplit}",
                    ["&-vertical"] = new CSSObject()
                    {
                        Position = "relative",
                        Top = "-0.06em",
                        Display = "inline-block",
                        Height = "0.9em",
                        MarginInline = verticalMarginInline,
                        MarginBlock = 0,
                        VerticalAlign = "middle",
                        BorderTop = 0,
                        BorderInlineStart = @$"{lineWidth}px solid {colorSplit}",
                    },
                    ["&-horizontal"] = new CSSObject()
                    {
                        Display = "flex",
                        Clear = "both",
                        Width = "100%",
                        MinWidth = "100%",
                        Margin = @$"{token.DividerHorizontalGutterMargin}px 0",
                    },
                    [$"&-horizontal{componentCls}-with-text"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignItems = "center",
                        Margin = @$"{token.DividerHorizontalWithTextGutterMargin}px 0",
                        Color = token.ColorTextHeading,
                        FontWeight = 500,
                        FontSize = token.FontSizeLG,
                        WhiteSpace = "nowrap",
                        TextAlign = "center",
                        BorderBlockStart = @$"0 {colorSplit}",
                        ["&::before, &::after"] = new CSSObject()
                        {
                            Position = "relative",
                            Width = "50%",
                            BorderBlockStart = @$"{lineWidth}px solid transparent",
                            BorderBlockStartColor = "inherit",
                            BorderBlockEnd = 0,
                            Transform = "translateY(50%)",
                            Content = "\"\"",
                        },
                    },
                    [$"&-horizontal{componentCls}-with-text-left"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Width = @$"{orientationMargin * 100}%",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Width = @$"{100 - orientationMargin * 100}%",
                        },
                    },
                    [$"&-horizontal{componentCls}-with-text-right"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Width = @$"{100 - orientationMargin * 100}%",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Width = @$"{orientationMargin * 100}%",
                        },
                    },
                    [$"{componentCls}-inner-text"] = new CSSObject()
                    {
                        Display = "inline-block",
                        PaddingBlock = 0,
                        PaddingInline = textPaddingInline,
                    },
                    ["&-dashed"] = new CSSObject()
                    {
                        Background = "none",
                        BorderColor = colorSplit,
                        BorderStyle = "dashed",
                        BorderWidth = @$"{lineWidth}px 0 0",
                    },
                    [$"&-horizontal{componentCls}-with-text{componentCls}-dashed"] = new CSSObject()
                    {
                        ["&::before, &::after"] = new CSSObject()
                        {
                            BorderStyle = "dashed none none",
                        },
                    },
                    [$"&-vertical{componentCls}-dashed"] = new CSSObject()
                    {
                        BorderInlineStartWidth = lineWidth,
                        BorderInlineEnd = 0,
                        BorderBlockStart = 0,
                        BorderBlockEnd = 0,
                    },
                    [$"&-plain{componentCls}-with-text"] = new CSSObject()
                    {
                        Color = token.ColorText,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                    },
                    [$"&-horizontal{componentCls}-with-text-left{componentCls}-no-default-orientation-margin-left"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Width = 0,
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Width = "100%",
                        },
                        [$"{componentCls}-inner-text"] = new CSSObject()
                        {
                            PaddingInlineStart = sizePaddingEdgeHorizontal,
                        },
                    },
                    [$"&-horizontal{componentCls}-with-text-right{componentCls}-no-default-orientation-margin-right"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Width = "100%",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Width = 0,
                        },
                        [$"{componentCls}-inner-text"] = new CSSObject()
                        {
                            PaddingInlineEnd = sizePaddingEdgeHorizontal,
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Divider",
                (token) =>
                {
                    var dividerToken = MergeToken(
                        token,
                        new DividerToken()
                        {
                            DividerHorizontalWithTextGutterMargin = token.Margin,
                            DividerHorizontalGutterMargin = token.MarginLG,
                            SizePaddingEdgeHorizontal = 0,
                        });
                    return new CSSInterpolation[] { GenSharedDividerStyle(dividerToken) };
                },
                (token) =>
                {
                    return new DividerToken()
                    {
                        TextPaddingInline = "1em",
                        OrientationMargin = 0.05f,
                        VerticalMarginInline = token.MarginXS,
                    };
                });
        }

    }

}
