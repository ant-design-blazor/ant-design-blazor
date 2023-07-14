using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TooltipToken
    {
        public int ZIndexPopup { get; set; }

        public string ColorBgDefault { get; set; }

    }

    public partial class TooltipToken : TokenWithCommonCls
    {
        public int TooltipMaxWidth { get; set; }

        public string TooltipColor { get; set; }

        public string TooltipBg { get; set; }

        public int TooltipBorderRadius { get; set; }

        public int TooltipRadiusOuter { get; set; }

    }

    public partial class Tooltip
    {
        public Unknown_2 GenTooltipStyle(Unknown_3 token)
        {
            var componentCls = token.ComponentCls;
            var tooltipMaxWidth = token.TooltipMaxWidth;
            var tooltipColor = token.TooltipColor;
            var tooltipBg = token.TooltipBg;
            var tooltipBorderRadius = token.TooltipBorderRadius;
            var zIndexPopup = token.ZIndexPopup;
            var controlHeight = token.ControlHeight;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var paddingSM = token.PaddingSM;
            var paddingXS = token.PaddingXS;
            var tooltipRadiusOuter = token.TooltipRadiusOuter;
            return new Unknown_4
            {
                new Unknown_5()
                {
                    [componentCls] = new Unknown_6()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        ZIndex = zIndexPopup,
                        Display = "block",
                        Width = "max-content",
                        MaxWidth = tooltipMaxWidth,
                        Visibility = "visible",
                        TransformOrigin = @$"var(--arrow-x, 50%) var(--arrow-y, 50%)",
                        ["&-hidden"] = new Unknown_7()
                        {
                            Display = "none",
                        },
                        ["--antd-arrow-background-color"] = tooltipBg,
                        [$"{componentCls}-inner"] = new Unknown_8()
                        {
                            MinWidth = controlHeight,
                            MinHeight = controlHeight,
                            Padding = @$"{paddingSM / 2}px {paddingXS}px",
                            Color = tooltipColor,
                            TextAlign = "start",
                            TextDecoration = "none",
                            WordWrap = "break-word",
                            BackgroundColor = tooltipBg,
                            BorderRadius = tooltipBorderRadius,
                            BoxShadow = boxShadowSecondary,
                        },
                        [["&-placement-left","&-placement-leftTop","&-placement-leftBottom","&-placement-right","&-placement-rightTop","&-placement-rightBottom",].join(",")] = new Unknown_9()
                        {
                            [$"{componentCls}-inner"] = new Unknown_10()
                            {
                                BorderRadius = Math.Min(tooltipBorderRadius, MAX_VERTICAL_CONTENT_RADIUS)
                            },
                        },
                        [$"{componentCls}-content"] = new Unknown_11()
                        {
                            Position = "relative",
                        },
                        ["..."] = GenPresetColor(
                            token,
                            (colorKey, args) => {
                                var darkColor = args.DarkColor;
                                return new Unknown_12()
                                {
                                    [$"&{componentCls}-{colorKey}"] = new Unknown_13()
                                    {
                                        [$"{componentCls}-inner"] = new Unknown_14()
                                        {
                                            BackgroundColor = darkColor,
                                        },
                                        [$"{componentCls}-arrow"] = new Unknown_15()
                                        {
                                            ["--antd-arrow-background-color"] = darkColor,
                                        },
                                    },
                                };
                            }),
                        ["&-rtl"] = new Unknown_16()
                        {
                            Direction = "rtl",
                        },
                    },
                },
                GetArrowStyle<TooltipToken>(
      mergeToken<TooltipToken>(token, {
        borderRadiusOuter: tooltipRadiusOuter,
      }),
      {
        colorBg: "var(--antd-arrow-background-color)",
        contentRadius: tooltipBorderRadius,
        limitVerticalRadius: true,
      },
    ),
                new Unknown_17()
                {
                    [$"{componentCls}-pure"] = new Unknown_18()
                    {
                        Position = "relative",
                        MaxWidth = "none",
                        Margin = token.SizePopupArrow,
                    },
                },
            };
        }

    }

}