using System;
using CssInCSharp;
using CssInCSharp.Colors;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.CollapseMotion;
using static AntDesign.Slide;
using static AntDesign.Zoom;

namespace AntDesign
{
    public partial class MenuToken
    {
        public double DropdownWidth
        {
            get => (double)_tokens["dropdownWidth"];
            set => _tokens["dropdownWidth"] = value;
        }

        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public string ColorGroupTitle
        {
            get => (string)_tokens["colorGroupTitle"];
            set => _tokens["colorGroupTitle"] = value;
        }

        public string GroupTitleColor
        {
            get => (string)_tokens["groupTitleColor"];
            set => _tokens["groupTitleColor"] = value;
        }

        public double GroupTitleLineHeight
        {
            get => (double)_tokens["groupTitleLineHeight"];
            set => _tokens["groupTitleLineHeight"] = value;
        }

        public double GroupTitleFontSize
        {
            get => (double)_tokens["groupTitleFontSize"];
            set => _tokens["groupTitleFontSize"] = value;
        }

        public double RadiusItem
        {
            get => (double)_tokens["radiusItem"];
            set => _tokens["radiusItem"] = value;
        }

        public double ItemBorderRadius
        {
            get => (double)_tokens["itemBorderRadius"];
            set => _tokens["itemBorderRadius"] = value;
        }

        public double RadiusSubMenuItem
        {
            get => (double)_tokens["radiusSubMenuItem"];
            set => _tokens["radiusSubMenuItem"] = value;
        }

        public double SubMenuItemBorderRadius
        {
            get => (double)_tokens["subMenuItemBorderRadius"];
            set => _tokens["subMenuItemBorderRadius"] = value;
        }

        public string ColorItemText
        {
            get => (string)_tokens["colorItemText"];
            set => _tokens["colorItemText"] = value;
        }

        public string ItemColor
        {
            get => (string)_tokens["itemColor"];
            set => _tokens["itemColor"] = value;
        }

        public string ColorItemTextHover
        {
            get => (string)_tokens["colorItemTextHover"];
            set => _tokens["colorItemTextHover"] = value;
        }

        public string ItemHoverColor
        {
            get => (string)_tokens["itemHoverColor"];
            set => _tokens["itemHoverColor"] = value;
        }

        public string ColorItemTextHoverHorizontal
        {
            get => (string)_tokens["colorItemTextHoverHorizontal"];
            set => _tokens["colorItemTextHoverHorizontal"] = value;
        }

        public string HorizontalItemHoverColor
        {
            get => (string)_tokens["horizontalItemHoverColor"];
            set => _tokens["horizontalItemHoverColor"] = value;
        }

        public string ColorItemTextSelected
        {
            get => (string)_tokens["colorItemTextSelected"];
            set => _tokens["colorItemTextSelected"] = value;
        }

        public string ItemSelectedColor
        {
            get => (string)_tokens["itemSelectedColor"];
            set => _tokens["itemSelectedColor"] = value;
        }

        public string ColorItemTextSelectedHorizontal
        {
            get => (string)_tokens["colorItemTextSelectedHorizontal"];
            set => _tokens["colorItemTextSelectedHorizontal"] = value;
        }

        public string HorizontalItemSelectedColor
        {
            get => (string)_tokens["horizontalItemSelectedColor"];
            set => _tokens["horizontalItemSelectedColor"] = value;
        }

        public string ColorItemTextDisabled
        {
            get => (string)_tokens["colorItemTextDisabled"];
            set => _tokens["colorItemTextDisabled"] = value;
        }

        public string ItemDisabledColor
        {
            get => (string)_tokens["itemDisabledColor"];
            set => _tokens["itemDisabledColor"] = value;
        }

        public string ColorDangerItemText
        {
            get => (string)_tokens["colorDangerItemText"];
            set => _tokens["colorDangerItemText"] = value;
        }

        public string DangerItemColor
        {
            get => (string)_tokens["dangerItemColor"];
            set => _tokens["dangerItemColor"] = value;
        }

        public string ColorDangerItemTextHover
        {
            get => (string)_tokens["colorDangerItemTextHover"];
            set => _tokens["colorDangerItemTextHover"] = value;
        }

        public string DangerItemHoverColor
        {
            get => (string)_tokens["dangerItemHoverColor"];
            set => _tokens["dangerItemHoverColor"] = value;
        }

        public string ColorDangerItemTextSelected
        {
            get => (string)_tokens["colorDangerItemTextSelected"];
            set => _tokens["colorDangerItemTextSelected"] = value;
        }

        public string DangerItemSelectedColor
        {
            get => (string)_tokens["dangerItemSelectedColor"];
            set => _tokens["dangerItemSelectedColor"] = value;
        }

        public string ColorDangerItemBgActive
        {
            get => (string)_tokens["colorDangerItemBgActive"];
            set => _tokens["colorDangerItemBgActive"] = value;
        }

        public string DangerItemActiveBg
        {
            get => (string)_tokens["dangerItemActiveBg"];
            set => _tokens["dangerItemActiveBg"] = value;
        }

        public string ColorDangerItemBgSelected
        {
            get => (string)_tokens["colorDangerItemBgSelected"];
            set => _tokens["colorDangerItemBgSelected"] = value;
        }

        public string DangerItemSelectedBg
        {
            get => (string)_tokens["dangerItemSelectedBg"];
            set => _tokens["dangerItemSelectedBg"] = value;
        }

