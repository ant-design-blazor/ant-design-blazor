using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.CollapseMotion;

namespace AntDesign
{
    public partial class CollapseToken : TokenWithCommonCls
    {
        public string HeaderPadding
        {
            get => (string)_tokens["headerPadding"];
            set => _tokens["headerPadding"] = value;
        }

        public string HeaderBg
        {
            get => (string)_tokens["headerBg"];
            set => _tokens["headerBg"] = value;
        }

        public string ContentPadding
        {
            get => (string)_tokens["contentPadding"];
            set => _tokens["contentPadding"] = value;
        }

        public string ContentBg
        {
            get => (string)_tokens["contentBg"];
            set => _tokens["contentBg"] = value;
        }

    }

    public partial class CollapseToken
    {
        public string CollapseHeaderPaddingSM
        {
            get => (string)_tokens["collapseHeaderPaddingSM"];
            set => _tokens["collapseHeaderPaddingSM"] = value;
        }

        public string CollapseHeaderPaddingLG
        {
            get => (string)_tokens["collapseHeaderPaddingLG"];
            set => _tokens["collapseHeaderPaddingLG"] = value;
        }

        public double CollapsePanelBorderRadius
        {
            get => (double)_tokens["collapsePanelBorderRadius"];
            set => _tokens["collapsePanelBorderRadius"] = value;
        }

    }

    public partial class CollapseStyle
    {
        public static CSSObject GenBaseStyle(CollapseToken token)
        {
            var componentCls = token.ComponentCls;
            var contentBg = token.ContentBg;
            var padding = token.Padding;
            var headerBg = token.HeaderBg;
            var headerPadding = token.HeaderPadding;
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
            var paddingXS = token.PaddingXS;
            var motionDurationSlow = token.MotionDurationSlow;
            var fontSizeIcon = token.FontSizeIcon;
            var contentPadding = token.ContentPadding;
            var borderBase = @$"{lineWidth}px {lineType} {colorBorder}";
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    BackgroundColor = headerBg,
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
                            Padding = headerPadding,
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
                        [$"{componentCls}-icon-collapsible-only"] = new CSSObject()
                        {
                            Cursor = "unset",
                            [$"{componentCls}-expand-icon"] = new CSSObject()
                            {
                                Cursor = "pointer",
                            },
                        },
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Color = colorText,
                        BackgroundColor = contentBg,
                        BorderTop = borderBase,
                        [$"& > {componentCls}-content-box"] = new CSSObject()
                        {
                            Padding = contentPadding,
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
                                PaddingInlineStart = paddingXS,
                                [$"> {componentCls}-expand-icon"] = new CSSObject()
                                {
                                    MarginInlineStart = paddingSM - paddingXS,
                                },
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
                                PaddingInlineStart = padding,
                                [$"> {componentCls}-expand-icon"] = new CSSObject()
                                {
                                    Height = fontSizeLG * lineHeight,
                                    MarginInlineStart = paddingLG - padding,
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

        public static CSSObject GenArrowStyle(CollapseToken token)
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

        public static CSSObject GenBorderlessStyle(CollapseToken token)
        {
            var componentCls = token.ComponentCls;
            var headerBg = token.HeaderBg;
            var paddingXXS = token.PaddingXXS;
            var colorBorder = token.ColorBorder;
            return new CSSObject()
            {
                [$"{componentCls}-borderless"] = new CSSObject()
                {
                    BackgroundColor = headerBg,
                    Border = 0,
                    [$"> {componentCls}-item"] = new CSSObject()
                    {
                        BorderBottom = @$"1px solid {colorBorder}",
                    },
                    [$"> {componentCls}-item:last-child,> {componentCls}-item:last-child {componentCls}-header"] = new CSSObject()
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

        public static CSSObject GenGhostStyle(CollapseToken token)
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

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Collapse",
                (token) =>
                {
                    var collapseToken = MergeToken(
                        token,
                        new CollapseToken()
                        {
                            CollapseHeaderPaddingSM = @$"{token.PaddingXS}px {token.PaddingSM}px",
                            CollapseHeaderPaddingLG = @$"{token.Padding}px {token.PaddingLG}px",
                            CollapsePanelBorderRadius = token.BorderRadiusLG,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(collapseToken),
                        GenBorderlessStyle(collapseToken),
                        GenGhostStyle(collapseToken),
                        GenArrowStyle(collapseToken),
                        GenCollapseMotion(collapseToken),
                    };
                },
                (token) =>
                {
                    return new CollapseToken()
                    {
                        HeaderPadding = @$"{token.PaddingSM}px {token.Padding}px",
                        HeaderBg = token.ColorFillAlter,
                        ContentPadding = @$"{token.Padding}px 16px",
                        ContentBg = token.ColorBgContainer,
                    };
                });
        }

    }

}
