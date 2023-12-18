using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class AvatarToken : TokenWithCommonCls
    {
        public double ContainerSize
        {
            get => (double)_tokens["containerSize"];
            set => _tokens["containerSize"] = value;
        }

        public double ContainerSizeLG
        {
            get => (double)_tokens["containerSizeLG"];
            set => _tokens["containerSizeLG"] = value;
        }

        public double ContainerSizeSM
        {
            get => (double)_tokens["containerSizeSM"];
            set => _tokens["containerSizeSM"] = value;
        }

        public double TextFontSize
        {
            get => (double)_tokens["textFontSize"];
            set => _tokens["textFontSize"] = value;
        }

        public double TextFontSizeLG
        {
            get => (double)_tokens["textFontSizeLG"];
            set => _tokens["textFontSizeLG"] = value;
        }

        public double TextFontSizeSM
        {
            get => (double)_tokens["textFontSizeSM"];
            set => _tokens["textFontSizeSM"] = value;
        }

        public double GroupSpace
        {
            get => (double)_tokens["groupSpace"];
            set => _tokens["groupSpace"] = value;
        }

        public double GroupOverlapping
        {
            get => (double)_tokens["groupOverlapping"];
            set => _tokens["groupOverlapping"] = value;
        }

        public string GroupBorderColor
        {
            get => (string)_tokens["groupBorderColor"];
            set => _tokens["groupBorderColor"] = value;
        }

    }

    public partial class AvatarToken
    {
        public string AvatarBgColor
        {
            get => (string)_tokens["avatarBgColor"];
            set => _tokens["avatarBgColor"] = value;
        }

        public string AvatarBg
        {
            get => (string)_tokens["avatarBg"];
            set => _tokens["avatarBg"] = value;
        }

        public string AvatarColor
        {
            get => (string)_tokens["avatarColor"];
            set => _tokens["avatarColor"] = value;
        }

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
            var containerSize = token.ContainerSize;
            var containerSizeLG = token.ContainerSizeLG;
            var containerSizeSM = token.ContainerSizeSM;
            var textFontSize = token.TextFontSize;
            var textFontSizeLG = token.TextFontSizeLG;
            var textFontSizeSM = token.TextFontSizeSM;
            var borderRadius = token.BorderRadius;
            var borderRadiusLG = token.BorderRadiusLG;
            var borderRadiusSM = token.BorderRadiusSM;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var avatarSizeStyle = (double size, double fontSize, double radius) => {
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
                    ["..."] = avatarSizeStyle(containerSize, textFontSize, borderRadius),
                    ["&-lg"] = new CSSObject()
                    {
                        ["..."] = avatarSizeStyle(containerSizeLG, textFontSizeLG, borderRadiusLG)
                    },
                    ["&-sm"] = new CSSObject()
                    {
                        ["..."] = avatarSizeStyle(containerSizeSM, textFontSizeSM, borderRadiusSM)
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
            var groupBorderColor = token.GroupBorderColor;
            var groupOverlapping = token.GroupOverlapping;
            var groupSpace = token.GroupSpace;
            return new CSSObject()
            {
                [$"{componentCls}-group"] = new CSSObject()
                {
                    Display = "inline-flex",
                    [$"{componentCls}"] = new CSSObject()
                    {
                        BorderColor = groupBorderColor,
                    },
                    ["> *:not(:first-child)"] = new CSSObject()
                    {
                        MarginInlineStart = groupOverlapping,
                    },
                },
                [$"{componentCls}-group-popover"] = new CSSObject()
                {
                    [$"{componentCls} + {componentCls}"] = new CSSObject()
                    {
                        MarginInlineStart = groupSpace,
                    },
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Avatar",
                (token) =>
                {
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorTextPlaceholder = token.ColorTextPlaceholder;
                    var avatarToken = MergeToken(
                        token,
                        new AvatarToken()
                        {
                            AvatarBg = colorTextPlaceholder,
                            AvatarColor = colorTextLightSolid,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(avatarToken),
                        GenGroupStyle(avatarToken)
                    };
                },
                (token) =>
                {
                    var controlHeight = token.ControlHeight;
                    var controlHeightLG = token.ControlHeightLG;
                    var controlHeightSM = token.ControlHeightSM;
                    var fontSize = token.FontSize;
                    var fontSizeLG = token.FontSizeLG;
                    var fontSizeXL = token.FontSizeXL;
                    var fontSizeHeading3 = token.FontSizeHeading3;
                    var marginXS = token.MarginXS;
                    var marginXXS = token.MarginXXS;
                    var colorBorderBg = token.ColorBorderBg;
                    return new AvatarToken()
                    {
                        ContainerSize = controlHeight,
                        ContainerSizeLG = controlHeightLG,
                        ContainerSizeSM = controlHeightSM,
                        TextFontSize = Math.Round((fontSizeLG + fontSizeXL) / 2),
                        TextFontSizeLG = fontSizeHeading3,
                        TextFontSizeSM = fontSize,
                        GroupSpace = marginXXS,
                        GroupOverlapping = -marginXS,
                        GroupBorderColor = colorBorderBg,
                    };
                });
        }

    }

}