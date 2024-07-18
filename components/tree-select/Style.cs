using System;
using CssInCSharp;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.TreeStyle;
using static AntDesign.CheckboxStyle;

namespace AntDesign
{
    public partial class TreeSelectToken
    {
    }

    public partial class TreeSelectToken : TokenWithCommonCls
    {
        public string TreePrefixCls
        {
            get => (string)_tokens["treePrefixCls"];
            set => _tokens["treePrefixCls"] = value;
        }

    }

    public class TreeSelectStyle
    {
        public static CSSInterpolation[] GenBaseStyle(TreeSelectToken token)
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

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "TreeSelect",
                (token) =>
                {
                    var treeSelectToken = MergeToken(
                        token,
                        new TreeSelectToken
                        {
                            TreePrefixCls = "ant-select-tree",
                            // 未赋值但后续有引用，这应该是react那边的Bug，导致生成的样式中包含undefined
                            ["directoryNodeSelectedColor"] = "undefined",
                            ["directoryNodeSelectedBg"] = "undefined",
                        });
                    return GenBaseStyle(treeSelectToken);
                },
                InitComponentToken);
        }

    }
}
