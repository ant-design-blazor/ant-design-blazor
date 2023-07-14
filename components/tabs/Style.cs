using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TabsToken
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class TabsToken : TokenWithCommonCls
    {
        public string TabsCardHorizontalPadding { get; set; }

        public int TabsCardHeight { get; set; }

        public int TabsCardGutter { get; set; }

        public string TabsHoverColor { get; set; }

        public string TabsActiveColor { get; set; }

        public int TabsHorizontalGutter { get; set; }

        public string TabsCardHeadBackground { get; set; }

        public int DropdownEdgeChildVerticalPadding { get; set; }

        public int TabsNavWrapPseudoWidth { get; set; }

        public string TabsActiveTextShadow { get; set; }

        public int TabsDropdownHeight { get; set; }

        public int TabsDropdownWidth { get; set; }

    }

    public partial class Tabs
    {
        public CSSObject GenCardStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsCardHorizontalPadding = token.TabsCardHorizontalPadding;
            var tabsCardHeadBackground = token.TabsCardHeadBackground;
            var tabsCardGutter = token.TabsCardGutter;
            var colorBorderSecondary = token.ColorBorderSecondary;
            return new CSSObject()
            {
                [$"{componentCls}-card"] = new CSSObject()
                {
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        [$"{componentCls}-tab"] = new CSSObject()
                        {
                            Margin = 0,
                            Padding = tabsCardHorizontalPadding,
                            Background = tabsCardHeadBackground,
                            Border = @$"{token.LineWidth}px {token.LineType} {colorBorderSecondary}",
                            Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseInOut}",
                        },
                        [$"{componentCls}-tab-active"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
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
                                    Value = @$"{tabsCardGutter}px",
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
                                MarginTop = @$"{tabsCardGutter}px",
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

        public CSSObject GenDropdownStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsHoverColor = token.TabsHoverColor;
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
                            ["..."] = textEllipsis,
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
                                    Color = tabsHoverColor,
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

        public CSSObject GenPositionStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var margin = token.Margin;
            var colorBorderSecondary = token.ColorBorderSecondary;
            return new CSSObject()
            {
                [$"{componentCls}-top, {componentCls}-bottom"] = new CSSObject()
                {
                    FlexDirection = "column",
                    [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new CSSObject()
                    {
                        Margin = @$"0 0 {margin}px 0",
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
                            Content = \"""\",
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
                            Padding = @$"{token.PaddingXS}px {token.PaddingLG}px",
                            TextAlign = "center",
                        },
                        [$"{componentCls}-tab + {componentCls}-tab"] = new CSSObject()
                        {
                            Margin = @$"{token.Margin}px 0 0 0",
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

        public CSSObject GenSizeStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var padding = token.Padding;
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
                                Padding = @$"{token.PaddingXS}px 0",
                                FontSize = token.FontSize,
                            },
                        },
                    },
                    ["&-large"] = new CSSObject()
                    {
                        [$"> {componentCls}-nav"] = new CSSObject()
                        {
                            [$"{componentCls}-tab"] = new CSSObject()
                            {
                                Padding = @$"{padding}px 0",
                                FontSize = token.FontSizeLG,
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
                                Padding = @$"{token.PaddingXXS * 1.5}px {padding}px",
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
                                Padding = @$"{token.PaddingXS}px {padding}px {token.PaddingXXS * 1.5}px",
                            },
                        },
                    },
                },
            };
        }

        public Unknown_1 GenTabStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsActiveColor = token.TabsActiveColor;
            var tabsHoverColor = token.TabsHoverColor;
            var iconCls = token.IconCls;
            var tabsHorizontalGutter = token.TabsHorizontalGutter;
            var tabCls = @$"{componentCls}-tab";
            return new Unknown_6()
            {
                [tabCls] = new Unknown_7()
                {
                    Position = "relative",
                    Display = "inline-flex",
                    AlignItems = "center",
                    Padding = @$"{token.PaddingSM}px 0",
                    FontSize = @$"{token.FontSize}px",
                    Background = "transparent",
                    Border = 0,
                    Outline = "none",
                    Cursor = "pointer",
                    ["&-btn, &-remove"] = new Unknown_8()
                    {
                        ["&:focus:not(:focus-visible), &:active"] = new Unknown_9()
                        {
                            Color = tabsActiveColor,
                        },
                        ["..."] = GenFocusStyle(token)
                    },
                    ["&-btn"] = new Unknown_10()
                    {
                        Outline = "none",
                        Transition = "all 0.3s",
                    },
                    ["&-remove"] = new Unknown_11()
                    {
                        Flex = "none",
                        MarginRight = new Unknown_12()
                        {
                            SkipCheck = true,
                            Value = -token.MarginXXS,
                        },
                        MarginLeft = new Unknown_13()
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
                        ["&:hover"] = new Unknown_14()
                        {
                            Color = token.ColorTextHeading,
                        },
                    },
                    ["&:hover"] = new Unknown_15()
                    {
                        Color = tabsHoverColor,
                    },
                    [$"&{tabCls}-active {tabCls}-btn"] = new Unknown_16()
                    {
                        Color = token.ColorPrimary,
                        TextShadow = token.TabsActiveTextShadow,
                    },
                    [$"&{tabCls}-disabled"] = new Unknown_17()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                    },
                    [$"&{tabCls}-disabled {tabCls}-btn, &{tabCls}-disabled {componentCls}-remove"] = new Unknown_18()
                    {
                        ["&:focus, &:active"] = new Unknown_19()
                        {
                            Color = token.ColorTextDisabled,
                        },
                    },
                    [$"& {tabCls}-remove {iconCls}"] = new Unknown_20()
                    {
                        Margin = 0,
                    },
                    [iconCls] = new Unknown_21()
                    {
                        MarginRight = new Unknown_22()
                        {
                            SkipCheck = true,
                            Value = token.MarginSM,
                        },
                    },
                },
                [$"{tabCls} + {tabCls}"] = new Unknown_23()
                {
                    Margin = new Unknown_24()
                    {
                        SkipCheck = true,
                        Value = @$"0 0 0 {tabsHorizontalGutter}px",
                    },
                },
            };
        }

        public Unknown_2 GenRtlStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsHorizontalGutter = token.TabsHorizontalGutter;
            var iconCls = token.IconCls;
            var tabsCardGutter = token.TabsCardGutter;
            var rtlCls = @$"{componentCls}-rtl";
            return new Unknown_25()
            {
                [rtlCls] = new Unknown_26()
                {
                    Direction = "rtl",
                    [$"{componentCls}-nav"] = new Unknown_27()
                    {
                        [$"{componentCls}-tab"] = new Unknown_28()
                        {
                            Margin = new Unknown_29()
                            {
                                SkipCheck = true,
                                Value = @$"0 0 0 {tabsHorizontalGutter}px",
                            },
                            [$"{componentCls}-tab:last-of-type"] = new Unknown_30()
                            {
                                MarginLeft = new Unknown_31()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                            },
                            [iconCls] = new Unknown_32()
                            {
                                MarginRight = new Unknown_33()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                MarginLeft = new Unknown_34()
                                {
                                    SkipCheck = true,
                                    Value = @$"{token.MarginSM}px",
                                },
                            },
                            [$"{componentCls}-tab-remove"] = new Unknown_35()
                            {
                                MarginRight = new Unknown_36()
                                {
                                    SkipCheck = true,
                                    Value = @$"{token.MarginXS}px",
                                },
                                MarginLeft = new Unknown_37()
                                {
                                    SkipCheck = true,
                                    Value = @$"-{token.MarginXXS}px",
                                },
                                [iconCls] = new Unknown_38()
                                {
                                    Margin = 0,
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-left"] = new Unknown_39()
                    {
                        [$"> {componentCls}-nav"] = new Unknown_40()
                        {
                            Order = 1,
                        },
                        [$"> {componentCls}-content-holder"] = new Unknown_41()
                        {
                            Order = 0,
                        },
                    },
                    [$"&{componentCls}-right"] = new Unknown_42()
                    {
                        [$"> {componentCls}-nav"] = new Unknown_43()
                        {
                            Order = 0,
                        },
                        [$"> {componentCls}-content-holder"] = new Unknown_44()
                        {
                            Order = 1,
                        },
                    },
                    [$"&{componentCls}-card{componentCls}-top, &{componentCls}-card{componentCls}-bottom"] = new Unknown_45()
                    {
                        [$"> {componentCls}-nav, > div > {componentCls}-nav"] = new Unknown_46()
                        {
                            [$"{componentCls}-tab + {componentCls}-tab"] = new Unknown_47()
                            {
                                MarginRight = new Unknown_48()
                                {
                                    SkipCheck = true,
                                    Value = @$"{tabsCardGutter}px",
                                },
                                MarginLeft = new Unknown_49()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                            },
                        },
                    },
                },
                [$"{componentCls}-dropdown-rtl"] = new Unknown_50()
                {
                    Direction = "rtl",
                },
                [$"{componentCls}-menu-item"] = new Unknown_51()
                {
                    [$"{componentCls}-dropdown-rtl"] = new Unknown_52()
                    {
                        TextAlign = new Unknown_53()
                        {
                            SkipCheck = true,
                            Value = "right",
                        },
                    },
                },
            };
        }

        public CSSObject GenTabsStyle(TabsToken token)
        {
            var componentCls = token.ComponentCls;
            var tabsCardHorizontalPadding = token.TabsCardHorizontalPadding;
            var tabsCardHeight = token.TabsCardHeight;
            var tabsCardGutter = token.TabsCardGutter;
            var tabsHoverColor = token.TabsHoverColor;
            var tabsActiveColor = token.TabsActiveColor;
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
                                Content = \"""\",
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
                            Padding = tabsCardHorizontalPadding,
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
                                Content = \"""\",
                            },
                        },
                        [$"{componentCls}-nav-add"] = new CSSObject()
                        {
                            MinWidth = @$"{tabsCardHeight}px",
                            MarginLeft = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = @$"{tabsCardGutter}px",
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
                                Color = tabsHoverColor,
                            },
                            ["&:active, &:focus:not(:focus-visible)"] = new CSSObject()
                            {
                                Color = tabsActiveColor,
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
                        Background = token.ColorPrimary,
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
                            [$"&:not([class*="{componentCls}-nav-wrap-ping"])"] = new CSSObject()
                            {
                                JustifyContent = "center",
                            },
                        },
                    },
                },
            };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_54 token)
        {
            var tabsCardHeight = token.ControlHeightLG;
            var tabsToken = MergeToken(
                token,
                new Unknown_55()
                {
                    TabsHoverColor = token.ColorPrimaryHover,
                    TabsActiveColor = token.ColorPrimaryActive,
                    TabsCardHorizontalPadding = @$"{
        (tabsCardHeight - Math.Round(token.FontSize * token.LineHeight)) / 2 - token.LineWidth
      }px {token.Padding}px",
                    TabsCardHeight = tabsCardHeight,
                    TabsCardGutter = token.MarginXXS / 2,
                    TabsHorizontalGutter = 32,
                    TabsCardHeadBackground = token.ColorFillAlter,
                    DropdownEdgeChildVerticalPadding = token.PaddingXXS,
                    TabsActiveTextShadow = "0 0 0.25px currentcolor",
                    TabsDropdownHeight = 200,
                    TabsDropdownWidth = 120,
                });
            return new Unknown_56
            {
                GenSizeStyle(tabsToken),
                GenRtlStyle(tabsToken),
                GenPositionStyle(tabsToken),
                GenDropdownStyle(tabsToken),
                GenCardStyle(tabsToken),
                GenTabsStyle(tabsToken),
                GenMotionStyle(tabsToken)
            };
        }

        public Unknown_4 GenComponentStyleHook(Unknown_57 token)
        {
            return new Unknown_58()
            {
                ZIndexPopup = token.ZIndexPopupBase + 50,
            };
        }

        public Unknown_5 GenMotionStyle(Unknown_59 token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            return new Unknown_60
            {
                new Unknown_61()
                {
                    [componentCls] = new Unknown_62()
                    {
                        [$"{componentCls}-switch"] = new Unknown_63()
                        {
                            ["&-appear, &-enter"] = new Unknown_64()
                            {
                                Transition = "none",
                                ["&-start"] = new Unknown_65()
                                {
                                    Opacity = 0,
                                },
                                ["&-active"] = new Unknown_66()
                                {
                                    Opacity = 1,
                                    Transition = @$"opacity {motionDurationSlow}",
                                },
                            },
                            ["&-leave"] = new Unknown_67()
                            {
                                Position = "absolute",
                                Transition = "none",
                                Inset = 0,
                                ["&-start"] = new Unknown_68()
                                {
                                    Opacity = 1,
                                },
                                ["&-active"] = new Unknown_69()
                                {
                                    Opacity = 0,
                                    Transition = @$"opacity {motionDurationSlow}",
                                },
                            },
                        },
                    },
                },
                [initSlideMotion(token, "slide-up"), initSlideMotion(token, "slide-down")]
            };
        }

    }

}