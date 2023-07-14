using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class PaginationToken : TokenWithCommonCls
    {
        public int PaginationItemSize { get; set; }

        public string PaginationFontFamily { get; set; }

        public string PaginationItemBg { get; set; }

        public string PaginationItemBgActive { get; set; }

        public int PaginationFontWeightActive { get; set; }

        public int PaginationItemSizeSM { get; set; }

        public string PaginationItemInputBg { get; set; }

        public int PaginationMiniOptionsSizeChangerTop { get; set; }

        public string PaginationItemDisabledBgActive { get; set; }

        public string PaginationItemDisabledColorActive { get; set; }

        public string PaginationItemLinkBg { get; set; }

        public string InputOutlineOffset { get; set; }

        public int PaginationMiniOptionsMarginInlineStart { get; set; }

        public int PaginationMiniQuickJumperInputWidth { get; set; }

        public int PaginationItemPaddingInline { get; set; }

        public int PaginationEllipsisLetterSpacing { get; set; }

        public string PaginationEllipsisTextIndent { get; set; }

        public int PaginationSlashMarginInlineStart { get; set; }

        public int PaginationSlashMarginInlineEnd { get; set; }

    }

    public partial class Pagination
    {
        public Unknown_1 GenPaginationDisabledStyle(Unknown_9 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_10()
            {
                [$"{componentCls}-disabled"] = new Unknown_11()
                {
                    ["&, &:hover"] = new Unknown_12()
                    {
                        Cursor = "not-allowed",
                        [$"{componentCls}-item-link"] = new Unknown_13()
                        {
                            Color = token.ColorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                    ["&:focus-visible"] = new Unknown_14()
                    {
                        Cursor = "not-allowed",
                        [$"{componentCls}-item-link"] = new Unknown_15()
                        {
                            Color = token.ColorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                },
                [$"&{componentCls}-disabled"] = new Unknown_16()
                {
                    Cursor = "not-allowed",
                    [$"&{componentCls}-mini"] = new Unknown_17()
                    {
                        [$"&:hover{componentCls}-item:not({componentCls}-item-active),&:active{componentCls}-item:not({componentCls}-item-active),&:hover{componentCls}-item-link,&:active{componentCls}-item-link"] = new Unknown_18()
                        {
                            BackgroundColor = "transparent",
                        },
                    },
                    [$"{componentCls}-item"] = new Unknown_19()
                    {
                        Cursor = "not-allowed",
                        ["&:hover, &:active"] = new Unknown_20()
                        {
                            BackgroundColor = "transparent",
                        },
                        ["a"] = new Unknown_21()
                        {
                            Color = token.ColorTextDisabled,
                            BackgroundColor = "transparent",
                            Border = "none",
                            Cursor = "not-allowed",
                        },
                        ["&-active"] = new Unknown_22()
                        {
                            BorderColor = token.ColorBorder,
                            BackgroundColor = token.PaginationItemDisabledBgActive,
                            ["&:hover, &:active"] = new Unknown_23()
                            {
                                BackgroundColor = token.PaginationItemDisabledBgActive,
                            },
                            ["a"] = new Unknown_24()
                            {
                                Color = token.PaginationItemDisabledColorActive,
                            },
                        },
                    },
                    [$"{componentCls}-item-link"] = new Unknown_25()
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                        ["&:hover, &:active"] = new Unknown_26()
                        {
                            BackgroundColor = "transparent",
                        },
                        [$"{componentCls}-simple&"] = new Unknown_27()
                        {
                            BackgroundColor = "transparent",
                            ["&:hover, &:active"] = new Unknown_28()
                            {
                                BackgroundColor = "transparent",
                            },
                        },
                    },
                    [$"{componentCls}-item-link-icon"] = new Unknown_29()
                    {
                        Opacity = 0,
                    },
                    [$"{componentCls}-item-ellipsis"] = new Unknown_30()
                    {
                        Opacity = 1,
                    },
                    [$"{componentCls}-simple-pager"] = new Unknown_31()
                    {
                        Color = token.ColorTextDisabled,
                    },
                },
                [$"&{componentCls}-simple"] = new Unknown_32()
                {
                    [$"{componentCls}-prev, {componentCls}-next"] = new Unknown_33()
                    {
                        [$"&{componentCls}-disabled {componentCls}-item-link"] = new Unknown_34()
                        {
                            ["&:hover, &:active"] = new Unknown_35()
                            {
                                BackgroundColor = "transparent",
                            },
                        },
                    },
                },
            };
        }

        public Unknown_2 GenPaginationMiniStyle(Unknown_36 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_37()
            {
                [$"&{componentCls}-mini {componentCls}-total-text, &{componentCls}-mini {componentCls}-simple-pager"] = new Unknown_38()
                {
                    Height = token.PaginationItemSizeSM,
                    LineHeight = @$"{token.PaginationItemSizeSM}px",
                },
                [$"&{componentCls}-mini {componentCls}-item"] = new Unknown_39()
                {
                    MinWidth = token.PaginationItemSizeSM,
                    Height = token.PaginationItemSizeSM,
                    Margin = 0,
                    LineHeight = @$"{token.PaginationItemSizeSM - 2}px",
                },
                [$"&{componentCls}-mini {componentCls}-item:not({componentCls}-item-active)"] = new Unknown_40()
                {
                    BackgroundColor = "transparent",
                    BorderColor = "transparent",
                    ["&:hover"] = new Unknown_41()
                    {
                        BackgroundColor = token.ColorBgTextHover,
                    },
                    ["&:active"] = new Unknown_42()
                    {
                        BackgroundColor = token.ColorBgTextActive,
                    },
                },
                [$"&{componentCls}-mini {componentCls}-prev, &{componentCls}-mini {componentCls}-next"] = new Unknown_43()
                {
                    MinWidth = token.PaginationItemSizeSM,
                    Height = token.PaginationItemSizeSM,
                    Margin = 0,
                    LineHeight = @$"{token.PaginationItemSizeSM}px",
                    [$"&:hover {componentCls}-item-link"] = new Unknown_44()
                    {
                        BackgroundColor = token.ColorBgTextHover,
                    },
                    [$"&:active {componentCls}-item-link"] = new Unknown_45()
                    {
                        BackgroundColor = token.ColorBgTextActive,
                    },
                    [$"&{componentCls}-disabled:hover {componentCls}-item-link"] = new Unknown_46()
                    {
                        BackgroundColor = "transparent",
                    },
                },
                [$"&{componentCls}-mini{componentCls}-prev{componentCls}-item-link,&{componentCls}-mini{componentCls}-next{componentCls}-item-link"] = new Unknown_47()
                {
                    BackgroundColor = "transparent",
                    BorderColor = "transparent",
                    ["&::after"] = new Unknown_48()
                    {
                        Height = token.PaginationItemSizeSM,
                        LineHeight = @$"{token.PaginationItemSizeSM}px",
                    },
                },
                [$"&{componentCls}-mini {componentCls}-jump-prev, &{componentCls}-mini {componentCls}-jump-next"] = new Unknown_49()
                {
                    Height = token.PaginationItemSizeSM,
                    MarginInlineEnd = 0,
                    LineHeight = @$"{token.PaginationItemSizeSM}px",
                },
                [$"&{componentCls}-mini {componentCls}-options"] = new Unknown_50()
                {
                    MarginInlineStart = token.PaginationMiniOptionsMarginInlineStart,
                    ["&-size-changer"] = new Unknown_51()
                    {
                        Top = token.PaginationMiniOptionsSizeChangerTop,
                    },
                    ["&-quick-jumper"] = new Unknown_52()
                    {
                        Height = token.PaginationItemSizeSM,
                        LineHeight = @$"{token.PaginationItemSizeSM}px",
                        Input = new Unknown_53()
                        {
                            ["..."] = GenInputSmallStyle(token),
                            Width = token.PaginationMiniQuickJumperInputWidth,
                            Height = token.ControlHeightSM,
                        },
                    },
                },
            };
        }

        public Unknown_3 GenPaginationSimpleStyle(Unknown_54 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_55()
            {
                [$"&{componentCls}-simple{componentCls}-prev,&{componentCls}-simple{componentCls}-next"] = new Unknown_56()
                {
                    Height = token.PaginationItemSizeSM,
                    LineHeight = @$"{token.PaginationItemSizeSM}px",
                    VerticalAlign = "top",
                    [$"{componentCls}-item-link"] = new Unknown_57()
                    {
                        Height = token.PaginationItemSizeSM,
                        BackgroundColor = "transparent",
                        Border = 0,
                        ["&:hover"] = new Unknown_58()
                        {
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        ["&:active"] = new Unknown_59()
                        {
                            BackgroundColor = token.ColorBgTextActive,
                        },
                        ["&::after"] = new Unknown_60()
                        {
                            Height = token.PaginationItemSizeSM,
                            LineHeight = @$"{token.PaginationItemSizeSM}px",
                        },
                    },
                },
                [$"&{componentCls}-simple {componentCls}-simple-pager"] = new Unknown_61()
                {
                    Display = "inline-block",
                    Height = token.PaginationItemSizeSM,
                    MarginInlineEnd = token.MarginXS,
                    Input = new Unknown_62()
                    {
                        BoxSizing = "border-box",
                        Height = "100%",
                        MarginInlineEnd = token.MarginXS,
                        Padding = @$"0 {token.PaginationItemPaddingInline}px",
                        TextAlign = "center",
                        BackgroundColor = token.PaginationItemInputBg,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        BorderRadius = token.BorderRadius,
                        Outline = "none",
                        Transition = @$"border-color {token.MotionDurationMid}",
                        Color = "inherit",
                        ["&:hover"] = new Unknown_63()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                        ["&:focus"] = new Unknown_64()
                        {
                            BorderColor = token.ColorPrimaryHover,
                            BoxShadow = @$"{token.InputOutlineOffset}px 0 {token.ControlOutlineWidth}px {token.ControlOutline}",
                        },
                        ["&[disabled]"] = new Unknown_65()
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

        public Unknown_4 GenPaginationJumpStyle(Unknown_66 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_67()
            {
                [$"{componentCls}-jump-prev, {componentCls}-jump-next"] = new Unknown_68()
                {
                    Outline = 0,
                    [$"{componentCls}-item-container"] = new Unknown_69()
                    {
                        Position = "relative",
                        [$"{componentCls}-item-link-icon"] = new Unknown_70()
                        {
                            Color = token.ColorPrimary,
                            FontSize = token.FontSizeSM,
                            Opacity = 0,
                            Transition = @$"all {token.MotionDurationMid}",
                            ["&-svg"] = new Unknown_71()
                            {
                                Top = 0,
                                InsetInlineEnd = 0,
                                Bottom = 0,
                                InsetInlineStart = 0,
                                Margin = "auto",
                            },
                        },
                        [$"{componentCls}-item-ellipsis"] = new Unknown_72()
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
                    ["&:hover"] = new Unknown_73()
                    {
                        [$"{componentCls}-item-link-icon"] = new Unknown_74()
                        {
                            Opacity = 1,
                        },
                        [$"{componentCls}-item-ellipsis"] = new Unknown_75()
                        {
                            Opacity = 0,
                        },
                    },
                    ["&:focus-visible"] = new Unknown_76()
                    {
                        [$"{componentCls}-item-link-icon"] = new Unknown_77()
                        {
                            Opacity = 1,
                        },
                        [$"{componentCls}-item-ellipsis"] = new Unknown_78()
                        {
                            Opacity = 0,
                        },
                        ["..."] = GenFocusOutline(token)
                    },
                },
                [$"{componentCls}-prev,{componentCls}-jump-prev,{componentCls}-jump-next"] = new Unknown_79()
                {
                    MarginInlineEnd = token.MarginXS,
                },
                [$"{componentCls}-prev,{componentCls}-next,{componentCls}-jump-prev,{componentCls}-jump-next"] = new Unknown_80()
                {
                    Display = "inline-block",
                    MinWidth = token.PaginationItemSize,
                    Height = token.PaginationItemSize,
                    Color = token.ColorText,
                    FontFamily = token.PaginationFontFamily,
                    LineHeight = @$"{token.PaginationItemSize}px",
                    TextAlign = "center",
                    VerticalAlign = "middle",
                    ListStyle = "none",
                    BorderRadius = token.BorderRadius,
                    Cursor = "pointer",
                    Transition = @$"all {token.MotionDurationMid}",
                },
                [$"{componentCls}-prev, {componentCls}-next"] = new Unknown_81()
                {
                    FontFamily = "Arial, Helvetica, sans-serif",
                    Outline = 0,
                    ["button"] = new Unknown_82()
                    {
                        Color = token.ColorText,
                        Cursor = "pointer",
                        UserSelect = "none",
                    },
                    [$"{componentCls}-item-link"] = new Unknown_83()
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
                        Transition = @$"border {token.MotionDurationMid}",
                    },
                    [$"&:focus-visible {componentCls}-item-link"] = new Unknown_84()
                    {
                        ["..."] = GenFocusOutline(token)
                    },
                    [$"&:hover {componentCls}-item-link"] = new Unknown_85()
                    {
                        BackgroundColor = token.ColorBgTextHover,
                    },
                    [$"&:active {componentCls}-item-link"] = new Unknown_86()
                    {
                        BackgroundColor = token.ColorBgTextActive,
                    },
                    [$"&{componentCls}-disabled:hover"] = new Unknown_87()
                    {
                        [$"{componentCls}-item-link"] = new Unknown_88()
                        {
                            BackgroundColor = "transparent",
                        },
                    },
                },
                [$"{componentCls}-slash"] = new Unknown_89()
                {
                    MarginInlineEnd = token.PaginationSlashMarginInlineEnd,
                    MarginInlineStart = token.PaginationSlashMarginInlineStart,
                },
                [$"{componentCls}-options"] = new Unknown_90()
                {
                    Display = "inline-block",
                    MarginInlineStart = token.Margin,
                    VerticalAlign = "middle",
                    ["&-size-changer.-select"] = new Unknown_91()
                    {
                        Display = "inline-block",
                        Width = "auto",
                    },
                    ["&-quick-jumper"] = new Unknown_92()
                    {
                        Display = "inline-block",
                        Height = token.ControlHeight,
                        MarginInlineStart = token.MarginXS,
                        LineHeight = @$"{token.ControlHeight}px",
                        VerticalAlign = "top",
                        Input = new Unknown_93()
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

        public Unknown_5 GenPaginationItemStyle(Unknown_94 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_95()
            {
                [$"{componentCls}-item"] = new Unknown_96()
                {
                    Display = "inline-block",
                    MinWidth = token.PaginationItemSize,
                    Height = token.PaginationItemSize,
                    MarginInlineEnd = token.MarginXS,
                    FontFamily = token.PaginationFontFamily,
                    LineHeight = @$"{token.PaginationItemSize - 2}px",
                    TextAlign = "center",
                    VerticalAlign = "middle",
                    ListStyle = "none",
                    BackgroundColor = "transparent",
                    Border = @$"{token.LineWidth}px {token.LineType} transparent",
                    BorderRadius = token.BorderRadius,
                    Outline = 0,
                    Cursor = "pointer",
                    UserSelect = "none",
                    ["a"] = new Unknown_97()
                    {
                        Display = "block",
                        Padding = @$"0 {token.PaginationItemPaddingInline}px",
                        Color = token.ColorText,
                        Transition = "none",
                        ["&:hover"] = new Unknown_98()
                        {
                            TextDecoration = "none",
                        },
                    },
                    [$"&:not({componentCls}-item-active)"] = new Unknown_99()
                    {
                        ["&:hover"] = new Unknown_100()
                        {
                            Transition = @$"all {token.MotionDurationMid}",
                            BackgroundColor = token.ColorBgTextHover,
                        },
                        ["&:active"] = new Unknown_101()
                        {
                            BackgroundColor = token.ColorBgTextActive,
                        },
                    },
                    ["..."] = GenFocusStyle(token),
                    ["&-active"] = new Unknown_102()
                    {
                        FontWeight = token.PaginationFontWeightActive,
                        BackgroundColor = token.PaginationItemBgActive,
                        BorderColor = token.ColorPrimary,
                        ["a"] = new Unknown_103()
                        {
                            Color = token.ColorPrimary,
                        },
                        ["&:hover"] = new Unknown_104()
                        {
                            BorderColor = token.ColorPrimaryHover,
                        },
                        ["&:hover a"] = new Unknown_105()
                        {
                            Color = token.ColorPrimaryHover,
                        },
                    },
                },
            };
        }

        public Unknown_6 GenPaginationStyle(Unknown_106 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_107()
            {
                [componentCls] = new Unknown_108()
                {
                    ["..."] = ResetComponent(token),
                    ["ul, ol"] = new Unknown_109()
                    {
                        Margin = 0,
                        Padding = 0,
                        ListStyle = "none",
                    },
                    ["&::after"] = new Unknown_110()
                    {
                        Display = "block",
                        Clear = "both",
                        Height = 0,
                        Overflow = "hidden",
                        Visibility = "hidden",
                        Content = "\"\"",
                    },
                    [$"{componentCls}-total-text"] = new Unknown_111()
                    {
                        Display = "inline-block",
                        Height = token.PaginationItemSize,
                        MarginInlineEnd = token.MarginXS,
                        LineHeight = @$"{token.PaginationItemSize - 2}px",
                        VerticalAlign = "middle",
                    },
                    ["..."] = GenPaginationItemStyle(token),
                    ["..."] = GenPaginationJumpStyle(token),
                    ["..."] = GenPaginationSimpleStyle(token),
                    ["..."] = GenPaginationMiniStyle(token),
                    ["..."] = GenPaginationDisabledStyle(token),
                    [$"@media only screen and (max-width: {token.ScreenLG}px)"] = new Unknown_112()
                    {
                        [$"{componentCls}-item"] = new Unknown_113()
                        {
                            ["&-after-jump-prev, &-before-jump-next"] = new Unknown_114()
                            {
                                Display = "none",
                            },
                        },
                    },
                    [$"@media only screen and (max-width: {token.ScreenSM}px)"] = new Unknown_115()
                    {
                        [$"{componentCls}-options"] = new Unknown_116()
                        {
                            Display = "none",
                        },
                    },
                },
                [$"&{token.ComponentCls}-rtl"] = new Unknown_117()
                {
                    Direction = "rtl",
                },
            };
        }

        public Unknown_7 GenBorderedStyle(Unknown_118 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_119()
            {
                [$"{componentCls}{componentCls}-disabled"] = new Unknown_120()
                {
                    ["&, &:hover"] = new Unknown_121()
                    {
                        [$"{componentCls}-item-link"] = new Unknown_122()
                        {
                            BorderColor = token.ColorBorder,
                        },
                    },
                    ["&:focus-visible"] = new Unknown_123()
                    {
                        [$"{componentCls}-item-link"] = new Unknown_124()
                        {
                            BorderColor = token.ColorBorder,
                        },
                    },
                    [$"{componentCls}-item, {componentCls}-item-link"] = new Unknown_125()
                    {
                        BackgroundColor = token.ColorBgContainerDisabled,
                        BorderColor = token.ColorBorder,
                        [$"&:hover:not({componentCls}-item-active)"] = new Unknown_126()
                        {
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                            ["a"] = new Unknown_127()
                            {
                                Color = token.ColorTextDisabled,
                            },
                        },
                        [$"&{componentCls}-item-active"] = new Unknown_128()
                        {
                            BackgroundColor = token.PaginationItemDisabledBgActive,
                        },
                    },
                    [$"{componentCls}-prev, {componentCls}-next"] = new Unknown_129()
                    {
                        ["&:hover button"] = new Unknown_130()
                        {
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                            Color = token.ColorTextDisabled,
                        },
                        [$"{componentCls}-item-link"] = new Unknown_131()
                        {
                            BackgroundColor = token.ColorBgContainerDisabled,
                            BorderColor = token.ColorBorder,
                        },
                    },
                },
                [componentCls] = new Unknown_132()
                {
                    [$"{componentCls}-prev, {componentCls}-next"] = new Unknown_133()
                    {
                        ["&:hover button"] = new Unknown_134()
                        {
                            BorderColor = token.ColorPrimaryHover,
                            BackgroundColor = token.PaginationItemBg,
                        },
                        [$"{componentCls}-item-link"] = new Unknown_135()
                        {
                            BackgroundColor = token.PaginationItemLinkBg,
                            BorderColor = token.ColorBorder,
                        },
                        [$"&:hover {componentCls}-item-link"] = new Unknown_136()
                        {
                            BorderColor = token.ColorPrimary,
                            BackgroundColor = token.PaginationItemBg,
                            Color = token.ColorPrimary,
                        },
                        [$"&{componentCls}-disabled"] = new Unknown_137()
                        {
                            [$"{componentCls}-item-link"] = new Unknown_138()
                            {
                                BorderColor = token.ColorBorder,
                                Color = token.ColorTextDisabled,
                            },
                        },
                    },
                    [$"{componentCls}-item"] = new Unknown_139()
                    {
                        BackgroundColor = token.PaginationItemBg,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                        [$"&:hover:not({componentCls}-item-active)"] = new Unknown_140()
                        {
                            BorderColor = token.ColorPrimary,
                            BackgroundColor = token.PaginationItemBg,
                            ["a"] = new Unknown_141()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                        ["&-active"] = new Unknown_142()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                    },
                },
            };
        }

        public Unknown_8 GenComponentStyleHook(Unknown_143 token)
        {
            var paginationToken = MergeToken(
                token,
                new Unknown_144()
                {
                    PaginationItemSize = token.ControlHeight,
                    PaginationFontFamily = token.FontFamily,
                    PaginationItemBg = token.ColorBgContainer,
                    PaginationItemBgActive = token.ColorBgContainer,
                    PaginationFontWeightActive = token.FontWeightStrong,
                    PaginationItemSizeSM = token.ControlHeightSM,
                    PaginationItemInputBg = token.ColorBgContainer,
                    PaginationMiniOptionsSizeChangerTop = 0,
                    PaginationItemDisabledBgActive = token.ControlItemBgActiveDisabled,
                    PaginationItemDisabledColorActive = token.ColorTextDisabled,
                    PaginationItemLinkBg = token.ColorBgContainer,
                    InputOutlineOffset = "0 0",
                    PaginationMiniOptionsMarginInlineStart = token.MarginXXS / 2,
                    PaginationMiniQuickJumperInputWidth = token.ControlHeightLG * 1.1,
                    PaginationItemPaddingInline = token.MarginXXS * 1.5,
                    PaginationEllipsisLetterSpacing = token.MarginXXS / 2,
                    PaginationSlashMarginInlineStart = token.MarginXXS,
                    PaginationSlashMarginInlineEnd = token.MarginSM,
                    PaginationEllipsisTextIndent = "0.13em",
                },
                initInputToken(token));
            return new Unknown_145
            {
                GenPaginationStyle(paginationToken),
                Token.Wireframe && genBorderedStyle(paginationToken)
            };
        }

    }

}