        public string ColorItemBg
        {
            get => (string)_tokens["colorItemBg"];
            set => _tokens["colorItemBg"] = value;
        }

        public string ItemBg
        {
            get => (string)_tokens["itemBg"];
            set => _tokens["itemBg"] = value;
        }

        public string ColorItemBgHover
        {
            get => (string)_tokens["colorItemBgHover"];
            set => _tokens["colorItemBgHover"] = value;
        }

        public string ItemHoverBg
        {
            get => (string)_tokens["itemHoverBg"];
            set => _tokens["itemHoverBg"] = value;
        }

        public string ColorSubItemBg
        {
            get => (string)_tokens["colorSubItemBg"];
            set => _tokens["colorSubItemBg"] = value;
        }

        public string SubMenuItemBg
        {
            get => (string)_tokens["subMenuItemBg"];
            set => _tokens["subMenuItemBg"] = value;
        }

        public string ColorItemBgActive
        {
            get => (string)_tokens["colorItemBgActive"];
            set => _tokens["colorItemBgActive"] = value;
        }

        public string ItemActiveBg
        {
            get => (string)_tokens["itemActiveBg"];
            set => _tokens["itemActiveBg"] = value;
        }

        public string ColorItemBgSelected
        {
            get => (string)_tokens["colorItemBgSelected"];
            set => _tokens["colorItemBgSelected"] = value;
        }

        public string ItemSelectedBg
        {
            get => (string)_tokens["itemSelectedBg"];
            set => _tokens["itemSelectedBg"] = value;
        }

        public string ColorItemBgSelectedHorizontal
        {
            get => (string)_tokens["colorItemBgSelectedHorizontal"];
            set => _tokens["colorItemBgSelectedHorizontal"] = value;
        }

        public string HorizontalItemSelectedBg
        {
            get => (string)_tokens["horizontalItemSelectedBg"];
            set => _tokens["horizontalItemSelectedBg"] = value;
        }

        public double ColorActiveBarWidth
        {
            get => (double)_tokens["colorActiveBarWidth"];
            set => _tokens["colorActiveBarWidth"] = value;
        }

        public double ActiveBarWidth
        {
            get => (double)_tokens["activeBarWidth"];
            set => _tokens["activeBarWidth"] = value;
        }

        public double ColorActiveBarHeight
        {
            get => (double)_tokens["colorActiveBarHeight"];
            set => _tokens["colorActiveBarHeight"] = value;
        }

        public double ActiveBarHeight
        {
            get => (double)_tokens["activeBarHeight"];
            set => _tokens["activeBarHeight"] = value;
        }

        public double ColorActiveBarBorderSize
        {
            get => (double)_tokens["colorActiveBarBorderSize"];
            set => _tokens["colorActiveBarBorderSize"] = value;
        }

        public double ActiveBarBorderWidth
        {
            get => (double)_tokens["activeBarBorderWidth"];
            set => _tokens["activeBarBorderWidth"] = value;
        }

        public double ItemMarginInline
        {
            get => (double)_tokens["itemMarginInline"];
            set => _tokens["itemMarginInline"] = value;
        }

        public string HorizontalItemHoverBg
        {
            get => (string)_tokens["horizontalItemHoverBg"];
            set => _tokens["horizontalItemHoverBg"] = value;
        }

        public double HorizontalItemBorderRadius
        {
            get => (double)_tokens["horizontalItemBorderRadius"];
            set => _tokens["horizontalItemBorderRadius"] = value;
        }

        public double ItemHeight
        {
            get => (double)_tokens["itemHeight"];
            set => _tokens["itemHeight"] = value;
        }

        public double CollapsedWidth
        {
            get => (double)_tokens["collapsedWidth"];
            set => _tokens["collapsedWidth"] = value;
        }

        public string PopupBg
        {
            get => (string)_tokens["popupBg"];
            set => _tokens["popupBg"] = value;
        }

        public double ItemMarginBlock
        {
            get => (double)_tokens["itemMarginBlock"];
            set => _tokens["itemMarginBlock"] = value;
        }

        public double ItemPaddingInline
        {
            get => (double)_tokens["itemPaddingInline"];
            set => _tokens["itemPaddingInline"] = value;
        }

        public string HorizontalLineHeight
        {
            get => (string)_tokens["horizontalLineHeight"];
            set => _tokens["horizontalLineHeight"] = value;
        }

        public double IconMarginInlineEnd
        {
            get => (double)_tokens["iconMarginInlineEnd"];
            set => _tokens["iconMarginInlineEnd"] = value;
        }

        public double IconSize
        {
            get => (double)_tokens["iconSize"];
            set => _tokens["iconSize"] = value;
        }

        public double CollapsedIconSize
        {
            get => (double)_tokens["collapsedIconSize"];
            set => _tokens["collapsedIconSize"] = value;
        }

        public string DarkItemColor
        {
            get => (string)_tokens["darkItemColor"];
            set => _tokens["darkItemColor"] = value;
        }

        public string DarkDangerItemColor
        {
            get => (string)_tokens["darkDangerItemColor"];
            set => _tokens["darkDangerItemColor"] = value;
        }

        public string DarkItemBg
        {
            get => (string)_tokens["darkItemBg"];
            set => _tokens["darkItemBg"] = value;
        }

        public string DarkSubMenuItemBg
        {
            get => (string)_tokens["darkSubMenuItemBg"];
            set => _tokens["darkSubMenuItemBg"] = value;
        }

