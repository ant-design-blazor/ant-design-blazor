using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class DescriptionsToken : TokenWithCommonCls
    {
        public int DescriptionsTitleMarginBottom { get; set; }

        public string DescriptionsExtraColor { get; set; }

        public int DescriptionItemPaddingBottom { get; set; }

        public string DescriptionsDefaultPadding { get; set; }

        public string DescriptionsBg { get; set; }

        public string DescriptionsMiddlePadding { get; set; }

        public string DescriptionsSmallPadding { get; set; }

        public int DescriptionsItemLabelColonMarginRight { get; set; }

        public int DescriptionsItemLabelColonMarginLeft { get; set; }

    }

    public partial class Descriptions
    {
        public CSSObject GenBorderedStyle(DescriptionsToken token)
        {
            var componentCls = token.ComponentCls;
            var descriptionsSmallPadding = token.DescriptionsSmallPadding;
            var descriptionsDefaultPadding = token.DescriptionsDefaultPadding;
            var descriptionsMiddlePadding = token.DescriptionsMiddlePadding;
            var descriptionsBg = token.DescriptionsBg;
            return new CSSObject()
            {
                [$"&{componentCls}-bordered"] = new CSSObject()
                {
                    [$"{componentCls}-view"] = new CSSObject()
                    {
                        Border = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                        ["> table"] = new CSSObject()
                        {
                            TableLayout = "auto",
                            BorderCollapse = "collapse",
                        },
                    },
                    [$"{componentCls}-item-label, {componentCls}-item-content"] = new CSSObject()
                    {
                        Padding = descriptionsDefaultPadding,
                        BorderInlineEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                        ["&:last-child"] = new CSSObject()
                        {
                            BorderInlineEnd = "none",
                        },
                    },
                    [$"{componentCls}-item-label"] = new CSSObject()
                    {
                        Color = token.ColorTextSecondary,
                        BackgroundColor = descriptionsBg,
                        ["&::after"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                    [$"{componentCls}-row"] = new CSSObject()
                    {
                        BorderBottom = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                        ["&:last-child"] = new CSSObject()
                        {
                            BorderBottom = "none",
                        },
                    },
                    [$"&{componentCls}-middle"] = new CSSObject()
                    {
                        [$"{componentCls}-item-label, {componentCls}-item-content"] = new CSSObject()
                        {
                            Padding = descriptionsMiddlePadding,
                        },
                    },
                    [$"&{componentCls}-small"] = new CSSObject()
                    {
                        [$"{componentCls}-item-label, {componentCls}-item-content"] = new CSSObject()
                        {
                            Padding = descriptionsSmallPadding,
                        },
                    },
                },
            };
        }

        public CSSObject GenDescriptionStyles(DescriptionsToken token)
        {
            var componentCls = token.ComponentCls;
            var descriptionsExtraColor = token.DescriptionsExtraColor;
            var descriptionItemPaddingBottom = token.DescriptionItemPaddingBottom;
            var descriptionsItemLabelColonMarginRight = token.DescriptionsItemLabelColonMarginRight;
            var descriptionsItemLabelColonMarginLeft = token.DescriptionsItemLabelColonMarginLeft;
            var descriptionsTitleMarginBottom = token.DescriptionsTitleMarginBottom;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenBorderedStyle(token),
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"{componentCls}-header"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignItems = "center",
                        MarginBottom = descriptionsTitleMarginBottom,
                    },
                    [$"{componentCls}-title"] = new CSSObject()
                    {
                        ["..."] = TextEllipsis,
                        Flex = "auto",
                        Color = token.ColorText,
                        FontWeight = token.FontWeightStrong,
                        FontSize = token.FontSizeLG,
                        LineHeight = token.LineHeightLG,
                    },
                    [$"{componentCls}-extra"] = new CSSObject()
                    {
                        MarginInlineStart = "auto",
                        Color = descriptionsExtraColor,
                        FontSize = token.FontSize,
                    },
                    [$"{componentCls}-view"] = new CSSObject()
                    {
                        Width = "100%",
                        BorderRadius = token.BorderRadiusLG,
                        ["table"] = new CSSObject()
                        {
                            Width = "100%",
                            TableLayout = "fixed",
                        },
                    },
                    [$"{componentCls}-row"] = new CSSObject()
                    {
                        ["> th, > td"] = new CSSObject()
                        {
                            PaddingBottom = descriptionItemPaddingBottom,
                        },
                        ["&:last-child"] = new CSSObject()
                        {
                            BorderBottom = "none",
                        },
                    },
                    [$"{componentCls}-item-label"] = new CSSObject()
                    {
                        Color = token.ColorTextTertiary,
                        FontWeight = "normal",
                        FontSize = token.FontSize,
                        LineHeight = token.LineHeight,
                        TextAlign = @$"start",
                        ["&::after"] = new CSSObject()
                        {
                            Content = "\":\"",
                            Position = "relative",
                            Top = -0.5f,
                            MarginInline = @$"{descriptionsItemLabelColonMarginLeft}px {descriptionsItemLabelColonMarginRight}px",
                        },
                        [$"&{componentCls}-item-no-colon::after"] = new CSSObject()
                        {
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-item-no-label"] = new CSSObject()
                    {
                        ["&::after"] = new CSSObject()
                        {
                            Margin = 0,
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-item-content"] = new CSSObject()
                    {
                        Display = "table-cell",
                        Flex = 1,
                        Color = token.ColorText,
                        FontSize = token.FontSize,
                        LineHeight = token.LineHeight,
                        WordBreak = "break-word",
                        OverflowWrap = "break-word",
                    },
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        PaddingBottom = 0,
                        VerticalAlign = "top",
                        ["&-container"] = new CSSObject()
                        {
                            Display = "flex",
                            [$"{componentCls}-item-label"] = new CSSObject()
                            {
                                Display = "inline-flex",
                                AlignItems = "baseline",
                            },
                            [$"{componentCls}-item-content"] = new CSSObject()
                            {
                                Display = "inline-flex",
                                AlignItems = "baseline",
                            },
                        },
                    },
                    ["&-middle"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                PaddingBottom = token.PaddingSM,
                            },
                        },
                    },
                    ["&-small"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                PaddingBottom = token.PaddingXS,
                            },
                        },
                    },
                },
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var descriptionsBg = token.ColorFillAlter;
            var descriptionsTitleMarginBottom = token.FontSizeSM * token.LineHeightSM;
            var descriptionsExtraColor = token.ColorText;
            var descriptionsSmallPadding = @$"{token.PaddingXS}px {token.Padding}px";
            var descriptionsDefaultPadding = @$"{token.Padding}px {token.PaddingLG}px";
            var descriptionsMiddlePadding = @$"{token.PaddingSM}px {token.PaddingLG}px";
            var descriptionItemPaddingBottom = token.Padding;
            var descriptionsItemLabelColonMarginRight = token.MarginXS;
            var descriptionsItemLabelColonMarginLeft = token.MarginXXS / 2;
            var descriptionToken = MergeToken(
                token,
                new DescriptionsToken()
                {
                    DescriptionsBg = descriptionsBg,
                    DescriptionsTitleMarginBottom = descriptionsTitleMarginBottom,
                    DescriptionsExtraColor = descriptionsExtraColor,
                    DescriptionItemPaddingBottom = descriptionItemPaddingBottom,
                    DescriptionsSmallPadding = descriptionsSmallPadding,
                    DescriptionsDefaultPadding = descriptionsDefaultPadding,
                    DescriptionsMiddlePadding = descriptionsMiddlePadding,
                    DescriptionsItemLabelColonMarginRight = descriptionsItemLabelColonMarginRight,
                    DescriptionsItemLabelColonMarginLeft = descriptionsItemLabelColonMarginLeft,
                });
            return new CSSInterpolation[] { GenDescriptionStyles(descriptionToken) };
        }

    }

}