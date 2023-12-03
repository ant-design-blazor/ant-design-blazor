using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AnchorToken : TokenWithCommonCls
    {
        public int LinkPaddingBlock { get; set; }

        public int LinkPaddingInlineStart { get; set; }

    }

    public partial class AnchorToken
    {
        public int HolderOffsetBlock { get; set; }

        public int AnchorPaddingBlockSecondary { get; set; }

        public int AnchorBallSize { get; set; }

        public int AnchorTitleBlock { get; set; }

    }

    public partial class Anchor
    {
        private const string PrefixCls = "ant-anchor";
        private TokenWithCommonCls _token = new TokenWithCommonCls { PrefixCls = PrefixCls, ComponentCls = $".{PrefixCls}" };

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
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "relative",
                        PaddingInlineStart = lineWidthBold,
                        [$"{componentCls}-link"] = new CSSObject()
                        {
                            PaddingBlock = token.LinkPaddingBlock,
                            PaddingInline = @$"{token.LinkPaddingInlineStart}px 0",
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
                                InsetInlineStart = 0,
                                Top = 0,
                                Height = "100%",
                                BorderInlineStart = @$"{lineWidthBold}px {lineType} {colorSplit}",
                                Content = "\" \"",
                            },
                            [$"{componentCls}-ink"] = new CSSObject()
                            {
                                Position = "absolute",
                                InsetInlineStart = 0,
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

        public RenderFragment UseStyle(TokenWithCommonCls token)
        {
            return GenComponentStyleHook("Anchor", token, () =>
            {

                var fontSize = token.FontSize;
                var fontSizeLG = token.FontSizeLG;
                var paddingXXS = token.PaddingXXS;
                var anchorToken = MergeToken(
                    token,
                    new AnchorToken()
                    {
                        HolderOffsetBlock = paddingXXS,
                        AnchorPaddingBlockSecondary = paddingXXS / 2,
                        AnchorTitleBlock = (fontSize / 14) * 3,
                        AnchorBallSize = fontSizeLG / 2,
                        LinkPaddingBlock = token.PaddingXXS,
                        LinkPaddingInlineStart = token.Padding,
                    });
                return new CSSInterpolation[]
                {
                    GenSharedAnchorStyle(anchorToken),
                    GenSharedAnchorHorizontalStyle(anchorToken)
                };
            });
        }
    }

}