        public string DarkItemSelectedColor
        {
            get => (string)_tokens["darkItemSelectedColor"];
            set => _tokens["darkItemSelectedColor"] = value;
        }

        public string DarkItemSelectedBg
        {
            get => (string)_tokens["darkItemSelectedBg"];
            set => _tokens["darkItemSelectedBg"] = value;
        }

        public string DarkItemHoverBg
        {
            get => (string)_tokens["darkItemHoverBg"];
            set => _tokens["darkItemHoverBg"] = value;
        }

        public string DarkGroupTitleColor
        {
            get => (string)_tokens["darkGroupTitleColor"];
            set => _tokens["darkGroupTitleColor"] = value;
        }

        public string DarkItemHoverColor
        {
            get => (string)_tokens["darkItemHoverColor"];
            set => _tokens["darkItemHoverColor"] = value;
        }

        public string DarkItemDisabledColor
        {
            get => (string)_tokens["darkItemDisabledColor"];
            set => _tokens["darkItemDisabledColor"] = value;
        }

        public string DarkDangerItemSelectedBg
        {
            get => (string)_tokens["darkDangerItemSelectedBg"];
            set => _tokens["darkDangerItemSelectedBg"] = value;
        }

        public string DarkDangerItemHoverColor
        {
            get => (string)_tokens["darkDangerItemHoverColor"];
            set => _tokens["darkDangerItemHoverColor"] = value;
        }

        public string DarkDangerItemSelectedColor
        {
            get => (string)_tokens["darkDangerItemSelectedColor"];
            set => _tokens["darkDangerItemSelectedColor"] = value;
        }

        public string DarkDangerItemActiveBg
        {
            get => (string)_tokens["darkDangerItemActiveBg"];
            set => _tokens["darkDangerItemActiveBg"] = value;
        }

    }

    public partial class MenuToken : TokenWithCommonCls
    {
        public double MenuHorizontalHeight
        {
            get => (double)_tokens["menuHorizontalHeight"];
            set => _tokens["menuHorizontalHeight"] = value;
        }

        public double MenuArrowSize
        {
            get => (double)_tokens["menuArrowSize"];
            set => _tokens["menuArrowSize"] = value;
        }

        public string MenuArrowOffset
        {
            get => (string)_tokens["menuArrowOffset"];
            set => _tokens["menuArrowOffset"] = value;
        }

        public double MenuPanelMaskInset
        {
            get => (double)_tokens["menuPanelMaskInset"];
            set => _tokens["menuPanelMaskInset"] = value;
        }

        public string MenuSubMenuBg
        {
            get => (string)_tokens["menuSubMenuBg"];
            set => _tokens["menuSubMenuBg"] = value;
        }

    }

    public partial class MenuStyle
    {
        public static CSSObject GenMenuItemStyle(MenuToken token)
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

        public static CSSObject GenSubMenuArrowStyle(MenuToken token)
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

