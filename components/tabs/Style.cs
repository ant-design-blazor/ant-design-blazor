using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Slide;

namespace AntDesign
{
    public partial class TabsToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public string CardBg
        {
            get => (string)_tokens["cardBg"];
            set => _tokens["cardBg"] = value;
        }

        public double CardHeight
        {
            get => (double)_tokens["cardHeight"];
            set => _tokens["cardHeight"] = value;
        }

        public string CardPadding
        {
            get => (string)_tokens["cardPadding"];
            set => _tokens["cardPadding"] = value;
        }

        public string CardPaddingSM
        {
            get => (string)_tokens["cardPaddingSM"];
            set => _tokens["cardPaddingSM"] = value;
        }

        public string CardPaddingLG
        {
            get => (string)_tokens["cardPaddingLG"];
            set => _tokens["cardPaddingLG"] = value;
        }

        public double TitleFontSize
        {
            get => (double)_tokens["titleFontSize"];
            set => _tokens["titleFontSize"] = value;
        }

        public double TitleFontSizeLG
        {
            get => (double)_tokens["titleFontSizeLG"];
            set => _tokens["titleFontSizeLG"] = value;
        }

        public double TitleFontSizeSM
        {
            get => (double)_tokens["titleFontSizeSM"];
            set => _tokens["titleFontSizeSM"] = value;
        }

        public string InkBarColor
        {
            get => (string)_tokens["inkBarColor"];
            set => _tokens["inkBarColor"] = value;
        }

        public string HorizontalMargin
        {
            get => (string)_tokens["horizontalMargin"];
            set => _tokens["horizontalMargin"] = value;
        }

        public double HorizontalItemGutter
        {
            get => (double)_tokens["horizontalItemGutter"];
            set => _tokens["horizontalItemGutter"] = value;
        }

        public string HorizontalItemMargin
        {
            get => (string)_tokens["horizontalItemMargin"];
            set => _tokens["horizontalItemMargin"] = value;
        }

        public string HorizontalItemMarginRTL
        {
            get => (string)_tokens["horizontalItemMarginRTL"];
            set => _tokens["horizontalItemMarginRTL"] = value;
        }

        public string HorizontalItemPadding
        {
            get => (string)_tokens["horizontalItemPadding"];
            set => _tokens["horizontalItemPadding"] = value;
        }

        public string HorizontalItemPaddingLG
        {
            get => (string)_tokens["horizontalItemPaddingLG"];
            set => _tokens["horizontalItemPaddingLG"] = value;
        }

        public string HorizontalItemPaddingSM
        {
            get => (string)_tokens["horizontalItemPaddingSM"];
            set => _tokens["horizontalItemPaddingSM"] = value;
        }

        public string VerticalItemPadding
        {
            get => (string)_tokens["verticalItemPadding"];
            set => _tokens["verticalItemPadding"] = value;
        }

        public string VerticalItemMargin
        {
            get => (string)_tokens["verticalItemMargin"];
            set => _tokens["verticalItemMargin"] = value;
        }

        public string ItemColor
        {
            get => (string)_tokens["itemColor"];
            set => _tokens["itemColor"] = value;
        }

        public string ItemActiveColor
        {
            get => (string)_tokens["itemActiveColor"];
            set => _tokens["itemActiveColor"] = value;
        }

        public string ItemHoverColor
        {
            get => (string)_tokens["itemHoverColor"];
            set => _tokens["itemHoverColor"] = value;
        }

        public string ItemSelectedColor
        {
            get => (string)_tokens["itemSelectedColor"];
            set => _tokens["itemSelectedColor"] = value;
        }

