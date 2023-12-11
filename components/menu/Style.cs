using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class MenuToken
    {
        public double DropdownWidth { get; set; }

        public double ZIndexPopup { get; set; }

        public string ColorGroupTitle { get; set; }

        public string GroupTitleColor { get; set; }

        public string GroupTitleLineHeight { get; set; }

        public double GroupTitleFontSize { get; set; }

        public double RadiusItem { get; set; }

        public double ItemBorderRadius { get; set; }

        public double RadiusSubMenuItem { get; set; }

        public double SubMenuItemBorderRadius { get; set; }

        public string ColorItemText { get; set; }

        public string ItemColor { get; set; }

        public string ColorItemTextHover { get; set; }

        public string ItemHoverColor { get; set; }

        public string ColorItemTextHoverHorizontal { get; set; }

        public string HorizontalItemHoverColor { get; set; }

        public string ColorItemTextSelected { get; set; }

        public string ItemSelectedColor { get; set; }

        public string ColorItemTextSelectedHorizontal { get; set; }

        public string HorizontalItemSelectedColor { get; set; }

        public string ColorItemTextDisabled { get; set; }

        public string ItemDisabledColor { get; set; }

        public string ColorDangerItemText { get; set; }

        public string DangerItemColor { get; set; }

        public string ColorDangerItemTextHover { get; set; }

        public string DangerItemHoverColor { get; set; }

        public string ColorDangerItemTextSelected { get; set; }

        public string DangerItemSelectedColor { get; set; }

        public string ColorDangerItemBgActive { get; set; }

        public string DangerItemActiveBg { get; set; }

        public string ColorDangerItemBgSelected { get; set; }

        public string DangerItemSelectedBg { get; set; }

        public string ColorItemBg { get; set; }

        public string ItemBg { get; set; }

        public string ColorItemBgHover { get; set; }

        public string ItemHoverBg { get; set; }

        public string ColorSubItemBg { get; set; }

        public string SubMenuItemBg { get; set; }

        public string ColorItemBgActive { get; set; }

        public string ItemActiveBg { get; set; }

        public string ColorItemBgSelected { get; set; }

        public string ItemSelectedBg { get; set; }

        public string ColorItemBgSelectedHorizontal { get; set; }

        public string HorizontalItemSelectedBg { get; set; }

        public double ColorActiveBarWidth { get; set; }

        public double ActiveBarWidth { get; set; }

        public double ColorActiveBarHeight { get; set; }

        public double ActiveBarHeight { get; set; }

        public double ColorActiveBarBorderSize { get; set; }

        public double ActiveBarBorderWidth { get; set; }

        public double ItemMarginInline { get; set; }

        public string HorizontalItemHoverBg { get; set; }

        public double HorizontalItemBorderRadius { get; set; }

        public double ItemHeight { get; set; }

        public double CollapsedWidth { get; set; }

        public string PopupBg { get; set; }

        public string ItemMarginBlock { get; set; }

        public string ItemPaddingInline { get; set; }

        public string HorizontalLineHeight { get; set; }

        public string IconMarginInlineEnd { get; set; }

        public double IconSize { get; set; }

        public double CollapsedIconSize { get; set; }

        public string DarkItemColor { get; set; }

        public string DarkDangerItemColor { get; set; }

        public string DarkItemBg { get; set; }

        public string DarkSubMenuItemBg { get; set; }

        public string DarkItemSelectedColor { get; set; }

        public string DarkItemSelectedBg { get; set; }

        public string DarkItemHoverBg { get; set; }

        public string DarkGroupTitleColor { get; set; }

        public string DarkItemHoverColor { get; set; }

        public string DarkItemDisabledColor { get; set; }

        public string DarkDangerItemSelectedBg { get; set; }

        public string DarkDangerItemHoverColor { get; set; }

        public string DarkDangerItemSelectedColor { get; set; }

        public string DarkDangerItemActiveBg { get; set; }

    }

    public partial class MenuToken : TokenWithCommonCls
    {
        public double MenuHorizontalHeight { get; set; }

        public double MenuArrowSize { get; set; }

        public string MenuArrowOffset { get; set; }

        public double MenuPanelMaskInset { get; set; }

        public string MenuSubMenuBg { get; set; }

    }

    public partial class Menu
    {
        public CSSObject GenMenuItemStyle(MenuToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var motionEaseOut = token.MotionEaseOut;
            var iconCls = token.IconCls;
            var iconSize = token.IconSize;
            var iconMarginInlineEnd = token.IconMarginInlineEnd;
            return new CSSObject()
            {
                [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                {
                    Position = "relative",
                    Display = "block",
                    Margin = 0,
                    WhiteSpace = "nowrap",
                    Cursor = "pointer",
                    Transition = new string[]
                    {
                        $"border-color {motionDurationSlow}",
                        $"background {motionDurationSlow}",
                        $"padding {motionDurationSlow} {motionEaseInOut}"
                    }.Join(","),
                    [$"{componentCls}-item-icon, {iconCls}"] = new CSSObject()
                    {
                        MinWidth = iconSize,
                        FontSize = iconSize,
                        Transition = new string[]
                        {
                            $"font-size {motionDurationMid} {motionEaseOut}",
                            $"margin {motionDurationSlow} {motionEaseInOut}",
                            $"color {motionDurationSlow}"
                        }.Join(","),
                        ["+ span"] = new CSSObject()
                        {
                            MarginInlineStart = iconMarginInlineEnd,
                            Opacity = 1,
                            Transition = new string[]
                            {
                                $"opacity {motionDurationSlow} {motionEaseInOut}",
                                $"margin {motionDurationSlow}",
                                $"color {motionDurationSlow}"
                            }.Join(",")
                        },
                    },
                    [$"{componentCls}-item-icon"] = new CSSObject()
                    {
                        ["..."] = ResetIcon()
                    },
                    [$"&{componentCls}-item-only-child"] = new CSSObject()
                    {
                        [$"> {iconCls}, > {componentCls}-item-icon"] = new CSSObject()
                        {
                            MarginInlineEnd = 0,
                        },
                    },
                },
                [$"{componentCls}-item-disabled, {componentCls}-submenu-disabled"] = new CSSObject()
                {
                    Background = "none !important",
                    Cursor = "not-allowed",
                    ["&::after"] = new CSSObject()
                    {
                        BorderColor = "transparent !important",
                    },
                    ["a"] = new CSSObject()
                    {
                        Color = "inherit !important",
                    },
                    [$"> {componentCls}-submenu-title"] = new CSSObject()
                    {
                        Color = "inherit !important",
                        Cursor = "not-allowed",
                    },
                },
            };
        }

        public CSSObject GenSubMenuArrowStyle(MenuToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionEaseInOut = token.MotionEaseInOut;
            var borderRadius = token.BorderRadius;
            var menuArrowSize = token.MenuArrowSize;
            var menuArrowOffset = token.MenuArrowOffset;
            return new CSSObject()
            {
                [$"{componentCls}-submenu"] = new CSSObject()
                {
                    ["&-expand-icon, &-arrow"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = "50%",
                        InsetInlineEnd = token.Margin,
                        Width = menuArrowSize,
                        Color = "currentcolor",
                        Transform = "translateY(-50%)",
                        Transition = @$"transform {motionDurationSlow} {motionEaseInOut}, opacity {motionDurationSlow}",
                    },
                    ["&-arrow"] = new CSSObject()
                    {
                        ["&::before, &::after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Width = menuArrowSize * 0.6,
                            Height = menuArrowSize * 0.15,
                            BackgroundColor = "currentcolor",
                            BorderRadius = borderRadius,
                            Transition = new string[]
                            {
                                $"background {motionDurationSlow} {motionEaseInOut}",
                                $"transform {motionDurationSlow} {motionEaseInOut}",
                                $"top {motionDurationSlow} {motionEaseInOut}",
                                $"color {motionDurationSlow} {motionEaseInOut}"
                            }.Join(","),
                            Content = "\"\"",
                        },
                        ["&::before"] = new CSSObject()
                        {
                            Transform = @$"rotate(45deg) translateY(-{menuArrowOffset})",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Transform = @$"rotate(-45deg) translateY({menuArrowOffset})",
                        },
                    },
                },
            };
        }

        public CSSInterpolation[] GetBaseStyle(MenuToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var fontSize = token.FontSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var paddingXS = token.PaddingXS;
            var padding = token.Padding;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var zIndexPopup = token.ZIndexPopup;
            var borderRadiusLG = token.BorderRadiusLG;
            var subMenuItemBorderRadius = token.SubMenuItemBorderRadius;
            var menuArrowSize = token.MenuArrowSize;
            var menuArrowOffset = token.MenuArrowOffset;
            var lineType = token.LineType;
            var menuPanelMaskInset = token.MenuPanelMaskInset;
            var groupTitleLineHeight = token.GroupTitleLineHeight;
            var groupTitleFontSize = token.GroupTitleFontSize;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [""] = new CSSObject()
                    {
                        [$"{componentCls}"] = new CSSObject()
                        {
                            ["..."] = ClearFix(),
                            ["&-hidden"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                    },
                    [$"{componentCls}-submenu-hidden"] = new CSSObject()
                    {
                        Display = "none",
                    },
                },
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        ["..."] = ClearFix(),
                        MarginBottom = 0,
                        PaddingInlineStart = 0,
                        FontSize = fontSize,
                        LineHeight = 0,
                        ListStyle = "none",
                        Outline = "none",
                        Transition = @$"width {motionDurationSlow} cubic-bezier(0.2, 0, 0, 1) 0s",
                        ["ul, ol"] = new CSSObject()
                        {
                            Margin = 0,
                            Padding = 0,
                            ListStyle = "none",
                        },
                        ["&-overflow"] = new CSSObject()
                        {
                            Display = "flex",
                            [$"{componentCls}-item"] = new CSSObject()
                            {
                                Flex = "none",
                            },
                        },
                        [$"{componentCls}-item, {componentCls}-submenu, {componentCls}-submenu-title"] = new CSSObject()
                        {
                            BorderRadius = token.ItemBorderRadius,
                        },
                        [$"{componentCls}-item-group-title"] = new CSSObject()
                        {
                            Padding = @$"{paddingXS}px {padding}px",
                            FontSize = groupTitleFontSize,
                            LineHeight = groupTitleLineHeight,
                            Transition = @$"all {motionDurationSlow}",
                        },
                        [$"&-horizontal {componentCls}-submenu"] = new CSSObject()
                        {
                            Transition = new string[]
                            {
                                $"border-color {motionDurationSlow} {motionEaseInOut}",
                                $"background {motionDurationSlow} {motionEaseInOut}"
                            }.Join(",")
                        },
                        [$"{componentCls}-submenu, {componentCls}-submenu-inline"] = new CSSObject()
                        {
                            Transition = new string[]
                            {
                                $"border-color {motionDurationSlow} {motionEaseInOut}",
                                $"background {motionDurationSlow} {motionEaseInOut}",
                                $"padding {motionDurationMid} {motionEaseInOut}"
                            }.Join(",")
                        },
                        [$"{componentCls}-submenu {componentCls}-sub"] = new CSSObject()
                        {
                            Cursor = "initial",
                            Transition = new string[]
                            {
                                $"background {motionDurationSlow} {motionEaseInOut}",
                                $"padding {motionDurationSlow} {motionEaseInOut}"
                            }.Join(",")
                        },
                        [$"{componentCls}-title-content"] = new CSSObject()
                        {
                            Transition = @$"color {motionDurationSlow}",
                            [$"> {antCls}-typography-ellipsis-single-line"] = new CSSObject()
                            {
                                Display = "inline",
                                VerticalAlign = "unset",
                            },
                        },
                        [$"{componentCls}-item a"] = new CSSObject()
                        {
                            ["&::before"] = new CSSObject()
                            {
                                Position = "absolute",
                                Inset = 0,
                                BackgroundColor = "transparent",
                                Content = "\"\"",
                            },
                        },
                        [$"{componentCls}-item-divider"] = new CSSObject()
                        {
                            Overflow = "hidden",
                            LineHeight = 0,
                            BorderColor = colorSplit,
                            BorderStyle = lineType,
                            BorderWidth = 0,
                            BorderTopWidth = lineWidth,
                            MarginBlock = lineWidth,
                            Padding = 0,
                            ["&-dashed"] = new CSSObject()
                            {
                                BorderStyle = "dashed",
                            },
                        },
                        ["..."] = GenMenuItemStyle(token),
                        [$"{componentCls}-item-group"] = new CSSObject()
                        {
                            [$"{componentCls}-item-group-list"] = new CSSObject()
                            {
                                Margin = 0,
                                Padding = 0,
                                [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                                {
                                    PaddingInline = @$"{fontSize * 2}px {padding}px",
                                },
                            },
                        },
                        ["&-submenu"] = new CSSObject()
                        {
                            ["&-popup"] = new CSSObject()
                            {
                                Position = "absolute",
                                ZIndex = zIndexPopup,
                                BorderRadius = borderRadiusLG,
                                BoxShadow = "none",
                                TransformOrigin = "0 0",
                                [$"&{componentCls}-submenu"] = new CSSObject()
                                {
                                    Background = "transparent",
                                },
                                ["&::before"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Inset = @$"{menuPanelMaskInset}px 0 0",
                                    ZIndex = -1,
                                    Width = "100%",
                                    Height = "100%",
                                    Opacity = 0,
                                    Content = "\"\"",
                                },
                            },
                            ["&-placement-rightTop::before"] = new CSSObject()
                            {
                                Top = 0,
                                InsetInlineStart = menuPanelMaskInset,
                            },
                            ["&-placement-leftTop,&-placement-bottomRight,"] = new CSSObject()
                            {
                                TransformOrigin = "100% 0",
                            },
                            ["&-placement-leftBottom,&-placement-topRight,"] = new CSSObject()
                            {
                                TransformOrigin = "100% 100%",
                            },
                            ["&-placement-rightBottom,&-placement-topLeft,"] = new CSSObject()
                            {
                                TransformOrigin = "0 100%",
                            },
                            ["&-placement-bottomLeft,&-placement-rightTop,"] = new CSSObject()
                            {
                                TransformOrigin = "0 0",
                            },
                            ["&-placement-leftTop,&-placement-leftBottom"] = new CSSObject()
                            {
                                PaddingInlineEnd = token.PaddingXS,
                            },
                            ["&-placement-rightTop,&-placement-rightBottom"] = new CSSObject()
                            {
                                PaddingInlineStart = token.PaddingXS,
                            },
                            ["&-placement-topRight,&-placement-topLeft"] = new CSSObject()
                            {
                                PaddingBottom = token.PaddingXS,
                            },
                            ["&-placement-bottomRight,&-placement-bottomLeft"] = new CSSObject()
                            {
                                PaddingTop = token.PaddingXS,
                            },
                            [$"> {componentCls}"] = new CSSObject()
                            {
                                BorderRadius = borderRadiusLG,
                                ["..."] = GenMenuItemStyle(token),
                                ["..."] = GenSubMenuArrowStyle(token),
                                [$"{componentCls}-item, {componentCls}-submenu > {componentCls}-submenu-title"] = new CSSObject()
                                {
                                    BorderRadius = subMenuItemBorderRadius,
                                },
                                [$"{componentCls}-submenu-title::after"] = new CSSObject()
                                {
                                    Transition = @$"transform {motionDurationSlow} {motionEaseInOut}",
                                },
                            },
                        },
                        ["..."] = GenSubMenuArrowStyle(token),
                        [$"&-inline-collapsed{componentCls}-submenu-arrow,&-inline{componentCls}-submenu-arrow"] = new CSSObject()
                        {
                            ["&::before"] = new CSSObject()
                            {
                                Transform = @$"rotate(-45deg) translateX({menuArrowOffset})",
                            },
                            ["&::after"] = new CSSObject()
                            {
                                Transform = @$"rotate(45deg) translateX(-{menuArrowOffset})",
                            },
                        },
                        [$"{componentCls}-submenu-open{componentCls}-submenu-inline > {componentCls}-submenu-title > {componentCls}-submenu-arrow"] = new CSSObject()
                        {
                            Transform = @$"translateY(-{menuArrowSize * 0.2}px)",
                            ["&::after"] = new CSSObject()
                            {
                                Transform = @$"rotate(-45deg) translateX(-{menuArrowOffset})",
                            },
                            ["&::before"] = new CSSObject()
                            {
                                Transform = @$"rotate(45deg) translateX({menuArrowOffset})",
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{antCls}-layout-header"] = new CSSObject()
                    {
                        [componentCls] = new CSSObject()
                        {
                            LineHeight = "inherit",
                        },
                    },
                },
            };
        }

    }

}