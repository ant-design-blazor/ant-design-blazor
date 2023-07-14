using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class NotificationToken
    {
        public int ZIndexPopup { get; set; }

        public int Width { get; set; }

    }

    public partial class NotificationToken : TokenWithCommonCls
    {
        public int AnimationMaxHeight { get; set; }

        public string NotificationBg { get; set; }

        public string NotificationPadding { get; set; }

        public int NotificationPaddingVertical { get; set; }

        public int NotificationPaddingHorizontal { get; set; }

        public int NotificationIconSize { get; set; }

        public int NotificationCloseButtonSize { get; set; }

        public int NotificationMarginBottom { get; set; }

        public int NotificationMarginEdge { get; set; }

    }

    public partial class Notification
    {
        public Unknown_1 GenNotificationStyle(Unknown_5 token)
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
            var motionDurationMid = token.MotionDurationMid;
            var motionEaseInOut = token.MotionEaseInOut;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var width = token.Width;
            var notificationIconSize = token.NotificationIconSize;
            var noticeCls = @$"{componentCls}-notice";
            var notificationFadeIn = new Keyframes("antNotificationFadeIn", {
    "0%": {
      left: {
        _skip_check_: true,
        value: width,
      },
      opacity: 0,
    },

    "100%": {
      left: {
        _skip_check_: true,
        value: 0,
      },
      opacity: 1,
    },
  });
            var notificationFadeOut = new Keyframes("antNotificationFadeOut", {
    "0%": {
      maxHeight: token.AnimationMaxHeight,
      marginBottom: notificationMarginBottom,
      opacity: 1,
    },

    "100%": {
      maxHeight: 0,
      marginBottom: 0,
      paddingTop: 0,
      paddingBottom: 0,
      opacity: 0,
    },
  });
            var noticeStyle = new Unknown_6()
            {
                Position = "relative",
                Width = width,
                MaxWidth = @$"calc(100vw - {notificationMarginEdge * 2}px)",
                MarginBottom = notificationMarginBottom,
                MarginInlineStart = "auto",
                Padding = notificationPadding,
                Overflow = "hidden",
                LineHeight = lineHeight,
                WordWrap = "break-word",
                Background = notificationBg,
                BorderRadius = borderRadiusLG,
                BoxShadow = boxShadow,
                [$"{componentCls}-close-icon"] = new Unknown_7()
                {
                    FontSize = fontSize,
                    Cursor = "pointer",
                },
                [$"{noticeCls}-message"] = new Unknown_8()
                {
                    MarginBottom = token.MarginXS,
                    Color = colorTextHeading,
                    FontSize = fontSizeLG,
                    LineHeight = token.LineHeightLG,
                },
                [$"{noticeCls}-description"] = new Unknown_9()
                {
                    FontSize = fontSize,
                },
                [$"&{noticeCls}-closable {noticeCls}-message"] = new Unknown_10()
                {
                    PaddingInlineEnd = token.PaddingLG,
                },
                [$"{noticeCls}-with-icon {noticeCls}-message"] = new Unknown_11()
                {
                    MarginBottom = token.MarginXS,
                    MarginInlineStart = token.MarginSM + notificationIconSize,
                    FontSize = fontSizeLG,
                },
                [$"{noticeCls}-with-icon {noticeCls}-description"] = new Unknown_12()
                {
                    MarginInlineStart = token.MarginSM + notificationIconSize,
                    FontSize = fontSize,
                },
                [$"{noticeCls}-icon"] = new Unknown_13()
                {
                    Position = "absolute",
                    FontSize = notificationIconSize,
                    LineHeight = 0,
                    [$"&-success{iconCls}"] = new Unknown_14()
                    {
                        Color = colorSuccess,
                    },
                    [$"&-info{iconCls}"] = new Unknown_15()
                    {
                        Color = colorInfo,
                    },
                    [$"&-warning{iconCls}"] = new Unknown_16()
                    {
                        Color = colorWarning,
                    },
                    [$"&-error{iconCls}"] = new Unknown_17()
                    {
                        Color = colorError,
                    },
                },
                [$"{noticeCls}-close"] = new Unknown_18()
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
                    ["&:hover"] = new Unknown_19()
                    {
                        Color = token.ColorIconHover,
                        BackgroundColor = token.Wireframe ? "transparent" : token.ColorFillContent,
                    },
                },
                [$"{noticeCls}-btn"] = new Unknown_20()
                {
                    Float = "right",
                    MarginTop = token.MarginSM,
                },
            };
            return new Unknown_21
            {
                new Unknown_22()
                {
                    [componentCls] = new Unknown_23()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "fixed",
                        ZIndex = token.ZIndexPopup,
                        MarginInlineEnd = notificationMarginEdge,
                        [$"{componentCls}-hook-holder"] = new Unknown_24()
                        {
                            Position = "relative",
                        },
                        [$"&{componentCls}-top, &{componentCls}-bottom"] = new Unknown_25()
                        {
                            [noticeCls] = new Unknown_26()
                            {
                                MarginInline = "auto auto",
                            },
                        },
                        [$"&{componentCls}-topLeft, &{componentCls}-bottomLeft"] = new Unknown_27()
                        {
                            [noticeCls] = new Unknown_28()
                            {
                                MarginInlineEnd = "auto",
                                MarginInlineStart = 0,
                            },
                        },
                        [$"{componentCls}-fade-enter, {componentCls}-fade-appear"] = new Unknown_29()
                        {
                            AnimationDuration = token.MotionDurationMid,
                            AnimationTimingFunction = motionEaseInOut,
                            AnimationFillMode = "both",
                            Opacity = 0,
                            AnimationPlayState = "paused",
                        },
                        [$"{componentCls}-fade-leave"] = new Unknown_30()
                        {
                            AnimationTimingFunction = motionEaseInOut,
                            AnimationFillMode = "both",
                            AnimationDuration = motionDurationMid,
                            AnimationPlayState = "paused",
                        },
                        [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new Unknown_31()
                        {
                            AnimationName = notificationFadeIn,
                            AnimationPlayState = "running",
                        },
                        [$"{componentCls}-fade-leave{componentCls}-fade-leave-active"] = new Unknown_32()
                        {
                            AnimationName = notificationFadeOut,
                            AnimationPlayState = "running",
                        },
                        ["..."] = GenNotificationPlacementStyle(token),
                        ["&-rtl"] = new Unknown_33()
                        {
                            Direction = "rtl",
                            [$"{noticeCls}-btn"] = new Unknown_34()
                            {
                                Float = "left",
                            },
                        },
                    },
                },
                new Unknown_35()
                {
                    [componentCls] = new Unknown_36()
                    {
                        [noticeCls] = new Unknown_37()
                        {
                            ["..."] = noticeStyle,
                        },
                    },
                },
                new Unknown_38()
                {
                    [$"{noticeCls}-pure-panel"] = new Unknown_39()
                    {
                        ["..."] = noticeStyle,
                        Margin = 0,
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_40 token)
        {
            var notificationPaddingVertical = token.PaddingMD;
            var notificationPaddingHorizontal = token.PaddingLG;
            var notificationToken = MergeToken(
                token,
                new Unknown_41()
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
                });
            return new Unknown_42 { GenNotificationStyle(notificationToken) };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_43 token)
        {
            return new Unknown_44()
            {
                ZIndexPopup = token.ZIndexPopupBase + 50,
                Width = 384,
            };
        }

        public Unknown_4 GenNotificationPlacementStyle(Unknown_45 token)
        {
            var componentCls = token.ComponentCls;
            var width = token.Width;
            var notificationMarginEdge = token.NotificationMarginEdge;
            var notificationTopFadeIn = new Keyframes("antNotificationTopFadeIn", {
    "0%": {
      marginTop: "-100%",
      opacity: 0,
    },

    "100%": {
      marginTop: 0,
      opacity: 1,
    },
  });
            var notificationBottomFadeIn = new Keyframes("antNotificationBottomFadeIn", {
    "0%": {
      marginBottom: "-100%",
      opacity: 0,
    },

    "100%": {
      marginBottom: 0,
      opacity: 1,
    },
  });
            var notificationLeftFadeIn = new Keyframes("antNotificationLeftFadeIn", {
    "0%": {
      right: {
        _skip_check_: true,
        value: width,
      },
      opacity: 0,
    },

    "100%": {
      right: {
        _skip_check_: true,
        value: 0,
      },
      opacity: 1,
    },
  });
            return new Unknown_46()
            {
                [$"&{componentCls}-top, &{componentCls}-bottom"] = new Unknown_47()
                {
                    MarginInline = 0,
                },
                [$"&{componentCls}-top"] = new Unknown_48()
                {
                    [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new Unknown_49()
                    {
                        AnimationName = notificationTopFadeIn,
                    },
                },
                [$"&{componentCls}-bottom"] = new Unknown_50()
                {
                    [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new Unknown_51()
                    {
                        AnimationName = notificationBottomFadeIn,
                    },
                },
                [$"&{componentCls}-topLeft, &{componentCls}-bottomLeft"] = new Unknown_52()
                {
                    MarginInlineEnd = 0,
                    MarginInlineStart = notificationMarginEdge,
                    [$"{componentCls}-fade-enter{componentCls}-fade-enter-active, {componentCls}-fade-appear{componentCls}-fade-appear-active"] = new Unknown_53()
                    {
                        AnimationName = notificationLeftFadeIn,
                    },
                },
            };
        }

    }

}