        public double CardGutter
        {
            get => (double)_tokens["cardGutter"];
            set => _tokens["cardGutter"] = value;
        }

    }

    public partial class TabsToken : TokenWithCommonCls
    {
        public string TabsCardPadding
        {
            get => (string)_tokens["tabsCardPadding"];
            set => _tokens["tabsCardPadding"] = value;
        }

        public double DropdownEdgeChildVerticalPadding
        {
            get => (double)_tokens["dropdownEdgeChildVerticalPadding"];
            set => _tokens["dropdownEdgeChildVerticalPadding"] = value;
        }

        public double TabsNavWrapPseudoWidth
        {
            get => (double)_tokens["tabsNavWrapPseudoWidth"];
            set => _tokens["tabsNavWrapPseudoWidth"] = value;
        }

        public string TabsActiveTextShadow
        {
            get => (string)_tokens["tabsActiveTextShadow"];
            set => _tokens["tabsActiveTextShadow"] = value;
        }

        public double TabsDropdownHeight
        {
            get => (double)_tokens["tabsDropdownHeight"];
            set => _tokens["tabsDropdownHeight"] = value;
        }

        public double TabsDropdownWidth
        {
            get => (double)_tokens["tabsDropdownWidth"];
            set => _tokens["tabsDropdownWidth"] = value;
        }

        public string TabsHorizontalItemMargin
        {
            get => (string)_tokens["tabsHorizontalItemMargin"];
            set => _tokens["tabsHorizontalItemMargin"] = value;
        }

        public string TabsHorizontalItemMarginRTL
        {
            get => (string)_tokens["tabsHorizontalItemMarginRTL"];
            set => _tokens["tabsHorizontalItemMarginRTL"] = value;
        }

    }

    public partial class TabsStyle
    {
        public static CSSObject GenCardStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsCardPadding = token.TabsCardPadding;
            var cardBg = token.CardBg;
            var cardGutter = token.CardGutter;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var itemSelectedColor = token.ItemSelectedColor;
            return new CSSObject()
            {
                [$"{componentCls}-card"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        [$"{componentCls}-tab"] = new CSSObject()
                        {
                            Margin = 0,
                            Padding = tabsCardPadding,
                            Background = cardBg,
                            Border = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                            Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseInOut}",
                        },
                        [$"{componentCls}-tab-active"] = new CSSObject()
                        {
                            Color = itemSelectedColor,
                            Background = token.ColorBgContainer,
                        },
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Visibility = "hidden",
                        },
                    },
                    [$"&{componentCls}-top, &{componentCls}-bottom"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab + {componentCls}-tab"] = new CSSObject()
                            {
                                MarginLeft = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"{cardGutter}px",
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-top"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
                            },
                            [$"{componentCls}-tab-active"] = new CSSObject()
                            {
                                BorderBottomColor = token.ColorBgContainer,
                            },
                        },
                    },
                    [$"&{componentCls}-bottom"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = @$"0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px",
                            },
                            [$"{componentCls}-tab-active"] = new CSSObject()
                            {
                                BorderTopColor = token.ColorBgContainer,
                            },
                        },
                    },
                    [$"&{componentCls}-left, &{componentCls}-right"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab + {componentCls}-tab"] = new CSSObject()
                            {
                                MarginTop = @$"{cardGutter}px",
                            },
                        },
                    },
                    [$"&{componentCls}-left"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"{token.BorderRadiusLG}px 0 0 {token.BorderRadiusLG}px",
                                },
                            },
                            [$"{componentCls}-tab-active"] = new CSSObject()
                            {
                                BorderRightColor = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = token.ColorBgContainer,
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-right"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px 0",
                                },
                            },
                            [$"{componentCls}-tab-active"] = new CSSObject()
                            {
                                BorderLeftColor = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = token.ColorBgContainer,
                                },
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenDropdownStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var itemHoverColor = token.ItemHoverColor;
            var dropdownEdgeChildVerticalPadding = token.DropdownEdgeChildVerticalPadding;
            return new CSSObject()
            {
                [$"{componentCls}-dropdown"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "absolute",
                    Top = -9999,
                    Left = new PropertySkip()
                    {
                        SkipCheck = true,
                        Value = -9999,
                    },
                    ZIndex = token.ZIndexPopup,
                    Display = "block",
                    ["&-hidden"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    [$"{componentCls}-dropdown-menu"] = new CSSObject()
                    {
                        MaxHeight = token.TabsDropdownHeight,
                        Margin = 0,
                        Padding = @$"{dropdownEdgeChildVerticalPadding}px 0",
                        OverflowX = "hidden",
                        OverflowY = "auto",
                        TextAlign = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "left",
                        },
                        ListStyleType = "none",
                        BackgroundColor = token.ColorBgContainer,
                        BackgroundClip = "padding-box",
                        BorderRadius = token.BorderRadiusLG,
                        Outline = "none",
                        BoxShadow = token.BoxShadowSecondary,
                        ["&-item"] = new CSSObject()
                        {
                            ["..."] = TextEllipsis,
                            Display = "flex",
                            AlignItems = "center",
                            MinWidth = token.TabsDropdownWidth,
                            Margin = 0,
                            Padding = @$"{token.PaddingXXS}px {token.PaddingSM}px",
                            Color = token.ColorText,
                            FontWeight = "normal",
                            FontSize = token.FontSize,
                            LineHeight = token.LineHeight,
                            Cursor = "pointer",
                            Transition = @$"all {token.MotionDurationSlow}",
                            ["> span"] = new CSSObject()
                            {
                                Flex = 1,
                                WhiteSpace = "nowrap",
                            },
                            ["&-remove"] = new CSSObject()
                            {
                                Flex = "none",
                                MarginLeft = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = token.MarginSM,
                                },
                                Color = token.ColorTextDescription,
                                FontSize = token.FontSizeSM,
                                Background = "transparent",
                                Border = 0,
                                Cursor = "pointer",
                                ["&:hover"] = new CSSObject()
                                {
                                    Color = itemHoverColor,
                                },
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                Background = token.ControlItemBgHover,
                            },
                            ["&-disabled"] = new CSSObject()
                            {
                                ["&, &:hover"] = new CSSObject()
                                {
                                    Color = token.ColorTextDisabled,
                                    Background = "transparent",
                                    Cursor = "not-allowed",
                                },
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenPositionStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var margin = token.Margin;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var horizontalMargin = token.HorizontalMargin;
            var verticalItemPadding = token.VerticalItemPadding;
            var verticalItemMargin = token.VerticalItemMargin;
            return new CSSObject()
            {
                [$"{componentCls}-top, {componentCls}-bottom"] = new CSSObject()
                {
                    FlexDirection = "column",
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        Margin = horizontalMargin,
                        ["&::before"] = new CSSObject()
                        {
                            Position = "absolute",
                            Right = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = 0,
                            },
                            Left = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = 0,
                            },
                            BorderBottom = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                            Content = "\"\"",
                        },
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Height = token.LineWidthBold,
                            ["&-animated"] = new CSSObject()
                            {
                                Transition = @$"width {token.MotionDurationSlow}, left {token.MotionDurationSlow},
            right {token.MotionDurationSlow}",
                            },
                        },
                        [$"{componentCls}-nav-wrap"] = new CSSObject()
                        {
                            ["&::before, &::after"] = new CSSObject()
                            {
                                Top = 0,
                                Bottom = 0,
                                Width = token.ControlHeight,
                            },
                            ["&::before"] = new CSSObject()
                            {
                                Left = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                BoxShadow = token.BoxShadowTabsOverflowLeft,
                            },
                            ["&::after"] = new CSSObject()
                            {
                                Right = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                BoxShadow = token.BoxShadowTabsOverflowRight,
                            },
                            [$"&{componentCls}-nav-wrap-ping-left::before"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                            [$"&{componentCls}-nav-wrap-ping-right::after"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                        },
                    },
                },
                [$"{componentCls}-top"] = new CSSObject()
                {
                    [$">{componentCls}-nav,>div>{componentCls}-nav"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Bottom = 0,
                        },
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Bottom = 0,
                        },
                    },
                },
                [$"{componentCls}-bottom"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        Order = 1,
                        MarginTop = @$"{margin}px",
                        MarginBottom = 0,
                        ["&::before"] = new CSSObject()
                        {
                            Top = 0,
                        },
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Top = 0,
                        },
                    },
                    [$"> {componentCls}-content-holder, > div > {componentCls}-content-holder"] = new CSSObject()
                    {
                        Order = 0,
                    },
                },
                [$"{componentCls}-left, {componentCls}-right"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        FlexDirection = "column",
                        MinWidth = token.ControlHeight * 1.25,
                        [$"{componentCls}-tab"] = new CSSObject()
                        {
                            Padding = verticalItemPadding,
                            TextAlign = "center",
                        },
                        [$"{componentCls}-tab + {componentCls}-tab"] = new CSSObject()
                        {
                            Margin = verticalItemMargin,
                        },
                        [$"{componentCls}-nav-wrap"] = new CSSObject()
                        {
                            FlexDirection = "column",
                            ["&::before, &::after"] = new CSSObject()
                            {
                                Right = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                Left = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                Height = token.ControlHeight,
                            },
                            ["&::before"] = new CSSObject()
                            {
                                Top = 0,
                                BoxShadow = token.BoxShadowTabsOverflowTop,
                            },
                            ["&::after"] = new CSSObject()
                            {
                                Bottom = 0,
                                BoxShadow = token.BoxShadowTabsOverflowBottom,
                            },
                            [$"&{componentCls}-nav-wrap-ping-top::before"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                            [$"&{componentCls}-nav-wrap-ping-bottom::after"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                        },
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Width = token.LineWidthBold,
                            ["&-animated"] = new CSSObject()
                            {
                                Transition = @$"height {token.MotionDurationSlow}, top {token.MotionDurationSlow}",
                            },
                        },
                        [$"{componentCls}-nav-list, {componentCls}-nav-operations"] = new CSSObject()
                        {
                            Flex = "1 0 auto",
                            FlexDirection = "column",
                        },
                    },
                },
                [$"{componentCls}-left"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Right = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = 0,
                            },
                        },
                    },
                    [$"> {componentCls}-content-holder, > div > {componentCls}-content-holder"] = new CSSObject()
                    {
                        MarginLeft = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = @$"-{token.LineWidth}px",
                        },
                        BorderLeft = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        },
                        [$"> {componentCls}-content > {componentCls}-tabpane"] = new CSSObject()
                        {
                            PaddingLeft = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = token.PaddingLG,
                            },
                        },
                    },
                },
                [$"{componentCls}-right"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        Order = 1,
                        [$"{componentCls}-ink-bar"] = new CSSObject()
                        {
                            Left = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = 0,
                            },
                        },
                    },
                    [$"> {componentCls}-content-holder, > div > {componentCls}-content-holder"] = new CSSObject()
                    {
                        Order = 0,
                        MarginRight = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = -token.LineWidth,
                        },
                        BorderRight = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        },
                        [$"> {componentCls}-content > {componentCls}-tabpane"] = new CSSObject()
                        {
                            PaddingRight = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = token.PaddingLG,
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenSizeStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var cardPaddingSM = token.CardPaddingSM;
            var cardPaddingLG = token.CardPaddingLG;
            var horizontalItemPaddingSM = token.HorizontalItemPaddingSM;
            var horizontalItemPaddingLG = token.HorizontalItemPaddingLG;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-small"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                Padding = horizontalItemPaddingSM,
                                FontSize = token.TitleFontSizeSM,
                            },
                        },
                    },
                    ["&-large"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                Padding = horizontalItemPaddingLG,
                                FontSize = token.TitleFontSizeLG,
                            },
                        },
                    },
                },
                [$"{componentCls}-card"] = new CSSObject()
                {
                    [$"&{componentCls}-small"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                Padding = cardPaddingSM,
                            },
                        },
                        [$"&{componentCls}-bottom"] = new CSSObject()
                        {
                            [$"> {componentCls}-nav {componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = @$"0 0 {token.BorderRadius}px {token.BorderRadius}px",
                            },
                        },
                        [$"&{componentCls}-top"] = new CSSObject()
                        {
                            [$"> {componentCls}-nav {componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = @$"{token.BorderRadius}px {token.BorderRadius}px 0 0",
                            },
                        },
                        [$"&{componentCls}-right"] = new CSSObject()
                        {
                            [$"> {componentCls}-nav {componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"0 {token.BorderRadius}px {token.BorderRadius}px 0",
                                },
                            },
                        },
                        [$"&{componentCls}-left"] = new CSSObject()
                        {
                            [$"> {componentCls}-nav {componentCls}-tab"] = new CSSObject()
                            {
                                BorderRadius = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"{token.BorderRadius}px 0 0 {token.BorderRadius}px",
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-large"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                Padding = cardPaddingLG,
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenTabStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var itemActiveColor = token.ItemActiveColor;
            var itemHoverColor = token.ItemHoverColor;
            var iconCls = token.IconCls;
            var tabsHorizontalItemMargin = token.TabsHorizontalItemMargin;
            var horizontalItemPadding = token.HorizontalItemPadding;
            var itemSelectedColor = token.ItemSelectedColor;
            var itemColor = token.ItemColor;
            var tabCls = @$"{componentCls}-tab";
            return new CSSObject()
            {
                [tabCls] = new CSSObject()
                {
                    Position = "relative",
                    WebkitTouchCallout = "none",
                    WebkitTapHighlightColor = "transparent",
                    Display = "inline-flex",
                    AlignItems = "center",
                    Padding = horizontalItemPadding,
                    FontSize = token.TitleFontSize,
                    Background = "transparent",
                    Border = 0,
                    Outline = "none",
                    Cursor = "pointer",
                    Color = itemColor,
                    ["&-btn, &-remove"] = new CSSObject()
                    {
                        ["&:focus:not(:focus-visible), &:active"] = new CSSObject()
                        {
                            Color = itemActiveColor,
                        },
                        ["..."] = GenFocusStyle(token)
                    },
                    ["&-btn"] = new CSSObject()
                    {
                        Outline = "none",
                        Transition = "all 0.3s",
                    },
                    ["&-remove"] = new CSSObject()
                    {
                        Flex = "none",
                        MarginRight = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = -token.MarginXXS,
                        },
                        MarginLeft = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = token.MarginXS,
                        },
                        Color = token.ColorTextDescription,
                        FontSize = token.FontSizeSM,
                        Background = "transparent",
                        Border = "none",
                        Outline = "none",
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationSlow}",
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.ColorTextHeading,
                        },
                    },
                    ["&:hover"] = new CSSObject()
                    {
                        Color = itemHoverColor,
                    },
                    [$"&{tabCls}-active {tabCls}-btn"] = new CSSObject()
                    {
                        Color = itemSelectedColor,
                        TextShadow = token.TabsActiveTextShadow,
                    },
                    [$"&{tabCls}-disabled"] = new CSSObject()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                    },
                    [$"&{tabCls}-disabled {tabCls}-btn, &{tabCls}-disabled {componentCls}-remove"] = new CSSObject()
                    {
                        ["&:focus, &:active"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                        },
                    },
                    [$"& {tabCls}-remove {iconCls}"] = new CSSObject()
                    {
                        Margin = 0,
                    },
                    [iconCls] = new CSSObject()
                    {
                        MarginRight = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = token.MarginSM,
                        },
                    },
                },
                [$"{tabCls} + {tabCls}"] = new CSSObject()
                {
                    Margin = new PropertySkip()
                    {
                        SkipCheck = true,
                        Value = tabsHorizontalItemMargin,
                    },
                },
            };
        }

        public static CSSObject GenRtlStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsHorizontalItemMarginRTL = token.TabsHorizontalItemMarginRTL;
            var iconCls = token.IconCls;
            var cardGutter = token.CardGutter;
            var rtlCls = @$"{componentCls}-rtl";
            return new CSSObject()
            {
                [rtlCls] = new CSSObject()
                {
                    Direction = "rtl",
                    [$"{componentCls}-nav"] = new CSSObject()
                    {
                        [$"{componentCls}-tab"] = new CSSObject()
                        {
                            Margin = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = tabsHorizontalItemMarginRTL,
                            },
                            [$"{componentCls}-tab:last-of-type"] = new CSSObject()
                            {
                                MarginLeft = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                            },
                            [iconCls] = new CSSObject()
                            {
                                MarginRight = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                MarginLeft = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"{token.MarginSM}px",
                                },
                            },
                            [$"{componentCls}-tab-remove"] = new CSSObject()
                            {
                                MarginRight = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"{token.MarginXS}px",
                                },
                                MarginLeft = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = @$"-{token.MarginXXS}px",
                                },
                                [iconCls] = new CSSObject()
                                {
                                    Margin = 0,
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-left"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            Order = 1,
                        },
                        [$"> {componentCls}-content-holder"] = new CSSObject()
                        {
                            Order = 0,
                        },
                    },
                    [$"&{componentCls}-right"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            Order = 0,
                        },
                        [$"> {componentCls}-content-holder"] = new CSSObject()
                        {
                            Order = 1,
                        },
                    },
                    [$"&{componentCls}-card{componentCls}-top, &{componentCls}-card{componentCls}-bottom"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab + {componentCls}-tab"] = new CSSObject()
                            {
                                MarginRight = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = cardGutter,
                                },
                                MarginLeft = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                            },
                        },
                    },
                },
                [$"{componentCls}-dropdown-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
                [$"{componentCls}-menu-item"] = new CSSObject()
                {
                    [$"{componentCls}-dropdown-rtl"] = new CSSObject()
                    {
                        TextAlign = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "right",
                        },
                    },
                },
            };
        }

        public static CSSObject GenTabsStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsCardPadding = token.TabsCardPadding;
            var cardHeight = token.CardHeight;
            var cardGutter = token.CardGutter;
            var itemHoverColor = token.ItemHoverColor;
            var itemActiveColor = token.ItemActiveColor;
            var colorBorderSecondary = token.ColorBorderSecondary;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "flex",
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "flex",
                        Flex = "none",
                        AlignItems = "center",
                        [$"{componentCls}-nav-wrap"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "flex",
                            Flex = "auto",
                            AlignSelf = "stretch",
                            Overflow = "hidden",
                            WhiteSpace = "nowrap",
                            Transform = "translate(0)",
                            ["&::before, &::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                ZIndex = 1,
                                Opacity = 0,
                                Transition = @$"opacity {token.MotionDurationSlow}",
                                Content = "\"\"",
                                PointerEvents = "none",
                            },
                        },
                        [$"{componentCls}-nav-list"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "flex",
                            Transition = @$"opacity {token.MotionDurationSlow}",
                        },
                        [$"{componentCls}-nav-operations"] = new CSSObject()
                        {
                            Display = "flex",
                            AlignSelf = "stretch",
                        },
                        [$"{componentCls}-nav-operations-hidden"] = new CSSObject()
                        {
                            Position = "absolute",
                            Visibility = "hidden",
                            PointerEvents = "none",
                        },
                        [$"{componentCls}-nav-more"] = new CSSObject()
                        {
                            Position = "relative",
                            Padding = tabsCardPadding,
                            Background = "transparent",
                            Border = 0,
                            Color = token.ColorText,
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Right = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                Bottom = 0,
                                Left = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                Height = token.ControlHeightLG / 8,
                                Transform = "translateY(100%)",
                                Content = "\"\"",
                            },
                        },
                        [$"{componentCls}-nav-add"] = new CSSObject()
                        {
                            MinWidth = cardHeight,
                            MarginLeft = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = cardGutter,
                            },
                            Padding = @$"0 {token.PaddingXS}px",
                            Background = "transparent",
                            Border = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                            BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
                            Outline = "none",
                            Cursor = "pointer",
                            Color = token.ColorText,
                            Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseInOut}",
                            ["&:hover"] = new CSSObject()
                            {
                                Color = itemHoverColor,
                            },
                            ["&:active, &:focus:not(:focus-visible)"] = new CSSObject()
                            {
                                Color = itemActiveColor,
                            },
                            ["..."] = GenFocusStyle(token)
                        },
                    },
                    [$"{componentCls}-extra-content"] = new CSSObject()
                    {
                        Flex = "none",
                    },
                    [$"{componentCls}-ink-bar"] = new CSSObject()
                    {
                        Position = "absolute",
                        Background = token.InkBarColor,
                        PointerEvents = "none",
                    },
                    ["..."] = GenTabStyle(token),
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Position = "relative",
                        Width = "100%",
                    },
                    [$"{componentCls}-content-holder"] = new CSSObject()
                    {
                        Flex = "auto",
                        MinWidth = 0,
                        MinHeight = 0,
                    },
                    [$"{componentCls}-tabpane"] = new CSSObject()
                    {
                        Outline = "none",
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                },
                [$"{componentCls}-centered"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        [$"{componentCls}-nav-wrap"] = new CSSObject()
                        {
                            [$"&:not([class*=\"{componentCls}-nav-wrap-ping\"])"] = new CSSObject()
                            {
                                JustifyContent = "center",
                            },
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Tabs",
                (token) =>
                {
                    var tabsToken = MergeToken(
                        token,
                        new TabsToken()
                        {
                            TabsCardPadding = token.CardPadding ?? $"{(token.CardHeight - Math.Round(token.FontSize * token.LineHeight)) / 2 - token.LineWidth}px {token.Padding}px",
                            DropdownEdgeChildVerticalPadding = token.PaddingXXS,
                            TabsActiveTextShadow = "0 0 0.25px currentcolor",
                            TabsDropdownHeight = 200,
                            TabsDropdownWidth = 120,
                            TabsHorizontalItemMargin = @$"0 0 0 {token.HorizontalItemGutter}px",
                            TabsHorizontalItemMarginRTL = @$"0 0 0 {token.HorizontalItemGutter}px",
                        });
                    return new CSSInterpolation[]
                    {
                        GenSizeStyle(tabsToken),
                        GenRtlStyle(tabsToken),
                        GenPositionStyle(tabsToken),
                        GenDropdownStyle(tabsToken),
                        GenCardStyle(tabsToken),
                        GenTabsStyle(tabsToken),
                        GenMotionStyle(tabsToken),
                    };
                },
                (token) =>
                {
                    var cardHeight = token.ControlHeightLG;
                    return new TabsToken()
                    {
                        ZIndexPopup = token.ZIndexPopupBase + 50,
                        CardBg = token.ColorFillAlter,
                        CardHeight = cardHeight,
                        CardPadding = @$"",
                        CardPaddingSM = @$"{token.PaddingXXS * 1.5}px {token.Padding}px",
                        CardPaddingLG = @$"{token.PaddingXS}px {token.Padding}px {token.PaddingXXS * 1.5}px",
                        TitleFontSize = token.FontSize,
                        TitleFontSizeLG = token.FontSizeLG,
                        TitleFontSizeSM = token.FontSize,
                        InkBarColor = token.ColorPrimary,
                        HorizontalMargin = @$"0 0 {token.Margin}px 0",
                        HorizontalItemGutter = 32,
                        HorizontalItemMargin = @$"",
                        HorizontalItemMarginRTL = @$"",
                        HorizontalItemPadding = @$"{token.PaddingSM}px 0",
                        HorizontalItemPaddingSM = @$"{token.PaddingXS}px 0",
                        HorizontalItemPaddingLG = @$"{token.Padding}px 0",
                        VerticalItemPadding = @$"{token.PaddingXS}px {token.PaddingLG}px",
                        VerticalItemMargin = @$"{token.Margin}px 0 0 0",
                        ItemColor = token.ColorText,
                        ItemSelectedColor = token.ColorPrimary,
                        ItemHoverColor = token.ColorPrimaryHover,
                        ItemActiveColor = token.ColorPrimaryActive,
                        CardGutter = token.MarginXXS / 2,
                    };
                });
        }

        public static CSSInterpolation[] GenMotionStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$"{componentCls}-switch"] = new CSSObject()
                        {
                            ["&-appear, &-enter"] = new CSSObject()
                            {
                                Transition = "none",
                                ["&-start"] = new CSSObject()
                                {
                                    Opacity = 0,
                                },
                                ["&-active"] = new CSSObject()
                                {
                                    Opacity = 1,
                                    Transition = @$"opacity {motionDurationSlow}",
                                },
                            },
                            ["&-leave"] = new CSSObject()
                            {
                                Position = "absolute",
                                Transition = "none",
                                Inset = 0,
                                ["&-start"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                                ["&-active"] = new CSSObject()
                                {
                                    Opacity = 0,
                                    Transition = @$"opacity {motionDurationSlow}",
                                },
                            },
                        },
                    },
                },
                InitSlideMotion(token, "slide-up"), InitSlideMotion(token, "slide-down"),
            };
        }

    }

}