        public static CSSInterpolation[] GetBaseStyle(MenuToken token)
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
                        [@$"&-inline-collapsed {componentCls}-submenu-arrow,
                        &-inline {componentCls}-submenu-arrow"] = new CSSObject()
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

        public static UseComponentStyleResult UseComponentStyle()
        {
            var useOriginHook = GenComponentStyleHook(
                "Menu",
                (token) =>
                {
                    var colorBgElevated = token.ColorBgElevated;
                    var colorPrimary = token.ColorPrimary;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var controlHeightLG = token.ControlHeightLG;
                    var fontSize = token.FontSize;
                    var darkItemColor = token.DarkItemColor;
                    var darkDangerItemColor = token.DarkDangerItemColor;
                    var darkItemBg = token.DarkItemBg;
                    var darkSubMenuItemBg = token.DarkSubMenuItemBg;
                    var darkItemSelectedColor = token.DarkItemSelectedColor;
                    var darkItemSelectedBg = token.DarkItemSelectedBg;
                    var darkDangerItemSelectedBg = token.DarkDangerItemSelectedBg;
                    var darkItemHoverBg = token.DarkItemHoverBg;
                    var darkGroupTitleColor = token.DarkGroupTitleColor;
                    var darkItemHoverColor = token.DarkItemHoverColor;
                    var darkItemDisabledColor = token.DarkItemDisabledColor;
                    var darkDangerItemHoverColor = token.DarkDangerItemHoverColor;
                    var darkDangerItemSelectedColor = token.DarkDangerItemSelectedColor;
                    var darkDangerItemActiveBg = token.DarkDangerItemActiveBg;
                    var menuArrowSize = (fontSize / 7) * 5;
                    var menuToken = MergeToken(
                        token,
                        new MenuToken()
                        {
                            MenuArrowSize = menuArrowSize,
                            MenuHorizontalHeight = controlHeightLG * 1.15,
                            MenuArrowOffset = @$"{menuArrowSize * 0.25}px",
                            MenuPanelMaskInset = -7,
                            MenuSubMenuBg = colorBgElevated,
                        });
                    var menuDarkToken = MergeToken(
                        menuToken,
                        new MenuToken()
                        {
                            ItemColor = darkItemColor,
                            ItemHoverColor = darkItemHoverColor,
                            GroupTitleColor = darkGroupTitleColor,
                            ItemSelectedColor = darkItemSelectedColor,
                            ItemBg = darkItemBg,
                            PopupBg = darkItemBg,
                            SubMenuItemBg = darkSubMenuItemBg,
                            ItemActiveBg = "transparent",
                            ItemSelectedBg = darkItemSelectedBg,
                            ActiveBarHeight = 0,
                            ActiveBarBorderWidth = 0,
                            ItemHoverBg = darkItemHoverBg,
                            ItemDisabledColor = darkItemDisabledColor,
                            DangerItemColor = darkDangerItemColor,
                            DangerItemHoverColor = darkDangerItemHoverColor,
                            DangerItemSelectedColor = darkDangerItemSelectedColor,
                            DangerItemActiveBg = darkDangerItemActiveBg,
                            DangerItemSelectedBg = darkDangerItemSelectedBg,
                            MenuSubMenuBg = darkSubMenuItemBg,
                            HorizontalItemSelectedColor = colorTextLightSolid,
                            HorizontalItemSelectedBg = colorPrimary,
                        });
                    return new CSSInterpolation[]
                    {
                        GetBaseStyle(menuToken),
                        GetHorizontalStyle(menuToken),
                        GetVerticalStyle(menuToken),
                        GetThemeStyle(menuToken, "light"),
                        GetThemeStyle(menuDarkToken, "dark"),
                        GetRTLStyle(menuToken),
                        GenCollapseMotion(menuToken),
                        InitSlideMotion(menuToken, "slide-up"),
                        InitSlideMotion(menuToken, "slide-down"),
                        InitZoomMotion(menuToken, "zoom-big"),
                    };
                },
                (token) =>
                {
                    var colorPrimary = token.ColorPrimary;
                    var colorError = token.ColorError;
                    var colorTextDisabled = token.ColorTextDisabled;
                    var colorErrorBg = token.ColorErrorBg;
                    var colorText = token.ColorText;
                    var colorTextDescription = token.ColorTextDescription;
                    var colorBgContainer = token.ColorBgContainer;
                    var colorFillAlter = token.ColorFillAlter;
                    var colorFillContent = token.ColorFillContent;
                    var lineWidth = token.LineWidth;
                    var lineWidthBold = token.LineWidthBold;
                    var controlItemBgActive = token.ControlItemBgActive;
                    var colorBgTextHover = token.ColorBgTextHover;
                    var controlHeightLG = token.ControlHeightLG;
                    var lineHeight = token.LineHeight;
                    var colorBgElevated = token.ColorBgElevated;
                    var marginXXS = token.MarginXXS;
                    var padding = token.Padding;
                    var fontSize = token.FontSize;
                    var controlHeightSM = token.ControlHeightSM;
                    var fontSizeLG = token.FontSizeLG;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorErrorHover = token.ColorErrorHover;
                    var colorTextDark = new TinyColor(colorTextLightSolid).SetAlpha(0.65).ToRgbString();
                    return new MenuToken()
                    {
                        DropdownWidth = 160,
                        ZIndexPopup = token.ZIndexPopupBase + 50,
                        RadiusItem = token.BorderRadiusLG,
                        ItemBorderRadius = token.BorderRadiusLG,
                        RadiusSubMenuItem = token.BorderRadiusSM,
                        SubMenuItemBorderRadius = token.BorderRadiusSM,
                        ColorItemText = colorText,
                        ItemColor = colorText,
                        ColorItemTextHover = colorText,
                        ItemHoverColor = colorText,
                        ColorItemTextHoverHorizontal = colorPrimary,
                        HorizontalItemHoverColor = colorPrimary,
                        ColorGroupTitle = colorTextDescription,
                        GroupTitleColor = colorTextDescription,
                        ColorItemTextSelected = colorPrimary,
                        ItemSelectedColor = colorPrimary,
                        ColorItemTextSelectedHorizontal = colorPrimary,
                        HorizontalItemSelectedColor = colorPrimary,
                        ColorItemBg = colorBgContainer,
                        ItemBg = colorBgContainer,
                        ColorItemBgHover = colorBgTextHover,
                        ItemHoverBg = colorBgTextHover,
                        ColorItemBgActive = colorFillContent,
                        ItemActiveBg = controlItemBgActive,
                        ColorSubItemBg = colorFillAlter,
                        SubMenuItemBg = colorFillAlter,
                        ColorItemBgSelected = controlItemBgActive,
                        ItemSelectedBg = controlItemBgActive,
                        ColorItemBgSelectedHorizontal = "transparent",
                        HorizontalItemSelectedBg = "transparent",
                        ColorActiveBarWidth = 0,
                        ActiveBarWidth = 0,
                        ColorActiveBarHeight = lineWidthBold,
                        ActiveBarHeight = lineWidthBold,
                        ColorActiveBarBorderSize = lineWidth,
                        ActiveBarBorderWidth = lineWidth,
                        ColorItemTextDisabled = colorTextDisabled,
                        ItemDisabledColor = colorTextDisabled,
                        ColorDangerItemText = colorError,
                        DangerItemColor = colorError,
                        ColorDangerItemTextHover = colorError,
                        DangerItemHoverColor = colorError,
                        ColorDangerItemTextSelected = colorError,
                        DangerItemSelectedColor = colorError,
                        ColorDangerItemBgActive = colorErrorBg,
                        DangerItemActiveBg = colorErrorBg,
                        ColorDangerItemBgSelected = colorErrorBg,
                        DangerItemSelectedBg = colorErrorBg,
                        ItemMarginInline = token.MarginXXS,
                        HorizontalItemBorderRadius = 0,
                        HorizontalItemHoverBg = "transparent",
                        ItemHeight = controlHeightLG,
                        GroupTitleLineHeight = lineHeight,
                        CollapsedWidth = controlHeightLG * 2,
                        PopupBg = colorBgElevated,
                        ItemMarginBlock = marginXXS,
                        ItemPaddingInline = padding,
                        HorizontalLineHeight = @$"{controlHeightLG * 1.15}px",
                        IconSize = fontSize,
                        IconMarginInlineEnd = controlHeightSM - fontSize,
                        CollapsedIconSize = fontSizeLG,
                        GroupTitleFontSize = fontSize,
                        DarkItemDisabledColor = new TinyColor(colorTextLightSolid).SetAlpha(0.25).ToRgbString(),
                        DarkItemColor = colorTextDark,
                        DarkDangerItemColor = colorError,
                        DarkItemBg = "#001529",
                        DarkSubMenuItemBg = "#000c17",
                        DarkItemSelectedColor = colorTextLightSolid,
                        DarkItemSelectedBg = colorPrimary,
                        DarkDangerItemSelectedBg = colorError,
                        DarkItemHoverBg = "transparent",
                        DarkGroupTitleColor = colorTextDark,
                        DarkItemHoverColor = colorTextLightSolid,
                        DarkDangerItemHoverColor = colorErrorHover,
                        DarkDangerItemSelectedColor = colorTextLightSolid,
                        DarkDangerItemActiveBg = colorError,
                    };
                },
                new GenOptions()
                {
                    DeprecatedTokens = new ()
                    {
                        ("colorGroupTitle", "groupTitleColor"),
                        ("radiusItem", "itemBorderRadius"),
                        ("radiusSubMenuItem", "subMenuItemBorderRadius"),
                        ("colorItemText", "itemColor"),
                        ("colorItemTextHover", "itemHoverColor"),
                        ("colorItemTextHoverHorizontal", "horizontalItemHoverColor"),
                        ("colorItemTextSelected", "itemSelectedColor"),
                        ("colorItemTextSelectedHorizontal", "horizontalItemSelectedColor"),
                        ("colorItemTextDisabled", "itemDisabledColor"),
                        ("colorDangerItemText", "dangerItemColor"),
                        ("colorDangerItemTextHover", "dangerItemHoverColor"),
                        ("colorDangerItemTextSelected", "dangerItemSelectedColor"),
                        ("colorDangerItemBgActive", "dangerItemActiveBg"),
                        ("colorDangerItemBgSelected", "dangerItemSelectedBg"),
                        ("colorItemBg", "itemBg"),
                        ("colorItemBgHover", "itemHoverBg"),
                        ("colorSubItemBg", "subMenuItemBg"),
                        ("colorItemBgActive", "itemActiveBg"),
                        ("colorItemBgSelectedHorizontal", "horizontalItemSelectedBg"),
                        ("colorActiveBarWidth", "activeBarWidth"),
                        ("colorActiveBarHeight", "activeBarHeight"),
                        ("colorActiveBarBorderSize", "activeBarBorderWidth"),
                        ("colorItemBgSelected", "itemSelectedBg"),
                    }
                });
            return useOriginHook;
        }

        public static CSSObject GetHorizontalStyle(MenuToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var horizontalLineHeight = token.HorizontalLineHeight;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var itemPaddingInline = token.ItemPaddingInline;
            return new CSSObject()
            {
                [$"{componentCls}-horizontal"] = new CSSObject()
                {
                    LineHeight = horizontalLineHeight,
                    Border = 0,
                    BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                    BoxShadow = "none",
                    ["&::after"] = new CSSObject()
                    {
                        Display = "block",
                        Clear = "both",
                        Height = 0,
                        Content = "\"\\20\"",
                    },
                    [$"{componentCls}-item, {componentCls}-submenu"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "inline-block",
                        VerticalAlign = "bottom",
                        PaddingInline = itemPaddingInline,
                    },
                    [@$"> {componentCls}-item:hover,
                        > {componentCls}-item-active,
                        > {componentCls}-submenu {componentCls}-submenu-title:hover"] = new CSSObject()
                    {
                        BackgroundColor = "transparent",
                    },
                    [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                    {
                        Transition = new string[]
                        {
                            $"border-color {motionDurationSlow}",
                            $"background {motionDurationSlow}"
                        }.Join(",")
                    },
                    [$"{componentCls}-submenu-arrow"] = new CSSObject()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public static CSSObject GetVerticalInlineStyle(MenuToken token)
        {
            var componentCls = token.ComponentCls;
            var itemHeight = token.ItemHeight;
            var itemMarginInline = token.ItemMarginInline;
            var padding = token.Padding;
            var menuArrowSize = token.MenuArrowSize;
            var marginXS = token.MarginXS;
            var itemMarginBlock = token.ItemMarginBlock;
            var paddingWithArrow = padding + menuArrowSize + marginXS;
            return new CSSObject()
            {
                [$"{componentCls}-item"] = new CSSObject()
                {
                    Position = "relative",
                    Overflow = "hidden",
                },
                [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                {
                    Height = itemHeight,
                    LineHeight = @$"{itemHeight}px",
                    PaddingInline = padding,
                    Overflow = "hidden",
                    TextOverflow = "ellipsis",
                    MarginInline = itemMarginInline,
                    MarginBlock = itemMarginBlock,
                    Width = @$"calc(100% - {itemMarginInline * 2}px)",
                },
                [$">{componentCls}-item,>{componentCls}-submenu>{componentCls}-submenu-title"] = new CSSObject()
                {
                    Height = itemHeight,
                    LineHeight = @$"{itemHeight}px",
                },
                [$"{componentCls}-item-group-list {componentCls}-submenu-title,{componentCls}-submenu-title"] = new CSSObject()
                {
                    PaddingInlineEnd = paddingWithArrow,
                },
            };
        }

        public static CSSInterpolation[] GetVerticalStyle(MenuToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var itemHeight = token.ItemHeight;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var dropdownWidth = token.DropdownWidth;
            var controlHeightLG = token.ControlHeightLG;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseOut = token.MotionEaseOut;
            var paddingXL = token.PaddingXL;
            var itemMarginInline = token.ItemMarginInline;
            var fontSizeLG = token.FontSizeLG;
            var motionDurationSlow = token.MotionDurationSlow;
            var paddingXS = token.PaddingXS;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var collapsedWidth = token.CollapsedWidth;
            var collapsedIconSize = token.CollapsedIconSize;
            var inlineItemStyle = new CSSObject()
            {
                Height = itemHeight,
                LineHeight = @$"{itemHeight}px",
                ListStylePosition = "inside",
                ListStyleType = "disc",
            };
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["&-inline, &-vertical"] = new CSSObject()
                        {
                            [$"&{componentCls}-root"] = new CSSObject()
                            {
                                BoxShadow = "none",
                            },
                            ["..."] = GetVerticalInlineStyle(token)
                        },
                    },
                    [$"{componentCls}-submenu-popup"] = new CSSObject()
                    {
                        [$"{componentCls}-vertical"] = new CSSObject()
                        {
                            ["..."] = GetVerticalInlineStyle(token),
                            BoxShadow = boxShadowSecondary,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-submenu-popup {componentCls}-vertical{componentCls}-sub"] = new CSSObject()
                    {
                        MinWidth = dropdownWidth,
                        MaxHeight = @$"calc(100vh - {controlHeightLG * 2.5}px)",
                        Padding = "0",
                        Overflow = "hidden",
                        BorderInlineEnd = 0,
                        ["&:not([class*='-active'])"] = new CSSObject()
                        {
                            OverflowX = "hidden",
                            OverflowY = "auto",
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-inline"] = new CSSObject()
                    {
                        Width = "100%",
                        [$"&{componentCls}-root"] = new CSSObject()
                        {
                            [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                            {
                                Display = "flex",
                                AlignItems = "center",
                                Transition = new string[]
                                {
                                    $"border-color {motionDurationSlow}",
                                    $"background {motionDurationSlow}",
                                    $"padding {motionDurationMid} {motionEaseOut}"
                                }.Join(","),
                                [$"> {componentCls}-title-content"] = new CSSObject()
                                {
                                    Flex = "auto",
                                    MinWidth = 0,
                                    Overflow = "hidden",
                                    TextOverflow = "ellipsis",
                                },
                                ["> *"] = new CSSObject()
                                {
                                    Flex = "none",
                                },
                            },
                        },
                        [$"{componentCls}-sub{componentCls}-inline"] = new CSSObject()
                        {
                            Padding = 0,
                            Border = 0,
                            BorderRadius = 0,
                            BoxShadow = "none",
                            [$"& > {componentCls}-submenu > {componentCls}-submenu-title"] = inlineItemStyle,
                            [$"& {componentCls}-item-group-title"] = new CSSObject()
                            {
                                PaddingInlineStart = paddingXL,
                            },
                        },
                        [$"{componentCls}-item"] = inlineItemStyle,
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-inline-collapsed"] = new CSSObject()
                    {
                        Width = collapsedWidth,
                        [$"&{componentCls}-root"] = new CSSObject()
                        {
                            [$"{componentCls}-item, {componentCls}-submenu {componentCls}-submenu-title"] = new CSSObject()
                            {
                                [$"> {componentCls}-inline-collapsed-noicon"] = new CSSObject()
                                {
                                    FontSize = fontSizeLG,
                                    TextAlign = "center",
                                },
                            },
                        },
                        [$">{componentCls}-item,>{componentCls}-item-group>{componentCls}-item-group-list>{componentCls}-item,>{componentCls}-item-group>{componentCls}-item-group-list>{componentCls}-submenu>{componentCls}-submenu-title,>{componentCls}-submenu>{componentCls}-submenu-title"] = new CSSObject()
                        {
                            InsetInlineStart = 0,
                            PaddingInline = @$"calc(50% - {fontSizeLG / 2}px - {itemMarginInline}px)",
                            TextOverflow = "clip",
                            [$"{componentCls}-submenu-arrow,{componentCls}-submenu-expand-icon"] = new CSSObject()
                            {
                                Opacity = 0,
                            },
                            [$"{componentCls}-item-icon, {iconCls}"] = new CSSObject()
                            {
                                Margin = 0,
                                FontSize = collapsedIconSize,
                                LineHeight = @$"{itemHeight}px",
                                ["+ span"] = new CSSObject()
                                {
                                    Display = "inline-block",
                                    Opacity = 0,
                                },
                            },
                        },
                        [$"{componentCls}-item-icon, {iconCls}"] = new CSSObject()
                        {
                            Display = "inline-block",
                        },
                        ["&-tooltip"] = new CSSObject()
                        {
                            PointerEvents = "none",
                            [$"{componentCls}-item-icon, {iconCls}"] = new CSSObject()
                            {
                                Display = "none",
                            },
                            ["a, a:hover"] = new CSSObject()
                            {
                                Color = colorTextLightSolid,
                            },
                        },
                        [$"{componentCls}-item-group-title"] = new CSSObject()
                        {
                            ["..."] = TextEllipsis,
                            PaddingInline = paddingXS,
                        },
                    },
                },
            };
        }

        public static CSSObject GetRTLStyle(MenuToken args)
        {
            var componentCls = args.ComponentCls;
            var menuArrowOffset = args.MenuArrowOffset;
            return new CSSObject()
            {
                [$"{componentCls}-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
                [$"{componentCls}-submenu-rtl"] = new CSSObject()
                {
                    TransformOrigin = "100% 0",
                },
                [@$"{componentCls}-rtl{componentCls}-vertical,
                    {componentCls}-submenu-rtl {componentCls}-vertical"] = new CSSObject()
                {
                    [$"{componentCls}-submenu-arrow"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Transform = @$"rotate(-45deg) translateY(-{menuArrowOffset})",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Transform = @$"rotate(45deg) translateY({menuArrowOffset})",
                        },
                    },
                },
            };
        }

        public static CSSObject AccessibilityFocus(MenuToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenFocusOutline(token)
            };
        }

        public static CSSObject GetThemeStyle(MenuToken token, string themeSuffix)
        {
            var componentCls = token.ComponentCls;
            var itemColor = token.ItemColor;
            var itemSelectedColor = token.ItemSelectedColor;
            var groupTitleColor = token.GroupTitleColor;
            var itemBg = token.ItemBg;
            var subMenuItemBg = token.SubMenuItemBg;
            var itemSelectedBg = token.ItemSelectedBg;
            var activeBarHeight = token.ActiveBarHeight;
            var activeBarWidth = token.ActiveBarWidth;
            var activeBarBorderWidth = token.ActiveBarBorderWidth;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionEaseInOut = token.MotionEaseInOut;
            var motionEaseOut = token.MotionEaseOut;
            var itemPaddingInline = token.ItemPaddingInline;
            var motionDurationMid = token.MotionDurationMid;
            var itemHoverColor = token.ItemHoverColor;
            var lineType = token.LineType;
            var colorSplit = token.ColorSplit;
            var itemDisabledColor = token.ItemDisabledColor;
            var dangerItemColor = token.DangerItemColor;
            var dangerItemHoverColor = token.DangerItemHoverColor;
            var dangerItemSelectedColor = token.DangerItemSelectedColor;
            var dangerItemActiveBg = token.DangerItemActiveBg;
            var dangerItemSelectedBg = token.DangerItemSelectedBg;
            var itemHoverBg = token.ItemHoverBg;
            var itemActiveBg = token.ItemActiveBg;
            var menuSubMenuBg = token.MenuSubMenuBg;
            var horizontalItemSelectedColor = token.HorizontalItemSelectedColor;
            var horizontalItemSelectedBg = token.HorizontalItemSelectedBg;
            var horizontalItemBorderRadius = token.HorizontalItemBorderRadius;
            var horizontalItemHoverBg = token.HorizontalItemHoverBg;
            var popupBg = token.PopupBg;
            return new CSSObject()
            {
                [$"{componentCls}-{themeSuffix}, {componentCls}-{themeSuffix} > {componentCls}"] = new CSSObject()
                {
                    Color = itemColor,
                    Background = itemBg,
                    [$"&{componentCls}-root:focus-visible"] = new CSSObject()
                    {
                        ["..."] = AccessibilityFocus(token)
                    },
                    [$"{componentCls}-item-group-title"] = new CSSObject()
                    {
                        Color = groupTitleColor,
                    },
                    [$"{componentCls}-submenu-selected"] = new CSSObject()
                    {
                        [$"> {componentCls}-submenu-title"] = new CSSObject()
                        {
                            Color = itemSelectedColor,
                        },
                    },
                    [$"{componentCls}-item-disabled, {componentCls}-submenu-disabled"] = new CSSObject()
                    {
                        Color = @$"{itemDisabledColor} !important",
                    },
                    [$"{componentCls}-item:not({componentCls}-item-selected):not({componentCls}-submenu-selected)"] = new CSSObject()
                    {
                        [$"&:hover, > {componentCls}-submenu-title:hover"] = new CSSObject()
                        {
                            Color = itemHoverColor,
                        },
                    },
                    [$"&:not({componentCls}-horizontal)"] = new CSSObject()
                    {
                        [$"{componentCls}-item:not({componentCls}-item-selected)"] = new CSSObject()
                        {
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = itemHoverBg,
                            },
                            ["&:active"] = new CSSObject()
                            {
                                BackgroundColor = itemActiveBg,
                            },
                        },
                        [$"{componentCls}-submenu-title"] = new CSSObject()
                        {
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = itemHoverBg,
                            },
                            ["&:active"] = new CSSObject()
                            {
                                BackgroundColor = itemActiveBg,
                            },
                        },
                    },
                    [$"{componentCls}-item-danger"] = new CSSObject()
                    {
                        Color = dangerItemColor,
                        [$"&{componentCls}-item:hover"] = new CSSObject()
                        {
                            [$"&:not({componentCls}-item-selected):not({componentCls}-submenu-selected)"] = new CSSObject()
                            {
                                Color = dangerItemHoverColor,
                            },
                        },
                        [$"&{componentCls}-item:active"] = new CSSObject()
                        {
                            Background = dangerItemActiveBg,
                        },
                    },
                    [$"{componentCls}-item a"] = new CSSObject()
                    {
                        ["&, &:hover"] = new CSSObject()
                        {
                            Color = "inherit",
                        },
                    },
                    [$"{componentCls}-item-selected"] = new CSSObject()
                    {
                        Color = itemSelectedColor,
                        [$"&{componentCls}-item-danger"] = new CSSObject()
                        {
                            Color = dangerItemSelectedColor,
                        },
                        ["a, a:hover"] = new CSSObject()
                        {
                            Color = "inherit",
                        },
                    },
                    [$"& {componentCls}-item-selected"] = new CSSObject()
                    {
                        BackgroundColor = itemSelectedBg,
                        [$"&{componentCls}-item-danger"] = new CSSObject()
                        {
                            BackgroundColor = dangerItemSelectedBg,
                        },
                    },
                    [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                    {
                        [$"&:not({componentCls}-item-disabled):focus-visible"] = new CSSObject()
                        {
                            ["..."] = AccessibilityFocus(token)
                        },
                    },
                    [$"&{componentCls}-submenu > {componentCls}"] = new CSSObject()
                    {
                        BackgroundColor = menuSubMenuBg,
                    },
                    [$"&{componentCls}-popup > {componentCls}"] = new CSSObject()
                    {
                        BackgroundColor = popupBg,
                    },
                    [$"&{componentCls}-horizontal"] = new CSSObject()
                    {
                        ["..."] = (themeSuffix == "dark"
                        ? new CSSObject()
                        {
                            BorderBottom = 0,
                        }
                        : new CSSObject()
                        {
                        }),
                        [$"> {componentCls}-item, > {componentCls}-submenu"] = new CSSObject()
                        {
                            Top = activeBarBorderWidth,
                            MarginTop = activeBarBorderWidth == 0 ? 0 : -activeBarBorderWidth,
                            MarginBottom = 0,
                            BorderRadius = horizontalItemBorderRadius,
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                InsetInline = itemPaddingInline,
                                Bottom = 0,
                                BorderBottom = @$"{activeBarHeight}px solid transparent",
                                Transition = @$"border-color {motionDurationSlow} {motionEaseInOut}",
                                Content = "\"\"",
                            },
                            ["&:hover, &-active, &-open"] = new CSSObject()
                            {
                                Background = horizontalItemHoverBg,
                                ["&::after"] = new CSSObject()
                                {
                                    BorderBottomWidth = activeBarHeight,
                                    BorderBottomColor = horizontalItemSelectedColor,
                                },
                            },
                            ["&-selected"] = new CSSObject()
                            {
                                Color = horizontalItemSelectedColor,
                                BackgroundColor = horizontalItemSelectedBg,
                                ["&:hover"] = new CSSObject()
                                {
                                    BackgroundColor = horizontalItemSelectedBg,
                                },
                                ["&::after"] = new CSSObject()
                                {
                                    BorderBottomWidth = activeBarHeight,
                                    BorderBottomColor = horizontalItemSelectedColor,
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-root"] = new CSSObject()
                    {
                        [$"&{componentCls}-inline, &{componentCls}-vertical"] = new CSSObject()
                        {
                            BorderInlineEnd = @$"{activeBarBorderWidth}px {lineType} {colorSplit}",
                        },
                    },
                    [$"&{componentCls}-inline"] = new CSSObject()
                    {
                        [$"{componentCls}-sub{componentCls}-inline"] = new CSSObject()
                        {
                            Background = subMenuItemBg,
                        },
                        [$"{componentCls}-item, {componentCls}-submenu-title"] = (activeBarBorderWidth != 0 && activeBarWidth != 0
                        ? new CSSObject()
                        {
                            Width = @$"calc(100% + {activeBarBorderWidth}px)",
                        }
                        : new CSSObject()
                        {
                        }),
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            Position = "relative",
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                InsetBlock = 0,
                                InsetInlineEnd = 0,
                                BorderInlineEnd = @$"{activeBarWidth}px solid {itemSelectedColor}",
                                Transform = "scaleY(0.0001)",
                                Opacity = 0,
                                Transition = new string[]
                                {
                                    $"transform {motionDurationMid} {motionEaseOut}",
                                    $"opacity {motionDurationMid} {motionEaseOut}"
                                }.Join(","),
                                Content = "\"\"",
                            },
                            [$"&{componentCls}-item-danger"] = new CSSObject()
                            {
                                ["&::after"] = new CSSObject()
                                {
                                    BorderInlineEndColor = dangerItemSelectedColor,
                                },
                            },
                        },
                        [$"{componentCls}-selected, {componentCls}-item-selected"] = new CSSObject()
                        {
                            ["&::after"] = new CSSObject()
                            {
                                Transform = "scaleY(1)",
                                Opacity = 1,
                                Transition = new string[]
                                {
                                    $"transform {motionDurationMid} {motionEaseInOut}",
                                    $"opacity {motionDurationMid} {motionEaseInOut}"
                                }.Join(",")
                            },
                        },
                    },
                },
            };
        }

    }

}
