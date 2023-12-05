using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AvatarToken : TokenWithCommonCls
    {
        public int ContainerSize { get; set; }

        public int ContainerSizeLG { get; set; }

        public int ContainerSizeSM { get; set; }

        public int TextFontSize { get; set; }

        public int TextFontSizeLG { get; set; }

        public int TextFontSizeSM { get; set; }

        public int GroupSpace { get; set; }

        public int GroupOverlapping { get; set; }

        public string GroupBorderColor { get; set; }

    }

    public partial class AvatarToken
    {
        public string AvatarBgColor { get; set; }

        public string AvatarBg { get; set; }

        public string AvatarColor { get; set; }

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
            return GenComponentStyleHook("Avatar", (token) =>
            {
                var colorTextLightSolid = token.ColorTextLightSolid;
                var colorTextPlaceholder = token.ColorTextPlaceholder;
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
                var avatarToken = MergeToken(
                    token,
                    new AvatarToken()
                    {
                        AvatarBg = colorTextPlaceholder,
                        AvatarColor = colorTextLightSolid,
                        ContainerSize = controlHeight,
                        ContainerSizeLG = controlHeightLG,
                        ContainerSizeSM = controlHeightSM,
                        TextFontSize = (int)Math.Round((double)(fontSizeLG + fontSizeXL) / 2),
                        TextFontSizeLG = fontSizeHeading3,
                        TextFontSizeSM = fontSize,
                        GroupSpace = marginXXS,
                        GroupOverlapping = -marginXS,
                        GroupBorderColor = colorBorderBg,
                    });
                return new CSSInterpolation[]
                {
                    GenBaseStyle(avatarToken),
                    GenGroupStyle(avatarToken)
                };
            });
        }
    }

}
