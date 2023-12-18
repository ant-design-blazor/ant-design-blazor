using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.CollapseMotion;
using static AntDesign.TreeStyle;
using Keyframes = CssInCSharp.Keyframe;

namespace AntDesign
{
    public partial class TreeSharedToken : TokenWithCommonCls
    {
        public double TitleHeight
        {
            get => (double)_tokens["titleHeight"];
            set => _tokens["titleHeight"] = value;
        }

        public string NodeHoverBg
        {
            get => (string)_tokens["nodeHoverBg"];
            set => _tokens["nodeHoverBg"] = value;
        }

        public string NodeSelectedBg
        {
            get => (string)_tokens["nodeSelectedBg"];
            set => _tokens["nodeSelectedBg"] = value;
        }

    }

    public partial class TreeToken
    {
        public string DirectoryNodeSelectedColor
        {
            get => (string)_tokens["directoryNodeSelectedColor"];
            set => _tokens["directoryNodeSelectedColor"] = value;
        }

        public string DirectoryNodeSelectedBg
        {
            get => (string)_tokens["directoryNodeSelectedBg"];
            set => _tokens["directoryNodeSelectedBg"] = value;
        }

    }

    public class TreeStyle
    {
        private static Keyframes _treeNodeFX = new Keyframes("ant-tree-node-fx-do-not-use",
            new CSSObject()
            {
                ["0%"] = new CSSObject()
                {
                    Opacity = 0,
                },
                ["100%"] = new CSSObject()
                {
                    Opacity = 1,
                },
            });

        public static CSSObject GetSwitchStyle(string prefixCls, GlobalToken token)
        {
            return new CSSObject()
            {
                [$".{prefixCls}-switcher-icon"] = new CSSObject()
                {
                    Display = "inline-block",
                    FontSize = 10,
                    VerticalAlign = "baseline",
                    ["svg"] = new CSSObject()
                    {
                        Transition = @$"transform {token.MotionDurationSlow}",
                    },
                },
            };
        }

        public static CSSObject GetDropIndicatorStyle(string prefixCls, GlobalToken token)
        {
            return new CSSObject()
            {
                [$".{prefixCls}-drop-indicator"] = new CSSObject()
                {
                    Position = "absolute",
                    ZIndex = 1,
                    Height = 2,
                    BackgroundColor = token.ColorPrimary,
                    BorderRadius = 1,
                    PointerEvents = "none",
                    ["&:after"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = -3,
                        InsetInlineStart = -6,
                        Width = 8,
                        Height = 8,
                        BackgroundColor = "transparent",
                        Border = @$"{token.LineWidthBold}px solid {token.ColorPrimary}",
                        BorderRadius = "50%",
                        Content = "\"\"",
                    },
                },
            };
        }

