using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class MessageToken
    {
        public double ZIndexPopup { get; set; }

        public string ContentBg { get; set; }

        public string ContentPadding { get; set; }

    }

    public partial class MessageToken : TokenWithCommonCls
    {
        public double Height { get; set; }

    }

    public partial class Message
    {
        public Unknown1_1 GenMessageStyle(Unknown1_2 token)
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
            var noticeStyle = new Unknown1_3()
            {
                Padding = paddingXS,
                TextAlign = "center",
                [$"{componentCls}-custom-content > {iconCls}"] = new Unknown1_4()
                {
                    VerticalAlign = "text-bottom",
                    MarginInlineEnd = marginXS,
                    FontSize = fontSizeLG,
                },
                [$"{noticeCls}-content"] = new Unknown1_5()
                {
                    Display = "inline-block",
                    Padding = contentPadding,
                    Background = contentBg,
                    BorderRadius = borderRadiusLG,
                    BoxShadow = boxShadow,
                    PointerEvents = "all",
                },
                [$"{componentCls}-success > {iconCls}"] = new Unknown1_6()
                {
                    Color = colorSuccess,
                },
                [$"{componentCls}-error > {iconCls}"] = new Unknown1_7()
                {
                    Color = colorError,
                },
                [$"{componentCls}-warning > {iconCls}"] = new Unknown1_8()
                {
                    Color = colorWarning,
                },
                [$"{componentCls}-info>{iconCls},{componentCls}-loading>{iconCls}"] = new Unknown1_9()
                {
                    Color = colorInfo,
                },
            };
            return new Unknown1_10
            {
                new Unknown1_11()
                {
                    [componentCls] = new Unknown1_12()
                    {
                        ["..."] = ResetComponent(token),
                        Color = colorText,
                        Position = "fixed",
                        Top = marginXS,
                        Width = "100%",
                        PointerEvents = "none",
                        ZIndex = zIndexPopup,
                        [$"{componentCls}-move-up"] = new Unknown1_13()
                        {
                            AnimationFillMode = "forwards",
                        },
                        [$"{componentCls}-move-up-appear,{componentCls}-move-up-enter"] = new Unknown1_14()
                        {
                            AnimationName = messageMoveIn,
                            AnimationDuration = motionDurationSlow,
                            AnimationPlayState = "paused",
                            AnimationTimingFunction = motionEaseInOutCirc,
                        },
                        [$"{componentCls}-move-up-appear{componentCls}-move-up-appear-active,{componentCls}-move-up-enter{componentCls}-move-up-enter-active"] = new Unknown1_15()
                        {
                            AnimationPlayState = "running",
                        },
                        [$"{componentCls}-move-up-leave"] = new Unknown1_16()
                        {
                            AnimationName = messageMoveOut,
                            AnimationDuration = motionDurationSlow,
                            AnimationPlayState = "paused",
                            AnimationTimingFunction = motionEaseInOutCirc,
                        },
                        [$"{componentCls}-move-up-leave{componentCls}-move-up-leave-active"] = new Unknown1_17()
                        {
                            AnimationPlayState = "running",
                        },
                        ["&-rtl"] = new Unknown1_18()
                        {
                            Direction = "rtl",
                            Span = new Unknown1_19()
                            {
                                Direction = "rtl",
                            },
                        },
                    },
                },
                new Unknown1_20()
                {
                    [componentCls] = new Unknown1_21()
                    {
                        [$"{noticeCls}-wrapper"] = new Unknown1_22()
                        {
                            ["..."] = noticeStyle,
                        },
                    },
                },
                new Unknown1_23()
                {
                    [$"{componentCls}-notice-pure-panel"] = new Unknown1_24()
                    {
                        ["..."] = noticeStyle,
                        Padding = 0,
                        TextAlign = "start",
                    },
                },
            };
        }

        public UseComponentStyleResult ExportDefault()
        {
            return GenComponentStyleHook(
                "Message",
                (token) =>
                {
                    var combinedToken = MergeToken(
                        token,
                        new Unknown2_1()
                        {
                            Height = 150,
                        });
                    return new Unknown2_2
                    {
                        GenMessageStyle(combinedToken),
                    };
                },
                (token) =>
                {
                    return new Unknown2_3()
                    {
                        ZIndexPopup = token.ZIndexPopupBase + CONTAINER_MAX_OFFSET + 10,
                        ContentBg = token.ColorBgElevated,
                        ContentPadding = @$"{(token.ControlHeightLG - token.FontSize * token.LineHeight) / 2}px {
      token.PaddingSM
    }px",
                    };
                });
        }

    }

}