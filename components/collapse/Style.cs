using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class CollapseToken : TokenWithCommonCls
    {
    }

    public partial class CollapseToken
    {
        public string CollapseContentBg { get; set; }

        public string CollapseHeaderBg { get; set; }

        public string CollapseHeaderPadding { get; set; }

        public string CollapseHeaderPaddingSM { get; set; }

        public string CollapseHeaderPaddingLG { get; set; }

        public int CollapsePanelBorderRadius { get; set; }

        public int CollapseContentPaddingHorizontal { get; set; }

    }

    public partial class Collapse
    {
        public CSSObject GenBaseStyle(CollapseToken token)
        {
            var componentCls = token.ComponentCls;
            var collapseContentBg = token.CollapseContentBg;
            var padding = token.Padding;
            var collapseContentPaddingHorizontal = token.CollapseContentPaddingHorizontal;
            var collapseHeaderBg = token.CollapseHeaderBg;
            var collapseHeaderPadding = token.CollapseHeaderPadding;
            var collapseHeaderPaddingSM = token.CollapseHeaderPaddingSM;
            var collapseHeaderPaddingLG = token.CollapseHeaderPaddingLG;
            var collapsePanelBorderRadius = token.CollapsePanelBorderRadius;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorBorder = token.ColorBorder;
            var colorText = token.ColorText;
            var colorTextHeading = token.ColorTextHeading;
            var colorTextDisabled = token.ColorTextDisabled;
            var fontSize = token.FontSize;
            var fontSizeLG = token.FontSizeLG;
            var lineHeight = token.LineHeight;
            var marginSM = token.MarginSM;
            var paddingSM = token.PaddingSM;
            var paddingLG = token.PaddingLG;
            var motionDurationSlow = token.MotionDurationSlow;
            var fontSizeIcon = token.FontSizeIcon;
            var borderBase = @$"{lineWidth}px {lineType} {colorBorder}";
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    BackgroundColor = collapseHeaderBg,
                    Border = borderBase,
                    BorderBottom = 0,
                    BorderRadius = @$"{collapsePanelBorderRadius}px",
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"& > {componentCls}-item"] = new CSSObject()
                    {
                        BorderBottom = borderBase,
                        ["&:last-child"] = new CSSObject()
                        {
                            [$"&,&>{componentCls}-header"] = new CSSObject()
                            {
                                BorderRadius = @$"0 0 {collapsePanelBorderRadius}px {collapsePanelBorderRadius}px",
                            },
                        },
                        [$"> {componentCls}-header"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "flex",
                            FlexWrap = "nowrap",
                            AlignItems = "flex-start",
                            Padding = collapseHeaderPadding,
                            Color = colorTextHeading,
                            LineHeight = lineHeight,
                            Cursor = "pointer",
                            Transition = @$"all {motionDurationSlow}, visibility 0s",
                            [$"> {componentCls}-header-text"] = new CSSObject()
                            {
                                Flex = "auto",
                            },
                            ["&:focus"] = new CSSObject()
                            {
                                Outline = "none",
                            },
                            [$"{componentCls}-expand-icon"] = new CSSObject()
                            {
                                Height = fontSize * lineHeight,
                                Display = "flex",
                                AlignItems = "center",
                                PaddingInlineEnd = marginSM,
                            },
                            [$"{componentCls}-arrow"] = new CSSObject()
                            {
                                ["..."] = ResetIcon(),
                                FontSize = fontSizeIcon,
                                ["svg"] = new CSSObject()
                                {
                                    Transition = @$"transform {motionDurationSlow}",
                                },
                            },
                            [$"{componentCls}-header-text"] = new CSSObject()
                            {
                                MarginInlineEnd = "auto",
                            },
                        },
                        [$"{componentCls}-header-collapsible-only"] = new CSSObject()
                        {
                            Cursor = "default",
                            [$"{componentCls}-header-text"] = new CSSObject()
                            {
                                Flex = "none",
                                Cursor = "pointer",
                            },
                        },
                        [$"{componentCls}-icon-collapsible-only"] = new CSSObject()
                        {
                            Cursor = "default",
                            [$"{componentCls}-expand-icon"] = new CSSObject()
                            {
                                Cursor = "pointer",
                            },
                        },
                        [$"&{componentCls}-no-arrow"] = new CSSObject()
                        {
                            [$"> {componentCls}-header"] = new CSSObject()
                            {
                                PaddingInlineStart = paddingSM,
                            },
                        },
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Color = colorText,
                        BackgroundColor = collapseContentBg,
                        BorderTop = borderBase,
                        [$"& > {componentCls}-content-box"] = new CSSObject()
                        {
                            Padding = @$"{padding}px {collapseContentPaddingHorizontal}px",
                        },
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                    ["&-small"] = new CSSObject()
                    {
                        [$"> {componentCls}-item"] = new CSSObject()
                        {
                            [$"> {componentCls}-header"] = new CSSObject()
                            {
                                Padding = collapseHeaderPaddingSM,
                            },
                            [$"> {componentCls}-content > {componentCls}-content-box"] = new CSSObject()
                            {
                                Padding = paddingSM,
                            },
                        },
                    },
                    ["&-large"] = new CSSObject()
                    {
                        [$"> {componentCls}-item"] = new CSSObject()
                        {
                            FontSize = fontSizeLG,
                            [$"> {componentCls}-header"] = new CSSObject()
                            {
                                Padding = collapseHeaderPaddingLG,
                                [$"> {componentCls}-expand-icon"] = new CSSObject()
                                {
                                    Height = fontSizeLG * lineHeight,
                                },
                            },
                            [$"> {componentCls}-content > {componentCls}-content-box"] = new CSSObject()
                            {
                                Padding = paddingLG,
                            },
                        },
                    },
                    [$"{componentCls}-item:last-child"] = new CSSObject()
                    {
                        [$"> {componentCls}-content"] = new CSSObject()
                        {
                            BorderRadius = @$"0 0 {collapsePanelBorderRadius}px {collapsePanelBorderRadius}px",
                        },
                    },
                    [$"& {componentCls}-item-disabled > {componentCls}-header"] = new CSSObject()
                    {
                        ["&,&>.arrow"] = new CSSObject()
                        {
                            Color = colorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                    [$"&{componentCls}-icon-position-end"] = new CSSObject()
                    {
                        [$"& > {componentCls}-item"] = new CSSObject()
                        {
                            [$"> {componentCls}-header"] = new CSSObject()
                            {
                                [$"{componentCls}-expand-icon"] = new CSSObject()
                                {
                                    Order = 1,
                                    PaddingInlineEnd = 0,
                                    PaddingInlineStart = marginSM,
                                },
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenArrowStyle(CollapseToken token)
        {
            var componentCls = token.ComponentCls;
            var fixedSelector = @$"> {componentCls}-item > {componentCls}-header {componentCls}-arrow svg";
            return new CSSObject()
            {
                [$"{componentCls}-rtl"] = new CSSObject()
                {
                    [fixedSelector] = new CSSObject()
                    {
                        Transform = @$"rotate(180deg)",
                    },
                },
            };
        }

        public CSSObject GenBorderlessStyle(CollapseToken token)
        {
            var componentCls = token.ComponentCls;
            var collapseHeaderBg = token.CollapseHeaderBg;
            var paddingXXS = token.PaddingXXS;
            var colorBorder = token.ColorBorder;
            return new CSSObject()
            {
                [$"{componentCls}-borderless"] = new CSSObject()
                {
                    BackgroundColor = collapseHeaderBg,
                    Border = 0,
                    [$"> {componentCls}-item"] = new CSSObject()
                    {
                        BorderBottom = @$"1px solid {colorBorder}",
                    },
                    [$">{componentCls}-item:last-child,>{componentCls}-item:last-child{componentCls}-header"] = new CSSObject()
                    {
                        BorderRadius = 0,
                    },
                    [$"> {componentCls}-item:last-child"] = new CSSObject()
                    {
                        BorderBottom = 0,
                    },
                    [$"> {componentCls}-item > {componentCls}-content"] = new CSSObject()
                    {
                        BackgroundColor = "transparent",
                        BorderTop = 0,
                    },
                    [$"> {componentCls}-item > {componentCls}-content > {componentCls}-content-box"] = new CSSObject()
                    {
                        PaddingTop = paddingXXS,
                    },
                },
            };
        }

        public CSSObject GenGhostStyle(CollapseToken token)
        {
            var componentCls = token.ComponentCls;
            var paddingSM = token.PaddingSM;
            return new CSSObject()
            {
                [$"{componentCls}-ghost"] = new CSSObject()
                {
                    BackgroundColor = "transparent",
                    Border = 0,
                    [$"> {componentCls}-item"] = new CSSObject()
                    {
                        BorderBottom = 0,
                        [$"> {componentCls}-content"] = new CSSObject()
                        {
                            BackgroundColor = "transparent",
                            Border = 0,
                            [$"> {componentCls}-content-box"] = new CSSObject()
                            {
                                PaddingBlock = paddingSM,
                            },
                        },
                    },
                },
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var collapseToken = MergeToken(
                token,
                new CollapseToken()
                {
                    CollapseContentBg = token.ColorBgContainer,
                    CollapseHeaderBg = token.ColorFillAlter,
                    CollapseHeaderPadding = @$"{token.PaddingSM}px {token.Padding}px",
                    CollapseHeaderPaddingSM = @$"{token.PaddingXS}px {token.PaddingSM}px",
                    CollapseHeaderPaddingLG = @$"{token.Padding}px {token.PaddingLG}px",
                    CollapsePanelBorderRadius = token.BorderRadiusLG,
                    CollapseContentPaddingHorizontal = 16,
                });
            return new CSSInterpolation[]
            {
                GenBaseStyle(collapseToken),
                GenBorderlessStyle(collapseToken),
                GenGhostStyle(collapseToken),
                GenArrowStyle(collapseToken),
                GenCollapseMotion(collapseToken)
            };
        }

    }

}