using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class TypographyToken
    {
        public string TitleMarginTop
        {
            get => (string)_tokens["titleMarginTop"];
            set => _tokens["titleMarginTop"] = value;
        }

        public string TitleMarginBottom
        {
            get => (string)_tokens["titleMarginBottom"];
            set => _tokens["titleMarginBottom"] = value;
        }

    }

    public partial class TypographyToken : TokenWithCommonCls
    {
    }

    public partial class TypographyBase
    {
        public CSSObject GenTypographyStyle(TypographyToken token)
        {
            var componentCls = token.ComponentCls;
            var titleMarginTop = token.TitleMarginTop;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Color = token.ColorText,
                    WordBreak = "break-word",
                    LineHeight = token.LineHeight,
                    [$"&{componentCls}-secondary"] = new CSSObject()
                    {
                        Color = token.ColorTextDescription,
                    },
                    [$"&{componentCls}-success"] = new CSSObject()
                    {
                        Color = token.ColorSuccess,
                    },
                    [$"&{componentCls}-warning"] = new CSSObject()
                    {
                        Color = token.ColorWarning,
                    },
                    [$"&{componentCls}-danger"] = new CSSObject()
                    {
                        Color = token.ColorError,
                        ["a&:active, a&:focus"] = new CSSObject()
                        {
                            Color = token.ColorErrorActive,
                        },
                        ["a&:hover"] = new CSSObject()
                        {
                            Color = token.ColorErrorHover,
                        },
                    },
                    [$"&{componentCls}-disabled"] = new CSSObject()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                        UserSelect = "none",
                    },
                    ["div&,p"] = new CSSObject()
                    {
                        MarginBottom = "1em",
                    },
                    ["..."] = GetTitleStyles(token),
                    [$"&+h1{componentCls},&+h2{componentCls},&+h3{componentCls},&+h4{componentCls},&+h5{componentCls}"] = new CSSObject()
                    {
                        MarginTop = titleMarginTop,
                    },
                    ["div,ul,li,p,h1,h2,h3,h4,h5"] = new CSSObject()
                    {
                        ["+h1,+h2,+h3,+h4,+h5"] = new CSSObject()
                        {
                            MarginTop = titleMarginTop,
                        },
                    },
                    ["..."] = GetResetStyles(token),
                    ["..."] = GetLinkStyles(token),
                    [$"{componentCls}-expand,{componentCls}-edit,{componentCls}-copy"] = new CSSObject()
                    {
                        ["..."] = OperationUnit(token),
                        MarginInlineStart = token.MarginXXS,
                    },
                    ["..."] = GetEditableStyles(token),
                    ["..."] = GetCopyableStyles(token),
                    ["..."] = GetEllipsisStyles(),
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Typography",
                (token) =>
                {
                    return new CSSInterpolation[]
                    {
                        GenTypographyStyle(token),
                    };
                },
                (token) =>
                {
                    return new TypographyToken()
                    {
                        TitleMarginTop = "1.2em",
                        TitleMarginBottom = "0.5em",
                    };
                });
        }

        public CSSObject GetTitleStyle(double fontSize, double lineHeight, string color, TypographyToken token)
        {
            var titleMarginBottom = token.TitleMarginBottom;
            var fontWeightStrong = token.FontWeightStrong;
            return new CSSObject()
            {
                MarginBottom = titleMarginBottom,
                Color = color,
                FontWeight = fontWeightStrong,
                FontSize = fontSize,
                LineHeight = lineHeight,
            };
        }

        public CSSObject GetTitleStyles(TypographyToken token)
        {
            var headings = new [] { 1, 2, 3, 4, 5 };
            var styles = new CSSObject();
            foreach (var headingLevel in headings)
            {
                styles[$"h{headingLevel}&,div&-h{headingLevel},div&-h{headingLevel} > textarea,h{headingLevel}"] =
                    GetTitleStyle(
                        token[@$"fontSizeHeading{headingLevel}"].To<double>(),
                        token[@$"lineHeightHeading{headingLevel}"].To<double>(),
                        token.ColorTextHeading,
                        token);
            }
            return styles;
        }

        public CSSObject GetLinkStyles(TypographyToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                ["a&, a"] = new CSSObject()
                {
                    ["..."] = OperationUnit(token),
                    TextDecoration = token.LinkDecoration,
                    ["&:active, &:hover"] = new CSSObject()
                    {
                        TextDecoration = token.LinkHoverDecoration,
                    },
                    [$"&[disabled], &{componentCls}-disabled"] = new CSSObject()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                        ["&:active, &:hover"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                        },
                        ["&:active"] = new CSSObject()
                        {
                            PointerEvents = "none",
                        },
                    },
                },
            };
        }

        public CSSObject GetResetStyles(TypographyToken token)
        {
            return new CSSObject()
            {
                ["code"] = new CSSObject()
                {
                    Margin = "0 0.2em",
                    PaddingInline = "0.4em",
                    PaddingBlock = "0.2em 0.1em",
                    FontSize = "85%",
                    FontFamily = token.FontFamilyCode,
                    Background = "rgba(150, 150, 150, 0.1)",
                    Border = "1px solid rgba(100, 100, 100, 0.2)",
                    BorderRadius = 3,
                },
                ["kbd"] = new CSSObject()
                {
                    Margin = "0 0.2em",
                    PaddingInline = "0.4em",
                    PaddingBlock = "0.15em 0.1em",
                    FontSize = "90%",
                    FontFamily = token.FontFamilyCode,
                    Background = "rgba(150, 150, 150, 0.06)",
                    Border = "1px solid rgba(100, 100, 100, 0.2)",
                    BorderBottomWidth = 2,
                    BorderRadius = 3,
                },
                ["mark"] = new CSSObject()
                {
                    Padding = 0,
                    // BackgroundColor = gold[2],
                    BackgroundColor = "",
                },
                ["u, ins"] = new CSSObject()
                {
                    TextDecoration = "underline",
                    TextDecorationSkipInk = "auto",
                },
                ["s, del"] = new CSSObject()
                {
                    TextDecoration = "line-through",
                },
                ["strong"] = new CSSObject()
                {
                    FontWeight = 600,
                },
                ["ul, ol"] = new CSSObject()
                {
                    MarginInline = 0,
                    MarginBlock = "0 1em",
                    Padding = 0,
                    ["li"] = new CSSObject()
                    {
                        MarginInline = "20px 0",
                        MarginBlock = 0,
                        PaddingInline = "4px 0",
                        PaddingBlock = 0,
                    },
                },
                ["ul"] = new CSSObject()
                {
                    ListStyleType = "circle",
                    ["ul"] = new CSSObject()
                    {
                        ListStyleType = "disc",
                    },
                },
                ["ol"] = new CSSObject()
                {
                    ListStyleType = "decimal",
                },
                ["pre, blockquote"] = new CSSObject()
                {
                    Margin = "1em 0",
                },
                ["pre"] = new CSSObject()
                {
                    Padding = "0.4em 0.6em",
                    WhiteSpace = "pre-wrap",
                    WordWrap = "break-word",
                    Background = "rgba(150, 150, 150, 0.1)",
                    Border = "1px solid rgba(100, 100, 100, 0.2)",
                    BorderRadius = 3,
                    FontFamily = token.FontFamilyCode,
                    ["code"] = new CSSObject()
                    {
                        Display = "inline",
                        Margin = 0,
                        Padding = 0,
                        FontSize = "inherit",
                        FontFamily = "inherit",
                        Background = "transparent",
                        Border = 0,
                    },
                },
                ["blockquote"] = new CSSObject()
                {
                    PaddingInline = "0.6em 0",
                    PaddingBlock = 0,
                    BorderInlineStart = "4px solid rgba(100, 100, 100, 0.2)",
                    Opacity = 0.85f,
                },
            };
        }

        public CSSObject GetEditableStyles(TypographyToken token)
        {
            var componentCls = token.ComponentCls;
            var paddingSM = token.PaddingSM;
            var inputShift = paddingSM;
            return new CSSObject()
            {
                ["&-edit-content"] = new CSSObject()
                {
                    Position = "relative",
                    ["div&"] = new CSSObject()
                    {
                        InsetInlineStart = -token.PaddingSM,
                        MarginTop = -inputShift,
                        MarginBottom = @$"calc(1em - {inputShift}px)",
                    },
                    [$"{componentCls}-edit-content-confirm"] = new CSSObject()
                    {
                        Position = "absolute",
                        InsetInlineEnd = token.MarginXS + 2,
                        InsetBlockEnd = token.MarginXS,
                        Color = token.ColorTextDescription,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                        FontStyle = "normal",
                        PointerEvents = "none",
                    },
                    ["Textarea"] = new CSSObject()
                    {
                        Margin = "0!important",
                        MozTransition = "none",
                        Height = "1em",
                    },
                },
            };
        }

        public CSSObject GetCopyableStyles(TypographyToken token)
        {
            return new CSSObject()
            {
                ["&-copy-success"] = new CSSObject()
                {
                    ["&,&:hover,&:focus"] = new CSSObject()
                    {
                        Color = token.ColorSuccess,
                    },
                },
            };
        }

        public CSSObject GetEllipsisStyles()
        {
            return new CSSObject()
            {
                ["a&-ellipsis,span&-ellipsis"] = new CSSObject()
                {
                    Display = "inline-block",
                    MaxWidth = "100%",
                },
                ["&-single-line"] = new CSSObject()
                {
                    WhiteSpace = "nowrap",
                },
                ["&-ellipsis-single-line"] = new CSSObject()
                {
                    Overflow = "hidden",
                    TextOverflow = "ellipsis",
                    ["a&, span&"] = new CSSObject()
                    {
                        VerticalAlign = "bottom",
                    },
                    ["> code"] = new CSSObject()
                    {
                        PaddingBlock = 0,
                        MaxWidth = "calc(100% - 1.2em)",
                        Display = "inline-block",
                        Overflow = "hidden",
                        TextOverflow = "ellipsis",
                        VerticalAlign = "bottom",
                        BoxSizing = "content-box",
                    },
                },
                ["&-ellipsis-multiple-line"] = new CSSObject()
                {
                    Display = "-webkit-box",
                    Overflow = "hidden",
                    WebkitLineClamp = 3,
                    ["-web-kit-box-orient"] = "vertical",
                },
            };
        }

    }

}
