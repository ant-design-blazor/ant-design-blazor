using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class PopconfirmToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

    }

    public partial class PopconfirmToken : TokenWithCommonCls
    {
    }

    public partial class Popconfirm
    {
        public CSSObject GenBaseStyle(PopconfirmToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var antCls = token.AntCls;
            var zIndexPopup = token.ZIndexPopup;
            var colorText = token.ColorText;
            var colorWarning = token.ColorWarning;
            var marginXXS = token.MarginXXS;
            var marginXS = token.MarginXS;
            var fontSize = token.FontSize;
            var fontWeightStrong = token.FontWeightStrong;
            var colorTextHeading = token.ColorTextHeading;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ZIndex = zIndexPopup,
                    [$"&{antCls}-popover"] = new CSSObject()
                    {
                        FontSize = fontSize,
                    },
                    [$"{componentCls}-message"] = new CSSObject()
                    {
                        MarginBottom = marginXS,
                        Display = "flex",
                        FlexWrap = "nowrap",
                        AlignItems = "start",
                        [$"> {componentCls}-message-icon {iconCls}"] = new CSSObject()
                        {
                            Color = colorWarning,
                            FontSize = fontSize,
                            LineHeight = 1,
                            MarginInlineEnd = marginXS,
                        },
                        [$"{componentCls}-title"] = new CSSObject()
                        {
                            FontWeight = fontWeightStrong,
                            Color = colorTextHeading,
                            ["&:only-child"] = new CSSObject()
                            {
                                FontWeight = "normal",
                            },
                        },
                        [$"{componentCls}-description"] = new CSSObject()
                        {
                            MarginTop = marginXXS,
                            Color = colorText,
                        },
                    },
                    [$"{componentCls}-buttons"] = new CSSObject()
                    {
                        TextAlign = "end",
                        WhiteSpace = "nowrap",
                        ["button"] = new CSSObject()
                        {
                            MarginInlineStart = marginXS,
                        },
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Popconfirm",
                (token) =>
                {
                    return GenBaseStyle(token);
                },
                (token) =>
                {
                    var zIndexPopupBase = token.ZIndexPopupBase;
                    return new PopconfirmToken()
                    {
                        ZIndexPopup = zIndexPopupBase + 60,
                    };
                },
                new GenOptions()
                {
                    ResetStyle = false,
                });
        }

    }

}