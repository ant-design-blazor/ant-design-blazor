using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Keyframes = CssInCSharp.Keyframe;

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
        public CSSInterpolation[] GenMessageStyle(MessageToken token)
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
            var messageMoveIn = new Keyframes("MessageMoveIn",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        Padding = 0,
                        Transform = "translateY(-100%)",
                        Opacity = 0,
                    },
                    ["100%"] = new CSSObject()
                    {
                        Padding = paddingXS,
                        Transform = "translateY(0)",
                        Opacity = 1,
                    },
                });
            var messageMoveOut = new Keyframes("MessageMoveOut",
                new CSSObject()
                {
                    ["0%"] = new CSSObject()
                    {
                        MaxHeight = token.Height,
                        Padding = paddingXS,
                        Opacity = 1,
                    },
                    ["100%"] = new CSSObject()
                    {
                        MaxHeight = 0,
                        Padding = 0,
                        Opacity = 0,
                    },
                });
            var noticeStyle = new CSSObject()
            {
                Padding = paddingXS,
                TextAlign = "center",
                [$"{componentCls}-custom-content > {iconCls}"] = new CSSObject()
                {
                    VerticalAlign = "text-bottom",
                    MarginInlineEnd = marginXS,
                    FontSize = fontSizeLG,
                },
                [$"{noticeCls}-content"] = new CSSObject()
                {
                    Display = "inline-block",
                    Padding = contentPadding,
                    Background = contentBg,
                    BorderRadius = borderRadiusLG,
                    BoxShadow = boxShadow,
                    PointerEvents = "all",
                },
                [$"{componentCls}-success > {iconCls}"] = new CSSObject()
                {
                    Color = colorSuccess,
                },
                [$"{componentCls}-error > {iconCls}"] = new CSSObject()
                {
                    Color = colorError,
                },
                [$"{componentCls}-warning > {iconCls}"] = new CSSObject()
                {
                    Color = colorWarning,
                },
                [$"{componentCls}-info>{iconCls},{componentCls}-loading>{iconCls}"] = new CSSObject()
                {
                    Color = colorInfo,
                },
            };
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Color = colorText,
                        Position = "fixed",
                        Top = marginXS,
                        Width = "100%",
                        PointerEvents = "none",
                        ZIndex = zIndexPopup,
                        [$"{componentCls}-move-up"] = new CSSObject()
                        {
                            AnimationFillMode = "forwards",
                        },
                        [$"{componentCls}-move-up-appear,{componentCls}-move-up-enter"] = new CSSObject()
                        {
                            AnimationName = messageMoveIn,
                            AnimationDuration = motionDurationSlow,
                            AnimationPlayState = "paused",
                            AnimationTimingFunction = motionEaseInOutCirc,
                        },
                        [$"{componentCls}-move-up-appear{componentCls}-move-up-appear-active,{componentCls}-move-up-enter{componentCls}-move-up-enter-active"] = new CSSObject()
                        {
                            AnimationPlayState = "running",
                        },
                        [$"{componentCls}-move-up-leave"] = new CSSObject()
                        {
                            AnimationName = messageMoveOut,
                            AnimationDuration = motionDurationSlow,
                            AnimationPlayState = "paused",
                            AnimationTimingFunction = motionEaseInOutCirc,
                        },
                        [$"{componentCls}-move-up-leave{componentCls}-move-up-leave-active"] = new CSSObject()
                        {
                            AnimationPlayState = "running",
                        },
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                            ["span"] = new CSSObject()
                            {
                                Direction = "rtl",
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
                            ["..."] = noticeStyle,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-notice-pure-panel"] = new CSSObject()
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
                        new MessageToken()
                        {
                            Height = 150,
                        });
                    return new CSSInterpolation[]
                    {
                        GenMessageStyle(combinedToken),
                    };
                },
                (token) =>
                {
                    return new MessageToken()
                    {
                        ZIndexPopup = token.ZIndexPopupBase + 1000 + 10,
                        ContentBg = token.ColorBgElevated,
                        ContentPadding = @$"{(token.ControlHeightLG - token.FontSize * token.LineHeight) / 2}px {token.PaddingSM}px",
                    };
                });
        }

    }

}
