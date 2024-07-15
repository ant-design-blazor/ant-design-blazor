using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.CheckboxStyle;

namespace AntDesign
{
    public partial class CascaderToken : TokenWithCommonCls
    {
        public double ControlWidth
        {
            get => (double)_tokens["controlWidth"];
            set => _tokens["controlWidth"] = value;
        }

        public double ControlItemWidth
        {
            get => (double)_tokens["controlItemWidth"];
            set => _tokens["controlItemWidth"] = value;
        }

        public double DropdownHeight
        {
            get => (double)_tokens["dropdownHeight"];
            set => _tokens["dropdownHeight"] = value;
        }

        public string OptionSelectedBg
        {
            get => (string)_tokens["optionSelectedBg"];
            set => _tokens["optionSelectedBg"] = value;
        }

        public double OptionSelectedFontWeight
        {
            get => (double)_tokens["optionSelectedFontWeight"];
            set => _tokens["optionSelectedFontWeight"] = value;
        }

        public string OptionPadding
        {
            get => (string)_tokens["optionPadding"];
            set => _tokens["optionPadding"] = value;
        }

        public double MenuPadding
        {
            get => (double)_tokens["menuPadding"];
            set => _tokens["menuPadding"] = value;
        }

    }

    public partial class CascaderToken
    {
    }

    public partial class CascaderStyle
    {
        public static CSSInterpolation[] GenBaseStyle(CascaderToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            return new CSSInterpolation[]
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
                    [$"{componentCls}-dropdown"] = new CSSInterpolation[]
                    {
                        new CSSObject()
                        {
                            [$"&{antCls}-select-dropdown"] = new CSSObject()
                            {
                                Padding = 0,
                            },
                        },
                        GetColumnsStyle(token),
                    }
                },
                new CSSObject()
                {
                    [$"{componentCls}-dropdown-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
                GenCompactItemStyle(token),
            };
        }

        public static CascaderToken PrepareComponentToken(GlobalToken token)
        {
            var itemPaddingVertical = Math.Round((token.ControlHeight - token.FontSize * token.LineHeight) / 2);
            return new CascaderToken()
            {
                ControlWidth = 184,
                ControlItemWidth = 111,
                DropdownHeight = 180,
                OptionSelectedBg = token.ControlItemBgActive,
                OptionSelectedFontWeight = token.FontWeightStrong,
                OptionPadding = @$"{itemPaddingVertical}px {token.PaddingSM}px",
                MenuPadding = token.PaddingXXS,
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Cascader",
                (token) =>
                {
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(token),
                    };
                },
                PrepareComponentToken);
        }

        public static CSSInterpolation[] GetColumnsStyle(CascaderToken token)
        {
            var prefixCls = token.PrefixCls;
            var componentCls = token.ComponentCls;
            var cascaderMenuItemCls = @$"{componentCls}-menu-item";
            var iconCls = @$"
  &{cascaderMenuItemCls}-expand {cascaderMenuItemCls}-expand-icon,
  {cascaderMenuItemCls}-loading-icon
";
            return new CSSInterpolation[]
            {
                GetCheckboxStyle($"{prefixCls}-checkbox", token),
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
                            FlexShrink = 0,
                            MinWidth = token.ControlItemWidth,
                            Height = token.DropdownHeight,
                            Margin = 0,
                            Padding = token.MenuPadding,
                            Overflow = "auto",
                            VerticalAlign = "top",
                            ListStyle = "none",
                            ["-ms-overflow-style"] = "-ms-autohiding-scrollbar",
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
                                Padding = token.OptionPadding,
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
                                        FontWeight = token.OptionSelectedFontWeight,
                                        BackgroundColor = token.OptionSelectedBg,
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
            };
        }

    }

}
