using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class BackTopToken : TokenWithCommonCls
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class BackTopToken
    {
        public string BackTopBackground { get; set; }

        public string BackTopColor { get; set; }

        public string BackTopHoverBackground { get; set; }

        public int BackTopFontSize { get; set; }

        public int BackTopSize { get; set; }

        public int BackTopBlockEnd { get; set; }

        public int BackTopInlineEnd { get; set; }

        public int BackTopInlineEndMD { get; set; }

        public int BackTopInlineEndXS { get; set; }

    }

    public partial class BackTop
    {
        public CSSObject GenSharedBackTopStyle(BackTopToken token)
        {
            var componentCls = token.ComponentCls;
            var backTopFontSize = token.BackTopFontSize;
            var backTopSize = token.BackTopSize;
            var zIndexPopup = token.ZIndexPopup;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "fixed",
                    InsetInlineEnd = token.BackTopInlineEnd,
                    InsetBlockEnd = token.BackTopBlockEnd,
                    ZIndex = zIndexPopup,
                    Width = 40,
                    Height = 40,
                    Cursor = "pointer",
                    ["&:empty"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Width = backTopSize,
                        Height = backTopSize,
                        Overflow = "hidden",
                        Color = token.BackTopColor,
                        TextAlign = "center",
                        BackgroundColor = token.BackTopBackground,
                        BorderRadius = backTopSize,
                        Transition = @$"all {token.MotionDurationMid}",
                        ["&:hover"] = new CSSObject()
                        {
                            BackgroundColor = token.BackTopHoverBackground,
                            Transition = @$"all {token.MotionDurationMid}",
                        },
                    },
                    [$"{componentCls}-icon"] = new CSSObject()
                    {
                        FontSize = backTopFontSize,
                        LineHeight = @$"{backTopSize}px",
                    },
                },
            };
        }

        public CSSObject GenMediaBackTopStyle(BackTopToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"@media (max-width: {token.ScreenMD}px)"] = new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        InsetInlineEnd = token.BackTopInlineEndMD,
                    },
                },
                [$"@media (max-width: {token.ScreenXS}px)"] = new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        InsetInlineEnd = token.BackTopInlineEndXS,
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "BackTop",
                (token) =>
                {
                    var fontSizeHeading3 = token.FontSizeHeading3;
                    var colorTextDescription = token.ColorTextDescription;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorText = token.ColorText;
                    var controlHeightLG = token.ControlHeightLG;
                    var backTopToken = MergeToken(
                        token,
                        new BackTopToken()
                        {
                            BackTopBackground = colorTextDescription,
                            BackTopColor = colorTextLightSolid,
                            BackTopHoverBackground = colorText,
                            BackTopFontSize = fontSizeHeading3,
                            BackTopSize = controlHeightLG,
                            BackTopBlockEnd = (int)(controlHeightLG * 1.25),
                            BackTopInlineEnd = (int)(controlHeightLG * 2.5),
                            BackTopInlineEndMD = (int)(controlHeightLG * 1.5),
                            BackTopInlineEndXS = (int)(controlHeightLG * 0.5),
                        });
                    return new CSSInterpolation[]
                    {
                        GenSharedBackTopStyle(backTopToken),
                        GenMediaBackTopStyle(backTopToken)
                    };
                },
                (token) =>
                {
                    return new BackTopToken()
                    {
                        ZIndexPopup = token.ZIndexBase + 10,
                    };
                });
        }

    }

}