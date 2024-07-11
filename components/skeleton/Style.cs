using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using Keyframes = CssInCSharp.Keyframe;

namespace AntDesign
{
    public partial class SkeletonToken
    {
        public string Color
        {
            get => (string)_tokens["color"];
            set => _tokens["color"] = value;
        }

        public string ColorGradientEnd
        {
            get => (string)_tokens["colorGradientEnd"];
            set => _tokens["colorGradientEnd"] = value;
        }

        public string GradientFromColor
        {
            get => (string)_tokens["gradientFromColor"];
            set => _tokens["gradientFromColor"] = value;
        }

        public string GradientToColor
        {
            get => (string)_tokens["gradientToColor"];
            set => _tokens["gradientToColor"] = value;
        }

        public double TitleHeight
        {
            get => (double)_tokens["titleHeight"];
            set => _tokens["titleHeight"] = value;
        }

        public double BlockRadius
        {
            get => (double)_tokens["blockRadius"];
            set => _tokens["blockRadius"] = value;
        }

        public double ParagraphMarginTop
        {
            get => (double)_tokens["paragraphMarginTop"];
            set => _tokens["paragraphMarginTop"] = value;
        }

        public double ParagraphLiHeight
        {
            get => (double)_tokens["paragraphLiHeight"];
            set => _tokens["paragraphLiHeight"] = value;
        }

    }

    public partial class SkeletonStyle
    {
        private static Keyframes _skeletonClsLoading = new Keyframes($"ant-skeleton-loading",
            new CSSObject()
            {
                ["0%"] = new CSSObject()
                {
                    BackgroundPosition = "100% 50%",
                },
                ["100%"] = new CSSObject()
                {
                    BackgroundPosition = "0 50%",
                },
            });

        public static CSSObject GenSkeletonElementCommonSize(double size)
        {
            return new CSSObject()
            {
                Height = size,
                LineHeight = @$"{size}px",
            };
        }

