using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.InputStyle;

namespace AntDesign
{
    public partial class PaginationToken
    {
        public string ItemBg
        {
            get => (string)_tokens["itemBg"];
            set => _tokens["itemBg"] = value;
        }

        public double ItemSize
        {
            get => (double)_tokens["itemSize"];
            set => _tokens["itemSize"] = value;
        }

        public string ItemActiveBg
        {
            get => (string)_tokens["itemActiveBg"];
            set => _tokens["itemActiveBg"] = value;
        }

        public double ItemSizeSM
        {
            get => (double)_tokens["itemSizeSM"];
            set => _tokens["itemSizeSM"] = value;
        }

        public string ItemLinkBg
        {
            get => (string)_tokens["itemLinkBg"];
            set => _tokens["itemLinkBg"] = value;
        }

        public string ItemActiveBgDisabled
        {
            get => (string)_tokens["itemActiveBgDisabled"];
            set => _tokens["itemActiveBgDisabled"] = value;
        }

        public string ItemActiveColorDisabled
        {
            get => (string)_tokens["itemActiveColorDisabled"];
            set => _tokens["itemActiveColorDisabled"] = value;
        }

        public string ItemInputBg
        {
            get => (string)_tokens["itemInputBg"];
            set => _tokens["itemInputBg"] = value;
        }

        public double MiniOptionsSizeChangerTop
        {
            get => (double)_tokens["miniOptionsSizeChangerTop"];
            set => _tokens["miniOptionsSizeChangerTop"] = value;
        }

    }

    public partial class PaginationToken : InputToken
    {
        public double InputOutlineOffset
        {
            get => (double)_tokens["inputOutlineOffset"];
            set => _tokens["inputOutlineOffset"] = value;
        }

        public double PaginationMiniOptionsMarginInlineStart
        {
            get => (double)_tokens["paginationMiniOptionsMarginInlineStart"];
            set => _tokens["paginationMiniOptionsMarginInlineStart"] = value;
        }

        public double PaginationMiniQuickJumperInputWidth
        {
            get => (double)_tokens["paginationMiniQuickJumperInputWidth"];
            set => _tokens["paginationMiniQuickJumperInputWidth"] = value;
        }

        public double PaginationItemPaddingInline
        {
            get => (double)_tokens["paginationItemPaddingInline"];
            set => _tokens["paginationItemPaddingInline"] = value;
        }

        public double PaginationEllipsisLetterSpacing
        {
            get => (double)_tokens["paginationEllipsisLetterSpacing"];
            set => _tokens["paginationEllipsisLetterSpacing"] = value;
        }

        public string PaginationEllipsisTextIndent
        {
            get => (string)_tokens["paginationEllipsisTextIndent"];
            set => _tokens["paginationEllipsisTextIndent"] = value;
        }

        public double PaginationSlashMarginInlineStart
        {
            get => (double)_tokens["paginationSlashMarginInlineStart"];
            set => _tokens["paginationSlashMarginInlineStart"] = value;
        }

        public double PaginationSlashMarginInlineEnd
        {
            get => (double)_tokens["paginationSlashMarginInlineEnd"];
            set => _tokens["paginationSlashMarginInlineEnd"] = value;
        }

    }

