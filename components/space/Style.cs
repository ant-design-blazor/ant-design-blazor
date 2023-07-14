using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SpaceToken
    {
    }

    public partial class SpaceToken : TokenWithCommonCls
    {
    }

    public partial class Space
    {
        public Unknown_1 GenSpaceStyle(Unknown_5 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_6()
            {
                [componentCls] = new Unknown_7()
                {
                    Display = "inline-flex",
                    ["&-rtl"] = new Unknown_8()
                    {
                        Direction = "rtl",
                    },
                    ["&-vertical"] = new Unknown_9()
                    {
                        FlexDirection = "column",
                    },
                    ["&-align"] = new Unknown_10()
                    {
                        FlexDirection = "column",
                        ["&-center"] = new Unknown_11()
                        {
                            AlignItems = "center",
                        },
                        ["&-start"] = new Unknown_12()
                        {
                            AlignItems = "flex-start",
                        },
                        ["&-end"] = new Unknown_13()
                        {
                            AlignItems = "flex-end",
                        },
                        ["&-baseline"] = new Unknown_14()
                        {
                            AlignItems = "baseline",
                        },
                    },
                    [$"{componentCls}-item:empty"] = new Unknown_15()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_16 token)
        {
            return new Unknown_17
            {
                GenSpaceStyle(token),
                GenSpaceCompactStyle(token)
            };
        }

        public Unknown_3 GenComponentStyleHook()
        {
            return new Unknown_18()
            {
            };
        }

        public Unknown_4 GenSpaceCompactStyle(Unknown_19 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_20()
            {
                [componentCls] = new Unknown_21()
                {
                    Display = "inline-flex",
                    ["&-block"] = new Unknown_22()
                    {
                        Display = "flex",
                        Width = "100%",
                    },
                    ["&-vertical"] = new Unknown_23()
                    {
                        FlexDirection = "column",
                    },
                },
            };
        }

    }

}