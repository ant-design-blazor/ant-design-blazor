using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class CardToken : TokenWithCommonCls
    {
    }

    public partial class CardToken
    {
        public int CardHeadHeight { get; set; }

        public int CardHeadHeightSM { get; set; }

        public string CardShadow { get; set; }

        public int CardHeadPadding { get; set; }

        public int CardPaddingSM { get; set; }

        public int CardPaddingBase { get; set; }

        public int CardHeadTabsMarginBottom { get; set; }

        public string CardActionsLiMargin { get; set; }

        public int CardActionsIconSize { get; set; }

    }

    public partial class Card
    {
        public CSSObject GenCardHeadStyle(CardToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var cardHeadHeight = token.CardHeadHeight;
            var cardPaddingBase = token.CardPaddingBase;
            var cardHeadTabsMarginBottom = token.CardHeadTabsMarginBottom;
            return new CSSObject()
            {
                Display = "flex",
                JustifyContent = "center",
                FlexDirection = "column",
                MinHeight = cardHeadHeight,
                MarginBottom = -1,
                Padding = @$"0 {cardPaddingBase}px",
                Color = token.ColorTextHeading,
                FontWeight = token.FontWeightStrong,
                FontSize = token.FontSizeLG,
                Background = "transparent",
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
                    MarginBottom = cardHeadTabsMarginBottom,
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

        public CSSObject GenCardGridStyle(CardToken token)
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

        public CSSObject GenCardActionsStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var cardActionsLiMargin = token.CardActionsLiMargin;
            var cardActionsIconSize = token.CardActionsIconSize;
            var colorBorderSecondary = token.ColorBorderSecondary;
            return new CSSObject()
            {
                Margin = 0,
                Padding = 0,
                ListStyle = "none",
                Background = token.ColorBgContainer,
                BorderTop = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                Display = "flex",
                BorderRadius = @$"0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px ",
                ["..."] = ClearFix(),
                ["& > li"] = new CSSObject()
                {
                    Margin = cardActionsLiMargin,
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

        public CSSObject GenCardMetaStyle(CardToken token)
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

        public CSSObject GenCardTypeInnerStyle(CardToken token)
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

        public CSSObject GenCardLoadingStyle(CardToken token)
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

        public CSSObject GenCardStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            var cardShadow = token.CardShadow;
            var cardHeadPadding = token.CardHeadPadding;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var boxShadowTertiary = token.BoxShadowTertiary;
            var cardPaddingBase = token.CardPaddingBase;
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
                        Color = "",
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
                        ["img"] = new CSSObject()
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

        public CSSObject GenCardSizeStyle(CardToken token)
        {
            var componentCls = token.ComponentCls;
            var cardPaddingSM = token.CardPaddingSM;
            var cardHeadHeightSM = token.CardHeadHeightSM;
            return new CSSObject()
            {
                [$"{componentCls}-small"] = new CSSObject()
                {
                    [$"> {componentCls}-head"] = new CSSObject()
                    {
                        MinHeight = cardHeadHeightSM,
                        Padding = @$"0 {cardPaddingSM}px",
                        FontSize = token.FontSize,
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
                            MinHeight = cardHeadHeightSM,
                            PaddingTop = 0,
                            Display = "flex",
                            AlignItems = "center",
                        },
                    },
                },
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var cardToken = MergeToken(
                token,
                new CardToken()
                {
                    CardShadow = token.BoxShadowCard,
                    CardHeadHeight = token.FontSizeLG * token.LineHeightLG + token.Padding * 2,
                    CardHeadHeightSM = token.FontSize * token.LineHeight + token.PaddingXS * 2,
                    CardHeadPadding = token.Padding,
                    CardPaddingBase = token.PaddingLG,
                    CardHeadTabsMarginBottom = -token.Padding - token.LineWidth,
                    CardActionsLiMargin = @$"{token.PaddingSM}px 0",
                    CardActionsIconSize = token.FontSize,
                    CardPaddingSM = 12,
                });
            return new CSSInterpolation[]
            {
                GenCardStyle(cardToken),
                GenCardSizeStyle(cardToken)
            };
        }

    }

}