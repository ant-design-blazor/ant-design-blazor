using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public partial class NotificationToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public double Width
        {
            get => (double)_tokens["width"];
            set => _tokens["width"] = value;
        }

    }

    public partial class NotificationToken : TokenWithCommonCls
    {
        public double AnimationMaxHeight
        {
            get => (double)_tokens["animationMaxHeight"];
            set => _tokens["animationMaxHeight"] = value;
        }

        public string NotificationBg
        {
            get => (string)_tokens["notificationBg"];
            set => _tokens["notificationBg"] = value;
        }

        public string NotificationPadding
        {
            get => (string)_tokens["notificationPadding"];
            set => _tokens["notificationPadding"] = value;
        }

        public double NotificationPaddingVertical
        {
            get => (double)_tokens["notificationPaddingVertical"];
            set => _tokens["notificationPaddingVertical"] = value;
        }

        public double NotificationPaddingHorizontal
        {
            get => (double)_tokens["notificationPaddingHorizontal"];
            set => _tokens["notificationPaddingHorizontal"] = value;
        }

        public double NotificationIconSize
        {
            get => (double)_tokens["notificationIconSize"];
            set => _tokens["notificationIconSize"] = value;
        }

        public double NotificationCloseButtonSize
        {
            get => (double)_tokens["notificationCloseButtonSize"];
            set => _tokens["notificationCloseButtonSize"] = value;
        }

        public double NotificationMarginBottom
        {
            get => (double)_tokens["notificationMarginBottom"];
            set => _tokens["notificationMarginBottom"] = value;
        }

        public double NotificationMarginEdge
        {
            get => (double)_tokens["notificationMarginEdge"];
            set => _tokens["notificationMarginEdge"] = value;
        }

        public double NotificationStackLayer
        {
            get => (double)_tokens["notificationStackLayer"];
            set => _tokens["notificationStackLayer"] = value;
        }

    }

    public partial class Notification
    {
        public CSSObject GenNoticeStyle(NotificationToken token)
        {
            var iconCls = token.IconCls;
            var componentCls = token.ComponentCls;
            var boxShadow = token.BoxShadow;
            var fontSizeLG = token.FontSizeLG;
            var notificationMarginBottom = token.NotificationMarginBottom;
            var borderRadiusLG = token.BorderRadiusLG;
            var colorSuccess = token.ColorSuccess;
            var colorInfo = token.ColorInfo;
            var colorWarning = token.ColorWarning;
            var colorError = token.ColorError;
            var colorTextHeading = token.ColorTextHeading;
            var notificationBg = token.NotificationBg;
            var notificationPadding = token.NotificationPadding;
            var notificationMarginEdge = token.NotificationMarginEdge;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var width = token.Width;
            var notificationIconSize = token.NotificationIconSize;
            var colorText = token.ColorText;
            var noticeCls = @$"{componentCls}-notice";
            return new CSSObject()
            {
                Position = "relative",
                MarginBottom = notificationMarginBottom,
                MarginInlineStart = "auto",
                Background = notificationBg,
                BorderRadius = borderRadiusLG,
                BoxShadow = boxShadow,
                [noticeCls] = new CSSObject()
                {
                    Padding = notificationPadding,
                    Width = width,
                    MaxWidth = @$"calc(100vw - {notificationMarginEdge * 2}px)",
                    Overflow = "hidden",
                    LineHeight = lineHeight,
                    WordWrap = "break-word",
                },
                [$"{componentCls}-close-icon"] = new CSSObject()
                {
                    FontSize = fontSize,
                    Cursor = "pointer",
                },
                [$"{noticeCls}-message"] = new CSSObject()
                {
                    MarginBottom = token.MarginXS,
                    Color = colorTextHeading,
                    FontSize = fontSizeLG,
                    LineHeight = token.LineHeightLG,
                },
                [$"{noticeCls}-description"] = new CSSObject()
                {
                    FontSize = fontSize,
                    Color = colorText,
                },
                [$"&{noticeCls}-closable {noticeCls}-message"] = new CSSObject()
                {
                    PaddingInlineEnd = token.PaddingLG,
                },
                [$"{noticeCls}-with-icon {noticeCls}-message"] = new CSSObject()
                {
                    MarginBottom = token.MarginXS,
                    MarginInlineStart = token.MarginSM + notificationIconSize,
                    FontSize = fontSizeLG,
                },
                [$"{noticeCls}-with-icon {noticeCls}-description"] = new CSSObject()
                {
                    MarginInlineStart = token.MarginSM + notificationIconSize,
                    FontSize = fontSize,
                },
                [$"{noticeCls}-icon"] = new CSSObject()
                {
                    Position = "absolute",
                    FontSize = notificationIconSize,
                    LineHeight = 0,
                    [$"&-success{iconCls}"] = new CSSObject()
                    {
                        Color = colorSuccess,
                    },
                    [$"&-info{iconCls}"] = new CSSObject()
                    {
                        Color = colorInfo,
                    },
                    [$"&-warning{iconCls}"] = new CSSObject()
                    {
                        Color = colorWarning,
                    },
                    [$"&-error{iconCls}"] = new CSSObject()
                    {
                        Color = colorError,
                    },
                },
                [$"{noticeCls}-close"] = new CSSObject()
                {
                    Position = "absolute",
                    Top = token.NotificationPaddingVertical,
                    InsetInlineEnd = token.NotificationPaddingHorizontal,
                    Color = token.ColorIcon,
                    Outline = "none",
                    Width = token.NotificationCloseButtonSize,
                    Height = token.NotificationCloseButtonSize,
                    BorderRadius = token.BorderRadiusSM,
                    Transition = @$"background-color {token.MotionDurationMid}, color {token.MotionDurationMid}",
                    Display = "flex",
                    AlignItems = "center",
                    JustifyContent = "center",
                    ["&:hover"] = new CSSObject()
                    {
                        Color = token.ColorIconHover,
                        BackgroundColor = token.Wireframe ? "transparent" : token.ColorFillContent
                    },
                },
                [$"{noticeCls}-btn"] = new CSSObject()
                {
                    Float = "right",
                    MarginTop = token.MarginSM,
                },
            };
        }

        public CSSInterpolation GenNotificationStyle(NotificationToken token)
        {
            var componentCls = token.ComponentCls;
            var notificationMarginBottom = token.NotificationMarginBottom;
            var notificationMarginEdge = token.NotificationMarginEdge;
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var noticeCls = @$"{componentCls}-notice";
            var fadeOut = new Keyframe("antNotificationFadeOut",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        MaxHeight = token.AnimationMaxHeight,
                        MarginBottom = notificationMarginBottom,
                    },
                    ["100%"] = new CSSObject()
                    {
                        MaxHeight = 0,
                        MarginBottom = 0,
                        PaddingTop = 0,
                        PaddingBottom = 0,
                        Opacity = 0,
                    },
                });
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "fixed",
                        ZIndex = token.ZIndexPopup,
                        MarginRight = new PropertySkip()
                        {
                            Value = notificationMarginEdge,
                            SkipCheck = true,
                        },
                        [$"{componentCls}-hook-holder"] = new CSSObject()
                        {
                            Position = "relative",
                        },
                        [$"{componentCls}-fade-appear-prepare"] = new CSSObject()
                        {
                            Opacity = "0 !important",
                        },
                        [$"{componentCls}-fade-enter, {componentCls}-fade-appear"] = new CSSObject()
                        {
                            AnimationDuration = token.MotionDurationMid,
                            AnimationTimingFunction = motionEaseInOut,
                            AnimationFillMode = "both",
                            Opacity = 0,
                            AnimationPlayState = "paused",
                        },
                        [$"{componentCls}-fade-leave"] = new CSSObject()
                        {
                            AnimationTimingFunction = motionEaseInOut,
                            AnimationFillMode = "both",
                            AnimationDuration = motionDurationMid,
                            AnimationPlayState = "paused",
                        },
                        [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new CSSObject()
                        {
                            AnimationPlayState = "running",
                        },
                        [$"{componentCls}-fade-leave{componentCls}-fade-leave-active"] = new CSSObject()
                        {
                            AnimationName = fadeOut,
                            AnimationPlayState = "running",
                        },
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                            [$"{noticeCls}-btn"] = new CSSObject()
                            {
                                Float = "left",
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$"{noticeCls}-wrapper"] = new CSSObject()
                        {
                            ["..."] = GenNoticeStyle(token)
                        },
                    },
                },
            };
        }

        public NotificationToken PrepareComponentToken(GlobalToken token)
        {
            return new NotificationToken()
            {
                ZIndexPopup = token.ZIndexPopupBase + 100 * 10 + 50,
                Width = 384,
            };
        }

        public NotificationToken PrepareNotificationToken(TokenWithCommonCls token)
        {
            var notificationPaddingVertical = token.PaddingMD;
            var notificationPaddingHorizontal = token.PaddingLG;
            var notificationToken = MergeToken(
                token,
                new NotificationToken()
                {
                    NotificationBg = token.ColorBgElevated,
                    NotificationPaddingVertical = notificationPaddingVertical,
                    NotificationPaddingHorizontal = notificationPaddingHorizontal,
                    NotificationIconSize = token.FontSizeLG * token.LineHeightLG,
                    NotificationCloseButtonSize = token.ControlHeightLG * 0.55,
                    NotificationMarginBottom = token.Margin,
                    NotificationPadding = @$"{token.PaddingMD}px {token.PaddingContentHorizontalLG}px",
                    NotificationMarginEdge = token.MarginLG,
                    AnimationMaxHeight = 150,
                    NotificationStackLayer = 3,
                });
            return notificationToken;
        }

        public UseComponentStyleResult ExportDefault()
        {
            return GenComponentStyleHook(
                "Notification",
                (token) =>
                {
                    var notificationToken = PrepareNotificationToken(token);
                    return new CSSInterpolation[]
                    {
                        GenNotificationStyle(notificationToken),
                        GenNotificationPlacementStyle(notificationToken),
                        GenStackStyle(notificationToken),
                    };
                },
                PrepareComponentToken);
        }

        public CSSObject GenNotificationPlacementStyle(NotificationToken token)
        {
            var componentCls = token.ComponentCls;
            var notificationMarginEdge = token.NotificationMarginEdge;
            var animationMaxHeight = token.AnimationMaxHeight;
            var noticeCls = @$"{componentCls}-notice";
            var rightFadeIn = new Keyframe("antNotificationFadeIn",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        Transform = @$"translate3d(100%, 0, 0)",
                        Opacity = 0,
                    },
                    ["100%"] = new CSSObject()
                    {
                        Transform = @$"translate3d(0, 0, 0)",
                        Opacity = 1,
                    },
                });
            var topFadeIn = new Keyframe("antNotificationTopFadeIn",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        Top = -animationMaxHeight,
                        Opacity = 0,
                    },
                    ["100%"] = new CSSObject()
                    {
                        Top = 0,
                        Opacity = 1,
                    },
                });
            var bottomFadeIn = new Keyframe("antNotificationBottomFadeIn",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        Bottom = -animationMaxHeight,
                        Opacity = 0,
                    },
                    ["100%"] = new CSSObject()
                    {
                        Bottom = 0,
                        Opacity = 1,
                    },
                });
            var leftFadeIn = new Keyframe("antNotificationLeftFadeIn",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        Transform = @$"translate3d(-100%, 0, 0)",
                        Opacity = 0,
                    },
                    ["100%"] = new CSSObject()
                    {
                        Transform = @$"translate3d(0, 0, 0)",
                        Opacity = 1,
                    },
                });
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"&{componentCls}-top, &{componentCls}-bottom"] = new CSSObject()
                    {
                        MarginInline = 0,
                        [noticeCls] = new CSSObject()
                        {
                            MarginInline = "auto auto",
                        },
                    },
                    [$"&{componentCls}-top"] = new CSSObject()
                    {
                        [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new CSSObject()
                        {
                            AnimationName = topFadeIn,
                        },
                    },
                    [$"&{componentCls}-bottom"] = new CSSObject()
                    {
                        [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new CSSObject()
                        {
                            AnimationName = bottomFadeIn,
                        },
                    },
                    [$"&{componentCls}-topRight, &{componentCls}-bottomRight"] = new CSSObject()
                    {
                        [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new CSSObject()
                        {
                            AnimationName = rightFadeIn,
                        },
                    },
                    [$"&{componentCls}-topLeft, &{componentCls}-bottomLeft"] = new CSSObject()
                    {
                        MarginRight = new PropertySkip()
                        {
                            Value = 0,
                            SkipCheck = true,
                        },
                        MarginLeft = new PropertySkip()
                        {
                            Value = notificationMarginEdge,
                            SkipCheck = true,
                        },
                        [noticeCls] = new CSSObject()
                        {
                            MarginInlineEnd = "auto",
                            MarginInlineStart = 0,
                        },
                        [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new CSSObject()
                        {
                            AnimationName = leftFadeIn,
                        },
                    },
                },
            };
        }

        //public UseComponentStyleResult ExportDefault()
        //{
        //    return GenSubStyleComponent(
        //        ["Notification", "PurePanel"],
        //        (token) =>
        //        {
        //            var noticeCls = @$"{token.ComponentCls}-notice";
        //            var notificationToken = PrepareNotificationToken(token);
        //            return new Unknown7_1()
        //            {
        //                [$"{noticeCls}-pure-panel"] = new Unknown7_2()
        //                {
        //                    ["..."] = GenNoticeStyle(notificationToken),
        //                    Width = notificationToken.Width,
        //                    MaxWidth = @$"calc(100vw - {notificationToken.NotificationMarginEdge * 2}px)",
        //                    Margin = 0,
        //                },
        //            };
        //        },
        //        prepareComponentToken);
        //}

        private Dictionary<string, string> _placementAlignProperty = new Dictionary<string, string>()
        {
            { "", "" }
        };

        public CSSObject GenPlacementStackStyle(NotificationToken token, string placement)
        {
            var componentCls = token.ComponentCls;
            var p = _placementAlignProperty[placement];
            return new CSSObject()
            {
                [$"{componentCls}-{placement}"] = new CSSObject()
                {
                    [$"&{componentCls}-stack > {componentCls}-notice-wrapper"] = new CSSObject()
                    {
                        [placement.StartsWith("top") ? "top" : "bottom"] = 0,
                        [_placementAlignProperty[placement]] = 0,
                    },
                },
            };
        }

        public CSSObject GenStackChildrenStyle(NotificationToken token)
        {
            var childrenStyle = new CSSObject()
            {
            };
            for (var i = 1; i < token.NotificationStackLayer; i++)
            {
                childrenStyle[@$"&:nth-last-child({i + 1})"] = new CSSObject
                {
                    Overflow = "hidden",
                    [$"& > {token.ComponentCls}-notice"] = new CSSObject
                    {
                        Opacity = 0,
                        Transition = $"opacity {token.MotionDurationMid}",
                    },
                };
            };
            return new CSSObject()
            {
                [$"&:not(:nth-last-child(-n+{token.NotificationStackLayer}))"] = new CSSObject()
                {
                    Opacity = 0,
                    Overflow = "hidden",
                    Color = "transparent",
                    PointerEvents = "none",
                },
                ["..."] = childrenStyle,
            };
        }

        public CSSObject GenStackedNoticeStyle(NotificationToken token)
        {
            var childrenStyle = new CSSObject();
            for (var i = 1; i < token.NotificationStackLayer; i++)
            {
                childrenStyle[$"&:nth-last-child({i + 1})"] = new CSSObject
                {
                    Background = token.ColorBgBlur,
                    BackdropFilter = "blur(10px)",
                    ["-webkit-backdrop-filter"] = "blur(10px)",
                };
            }
            return new CSSObject()
            {
                ["..."] = childrenStyle,
            };
        }

        public static string[] NotificationPlacements = new string[]
        {
            "top",
            "topLeft",
            "topRight",
            "bottom",
            "bottomLeft",
            "bottomRight"
        };

        public CSSObject GenStackStyle(NotificationToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-stack"] = new CSSObject()
                {
                    [$"& > {componentCls}-notice-wrapper"] = new CSSObject()
                    {
                        Transition = @$"all {token.MotionDurationSlow}, backdrop-filter 0s",
                        Position = "absolute",
                        ["..."] = GenStackChildrenStyle(token)
                    },
                },
                [$"{componentCls}-stack:not({componentCls}-stack-expanded)"] = new CSSObject()
                {
                    [$"& > {componentCls}-notice-wrapper"] = new CSSObject()
                    {
                        ["..."] = GenStackedNoticeStyle(token)
                    },
                },
                [$"{componentCls}-stack{componentCls}-stack-expanded"] = new CSSObject()
                {
                    [$"& > {componentCls}-notice-wrapper"] = new CSSObject()
                    {
                        ["&:not(:nth-last-child(-n + 1))"] = new CSSObject()
                        {
                            Opacity = 1,
                            Overflow = "unset",
                            Color = "inherit",
                            PointerEvents = "auto",
                            [$"& > {token.ComponentCls}-notice"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                        },
                        ["&:after"] = new CSSObject()
                        {
                            Content = "\"\"",
                            Position = "absolute",
                            Height = token.Margin,
                            Width = "100%",
                            InsetInline = 0,
                            Bottom = -token.Margin,
                            Background = "transparent",
                            PointerEvents = "auto",
                        },
                    },
                },
                ["..."] = NotificationPlacements.Select((placement) => GenPlacementStackStyle(token, placement)).Aggregate(new CSSObject(),
                    (acc, cur) =>
                    {
                        return new CSSObject()
                        {
                            ["..."] = acc,
                            ["..."] = cur,
                        };
                    })
            };
        }

    }

}
