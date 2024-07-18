using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class FlexToken : TokenWithCommonCls
    {
    }

    public partial class FlexToken
    {
        public double FlexGapSM
        {
            get => (double)_tokens["flexGapSM"];
            set => _tokens["flexGapSM"] = value;
        }

        public double FlexGap
        {
            get => (double)_tokens["flexGap"];
            set => _tokens["flexGap"] = value;
        }

        public double FlexGapLG
        {
            get => (double)_tokens["flexGapLG"];
            set => _tokens["flexGapLG"] = value;
        }

    }

    public class FlexStyle
    {
        private static readonly string[] flexWrapValues = new string[] { "wrap", "nowrap", "wrap-reverse" };
        private static readonly string[] justifyContentValues = new string[]
        {
            "flex-start",
            "flex-end",
            "start",
            "end",
            "center",
            "space-between",
            "space-around",
            "space-evenly",
            "stretch",
            "normal",
            "left",
            "right",
        };
        private static readonly string[] alignItemsValues = new string[]
        {
            "center",
            "start",
            "end",
            "flex-start",
            "flex-end",
            "self-start",
            "self-end",
            "baseline",
            "normal",
            "stretch",
        };

        public static CSSObject GenFlexStyle(FlexToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Display = "flex",
                    ["&-vertical"] = new CSSObject()
                    {
                        FlexDirection = "column",
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    ["&:empty"] = new CSSObject()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public static CSSObject GenFlexGapStyle(FlexToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-gap-small"] = new CSSObject()
                    {
                        Gap = token.FlexGapSM,
                    },
                    ["&-gap-middle"] = new CSSObject()
                    {
                        Gap = token.FlexGap,
                    },
                    ["&-gap-large"] = new CSSObject()
                    {
                        Gap = token.FlexGapLG,
                    },
                },
            };
        }

        public static CSSObject GenFlexWrapStyle(FlexToken token)
        {
            var componentCls = token.ComponentCls;
            var wrapStyle = new CSSObject()
            {
            };
            flexWrapValues.ForEach((value) =>
            {
                wrapStyle[@$"{componentCls}-wrap-{value}"] = new CSSObject() { FlexWrap = value };
            });
            return wrapStyle;
        }

        public static CSSObject GenAlignItemsStyle(FlexToken token)
        {
            var componentCls = token.ComponentCls;
            var alignStyle = new CSSObject()
            {
            };
            alignItemsValues.ForEach((value) =>
            {
                alignStyle[@$"{componentCls}-align-{value}"] = new CSSObject() { AlignItems = value };
            });
            return alignStyle;
        }

        public static CSSObject GenJustifyContentStyle(FlexToken token)
        {
            var componentCls = token.ComponentCls;
            var justifyStyle = new CSSObject()
            {
            };
            justifyContentValues.ForEach((value) =>
            {
                justifyStyle[@$"{componentCls}-justify-{value}"] = new CSSObject() { JustifyContent = value };
            });
            return justifyStyle;
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Flex",
                (token) =>
                {
                    var flexToken = MergeToken(
                        token,
                        new FlexToken()
                        {
                            FlexGapSM = token.PaddingXS,
                            FlexGap = token.Padding,
                            FlexGapLG = token.PaddingLG,
                        });
                    return new CSSInterpolation[]
                    {
                        GenFlexStyle(flexToken),
                        GenFlexGapStyle(flexToken),
                        GenFlexWrapStyle(flexToken),
                        GenAlignItemsStyle(flexToken),
                        GenJustifyContentStyle(flexToken),
                    };
                });
        }
    }
}
