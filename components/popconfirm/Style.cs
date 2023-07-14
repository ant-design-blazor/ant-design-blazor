using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class PopconfirmToken
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class PopconfirmToken : TokenWithCommonCls
    {
    }

    public partial class Popconfirm
    {
        public Unknown_1 GenBaseStyle(Unknown_4 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var zIndexPopup = token.ZIndexPopup;
            var colorText = token.ColorText;
            var colorWarning = token.ColorWarning;
            var marginXS = token.MarginXS;
            var fontSize = token.FontSize;
            var fontWeightStrong = token.FontWeightStrong;
            var lineHeight = token.LineHeight;
            return new Unknown_5()
            {
                [componentCls] = new Unknown_6()
                {
                    ZIndex = zIndexPopup,
                    [$"{componentCls}-inner-content"] = new Unknown_7()
                    {
                        Color = colorText,
                    },
                    [$"{componentCls}-message"] = new Unknown_8()
                    {
                        Position = "relative",
                        MarginBottom = marginXS,
                        Color = colorText,
                        FontSize = fontSize,
                        Display = "flex",
                        FlexWrap = "nowrap",
                        AlignItems = "start",
                        [$"> {componentCls}-message-icon {iconCls}"] = new Unknown_9()
                        {
                            Color = colorWarning,
                            FontSize = fontSize,
                            Flex = "none",
                            LineHeight = 1,
                            PaddingTop = (Math.Round(fontSize * lineHeight) - fontSize) / 2,
                        },
                        ["&-title"] = new Unknown_10()
                        {
                            Flex = "auto",
                            MarginInlineStart = marginXS,
                        },
                        ["&-title-only"] = new Unknown_11()
                        {
                            FontWeight = fontWeightStrong,
                        },
                    },
                    [$"{componentCls}-description"] = new Unknown_12()
                    {
                        Position = "relative",
                        MarginInlineStart = fontSize + marginXS,
                        MarginBottom = marginXS,
                        Color = colorText,
                        FontSize = fontSize,
                    },
                    [$"{componentCls}-buttons"] = new Unknown_13()
                    {
                        TextAlign = "end",
                        ["button"] = new Unknown_14()
                        {
                            MarginInlineStart = marginXS,
                        },
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_15 token)
        {
            return GenBaseStyle(token);
        }

        public Unknown_3 GenComponentStyleHook(Unknown_16 token)
        {
            var zIndexPopupBase = token.ZIndexPopupBase;
            return new Unknown_17()
            {
                ZIndexPopup = zIndexPopupBase + 60,
            };
        }

    }

}