        public static CSSObject GenBaseStyle(string prefixCls, TreeToken token)
        {
            var treeCls = token.TreeCls;
            var treeNodeCls = token.TreeNodeCls;
            var treeNodePadding = token.TreeNodePadding;
            var titleHeight = token.TitleHeight;
            var nodeSelectedBg = token.NodeSelectedBg;
            var nodeHoverBg = token.NodeHoverBg;
            var treeCheckBoxMarginHorizontal = token.PaddingXS;
            return new CSSObject()
            {
                [treeCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Background = token.ColorBgContainer,
                    BorderRadius = token.BorderRadius,
                    Transition = @$"background-color {token.MotionDurationSlow}",
                    [$"&{treeCls}-rtl"] = new CSSObject()
                    {
                        [$"{treeCls}-switcher"] = new CSSObject()
                        {
                            ["&_close"] = new CSSObject()
                            {
                                [$"{treeCls}-switcher-icon"] = new CSSObject()
                                {
                                    ["svg"] = new CSSObject()
                                    {
                                        Transform = "rotate(90deg)",
                                    },
                                },
                            },
                        },
                    },
                    [$"&-focused:not(:hover):not({treeCls}-active-focused)"] = new CSSObject()
                    {
                        ["..."] = GenFocusOutline(token)
                    },
                    [$"{treeCls}-list-holder-inner"] = new CSSObject()
                    {
                        AlignItems = "flex-start",
                    },
                    [$"&{treeCls}-block-node"] = new CSSObject()
                    {
                        [$"{treeCls}-list-holder-inner"] = new CSSObject()
                        {
                            AlignItems = "stretch",
                            [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                            {
                                Flex = "auto",
                            },
                            [$"{treeNodeCls}.dragging"] = new CSSObject()
                            {
                                Position = "relative",
                                ["&:after"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Top = 0,
                                    InsetInlineEnd = 0,
                                    Bottom = treeNodePadding,
                                    InsetInlineStart = 0,
                                    Border = @$"1px solid {token.ColorPrimary}",
                                    Opacity = 0,
                                    AnimationName = _treeNodeFX,
                                    AnimationDuration = token.MotionDurationSlow,
                                    AnimationPlayState = "running",
                                    AnimationFillMode = "forwards",
                                    Content = "\"\"",
                                    PointerEvents = "none",
                                },
                            },
                        },
                    },
                    [$"{treeNodeCls}"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignItems = "flex-start",
                        Padding = @$"0 0 {treeNodePadding}px 0",
                        Outline = "none",
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                        },
                        ["&-disabled"] = new CSSObject()
                        {
                            [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                            {
                                Color = token.ColorTextDisabled,
                                Cursor = "not-allowed",
                                ["&:hover"] = new CSSObject()
                                {
                                    Background = "transparent",
                                },
                            },
                        },
                        [$"&-active {treeCls}-node-content-wrapper"] = new CSSObject()
                        {
                            ["..."] = GenFocusOutline(token)
                        },
                        [$"&:not({treeNodeCls}-disabled).filter-node {treeCls}-title"] = new CSSObject()
                        {
                            Color = "inherit",
                            FontWeight = 500,
                        },
                        ["&-draggable"] = new CSSObject()
                        {
                            [$"{treeCls}-draggable-icon"] = new CSSObject()
                            {
                                FlexShrink = 0,
                                Width = titleHeight,
                                LineHeight = @$"{titleHeight}px",
                                TextAlign = "center",
                                Visibility = "visible",
                                Opacity = 0.2f,
                                Transition = @$"opacity {token.MotionDurationSlow}",
                                [$"{treeNodeCls}:hover &"] = new CSSObject()
                                {
                                    Opacity = 0.45f,
                                },
                            },
                            [$"&{treeNodeCls}-disabled"] = new CSSObject()
                            {
                                [$"{treeCls}-draggable-icon"] = new CSSObject()
                                {
                                    Visibility = "hidden",
                                },
                            },
                        },
                    },
                    [$"{treeCls}-indent"] = new CSSObject()
                    {
                        AlignSelf = "stretch",
                        WhiteSpace = "nowrap",
                        UserSelect = "none",
                        ["&-unit"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = titleHeight,
                        },
                    },
                    [$"{treeCls}-draggable-icon"] = new CSSObject()
                    {
                        Visibility = "hidden",
                    },
                    [$"{treeCls}-switcher"] = new CSSObject()
                    {
                        ["..."] = GetSwitchStyle(prefixCls, token),
                        Position = "relative",
                        Flex = "none",
                        AlignSelf = "stretch",
                        Width = titleHeight,
                        Margin = 0,
                        LineHeight = @$"{titleHeight}px",
                        TextAlign = "center",
                        Cursor = "pointer",
                        UserSelect = "none",
                        ["&-noop"] = new CSSObject()
                        {
                            Cursor = "default",
                        },
                        ["&_close"] = new CSSObject()
                        {
                            [$"{treeCls}-switcher-icon"] = new CSSObject()
                            {
                                ["svg"] = new CSSObject()
                                {
                                    Transform = "rotate(-90deg)",
                                },
                            },
                        },
                        ["&-loading-icon"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                        },
                        ["&-leaf-line"] = new CSSObject()
                        {
                            Position = "relative",
                            ZIndex = 1,
                            Display = "inline-block",
                            Width = "100%",
                            Height = "100%",
                            ["&:before"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = 0,
                                InsetInlineEnd = titleHeight / 2,
                                Bottom = -treeNodePadding,
                                MarginInlineStart = -1,
                                BorderInlineEnd = @$"1px solid {token.ColorBorder}",
                                Content = "\"\"",
                            },
                            ["&:after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Width = (titleHeight / 2) * 0.8,
                                Height = titleHeight / 2,
                                BorderBottom = @$"1px solid {token.ColorBorder}",
                                Content = "\"\"",
                            },
                        },
                    },
                    [$"{treeCls}-checkbox"] = new CSSObject()
                    {
                        Top = "initial",
                        MarginInlineEnd = treeCheckBoxMarginHorizontal,
                        AlignSelf = "flex-start",
                        MarginTop = token.MarginXXS,
                    },
                    [$"{treeCls}-node-content-wrapper, {treeCls}-checkbox + span"] = new CSSObject()
                    {
                        Position = "relative",
                        ZIndex = "auto",
                        MinHeight = titleHeight,
                        Margin = 0,
                        Padding = @$"0 {token.PaddingXS / 2}px",
                        Color = "inherit",
                        LineHeight = @$"{titleHeight}px",
                        Background = "transparent",
                        BorderRadius = token.BorderRadius,
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationMid}, border 0s, line-height 0s, box-shadow 0s",
                        ["&:hover"] = new CSSObject()
                        {
                            BackgroundColor = nodeHoverBg,
                        },
                        [$"&{treeCls}-node-selected"] = new CSSObject()
                        {
                            BackgroundColor = nodeSelectedBg,
                        },
                        [$"{treeCls}-iconEle"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = titleHeight,
                            Height = titleHeight,
                            LineHeight = @$"{titleHeight}px",
                            TextAlign = "center",
                            VerticalAlign = "top",
                            ["&:empty"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                    },
                    [$"{treeCls}-unselectable {treeCls}-node-content-wrapper:hover"] = new CSSObject()
                    {
                        BackgroundColor = "transparent",
                    },
                    [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                    {
                        LineHeight = @$"{titleHeight}px",
                        UserSelect = "none",
                        ["..."] = GetDropIndicatorStyle(prefixCls, token)
                    },
                    [$"{treeNodeCls}.drop-container"] = new CSSObject()
                    {
                        ["> [draggable]"] = new CSSObject()
                        {
                            BoxShadow = @$"0 0 0 2px {token.ColorPrimary}",
                        },
                    },
                    ["&-show-line"] = new CSSObject()
                    {
                        [$"{treeCls}-indent"] = new CSSObject()
                        {
                            ["&-unit"] = new CSSObject()
                            {
                                Position = "relative",
                                Height = "100%",
                                ["&:before"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Top = 0,
                                    InsetInlineEnd = titleHeight / 2,
                                    Bottom = -treeNodePadding,
                                    BorderInlineEnd = @$"1px solid {token.ColorBorder}",
                                    Content = "\"\"",
                                },
                                ["&-end"] = new CSSObject()
                                {
                                    ["&:before"] = new CSSObject()
                                    {
                                        Display = "none",
                                    },
                                },
                            },
                        },
                        [$"{treeCls}-switcher"] = new CSSObject()
                        {
                            Background = "transparent",
                            ["&-line-icon"] = new CSSObject()
                            {
                                VerticalAlign = "-0.15em",
                            },
                        },
                    },
                    [$"{treeNodeCls}-leaf-last"] = new CSSObject()
                    {
                        [$"{treeCls}-switcher"] = new CSSObject()
                        {
                            ["&-leaf-line"] = new CSSObject()
                            {
                                ["&:before"] = new CSSObject()
                                {
                                    Top = "auto !important",
                                    Bottom = "auto !important",
                                    Height = @$"{titleHeight / 2}px !important",
                                },
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenDirectoryStyle(TreeToken token)
        {
            var treeCls = token.TreeCls;
            var treeNodeCls = token.TreeNodeCls;
            var treeNodePadding = token.TreeNodePadding;
            var directoryNodeSelectedBg = token.DirectoryNodeSelectedBg;
            var directoryNodeSelectedColor = token.DirectoryNodeSelectedColor;
            return new CSSObject()
            {
                [$"{treeCls}{treeCls}-directory"] = new CSSObject()
                {
                    [treeNodeCls] = new CSSObject()
                    {
                        Position = "relative",
                        ["&:before"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineEnd = 0,
                            Bottom = treeNodePadding,
                            InsetInlineStart = 0,
                            Transition = @$"background-color {token.MotionDurationMid}",
                            Content = "\"\"",
                            PointerEvents = "none",
                        },
                        ["&:hover"] = new CSSObject()
                        {
                            ["&:before"] = new CSSObject()
                            {
                                Background = token.ControlItemBgHover,
                            },
                        },
                        ["> *"] = new CSSObject()
                        {
                            ZIndex = 1,
                        },
                        [$"{treeCls}-switcher"] = new CSSObject()
                        {
                            Transition = @$"color {token.MotionDurationMid}",
                        },
                        [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                        {
                            BorderRadius = 0,
                            UserSelect = "none",
                            ["&:hover"] = new CSSObject()
                            {
                                Background = "transparent",
                            },
                            [$"&{treeCls}-node-selected"] = new CSSObject()
                            {
                                Color = directoryNodeSelectedColor,
                                Background = "transparent",
                            },
                        },
                        ["&-selected"] = new CSSObject()
                        {
                            ["&:hover::before,&::before"] = new CSSObject()
                            {
                                Background = directoryNodeSelectedBg,
                            },
                            [$"{treeCls}-switcher"] = new CSSObject()
                            {
                                Color = directoryNodeSelectedColor,
                            },
                            [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                            {
                                Color = directoryNodeSelectedColor,
                                Background = "transparent",
                            },
                        },
                    },
                },
            };
        }

        public static CSSInterpolation[] GenTreeStyle(string prefixCls, TreeSharedToken token)
        {
            var treeCls = @$".{prefixCls}";
            var treeNodeCls = @$"{treeCls}-treenode";
            var treeNodePadding = token.PaddingXS / 2;
            var treeToken = MergeToken(
                token,
                new TreeToken()
                {
                    TreeCls = treeCls,
                    TreeNodeCls = treeNodeCls,
                    TreeNodePadding = treeNodePadding,
                });
            return new CSSInterpolation[]
            {
                GenBaseStyle(prefixCls, treeToken),
                GenDirectoryStyle(treeToken),
            };
        }

        public static TreeSharedToken InitComponentToken(GlobalToken token)
        {
            var controlHeightSM = token.ControlHeightSM;
            return new TreeSharedToken()
            {
                TitleHeight = controlHeightSM,
                NodeHoverBg = token.ControlItemBgHover,
                NodeSelectedBg = token.ControlItemBgActive,
            };
        }
    }

    public partial class Tree<TItem>
    {
        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Tree",
                (token) =>
                {
                    var prefixCls = token.PrefixCls;
                    return new CSSInterpolation[]
                    {
                        new CSSObject()
                        {
                            [token.ComponentCls] = GetCheckboxStyle($"{prefixCls}-checkbox", token)
                        },
                        GenTreeStyle(prefixCls, token),
                        GenCollapseMotion(token),
                    };
                },
                (token) =>
                {
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorPrimary = token.ColorPrimary;
                    return MergeToken(InitComponentToken(token),
                        new TreeToken()
                        {
                            DirectoryNodeSelectedColor = colorTextLightSolid,
                            DirectoryNodeSelectedBg = colorPrimary,
                        });
                });
        }
    }

    public partial class TreeToken : TreeSharedToken
    {
        public string TreeCls
        {
            get => (string)_tokens["treeCls"];
            set => _tokens["treeCls"] = value;
        }

        public string TreeNodeCls
        {
            get => (string)_tokens["treeNodeCls"];
            set => _tokens["treeNodeCls"] = value;
        }

        public double TreeNodePadding
        {
            get => (double)_tokens["treeNodePadding"];
            set => _tokens["treeNodePadding"] = value;
        }

    }

}
