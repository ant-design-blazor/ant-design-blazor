using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using Keyframes = CssInCs.Keyframe;

namespace AntDesign
{
    public partial class BadgeToken : TokenWithCommonCls
    {
        public int BadgeFontHeight { get; set; }

        public string BadgeZIndex { get; set; }

        public int BadgeHeight { get; set; }

        public int BadgeHeightSm { get; set; }

        public string BadgeTextColor { get; set; }

        public string BadgeFontWeight { get; set; }

        public int BadgeFontSize { get; set; }

        public string BadgeColor { get; set; }

        public string BadgeColorHover { get; set; }

        public int BadgeDotSize { get; set; }

        public int BadgeFontSizeSm { get; set; }

        public int BadgeStatusSize { get; set; }

        public int BadgeShadowSize { get; set; }

        public string BadgeShadowColor { get; set; }

        public string BadgeProcessingDuration { get; set; }

        public int BadgeRibbonOffset { get; set; }

        public string BadgeRibbonCornerTransform { get; set; }

        public string BadgeRibbonCornerFilter { get; set; }

    }

    public partial class Badge
    {
        private Keyframes antStatusProcessing = new Keyframes("antStatusProcessing")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(0.8)",
                Opacity = 0.5f,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(2.4)",
                Opacity = 0,
            },
        };

        private Keyframes antZoomBadgeIn = new Keyframes("antZoomBadgeIn")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(0) translate(50%, -50%)",
                Opacity = 0,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(1) translate(50%, -50%)",
            },
        };

        private Keyframes antZoomBadgeOut = new Keyframes("antZoomBadgeOut")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(1) translate(50%, -50%)",
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(0) translate(50%, -50%)",
                Opacity = 0,
            },
        };

        private Keyframes antNoWrapperZoomBadgeIn = new Keyframes("antNoWrapperZoomBadgeIn")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(0)",
                Opacity = 0,
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(1)",
            },
        };

        private Keyframes antNoWrapperZoomBadgeOut = new Keyframes("antNoWrapperZoomBadgeOut")
        {
            ["0%"] = new CSSObject()
            {
                Transform = "scale(1)",
            },
            ["100%"] = new CSSObject()
            {
                Transform = "scale(0)",
                Opacity = 0,
            },
        };

        private Keyframes antBadgeLoadingCircle = new Keyframes("antBadgeLoadingCircle")
        {
            ["0%"] = new CSSObject()
            {
                TransformOrigin = "50%",
            },
            ["100%"] = new CSSObject()
            {
                Transform = "translate(50%, -50%) rotate(360deg)",
                TransformOrigin = "50%",
            },
        };

        public CSSObject GenSharedBadgeStyle(BadgeToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var antCls = token.AntCls;
            var badgeFontHeight = token.BadgeFontHeight;
            var badgeShadowSize = token.BadgeShadowSize;
            var badgeHeightSm = token.BadgeHeightSm;
            var motionDurationSlow = token.MotionDurationSlow;
            var badgeStatusSize = token.BadgeStatusSize;
            var marginXS = token.MarginXS;
            var badgeRibbonOffset = token.BadgeRibbonOffset;
            var numberPrefixCls = @$"{antCls}-scroll-number";
            var ribbonPrefixCls = @$"{antCls}-ribbon";
            var ribbonWrapperPrefixCls = @$"{antCls}-ribbon-wrapper";
            var colorPreset = GenPresetColor(
                token,
                (colorKey, args) => {
                    var darkColor = args.DarkColor;
                    return new CSSObject()
                    {
                        [$"{componentCls}-color-{colorKey}"] = new CSSObject()
                        {
                            Background = darkColor,
                        },
                    };
                });
            var statusRibbonPreset = GenPresetColor(
                token,
                (colorKey, args) => {
                    var darkColor = args.DarkColor;
                    return new CSSObject()
                    {
                        [$"&{ribbonPrefixCls}-color-{colorKey}"] = new CSSObject()
                        {
                            Background = darkColor,
                            Color = darkColor,
                        },
                    };
                });
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    Width = "fit-content",
                    LineHeight = 1,
                    [$"{componentCls}-count"] = new CSSObject()
                    {
                        ZIndex = token.BadgeZIndex,
                        MinWidth = token.BadgeHeight,
                        Height = token.BadgeHeight,
                        Color = token.BadgeTextColor,
                        FontWeight = token.BadgeFontWeight,
                        FontSize = token.BadgeFontSize,
                        LineHeight = @$"{token.BadgeHeight}px",
                        WhiteSpace = "nowrap",
                        TextAlign = "center",
                        Background = token.BadgeColor,
                        BorderRadius = token.BadgeHeight / 2,
                        BoxShadow = @$"0 0 0 {badgeShadowSize}px {token.BadgeShadowColor}",
                        Transition = @$"background {token.MotionDurationMid}",
                        ["a"] = new CSSObject()
                        {
                            Color = token.BadgeTextColor,
                        },
                        ["a:hover"] = new CSSObject()
                        {
                            Color = token.BadgeTextColor,
                        },
                        ["a:hover &"] = new CSSObject()
                        {
                            Background = token.BadgeColorHover,
                        },
                    },
                    [$"{componentCls}-count-sm"] = new CSSObject()
                    {
                        MinWidth = badgeHeightSm,
                        Height = badgeHeightSm,
                        FontSize = token.BadgeFontSizeSm,
                        LineHeight = @$"{badgeHeightSm}px",
                        BorderRadius = badgeHeightSm / 2,
                    },
                    [$"{componentCls}-multiple-words"] = new CSSObject()
                    {
                        Padding = @$"0 {token.PaddingXS}px",
                    },
                    [$"{componentCls}-dot"] = new CSSObject()
                    {
                        ZIndex = token.BadgeZIndex,
                        Width = token.BadgeDotSize,
                        MinWidth = token.BadgeDotSize,
                        Height = token.BadgeDotSize,
                        Background = token.BadgeColor,
                        BorderRadius = "100%",
                        BoxShadow = @$"0 0 0 {badgeShadowSize}px {token.BadgeShadowColor}",
                    },
                    [$"{componentCls}-dot{numberPrefixCls}"] = new CSSObject()
                    {
                        Transition = @$"background {motionDurationSlow}",
                    },
                    [$"{componentCls}-count, {componentCls}-dot, {numberPrefixCls}-custom-component"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = 0,
                        Transform = "translate(50%, -50%)",
                        TransformOrigin = "100% 0%",
                        [$"{iconCls}-spin"] = new CSSObject()
                        {
                            AnimationName = antBadgeLoadingCircle,
                            AnimationDuration = token.MotionDurationMid,
                            AnimationIterationCount = "infinite",
                            AnimationTimingFunction = "linear",
                        },
                    },
                    [$"&{componentCls}-status"] = new CSSObject()
                    {
                        LineHeight = "inherit",
                        VerticalAlign = "baseline",
                        [$"{componentCls}-status-dot"] = new CSSObject()
                        {
                            Position = "relative",
                            Top = -1,
                            Display = "inline-block",
                            Width = badgeStatusSize,
                            Height = badgeStatusSize,
                            VerticalAlign = "middle",
                            BorderRadius = "50%",
                        },
                        [$"{componentCls}-status-success"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorSuccess,
                        },
                        [$"{componentCls}-status-processing"] = new CSSObject()
                        {
                            Position = "relative",
                            Color = token.ColorPrimary,
                            BackgroundColor = token.ColorPrimary,
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = 0,
                                InsetInlineStart = 0,
                                Width = "100%",
                                Height = "100%",
                                BorderWidth = badgeShadowSize,
                                BorderStyle = "solid",
                                BorderColor = "inherit",
                                BorderRadius = "50%",
                                AnimationName = antStatusProcessing,
                                AnimationDuration = token.BadgeProcessingDuration,
                                AnimationIterationCount = "infinite",
                                AnimationTimingFunction = "ease-in-out",
                                Content = "\"\"",
                            },
                        },
                        [$"{componentCls}-status-default"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorTextPlaceholder,
                        },
                        [$"{componentCls}-status-error"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorError,
                        },
                        [$"{componentCls}-status-warning"] = new CSSObject()
                        {
                            BackgroundColor = token.ColorWarning,
                        },
                        [$"{componentCls}-status-text"] = new CSSObject()
                        {
                            MarginInlineStart = marginXS,
                            Color = token.ColorText,
                            FontSize = token.FontSize,
                        },
                    },
                    ["..."] = colorPreset,
                    [$"{componentCls}-zoom-appear, {componentCls}-zoom-enter"] = new CSSObject()
                    {
                        AnimationName = antZoomBadgeIn,
                        AnimationDuration = token.MotionDurationSlow,
                        AnimationTimingFunction = token.MotionEaseOutBack,
                        AnimationFillMode = "both",
                    },
                    [$"{componentCls}-zoom-leave"] = new CSSObject()
                    {
                        AnimationName = antZoomBadgeOut,
                        AnimationDuration = token.MotionDurationSlow,
                        AnimationTimingFunction = token.MotionEaseOutBack,
                        AnimationFillMode = "both",
                    },
                    [$"&{componentCls}-not-a-wrapper"] = new CSSObject()
                    {
                        [$"{componentCls}-zoom-appear, {componentCls}-zoom-enter"] = new CSSObject()
                        {
                            AnimationName = antNoWrapperZoomBadgeIn,
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = token.MotionEaseOutBack,
                        },
                        [$"{componentCls}-zoom-leave"] = new CSSObject()
                        {
                            AnimationName = antNoWrapperZoomBadgeOut,
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = token.MotionEaseOutBack,
                        },
                        [$"&:not({componentCls}-status)"] = new CSSObject()
                        {
                            VerticalAlign = "middle",
                        },
                        [$"{numberPrefixCls}-custom-component, {componentCls}-count"] = new CSSObject()
                        {
                            Transform = "none",
                        },
                        [$"{numberPrefixCls}-custom-component, {numberPrefixCls}"] = new CSSObject()
                        {
                            Position = "relative",
                            Top = "auto",
                            Display = "block",
                            TransformOrigin = "50% 50%",
                        },
                    },
                    [$"{numberPrefixCls}"] = new CSSObject()
                    {
                        Overflow = "hidden",
                        [$"{numberPrefixCls}-only"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-block",
                            Height = token.BadgeHeight,
                            Transition = @$"all {token.MotionDurationSlow} {token.MotionEaseOutBack}",
                            WebkitTransformStyle = "preserve-3d",
                            WebkitBackfaceVisibility = "hidden",
                            [$"> p{numberPrefixCls}-only-unit"] = new CSSObject()
                            {
                                Height = token.BadgeHeight,
                                Margin = 0,
                                WebkitTransformStyle = "preserve-3d",
                                WebkitBackfaceVisibility = "hidden",
                            },
                        },
                        [$"{numberPrefixCls}-symbol"] = new CSSObject()
                        {
                            VerticalAlign = "top",
                        },
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                        [$"{componentCls}-count, {componentCls}-dot, {numberPrefixCls}-custom-component"] = new CSSObject()
                        {
                            Transform = "translate(-50%, -50%)",
                        },
                    },
                },
                [$"{ribbonWrapperPrefixCls}"] = new CSSObject()
                {
                    Position = "relative",
                },
                [$"{ribbonPrefixCls}"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "absolute",
                    Top = marginXS,
                    Padding = @$"0 {token.PaddingXS}px",
                    Color = token.ColorPrimary,
                    LineHeight = @$"{badgeFontHeight}px",
                    WhiteSpace = "nowrap",
                    BackgroundColor = token.ColorPrimary,
                    BorderRadius = token.BorderRadiusSM,
                    [$"{ribbonPrefixCls}-text"] = new CSSObject()
                    {
                        Color = token.ColorTextLightSolid,
                    },
                    [$"{ribbonPrefixCls}-corner"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = "100%",
                        Width = badgeRibbonOffset,
                        Height = badgeRibbonOffset,
                        Color = "currentcolor",
                        Border = @$"{badgeRibbonOffset / 2}px solid",
                        Transform = token.BadgeRibbonCornerTransform,
                        TransformOrigin = "top",
                        Filter = token.BadgeRibbonCornerFilter,
                    },
                    ["..."] = statusRibbonPreset,
                    [$"&{ribbonPrefixCls}-placement-end"] = new CSSObject()
                    {
                        InsetInlineEnd = -badgeRibbonOffset,
                        BorderEndEndRadius = 0,
                        [$"{ribbonPrefixCls}-corner"] = new CSSObject()
                        {
                            InsetInlineEnd = 0,
                            BorderInlineEndColor = "transparent",
                            BorderBlockEndColor = "transparent",
                        },
                    },
                    [$"&{ribbonPrefixCls}-placement-start"] = new CSSObject()
                    {
                        InsetInlineStart = -badgeRibbonOffset,
                        BorderEndStartRadius = 0,
                        [$"{ribbonPrefixCls}-corner"] = new CSSObject()
                        {
                            InsetInlineStart = 0,
                            BorderBlockEndColor = "transparent",
                            BorderInlineStartColor = "transparent",
                        },
                    },
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var fontSizeSM = token.FontSizeSM;
            var lineWidth = token.LineWidth;
            var marginXS = token.MarginXS;
            var colorBorderBg = token.ColorBorderBg;
            var badgeFontHeight = (int)Math.Round((double)fontSize * lineHeight);
            var badgeShadowSize = lineWidth;
            var badgeZIndex = "auto";
            var badgeHeight = badgeFontHeight - 2 * badgeShadowSize;
            var badgeTextColor = token.ColorBgContainer;
            var badgeFontWeight = "normal";
            var badgeFontSize = fontSizeSM;
            var badgeColor = token.ColorError;
            var badgeColorHover = token.ColorErrorHover;
            var badgeHeightSm = fontSize;
            var badgeDotSize = fontSizeSM / 2;
            var badgeFontSizeSm = fontSizeSM;
            var badgeStatusSize = fontSizeSM / 2;
            var badgeToken = MergeToken(
                token,
                new BadgeToken()
                {
                    BadgeFontHeight = badgeFontHeight,
                    BadgeShadowSize = badgeShadowSize,
                    BadgeZIndex = badgeZIndex,
                    BadgeHeight = badgeHeight,
                    BadgeTextColor = badgeTextColor,
                    BadgeFontWeight = badgeFontWeight,
                    BadgeFontSize = badgeFontSize,
                    BadgeColor = badgeColor,
                    BadgeColorHover = badgeColorHover,
                    BadgeShadowColor = colorBorderBg,
                    BadgeHeightSm = badgeHeightSm,
                    BadgeDotSize = badgeDotSize,
                    BadgeFontSizeSm = badgeFontSizeSm,
                    BadgeStatusSize = badgeStatusSize,
                    BadgeProcessingDuration = "1.2s",
                    BadgeRibbonOffset = marginXS,
                    BadgeRibbonCornerTransform = "scaleY(0.75)",
                    BadgeRibbonCornerFilter = @$"brightness(75%)",
                });
            return new CSSInterpolation[] { GenSharedBadgeStyle(badgeToken) };
        }

    }

}