using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class SpaceToken
    {
    }

    public partial class SpaceToken : TokenWithCommonCls
    {
        public double SpaceGapSmallSize
        {
            get => (double)_tokens["spaceGapSmallSize"];
            set => _tokens["spaceGapSmallSize"] = value;
        }

        public double SpaceGapMiddleSize
        {
            get => (double)_tokens["spaceGapMiddleSize"];
            set => _tokens["spaceGapMiddleSize"] = value;
        }

        public double SpaceGapLargeSize
        {
            get => (double)_tokens["spaceGapLargeSize"];
            set => _tokens["spaceGapLargeSize"] = value;
        }

    }

    public partial class SpaceStyle
    {
        public static CSSObject GenSpaceStyle(SpaceToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Display = "inline-flex",
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    ["&-vertical"] = new CSSObject()
                    {
                        FlexDirection = "column",
                    },
                    ["&-align"] = new CSSObject()
                    {
                        FlexDirection = "column",
                        ["&-center"] = new CSSObject()
                        {
                            AlignItems = "center",
                        },
                        ["&-start"] = new CSSObject()
                        {
                            AlignItems = "flex-start",
                        },
                        ["&-end"] = new CSSObject()
                        {
                            AlignItems = "flex-end",
                        },
                        ["&-baseline"] = new CSSObject()
                        {
                            AlignItems = "baseline",
                        },
                    },
                    [$"{componentCls}-item:empty"] = new CSSObject()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public static CSSObject GenSpaceGapStyle(SpaceToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-gap-row-small"] = new CSSObject()
                    {
                        RowGap = token.SpaceGapSmallSize,
                    },
                    ["&-gap-row-middle"] = new CSSObject()
                    {
                        RowGap = token.SpaceGapMiddleSize,
                    },
                    ["&-gap-row-large"] = new CSSObject()
                    {
                        RowGap = token.SpaceGapLargeSize,
                    },
                    ["&-gap-col-small"] = new CSSObject()
                    {
                        ColumnGap = token.SpaceGapSmallSize,
                    },
                    ["&-gap-col-middle"] = new CSSObject()
                    {
                        ColumnGap = token.SpaceGapMiddleSize,
                    },
                    ["&-gap-col-large"] = new CSSObject()
                    {
                        ColumnGap = token.SpaceGapLargeSize,
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Space",
                (token) =>
                {
                    var spaceToken = MergeToken(
                        token,
                        new SpaceToken()
                        {
                            SpaceGapSmallSize = token.PaddingXS,
                            SpaceGapMiddleSize = token.Padding,
                            SpaceGapLargeSize = token.PaddingLG,
                        });
                    return new CSSInterpolation[]
                    {
                        GenSpaceStyle(spaceToken),
                        GenSpaceGapStyle(spaceToken),
                        GenSpaceCompactStyle(spaceToken),
                    };
                },
                (token) =>
                {
                    return new SpaceToken()
                    {
                    };
                },
                new GenOptions()
                {
                    ResetStyle = false,
                });
        }

        public static CSSObject GenSpaceCompactStyle(SpaceToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-block"] = new CSSObject()
                    {
                        Display = "flex",
                        Width = "100%",
                    },
                    ["&-vertical"] = new CSSObject()
                    {
                        FlexDirection = "column",
                    },
                },
            };
        }

    }

}
