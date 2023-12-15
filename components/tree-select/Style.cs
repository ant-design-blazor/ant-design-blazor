using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.TreeStyle;

namespace AntDesign
{
    public partial class TreeSelectToken
    {
    }

    public partial class TreeSelectToken : TokenWithCommonCls
    {
        public string TreePrefixCls { get; set; }

    }

    public partial class TreeSelect<TItem>
    {
        public CSSInterpolation[] GenBaseStyle(TreeSelectToken token)
        {
            var componentCls = token.ComponentCls;
            var treePrefixCls = token.TreePrefixCls;
            var colorBgElevated = token.ColorBgElevated;
            var treeCls = @$".{treePrefixCls}";
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-dropdown"] = new CSSInterpolation[]
                    {
                        new CSSObject()
                        {
                            Padding = @$"{token.PaddingXS}px {token.PaddingXS / 2}px",
                        },
                        GenTreeStyle(
                            treePrefixCls,
                            MergeToken(
                                token,
                                new TreeSharedToken()
                                {
                                    ColorBgContainer = colorBgElevated,
                                })),
                        new CSSObject()
                        {
                            [treeCls] = new CSSObject()
                            {
                                BorderRadius = 0,
                                [$"{treeCls}-list-holder-inner"] = new CSSObject()
                                {
                                    AlignItems = "stretch",
                                    [$"{treeCls}-treenode"] = new CSSObject()
                                    {
                                        [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                                        {
                                            Flex = "auto",
                                        },
                                    },
                                },
                            },
                        },
                        GetCheckboxStyle($"{treePrefixCls}-checkbox", token),
                        new CSSObject()
                        {
                            ["&-rtl"] = new CSSObject()
                            {
                                Direction = "rtl",
                                [$"{treeCls}-switcher{treeCls}-switcher_close"] = new CSSObject()
                                {
                                    [$"{treeCls}-switcher-icon svg"] = new CSSObject()
                                    {
                                        Transform = "rotate(90deg)",
                                    },
                                },
                            },
                        },
                    }
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "TreeSelect",
                (token) =>
                {
                    var treeSelectToken = MergeToken(
                        token,
                        new TreeSelectToken
                        {
                            TreePrefixCls = "select-tree"
                        });
                    return GenBaseStyle(treeSelectToken);
                }, 
                InitComponentToken);
        }

    }
}
