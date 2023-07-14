using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class PopoverToken
    {
        public int ZIndexPopup { get; set; }

        public int Width { get; set; }

    }

    public partial class PopoverToken : TokenWithCommonCls
    {
        public string PopoverBg { get; set; }

        public string PopoverColor { get; set; }

        public string PopoverPadding { get; set; }

    }

    public partial class Popover
    {
        public Unknown_1 GenBaseStyle(Unknown_7 token)
        {
            var componentCls = token.ComponentCls;
            var popoverBg = token.PopoverBg;
            var popoverColor = token.PopoverColor;
            var width = token.Width;
            var fontWeightStrong = token.FontWeightStrong;
            var popoverPadding = token.PopoverPadding;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var colorTextHeading = token.ColorTextHeading;
            var borderRadius = token.BorderRadiusLG;
            var zIndexPopup = token.ZIndexPopup;
            var marginXS = token.MarginXS;
            var colorBgElevated = token.ColorBgElevated;
            return new Unknown_8
            {
                new Unknown_9()
                {
                    [componentCls] = new Unknown_10()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        Top = 0,
                        Left = new Unknown_11()
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
                        ["&-rtl"] = new Unknown_12()
                        {
                            Direction = "rtl",
                        },
                        ["&-hidden"] = new Unknown_13()
                        {
                            Display = "none",
                        },
                        [$"{componentCls}-content"] = new Unknown_14()
                        {
                            Position = "relative",
                        },
                        [$"{componentCls}-inner"] = new Unknown_15()
                        {
                            BackgroundColor = popoverBg,
                            BackgroundClip = "padding-box",
                            BorderRadius = borderRadius,
                            BoxShadow = boxShadowSecondary,
                            Padding = popoverPadding,
                        },
                        [$"{componentCls}-title"] = new Unknown_16()
                        {
                            MinWidth = width,
                            MarginBottom = marginXS,
                            Color = colorTextHeading,
                            FontWeight = fontWeightStrong,
                        },
                        [$"{componentCls}-inner-content"] = new Unknown_17()
                        {
                            Color = popoverColor,
                        },
                    },
                },
                GetArrowStyle(token, {
      colorBg: "var(--antd-arrow-background-color)",
    }),
                new Unknown_18()
                {
                    [$"{componentCls}-pure"] = new Unknown_19()
                    {
                        Position = "relative",
                        MaxWidth = "none",
                        Margin = token.SizePopupArrow,
                        Display = "inline-block",
                        [$"{componentCls}-content"] = new Unknown_20()
                        {
                            Display = "inline-block",
                        },
                    },
                },
            };
        }

        public Unknown_3 GenColorStyle(Unknown_21 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_22()
            {
                [componentCls] = PresetColors.Map(
                    (colorKey) => {
                        var lightColor = token[@$"{colorKey}6"];
                        return new Unknown_23()
                        {
                            [$"&{componentCls}-{colorKey}"] = new Unknown_24()
                            {
                                ["--antd-arrow-background-color"] = lightColor,
                                [$"{componentCls}-inner"] = new Unknown_25()
                                {
                                    BackgroundColor = lightColor,
                                },
                                [$"{componentCls}-arrow"] = new Unknown_26()
                                {
                                    Background = "transparent",
                                },
                            },
                        };
                    })
            };
        }

        public Unknown_4 GenWireframeStyle(Unknown_27 token)
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
            var titlePaddingBlockDist = ControlHeight - Math.Round(fontSize * lineHeight);
            var popoverTitlePaddingBlockTop = titlePaddingBlockDist / 2;
            var popoverTitlePaddingBlockBottom = titlePaddingBlockDist / 2 - lineWidth;
            var popoverPaddingHorizontal = padding;
            return new Unknown_28()
            {
                [componentCls] = new Unknown_29()
                {
                    [$"{componentCls}-inner"] = new Unknown_30()
                    {
                        Padding = 0,
                    },
                    [$"{componentCls}-title"] = new Unknown_31()
                    {
                        Margin = 0,
                        Padding = @$"{popoverTitlePaddingBlockTop}px {popoverPaddingHorizontal}px {popoverTitlePaddingBlockBottom}px",
                        BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                    },
                    [$"{componentCls}-inner-content"] = new Unknown_32()
                    {
                        Padding = @$"{paddingSM}px {popoverPaddingHorizontal}px",
                    },
                },
            };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_33 token)
        {
            var colorBgElevated = token.ColorBgElevated;
            var colorText = token.ColorText;
            var wireframe = token.Wireframe;
            var popoverToken = MergeToken(
                token,
                new Unknown_34()
                {
                    PopoverBg = colorBgElevated,
                    PopoverColor = colorText,
                    PopoverPadding = 12,
                });
            return new Unknown_35
            {
                GenBaseStyle(popoverToken),
                GenColorStyle(popoverToken),
                Wireframe && genWireframeStyle(popoverToken),
                InitZoomMotion(popoverToken, "zoom-big")
            };
        }

        public Unknown_6 GenComponentStyleHook(Unknown_36 args)
        {
            return new Unknown_37()
            {
                ZIndexPopup = zIndexPopupBase + 30,
                Width = 177,
            };
        }

    }

}