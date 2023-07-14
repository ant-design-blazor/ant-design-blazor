using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class MenuToken
    {
        public int DropdownWidth { get; set; }

        public int ZIndexPopup { get; set; }

        public string ColorGroupTitle { get; set; }

        public int RadiusItem { get; set; }

        public int RadiusSubMenuItem { get; set; }

        public string ColorItemText { get; set; }

        public string ColorItemTextHover { get; set; }

        public string ColorItemTextHoverHorizontal { get; set; }

        public string ColorItemTextSelected { get; set; }

        public string ColorItemTextSelectedHorizontal { get; set; }

        public string ColorItemTextDisabled { get; set; }

        public string ColorDangerItemText { get; set; }

        public string ColorDangerItemTextHover { get; set; }

        public string ColorDangerItemTextSelected { get; set; }

        public string ColorDangerItemBgActive { get; set; }

        public string ColorDangerItemBgSelected { get; set; }

        public string ColorItemBg { get; set; }

        public string ColorItemBgHover { get; set; }

        public string ColorSubItemBg { get; set; }

        public string ColorItemBgActive { get; set; }

        public string ColorItemBgSelected { get; set; }

        public string ColorItemBgSelectedHorizontal { get; set; }

        public int ColorActiveBarWidth { get; set; }

        public int ColorActiveBarHeight { get; set; }

        public int ColorActiveBarBorderSize { get; set; }

        public int ItemMarginInline { get; set; }

    }

    public partial class MenuToken : TokenWithCommonCls
    {
        public int MenuItemHeight { get; set; }

        public int MenuHorizontalHeight { get; set; }

        public int MenuItemPaddingInline { get; set; }

        public int MenuArrowSize { get; set; }

        public string MenuArrowOffset { get; set; }

        public int MenuPanelMaskInset { get; set; }

        public string MenuSubMenuBg { get; set; }

    }

    public partial class Menu
    {
        public CSSObject GenMenuItemStyle(MenuToken token)
        {
            var componentCls = token.ComponentCls;
            var fontSize = token.FontSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var motionEaseOut = token.MotionEaseOut;
            var iconCls = token.IconCls;
            var controlHeightSM = token.ControlHeightSM;
            return new CSSObject()
            {
                [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSObject()
                {
                    Position = "relative",
                    Display = "block",
                    Margin = 0,
                    WhiteSpace = "nowrap",
                    Cursor = "pointer",
                    Transition = [
        `border-color ${motionDurationSlow}`,
        `background ${motionDurationSlow}`,
        `padding ${motionDurationSlow} ${motionEaseInOut}`,
      ].join(","),
                    [$"{componentCls}-item-icon, {iconCls}"] = new CSSObject()
                    {
                        MinWidth = fontSize,
                        FontSize = fontSize,
                        Transition = [
          `font-size ${motionDurationMid} ${motionEaseOut}`,
          `margin ${motionDurationSlow} ${motionEaseInOut}`,
          `color ${motionDurationSlow}`,
        ].join(","),
                        ["+ span"] = new CSSObject()
                        {
                            MarginInlineStart = controlHeightSM - fontSize,
                            Opacity = 1,
                            Transition = [
            `opacity ${motionDurationSlow} ${motionEaseInOut}`,
            `margin ${motionDurationSlow}`,
            `color ${motionDurationSlow}`,
          ].join(",")
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
                            Transition = [
            `background ${motionDurationSlow} ${motionEaseInOut}`,
            `transform ${motionDurationSlow} ${motionEaseInOut}`,
            `top ${motionDurationSlow} ${motionEaseInOut}`,
            `color ${motionDurationSlow} ${motionEaseInOut}`,
          ].join(","),
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

        public Unknown_1 GetBaseStyle(Unknown_7 token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var fontSize = token.FontSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var lineHeight = token.LineHeight;
            var paddingXS = token.PaddingXS;
            var padding = token.Padding;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var zIndexPopup = token.ZIndexPopup;
            var borderRadiusLG = token.BorderRadiusLG;
            var radiusSubMenuItem = token.RadiusSubMenuItem;
            var menuArrowSize = token.MenuArrowSize;
            var menuArrowOffset = token.MenuArrowOffset;
            var lineType = token.LineType;
            var menuPanelMaskInset = token.MenuPanelMaskInset;
            return new Unknown_8
            {
                new Unknown_9()
                {
                    [""] = new Unknown_10()
                    {
                        [$"{componentCls}"] = new Unknown_11()
                        {
                            ["..."] = ClearFix(),
                            ["&-hidden"] = new Unknown_12()
                            {
                                Display = "none",
                            },
                        },
                    },
                    [$"{componentCls}-submenu-hidden"] = new Unknown_13()
                    {
                        Display = "none",
                    },
                },
                new Unknown_14()
                {
                    [componentCls] = new Unknown_15()
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
                        ["ul, ol"] = new Unknown_16()
                        {
                            Margin = 0,
                            Padding = 0,
                            ListStyle = "none",
                        },
                        ["&-overflow"] = new Unknown_17()
                        {
                            Display = "flex",
                            [$"{componentCls}-item"] = new Unknown_18()
                            {
                                Flex = "none",
                            },
                        },
                        [$"{componentCls}-item, {componentCls}-submenu, {componentCls}-submenu-title"] = new Unknown_19()
                        {
                            BorderRadius = token.RadiusItem,
                        },
                        [$"{componentCls}-item-group-title"] = new Unknown_20()
                        {
                            Padding = @$"{paddingXS}px {padding}px",
                            FontSize = fontSize,
                            LineHeight = lineHeight,
                            Transition = @$"all {motionDurationSlow}",
                        },
                        [$"&-horizontal {componentCls}-submenu"] = new Unknown_21()
                        {
                            Transition = [
            `border-color ${motionDurationSlow} ${motionEaseInOut}`,
            `background ${motionDurationSlow} ${motionEaseInOut}`,
          ].join(",")
                        },
                        [$"{componentCls}-submenu, {componentCls}-submenu-inline"] = new Unknown_22()
                        {
                            Transition = [
            `border-color ${motionDurationSlow} ${motionEaseInOut}`,
            `background ${motionDurationSlow} ${motionEaseInOut}`,
            `padding ${motionDurationMid} ${motionEaseInOut}`,
          ].join(",")
                        },
                        [$"{componentCls}-submenu {componentCls}-sub"] = new Unknown_23()
                        {
                            Cursor = "initial",
                            Transition = [
            `background ${motionDurationSlow} ${motionEaseInOut}`,
            `padding ${motionDurationSlow} ${motionEaseInOut}`,
          ].join(",")
                        },
                        [$"{componentCls}-title-content"] = new Unknown_24()
                        {
                            Transition = @$"color {motionDurationSlow}",
                        },
                        [$"{componentCls}-item a"] = new Unknown_25()
                        {
                            ["&::before"] = new Unknown_26()
                            {
                                Position = "absolute",
                                Inset = 0,
                                BackgroundColor = "transparent",
                                Content = "\"\"",
                            },
                        },
                        [$"{componentCls}-item-divider"] = new Unknown_27()
                        {
                            Overflow = "hidden",
                            LineHeight = 0,
                            BorderColor = colorSplit,
                            BorderStyle = lineType,
                            BorderWidth = 0,
                            BorderTopWidth = lineWidth,
                            MarginBlock = lineWidth,
                            Padding = 0,
                            ["&-dashed"] = new Unknown_28()
                            {
                                BorderStyle = "dashed",
                            },
                        },
                        ["..."] = GenMenuItemStyle(token),
                        [$"{componentCls}-item-group"] = new Unknown_29()
                        {
                            [$"{componentCls}-item-group-list"] = new Unknown_30()
                            {
                                Margin = 0,
                                Padding = 0,
                                [$"{componentCls}-item, {componentCls}-submenu-title"] = new Unknown_31()
                                {
                                    PaddingInline = @$"{fontSize * 2}px {padding}px",
                                },
                            },
                        },
                        ["&-submenu"] = new Unknown_32()
                        {
                            ["&-popup"] = new Unknown_33()
                            {
                                Position = "absolute",
                                ZIndex = zIndexPopup,
                                Background = "transparent",
                                BorderRadius = borderRadiusLG,
                                BoxShadow = "none",
                                TransformOrigin = "0 0",
                                ["&::before"] = new Unknown_34()
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
                            ["&-placement-rightTop::before"] = new Unknown_35()
                            {
                                Top = 0,
                                InsetInlineStart = menuPanelMaskInset,
                            },
                            [$"> {componentCls}"] = new Unknown_36()
                            {
                                BorderRadius = borderRadiusLG,
                                ["..."] = GenMenuItemStyle(token),
                                ["..."] = GenSubMenuArrowStyle(token),
                                [$"{componentCls}-item, {componentCls}-submenu > {componentCls}-submenu-title"] = new Unknown_37()
                                {
                                    BorderRadius = radiusSubMenuItem,
                                },
                                [$"{componentCls}-submenu-title::after"] = new Unknown_38()
                                {
                                    Transition = @$"transform {motionDurationSlow} {motionEaseInOut}",
                                },
                            },
                        },
                        ["..."] = GenSubMenuArrowStyle(token),
                        [$"&-inline-collapsed{componentCls}-submenu-arrow,&-inline{componentCls}-submenu-arrow"] = new Unknown_39()
                        {
                            ["&::before"] = new Unknown_40()
                            {
                                Transform = @$"rotate(-45deg) translateX({menuArrowOffset})",
                            },
                            ["&::after"] = new Unknown_41()
                            {
                                Transform = @$"rotate(45deg) translateX(-{menuArrowOffset})",
                            },
                        },
                        [$"{componentCls}-submenu-open{componentCls}-submenu-inline > {componentCls}-submenu-title > {componentCls}-submenu-arrow"] = new Unknown_42()
                        {
                            Transform = @$"translateY(-{menuArrowSize * 0.2}px)",
                            ["&::after"] = new Unknown_43()
                            {
                                Transform = @$"rotate(-45deg) translateX(-{menuArrowOffset})",
                            },
                            ["&::before"] = new Unknown_44()
                            {
                                Transform = @$"rotate(45deg) translateX({menuArrowOffset})",
                            },
                        },
                    },
                },
                new Unknown_45()
                {
                    [$"{antCls}-layout-header"] = new Unknown_46()
                    {
                        [componentCls] = new Unknown_47()
                        {
                            LineHeight = "inherit",
                        },
                    },
                },
            };
        }

        public Unknown_2 GetHorizontalStyle(Unknown_48 token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var menuHorizontalHeight = token.MenuHorizontalHeight;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var menuItemPaddingInline = token.MenuItemPaddingInline;
            return new Unknown_49()
            {
                [$"{componentCls}-horizontal"] = new Unknown_50()
                {
                    LineHeight = @$"{menuHorizontalHeight}px",
                    Border = 0,
                    BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                    BoxShadow = "none",
                    ["&::after"] = new Unknown_51()
                    {
                        Display = "block",
                        Clear = "both",
                        Height = 0,
                        Content = "\"\\20\"",
                    },
                    [$"{componentCls}-item, {componentCls}-submenu"] = new Unknown_52()
                    {
                        Position = "relative",
                        Display = "inline-block",
                        VerticalAlign = "bottom",
                        PaddingInline = menuItemPaddingInline,
                    },
                    [$">{componentCls}-item:hover,>{componentCls}-item-active,>{componentCls}-submenu{componentCls}-submenu-title:hover"] = new Unknown_53()
                    {
                        BackgroundColor = "transparent",
                    },
                    [$"{componentCls}-item, {componentCls}-submenu-title"] = new Unknown_54()
                    {
                        Transition = [`border-color ${motionDurationSlow}`, `background ${motionDurationSlow}`].join(",")
                    },
                    [$"{componentCls}-submenu-arrow"] = new Unknown_55()
                    {
                        Display = "none",
                    },
                },
            };
        }

        public Unknown_3 GetVerticalInlineStyle(Unknown_56 token)
        {
            var componentCls = token.ComponentCls;
            var menuItemHeight = token.MenuItemHeight;
            var itemMarginInline = token.ItemMarginInline;
            var padding = token.Padding;
            var menuArrowSize = token.MenuArrowSize;
            var marginXS = token.MarginXS;
            var marginXXS = token.MarginXXS;
            var paddingWithArrow = padding + menuArrowSize + marginXS;
            return new Unknown_57()
            {
                [$"{componentCls}-item"] = new Unknown_58()
                {
                    Position = "relative",
                    ["&:not(:last-child)"] = new Unknown_59()
                    {
                        MarginBottom = marginXS,
                    },
                },
                [$"{componentCls}-item, {componentCls}-submenu-title"] = new Unknown_60()
                {
                    Height = menuItemHeight,
                    LineHeight = @$"{menuItemHeight}px",
                    PaddingInline = padding,
                    Overflow = "hidden",
                    TextOverflow = "ellipsis",
                    MarginInline = itemMarginInline,
                    MarginBlock = marginXXS,
                    Width = @$"calc(100% - {itemMarginInline * 2}px)",
                },
                [$"{componentCls}-submenu"] = new Unknown_61()
                {
                    PaddingBottom = 0.02f,
                },
                [$">{componentCls}-item,>{componentCls}-submenu>{componentCls}-submenu-title"] = new Unknown_62()
                {
                    Height = menuItemHeight,
                    LineHeight = @$"{menuItemHeight}px",
                },
                [$"{componentCls}-item-group-list{componentCls}-submenu-title,{componentCls}-submenu-title"] = new Unknown_63()
                {
                    PaddingInlineEnd = paddingWithArrow,
                },
            };
        }

        public Unknown_4 GetVerticalStyle(Unknown_64 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var menuItemHeight = token.MenuItemHeight;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var dropdownWidth = token.DropdownWidth;
            var controlHeightLG = token.ControlHeightLG;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseOut = token.MotionEaseOut;
            var paddingXL = token.PaddingXL;
            var fontSizeSM = token.FontSizeSM;
            var fontSizeLG = token.FontSizeLG;
            var motionDurationSlow = token.MotionDurationSlow;
            var paddingXS = token.PaddingXS;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var inlineItemStyle = new Unknown_65()
            {
                Height = menuItemHeight,
                LineHeight = @$"{menuItemHeight}px",
                ListStylePosition = "inside",
                ListStyleType = "disc",
            };
            return new Unknown_66
            {
                new Unknown_67()
                {
                    [componentCls] = new Unknown_68()
                    {
                        ["&-inline, &-vertical"] = new Unknown_69()
                        {
                            [$"&{componentCls}-root"] = new Unknown_70()
                            {
                                BoxShadow = "none",
                            },
                            ["..."] = GetVerticalInlineStyle(token)
                        },
                    },
                    [$"{componentCls}-submenu-popup"] = new Unknown_71()
                    {
                        [$"{componentCls}-vertical"] = new Unknown_72()
                        {
                            ["..."] = GetVerticalInlineStyle(token),
                            BoxShadow = boxShadowSecondary,
                        },
                    },
                },
                new Unknown_73()
                {
                    [$"{componentCls}-submenu-popup {componentCls}-vertical{componentCls}-sub"] = new Unknown_74()
                    {
                        MinWidth = dropdownWidth,
                        MaxHeight = @$"calc(100vh - {controlHeightLG * 2.5}px)",
                        Padding = "0",
                        Overflow = "hidden",
                        BorderInlineEnd = 0,
                        ["&:not([class*="-active"])"] = new Unknown_75()
                        {
                            OverflowX = "hidden",
                            OverflowY = "auto",
                        },
                    },
                },
                new Unknown_76()
                {
                    [$"{componentCls}-inline"] = new Unknown_77()
                    {
                        Width = "100%",
                        [$"&{componentCls}-root"] = new Unknown_78()
                        {
                            [$"{componentCls}-item, {componentCls}-submenu-title"] = new Unknown_79()
                            {
                                Display = "flex",
                                AlignItems = "center",
                                Transition = [
              `border-color ${motionDurationSlow}`,
              `background ${motionDurationSlow}`,
              `padding ${motionDurationMid} ${motionEaseOut}`,
            ].join(","),
                                [$"> {componentCls}-title-content"] = new Unknown_80()
                                {
                                    Flex = "auto",
                                    MinWidth = 0,
                                    Overflow = "hidden",
                                    TextOverflow = "ellipsis",
                                },
                                ["> *"] = new Unknown_81()
                                {
                                    Flex = "none",
                                },
                            },
                        },
                        [$"{componentCls}-sub{componentCls}-inline"] = new Unknown_82()
                        {
                            Padding = 0,
                            Border = 0,
                            BorderRadius = 0,
                            BoxShadow = "none",
                            [$"& > {componentCls}-submenu > {componentCls}-submenu-title"] = inlineItemStyle,
                            [$"& {componentCls}-item-group-title"] = new Unknown_83()
                            {
                                PaddingInlineStart = paddingXL,
                            },
                        },
                        [$"{componentCls}-item"] = inlineItemStyle,
                    },
                },
                new Unknown_84()
                {
                    [$"{componentCls}-inline-collapsed"] = new Unknown_85()
                    {
                        Width = menuItemHeight * 2,
                        [$"&{componentCls}-root"] = new Unknown_86()
                        {
                            [$"{componentCls}-item, {componentCls}-submenu {componentCls}-submenu-title"] = new Unknown_87()
                            {
                                [$"> {componentCls}-inline-collapsed-noicon"] = new Unknown_88()
                                {
                                    FontSize = fontSizeLG,
                                    TextAlign = "center",
                                },
                            },
                        },
                        [$">{componentCls}-item,>{componentCls}-item-group>{componentCls}-item-group-list>{componentCls}-item,>{componentCls}-item-group>{componentCls}-item-group-list>{componentCls}-submenu>{componentCls}-submenu-title,>{componentCls}-submenu>{componentCls}-submenu-title"] = new Unknown_89()
                        {
                            InsetInlineStart = 0,
                            PaddingInline = @$"calc(50% - {fontSizeSM}px)",
                            TextOverflow = "clip",
                            [$"{componentCls}-submenu-arrow,{componentCls}-submenu-expand-icon"] = new Unknown_90()
                            {
                                Opacity = 0,
                            },
                            [$"{componentCls}-item-icon, {iconCls}"] = new Unknown_91()
                            {
                                Margin = 0,
                                FontSize = fontSizeLG,
                                LineHeight = @$"{menuItemHeight}px",
                                ["+ span"] = new Unknown_92()
                                {
                                    Display = "inline-block",
                                    Opacity = 0,
                                },
                            },
                        },
                        [$"{componentCls}-item-icon, {iconCls}"] = new Unknown_93()
                        {
                            Display = "inline-block",
                        },
                        ["&-tooltip"] = new Unknown_94()
                        {
                            PointerEvents = "none",
                            [$"{componentCls}-item-icon, {iconCls}"] = new Unknown_95()
                            {
                                Display = "none",
                            },
                            ["a, a:hover"] = new Unknown_96()
                            {
                                Color = colorTextLightSolid,
                            },
                        },
                        [$"{componentCls}-item-group-title"] = new Unknown_97()
                        {
                            ["..."] = textEllipsis,
                            PaddingInline = paddingXS,
                        },
                    },
                },
            };
        }

        public Unknown_5 GetRTLStyle(Unknown_98 args)
        {
            return new Unknown_99()
            {
                [$"{componentCls}-rtl"] = new Unknown_100()
                {
                    Direction = "rtl",
                },
                [$"{componentCls}-submenu-rtl"] = new Unknown_101()
                {
                    TransformOrigin = "100% 0",
                },
                [$"{componentCls}-rtl{componentCls}-vertical,{componentCls}-submenu-rtl{componentCls}-vertical"] = new Unknown_102()
                {
                    [$"{componentCls}-submenu-arrow"] = new Unknown_103()
                    {
                        ["&::before"] = new Unknown_104()
                        {
                            Transform = @$"rotate(-45deg) translateY(-{menuArrowOffset})",
                        },
                        ["&::after"] = new Unknown_105()
                        {
                            Transform = @$"rotate(45deg) translateY({menuArrowOffset})",
                        },
                    },
                },
            };
        }

        public Unknown_6 AccessibilityFocus(MenuToken token)
        {
            return new Unknown_106()
            {
                ["..."] = GenFocusOutline(token)
            };
        }

        public CSSInterpolation GetThemeStyle(MenuToken token, string themeSuffix)
        {
            var componentCls = token.ComponentCls;
            var colorItemText = token.ColorItemText;
            var colorItemTextSelected = token.ColorItemTextSelected;
            var colorGroupTitle = token.ColorGroupTitle;
            var colorItemBg = token.ColorItemBg;
            var colorSubItemBg = token.ColorSubItemBg;
            var colorItemBgSelected = token.ColorItemBgSelected;
            var colorActiveBarHeight = token.ColorActiveBarHeight;
            var colorActiveBarWidth = token.ColorActiveBarWidth;
            var colorActiveBarBorderSize = token.ColorActiveBarBorderSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var motionEaseInOut = token.MotionEaseInOut;
            var motionEaseOut = token.MotionEaseOut;
            var menuItemPaddingInline = token.MenuItemPaddingInline;
            var motionDurationMid = token.MotionDurationMid;
            var colorItemTextHover = token.ColorItemTextHover;
            var lineType = token.LineType;
            var colorSplit = token.ColorSplit;
            var colorItemTextDisabled = token.ColorItemTextDisabled;
            var colorDangerItemText = token.ColorDangerItemText;
            var colorDangerItemTextHover = token.ColorDangerItemTextHover;
            var colorDangerItemTextSelected = token.ColorDangerItemTextSelected;
            var colorDangerItemBgActive = token.ColorDangerItemBgActive;
            var colorDangerItemBgSelected = token.ColorDangerItemBgSelected;
            var colorItemBgHover = token.ColorItemBgHover;
            var menuSubMenuBg = token.MenuSubMenuBg;
            var colorItemTextSelectedHorizontal = token.ColorItemTextSelectedHorizontal;
            var colorItemBgSelectedHorizontal = token.ColorItemBgSelectedHorizontal;
            return new CSSInterpolation()
            {
                [$"{componentCls}-{themeSuffix}, {componentCls}-{themeSuffix} > {componentCls}"] = new CSSInterpolation()
                {
                    Color = colorItemText,
                    Background = colorItemBg,
                    [$"&{componentCls}-root:focus-visible"] = new CSSInterpolation()
                    {
                        ["..."] = AccessibilityFocus(token)
                    },
                    [$"{componentCls}-item-group-title"] = new CSSInterpolation()
                    {
                        Color = colorGroupTitle,
                    },
                    [$"{componentCls}-submenu-selected"] = new CSSInterpolation()
                    {
                        [$"> {componentCls}-submenu-title"] = new CSSInterpolation()
                        {
                            Color = colorItemTextSelected,
                        },
                    },
                    [$"{componentCls}-item-disabled, {componentCls}-submenu-disabled"] = new CSSInterpolation()
                    {
                        Color = @$"{colorItemTextDisabled} !important",
                    },
                    [$"{componentCls}-item:hover, {componentCls}-submenu-title:hover"] = new CSSInterpolation()
                    {
                        [$"&:not({componentCls}-item-selected):not({componentCls}-submenu-selected)"] = new CSSInterpolation()
                        {
                            Color = colorItemTextHover,
                        },
                    },
                    [$"&:not({componentCls}-horizontal)"] = new CSSInterpolation()
                    {
                        [$"{componentCls}-item:not({componentCls}-item-selected)"] = new CSSInterpolation()
                        {
                            ["&:hover"] = new CSSInterpolation()
                            {
                                BackgroundColor = colorItemBgHover,
                            },
                            ["&:active"] = new CSSInterpolation()
                            {
                                BackgroundColor = colorItemBgSelected,
                            },
                        },
                        [$"{componentCls}-submenu-title"] = new CSSInterpolation()
                        {
                            ["&:hover"] = new CSSInterpolation()
                            {
                                BackgroundColor = colorItemBgHover,
                            },
                            ["&:active"] = new CSSInterpolation()
                            {
                                BackgroundColor = colorItemBgSelected,
                            },
                        },
                    },
                    [$"{componentCls}-item-danger"] = new CSSInterpolation()
                    {
                        Color = colorDangerItemText,
                        [$"&{componentCls}-item:hover"] = new CSSInterpolation()
                        {
                            [$"&:not({componentCls}-item-selected):not({componentCls}-submenu-selected)"] = new CSSInterpolation()
                            {
                                Color = colorDangerItemTextHover,
                            },
                        },
                        [$"&{componentCls}-item:active"] = new CSSInterpolation()
                        {
                            Background = colorDangerItemBgActive,
                        },
                    },
                    [$"{componentCls}-item a"] = new CSSInterpolation()
                    {
                        ["&, &:hover"] = new CSSInterpolation()
                        {
                            Color = "inherit",
                        },
                    },
                    [$"{componentCls}-item-selected"] = new CSSInterpolation()
                    {
                        Color = colorItemTextSelected,
                        [$"&{componentCls}-item-danger"] = new CSSInterpolation()
                        {
                            Color = colorDangerItemTextSelected,
                        },
                        ["a, a:hover"] = new CSSInterpolation()
                        {
                            Color = "inherit",
                        },
                    },
                    [$"& {componentCls}-item-selected"] = new CSSInterpolation()
                    {
                        BackgroundColor = colorItemBgSelected,
                        [$"&{componentCls}-item-danger"] = new CSSInterpolation()
                        {
                            BackgroundColor = colorDangerItemBgSelected,
                        },
                    },
                    [$"{componentCls}-item, {componentCls}-submenu-title"] = new CSSInterpolation()
                    {
                        [$"&:not({componentCls}-item-disabled):focus-visible"] = new CSSInterpolation()
                        {
                            ["..."] = AccessibilityFocus(token)
                        },
                    },
                    [$"&{componentCls}-submenu > {componentCls}"] = new CSSInterpolation()
                    {
                        BackgroundColor = menuSubMenuBg,
                    },
                    [$"&{componentCls}-popup > {componentCls}"] = new CSSInterpolation()
                    {
                        BackgroundColor = colorItemBg,
                    },
                    [$"&{componentCls}-horizontal"] = new CSSInterpolation()
                    {
                        ["..."] = (themeSuffix === "dark"
          ? {
              borderBottom: 0,
            }
          : {}),
                        [$"> {componentCls}-item, > {componentCls}-submenu"] = new CSSInterpolation()
                        {
                            Top = colorActiveBarBorderSize,
                            MarginTop = -colorActiveBarBorderSize,
                            MarginBottom = 0,
                            BorderRadius = 0,
                            ["&::after"] = new CSSInterpolation()
                            {
                                Position = "absolute",
                                InsetInline = menuItemPaddingInline,
                                Bottom = 0,
                                BorderBottom = @$"{colorActiveBarHeight}px solid transparent",
                                Transition = @$"border-color {motionDurationSlow} {motionEaseInOut}",
                                Content = "\"\"",
                            },
                            ["&:hover, &-active, &-open"] = new CSSInterpolation()
                            {
                                ["&::after"] = new CSSInterpolation()
                                {
                                    BorderBottomWidth = colorActiveBarHeight,
                                    BorderBottomColor = colorItemTextSelectedHorizontal,
                                },
                            },
                            ["&-selected"] = new CSSInterpolation()
                            {
                                Color = colorItemTextSelectedHorizontal,
                                BackgroundColor = colorItemBgSelectedHorizontal,
                                ["&::after"] = new CSSInterpolation()
                                {
                                    BorderBottomWidth = colorActiveBarHeight,
                                    BorderBottomColor = colorItemTextSelectedHorizontal,
                                },
                            },
                        },
                    },
                    [$"&{componentCls}-root"] = new CSSInterpolation()
                    {
                        [$"&{componentCls}-inline, &{componentCls}-vertical"] = new CSSInterpolation()
                        {
                            BorderInlineEnd = @$"{colorActiveBarBorderSize}px {lineType} {colorSplit}",
                        },
                    },
                    [$"&{componentCls}-inline"] = new CSSInterpolation()
                    {
                        [$"{componentCls}-sub{componentCls}-inline"] = new CSSInterpolation()
                        {
                            Background = colorSubItemBg,
                        },
                        [$"{componentCls}-item, {componentCls}-submenu-title"] = ColorActiveBarBorderSize && colorActiveBarWidth
            ? {
                width: @$"calc(100% + {colorActiveBarBorderSize}px)",
              }
            : {},
                        [$"{componentCls}-item"] = new CSSInterpolation()
                        {
                            Position = "relative",
                            ["&::after"] = new CSSInterpolation()
                            {
                                Position = "absolute",
                                InsetBlock = 0,
                                InsetInlineEnd = 0,
                                BorderInlineEnd = @$"{colorActiveBarWidth}px solid {colorItemTextSelected}",
                                Transform = "scaleY(0.0001)",
                                Opacity = 0,
                                Transition = [
              `transform ${motionDurationMid} ${motionEaseOut}`,
              `opacity ${motionDurationMid} ${motionEaseOut}`,
            ].join(","),
                                Content = "\"\"",
                            },
                            [$"&{componentCls}-item-danger"] = new CSSInterpolation()
                            {
                                ["&::after"] = new CSSInterpolation()
                                {
                                    BorderInlineEndColor = colorDangerItemTextSelected,
                                },
                            },
                        },
                        [$"{componentCls}-selected, {componentCls}-item-selected"] = new CSSInterpolation()
                        {
                            ["&::after"] = new CSSInterpolation()
                            {
                                Transform = "scaleY(1)",
                                Opacity = 1,
                                Transition = [
              `transform ${motionDurationMid} ${motionEaseInOut}`,
              `opacity ${motionDurationMid} ${motionEaseInOut}`,
            ].join(",")
                            },
                        },
                    },
                },
            };
        }

    }

}