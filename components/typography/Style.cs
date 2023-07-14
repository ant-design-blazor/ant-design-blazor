using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TypographyToken
    {
        public string SizeMarginHeadingVerticalStart { get; set; }

        public string SizeMarginHeadingVerticalEnd { get; set; }

    }

    public partial class TypographyToken : TokenWithCommonCls
    {
    }

    public partial class Typography
    {
        public Unknown_1 GenTypographyStyle(Unknown_8 token)
        {
            var componentCls = token.ComponentCls;
            var sizeMarginHeadingVerticalStart = token.SizeMarginHeadingVerticalStart;
            return new Unknown_9()
            {
                [componentCls] = new Unknown_10()
                {
                    Color = token.ColorText,
                    WordBreak = "break-word",
                    LineHeight = token.LineHeight,
                    [$"&{componentCls}-secondary"] = new Unknown_11()
                    {
                        Color = token.ColorTextDescription,
                    },
                    [$"&{componentCls}-success"] = new Unknown_12()
                    {
                        Color = token.ColorSuccess,
                    },
                    [$"&{componentCls}-warning"] = new Unknown_13()
                    {
                        Color = token.ColorWarning,
                    },
                    [$"&{componentCls}-danger"] = new Unknown_14()
                    {
                        Color = token.ColorError,
                        ["a&:active, a&:focus"] = new Unknown_15()
                        {
                            Color = token.ColorErrorActive,
                        },
                        ["a&:hover"] = new Unknown_16()
                        {
                            Color = token.ColorErrorHover,
                        },
                    },
                    [$"&{componentCls}-disabled"] = new Unknown_17()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                        UserSelect = "none",
                    },
                    ["div&,p"] = new Unknown_18()
                    {
                        MarginBottom = "1em",
                    },
                    ["..."] = GetTitleStyles(token),
                    [$"&+h1{componentCls},&+h2{componentCls},&+h3{componentCls},&+h4{componentCls},&+h5{componentCls}"] = new Unknown_19()
                    {
                        MarginTop = sizeMarginHeadingVerticalStart,
                    },
                    ["div,ul,li,p,h1,h2,h3,h4,h5"] = new Unknown_20()
                    {
                        ["+h1,+h2,+h3,+h4,+h5"] = new Unknown_21()
                        {
                            MarginTop = sizeMarginHeadingVerticalStart,
                        },
                    },
                    ["..."] = GetResetStyles(token),
                    ["..."] = GetLinkStyles(token),
                    [$"{componentCls}-expand,{componentCls}-edit,{componentCls}-copy"] = new Unknown_22()
                    {
                        ["..."] = OperationUnit(token),
                        MarginInlineStart = token.MarginXXS,
                    },
                    ["..."] = GetEditableStyles(token),
                    ["..."] = GetCopyableStyles(token),
                    ["..."] = GetEllipsisStyles(),
                    ["&-rtl"] = new Unknown_23()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_24 token)
        {
            return new Unknown_25 { GenTypographyStyle(token) };
        }

        public Unknown_3 GetTitleStyle(int fontSize, int lineHeight, string color, TypographyToken token)
        {
            var sizeMarginHeadingVerticalEnd = token.SizeMarginHeadingVerticalEnd;
            var fontWeightStrong = token.FontWeightStrong;
            return new Unknown_26()
            {
                MarginBottom = sizeMarginHeadingVerticalEnd,
                Color = color,
                FontWeight = fontWeightStrong,
                FontSize = fontSize,
                LineHeight = lineHeight,
            };
        }

        public Unknown_4 GetTitleStyles(Unknown_27 token)
        {
            var headings = [1, 2, 3, 4, 5] as const;
            var styles = {} as CSSObject;
        }

        public Unknown_5 GetLinkStyles(Unknown_28 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_29()
            {
                ["a&, a"] = new Unknown_30()
                {
                    ["..."] = OperationUnit(token),
                    TextDecoration = token.LinkDecoration,
                    ["&:active, &:hover"] = new Unknown_31()
                    {
                        TextDecoration = token.LinkHoverDecoration,
                    },
                    [$"&[disabled], &{componentCls}-disabled"] = new Unknown_32()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                        ["&:active, &:hover"] = new Unknown_33()
                        {
                            Color = token.ColorTextDisabled,
                        },
                        ["&:active"] = new Unknown_34()
                        {
                            PointerEvents = "none",
                        },
                    },
                },
            };
        }

        public CSSObject GetResetStyles(Unknown_35 token)
        {
            return new CSSObject()
            {
                Code = new CSSObject()
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
                Kbd = new CSSObject()
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
                Mark = new CSSObject()
                {
                    Padding = 0,
                    BackgroundColor = gold[2],
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
                Strong = new CSSObject()
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
                Pre = new CSSObject()
                {
                    Padding = "0.4em 0.6em",
                    WhiteSpace = "pre-wrap",
                    WordWrap = "break-word",
                    Background = "rgba(150, 150, 150, 0.1)",
                    Border = "1px solid rgba(100, 100, 100, 0.2)",
                    BorderRadius = 3,
                    FontFamily = token.FontFamilyCode,
                    Code = new CSSObject()
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
                Blockquote = new CSSObject()
                {
                    PaddingInline = "0.6em 0",
                    PaddingBlock = 0,
                    BorderInlineStart = "4px solid rgba(100, 100, 100, 0.2)",
                    Opacity = 0.85f,
                },
            };
        }

        public Unknown_6 GetEditableStyles(Unknown_36 token)
        {
            var componentCls = token.ComponentCls;
            var inputToken = InitInputToken(token);
            var inputShift = inputToken.InputPaddingVertical + 1;
            return new Unknown_37()
            {
                ["&-edit-content"] = new Unknown_38()
                {
                    Position = "relative",
                    ["div&"] = new Unknown_39()
                    {
                        InsetInlineStart = -token.PaddingSM,
                        MarginTop = -inputShift,
                        MarginBottom = @$"calc(1em - {inputShift}px)",
                    },
                    [$"{componentCls}-edit-content-confirm"] = new Unknown_40()
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
                    Textarea = new Unknown_41()
                    {
                        Margin = "0!important",
                        MozTransition = "none",
                        Height = "1em",
                    },
                },
            };
        }

        public Unknown_7 GetCopyableStyles(Unknown_42 token)
        {
            return new Unknown_43()
            {
                ["&-copy-success"] = new Unknown_44()
                {
                    ["&,&:hover,&:focus"] = new Unknown_45()
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
                },
                ["&-ellipsis-multiple-line"] = new CSSObject()
                {
                    Display = "-webkit-box",
                    Overflow = "hidden",
                    WebkitLineClamp = 3,
                    WebkitBoxOrient = "vertical",
                },
            };
        }

    }

}