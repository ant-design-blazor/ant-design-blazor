using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class DropdownToken
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class DropdownToken : TokenWithCommonCls
    {
        public string RootPrefixCls { get; set; }

        public int DropdownArrowDistance { get; set; }

        public int DropdownArrowOffset { get; set; }

        public int DropdownPaddingVertical { get; set; }

        public int DropdownEdgeChildPadding { get; set; }

        public string MenuCls { get; set; }

    }

    public partial class Dropdown
    {
        public Unknown_1 GenBaseStyle(Unknown_5 token)
        {
            var componentCls = token.ComponentCls;
            var menuCls = token.MenuCls;
            var zIndexPopup = token.ZIndexPopup;
            var dropdownArrowDistance = token.DropdownArrowDistance;
            var sizePopupArrow = token.SizePopupArrow;
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var motionDurationMid = token.MotionDurationMid;
            var dropdownPaddingVertical = token.DropdownPaddingVertical;
            var fontSize = token.FontSize;
            var dropdownEdgeChildPadding = token.DropdownEdgeChildPadding;
            var colorTextDisabled = token.ColorTextDisabled;
            var fontSizeIcon = token.FontSizeIcon;
            var controlPaddingHorizontal = token.ControlPaddingHorizontal;
            var colorBgElevated = token.ColorBgElevated;
            return new Unknown_6
            {
                new Unknown_7()
                {
                    [componentCls] = new Unknown_8()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        Top = -9999,
                        Left = new Unknown_9()
                        {
                            SkipCheck = true,
                            Value = -9999,
                        },
                        ZIndex = zIndexPopup,
                        Display = "block",
                        ["&::before"] = new Unknown_10()
                        {
                            Position = "absolute",
                            InsetBlock = -dropdownArrowDistance + sizePopupArrow / 2,
                            ZIndex = -9999,
                            Opacity = 0.0001f,
                            Content = "\"\"",
                        },
                        [$"&-trigger{antCls}-btn > {iconCls}-down"] = new Unknown_11()
                        {
                            FontSize = fontSizeIcon,
                            Transform = "none",
                        },
                        [$"{componentCls}-wrap"] = new Unknown_12()
                        {
                            Position = "relative",
                            [$"{antCls}-btn > {iconCls}-down"] = new Unknown_13()
                            {
                                FontSize = fontSizeIcon,
                            },
                            [$"{iconCls}-down::before"] = new Unknown_14()
                            {
                                Transition = @$"transform {motionDurationMid}",
                            },
                        },
                        [$"{componentCls}-wrap-open"] = new Unknown_15()
                        {
                            [$"{iconCls}-down::before"] = new Unknown_16()
                            {
                                Transform = @$"rotate(180deg)",
                            },
                        },
                        ["&-hidden,&-menu-hidden,&-menu-submenu-hidden"] = new Unknown_17()
                        {
                            Display = "none",
                        },
                        [$"&{antCls}-slide-down-enter{antCls}-slide-down-enter-active{componentCls}-placement-bottomLeft,&{antCls}-slide-down-appear{antCls}-slide-down-appear-active{componentCls}-placement-bottomLeft,&{antCls}-slide-down-enter{antCls}-slide-down-enter-active{componentCls}-placement-bottom,&{antCls}-slide-down-appear{antCls}-slide-down-appear-active{componentCls}-placement-bottom,&{antCls}-slide-down-enter{antCls}-slide-down-enter-active{componentCls}-placement-bottomRight,&{antCls}-slide-down-appear{antCls}-slide-down-appear-active{componentCls}-placement-bottomRight"] = new Unknown_18()
                        {
                            AnimationName = slideUpIn,
                        },
                        [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-placement-topLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-placement-topLeft,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-placement-top,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-placement-top,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-placement-topRight,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-placement-topRight"] = new Unknown_19()
                        {
                            AnimationName = slideDownIn,
                        },
                        [$"&{antCls}-slide-down-leave{antCls}-slide-down-leave-active{componentCls}-placement-bottomLeft,&{antCls}-slide-down-leave{antCls}-slide-down-leave-active{componentCls}-placement-bottom,&{antCls}-slide-down-leave{antCls}-slide-down-leave-active{componentCls}-placement-bottomRight"] = new Unknown_20()
                        {
                            AnimationName = slideUpOut,
                        },
                        [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-placement-topLeft,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-placement-top,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-placement-topRight"] = new Unknown_21()
                        {
                            AnimationName = slideDownOut,
                        },
                    },
                },
                GetArrowStyle<DropdownToken>(token, {
      colorBg: colorBgElevated,
      limitVerticalRadius: true,
      arrowPlacement: { top: true, bottom: true },
    }),
                new Unknown_22()
                {
                    [$"{componentCls} {menuCls}"] = new Unknown_23()
                    {
                        Position = "relative",
                        Margin = 0,
                    },
                    [$"{menuCls}-submenu-popup"] = new Unknown_24()
                    {
                        Position = "absolute",
                        ZIndex = zIndexPopup,
                        Background = "transparent",
                        BoxShadow = "none",
                        TransformOrigin = "0 0",
                        ["ul, li"] = new Unknown_25()
                        {
                            ListStyle = "none",
                            Margin = 0,
                        },
                    },
                    [$"{componentCls}, {componentCls}-menu-submenu"] = new Unknown_26()
                    {
                        [menuCls] = new Unknown_27()
                        {
                            Padding = dropdownEdgeChildPadding,
                            ListStyleType = "none",
                            BackgroundColor = colorBgElevated,
                            BackgroundClip = "padding-box",
                            BorderRadius = token.BorderRadiusLG,
                            Outline = "none",
                            BoxShadow = token.BoxShadowSecondary,
                            ["..."] = GenFocusStyle(token),
                            [$"{menuCls}-item-group-title"] = new Unknown_28()
                            {
                                Padding = @$"{dropdownPaddingVertical}px {controlPaddingHorizontal}px",
                                Color = token.ColorTextDescription,
                                Transition = @$"all {motionDurationMid}",
                            },
                            [$"{menuCls}-item"] = new Unknown_29()
                            {
                                Position = "relative",
                                Display = "flex",
                                AlignItems = "center",
                            },
                            [$"{menuCls}-item-icon"] = new Unknown_30()
                            {
                                MinWidth = fontSize,
                                MarginInlineEnd = token.MarginXS,
                                FontSize = token.FontSizeSM,
                            },
                            [$"{menuCls}-title-content"] = new Unknown_31()
                            {
                                Flex = "auto",
                                ["> a"] = new Unknown_32()
                                {
                                    Color = "inherit",
                                    Transition = @$"all {motionDurationMid}",
                                    ["&:hover"] = new Unknown_33()
                                    {
                                        Color = "inherit",
                                    },
                                    ["&::after"] = new Unknown_34()
                                    {
                                        Position = "absolute",
                                        Inset = 0,
                                        Content = "\"\"",
                                    },
                                },
                            },
                            [$"{menuCls}-item, {menuCls}-submenu-title"] = new Unknown_35()
                            {
                                Clear = "both",
                                Margin = 0,
                                Padding = @$"{dropdownPaddingVertical}px {controlPaddingHorizontal}px",
                                Color = token.ColorText,
                                FontWeight = "normal",
                                FontSize = fontSize,
                                LineHeight = token.LineHeight,
                                Cursor = "pointer",
                                Transition = @$"all {motionDurationMid}",
                                BorderRadius = token.BorderRadiusSM,
                                ["&:hover, &-active"] = new Unknown_36()
                                {
                                    BackgroundColor = token.ControlItemBgHover,
                                },
                                ["..."] = GenFocusStyle(token),
                                ["&-selected"] = new Unknown_37()
                                {
                                    Color = token.ColorPrimary,
                                    BackgroundColor = token.ControlItemBgActive,
                                    ["&:hover, &-active"] = new Unknown_38()
                                    {
                                        BackgroundColor = token.ControlItemBgActiveHover,
                                    },
                                },
                                ["&-disabled"] = new Unknown_39()
                                {
                                    Color = colorTextDisabled,
                                    Cursor = "not-allowed",
                                    ["&:hover"] = new Unknown_40()
                                    {
                                        Color = colorTextDisabled,
                                        BackgroundColor = colorBgElevated,
                                        Cursor = "not-allowed",
                                    },
                                    ["a"] = new Unknown_41()
                                    {
                                        PointerEvents = "none",
                                    },
                                },
                                ["&-divider"] = new Unknown_42()
                                {
                                    Height = 1,
                                    Margin = @$"{token.MarginXXS}px 0",
                                    Overflow = "hidden",
                                    LineHeight = 0,
                                    BackgroundColor = token.ColorSplit,
                                },
                                [$"{componentCls}-menu-submenu-expand-icon"] = new Unknown_43()
                                {
                                    Position = "absolute",
                                    InsetInlineEnd = token.PaddingXS,
                                    [$"{componentCls}-menu-submenu-arrow-icon"] = new Unknown_44()
                                    {
                                        MarginInlineEnd = "0 !important",
                                        Color = token.ColorTextDescription,
                                        FontSize = fontSizeIcon,
                                        FontStyle = "normal",
                                    },
                                },
                            },
                            [$"{menuCls}-item-group-list"] = new Unknown_45()
                            {
                                Margin = @$"0 {token.MarginXS}px",
                                Padding = 0,
                                ListStyle = "none",
                            },
                            [$"{menuCls}-submenu-title"] = new Unknown_46()
                            {
                                PaddingInlineEnd = controlPaddingHorizontal + token.FontSizeSM,
                            },
                            [$"{menuCls}-submenu-vertical"] = new Unknown_47()
                            {
                                Position = "relative",
                            },
                            [$"{menuCls}-submenu{menuCls}-submenu-disabled {componentCls}-menu-submenu-title"] = new Unknown_48()
                            {
                                [$"&, {componentCls}-menu-submenu-arrow-icon"] = new Unknown_49()
                                {
                                    Color = colorTextDisabled,
                                    BackgroundColor = colorBgElevated,
                                    Cursor = "not-allowed",
                                },
                            },
                            [$"{menuCls}-submenu-selected {componentCls}-menu-submenu-title"] = new Unknown_50()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                    },
                },
                [
      initSlideMotion(token, "slide-up"),
      initSlideMotion(token, "slide-down"),
      initMoveMotion(token, "move-up"),
      initMoveMotion(token, "move-down"),
      initZoomMotion(token, "zoom-big"),
    ]
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_51 token, Unknown_52 args)
        {
            var marginXXS = token.MarginXXS;
            var sizePopupArrow = token.SizePopupArrow;
            var controlHeight = token.ControlHeight;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var paddingXXS = token.PaddingXXS;
            var componentCls = token.ComponentCls;
            var borderRadiusLG = token.BorderRadiusLG;
            var dropdownPaddingVertical = (controlHeight - fontSize * lineHeight) / 2;
            var dropdownArrowOffset = getArrowOffset({
      contentRadius: borderRadiusLG,
    }).DropdownArrowOffset;
            var dropdownToken = MergeToken(
                token,
                new Unknown_53()
                {
                    MenuCls = @$"{componentCls}-menu",
                    RootPrefixCls = rootPrefixCls,
                    DropdownArrowDistance = sizePopupArrow / 2 + marginXXS,
                    DropdownArrowOffset = dropdownArrowOffset,
                    DropdownPaddingVertical = dropdownPaddingVertical,
                    DropdownEdgeChildPadding = paddingXXS,
                });
            return new Unknown_54
            {
                GenBaseStyle(dropdownToken),
                GenStatusStyle(dropdownToken)
            };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_55 token)
        {
            return new Unknown_56()
            {
                ZIndexPopup = token.ZIndexPopupBase + 50,
            };
        }

        public Unknown_4 GenStatusStyle(Unknown_57 token)
        {
            var componentCls = token.ComponentCls;
            var menuCls = token.MenuCls;
            var colorError = token.ColorError;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var itemCls = @$"{menuCls}-item";
            return new Unknown_58()
            {
                [$"{componentCls}, {componentCls}-menu-submenu"] = new Unknown_59()
                {
                    [$"{menuCls} {itemCls}"] = new Unknown_60()
                    {
                        [$"&{itemCls}-danger:not({itemCls}-disabled)"] = new Unknown_61()
                        {
                            Color = colorError,
                            ["&:hover"] = new Unknown_62()
                            {
                                Color = colorTextLightSolid,
                                BackgroundColor = colorError,
                            },
                        },
                    },
                },
            };
        }

    }

}