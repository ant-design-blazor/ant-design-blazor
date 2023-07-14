using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class AvatarToken : TokenWithCommonCls
    {
    }

    public partial class AvatarToken
    {
        public string AvatarBg { get; set; }

        public string AvatarColor { get; set; }

        public int AvatarSizeBase { get; set; }

        public int AvatarSizeLG { get; set; }

        public int AvatarSizeSM { get; set; }

        public int AvatarFontSizeBase { get; set; }

        public int AvatarFontSizeLG { get; set; }

        public int AvatarFontSizeSM { get; set; }

        public int AvatarGroupOverlapping { get; set; }

        public int AvatarGroupSpace { get; set; }

        public string AvatarGroupBorderColor { get; set; }

        public string AvatarBgColor { get; set; }

    }

    public partial class Avatar
    {
        public CSSObject GenBaseStyle(AvatarToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var avatarBg = token.AvatarBg;
            var avatarColor = token.AvatarColor;
            var avatarSizeBase = token.AvatarSizeBase;
            var avatarSizeLG = token.AvatarSizeLG;
            var avatarSizeSM = token.AvatarSizeSM;
            var avatarFontSizeBase = token.AvatarFontSizeBase;
            var avatarFontSizeLG = token.AvatarFontSizeLG;
            var avatarFontSizeSM = token.AvatarFontSizeSM;
            var borderRadius = token.BorderRadius;
            var borderRadiusLG = token.BorderRadiusLG;
            var borderRadiusSM = token.BorderRadiusSM;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var avatarSizeStyle = (int size, int fontSize, int radius) => {
                return new CSSObject()
                {
                    Width = size,
                    Height = size,
                    LineHeight = @$"{size - lineWidth * 2}px",
                    BorderRadius = "50%",
                    [$"&{componentCls}-square"] = new CSSObject()
                    {
                        BorderRadius = radius,
                    },
                    [$"{componentCls}-string"] = new CSSObject()
                    {
                        Position = "absolute",
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "50%",
                        },
                        TransformOrigin = "0 center",
                    },
                    [$"&{componentCls}-icon"] = new CSSObject()
                    {
                        FontSize = fontSize,
                        [$"> {iconCls}"] = new CSSObject()
                        {
                            Margin = 0,
                        },
                    },
                };
            };
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    Overflow = "hidden",
                    Color = avatarColor,
                    WhiteSpace = "nowrap",
                    TextAlign = "center",
                    VerticalAlign = "middle",
                    Background = avatarBg,
                    Border = @$"{lineWidth}px {lineType} transparent",
                    ["&-image"] = new CSSObject()
                    {
                        Background = "transparent",
                    },
                    [$"{antCls}-image-img"] = new CSSObject()
                    {
                        Display = "block",
                    },
                    ["..."] = avatarSizeStyle(avatarSizeBase, avatarFontSizeBase, borderRadius),
                    ["&-lg"] = new CSSObject()
                    {
                        ["..."] = avatarSizeStyle(avatarSizeLG, avatarFontSizeLG, borderRadiusLG)
                    },
                    ["&-sm"] = new CSSObject()
                    {
                        ["..."] = avatarSizeStyle(avatarSizeSM, avatarFontSizeSM, borderRadiusSM)
                    },
                    ["> img"] = new CSSObject()
                    {
                        Display = "block",
                        Width = "100%",
                        Height = "100%",
                        ObjectFit = "cover",
                    },
                },
            };
        }

        public CSSObject GenGroupStyle(AvatarToken token)
        {
            var componentCls = token.ComponentCls;
            var avatarGroupBorderColor = token.AvatarGroupBorderColor;
            var avatarGroupSpace = token.AvatarGroupSpace;
            return new CSSObject()
            {
                [$"{componentCls}-group"] = new CSSObject()
                {
                    Display = "inline-flex",
                    [$"{componentCls}"] = new CSSObject()
                    {
                        BorderColor = avatarGroupBorderColor,
                    },
                    ["> *:not(:first-child)"] = new CSSObject()
                    {
                        MarginInlineStart = avatarGroupSpace,
                    },
                },
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var colorTextLightSolid = token.ColorTextLightSolid;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var fontSize = token.FontSize;
            var fontSizeLG = token.FontSizeLG;
            var fontSizeXL = token.FontSizeXL;
            var fontSizeHeading3 = token.FontSizeHeading3;
            var marginXS = token.MarginXS;
            var colorBorderBg = token.ColorBorderBg;
            var colorTextPlaceholder = token.ColorTextPlaceholder;
            var avatarToken = MergeToken(
                token,
                new AvatarToken()
                {
                    AvatarBg = colorTextPlaceholder,
                    AvatarColor = colorTextLightSolid,
                    AvatarSizeBase = controlHeight,
                    AvatarSizeLG = controlHeightLG,
                    AvatarSizeSM = controlHeightSM,
                    AvatarFontSizeBase = (int)Math.Round((double)(fontSizeLG + fontSizeXL) / 2),
                    AvatarFontSizeLG = fontSizeHeading3,
                    AvatarFontSizeSM = fontSize,
                    AvatarGroupSpace = -marginXS,
                    AvatarGroupBorderColor = colorBorderBg,
                });
            return new CSSInterpolation[]
            {
                GenBaseStyle(avatarToken),
                GenGroupStyle(avatarToken)
            };
        }

    }

}