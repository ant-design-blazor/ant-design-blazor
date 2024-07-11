using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using System.Linq;

namespace AntDesign
{
    public partial class ListToken
    {
        public double ContentWidth
        {
            get => (double)_tokens["contentWidth"];
            set => _tokens["contentWidth"] = value;
        }

        public string ItemPaddingLG
        {
            get => (string)_tokens["itemPaddingLG"];
            set => _tokens["itemPaddingLG"] = value;
        }

        public string ItemPaddingSM
        {
            get => (string)_tokens["itemPaddingSM"];
            set => _tokens["itemPaddingSM"] = value;
        }

        public string ItemPadding
        {
            get => (string)_tokens["itemPadding"];
            set => _tokens["itemPadding"] = value;
        }

        public string HeaderBg
        {
            get => (string)_tokens["headerBg"];
            set => _tokens["headerBg"] = value;
        }

        public string FooterBg
        {
            get => (string)_tokens["footerBg"];
            set => _tokens["footerBg"] = value;
        }

        public double EmptyTextPadding
        {
            get => (double)_tokens["emptyTextPadding"];
            set => _tokens["emptyTextPadding"] = value;
        }

        public double MetaMarginBottom
        {
            get => (double)_tokens["metaMarginBottom"];
            set => _tokens["metaMarginBottom"] = value;
        }

        public double AvatarMarginRight
        {
            get => (double)_tokens["avatarMarginRight"];
            set => _tokens["avatarMarginRight"] = value;
        }

        public double TitleMarginBottom
        {
            get => (double)_tokens["titleMarginBottom"];
            set => _tokens["titleMarginBottom"] = value;
        }

        public double DescriptionFontSize
        {
            get => (double)_tokens["descriptionFontSize"];
            set => _tokens["descriptionFontSize"] = value;
        }

    }

    public partial class ListToken : TokenWithCommonCls
    {
        public string ListBorderedCls
        {
            get => (string)_tokens["listBorderedCls"];
            set => _tokens["listBorderedCls"] = value;
        }

        public double MinHeight
        {
            get => (double)_tokens["minHeight"];
            set => _tokens["minHeight"] = value;
        }

    }

    public partial class AntListStyle
    {
        public static CSSObject GenBorderedStyle(ListToken token)
        {
            var listBorderedCls = token.ListBorderedCls;
            var componentCls = token.ComponentCls;
            var paddingLG = token.PaddingLG;
            var margin = token.Margin;
            var itemPaddingSM = token.ItemPaddingSM;
            var itemPaddingLG = token.ItemPaddingLG;
            var marginLG = token.MarginLG;
            var borderRadiusLG = token.BorderRadiusLG;
            return new CSSObject()
            {
                [$"{listBorderedCls}"] = new CSSObject()
                {
                    Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                    BorderRadius = borderRadiusLG,
                    [$"{componentCls}-header,{componentCls}-footer,{componentCls}-item"] = new CSSObject()
                    {
                        PaddingInline = paddingLG,
                    },
                    [$"{componentCls}-pagination"] = new CSSObject()
                    {
                        Margin = @$"{margin}px {marginLG}px",
                    },
                },
                [$"{listBorderedCls}{componentCls}-sm"] = new CSSObject()
                {
                    [$"{componentCls}-item,{componentCls}-header,{componentCls}-footer"] = new CSSObject()
                    {
                        Padding = itemPaddingSM,
                    },
                },
                [$"{listBorderedCls}{componentCls}-lg"] = new CSSObject()
                {
                    [$"{componentCls}-item,{componentCls}-header,{componentCls}-footer"] = new CSSObject()
                    {
                        Padding = itemPaddingLG,
                    },
                },
            };
        }

