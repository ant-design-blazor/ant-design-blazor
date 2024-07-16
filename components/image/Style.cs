using System;
using CssInCSharp;
using CssInCSharp.Colors;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Zoom;
using static AntDesign.Fade;
using static AntDesign.ModalStyle;

namespace AntDesign
{
    public partial class ImageToken : TokenWithCommonCls
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public double PreviewOperationSize
        {
            get => (double)_tokens["previewOperationSize"];
            set => _tokens["previewOperationSize"] = value;
        }

        public string PreviewOperationColor
        {
            get => (string)_tokens["previewOperationColor"];
            set => _tokens["previewOperationColor"] = value;
        }

        public string PreviewOperationHoverColor
        {
            get => (string)_tokens["previewOperationHoverColor"];
            set => _tokens["previewOperationHoverColor"] = value;
        }

        public string PreviewOperationColorDisabled
        {
            get => (string)_tokens["previewOperationColorDisabled"];
            set => _tokens["previewOperationColorDisabled"] = value;
        }

    }

    public partial class ImageToken
    {
        public string PreviewCls
        {
            get => (string)_tokens["previewCls"];
            set => _tokens["previewCls"] = value;
        }

        public string ModalMaskBg
        {
            get => (string)_tokens["modalMaskBg"];
            set => _tokens["modalMaskBg"] = value;
        }

        public double ImagePreviewSwitchSize
        {
            get => (double)_tokens["imagePreviewSwitchSize"];
            set => _tokens["imagePreviewSwitchSize"] = value;
        }

    }

    public partial class ImageStyle
    {
        public static CSSObject GenBoxStyle(string position = null)
        {
            return new CSSObject()
            {
                Position = position ?? "absolute",
                Inset = 0,
            };
        }

        public static CSSObject GenImageMaskStyle(ImageToken token)
        {
            var iconCls = token.IconCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var paddingXXS = token.PaddingXXS;
            var marginXXS = token.MarginXXS;
            var prefixCls = token.PrefixCls;
            var colorTextLightSolid = token.ColorTextLightSolid;
            return new CSSObject()
            {
                Position = "absolute",
                Inset = 0,
                Display = "flex",
                AlignItems = "center",
                JustifyContent = "center",
                Color = colorTextLightSolid,
                Background = new TinyColor("#000").SetAlpha(0.5).ToRgbString(),
                Cursor = "pointer",
                Opacity = 0,
                Transition = @$"opacity {motionDurationSlow}",
                [$".{prefixCls}-mask-info"] = new CSSObject()
                {
                    ["..."] = TextEllipsis,
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

        public static CSSObject GenPreviewOperationsStyle(ImageToken token)
        {
            var previewCls = token.PreviewCls;
            var modalMaskBg = token.ModalMaskBg;
            var paddingSM = token.PaddingSM;
            var marginXL = token.MarginXL;
            var margin = token.Margin;
            var paddingLG = token.PaddingLG;
            var previewOperationColorDisabled = token.PreviewOperationColorDisabled;
            var previewOperationHoverColor = token.PreviewOperationHoverColor;
            var motionDurationSlow = token.MotionDurationSlow;
            var iconCls = token.IconCls;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var operationBg = new TinyColor(modalMaskBg).SetAlpha(0.1);
            var operationBgHover = operationBg.Clone().SetAlpha(0.2);
            return new CSSObject()
            {
                [$"{previewCls}-footer"] = new CSSObject()
                {
                    Position = "fixed",
                    Bottom = marginXL,
                    Left = new PropertySkip()
                    {
                        SkipCheck = true,
                        Value = 0,
                    },
                    Width = "100%",
                    Display = "flex",
                    FlexDirection = "column",
                    AlignItems = "center",
                    Color = token.PreviewOperationColor,
                },
                [$"{previewCls}-progress"] = new CSSObject()
                {
                    MarginBottom = margin,
                },
                [$"{previewCls}-close"] = new CSSObject()
                {
                    Position = "fixed",
                    Top = marginXL,
                    Right = new PropertySkip()
                    {
                        SkipCheck = true,
                        Value = marginXL,
                    },
                    Display = "flex",
                    Color = colorTextLightSolid,
                    BackgroundColor = operationBg.ToRgbString(),
                    BorderRadius = "50%",
                    Padding = paddingSM,
                    Outline = 0,
                    Border = 0,
                    Cursor = "pointer",
                    Transition = @$"all {motionDurationSlow}",
                    ["&:hover"] = new CSSObject()
                    {
                        BackgroundColor = operationBgHover.ToRgbString(),
                    },
                    [$"& > {iconCls}"] = new CSSObject()
                    {
                        FontSize = token.PreviewOperationSize,
                    },
                },
                [$"{previewCls}-operations"] = new CSSObject()
                {
                    Display = "flex",
                    AlignItems = "center",
                    Padding = @$"0 {paddingLG}px",
                    BackgroundColor = operationBg.ToRgbString(),
                    BorderRadius = 100,
                    ["&-operation"] = new CSSObject()
                    {
                        MarginInlineStart = paddingSM,
                        Padding = paddingSM,
                        Cursor = "pointer",
                        Transition = @$"all {motionDurationSlow}",
                        UserSelect = "none",
                        [$"&:not({previewCls}-operations-operation-disabled):hover > {iconCls}"] = new CSSObject()
                        {
                            Color = previewOperationHoverColor,
                        },
                        ["&-disabled"] = new CSSObject()
                        {
                            Color = previewOperationColorDisabled,
                            Cursor = "not-allowed",
                        },
                        ["&:first-of-type"] = new CSSObject()
                        {
                            MarginInlineStart = 0,
                        },
                        [$"& > {iconCls}"] = new CSSObject()
                        {
                            FontSize = token.PreviewOperationSize,
                        },
                    },
                },
            };
        }

        public static CSSObject GenPreviewSwitchStyle(ImageToken token)
        {
            var modalMaskBg = token.ModalMaskBg;
            var iconCls = token.IconCls;
            var previewOperationColorDisabled = token.PreviewOperationColorDisabled;
            var previewCls = token.PreviewCls;
            var zIndexPopup = token.ZIndexPopup;
            var motionDurationSlow = token.MotionDurationSlow;
            var operationBg = new TinyColor(modalMaskBg).SetAlpha(0.1);
            var operationBgHover = operationBg.Clone().SetAlpha(0.2);
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
                    Color = token.PreviewOperationColor,
                    Background = operationBg.ToRgbString(),
                    BorderRadius = "50%",
                    Transform = @$"translateY(-50%)",
                    Cursor = "pointer",
                    Transition = @$"all {motionDurationSlow}",
                    UserSelect = "none",
                    ["&:hover"] = new CSSObject()
                    {
                        Background = operationBgHover.ToRgbString(),
                    },
                    ["&-disabled"] = new CSSObject()
                    {
                        ["&, &:hover"] = new CSSObject()
                        {
                            Color = previewOperationColorDisabled,
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
                        FontSize = token.PreviewOperationSize,
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

        public static CSSInterpolation[] GenImagePreviewStyle(ImageToken token)
        {
            var motionEaseOut = token.MotionEaseOut;
            var previewCls = token.PreviewCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var componentCls = token.ComponentCls;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-preview-root"] = new CSSObject()
                    {
                        [previewCls] = new CSSObject()
                        {
                            Height = "100%",
                            TextAlign = "center",
                            PointerEvents = "none",
                        },
                        [$"{previewCls}-body"] = new CSSObject()
                        {
                            ["..."] = GenBoxStyle(),
                            Overflow = "hidden",
                        },
                        [$"{previewCls}-img"] = new CSSObject()
                        {
                            MaxWidth = "100%",
                            MaxHeight = "70%",
                            VerticalAlign = "middle",
                            Transform = "scale3d(1, 1, 1)",
                            Cursor = "grab",
                            Transition = @$"transform {motionDurationSlow} {motionEaseOut} 0s",
                            UserSelect = "none",
                            ["&-wrapper"] = new CSSObject()
                            {
                                ["..."] = GenBoxStyle(),
                                Transition = @$"transform {motionDurationSlow} {motionEaseOut} 0s",
                                Display = "flex",
                                JustifyContent = "center",
                                AlignItems = "center",
                                ["& > *"] = new CSSObject()
                                {
                                    PointerEvents = "auto",
                                },
                                ["&::before"] = new CSSObject()
                                {
                                    Display = "inline-block",
                                    Width = 1,
                                    Height = "50%",
                                    MarginInlineEnd = -1,
                                    Content = "\"\"",
                                },
                            },
                        },
                        [$"{previewCls}-moving"] = new CSSObject()
                        {
                            [$"{previewCls}-preview-img"] = new CSSObject()
                            {
                                Cursor = "grabbing",
                                ["&-wrapper"] = new CSSObject()
                                {
                                    TransitionDuration = "0s",
                                },
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-preview-root"] = new CSSObject()
                    {
                        [$"{previewCls}-wrap"] = new CSSObject()
                        {
                            ZIndex = token.ZIndexPopup,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-preview-operations-wrapper"] = new CSSObject()
                    {
                        Position = "fixed",
                        ZIndex = token.ZIndexPopup + 1,
                    },
                    ["&"] = new CSSInterpolation[]
                    {
                        GenPreviewOperationsStyle(token),
                        GenPreviewSwitchStyle(token),
                    }
                },
            };
        }

        public static CSSObject GenImageStyle(ImageToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Position = "relative",
                    Display = "inline-block",
                    [$"{componentCls}-img"] = new CSSObject()
                    {
                        Width = "100%",
                        Height = "auto",
                        VerticalAlign = "middle",
                    },
                    [$"{componentCls}-img-placeholder"] = new CSSObject()
                    {
                        BackgroundColor = token.ColorBgContainerDisabled,
                        BackgroundImage = "url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTYiIGhlaWdodD0iMTYiIHZpZXdCb3g9IjAgMCAxNiAxNiIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCBkPSJNMTQuNSAyLjVoLTEzQS41LjUgMCAwIDAgMSAzdjEwYS41LjUgMCAwIDAgLjUuNWgxM2EuNS41IDAgMCAwIC41LS41VjNhLjUuNSAwIDAgMC0uNS0uNXpNNS4yODEgNC43NWExIDEgMCAwIDEgMCAyIDEgMSAwIDAgMSAwLTJ6bTguMDMgNi44M2EuMTI3LjEyNyAwIDAgMS0uMDgxLjAzSDIuNzY5YS4xMjUuMTI1IDAgMCAxLS4wOTYtLjIwN2wyLjY2MS0zLjE1NmEuMTI2LjEyNiAwIDAgMSAuMTc3LS4wMTZsLjAxNi4wMTZMNy4wOCAxMC4wOWwyLjQ3LTIuOTNhLjEyNi4xMjYgMCAwIDEgLjE3Ny0uMDE2bC4wMTUuMDE2IDMuNTg4IDQuMjQ0YS4xMjcuMTI3IDAgMCAxLS4wMi4xNzV6IiBmaWxsPSIjOEM4QzhDIiBmaWxsLXJ1bGU9Im5vbnplcm8iLz48L3N2Zz4=')",
                        BackgroundRepeat = "no-repeat",
                        BackgroundPosition = "center center",
                        BackgroundSize = "30%",
                    },
                    [$"{componentCls}-mask"] = new CSSObject()
                    {
                        ["..."] = GenImageMaskStyle(token)
                    },
                    [$"{componentCls}-mask:hover"] = new CSSObject()
                    {
                        Opacity = 1,
                    },
                    [$"{componentCls}-placeholder"] = new CSSObject()
                    {
                        ["..."] = GenBoxStyle()
                    },
                },
            };
        }

        public static CSSObject GenPreviewMotion(ImageToken token)
        {
            var previewCls = token.PreviewCls;
            return new CSSObject()
            {
                [$"{previewCls}-root"] = InitZoomMotion(token, "zoom"),
                ["&"] = InitFadeMotion(token, true)
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Image",
                (token) =>
                {
                    var previewCls = @$"{token.ComponentCls}-preview";
                    var imageToken = MergeToken(
                        token,
                        new ImageToken()
                        {
                            PreviewCls = previewCls,
                            ModalMaskBg = new TinyColor("#000").SetAlpha(0.45).ToRgbString(),
                            ImagePreviewSwitchSize = token.ControlHeightLG,
                        });
                    return new CSSInterpolation[]
                    {
                        GenImageStyle(imageToken),
                        GenImagePreviewStyle(imageToken),
                        GenModalMaskStyle(
                            MergeToken(
                                imageToken,
                                new ModalToken()
                                {
                                    ComponentCls = previewCls,
                                })),
                        GenPreviewMotion(imageToken),
                    };
                },
                (token) =>
                {
                    return new ImageToken()
                    {
                        ZIndexPopup = token.ZIndexPopupBase + 80,
                        PreviewOperationColor = new TinyColor(token.ColorTextLightSolid).SetAlpha(0.65).ToRgbString(),
                        PreviewOperationHoverColor = new TinyColor(token.ColorTextLightSolid).SetAlpha(0.85).ToRgbString(),
                        PreviewOperationColorDisabled = new TinyColor(token.ColorTextLightSolid).SetAlpha(0.25).ToRgbString(),
                        PreviewOperationSize = token.FontSizeIcon * 1.5,
                    };
                });
        }
    }
}
