using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class DividerToken
    {
        public int SizePaddingEdgeHorizontal { get; set; }

    }

    public partial class DividerToken : TokenWithCommonCls
    {
        public int DividerVerticalGutterMargin { get; set; }

        public int DividerHorizontalWithTextGutterMargin { get; set; }

        public int DividerHorizontalGutterMargin { get; set; }

    }

    public partial class Divider
    {
        public CSSObject GenSharedDividerStyle(Unknown_2 token)
        {
            var componentCls = token.ComponentCls;
            var sizePaddingEdgeHorizontal = token.SizePaddingEdgeHorizontal;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
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
                        Margin = @$"0 {token.DividerVerticalGutterMargin}px",
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
                            Content = \"""\",
                        },
                    },
                    [$"&-horizontal{componentCls}-with-text-left"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Width = "5%",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Width = "95%",
                        },
                    },
                    [$"&-horizontal{componentCls}-with-text-right"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Width = "95%",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Width = "5%",
                        },
                    },
                    [$"{componentCls}-inner-text"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Padding = "0 1em",
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

        public Unknown_1 GenComponentStyleHook(Unknown_3 token)
        {
            var dividerToken = MergeToken(
                token,
                new Unknown_4()
                {
                    DividerVerticalGutterMargin = token.MarginXS,
                    DividerHorizontalWithTextGutterMargin = token.Margin,
                    DividerHorizontalGutterMargin = token.MarginLG,
                });
            return new Unknown_5 { GenSharedDividerStyle(dividerToken) };
        }

    }

}