        public static CSSObject GenResponsiveStyle(ListToken token)
        {
            var componentCls = token.ComponentCls;
            var screenSM = token.ScreenSM;
            var screenMD = token.ScreenMD;
            var marginLG = token.MarginLG;
            var marginSM = token.MarginSM;
            var margin = token.Margin;
            return new CSSObject()
            {
                [$"@media screen and (max-width:{screenMD})"] = new CSSObject()
                {
                    [$"{componentCls}"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            [$"{componentCls}-item-action"] = new CSSObject()
                            {
                                MarginInlineStart = marginLG,
                            },
                        },
                    },
                    [$"{componentCls}-vertical"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            [$"{componentCls}-item-extra"] = new CSSObject()
                            {
                                MarginInlineStart = marginLG,
                            },
                        },
                    },
                },
                [$"@media screen and (max-width: {screenSM})"] = new CSSObject()
                {
                    [$"{componentCls}"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            FlexWrap = "wrap",
                            [$"{componentCls}-action"] = new CSSObject()
                            {
                                MarginInlineStart = marginSM,
                            },
                        },
                    },
                    [$"{componentCls}-vertical"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            FlexWrap = "wrap-reverse",
                            [$"{componentCls}-item-main"] = new CSSObject()
                            {
                                MinWidth = token.ContentWidth,
                            },
                            [$"{componentCls}-item-extra"] = new CSSObject()
                            {
                                Margin = @$"auto auto {margin}px",
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenBaseStyle(ListToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var controlHeight = token.ControlHeight;
            var minHeight = token.MinHeight;
            var paddingSM = token.PaddingSM;
            var marginLG = token.MarginLG;
            var padding = token.Padding;
            var itemPadding = token.ItemPadding;
            var colorPrimary = token.ColorPrimary;
            var itemPaddingSM = token.ItemPaddingSM;
            var itemPaddingLG = token.ItemPaddingLG;
            var paddingXS = token.PaddingXS;
            var margin = token.Margin;
            var colorText = token.ColorText;
            var colorTextDescription = token.ColorTextDescription;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineWidth = token.LineWidth;
            var headerBg = token.HeaderBg;
            var footerBg = token.FooterBg;
            var emptyTextPadding = token.EmptyTextPadding;
            var metaMarginBottom = token.MetaMarginBottom;
            var avatarMarginRight = token.AvatarMarginRight;
            var titleMarginBottom = token.TitleMarginBottom;
            var descriptionFontSize = token.DescriptionFontSize;
            var alignCls = new string[] { "start", "center", "end" }.Aggregate(new CSSObject(), (css, item) =>
            {
                css[$"&-align-{item}"] = item;
                return css;
            });
            return new CSSObject()
            {
                [$"{componentCls}"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    ["*"] = new CSSObject()
                    {
                        Outline = "none",
                    },
                    [$"{componentCls}-header"] = new CSSObject()
                    {
                        Background = headerBg,
                    },
                    [$"{componentCls}-footer"] = new CSSObject()
                    {
                        Background = footerBg,
                    },
                    [$"{componentCls}-header, {componentCls}-footer"] = new CSSObject()
                    {
                        PaddingBlock = paddingSM,
                    },
                    [$"{componentCls}-pagination"] = new CSSObject()
                    {
                        MarginBlockStart = marginLG,
                        ["..."] = alignCls,
                        [$"{antCls}-pagination-options"] = new CSSObject()
                        {
                            TextAlign = "start",
                        },
                    },
                    [$"{componentCls}-spin"] = new CSSObject()
                    {
                        MinHeight = minHeight,
                        TextAlign = "center",
                    },
                    [$"{componentCls}-items"] = new CSSObject()
                    {
                        Margin = 0,
                        Padding = 0,
                        ListStyle = "none",
                    },
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignItems = "center",
                        JustifyContent = "space-between",
                        Padding = itemPadding,
                        Color = colorText,
                        [$"{componentCls}-item-meta"] = new CSSObject()
                        {
                            Display = "flex",
                            Flex = 1,
                            AlignItems = "flex-start",
                            MaxWidth = "100%",
                            [$"{componentCls}-item-meta-avatar"] = new CSSObject()
                            {
                                MarginInlineEnd = avatarMarginRight,
                            },
                            [$"{componentCls}-item-meta-content"] = new CSSObject()
                            {
                                Flex = "1 0",
                                Width = 0,
                                Color = colorText,
                            },
                            [$"{componentCls}-item-meta-title"] = new CSSObject()
                            {
                                Margin = @$"0 0 {token.MarginXXS}px 0",
                                Color = colorText,
                                FontSize = token.FontSize,
                                LineHeight = token.LineHeight,
                                ["> a"] = new CSSObject()
                                {
                                    Color = colorText,
                                    Transition = @$"all {motionDurationSlow}",
                                    ["&:hover"] = new CSSObject()
                                    {
                                        Color = colorPrimary,
                                    },
                                },
                            },
                            [$"{componentCls}-item-meta-description"] = new CSSObject()
                            {
                                Color = colorTextDescription,
                                FontSize = descriptionFontSize,
                                LineHeight = token.LineHeight,
                            },
                        },
                        [$"{componentCls}-item-action"] = new CSSObject()
                        {
                            Flex = "0 0 auto",
                            MarginInlineStart = token.MarginXXL,
                            Padding = 0,
                            FontSize = 0,
                            ListStyle = "none",
                            ["& > li"] = new CSSObject()
                            {
                                Position = "relative",
                                Display = "inline-block",
                                Padding = @$"0 {paddingXS}px",
                                Color = colorTextDescription,
                                FontSize = token.FontSize,
                                LineHeight = token.LineHeight,
                                TextAlign = "center",
                                ["&:first-child"] = new CSSObject()
                                {
                                    PaddingInlineStart = 0,
                                },
                            },
                            [$"{componentCls}-item-action-split"] = new CSSObject()
                            {
                                Position = "absolute",
                                InsetBlockStart = "50%",
                                InsetInlineEnd = 0,
                                Width = lineWidth,
                                Height = Math.Ceiling(token.FontSize * token.LineHeight) - token.MarginXXS * 2,
                                Transform = "translateY(-50%)",
                                BackgroundColor = token.ColorSplit,
                            },
                        },
                    },
                    [$"{componentCls}-empty"] = new CSSObject()
                    {
                        Padding = @$"{padding}px 0",
                        Color = colorTextDescription,
                        FontSize = token.FontSizeSM,
                        TextAlign = "center",
                    },
                    [$"{componentCls}-empty-text"] = new CSSObject()
                    {
                        Padding = emptyTextPadding,
                        Color = token.ColorTextDisabled,
                        FontSize = token.FontSize,
                        TextAlign = "center",
                    },
                    [$"{componentCls}-item-no-flex"] = new CSSObject()
                    {
                        Display = "block",
                    },
                },
                [$"{componentCls}-grid {antCls}-col > {componentCls}-item"] = new CSSObject()
                {
                    Display = "block",
                    MaxWidth = "100%",
                    MarginBlockEnd = margin,
                    PaddingBlock = 0,
                    BorderBlockEnd = "none",
                },
                [$"{componentCls}-vertical {componentCls}-item"] = new CSSObject()
                {
                    AlignItems = "initial",
                    [$"{componentCls}-item-main"] = new CSSObject()
                    {
                        Display = "block",
                        Flex = 1,
                    },
                    [$"{componentCls}-item-extra"] = new CSSObject()
                    {
                        MarginInlineStart = marginLG,
                    },
                    [$"{componentCls}-item-meta"] = new CSSObject()
                    {
                        MarginBlockEnd = metaMarginBottom,
                        [$"{componentCls}-item-meta-title"] = new CSSObject()
                        {
                            MarginBlockStart = 0,
                            MarginBlockEnd = titleMarginBottom,
                            Color = colorText,
                            FontSize = token.FontSizeLG,
                            LineHeight = token.LineHeightLG,
                        },
                    },
                    [$"{componentCls}-item-action"] = new CSSObject()
                    {
                        MarginBlockStart = padding,
                        MarginInlineStart = "auto",
                        ["> li"] = new CSSObject()
                        {
                            Padding = @$"0 {padding}px",
                            ["&:first-child"] = new CSSObject()
                            {
                                PaddingInlineStart = 0,
                            },
                        },
                    },
                },
                [$"{componentCls}-split {componentCls}-item"] = new CSSObject()
                {
                    BorderBlockEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                    ["&:last-child"] = new CSSObject()
                    {
                        BorderBlockEnd = "none",
                    },
                },
                [$"{componentCls}-split {componentCls}-header"] = new CSSObject()
                {
                    BorderBlockEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                },
                [$"{componentCls}-split{componentCls}-empty {componentCls}-footer"] = new CSSObject()
                {
                    BorderTop = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                },
                [$"{componentCls}-loading {componentCls}-spin-nested-loading"] = new CSSObject()
                {
                    MinHeight = controlHeight,
                },
                [$"{componentCls}-split{componentCls}-something-after-last-item {antCls}-spin-container > {componentCls}-items > {componentCls}-item:last-child"] = new CSSObject()
                {
                    BorderBlockEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                },
                [$"{componentCls}-lg {componentCls}-item"] = new CSSObject()
                {
                    Padding = itemPaddingLG,
                },
                [$"{componentCls}-sm {componentCls}-item"] = new CSSObject()
                {
                    Padding = itemPaddingSM,
                },
                [$"{componentCls}:not({componentCls}-vertical)"] = new CSSObject()
                {
                    [$"{componentCls}-item-no-flex"] = new CSSObject()
                    {
                        [$"{componentCls}-item-action"] = new CSSObject()
                        {
                            Float = "right",
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "List",
                (token) =>
                {
                    var listToken = MergeToken(
                        token,
                        new ListToken()
                        {
                            ListBorderedCls = @$"{token.ComponentCls}-bordered",
                            MinHeight = token.ControlHeightLG,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(listToken),
                        GenBorderedStyle(listToken),
                        GenResponsiveStyle(listToken),
                    };
                },
                (token) =>
                {
                    return new ListToken()
                    {
                        ContentWidth = 220,
                        ItemPadding = @$"{token.PaddingContentVertical}px 0",
                        ItemPaddingSM = @$"{token.PaddingContentVerticalSM}px {token.PaddingContentHorizontal}px",
                        ItemPaddingLG = @$"{token.PaddingContentVerticalLG}px {token.PaddingContentHorizontalLG}px",
                        HeaderBg = "transparent",
                        FooterBg = "transparent",
                        EmptyTextPadding = token.Padding,
                        MetaMarginBottom = token.Padding,
                        AvatarMarginRight = token.Padding,
                        TitleMarginBottom = token.PaddingSM,
                        DescriptionFontSize = token.FontSize,
                    };
                });
        }

    }

}
