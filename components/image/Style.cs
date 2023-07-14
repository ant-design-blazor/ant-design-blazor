using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class ImageToken
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class ImageToken : TokenWithCommonCls
    {
        public string PreviewCls { get; set; }

        public string ModalMaskBg { get; set; }

        public string ImagePreviewOperationDisabledColor { get; set; }

        public int ImagePreviewOperationSize { get; set; }

        public int ImagePreviewSwitchSize { get; set; }

        public string ImagePreviewOperationColor { get; set; }

    }

    public class PositionType
    {
    }

    public partial class Image
    {
        public CSSObject GenBoxStyle(PositionType position)
        {
            return new CSSObject()
            {
                Position = position,
                Inset = 0,
            };
        }

        public CSSObject GenImageMaskStyle(ImageToken token)
        {
            var iconCls = token.IconCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var paddingXXS = token.PaddingXXS;
            var marginXXS = token.MarginXXS;
            var prefixCls = token.PrefixCls;
            return new CSSObject()
            {
                Position = "absolute",
                Inset = 0,
                Display = "flex",
                AlignItems = "center",
                JustifyContent = "center",
                Color = "#fff",
                Background = New TinyColor('#000').setAlpha(0.5).toRgbString(),
                Cursor = "pointer",
                Opacity = 0,
                Transition = @$"opacity {motionDurationSlow}",
                [$".{prefixCls}-mask-info"] = new CSSObject()
                {
                    ["..."] = textEllipsis,
                    Padding = @$"0 {paddingXXS}px",
                    [iconCls] = new CSSObject()
                    {
                        MarginInlineEnd = marginXXS,
                        ["svg"] = new CSSObject()
                        {
                            VerticalAlign = "baseline",
                        },
                    },
                },
            };
        }

        public CSSObject GenPreviewOperationsStyle(ImageToken token)
        {
            var previewCls = token.PreviewCls;
            var modalMaskBg = token.ModalMaskBg;
            var paddingSM = token.PaddingSM;
            var imagePreviewOperationDisabledColor = token.ImagePreviewOperationDisabledColor;
            var motionDurationSlow = token.MotionDurationSlow;
            var operationBg = New TinyColor(modalMaskBg).setAlpha(0.1);
            var operationBgHover = OperationBg.Clone().setAlpha(0.2);
            return new CSSObject()
            {
                [$"{previewCls}-operations"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "flex",
                    FlexDirection = "row-reverse",
                    AlignItems = "center",
                    Color = token.ImagePreviewOperationColor,
                    ListStyle = "none",
                    Background = OperationBg.ToRgbString(),
                    PointerEvents = "auto",
                    ["&-operation"] = new CSSObject()
                    {
                        MarginInlineStart = paddingSM,
                        Padding = paddingSM,
                        Cursor = "pointer",
                        Transition = @$"all {motionDurationSlow}",
                        ["&:hover"] = new CSSObject()
                        {
                            Background = OperationBgHover.ToRgbString()
                        },
                        ["&-disabled"] = new CSSObject()
                        {
                            Color = imagePreviewOperationDisabledColor,
                            PointerEvents = "none",
                        },
                        ["&:last-of-type"] = new CSSObject()
                        {
                            MarginInlineStart = 0,
                        },
                    },
                    ["&-progress"] = new CSSObject()
                    {
                        Position = "absolute",
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "50%",
                        },
                        Transform = "translateX(-50%)",
                    },
                    ["&-icon"] = new CSSObject()
                    {
                        FontSize = token.ImagePreviewOperationSize,
                    },
                },
            };
        }

        public CSSObject GenPreviewSwitchStyle(ImageToken token)
        {
            var modalMaskBg = token.ModalMaskBg;
            var iconCls = token.IconCls;
            var imagePreviewOperationDisabledColor = token.ImagePreviewOperationDisabledColor;
            var previewCls = token.PreviewCls;
            var zIndexPopup = token.ZIndexPopup;
            var motionDurationSlow = token.MotionDurationSlow;
            var operationBg = New TinyColor(modalMaskBg).setAlpha(0.1);
            var operationBgHover = OperationBg.Clone().setAlpha(0.2);
            return new CSSObject()
            {
                [$"{previewCls}-switch-left, {previewCls}-switch-right"] = new CSSObject()
                {
                    Position = "fixed",
                    InsetBlockStart = "50%",
                    ZIndex = zIndexPopup + 1,
                    Display = "flex",
                    AlignItems = "center",
                    JustifyContent = "center",
                    Width = token.ImagePreviewSwitchSize,
                    Height = token.ImagePreviewSwitchSize,
                    MarginTop = -token.ImagePreviewSwitchSize / 2,
                    Color = token.ImagePreviewOperationColor,
                    Background = OperationBg.ToRgbString(),
                    BorderRadius = "50%",
                    Transform = @$"translateY(-50%)",
                    Cursor = "pointer",
                    Transition = @$"all {motionDurationSlow}",
                    PointerEvents = "auto",
                    ["&:hover"] = new CSSObject()
                    {
                        Background = OperationBgHover.ToRgbString()
                    },
                    ["&-disabled"] = new CSSObject()
                    {
                        ["&, &:hover"] = new CSSObject()
                        {
                            Color = imagePreviewOperationDisabledColor,
                            Background = "transparent",
                            Cursor = "not-allowed",
                            [$"> {iconCls}"] = new CSSObject()
                            {
                                Cursor = "not-allowed",
                            },
                        },
                    },
                    [$"> {iconCls}"] = new CSSObject()
                    {
                        FontSize = token.ImagePreviewOperationSize,
                    },
                },
                [$"{previewCls}-switch-left"] = new CSSObject()
                {
                    InsetInlineStart = token.MarginSM,
                },
                [$"{previewCls}-switch-right"] = new CSSObject()
                {
                    InsetInlineEnd = token.MarginSM,
                },
            };
        }

        public Unknown_1 GenImagePreviewStyle(ImageToken token)
        {
            var motionEaseOut = token.MotionEaseOut;
            var previewCls = token.PreviewCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var componentCls = token.ComponentCls;
            return new Unknown_6
            {
                new Unknown_7()
                {
                    [$"{componentCls}-preview-root"] = new Unknown_8()
                    {
                        [previewCls] = new Unknown_9()
                        {
                            Height = "100%",
                            TextAlign = "center",
                            PointerEvents = "none",
                        },
                        [$"{previewCls}-body"] = new Unknown_10()
                        {
                            ["..."] = GenBoxStyle(),
                            Overflow = "hidden",
                        },
                        [$"{previewCls}-img"] = new Unknown_11()
                        {
                            MaxWidth = "100%",
                            MaxHeight = "100%",
                            VerticalAlign = "middle",
                            Transform = "scale3d(1, 1, 1)",
                            Cursor = "grab",
                            Transition = @$"transform {motionDurationSlow} {motionEaseOut} 0s",
                            UserSelect = "none",
                            PointerEvents = "auto",
                            ["&-wrapper"] = new Unknown_12()
                            {
                                ["..."] = GenBoxStyle(),
                                Transition = @$"transform {motionDurationSlow} {motionEaseOut} 0s",
                                Display = "flex",
                                JustifyContent = "center",
                                AlignItems = "center",
                                ["&::before"] = new Unknown_13()
                                {
                                    Display = "inline-block",
                                    Width = 1,
                                    Height = "50%",
                                    MarginInlineEnd = -1,
                                    Content = "\"\"",
                                },
                            },
                        },
                        [$"{previewCls}-moving"] = new Unknown_14()
                        {
                            [$"{previewCls}-preview-img"] = new Unknown_15()
                            {
                                Cursor = "grabbing",
                                ["&-wrapper"] = new Unknown_16()
                                {
                                    TransitionDuration = "0s",
                                },
                            },
                        },
                    },
                },
                new Unknown_17()
                {
                    [$"{componentCls}-preview-root"] = new Unknown_18()
                    {
                        [$"{previewCls}-wrap"] = new Unknown_19()
                        {
                            ZIndex = token.ZIndexPopup,
                        },
                    },
                },
                new Unknown_20()
                {
                    [$"{componentCls}-preview-operations-wrapper"] = new Unknown_21()
                    {
                        Position = "fixed",
                        InsetBlockStart = 0,
                        InsetInlineEnd = 0,
                        ZIndex = token.ZIndexPopup + 1,
                        Width = "100%",
                    },
                    ["&"] = new Unknown_22
                    {
                        GenPreviewOperationsStyle(token),
                        GenPreviewSwitchStyle(token)
                    }
                },
            };
        }

        public Unknown_2 GenImageStyle(ImageToken token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_23()
            {
                [componentCls] = new Unknown_24()
                {
                    Position = "relative",
                    Display = "inline-block",
                    [$"{componentCls}-img"] = new Unknown_25()
                    {
                        Width = "100%",
                        Height = "auto",
                        VerticalAlign = "middle",
                    },
                    [$"{componentCls}-img-placeholder"] = new Unknown_26()
                    {
                        BackgroundColor = token.ColorBgContainerDisabled,
                        BackgroundImage = \"url("data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAxNiAxNiIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTQuNSAyLjVoLTEzQS41LjUgMCAwIDAgMSAzdjEwYS41LjUgMCAwIDAgLjUuNWgxM2EuNS41IDAgMCAwIC41LS41VjNhLjUuNSAwIDAgMC0uNS0uNXpNNS4yODEgNC43NWExIDEgMCAwIDEgMCAyIDEgMSAwIDAgMSAwLTJ6bTguMDMgNi44M2EuMTI3LjEyNyAwIDAgMS0uMDgxLjAzSDIuNzY5YS4xMjUuMTI1IDAgMCAxLS4wOTYtLjIwN2wyLjY2MS0zLjE1NmEuMTI2LjEyNiAwIDAgMSAuMTc3LS4wMTZsLjAxNi4wMTZMNy4wOCAxMC4wOWwyLjQ3LTIuOTNhLjEyNi4xMjYgMCAwIDEgLjE3Ny0uMDE2bC4wMTUuMDE2IDMuNTg4IDQuMjQ0YS4xMjcuMTI3IDAgMCAxLS4wMi4xNzV6IiBmaWxsPSIjOEM4QzhDIiBmaWxsLXJ1bGU9Im5vbnplcm8iLz48L3N2Zz4=")\",
                        BackgroundRepeat = "no-repeat",
                        BackgroundPosition = "center center",
                        BackgroundSize = "30%",
                    },
                    [$"{componentCls}-mask"] = new Unknown_27()
                    {
                        ["..."] = GenImageMaskStyle(token)
                    },
                    [$"{componentCls}-mask:hover"] = new Unknown_28()
                    {
                        Opacity = 1,
                    },
                    [$"{componentCls}-placeholder"] = new Unknown_29()
                    {
                        ["..."] = GenBoxStyle()
                    },
                },
            };
        }

        public Unknown_3 GenPreviewMotion(Unknown_30 token)
        {
            var previewCls = token.PreviewCls;
            return new Unknown_31()
            {
                [$"{previewCls}-root"] = InitZoomMotion(token, "zoom"),
                ["&"] = InitFadeMotion(token, true)
            };
        }

        public Unknown_4 GenComponentStyleHook(Unknown_32 token)
        {
            var imagePreviewOperationColor = New TinyColor(token.ColorTextLightSolid);
            var previewCls = @$"{token.ComponentCls}-preview";
            var imageToken = MergeToken(
                token,
                new Unknown_33()
                {
                    PreviewCls = previewCls,
                    ImagePreviewOperationColor = ImagePreviewOperationColor.ToRgbString(),
                    ImagePreviewOperationDisabledColor = New TinyColor(imagePreviewOperationColor)
        .setAlpha(0.25)
        .toRgbString(),
                    ModalMaskBg = New TinyColor('#000').setAlpha(0.45).toRgbString(),
                    ImagePreviewOperationSize = token.FontSizeIcon * 1.5,
                    ImagePreviewSwitchSize = token.ControlHeightLG,
                });
            return new Unknown_34
            {
                GenImageStyle(imageToken),
                GenImagePreviewStyle(imageToken),
                GenModalMaskStyle(mergeToken<ImageToken>(imageToken, { componentCls: previewCls })),
                GenPreviewMotion(imageToken)
            };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_35 token)
        {
            return new Unknown_36()
            {
                ZIndexPopup = token.ZIndexPopupBase + 80,
            };
        }

    }

}