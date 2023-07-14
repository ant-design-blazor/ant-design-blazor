using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SkeletonToken
    {
    }

    public partial class Skeleton
    {
        private Keyframes skeletonClsLoading = new Keyframes()
        {
            ["0%"] = new Keyframes()
            {
                BackgroundPosition = "100% 50%",
            },
            ["100%"] = new Keyframes()
            {
                BackgroundPosition = "0 50%",
            },
        };

        public CSSObject GenSkeletonElementCommonSize(int size)
        {
            return new CSSObject()
            {
                Height = size,
                LineHeight = @$"{size}px",
            };
        }

        public CSSObject GenSkeletonElementAvatarSize(int size)
        {
            return new CSSObject()
            {
                Width = size,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public CSSObject GenSkeletonColor(SkeletonToken token)
        {
            return new CSSObject()
            {
                Background = token.SkeletonLoadingBackground,
                BackgroundSize = "400% 100%",
                AnimationName = skeletonClsLoading,
                AnimationDuration = token.SkeletonLoadingMotionDuration,
                AnimationTimingFunction = "ease",
                AnimationIterationCount = "infinite",
            };
        }

        public CSSObject GenSkeletonElementInputSize(int size)
        {
            return new CSSObject()
            {
                Width = size * 5,
                MinWidth = size * 5,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public CSSObject GenSkeletonElementAvatar(SkeletonToken token)
        {
            var skeletonAvatarCls = token.SkeletonAvatarCls;
            var color = token.Color;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            return new CSSObject()
            {
                [$"{skeletonAvatarCls}"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                    Background = color,
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

        public CSSObject GenSkeletonElementInput(SkeletonToken token)
        {
            var controlHeight = token.ControlHeight;
            var borderRadiusSM = token.BorderRadiusSM;
            var skeletonInputCls = token.SkeletonInputCls;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var color = token.Color;
            return new CSSObject()
            {
                [$"{skeletonInputCls}"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                    Background = color,
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

        public CSSObject GenSkeletonElementImageSize(int size)
        {
            return new CSSObject()
            {
                Width = size,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public CSSObject GenSkeletonElementImage(SkeletonToken token)
        {
            var skeletonImageCls = token.SkeletonImageCls;
            var imageSizeBase = token.ImageSizeBase;
            var color = token.Color;
            var borderRadiusSM = token.BorderRadiusSM;
            return new CSSObject()
            {
                [$"{skeletonImageCls}"] = new CSSObject()
                {
                    Display = "flex",
                    AlignItems = "center",
                    JustifyContent = "center",
                    VerticalAlign = "top",
                    Background = color,
                    BorderRadius = borderRadiusSM,
                    ["..."] = GenSkeletonElementImageSize(imageSizeBase * 2),
                    [$"{skeletonImageCls}-path"] = new CSSObject()
                    {
                        Fill = "#bfbfbf",
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

        public CSSObject GenSkeletonElementButtonShape(SkeletonToken token, int size, string buttonCls)
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

        public CSSObject GenSkeletonElementButtonSize(int size)
        {
            return new CSSObject()
            {
                Width = size * 2,
                MinWidth = size * 2,
                ["..."] = GenSkeletonElementCommonSize(size)
            };
        }

        public CSSObject GenSkeletonElementButton(SkeletonToken token)
        {
            var borderRadiusSM = token.BorderRadiusSM;
            var skeletonButtonCls = token.SkeletonButtonCls;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var color = token.Color;
            return new CSSObject()
            {
                [$"{skeletonButtonCls}"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                    Background = color,
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

        public Unknown_1 GenBaseStyle(SkeletonToken token)
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
            var color = token.Color;
            var padding = token.Padding;
            var marginSM = token.MarginSM;
            var borderRadius = token.BorderRadius;
            var skeletonTitleHeight = token.SkeletonTitleHeight;
            var skeletonBlockRadius = token.SkeletonBlockRadius;
            var skeletonParagraphLineHeight = token.SkeletonParagraphLineHeight;
            var controlHeightXS = token.ControlHeightXS;
            var skeletonParagraphMarginTop = token.SkeletonParagraphMarginTop;
            return new Unknown_4()
            {
                [$"{componentCls}"] = new Unknown_5()
                {
                    Display = "table",
                    Width = "100%",
                    [$"{componentCls}-header"] = new Unknown_6()
                    {
                        Display = "table-cell",
                        PaddingInlineEnd = padding,
                        VerticalAlign = "top",
                        [$"{skeletonAvatarCls}"] = new Unknown_7()
                        {
                            Display = "inline-block",
                            VerticalAlign = "top",
                            Background = color,
                            ["..."] = GenSkeletonElementAvatarSize(controlHeight)
                        },
                        [$"{skeletonAvatarCls}-circle"] = new Unknown_8()
                        {
                            BorderRadius = "50%",
                        },
                        [$"{skeletonAvatarCls}-lg"] = new Unknown_9()
                        {
                            ["..."] = GenSkeletonElementAvatarSize(controlHeightLG)
                        },
                        [$"{skeletonAvatarCls}-sm"] = new Unknown_10()
                        {
                            ["..."] = GenSkeletonElementAvatarSize(controlHeightSM)
                        },
                    },
                    [$"{componentCls}-content"] = new Unknown_11()
                    {
                        Display = "table-cell",
                        Width = "100%",
                        VerticalAlign = "top",
                        [$"{skeletonTitleCls}"] = new Unknown_12()
                        {
                            Width = "100%",
                            Height = skeletonTitleHeight,
                            Background = color,
                            BorderRadius = skeletonBlockRadius,
                            [$"+ {skeletonParagraphCls}"] = new Unknown_13()
                            {
                                MarginBlockStart = controlHeightSM,
                            },
                        },
                        [$"{skeletonParagraphCls}"] = new Unknown_14()
                        {
                            Padding = 0,
                            ["> li"] = new Unknown_15()
                            {
                                Width = "100%",
                                Height = skeletonParagraphLineHeight,
                                ListStyle = "none",
                                Background = color,
                                BorderRadius = skeletonBlockRadius,
                                ["+ li"] = new Unknown_16()
                                {
                                    MarginBlockStart = controlHeightXS,
                                },
                            },
                        },
                        [$"{skeletonParagraphCls}> li:last-child:not(:first-child):not(:nth-child(2))"] = new Unknown_17()
                        {
                            Width = "61%",
                        },
                    },
                    [$"&-round {componentCls}-content"] = new Unknown_18()
                    {
                        [$"{skeletonTitleCls}, {skeletonParagraphCls} > li"] = new Unknown_19()
                        {
                            BorderRadius = borderRadius,
                        },
                    },
                },
                [$"{componentCls}-with-avatar {componentCls}-content"] = new Unknown_20()
                {
                    [$"{skeletonTitleCls}"] = new Unknown_21()
                    {
                        MarginBlockStart = marginSM,
                        [$"+ {skeletonParagraphCls}"] = new Unknown_22()
                        {
                            MarginBlockStart = skeletonParagraphMarginTop,
                        },
                    },
                },
                [$"{componentCls}{componentCls}-element"] = new Unknown_23()
                {
                    Display = "inline-block",
                    Width = "auto",
                    ["..."] = GenSkeletonElementButton(token),
                    ["..."] = GenSkeletonElementAvatar(token),
                    ["..."] = GenSkeletonElementInput(token),
                    ["..."] = GenSkeletonElementImage(token)
                },
                [$"{componentCls}{componentCls}-block"] = new Unknown_24()
                {
                    Width = "100%",
                    [$"{skeletonButtonCls}"] = new Unknown_25()
                    {
                        Width = "100%",
                    },
                    [$"{skeletonInputCls}"] = new Unknown_26()
                    {
                        Width = "100%",
                    },
                },
                [$"{componentCls}{componentCls}-active"] = new Unknown_27()
                {
                    [$"{skeletonTitleCls},{skeletonParagraphCls}>li,{skeletonAvatarCls},{skeletonButtonCls},{skeletonInputCls},{skeletonImageCls}"] = new Unknown_28()
                    {
                        ["..."] = GenSkeletonColor(token)
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_29 token)
        {
            var componentCls = token.ComponentCls;
            var skeletonToken = MergeToken(
                token,
                new Unknown_30()
                {
                    SkeletonAvatarCls = @$"{componentCls}-avatar",
                    SkeletonTitleCls = @$"{componentCls}-title",
                    SkeletonParagraphCls = @$"{componentCls}-paragraph",
                    SkeletonButtonCls = @$"{componentCls}-button",
                    SkeletonInputCls = @$"{componentCls}-input",
                    SkeletonImageCls = @$"{componentCls}-image",
                    ImageSizeBase = token.ControlHeight * 1.5,
                    SkeletonTitleHeight = token.ControlHeight / 2,
                    SkeletonBlockRadius = token.BorderRadiusSM,
                    SkeletonParagraphLineHeight = token.ControlHeight / 2,
                    SkeletonParagraphMarginTop = token.MarginLG + token.MarginXXS,
                    BorderRadius = 100,
                    SkeletonLoadingBackground = @$"linear-gradient(90deg, {token.Color} 25%, {token.ColorGradientEnd} 37%, {token.Color} 63%)",
                    SkeletonLoadingMotionDuration = "1.4s",
                });
            return new Unknown_31 { GenBaseStyle(skeletonToken) };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_32 token)
        {
            var colorFillContent = token.ColorFillContent;
            var colorFill = token.ColorFill;
            return new Unknown_33()
            {
                Color = colorFillContent,
                ColorGradientEnd = colorFill,
            };
        }

    }

    public partial class SkeletonToken : TokenWithCommonCls
    {
        public string SkeletonAvatarCls { get; set; }

        public string SkeletonTitleCls { get; set; }

        public string SkeletonParagraphCls { get; set; }

        public string SkeletonButtonCls { get; set; }

        public string SkeletonInputCls { get; set; }

        public string SkeletonImageCls { get; set; }

        public int ImageSizeBase { get; set; }

        public int SkeletonTitleHeight { get; set; }

        public int SkeletonBlockRadius { get; set; }

        public int SkeletonParagraphLineHeight { get; set; }

        public int SkeletonParagraphMarginTop { get; set; }

        public string SkeletonLoadingBackground { get; set; }

        public string SkeletonLoadingMotionDuration { get; set; }

        public int BorderRadius { get; set; }

    }

}