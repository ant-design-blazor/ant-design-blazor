using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class SegmentedToken
    {
        public string ItemColor
        {
            get => (string)_tokens["itemColor"];
            set => _tokens["itemColor"] = value;
        }

        public string ItemHoverColor
        {
            get => (string)_tokens["itemHoverColor"];
            set => _tokens["itemHoverColor"] = value;
        }

        public string ItemHoverBg
        {
            get => (string)_tokens["itemHoverBg"];
            set => _tokens["itemHoverBg"] = value;
        }

        public string ItemActiveBg
        {
            get => (string)_tokens["itemActiveBg"];
            set => _tokens["itemActiveBg"] = value;
        }

        public string ItemSelectedBg
        {
            get => (string)_tokens["itemSelectedBg"];
            set => _tokens["itemSelectedBg"] = value;
        }

        public string ItemSelectedColor
        {
            get => (string)_tokens["itemSelectedColor"];
            set => _tokens["itemSelectedColor"] = value;
        }

    }

    public partial class SegmentedToken : TokenWithCommonCls
    {
        public double SegmentedPadding
        {
            get => (double)_tokens["segmentedPadding"];
            set => _tokens["segmentedPadding"] = value;
        }

        public string SegmentedBgColor
        {
            get => (string)_tokens["segmentedBgColor"];
            set => _tokens["segmentedBgColor"] = value;
        }

        public double SegmentedPaddingHorizontal
        {
            get => (double)_tokens["segmentedPaddingHorizontal"];
            set => _tokens["segmentedPaddingHorizontal"] = value;
        }

        public double SegmentedPaddingHorizontalSM
        {
            get => (double)_tokens["segmentedPaddingHorizontalSM"];
            set => _tokens["segmentedPaddingHorizontalSM"] = value;
        }

    }

    public partial class SegmentedStyle
    {
        public static CSSObject GetItemDisabledStyle(string cls, SegmentedToken token)
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

        public static CSSObject GetItemSelectedStyle(SegmentedToken token)
        {
            return new CSSObject()
            {
                BackgroundColor = token.ItemSelectedBg,
                BoxShadow = token.BoxShadowTertiary,
            };
        }

        public static CSSObject GenSegmentedStyle(SegmentedToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "inline-block",
                    Padding = token.SegmentedPadding,
                    Color = token.ItemColor,
                    BackgroundColor = token.SegmentedBgColor,
                    BorderRadius = token.BorderRadius,
                    Transition = @$"all {token.MotionDurationMid} {token.MotionEaseInOut}",
                    [$"{componentCls}-group"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "flex",
                        AlignItems = "stretch",
                        JustifyItems = "flex-start",
                        Width = "100%",
                    },
                    [$"&{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"&{componentCls}-block"] = new CSSObject()
                    {
                        Display = "flex",
                    },
                    [$"&{componentCls}-block {componentCls}-item"] = new CSSObject()
                    {
                        Flex = 1,
                        MinWidth = 0,
                    },
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Position = "relative",
                        TextAlign = "center",
                        Cursor = "pointer",
                        Transition = @$"color {token.MotionDurationMid} {token.MotionEaseInOut}",
                        BorderRadius = token.BorderRadiusSM,
                        Transform = "translateZ(0)",
                        ["&-selected"] = new CSSObject()
                        {
                            ["..."] = GetItemSelectedStyle(token),
                            Color = token.ItemSelectedColor,
                        },
                        ["&::after"] = new CSSObject()
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
                        [$"&:hover:not({componentCls}-item-selected):not({componentCls}-item-disabled)"] = new CSSObject()
                        {
                            Color = token.ItemHoverColor,
                            ["&::after"] = new CSSObject()
                            {
                                BackgroundColor = token.ItemHoverBg,
                            },
                        },
                        [$"&:active:not({componentCls}-item-selected):not({componentCls}-item-disabled)"] = new CSSObject()
                        {
                            Color = token.ItemHoverColor,
                            ["&::after"] = new CSSObject()
                            {
                                BackgroundColor = token.ItemActiveBg,
                            },
                        },
                        ["&-label"] = new CSSObject()
                        {
                            MinHeight = token.ControlHeight - token.SegmentedPadding * 2,
                            LineHeight = @$"{token.ControlHeight - token.SegmentedPadding * 2}px",
                            Padding = @$"0 {token.SegmentedPaddingHorizontal}px",
                            ["..."] = SegmentedTextEllipsisCss,
                        },
                        ["&-icon + *"] = new CSSObject()
                        {
                            MarginInlineStart = token.MarginSM / 2,
                        },
                        ["&-input"] = new CSSObject()
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
                    [$"{componentCls}-thumb"] = new CSSObject()
                    {
                        ["..."] = GetItemSelectedStyle(token),
                        Position = "absolute",
                        InsetBlockStart = 0,
                        InsetInlineStart = 0,
                        Width = 0,
                        Height = "100%",
                        Padding = @$"{token.PaddingXXS}px 0",
                        BorderRadius = token.BorderRadiusSM,
                        [$"& ~ {componentCls}-item:not({componentCls}-item-selected):not({componentCls}-item-disabled)::after"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                        },
                    },
                    [$"&{componentCls}-lg"] = new CSSObject()
                    {
                        BorderRadius = token.BorderRadiusLG,
                        [$"{componentCls}-item-label"] = new CSSObject()
                        {
                            MinHeight = token.ControlHeightLG - token.SegmentedPadding * 2,
                            LineHeight = @$"{token.ControlHeightLG - token.SegmentedPadding * 2}px",
                            Padding = @$"0 {token.SegmentedPaddingHorizontal}px",
                            FontSize = token.FontSizeLG,
                        },
                        [$"{componentCls}-item, {componentCls}-thumb"] = new CSSObject()
                        {
                            BorderRadius = token.BorderRadius,
                        },
                    },
                    [$"&{componentCls}-sm"] = new CSSObject()
                    {
                        BorderRadius = token.BorderRadiusSM,
                        [$"{componentCls}-item-label"] = new CSSObject()
                        {
                            MinHeight = token.ControlHeightSM - token.SegmentedPadding * 2,
                            LineHeight = @$"{token.ControlHeightSM - token.SegmentedPadding * 2}px",
                            Padding = @$"0 {token.SegmentedPaddingHorizontalSM}px",
                        },
                        [$"{componentCls}-item, {componentCls}-thumb"] = new CSSObject()
                        {
                            BorderRadius = token.BorderRadiusXS,
                        },
                    },
                    ["..."] = GetItemDisabledStyle($"&-disabled {componentCls}-item", token),
                    ["..."] = GetItemDisabledStyle($"{componentCls}-item-disabled", token),
                    [$"{componentCls}-thumb-motion-appear-active"] = new CSSObject()
                    {
                        Transition = @$"transform {token.MotionDurationSlow} {token.MotionEaseInOut}, width {token.MotionDurationSlow} {token.MotionEaseInOut}",
                        WillChange = "transform, width",
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Segmented",
                (token) =>
                {
                    var lineWidth = token.LineWidth;
                    var lineWidthBold = token.LineWidthBold;
                    var colorBgLayout = token.ColorBgLayout;
                    var segmentedToken = MergeToken(
                        token,
                        new SegmentedToken()
                        {
                            SegmentedPadding = lineWidthBold,
                            SegmentedBgColor = colorBgLayout,
                            SegmentedPaddingHorizontal = token.ControlPaddingHorizontal - lineWidth,
                            SegmentedPaddingHorizontalSM = token.ControlPaddingHorizontalSM - lineWidth,
                        });
                    return new CSSInterpolation[]
                    {
                        GenSegmentedStyle(segmentedToken),
                    };
                },
                (token) =>
                {
                    var colorTextLabel = token.ColorTextLabel;
                    var colorText = token.ColorText;
                    var colorFillSecondary = token.ColorFillSecondary;
                    var colorBgElevated = token.ColorBgElevated;
                    var colorFill = token.ColorFill;
                    return new SegmentedToken()
                    {
                        ItemColor = colorTextLabel,
                        ItemHoverColor = colorText,
                        ItemHoverBg = colorFillSecondary,
                        ItemSelectedBg = colorBgElevated,
                        ItemActiveBg = colorFill,
                        ItemSelectedColor = colorText,
                    };
                });
        }

    }

}
