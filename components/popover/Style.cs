using System;
using System.Linq;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Zoom;
using static AntDesign.PlacementArrow;

namespace AntDesign
{
    public partial class PopoverToken
    {
        public double Width
        {
            get => (double)_tokens["width"];
            set => _tokens["width"] = value;
        }

        public double MinWidth
        {
            get => (double)_tokens["minWidth"];
            set => _tokens["minWidth"] = value;
        }

        public double TitleMinWidth
        {
            get => (double)_tokens["titleMinWidth"];
            set => _tokens["titleMinWidth"] = value;
        }

        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

    }

    public partial class PopoverToken : TokenWithCommonCls
    {
        public string PopoverBg
        {
            get => (string)_tokens["popoverBg"];
            set => _tokens["popoverBg"] = value;
        }

        public string PopoverColor
        {
            get => (string)_tokens["popoverColor"];
            set => _tokens["popoverColor"] = value;
        }

        public double PopoverPadding
        {
            get => (double)_tokens["popoverPadding"];
            set => _tokens["popoverPadding"] = value;
        }

    }

    public partial class PopoverStyle
    {
        public static CSSInterpolation[] GenBaseStyle(PopoverToken token)
        {
            var componentCls = token.ComponentCls;
            var popoverColor = token.PopoverColor;
            var titleMinWidth = token.TitleMinWidth;
            var fontWeightStrong = token.FontWeightStrong;
            var popoverPadding = token.PopoverPadding;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var colorTextHeading = token.ColorTextHeading;
            var borderRadius = token.BorderRadiusLG;
            var zIndexPopup = token.ZIndexPopup;
            var marginXS = token.MarginXS;
            var colorBgElevated = token.ColorBgElevated;
            var popoverBg = token.PopoverBg;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        Top = 0,
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        ZIndex = zIndexPopup,
                        FontWeight = "normal",
                        WhiteSpace = "normal",
                        TextAlign = "start",
                        Cursor = "auto",
                        UserSelect = "text",
                        TransformOrigin = @$"var(--arrow-x, 50%) var(--arrow-y, 50%)",
                        ["--antd-arrow-background-color"] = colorBgElevated,
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                        },
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Position = "relative",
                        },
                        [$"{componentCls}-inner"] = new CSSObject()
                        {
                            BackgroundColor = popoverBg,
                            BackgroundClip = "padding-box",
                            BorderRadius = borderRadius,
                            BoxShadow = boxShadowSecondary,
                            Padding = popoverPadding,
                        },
                        [$"{componentCls}-title"] = new CSSObject()
                        {
                            MinWidth = titleMinWidth,
                            MarginBottom = marginXS,
                            Color = colorTextHeading,
                            FontWeight = fontWeightStrong,
                        },
                        [$"{componentCls}-inner-content"] = new CSSObject()
                        {
                            Color = popoverColor,
                        },
                    },
                },
                GetArrowStyle(
                    token,
                    new PlacementArrowOptions()
                    {
                        ColorBg = "var(--antd-arrow-background-color)",
                    }),
                new CSSObject()
                {
                    [$"{componentCls}-pure"] = new CSSObject()
                    {
                        Position = "relative",
                        MaxWidth = "none",
                        Margin = token.SizePopupArrow,
                        Display = "inline-block",
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Display = "inline-block",
                        },
                    },
                },
            };
        }

        public static CSSObject GenColorStyle(PopoverToken token)
        {
            var componentCls = token.ComponentCls;

            return new CSSObject()
            {
                [componentCls] = Enum.GetNames(typeof(PresetColor)).Select(x =>
                {
                    var colorKey = x.ToLower();
                    var lightColor = token[$"{colorKey}6"].ToString();
                    CSSInterpolation css = new CSSObject()
                    {
                        [$"&{componentCls}-{colorKey}"] = new CSSObject()
                        {
                            ["--antd-arrow-background-color"] = lightColor,
                            [$"{componentCls}-inner"] = new CSSObject()
                            {
                                BackgroundColor = lightColor,
                            },
                            [$"{componentCls}-arrow"] = new CSSObject()
                            {
                                BackgroundColor = "transparent",
                            },
                        }
                    };
                    return css;
                }).ToArray(),
            };
        }

        public static CSSObject GenWireframeStyle(PopoverToken token)
        {
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorSplit = token.ColorSplit;
            var paddingSM = token.PaddingSM;
            var controlHeight = token.ControlHeight;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var padding = token.Padding;
            var titlePaddingBlockDist = controlHeight - Math.Round(fontSize * lineHeight);
            var popoverTitlePaddingBlockTop = titlePaddingBlockDist / 2;
            var popoverTitlePaddingBlockBottom = titlePaddingBlockDist / 2 - lineWidth;
            var popoverPaddingHorizontal = padding;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"{componentCls}-inner"] = new CSSObject()
                    {
                        Padding = 0,
                    },
                    [$"{componentCls}-title"] = new CSSObject()
                    {
                        Margin = 0,
                        Padding = @$"{popoverTitlePaddingBlockTop}px {popoverPaddingHorizontal}px {popoverTitlePaddingBlockBottom}px",
                        BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                    },
                    [$"{componentCls}-inner-content"] = new CSSObject()
                    {
                        Padding = @$"{paddingSM}px {popoverPaddingHorizontal}px",
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Popover",
                (token) =>
                {
                    var colorBgElevated = token.ColorBgElevated;
                    var colorText = token.ColorText;
                    var wireframe = token.Wireframe;
                    var popoverToken = MergeToken(
                        token,
                        new PopoverToken()
                        {
                            PopoverPadding = 12,
                            PopoverBg = colorBgElevated,
                            PopoverColor = colorText,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(popoverToken),
                        GenColorStyle(popoverToken),
                        wireframe ? GenWireframeStyle(popoverToken) : null,
                        InitZoomMotion(popoverToken, "zoom-big"),
                    };
                },
                (token) =>
                {
                    return new PopoverToken()
                    {
                        Width = 177,
                        MinWidth = 177,
                        TitleMinWidth = 177,
                        ZIndexPopup = token.ZIndexPopupBase + 30,
                    };
                },
                new GenOptions()
                {
                    ResetStyle = false,
                    DeprecatedTokens = new ()
                    {
                        ("width", "titleMinWidth"),
                        ("minWidth", "titleMinWidth"),
                    }
                });
        }

    }

}
