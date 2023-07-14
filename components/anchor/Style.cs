using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class AnchorToken : TokenWithCommonCls
    {
    }

    public partial class AnchorToken
    {
        public int HolderOffsetBlock { get; set; }

        public int AnchorPaddingBlock { get; set; }

        public int AnchorPaddingBlockSecondary { get; set; }

        public int AnchorPaddingInline { get; set; }

        public int AnchorBallSize { get; set; }

        public int AnchorTitleBlock { get; set; }

    }

    public partial class Anchor
    {
        public CSSObject GenSharedAnchorStyle(AnchorToken token)
        {
            var componentCls = token.ComponentCls;
            var holderOffsetBlock = token.HolderOffsetBlock;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineWidthBold = token.LineWidthBold;
            var colorPrimary = token.ColorPrimary;
            var lineType = token.LineType;
            var colorSplit = token.ColorSplit;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    MarginBlockStart = -holderOffsetBlock,
                    PaddingBlockStart = holderOffsetBlock,
                    BackgroundColor = "transparent",
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "relative",
                        PaddingInlineStart = lineWidthBold,
                        [$"{componentCls}-link"] = new CSSObject()
                        {
                            PaddingBlock = token.AnchorPaddingBlock,
                            PaddingInline = @$"{token.AnchorPaddingInline}px 0",
                            ["&-title"] = new CSSObject()
                            {
                                ["..."] = TextEllipsis,
                                Position = "relative",
                                Display = "block",
                                MarginBlockEnd = token.AnchorTitleBlock,
                                Color = token.ColorText,
                                Transition = @$"all {token.MotionDurationSlow}",
                                ["&:only-child"] = new CSSObject()
                                {
                                    MarginBlockEnd = 0,
                                },
                            },
                            [$"&-active > {componentCls}-link-title"] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                            },
                            [$"{componentCls}-link"] = new CSSObject()
                            {
                                PaddingBlock = token.AnchorPaddingBlockSecondary,
                            },
                        },
                    },
                    [$"&:not({componentCls}-wrapper-horizontal)"] = new CSSObject()
                    {
                        [componentCls] = new CSSObject()
                        {
                            ["&::before"] = new CSSObject()
                            {
                                Position = "absolute",
                                Left = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                Top = 0,
                                Height = "100%",
                                BorderInlineStart = @$"{lineWidthBold}px {lineType} {colorSplit}",
                                Content = "\" \"",
                            },
                            [$"{componentCls}-ink"] = new CSSObject()
                            {
                                Position = "absolute",
                                Left = new PropertySkip()
                                {
                                    SkipCheck = true,
                                    Value = 0,
                                },
                                Display = "none",
                                Transform = "translateY(-50%)",
                                Transition = @$"top {motionDurationSlow} ease-in-out",
                                Width = lineWidthBold,
                                BackgroundColor = colorPrimary,
                                [$"&{componentCls}-ink-visible"] = new CSSObject()
                                {
                                    Display = "inline-block",
                                },
                            },
                        },
                    },
                    [$"{componentCls}-fixed {componentCls}-ink {componentCls}-ink"] = new CSSObject()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public CSSObject GenSharedAnchorHorizontalStyle(AnchorToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineWidthBold = token.LineWidthBold;
            var colorPrimary = token.ColorPrimary;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper-horizontal"] = new CSSObject()
                {
                    Position = "relative",
                    ["&::before"] = new CSSObject()
                    {
                        Position = "absolute",
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        Right = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        Bottom = 0,
                        BorderBottom = @$"1px {token.LineType} {token.ColorSplit}",
                        Content = "\" \"",
                    },
                    [componentCls] = new CSSObject()
                    {
                        OverflowX = "scroll",
                        Position = "relative",
                        Display = "flex",
                        ScrollbarWidth = "none",
                        ["&::-webkit-scrollbar"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"{componentCls}-link:first-of-type"] = new CSSObject()
                        {
                            PaddingInline = 0,
                        },
                        [$"{componentCls}-ink"] = new CSSObject()
                        {
                            Position = "absolute",
                            Bottom = 0,
                            Transition = @$"left {motionDurationSlow} ease-in-out, width {motionDurationSlow} ease-in-out",
                            Height = lineWidthBold,
                            BackgroundColor = colorPrimary,
                        },
                    },
                },
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var fontSize = token.FontSize;
            var fontSizeLG = token.FontSizeLG;
            var padding = token.Padding;
            var paddingXXS = token.PaddingXXS;
            var anchorToken = MergeToken(
                token,
                new AnchorToken()
                {
                    HolderOffsetBlock = paddingXXS,
                    AnchorPaddingBlock = paddingXXS,
                    AnchorPaddingBlockSecondary = paddingXXS / 2,
                    AnchorPaddingInline = padding,
                    AnchorTitleBlock = (fontSize / 14) * 3,
                    AnchorBallSize = fontSizeLG / 2,
                });
            return new CSSInterpolation[]
            {
                GenSharedAnchorStyle(anchorToken),
                GenSharedAnchorHorizontalStyle(anchorToken)
            };
        }

    }

}