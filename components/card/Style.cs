using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class CardToken : TokenWithCommonCls
    {
        public string HeaderBg
        {
            get => (string)_tokens["headerBg"];
            set => _tokens["headerBg"] = value;
        }

        public double HeaderFontSize
        {
            get => (double)_tokens["headerFontSize"];
            set => _tokens["headerFontSize"] = value;
        }

        public double HeaderFontSizeSM
        {
            get => (double)_tokens["headerFontSizeSM"];
            set => _tokens["headerFontSizeSM"] = value;
        }

        public double HeaderHeight
        {
            get => (double)_tokens["headerHeight"];
            set => _tokens["headerHeight"] = value;
        }

        public double HeaderHeightSM
        {
            get => (double)_tokens["headerHeightSM"];
            set => _tokens["headerHeightSM"] = value;
        }

        public string ActionsBg
        {
            get => (string)_tokens["actionsBg"];
            set => _tokens["actionsBg"] = value;
        }

        public string ActionsLiMargin
        {
            get => (string)_tokens["actionsLiMargin"];
            set => _tokens["actionsLiMargin"] = value;
        }

        public double TabsMarginBottom
        {
            get => (double)_tokens["tabsMarginBottom"];
            set => _tokens["tabsMarginBottom"] = value;
        }

        public string ExtraColor
        {
            get => (string)_tokens["extraColor"];
            set => _tokens["extraColor"] = value;
        }

    }

    public partial class CardToken
    {
        public string CardShadow
        {
            get => (string)_tokens["cardShadow"];
            set => _tokens["cardShadow"] = value;
        }

        public double CardHeadPadding
        {
            get => (double)_tokens["cardHeadPadding"];
            set => _tokens["cardHeadPadding"] = value;
        }

        public double CardPaddingSM
        {
            get => (double)_tokens["cardPaddingSM"];
            set => _tokens["cardPaddingSM"] = value;
        }

        public double CardPaddingBase
        {
            get => (double)_tokens["cardPaddingBase"];
            set => _tokens["cardPaddingBase"] = value;
        }

        public double CardActionsIconSize
        {
            get => (double)_tokens["cardActionsIconSize"];
            set => _tokens["cardActionsIconSize"] = value;
        }

    }

    public partial class CardStyle
    {
        public static CSSObject GenCardHeadStyle(CardToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var headerHeight = token.HeaderHeight;
            var cardPaddingBase = token.CardPaddingBase;
            var tabsMarginBottom = token.TabsMarginBottom;
            return new CSSObject()
            {
                Display = "flex",
                JustifyContent = "center",
                FlexDirection = "column",
                MinHeight = headerHeight,
                MarginBottom = -1,
                Padding = @$"0 {cardPaddingBase}px",
                Color = token.ColorTextHeading,
                FontWeight = token.FontWeightStrong,
                FontSize = token.HeaderFontSize,
                Background = token.HeaderBg,
                BorderBottom = @$"{token.LineWidth}px {token.LineType} {token.ColorBorderSecondary}",
                BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
                ["..."] = ClearFix(),
                ["&-wrapper"] = new CSSObject()
                {
                    Width = "100%",
                    Display = "flex",
                    AlignItems = "center",
                },
                ["&-title"] = new CSSObject()
                {
                    Display = "inline-block",
                    Flex = 1,
                    ["..."] = TextEllipsis,
                    [$">{componentCls}-typography,>{componentCls}-typography-edit-content"] = new CSSObject()
                    {
                        InsetInlineStart = 0,
                        MarginTop = 0,
                        MarginBottom = 0,
                    },
                },
                [$"{antCls}-tabs-top"] = new CSSObject()
                {
                    Clear = "both",
                    MarginBottom = tabsMarginBottom,
                    Color = token.ColorText,
                    FontWeight = "normal",
                    FontSize = token.FontSize,
                    ["&-bar"] = new CSSObject()
                    {
                        BorderBottom = @$"{token.LineWidth}px {token.LineType} {token.ColorBorderSecondary}",
                    },
                },
            };
        }

        public static CSSObject GenCardGridStyle(CardToken token)
        {
            var cardPaddingBase = token.CardPaddingBase;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var cardShadow = token.CardShadow;
            var lineWidth = token.LineWidth;
            return new CSSObject()
            {
                Width = "33.33%",
                Padding = cardPaddingBase,
                Border = 0,
                BorderRadius = 0,
                BoxShadow = @$"
      {lineWidth}px 0 0 0 {colorBorderSecondary},
      0 {lineWidth}px 0 0 {colorBorderSecondary},
      {lineWidth}px {lineWidth}px 0 0 {colorBorderSecondary},
      {lineWidth}px 0 0 0 {colorBorderSecondary} inset,
      0 {lineWidth}px 0 0 {colorBorderSecondary} inset;
    ",
                Transition = @$"all {token.MotionDurationMid}",
                ["&-hoverable:hover"] = new CSSObject()
                {
                    Position = "relative",
                    ZIndex = 1,
                    BoxShadow = cardShadow,
                },
            };
        }

        public static CSSObject GenCardActionsStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var actionsLiMargin = token.ActionsLiMargin;
            var cardActionsIconSize = token.CardActionsIconSize;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var actionsBg = token.ActionsBg;
            return new CSSObject()
            {
                Margin = 0,
                Padding = 0,
                ListStyle = "none",
                Background = actionsBg,
                BorderTop = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                Display = "flex",
                BorderRadius = @$"0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px ",
                ["..."] = ClearFix(),
                ["& > li"] = new CSSObject()
                {
                    Margin = actionsLiMargin,
                    Color = token.ColorTextDescription,
                    TextAlign = "center",
                    ["> span"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "block",
                        MinWidth = token.CardActionsIconSize * 2,
                        FontSize = token.FontSize,
                        LineHeight = token.LineHeight,
                        Cursor = "pointer",
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                            Transition = @$"color {token.MotionDurationMid}",
                        },
                        [$"a:not({componentCls}-btn), > {iconCls}"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = "100%",
                            Color = token.ColorTextDescription,
                            LineHeight = @$"{token.FontSize * token.LineHeight}px",
                            Transition = @$"color {token.MotionDurationMid}",
                            ["&:hover"] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                        [$"> {iconCls}"] = new CSSObject()
                        {
                            FontSize = cardActionsIconSize,
                            LineHeight = @$"{cardActionsIconSize * token.LineHeight}px",
                        },
                    },
                    ["&:not(:last-child)"] = new CSSObject()
                    {
                        BorderInlineEnd = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                    },
                },
            };
        }

        public static CSSObject GenCardMetaStyle(CardToken token)
        {
            return new CSSObject()
            {
                Margin = @$"-{token.MarginXXS}px 0",
                Display = "flex",
                ["..."] = ClearFix(),
                ["&-avatar"] = new CSSObject()
                {
                    PaddingInlineEnd = token.Padding,
                },
                ["&-detail"] = new CSSObject()
                {
                    Overflow = "hidden",
                    Flex = 1,
                    ["> div:not(:last-child)"] = new CSSObject()
                    {
                        MarginBottom = token.MarginXS,
                    },
                },
                ["&-title"] = new CSSObject()
                {
                    Color = token.ColorTextHeading,
                    FontWeight = token.FontWeightStrong,
                    FontSize = token.FontSizeLG,
                    ["..."] = TextEllipsis,
                },
                ["&-description"] = new CSSObject()
                {
                    Color = token.ColorTextDescription,
                },
            };
        }

        public static CSSObject GenCardTypeInnerStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            var cardPaddingBase = token.CardPaddingBase;
            var colorFillAlter = token.ColorFillAlter;
            return new CSSObject()
            {
                [$"{componentCls}-head"] = new CSSObject()
                {
                    Padding = @$"0 {cardPaddingBase}px",
                    Background = colorFillAlter,
                    ["&-title"] = new CSSObject()
                    {
                        FontSize = token.FontSize,
                    },
                },
                [$"{componentCls}-body"] = new CSSObject()
                {
                    Padding = @$"{token.Padding}px {cardPaddingBase}px",
                },
            };
        }

        public static CSSObject GenCardLoadingStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                Overflow = "hidden",
                [$"{componentCls}-body"] = new CSSObject()
                {
                    UserSelect = "none",
                },
            };
        }

        public static CSSObject GenCardStyle(CardToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var cardShadow = token.CardShadow;
            var cardHeadPadding = token.CardHeadPadding;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var boxShadowTertiary = token.BoxShadowTertiary;
            var cardPaddingBase = token.CardPaddingBase;
            var extraColor = token.ExtraColor;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Background = token.ColorBgContainer,
                    BorderRadius = token.BorderRadiusLG,
                    [$"&:not({componentCls}-bordered)"] = new CSSObject()
                    {
                        BoxShadow = boxShadowTertiary,
                    },
                    [$"{componentCls}-head"] = GenCardHeadStyle(token),
                    [$"{componentCls}-extra"] = new CSSObject()
                    {
                        MarginInlineStart = "auto",
                        Color = extraColor,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                    },
                    [$"{componentCls}-body"] = new CSSObject()
                    {
                        Padding = cardPaddingBase,
                        BorderRadius = @$" 0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px",
                        ["..."] = ClearFix()
                    },
                    [$"{componentCls}-grid"] = GenCardGridStyle(token),
                    [$"{componentCls}-cover"] = new CSSObject()
                    {
                        ["> *"] = new CSSObject()
                        {
                            Display = "block",
                            Width = "100%",
                        },
                        [$"img, img + {antCls}-image-mask"] = new CSSObject()
                        {
                            BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
                        },
                    },
                    [$"{componentCls}-actions"] = GenCardActionsStyle(token),
                    [$"{componentCls}-meta"] = GenCardMetaStyle(token)
                },
                [$"{componentCls}-bordered"] = new CSSObject()
                {
                    Border = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                    [$"{componentCls}-cover"] = new CSSObject()
                    {
                        MarginTop = -1,
                        MarginInlineStart = -1,
                        MarginInlineEnd = -1,
                    },
                },
                [$"{componentCls}-hoverable"] = new CSSObject()
                {
                    Cursor = "pointer",
                    Transition = @$"box-shadow {token.MotionDurationMid}, border-color {token.MotionDurationMid}",
                    ["&:hover"] = new CSSObject()
                    {
                        BorderColor = "transparent",
                        BoxShadow = cardShadow,
                    },
                },
                [$"{componentCls}-contain-grid"] = new CSSObject()
                {
                    BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0 ",
                    [$"{componentCls}-body"] = new CSSObject()
                    {
                        Display = "flex",
                        FlexWrap = "wrap",
                    },
                    [$"&:not({componentCls}-loading) {componentCls}-body"] = new CSSObject()
                    {
                        MarginBlockStart = -token.LineWidth,
                        MarginInlineStart = -token.LineWidth,
                        Padding = 0,
                    },
                },
                [$"{componentCls}-contain-tabs"] = new CSSObject()
                {
                    [$"> {componentCls}-head"] = new CSSObject()
                    {
                        MinHeight = 0,
                        [$"{componentCls}-head-title, {componentCls}-extra"] = new CSSObject()
                        {
                            PaddingTop = cardHeadPadding,
                        },
                    },
                },
                [$"{componentCls}-type-inner"] = GenCardTypeInnerStyle(token),
                [$"{componentCls}-loading"] = GenCardLoadingStyle(token),
                [$"{componentCls}-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
            };
        }

        public static CSSObject GenCardSizeStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            var cardPaddingSM = token.CardPaddingSM;
            var headerHeightSM = token.HeaderHeightSM;
            var headerFontSizeSM = token.HeaderFontSizeSM;
            return new CSSObject()
            {
                [$"{componentCls}-small"] = new CSSObject()
                {
                    [$"> {componentCls}-head"] = new CSSObject()
                    {
                        MinHeight = headerHeightSM,
                        Padding = @$"0 {cardPaddingSM}px",
                        FontSize = headerFontSizeSM,
                        [$"> {componentCls}-head-wrapper"] = new CSSObject()
                        {
                            [$"> {componentCls}-extra"] = new CSSObject()
                            {
                                FontSize = token.FontSize,
                            },
                        },
                    },
                    [$"> {componentCls}-body"] = new CSSObject()
                    {
                        Padding = cardPaddingSM,
                    },
                },
                [$"{componentCls}-small{componentCls}-contain-tabs"] = new CSSObject()
                {
                    [$"> {componentCls}-head"] = new CSSObject()
                    {
                        [$"{componentCls}-head-title, {componentCls}-extra"] = new CSSObject()
                        {
                            PaddingTop = 0,
                            Display = "flex",
                            AlignItems = "center",
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Card",
                (token) =>
                {
                    var cardToken = MergeToken(
                        token,
                        new CardToken()
                        {
                            CardShadow = token.BoxShadowCard,
                            CardHeadPadding = token.Padding,
                            CardPaddingBase = token.PaddingLG,
                            CardActionsIconSize = token.FontSize,
                            CardPaddingSM = 12,
                        });
                    return new CSSInterpolation[]
                    {
                        GenCardStyle(cardToken),
                        GenCardSizeStyle(cardToken),
                    };
                },
                (token) =>
                {
                    return new CardToken()
                    {
                        HeaderBg = "transparent",
                        HeaderFontSize = token.FontSizeLG,
                        HeaderFontSizeSM = token.FontSize,
                        HeaderHeight = token.FontSizeLG * token.LineHeightLG + token.Padding * 2,
                        HeaderHeightSM = token.FontSize * token.LineHeight + token.PaddingXS * 2,
                        ActionsBg = token.ColorBgContainer,
                        ActionsLiMargin = @$"{token.PaddingSM}px 0",
                        TabsMarginBottom = -token.Padding - token.LineWidth,
                        ExtraColor = token.ColorText,
                    };
                });
        }

    }

}
