using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SegmentedToken
    {
    }

    public partial class SegmentedToken : TokenWithCommonCls
    {
        public int SegmentedPaddingHorizontal { get; set; }

        public int SegmentedPaddingHorizontalSM { get; set; }

        public int SegmentedContainerPadding { get; set; }

        public string LabelColor { get; set; }

        public string LabelColorHover { get; set; }

        public string BgColor { get; set; }

        public string BgColorHover { get; set; }

        public string BgColorActive { get; set; }

        public string BgColorSelected { get; set; }

    }

    public partial class Segmented
    {
        public CSSObject GetItemDisabledStyle(string cls, SegmentedToken token)
        {
            return new CSSObject()
            {
                [$"{cls}, {cls}:hover, {cls}:focus"] = new CSSObject()
                {
                    Color = token.ColorTextDisabled,
                    Cursor = "not-allowed",
                },
            };
        }

        public CSSObject GetItemSelectedStyle(SegmentedToken token)
        {
            return new CSSObject()
            {
                BackgroundColor = token.BgColorSelected,
                BoxShadow = token.BoxShadowTertiary,
            };
        }

        public Unknown_1 GenSegmentedStyle(SegmentedToken token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_3()
            {
                [componentCls] = new Unknown_4()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    Padding = token.SegmentedContainerPadding,
                    Color = token.LabelColor,
                    BackgroundColor = token.BgColor,
                    BorderRadius = token.BorderRadius,
                    Transition = @$"all {token.MotionDurationMid} {token.MotionEaseInOut}",
                    [$"{componentCls}-group"] = new Unknown_5()
                    {
                        Position = "relative",
                        Display = "flex",
                        AlignItems = "stretch",
                        JustifyItems = "flex-start",
                        Width = "100%",
                    },
                    [$"&{componentCls}-rtl"] = new Unknown_6()
                    {
                        Direction = "rtl",
                    },
                    [$"&{componentCls}-block"] = new Unknown_7()
                    {
                        Display = "flex",
                    },
                    [$"&{componentCls}-block {componentCls}-item"] = new Unknown_8()
                    {
                        Flex = 1,
                        MinWidth = 0,
                    },
                    [$"{componentCls}-item"] = new Unknown_9()
                    {
                        Position = "relative",
                        TextAlign = "center",
                        Cursor = "pointer",
                        Transition = @$"color {token.MotionDurationMid} {token.MotionEaseInOut}",
                        BorderRadius = token.BorderRadiusSM,
                        ["&-selected"] = new Unknown_10()
                        {
                            ["..."] = GetItemSelectedStyle(token),
                            Color = token.LabelColorHover,
                        },
                        ["&::after"] = new Unknown_11()
                        {
                            Content = "\"\"",
                            Position = "absolute",
                            Width = "100%",
                            Height = "100%",
                            Top = 0,
                            InsetInlineStart = 0,
                            BorderRadius = "inherit",
                            Transition = @$"background-color {token.MotionDurationMid}",
                            PointerEvents = "none",
                        },
                        [$"&:hover:not({componentCls}-item-selected):not({componentCls}-item-disabled)"] = new Unknown_12()
                        {
                            Color = token.LabelColorHover,
                            ["&::after"] = new Unknown_13()
                            {
                                BackgroundColor = token.BgColorHover,
                            },
                        },
                        [$"&:active:not({componentCls}-item-selected):not({componentCls}-item-disabled)"] = new Unknown_14()
                        {
                            Color = token.LabelColorHover,
                            ["&::after"] = new Unknown_15()
                            {
                                BackgroundColor = token.BgColorActive,
                            },
                        },
                        ["&-label"] = new Unknown_16()
                        {
                            MinHeight = token.ControlHeight - token.SegmentedContainerPadding * 2,
                            LineHeight = @$"{token.ControlHeight - token.SegmentedContainerPadding * 2}px",
                            Padding = @$"0 {token.SegmentedPaddingHorizontal}px",
                            ["..."] = segmentedTextEllipsisCss,
                        },
                        ["&-icon + *"] = new Unknown_17()
                        {
                            MarginInlineStart = token.MarginSM / 2,
                        },
                        ["&-input"] = new Unknown_18()
                        {
                            Position = "absolute",
                            InsetBlockStart = 0,
                            InsetInlineStart = 0,
                            Width = 0,
                            Height = 0,
                            Opacity = 0,
                            PointerEvents = "none",
                        },
                    },
                    [$"{componentCls}-thumb"] = new Unknown_19()
                    {
                        ["..."] = GetItemSelectedStyle(token),
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        Width = 0,
                        Height = "100%",
                        Padding = @$"{token.PaddingXXS}px 0",
                        BorderRadius = token.BorderRadiusSM,
                        [$"& ~ {componentCls}-item:not({componentCls}-item-selected):not({componentCls}-item-disabled)::after"] = new Unknown_20()
                        {
                            BackgroundColor = "transparent",
                        },
                    },
                    [$"&{componentCls}-lg"] = new Unknown_21()
                    {
                        BorderRadius = token.BorderRadiusLG,
                        [$"{componentCls}-item-label"] = new Unknown_22()
                        {
                            MinHeight = token.ControlHeightLG - token.SegmentedContainerPadding * 2,
                            LineHeight = @$"{token.ControlHeightLG - token.SegmentedContainerPadding * 2}px",
                            Padding = @$"0 {token.SegmentedPaddingHorizontal}px",
                            FontSize = token.FontSizeLG,
                        },
                        [$"{componentCls}-item, {componentCls}-thumb"] = new Unknown_23()
                        {
                            BorderRadius = token.BorderRadius,
                        },
                    },
                    [$"&{componentCls}-sm"] = new Unknown_24()
                    {
                        BorderRadius = token.BorderRadiusSM,
                        [$"{componentCls}-item-label"] = new Unknown_25()
                        {
                            MinHeight = token.ControlHeightSM - token.SegmentedContainerPadding * 2,
                            LineHeight = @$"{token.ControlHeightSM - token.SegmentedContainerPadding * 2}px",
                            Padding = @$"0 {token.SegmentedPaddingHorizontalSM}px",
                        },
                        [$"{componentCls}-item, {componentCls}-thumb"] = new Unknown_26()
                        {
                            BorderRadius = token.BorderRadiusXS,
                        },
                    },
                    ["..."] = GetItemDisabledStyle($"&-disabled {componentCls}-item", token),
                    ["..."] = GetItemDisabledStyle($"{componentCls}-item-disabled", token),
                    [$"{componentCls}-thumb-motion-appear-active"] = new Unknown_27()
                    {
                        Transition = @$"transform {token.MotionDurationSlow} {token.MotionEaseInOut}, width {token.MotionDurationSlow} {token.MotionEaseInOut}",
                        WillChange = "transform, width",
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_28 token)
        {
            var lineWidthBold = token.LineWidthBold;
            var lineWidth = token.LineWidth;
            var colorTextLabel = token.ColorTextLabel;
            var colorText = token.ColorText;
            var colorFillSecondary = token.ColorFillSecondary;
            var colorFill = token.ColorFill;
            var colorBgLayout = token.ColorBgLayout;
            var colorBgElevated = token.ColorBgElevated;
            var segmentedToken = MergeToken(
                token,
                new Unknown_29()
                {
                    SegmentedPaddingHorizontal = token.ControlPaddingHorizontal - lineWidth,
                    SegmentedPaddingHorizontalSM = token.ControlPaddingHorizontalSM - lineWidth,
                    SegmentedContainerPadding = lineWidthBold,
                    LabelColor = colorTextLabel,
                    LabelColorHover = colorText,
                    BgColor = colorBgLayout,
                    BgColorHover = colorFillSecondary,
                    BgColorActive = colorFill,
                    BgColorSelected = colorBgElevated,
                });
            return new Unknown_30 { GenSegmentedStyle(segmentedToken) };
        }

    }

}