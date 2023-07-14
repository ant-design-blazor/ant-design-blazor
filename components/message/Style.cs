using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class MessageToken
    {
        public int ZIndexPopup { get; set; }

        public string ContentBg { get; set; }

        public CSSProperties ContentPadding { get; set; }

    }

    public partial class MessageToken : TokenWithCommonCls
    {
        public int Height { get; set; }

    }

    public partial class Message
    {
        public Unknown_1 GenMessageStyle(Unknown_4 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var boxShadow = token.BoxShadow;
            var colorText = token.ColorText;
            var colorSuccess = token.ColorSuccess;
            var colorError = token.ColorError;
            var colorWarning = token.ColorWarning;
            var colorInfo = token.ColorInfo;
            var fontSizeLG = token.FontSizeLG;
            var motionEaseInOutCirc = token.MotionEaseInOutCirc;
            var motionDurationSlow = token.MotionDurationSlow;
            var marginXS = token.MarginXS;
            var paddingXS = token.PaddingXS;
            var borderRadiusLG = token.BorderRadiusLG;
            var zIndexPopup = token.ZIndexPopup;
            var contentPadding = token.ContentPadding;
            var contentBg = token.ContentBg;
            var noticeCls = @$"{componentCls}-notice";
            var messageMoveIn = New Keyframes("MessageMoveIn", {
    "0%": {
      padding: 0,
      transform: "translateY(-100%)",
      opacity: 0,
    },

    "100%": {
      padding: paddingXS,
      transform: "translateY(0)",
      opacity: 1,
    },
  });
            var messageMoveOut = new Keyframes("MessageMoveOut", {
    "0%": {
      maxHeight: token.Height,
      padding: paddingXS,
      opacity: 1,
    },
    "100%": {
      maxHeight: 0,
      padding: 0,
      opacity: 0,
    },
  });
            var noticeStyle = new Unknown_5()
            {
                Padding = paddingXS,
                TextAlign = "center",
                [$"{componentCls}-custom-content > {iconCls}"] = new Unknown_6()
                {
                    VerticalAlign = "text-bottom",
                    MarginInlineEnd = marginXS,
                    FontSize = fontSizeLG,
                },
                [$"{noticeCls}-content"] = new Unknown_7()
                {
                    Display = "inline-block",
                    Padding = contentPadding,
                    Background = contentBg,
                    BorderRadius = borderRadiusLG,
                    BoxShadow = boxShadow,
                    PointerEvents = "all",
                },
                [$"{componentCls}-success > {iconCls}"] = new Unknown_8()
                {
                    Color = colorSuccess,
                },
                [$"{componentCls}-error > {iconCls}"] = new Unknown_9()
                {
                    Color = colorError,
                },
                [$"{componentCls}-warning > {iconCls}"] = new Unknown_10()
                {
                    Color = colorWarning,
                },
                [$"{componentCls}-info>{iconCls},{componentCls}-loading>{iconCls}"] = new Unknown_11()
                {
                    Color = colorInfo,
                },
            };
            return new Unknown_12
            {
                new Unknown_13()
                {
                    [componentCls] = new Unknown_14()
                    {
                        ["..."] = ResetComponent(token),
                        Color = colorText,
                        Position = "fixed",
                        Top = marginXS,
                        Width = "100%",
                        PointerEvents = "none",
                        ZIndex = zIndexPopup,
                        [$"{componentCls}-move-up"] = new Unknown_15()
                        {
                            AnimationFillMode = "forwards",
                        },
                        [$"{componentCls}-move-up-appear,{componentCls}-move-up-enter"] = new Unknown_16()
                        {
                            AnimationName = messageMoveIn,
                            AnimationDuration = motionDurationSlow,
                            AnimationPlayState = "paused",
                            AnimationTimingFunction = motionEaseInOutCirc,
                        },
                        [$"{componentCls}-move-up-appear{componentCls}-move-up-appear-active,{componentCls}-move-up-enter{componentCls}-move-up-enter-active"] = new Unknown_17()
                        {
                            AnimationPlayState = "running",
                        },
                        [$"{componentCls}-move-up-leave"] = new Unknown_18()
                        {
                            AnimationName = messageMoveOut,
                            AnimationDuration = motionDurationSlow,
                            AnimationPlayState = "paused",
                            AnimationTimingFunction = motionEaseInOutCirc,
                        },
                        [$"{componentCls}-move-up-leave{componentCls}-move-up-leave-active"] = new Unknown_19()
                        {
                            AnimationPlayState = "running",
                        },
                        ["&-rtl"] = new Unknown_20()
                        {
                            Direction = "rtl",
                            Span = new Unknown_21()
                            {
                                Direction = "rtl",
                            },
                        },
                    },
                },
                new Unknown_22()
                {
                    [componentCls] = new Unknown_23()
                    {
                        [noticeCls] = new Unknown_24()
                        {
                            ["..."] = noticeStyle,
                        },
                    },
                },
                new Unknown_25()
                {
                    [$"{componentCls}-notice-pure-panel"] = new Unknown_26()
                    {
                        ["..."] = noticeStyle,
                        Padding = 0,
                        TextAlign = "start",
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_27 token)
        {
            var combinedToken = MergeToken(
                token,
                new Unknown_28()
                {
                    Height = 150,
                });
            return new Unknown_29 { GenMessageStyle(combinedToken) };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_30 token)
        {
            return new Unknown_31()
            {
                ZIndexPopup = token.ZIndexPopupBase + 10,
                ContentBg = token.ColorBgElevated,
                ContentPadding = @$"{(token.ControlHeightLG - token.FontSize * token.LineHeight) / 2}px {
      token.PaddingSM
    }px",
            };
        }

    }

}