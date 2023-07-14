using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class Tree
    {
        private Keyframes treeNodeFX = new Keyframes("ant-tree-node-fx-do-not-use")
        {
            ["0%"] = new Keyframes()
            {
                Opacity = 0,
            },
            ["100%"] = new Keyframes()
            {
                Opacity = 1,
            },
        };

        public CSSObject GetSwitchStyle(string prefixCls, DerivativeToken token)
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

        public Unknown_1 GetDropIndicatorStyle(string prefixCls, DerivativeToken token)
        {
            return new Unknown_3()
            {
                [$".{prefixCls}-drop-indicator"] = new Unknown_4()
                {
                    Position = "absolute",
                    ZIndex = 1,
                    Height = 2,
                    BackgroundColor = token.ColorPrimary,
                    BorderRadius = 1,
                    PointerEvents = "none",
                    ["&:after"] = new Unknown_5()
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

        public CSSObject GenBaseStyle(string prefixCls, TreeToken token)
        {
            var treeCls = token.TreeCls;
            var treeNodeCls = token.TreeNodeCls;
            var checkboxSize = token.ControlInteractiveSize;
            var treeNodePadding = token.TreeNodePadding;
            var treeTitleHeight = token.TreeTitleHeight;
            var checkBoxOffset = (token.LineHeight * token.FontSize) / 2 - checkboxSize / 2;
            var treeCheckBoxMarginVertical = (treeTitleHeight - token.FontSizeLG) / 2 - checkBoxOffset;
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
                                    AnimationName = treeNodeFX,
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
                                Width = treeTitleHeight,
                                LineHeight = @$"{treeTitleHeight}px",
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
                            Width = treeTitleHeight,
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
                        Width = treeTitleHeight,
                        Margin = 0,
                        LineHeight = @$"{treeTitleHeight}px",
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
                                InsetInlineEnd = treeTitleHeight / 2,
                                Bottom = -treeNodePadding,
                                MarginInlineStart = -1,
                                BorderInlineEnd = @$"1px solid {token.ColorBorder}",
                                Content = "\"\"",
                            },
                            ["&:after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Width = (treeTitleHeight / 2) * 0.8,
                                Height = treeTitleHeight / 2,
                                BorderBottom = @$"1px solid {token.ColorBorder}",
                                Content = "\"\"",
                            },
                        },
                    },
                    [$"{treeCls}-checkbox"] = new CSSObject()
                    {
                        Top = "initial",
                        MarginInlineEnd = treeCheckBoxMarginHorizontal,
                        MarginBlockStart = treeCheckBoxMarginVertical,
                    },
                    [$"{treeCls}-node-content-wrapper, {treeCls}-checkbox + span"] = new CSSObject()
                    {
                        Position = "relative",
                        ZIndex = "auto",
                        MinHeight = treeTitleHeight,
                        Margin = 0,
                        Padding = @$"0 {token.PaddingXS / 2}px",
                        Color = "inherit",
                        LineHeight = @$"{treeTitleHeight}px",
                        Background = "transparent",
                        BorderRadius = token.BorderRadius,
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationMid}, border 0s, line-height 0s, box-shadow 0s",
                        ["&:hover"] = new CSSObject()
                        {
                            BackgroundColor = token.ControlItemBgHover,
                        },
                        [$"&{treeCls}-node-selected"] = new CSSObject()
                        {
                            BackgroundColor = token.ControlItemBgActive,
                        },
                        [$"{treeCls}-iconEle"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = treeTitleHeight,
                            Height = treeTitleHeight,
                            LineHeight = @$"{treeTitleHeight}px",
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
                        LineHeight = @$"{treeTitleHeight}px",
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
                                    InsetInlineEnd = treeTitleHeight / 2,
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
                                    Height = @$"{treeTitleHeight / 2}px !important",
                                },
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenDirectoryStyle(TreeToken token)
        {
            var treeCls = token.TreeCls;
            var treeNodeCls = token.TreeNodeCls;
            var treeNodePadding = token.TreeNodePadding;
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
                                Color = token.ColorTextLightSolid,
                                Background = "transparent",
                            },
                        },
                        ["&-selected"] = new CSSObject()
                        {
                            ["&:hover::before,&::before"] = new CSSObject()
                            {
                                Background = token.ColorPrimary,
                            },
                            [$"{treeCls}-switcher"] = new CSSObject()
                            {
                                Color = token.ColorTextLightSolid,
                            },
                            [$"{treeCls}-node-content-wrapper"] = new CSSObject()
                            {
                                Color = token.ColorTextLightSolid,
                                Background = "transparent",
                            },
                        },
                    },
                },
            };
        }

        public CSSInterpolation GenTreeStyle(string prefixCls, DerivativeToken token)
        {
            var treeCls = @$".{prefixCls}";
            var treeNodeCls = @$"{treeCls}-treenode";
            var treeNodePadding = token.PaddingXS / 2;
            var treeTitleHeight = token.ControlHeightSM;
            var treeToken = MergeToken(
                token,
                new CSSInterpolation()
                {
                    TreeCls = treeCls,
                    TreeNodeCls = treeNodeCls,
                    TreeNodePadding = treeNodePadding,
                    TreeTitleHeight = treeTitleHeight,
                });
            return new CSSInterpolation
            {
                GenBaseStyle(prefixCls, treeToken),
                GenDirectoryStyle(treeToken)
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_6 token, Unknown_7 args)
        {
            return new Unknown_8
            {
                new Unknown_9()
                {
                    [token.ComponentCls] = GetCheckboxStyle($"{prefixCls}-checkbox", token)
                },
                GenTreeStyle(prefixCls, token),
                GenCollapseMotion(token)
            };
        }

    }

    public partial class TreeToken : TokenWithCommonCls
    {
        public string TreeCls { get; set; }

        public string TreeNodeCls { get; set; }

        public int TreeNodePadding { get; set; }

        public int TreeTitleHeight { get; set; }

    }

}