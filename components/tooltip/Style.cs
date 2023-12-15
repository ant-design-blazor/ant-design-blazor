using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Zoom;
using static AntDesign.PlacementArrow;

namespace AntDesign
{
    public partial class TooltipToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public string ColorBgDefault
        {
            get => (string)_tokens["colorBgDefault"];
            set => _tokens["colorBgDefault"] = value;
        }

    }

    public partial class TooltipToken : TokenWithCommonCls
    {
        public double TooltipMaxWidth
        {
            get => (double)_tokens["tooltipMaxWidth"];
            set => _tokens["tooltipMaxWidth"] = value;
        }

        public string TooltipColor
        {
            get => (string)_tokens["tooltipColor"];
            set => _tokens["tooltipColor"] = value;
        }

        public string TooltipBg
        {
            get => (string)_tokens["tooltipBg"];
            set => _tokens["tooltipBg"] = value;
        }

        public double TooltipBorderRadius
        {
            get => (double)_tokens["tooltipBorderRadius"];
            set => _tokens["tooltipBorderRadius"] = value;
        }

        public double TooltipRadiusOuter
        {
            get => (double)_tokens["tooltipRadiusOuter"];
            set => _tokens["tooltipRadiusOuter"] = value;
        }

    }

    public partial class Tooltip
    {
        public CSSInterpolation[] GenTooltipStyle(TooltipToken token)
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
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        ZIndex = zIndexPopup,
                        Display = "block",
                        Width = "max-content",
                        MaxWidth = tooltipMaxWidth,
                        Visibility = "visible",
                        TransformOrigin = @$"var(--arrow-x, 50%) var(--arrow-y, 50%)",
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        ["--antd-arrow-background-color"] = tooltipBg,
                        [$"{componentCls}-inner"] = new CSSObject()
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
                            BoxSizing = "border-box",
                        },
                        [new []{"&-placement-left","&-placement-leftTop","&-placement-leftBottom","&-placement-right","&-placement-rightTop","&-placement-rightBottom"}.Join(",")] = new CSSObject()
                        {
                            [$"{componentCls}-inner"] = new CSSObject()
                            {
                                BorderRadius = Math.Min(tooltipBorderRadius, MAX_VERTICAL_CONTENT_RADIUS),
                            },
                        },
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Position = "relative",
                        },
                        ["..."] = GenPresetColor(
                            token,
                            (colorKey, args) =>
                            {
                                var darkColor = args.DarkColor;
                                return new CSSObject()
                                {
                                    [$"&{componentCls}-{colorKey}"] = new CSSObject()
                                    {
                                        [$"{componentCls}-inner"] = new CSSObject()
                                        {
                                            BackgroundColor = darkColor,
                                        },
                                        [$"{componentCls}-arrow"] = new CSSObject()
                                        {
                                            ["--antd-arrow-background-color"] = darkColor,
                                        },
                                    },
                                };
                            }),
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                        },
                    },
                },
                GetArrowStyle(
                    MergeToken(
                        token,
                        new TooltipToken()
                        {
                            BorderRadiusOuter = tooltipRadiusOuter,
                        }),
                    new PlacementArrowOptions()
                    {
                        ColorBg = "var(--antd-arrow-background-color)",
                        ContentRadius = tooltipBorderRadius,
                        LimitVerticalRadius = true,
                    }),
                new CSSObject()
                {
                    [$"{componentCls}-pure"] = new CSSObject()
                    {
                        Position = "relative",
                        MaxWidth = "none",
                        Margin = token.SizePopupArrow,
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Tooltip",
                (token) =>
                {
                    // if (injectStyle === false) {
                    //         return [];
                    //       }
                    var borderRadius = token.BorderRadius;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorBgDefault = token.ColorBgDefault;
                    var borderRadiusOuter = token.BorderRadiusOuter;
                    var tooltipToken = MergeToken(
                        token,
                        new TooltipToken()
                        {
                            TooltipMaxWidth = 250,
                            TooltipColor = colorTextLightSolid,
                            TooltipBorderRadius = borderRadius,
                            TooltipBg = colorBgDefault,
                            TooltipRadiusOuter = borderRadiusOuter > 4 ? 4 : borderRadiusOuter
                        });
                    return new CSSInterpolation[]
                    {
                        GenTooltipStyle(tooltipToken),
                        InitZoomMotion(token, "zoom-big-fast"),
                    };
                },
                (args) =>
                {
                    var zIndexPopupBase = args.ZIndexPopupBase;
                    var colorBgSpotlight = args.ColorBgSpotlight;
                    return new TooltipToken()
                    {
                        ZIndexPopup = zIndexPopupBase + 70,
                        ColorBgDefault = colorBgSpotlight,
                    };
                },
                new GenOptions()
                {
                    ResetStyle = false,
                });
        }

    }

}
