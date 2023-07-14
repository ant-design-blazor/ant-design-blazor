using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class CascaderToken : TokenWithCommonCls
    {
        public int ControlWidth { get; set; }

        public int ControlItemWidth { get; set; }

        public int DropdownHeight { get; set; }

    }

    public partial class CascaderToken
    {
    }

    public partial class Cascader
    {
        public CSSObject[] GenBaseStyle(CascaderToken token)
        {
            var prefixCls = token.PrefixCls;
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var cascaderMenuItemCls = @$"{componentCls}-menu-item";
            var iconCls = @$"
    &{cascaderMenuItemCls}-expand {cascaderMenuItemCls}-expand-icon,
    {cascaderMenuItemCls}-loading-icon
  ";
            var itemPaddingVertical = Math.Round((double)(token.ControlHeight - token.FontSize * token.LineHeight) / 2);
            return new CSSObject[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        Width = token.ControlWidth,
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-dropdown"] = new CSSObject[]
                    {
                        GetCheckboxStyle($"{prefixCls}-checkbox", token),
                        new CSSObject()
                        {
                            [$"&{antCls}-select-dropdown"] = new CSSObject()
                            {
                                Padding = 0,
                            },
                        },
                        new CSSObject()
                        {
                            [componentCls] = new CSSObject()
                            {
                                ["&-checkbox"] = new CSSObject()
                                {
                                    Top = 0,
                                    MarginInlineEnd = token.PaddingXS,
                                },
                                ["&-menus"] = new CSSObject()
                                {
                                    Display = "flex",
                                    FlexWrap = "nowrap",
                                    AlignItems = "flex-start",
                                    [$"&{componentCls}-menu-empty"] = new CSSObject()
                                    {
                                        [$"{componentCls}-menu"] = new CSSObject()
                                        {
                                            Width = "100%",
                                            Height = "auto",
                                            [cascaderMenuItemCls] = new CSSObject()
                                            {
                                                Color = token.ColorTextDisabled,
                                            },
                                        },
                                    },
                                },
                                ["&-menu"] = new CSSObject()
                                {
                                    FlexGrow = 1,
                                    MinWidth = token.ControlItemWidth,
                                    Height = token.DropdownHeight,
                                    Margin = 0,
                                    Padding = token.PaddingXXS,
                                    Overflow = "auto",
                                    VerticalAlign = "top",
                                    ListStyle = "none",
                                    MsOverflowStyle = "-ms-autohiding-scrollbar",
                                    ["&:not(:last-child)"] = new CSSObject()
                                    {
                                        BorderInlineEnd = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                                    },
                                    ["&-item"] = new CSSObject()
                                    {
                                        ["..."] = TextEllipsis,
                                        Display = "flex",
                                        FlexWrap = "nowrap",
                                        AlignItems = "center",
                                        Padding = @$"{itemPaddingVertical}px {token.PaddingSM}px",
                                        LineHeight = token.LineHeight,
                                        Cursor = "pointer",
                                        Transition = @$"all {token.MotionDurationMid}",
                                        BorderRadius = token.BorderRadiusSM,
                                        ["&:hover"] = new CSSObject()
                                        {
                                            Background = token.ControlItemBgHover,
                                        },
                                        ["&-disabled"] = new CSSObject()
                                        {
                                            Color = token.ColorTextDisabled,
                                            Cursor = "not-allowed",
                                            ["&:hover"] = new CSSObject()
                                            {
                                                Background = "transparent",
                                            },
                                            [iconCls] = new CSSObject()
                                            {
                                                Color = token.ColorTextDisabled,
                                            },
                                        },
                                        [$"&-active:not({cascaderMenuItemCls}-disabled)"] = new CSSObject()
                                        {
                                            ["&, &:hover"] = new CSSObject()
                                            {
                                                FontWeight = token.FontWeightStrong,
                                                BackgroundColor = token.ControlItemBgActive,
                                            },
                                        },
                                        ["&-content"] = new CSSObject()
                                        {
                                            Flex = "auto",
                                        },
                                        [iconCls] = new CSSObject()
                                        {
                                            MarginInlineStart = token.PaddingXXS,
                                            Color = token.ColorTextDescription,
                                            FontSize = token.FontSizeIcon,
                                        },
                                        ["&-keyword"] = new CSSObject()
                                        {
                                            Color = token.ColorHighlight,
                                        },
                                    },
                                },
                            },
                        },
                    }
                },
                new CSSObject()
                {
                    [$"{componentCls}-dropdown-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
                GenCompactItemStyle(token)
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            return new CSSInterpolation[] { GenBaseStyle(token as CascaderToken) };
        }

    }

}