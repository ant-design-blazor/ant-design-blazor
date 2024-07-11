using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

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
                        // GetColumnsStyle(token),
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

    }

}