    public partial class PaginationStyle
    {
        public static CSSObject GenPaginationDisabledStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-disabled"] = new CSSObject()
                {
                    ["&, &:hover"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                    ["&:focus-visible"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                },
                [$"&{componentCls}-disabled"] = new CSSObject()
                {
                    Cursor = "not-allowed",
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        ["&:hover, &:active"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                        },
                        ["a"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                            BackgroundColor = "transparent",
                            Border = "none",
                            Cursor = "not-allowed",
                        },
                        ["&-active"] = new CSSObject()
                        {
                            BorderColor = token.ColorBorder,
                            BackgroundColor = token.ItemActiveBgDisabled,
                            ["&:hover, &:active"] = new CSSObject()
                            {
                                BackgroundColor = token.ItemActiveBgDisabled,
                            },
                            ["a"] = new CSSObject()
                            {
                                Color = token.ItemActiveColorDisabled,
                            },
                        },
                    },
                    [$"{componentCls}-item-link"] = new CSSObject()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                        ["&:hover, &:active"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                        },
                        [$"{componentCls}-simple&"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                            ["&:hover, &:active"] = new CSSObject()
                            {
                                BackgroundColor = "transparent",
                            },
                        },
                    },
                    [$"{componentCls}-simple-pager"] = new CSSObject()
                    {
                        Color = token.ColorTextDisabled,
                    },
                    [$"{componentCls}-jump-prev, {componentCls}-jump-next"] = new CSSObject()
                    {
                        [$"{componentCls}-item-link-icon"] = new CSSObject()
                        {
                            Opacity = 0,
                        },
                        [$"{componentCls}-item-ellipsis"] = new CSSObject()
                        {
                            Opacity = 1,
                        },
                    },
                },
                [$"&{componentCls}-simple"] = new CSSObject()
                {
                    [$"{componentCls}-prev, {componentCls}-next"] = new CSSObject()
                    {
                        [$"&{componentCls}-disabled {componentCls}-item-link"] = new CSSObject()
                        {
                            ["&:hover, &:active"] = new CSSObject()
                            {
                                BackgroundColor = "transparent",
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenPaginationMiniStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"&{componentCls}-mini {componentCls}-total-text, &{componentCls}-mini {componentCls}-simple-pager"] = new CSSObject()
                {
                    Height = token.ItemSizeSM,
                    LineHeight = @$"{token.ItemSizeSM}px",
                },
                [$"&{componentCls}-mini {componentCls}-item"] = new CSSObject()
                {
                    MinWidth = token.ItemSizeSM,
                    Height = token.ItemSizeSM,
                    Margin = 0,
                    LineHeight = @$"{token.ItemSizeSM - 2}px",
                },
                [$"&{componentCls}-mini:not({componentCls}-disabled) {componentCls}-item:not({componentCls}-item-active)"] = new CSSObject()
                {
                    BackgroundColor = "transparent",
                    BorderColor = "transparent",
                    ["&:hover"] = new CSSObject()
                    {
                        BackgroundColor = token.ColorBgTextHover,
                    },
                    ["&:active"] = new CSSObject()
                    {
                        BackgroundColor = token.ColorBgTextActive,
                    },
                },
                [$"&{componentCls}-mini {componentCls}-prev, &{componentCls}-mini {componentCls}-next"] = new CSSObject()
                {
                    MinWidth = token.ItemSizeSM,
                    Height = token.ItemSizeSM,
                    Margin = 0,
                    LineHeight = @$"{token.ItemSizeSM}px",
                },
                [$"&{componentCls}-mini:not({componentCls}-disabled)"] = new CSSObject()
                {
                    [$"{componentCls}-prev, {componentCls}-next"] = new CSSObject()
                    {
                        [$"&:hover {componentCls}-item-link"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        [$"&:active {componentCls}-item-link"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgTextActive,
                        },
                        [$"&{componentCls}-disabled:hover {componentCls}-item-link"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                        },
                    },
                },
                [$"&{componentCls}-mini{componentCls}-prev{componentCls}-item-link,&{componentCls}-mini{componentCls}-next{componentCls}-item-link"] = new CSSObject()
                {
                    BackgroundColor = "transparent",
                    BorderColor = "transparent",
                    ["&::after"] = new CSSObject()
                    {
                        Height = token.ItemSizeSM,
                        LineHeight = @$"{token.ItemSizeSM}px",
                    },
                },
                [$"&{componentCls}-mini {componentCls}-jump-prev, &{componentCls}-mini {componentCls}-jump-next"] = new CSSObject()
                {
                    Height = token.ItemSizeSM,
                    MarginInlineEnd = 0,
                    LineHeight = @$"{token.ItemSizeSM}px",
                },
                [$"&{componentCls}-mini {componentCls}-options"] = new CSSObject()
                {
                    MarginInlineStart = token.PaginationMiniOptionsMarginInlineStart,
                    ["&-size-changer"] = new CSSObject()
                    {
                        Top = token.MiniOptionsSizeChangerTop,
                    },
                    ["&-quick-jumper"] = new CSSObject()
                    {
                        Height = token.ItemSizeSM,
                        LineHeight = @$"{token.ItemSizeSM}px",
                        ["input"] = new CSSObject()
                        {
                            ["..."] = GenInputSmallStyle(token),
                            Width = token.PaginationMiniQuickJumperInputWidth,
                            Height = token.ControlHeightSM,
                        },
                    },
                },
            };
        }

        public static CSSObject GenPaginationSimpleStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"&{componentCls}-simple{componentCls}-prev,&{componentCls}-simple{componentCls}-next"] = new CSSObject()
                {
                    Height = token.ItemSizeSM,
                    LineHeight = @$"{token.ItemSizeSM}px",
                    VerticalAlign = "top",
                    [$"{componentCls}-item-link"] = new CSSObject()
                    {
                        Height = token.ItemSizeSM,
                        BackgroundColor = "transparent",
                        Border = 0,
                        ["&:hover"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        ["&:active"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgTextActive,
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Height = token.ItemSizeSM,
                            LineHeight = @$"{token.ItemSizeSM}px",
                        },
                    },
                },
                [$"&{componentCls}-simple {componentCls}-simple-pager"] = new CSSObject()
                {
                    Display = "inline-block",
                    Height = token.ItemSizeSM,
                    MarginInlineEnd = token.MarginXS,
                    ["input"] = new CSSObject()
                    {
                        BoxSizing = "border-box",
                        Height = "100%",
                        MarginInlineEnd = token.MarginXS,
                        Padding = @$"0 {token.PaginationItemPaddingInline}px",
                        TextAlign = "center",
                        BackgroundColor = token.ItemInputBg,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        BorderRadius = token.BorderRadius,
                        Outline = "none",
                        Transition = @$"border-color {token.MotionDurationMid}",
                        Color = "inherit",
                        ["&:hover"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                        ["&:focus"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                            BoxShadow = @$"{token.InputOutlineOffset}px 0 {token.ControlOutlineWidth}px {token.ControlOutline}",
                        },
                        ["&[disabled]"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                            Cursor = "not-allowed",
                        },
                    },
                },
            };
        }

        public static CSSObject GenPaginationJumpStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-jump-prev, {componentCls}-jump-next"] = new CSSObject()
                {
                    Outline = 0,
                    [$"{componentCls}-item-container"] = new CSSObject()
                    {
                        Position = "relative",
                        [$"{componentCls}-item-link-icon"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                            FontSize = token.FontSizeSM,
                            Opacity = 0,
                            Transition = @$"all {token.MotionDurationMid}",
                            ["&-svg"] = new CSSObject()
                            {
                                Top = 0,
                                InsetInlineEnd = 0,
                                Bottom = 0,
                                InsetInlineStart = 0,
                                Margin = "auto",
                            },
                        },
                        [$"{componentCls}-item-ellipsis"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineEnd = 0,
                            Bottom = 0,
                            InsetInlineStart = 0,
                            Display = "block",
                            Margin = "auto",
                            Color = token.ColorTextDisabled,
                            FontFamily = "Arial, Helvetica, sans-serif",
                            LetterSpacing = token.PaginationEllipsisLetterSpacing,
                            TextAlign = "center",
                            TextIndent = token.PaginationEllipsisTextIndent,
                            Opacity = 1,
                            Transition = @$"all {token.MotionDurationMid}",
                        },
                    },
                    ["&:hover"] = new CSSObject()
                    {
                        [$"{componentCls}-item-link-icon"] = new CSSObject()
                        {
                            Opacity = 1,
                        },
                        [$"{componentCls}-item-ellipsis"] = new CSSObject()
                        {
                            Opacity = 0,
                        },
                    },
                },
                [$"{componentCls}-prev,{componentCls}-jump-prev,{componentCls}-jump-next"] = new CSSObject()
                {
                    MarginInlineEnd = token.MarginXS,
                },
                [$"{componentCls}-prev,{componentCls}-next,{componentCls}-jump-prev,{componentCls}-jump-next"] = new CSSObject()
                {
                    Display = "inline-block",
                    MinWidth = token.ItemSize,
                    Height = token.ItemSize,
                    Color = token.ColorText,
                    FontFamily = token.FontFamily,
                    LineHeight = @$"{token.ItemSize}px",
                    TextAlign = "center",
                    VerticalAlign = "middle",
                    ListStyle = "none",
                    BorderRadius = token.BorderRadius,
                    Cursor = "pointer",
                    Transition = @$"all {token.MotionDurationMid}",
                },
                [$"{componentCls}-prev, {componentCls}-next"] = new CSSObject()
                {
                    FontFamily = "Arial, Helvetica, sans-serif",
                    Outline = 0,
                    ["button"] = new CSSObject()
                    {
                        Color = token.ColorText,
                        Cursor = "pointer",
                        UserSelect = "none",
                    },
                    [$"{componentCls}-item-link"] = new CSSObject()
                    {
                        Display = "block",
                        Width = "100%",
                        Height = "100%",
                        Padding = 0,
                        FontSize = token.FontSizeSM,
                        TextAlign = "center",
                        BackgroundColor = "transparent",
                        Border = @$"{token.LineWidth}px {token.LineType} transparent",
                        BorderRadius = token.BorderRadius,
                        Outline = "none",
                        Transition = @$"all {token.MotionDurationMid}",
                    },
                    [$"&:hover {componentCls}-item-link"] = new CSSObject()
                    {
                        BackgroundColor = token.ColorBgTextHover,
                    },
                    [$"&:active {componentCls}-item-link"] = new CSSObject()
                    {
                        BackgroundColor = token.ColorBgTextActive,
                    },
                    [$"&{componentCls}-disabled:hover"] = new CSSObject()
                    {
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                        },
                    },
                },
                [$"{componentCls}-slash"] = new CSSObject()
                {
                    MarginInlineEnd = token.PaginationSlashMarginInlineEnd,
                    MarginInlineStart = token.PaginationSlashMarginInlineStart,
                },
                [$"{componentCls}-options"] = new CSSObject()
                {
                    Display = "inline-block",
                    MarginInlineStart = token.Margin,
                    VerticalAlign = "middle",
                    ["&-size-changer.-select"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = "auto",
                    },
                    ["&-quick-jumper"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Height = token.ControlHeight,
                        MarginInlineStart = token.MarginXS,
                        LineHeight = @$"{token.ControlHeight}px",
                        VerticalAlign = "top",
                        ["input"] = new CSSObject()
                        {
                            ["..."] = GenBasicInputStyle(token),
                            Width = token.ControlHeightLG * 1.25,
                            Height = token.ControlHeight,
                            BoxSizing = "border-box",
                            Margin = 0,
                            MarginInlineStart = token.MarginXS,
                            MarginInlineEnd = token.MarginXS,
                        },
                    },
                },
            };
        }

        public static CSSObject GenPaginationItemStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-item"] = new CSSObject()
                {
                    Display = "inline-block",
                    MinWidth = token.ItemSize,
                    Height = token.ItemSize,
                    MarginInlineEnd = token.MarginXS,
                    FontFamily = token.FontFamily,
                    LineHeight = @$"{token.ItemSize - 2}px",
                    TextAlign = "center",
                    VerticalAlign = "middle",
                    ListStyle = "none",
                    BackgroundColor = "transparent",
                    Border = @$"{token.LineWidth}px {token.LineType} transparent",
                    BorderRadius = token.BorderRadius,
                    Outline = 0,
                    Cursor = "pointer",
                    UserSelect = "none",
                    ["a"] = new CSSObject()
                    {
                        Display = "block",
                        Padding = @$"0 {token.PaginationItemPaddingInline}px",
                        Color = token.ColorText,
                        ["&:hover"] = new CSSObject()
                        {
                            TextDecoration = "none",
                        },
                    },
                    [$"&:not({componentCls}-item-active)"] = new CSSObject()
                    {
                        ["&:hover"] = new CSSObject()
                        {
                            Transition = @$"all {token.MotionDurationMid}",
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        ["&:active"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgTextActive,
                        },
                    },
                    ["&-active"] = new CSSObject()
                    {
                        FontWeight = token.FontWeightStrong,
                        BackgroundColor = token.ItemActiveBg,
                        BorderColor = token.ColorPrimary,
                        ["a"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                        },
                        ["&:hover"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                        },
                        ["&:hover a"] = new CSSObject()
                        {
                            Color = token.ColorPrimaryHover,
                        },
                    },
                },
            };
        }

        public static CSSObject GenPaginationStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    ["ul, ol"] = new CSSObject()
                    {
                        Margin = 0,
                        Padding = 0,
                        ListStyle = "none",
                    },
                    ["&::after"] = new CSSObject()
                    {
                        Display = "block",
                        Clear = "both",
                        Height = 0,
                        Overflow = "hidden",
                        Visibility = "hidden",
                        Content = "\"\"",
                    },
                    [$"{componentCls}-total-text"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Height = token.ItemSize,
                        MarginInlineEnd = token.MarginXS,
                        LineHeight = @$"{token.ItemSize - 2}px",
                        VerticalAlign = "middle",
                    },
                    ["..."] = GenPaginationItemStyle(token),
                    ["..."] = GenPaginationJumpStyle(token),
                    ["..."] = GenPaginationSimpleStyle(token),
                    ["..."] = GenPaginationMiniStyle(token),
                    ["..."] = GenPaginationDisabledStyle(token),
                    [$"@media only screen and (max-width: {token.ScreenLG}px)"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            ["&-after-jump-prev, &-before-jump-next"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                    },
                    [$"@media only screen and (max-width: {token.ScreenSM}px)"] = new CSSObject()
                    {
                        [$"{componentCls}-options"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                },
                [$"&{token.ComponentCls}-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
            };
        }

        public static CSSObject GenBorderedStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}{componentCls}-disabled:not({componentCls}-mini)"] = new CSSObject()
                {
                    ["&, &:hover"] = new CSSObject()
                    {
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            BorderColor = token.ColorBorder,
                        },
                    },
                    ["&:focus-visible"] = new CSSObject()
                    {
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            BorderColor = token.ColorBorder,
                        },
                    },
                    [$"{componentCls}-item, {componentCls}-item-link"] = new CSSObject()
                    {
                        BackgroundColor = token.ColorBgContainerDisabled,
                        BorderColor = token.ColorBorder,
                        [$"&:hover:not({componentCls}-item-active)"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                            ["a"] = new CSSObject()
                            {
                                Color = token.ColorTextDisabled,
                            },
                        },
                        [$"&{componentCls}-item-active"] = new CSSObject()
                        {
                            BackgroundColor = token.ItemActiveBgDisabled,
                        },
                    },
                    [$"{componentCls}-prev, {componentCls}-next"] = new CSSObject()
                    {
                        ["&:hover button"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                            Color = token.ColorTextDisabled,
                        },
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                        },
                    },
                },
                [$"{componentCls}:not({componentCls}-mini)"] = new CSSObject()
                {
                    [$"{componentCls}-prev, {componentCls}-next"] = new CSSObject()
                    {
                        ["&:hover button"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                            BackgroundColor = token.ItemBg,
                        },
                        [$"{componentCls}-item-link"] = new CSSObject()
                        {
                            BackgroundColor = token.ItemLinkBg,
                            BorderColor = token.ColorBorder,
                        },
                        [$"&:hover {componentCls}-item-link"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                            BackgroundColor = token.ItemBg,
                            Color = token.ColorPrimary,
                        },
                        [$"&{componentCls}-disabled"] = new CSSObject()
                        {
                            [$"{componentCls}-item-link"] = new CSSObject()
                            {
                                BorderColor = token.ColorBorder,
                                Color = token.ColorTextDisabled,
                            },
                        },
                    },
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        BackgroundColor = token.ItemBg,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        [$"&:hover:not({componentCls}-item-active)"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                            BackgroundColor = token.ItemBg,
                            ["a"] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                        ["&-active"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                    },
                },
            };
        }

        public static CSSObject GenPaginationFocusStyle(PaginationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}:not({componentCls}-disabled)"] = new CSSObject()
                {
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        ["..."] = GenFocusStyle(token)
                    },
                    [$"{componentCls}-jump-prev, {componentCls}-jump-next"] = new CSSObject()
                    {
                        ["&:focus-visible"] = new CSSObject()
                        {
                            [$"{componentCls}-item-link-icon"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                            [$"{componentCls}-item-ellipsis"] = new CSSObject()
                            {
                                Opacity = 0,
                            },
                            ["..."] = GenFocusOutline(token)
                        },
                    },
                    [$"{componentCls}-prev, {componentCls}-next"] = new CSSObject()
                    {
                        [$"&:focus-visible {componentCls}-item-link"] = new CSSObject()
                        {
                            ["..."] = GenFocusOutline(token)
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Pagination",
                (token) =>
                {
                    var paginationToken = MergeToken<PaginationToken>(
                        token,
                        new PaginationToken()
                        {
                            InputOutlineOffset = 0,
                            PaginationMiniOptionsMarginInlineStart = token.MarginXXS / 2,
                            PaginationMiniQuickJumperInputWidth = token.ControlHeightLG * 1.1,
                            PaginationItemPaddingInline = token.MarginXXS * 1.5,
                            PaginationEllipsisLetterSpacing = token.MarginXXS / 2,
                            PaginationSlashMarginInlineStart = token.MarginXXS,
                            PaginationSlashMarginInlineEnd = token.MarginSM,
                            PaginationEllipsisTextIndent = "0.13em",
                        },
                        InitInputToken(token),
                        InitComponentToken(token));
                    return new CSSInterpolation[]
                    {
                        GenPaginationStyle(paginationToken),
                        GenPaginationFocusStyle(paginationToken),
                        token.Wireframe ? GenBorderedStyle(paginationToken) : null
                    };
                },
                (token) =>
                {
                    return new PaginationToken()
                    {
                        ItemBg = token.ColorBgContainer,
                        ItemSize = token.ControlHeight,
                        ItemSizeSM = token.ControlHeightSM,
                        ItemActiveBg = token.ColorBgContainer,
                        ItemLinkBg = token.ColorBgContainer,
                        ItemActiveColorDisabled = token.ColorTextDisabled,
                        ItemActiveBgDisabled = token.ControlItemBgActiveDisabled,
                        ItemInputBg = token.ColorBgContainer,
                        MiniOptionsSizeChangerTop = 0,
                    };
                });
        }

    }

}