        public static CSSObject GenSkeletonElementAvatarSize(double size)
        {
            return new CSSObject()
            {
                Width = size,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public static CSSObject GenSkeletonColor(SkeletonToken token)
        {
            return new CSSObject()
            {
                Background = token.SkeletonLoadingBackground,
                BackgroundSize = "400% 100%",
                AnimationName = _skeletonClsLoading,
                AnimationDuration = token.SkeletonLoadingMotionDuration,
                AnimationTimingFunction = "ease",
                AnimationIterationCount = "infinite",
            };
        }

        public static CSSObject GenSkeletonElementInputSize(double size)
        {
            return new CSSObject()
            {
                Width = size * 5,
                MinWidth = size * 5,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public static CSSObject GenSkeletonElementAvatar(SkeletonToken token)
        {
            var skeletonAvatarCls = token.SkeletonAvatarCls;
            var gradientFromColor = token.GradientFromColor;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            return new CSSObject()
            {
                [$"{skeletonAvatarCls}"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                    Background = gradientFromColor,
                    ["..."] = GenSkeletonElementAvatarSize(controlHeight)
                },
                [$"{skeletonAvatarCls}{skeletonAvatarCls}-circle"] = new CSSObject()
                {
                    BorderRadius = "50%",
                },
                [$"{skeletonAvatarCls}{skeletonAvatarCls}-lg"] = new CSSObject()
                {
                    ["..."] = GenSkeletonElementAvatarSize(controlHeightLG)
                },
                [$"{skeletonAvatarCls}{skeletonAvatarCls}-sm"] = new CSSObject()
                {
                    ["..."] = GenSkeletonElementAvatarSize(controlHeightSM)
                },
            };
        }

        public static CSSObject GenSkeletonElementInput(SkeletonToken token)
        {
            var controlHeight = token.ControlHeight;
            var borderRadiusSM = token.BorderRadiusSM;
            var skeletonInputCls = token.SkeletonInputCls;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var gradientFromColor = token.GradientFromColor;
            return new CSSObject()
            {
                [$"{skeletonInputCls}"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                    Background = gradientFromColor,
                    BorderRadius = borderRadiusSM,
                    ["..."] = GenSkeletonElementInputSize(controlHeight)
                },
                [$"{skeletonInputCls}-lg"] = new CSSObject()
                {
                    ["..."] = GenSkeletonElementInputSize(controlHeightLG)
                },
                [$"{skeletonInputCls}-sm"] = new CSSObject()
                {
                    ["..."] = GenSkeletonElementInputSize(controlHeightSM)
                },
            };
        }

        public static CSSObject GenSkeletonElementImageSize(double size)
        {
            return new CSSObject()
            {
                Width = size,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public static CSSObject GenSkeletonElementImage(SkeletonToken token)
        {
            var skeletonImageCls = token.SkeletonImageCls;
            var imageSizeBase = token.ImageSizeBase;
            var gradientFromColor = token.GradientFromColor;
            var borderRadiusSM = token.BorderRadiusSM;
            return new CSSObject()
            {
                [$"{skeletonImageCls}"] = new CSSObject()
                {
                    Display = "flex",
                    AlignItems = "center",
                    JustifyContent = "center",
                    VerticalAlign = "top",
                    Background = gradientFromColor,
                    BorderRadius = borderRadiusSM,
                    ["..."] = GenSkeletonElementImageSize(imageSizeBase * 2),
                    [$"{skeletonImageCls}-path"] = new CSSObject()
                    {
                        ["fill"] = "#bfbfbf",
                    },
                    [$"{skeletonImageCls}-svg"] = new CSSObject()
                    {
                        ["..."] = GenSkeletonElementImageSize(imageSizeBase),
                        MaxWidth = imageSizeBase * 4,
                        MaxHeight = imageSizeBase * 4,
                    },
                    [$"{skeletonImageCls}-svg{skeletonImageCls}-svg-circle"] = new CSSObject()
                    {
                        BorderRadius = "50%",
                    },
                },
                [$"{skeletonImageCls}{skeletonImageCls}-circle"] = new CSSObject()
                {
                    BorderRadius = "50%",
                },
            };
        }

        public static CSSObject GenSkeletonElementButtonShape(SkeletonToken token, double size, string buttonCls)
        {
            var skeletonButtonCls = token.SkeletonButtonCls;
            return new CSSObject()
            {
                [$"{buttonCls}{skeletonButtonCls}-circle"] = new CSSObject()
                {
                    Width = size,
                    MinWidth = size,
                    BorderRadius = "50%",
                },
                [$"{buttonCls}{skeletonButtonCls}-round"] = new CSSObject()
                {
                    BorderRadius = size,
                },
            };
        }

        public static CSSObject GenSkeletonElementButtonSize(double size)
        {
            return new CSSObject()
            {
                Width = size * 2,
                MinWidth = size * 2,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public static CSSObject GenSkeletonElementButton(SkeletonToken token)
        {
            var borderRadiusSM = token.BorderRadiusSM;
            var skeletonButtonCls = token.SkeletonButtonCls;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var gradientFromColor = token.GradientFromColor;
            return new CSSObject()
            {
                [$"{skeletonButtonCls}"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                    Background = gradientFromColor,
                    BorderRadius = borderRadiusSM,
                    Width = controlHeight * 2,
                    MinWidth = controlHeight * 2,
                    ["..."] = GenSkeletonElementButtonSize(controlHeight)
                },
                ["..."] = GenSkeletonElementButtonShape(token, controlHeight, skeletonButtonCls),
                [$"{skeletonButtonCls}-lg"] = new CSSObject()
                {
                    ["..."] = GenSkeletonElementButtonSize(controlHeightLG)
                },
                ["..."] = GenSkeletonElementButtonShape(token, controlHeightLG, $"{skeletonButtonCls}-lg"),
                [$"{skeletonButtonCls}-sm"] = new CSSObject()
                {
                    ["..."] = GenSkeletonElementButtonSize(controlHeightSM)
                },
                ["..."] = GenSkeletonElementButtonShape(token, controlHeightSM, $"{skeletonButtonCls}-sm")
            };
        }

        public static CSSObject GenBaseStyle(SkeletonToken token)
        {
            var componentCls = token.ComponentCls;
            var skeletonAvatarCls = token.SkeletonAvatarCls;
            var skeletonTitleCls = token.SkeletonTitleCls;
            var skeletonParagraphCls = token.SkeletonParagraphCls;
            var skeletonButtonCls = token.SkeletonButtonCls;
            var skeletonInputCls = token.SkeletonInputCls;
            var skeletonImageCls = token.SkeletonImageCls;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var gradientFromColor = token.GradientFromColor;
            var padding = token.Padding;
            var marginSM = token.MarginSM;
            var borderRadius = token.BorderRadius;
            var titleHeight = token.TitleHeight;
            var blockRadius = token.BlockRadius;
            var paragraphLiHeight = token.ParagraphLiHeight;
            var controlHeightXS = token.ControlHeightXS;
            var paragraphMarginTop = token.ParagraphMarginTop;
            return new CSSObject()
            {
                [$"{componentCls}"] = new CSSObject()
                {
                    Display = "table",
                    Width = "100%",
                    [$"{componentCls}-header"] = new CSSObject()
                    {
                        Display = "table-cell",
                        PaddingInlineEnd = padding,
                        VerticalAlign = "top",
                        [$"{skeletonAvatarCls}"] = new CSSObject()
                        {
                            Display = "inline-block",
                            VerticalAlign = "top",
                            Background = gradientFromColor,
                            ["..."] = GenSkeletonElementAvatarSize(controlHeight)
                        },
                        [$"{skeletonAvatarCls}-circle"] = new CSSObject()
                        {
                            BorderRadius = "50%",
                        },
                        [$"{skeletonAvatarCls}-lg"] = new CSSObject()
                        {
                            ["..."] = GenSkeletonElementAvatarSize(controlHeightLG)
                        },
                        [$"{skeletonAvatarCls}-sm"] = new CSSObject()
                        {
                            ["..."] = GenSkeletonElementAvatarSize(controlHeightSM)
                        },
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Display = "table-cell",
                        Width = "100%",
                        VerticalAlign = "top",
                        [$"{skeletonTitleCls}"] = new CSSObject()
                        {
                            Width = "100%",
                            Height = titleHeight,
                            Background = gradientFromColor,
                            BorderRadius = blockRadius,
                            [$"+ {skeletonParagraphCls}"] = new CSSObject()
                            {
                                MarginBlockStart = controlHeightSM,
                            },
                        },
                        [$"{skeletonParagraphCls}"] = new CSSObject()
                        {
                            Padding = 0,
                            ["> li"] = new CSSObject()
                            {
                                Width = "100%",
                                Height = paragraphLiHeight,
                                ListStyle = "none",
                                Background = gradientFromColor,
                                BorderRadius = blockRadius,
                                ["+ li"] = new CSSObject()
                                {
                                    MarginBlockStart = controlHeightXS,
                                },
                            },
                        },
                        [$"{skeletonParagraphCls}> li:last-child:not(:first-child):not(:nth-child(2))"] = new CSSObject()
                        {
                            Width = "61%",
                        },
                    },
                    [$"&-round {componentCls}-content"] = new CSSObject()
                    {
                        [$"{skeletonTitleCls}, {skeletonParagraphCls} > li"] = new CSSObject()
                        {
                            BorderRadius = borderRadius,
                        },
                    },
                },
                [$"{componentCls}-with-avatar {componentCls}-content"] = new CSSObject()
                {
                    [$"{skeletonTitleCls}"] = new CSSObject()
                    {
                        MarginBlockStart = marginSM,
                        [$"+ {skeletonParagraphCls}"] = new CSSObject()
                        {
                            MarginBlockStart = paragraphMarginTop,
                        },
                    },
                },
                [$"{componentCls}{componentCls}-element"] = new CSSObject()
                {
                    Display = "inline-block",
                    Width = "auto",
                    ["..."] = GenSkeletonElementButton(token),
                    ["..."] = GenSkeletonElementAvatar(token),
                    ["..."] = GenSkeletonElementInput(token),
                    ["..."] = GenSkeletonElementImage(token)
                },
                [$"{componentCls}{componentCls}-block"] = new CSSObject()
                {
                    Width = "100%",
                    [$"{skeletonButtonCls}"] = new CSSObject()
                    {
                        Width = "100%",
                    },
                    [$"{skeletonInputCls}"] = new CSSObject()
                    {
                        Width = "100%",
                    },
                },
                [$"{componentCls}{componentCls}-active"] = new CSSObject()
                {
                    [$"{skeletonTitleCls},{skeletonParagraphCls}>li,{skeletonAvatarCls},{skeletonButtonCls},{skeletonInputCls},{skeletonImageCls}"] = new CSSObject()
                    {
                        ["..."] = GenSkeletonColor(token)
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Skeleton",
                (token) =>
                {
                    var componentCls = token.ComponentCls;
                    var skeletonToken = MergeToken(
                        token,
                        new SkeletonToken()
                        {
                            SkeletonAvatarCls = @$"{componentCls}-avatar",
                            SkeletonTitleCls = @$"{componentCls}-title",
                            SkeletonParagraphCls = @$"{componentCls}-paragraph",
                            SkeletonButtonCls = @$"{componentCls}-button",
                            SkeletonInputCls = @$"{componentCls}-input",
                            SkeletonImageCls = @$"{componentCls}-image",
                            ImageSizeBase = token.ControlHeight * 1.5,
                            BorderRadius = 100,
                            SkeletonLoadingBackground = @$"linear-gradient(90deg, {token.GradientFromColor} 25%, {token.GradientToColor} 37%, {token.GradientFromColor} 63%)",
                            SkeletonLoadingMotionDuration = "1.4s",
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(skeletonToken),
                    };
                },
                (token) =>
                {
                    var colorFillContent = token.ColorFillContent;
                    var colorFill = token.ColorFill;
                    var gradientFromColor = colorFillContent;
                    var gradientToColor = colorFill;
                    return new SkeletonToken()
                    {
                        Color = gradientFromColor,
                        ColorGradientEnd = gradientToColor,
                        GradientFromColor = gradientFromColor,
                        GradientToColor = gradientToColor,
                        TitleHeight = token.ControlHeight / 2,
                        BlockRadius = token.BorderRadiusSM,
                        ParagraphMarginTop = token.MarginLG + token.MarginXXS,
                        ParagraphLiHeight = token.ControlHeight / 2,
                    };
                },
                new GenOptions()
                {
                    DeprecatedTokens = new ()
                    {
                        ("color", "gradientFromColor"),
                        ("colorGradientEnd", "gradientToColor"),
                    }
                });
        }

    }

    public partial class SkeletonToken : TokenWithCommonCls
    {
        public string SkeletonAvatarCls
        {
            get => (string)_tokens["skeletonAvatarCls"];
            set => _tokens["skeletonAvatarCls"] = value;
        }

        public string SkeletonTitleCls
        {
            get => (string)_tokens["skeletonTitleCls"];
            set => _tokens["skeletonTitleCls"] = value;
        }

        public string SkeletonParagraphCls
        {
            get => (string)_tokens["skeletonParagraphCls"];
            set => _tokens["skeletonParagraphCls"] = value;
        }

        public string SkeletonButtonCls
        {
            get => (string)_tokens["skeletonButtonCls"];
            set => _tokens["skeletonButtonCls"] = value;
        }

        public string SkeletonInputCls
        {
            get => (string)_tokens["skeletonInputCls"];
            set => _tokens["skeletonInputCls"] = value;
        }

        public string SkeletonImageCls
        {
            get => (string)_tokens["skeletonImageCls"];
            set => _tokens["skeletonImageCls"] = value;
        }

        public double ImageSizeBase
        {
            get => (double)_tokens["imageSizeBase"];
            set => _tokens["imageSizeBase"] = value;
        }

        public string SkeletonLoadingBackground
        {
            get => (string)_tokens["skeletonLoadingBackground"];
            set => _tokens["skeletonLoadingBackground"] = value;
        }

        public string SkeletonLoadingMotionDuration
        {
            get => (string)_tokens["skeletonLoadingMotionDuration"];
            set => _tokens["skeletonLoadingMotionDuration"] = value;
        }

        public double BorderRadius
        {
            get => (double)_tokens["borderRadius"];
            set => _tokens["borderRadius"] = value;
        }

    }

}
