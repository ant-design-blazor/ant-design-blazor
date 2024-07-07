using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Slide;
using static AntDesign.PlacementArrow;

namespace AntDesign
{
    public partial class DropdownToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

    }

    public partial class DropdownToken : TokenWithCommonCls
    {
        public string RootPrefixCls
        {
            get => (string)_tokens["rootPrefixCls"];
            set => _tokens["rootPrefixCls"] = value;
        }

        public double DropdownArrowDistance
        {
            get => (double)_tokens["dropdownArrowDistance"];
            set => _tokens["dropdownArrowDistance"] = value;
        }

        public double DropdownArrowOffset
        {
            get => (double)_tokens["dropdownArrowOffset"];
            set => _tokens["dropdownArrowOffset"] = value;
        }

        public double DropdownPaddingVertical
        {
            get => (double)_tokens["dropdownPaddingVertical"];
            set => _tokens["dropdownPaddingVertical"] = value;
        }

        public double DropdownEdgeChildPadding
        {
            get => (double)_tokens["dropdownEdgeChildPadding"];
            set => _tokens["dropdownEdgeChildPadding"] = value;
        }

        public string MenuCls
        {
            get => (string)_tokens["menuCls"];
            set => _tokens["menuCls"] = value;
        }

    }

    public partial class Dropdown
    {
        public CSSInterpolation[] GenBaseStyle(DropdownToken token)
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
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        Top = -9999,
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = -9999,
                        },
                        ZIndex = zIndexPopup,
                        Display = "block",
                        ["&::before"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlock = -dropdownArrowDistance + sizePopupArrow / 2,
                            ZIndex = -9999,
                            Opacity = 0.0001f,
                            Content = "\"\"",
                        },
                        [$"&-trigger{antCls}-btn"] = new CSSObject()
                        {
                            [$"& > {iconCls}-down, & > {antCls}-btn-icon > {iconCls}-down"] = new CSSObject()
                            {
                                FontSize = fontSizeIcon,
                            },
                        },
                        [$"{componentCls}-wrap"] = new CSSObject()
                        {
                            Position = "relative",
                            [$"{antCls}-btn > {iconCls}-down"] = new CSSObject()
                            {
                                FontSize = fontSizeIcon,
                            },
                            [$"{iconCls}-down::before"] = new CSSObject()
                            {
                                Transition = @$"transform {motionDurationMid}",
                            },
                        },
                        [$"{componentCls}-wrap-open"] = new CSSObject()
                        {
                            [$"{iconCls}-down::before"] = new CSSObject()
                            {
                                Transform = @$"rotate(180deg)",
                            },
                        },
                        ["&-hidden,&-menu-hidden,&-menu-submenu-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"&{antCls}-slide-down-enter{antCls}-slide-down-enter-active{componentCls}-placement-bottomLeft,&{antCls}-slide-down-appear{antCls}-slide-down-appear-active{componentCls}-placement-bottomLeft,&{antCls}-slide-down-enter{antCls}-slide-down-enter-active{componentCls}-placement-bottom,&{antCls}-slide-down-appear{antCls}-slide-down-appear-active{componentCls}-placement-bottom,&{antCls}-slide-down-enter{antCls}-slide-down-enter-active{componentCls}-placement-bottomRight,&{antCls}-slide-down-appear{antCls}-slide-down-appear-active{componentCls}-placement-bottomRight"] = new CSSObject()
                        {
                            AnimationName = SlideUpIn,
                        },
                        [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-placement-topLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-placement-topLeft,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-placement-top,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-placement-top,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-placement-topRight,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-placement-topRight"] = new CSSObject()
                        {
                            AnimationName = SlideDownIn,
                        },
                        [$"&{antCls}-slide-down-leave{antCls}-slide-down-leave-active{componentCls}-placement-bottomLeft,&{antCls}-slide-down-leave{antCls}-slide-down-leave-active{componentCls}-placement-bottom,&{antCls}-slide-down-leave{antCls}-slide-down-leave-active{componentCls}-placement-bottomRight"] = new CSSObject()
                        {
                            AnimationName = SlideUpOut,
                        },
                        [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-placement-topLeft,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-placement-top,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-placement-topRight"] = new CSSObject()
                        {
                            AnimationName = SlideDownOut,
                        },
                    },
                },
                GetArrowStyle(
                    token,
                    new PlacementArrowOptions()
                    {
                        ColorBg = colorBgElevated,
                        LimitVerticalRadius = true,
                        ArrowPlacement = new ArrowPlacement()
                        {
                            Top = true,
                            Bottom = true,
                        },
                    }),
                new CSSObject()
                {
                    [$"{componentCls} {menuCls}"] = new CSSObject()
                    {
                        Position = "relative",
                        Margin = 0,
                    },
                    [$"{menuCls}-submenu-popup"] = new CSSObject()
                    {
                        Position = "absolute",
                        ZIndex = zIndexPopup,
                        Background = "transparent",
                        BoxShadow = "none",
                        TransformOrigin = "0 0",
                        ["ul, li"] = new CSSObject()
                        {
                            ListStyle = "none",
                            Margin = 0,
                        },
                    },
                    [$"{componentCls}, {componentCls}-menu-submenu"] = new CSSObject()
                    {
                        [menuCls] = new CSSObject()
                        {
                            Padding = dropdownEdgeChildPadding,
                            ListStyleType = "none",
                            BackgroundColor = colorBgElevated,
                            BackgroundClip = "padding-box",
                            BorderRadius = token.BorderRadiusLG,
                            Outline = "none",
                            BoxShadow = token.BoxShadowSecondary,
                            ["..."] = GenFocusStyle(token),
                            [$"{menuCls}-item-group-title"] = new CSSObject()
                            {
                                Padding = @$"{dropdownPaddingVertical}px {controlPaddingHorizontal}px",
                                Color = token.ColorTextDescription,
                                Transition = @$"all {motionDurationMid}",
                            },
                            [$"{menuCls}-item"] = new CSSObject()
                            {
                                Position = "relative",
                                Display = "flex",
                                AlignItems = "center",
                            },
                            [$"{menuCls}-item-icon"] = new CSSObject()
                            {
                                MinWidth = fontSize,
                                MarginInlineEnd = token.MarginXS,
                                FontSize = token.FontSizeSM,
                            },
                            [$"{menuCls}-title-content"] = new CSSObject()
                            {
                                Flex = "auto",
                                ["> a"] = new CSSObject()
                                {
                                    Color = "inherit",
                                    Transition = @$"all {motionDurationMid}",
                                    ["&:hover"] = new CSSObject()
                                    {
                                        Color = "inherit",
                                    },
                                    ["&::after"] = new CSSObject()
                                    {
                                        Position = "absolute",
                                        Inset = 0,
                                        Content = "\"\"",
                                    },
                                },
                            },
                            [$"{menuCls}-item, {menuCls}-submenu-title"] = new CSSObject()
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
                                ["&:hover, &-active"] = new CSSObject()
                                {
                                    BackgroundColor = token.ControlItemBgHover,
                                },
                                ["..."] = GenFocusStyle(token),
                                ["&-selected"] = new CSSObject()
                                {
                                    Color = token.ColorPrimary,
                                    BackgroundColor = token.ControlItemBgActive,
                                    ["&:hover, &-active"] = new CSSObject()
                                    {
                                        BackgroundColor = token.ControlItemBgActiveHover,
                                    },
                                },
                                ["&-disabled"] = new CSSObject()
                                {
                                    Color = colorTextDisabled,
                                    Cursor = "not-allowed",
                                    ["&:hover"] = new CSSObject()
                                    {
                                        Color = colorTextDisabled,
                                        BackgroundColor = colorBgElevated,
                                        Cursor = "not-allowed",
                                    },
                                    ["a"] = new CSSObject()
                                    {
                                        PointerEvents = "none",
                                    },
                                },
                                ["&-divider"] = new CSSObject()
                                {
                                    Height = 1,
                                    Margin = @$"{token.MarginXXS}px 0",
                                    Overflow = "hidden",
                                    LineHeight = 0,
                                    BackgroundColor = token.ColorSplit,
                                },
                                [$"{componentCls}-menu-submenu-expand-icon"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    InsetInlineEnd = token.PaddingXS,
                                    [$"{componentCls}-menu-submenu-arrow-icon"] = new CSSObject()
                                    {
                                        MarginInlineEnd = "0 !important",
                                        Color = token.ColorTextDescription,
                                        FontSize = fontSizeIcon,
                                        FontStyle = "normal",
                                    },
                                },
                            },
                            [$"{menuCls}-item-group-list"] = new CSSObject()
                            {
                                Margin = @$"0 {token.MarginXS}px",
                                Padding = 0,
                                ListStyle = "none",
                            },
                            [$"{menuCls}-submenu-title"] = new CSSObject()
                            {
                                PaddingInlineEnd = controlPaddingHorizontal + token.FontSizeSM,
                            },
                            [$"{menuCls}-submenu-vertical"] = new CSSObject()
                            {
                                Position = "relative",
                            },
                            [$"{menuCls}-submenu{menuCls}-submenu-disabled {componentCls}-menu-submenu-title"] = new CSSObject()
                            {
                                [$"&, {componentCls}-menu-submenu-arrow-icon"] = new CSSObject()
                                {
                                    Color = colorTextDisabled,
                                    BackgroundColor = colorBgElevated,
                                    Cursor = "not-allowed",
                                },
                            },
                            [$"{menuCls}-submenu-selected {componentCls}-menu-submenu-title"] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                    },
                },
                new CSSInterpolation[]
                {
                    InitSlideMotion(token, "slide-up"),
                    InitSlideMotion(token, "slide-down"),
                    InitSlideMotion(token, "move-up"),
                    InitSlideMotion(token, "move-down"),
                    InitSlideMotion(token, "zoom-big"),
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Dropdown",
                (token) =>
                {
                    var rootPrefixCls = token.RootPrefixCls;
                    var marginXXS = token.MarginXXS;
                    var sizePopupArrow = token.SizePopupArrow;
                    var controlHeight = token.ControlHeight;
                    var fontSize = token.FontSize;
                    var lineHeight = token.LineHeight;
                    var paddingXXS = token.PaddingXXS;
                    var componentCls = token.ComponentCls;
                    var borderRadiusLG = token.BorderRadiusLG;
                    var dropdownPaddingVertical = (controlHeight - fontSize * lineHeight) / 2;
                    var arrowToken = GetArrowOffset(
                        new ArrowOffsetOptions()
                        {
                            ContentRadius = borderRadiusLG,
                        });
                    var dropdownArrowOffset = arrowToken.DropdownArrowOffset;
                    var dropdownToken = MergeToken(
                        token,
                        new DropdownToken()
                        {
                            MenuCls = @$"{componentCls}-menu",
                            RootPrefixCls = rootPrefixCls,
                            DropdownArrowDistance = sizePopupArrow / 2 + marginXXS,
                            DropdownArrowOffset = dropdownArrowOffset,
                            DropdownPaddingVertical = dropdownPaddingVertical,
                            DropdownEdgeChildPadding = paddingXXS,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(dropdownToken),
                        GenStatusStyle(dropdownToken),
                    };
                },
                (token) =>
                {
                    return new DropdownToken()
                    {
                        ZIndexPopup = token.ZIndexPopupBase + 50,
                    };
                });
        }

        public CSSObject GenStatusStyle(DropdownToken token)
        {
            var componentCls = token.ComponentCls;
            var menuCls = token.MenuCls;
            var colorError = token.ColorError;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var itemCls = @$"{menuCls}-item";
            return new CSSObject()
            {
                [$"{componentCls}, {componentCls}-menu-submenu"] = new CSSObject()
                {
                    [$"{menuCls} {itemCls}"] = new CSSObject()
                    {
                        [$"&{itemCls}-danger:not({itemCls}-disabled)"] = new CSSObject()
                        {
                            Color = colorError,
                            ["&:hover"] = new CSSObject()
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
