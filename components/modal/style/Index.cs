using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Fade;
using static AntDesign.Zoom;

namespace AntDesign
{
    public partial class ModalToken
    {
        public string HeaderBg
        {
            get => (string)_tokens["headerBg"];
            set => _tokens["headerBg"] = value;
        }

        public double TitleLineHeight
        {
            get => (double)_tokens["titleLineHeight"];
            set => _tokens["titleLineHeight"] = value;
        }

        public double TitleFontSize
        {
            get => (double)_tokens["titleFontSize"];
            set => _tokens["titleFontSize"] = value;
        }

        public string TitleColor
        {
            get => (string)_tokens["titleColor"];
            set => _tokens["titleColor"] = value;
        }

        public string ContentBg
        {
            get => (string)_tokens["contentBg"];
            set => _tokens["contentBg"] = value;
        }

        public string FooterBg
        {
            get => (string)_tokens["footerBg"];
            set => _tokens["footerBg"] = value;
        }

    }

    public partial class ModalToken : TokenWithCommonCls
    {
        public double ModalHeaderHeight
        {
            get => (double)_tokens["modalHeaderHeight"];
            set => _tokens["modalHeaderHeight"] = value;
        }

        public double ModalBodyPadding
        {
            get => (double)_tokens["modalBodyPadding"];
            set => _tokens["modalBodyPadding"] = value;
        }

        public string ModalHeaderPadding
        {
            get => (string)_tokens["modalHeaderPadding"];
            set => _tokens["modalHeaderPadding"] = value;
        }

        public double ModalHeaderBorderWidth
        {
            get => (double)_tokens["modalHeaderBorderWidth"];
            set => _tokens["modalHeaderBorderWidth"] = value;
        }

        public string ModalHeaderBorderStyle
        {
            get => (string)_tokens["modalHeaderBorderStyle"];
            set => _tokens["modalHeaderBorderStyle"] = value;
        }

        public string ModalHeaderBorderColorSplit
        {
            get => (string)_tokens["modalHeaderBorderColorSplit"];
            set => _tokens["modalHeaderBorderColorSplit"] = value;
        }

        public string ModalFooterBorderColorSplit
        {
            get => (string)_tokens["modalFooterBorderColorSplit"];
            set => _tokens["modalFooterBorderColorSplit"] = value;
        }

        public string ModalFooterBorderStyle
        {
            get => (string)_tokens["modalFooterBorderStyle"];
            set => _tokens["modalFooterBorderStyle"] = value;
        }

        public double ModalFooterPaddingVertical
        {
            get => (double)_tokens["modalFooterPaddingVertical"];
            set => _tokens["modalFooterPaddingVertical"] = value;
        }

        public double ModalFooterPaddingHorizontal
        {
            get => (double)_tokens["modalFooterPaddingHorizontal"];
            set => _tokens["modalFooterPaddingHorizontal"] = value;
        }

        public double ModalFooterBorderWidth
        {
            get => (double)_tokens["modalFooterBorderWidth"];
            set => _tokens["modalFooterBorderWidth"] = value;
        }

        public string ModalIconHoverColor
        {
            get => (string)_tokens["modalIconHoverColor"];
            set => _tokens["modalIconHoverColor"] = value;
        }

        public string ModalCloseIconColor
        {
            get => (string)_tokens["modalCloseIconColor"];
            set => _tokens["modalCloseIconColor"] = value;
        }

        public double ModalCloseBtnSize
        {
            get => (double)_tokens["modalCloseBtnSize"];
            set => _tokens["modalCloseBtnSize"] = value;
        }

        public double ModalConfirmIconSize
        {
            get => (double)_tokens["modalConfirmIconSize"];
            set => _tokens["modalConfirmIconSize"] = value;
        }

    }

    public partial class ModalStyle
    {
        public static CSSObject Box(string position)
        {
            return new CSSObject()
            {
                Position = position,
                Inset = 0,
            };
        }

        public static CSSInterpolation GenModalMaskStyle(ModalToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-root"] = new CSSObject()
                    {
                        [$"{componentCls}{antCls}-zoom-enter, {componentCls}{antCls}-zoom-appear"] = new CSSObject()
                        {
                            Transform = "none",
                            Opacity = 0,
                            AnimationDuration = token.MotionDurationSlow,
                            UserSelect = "none",
                        },
                        [$"{componentCls}{antCls}-zoom-leave {componentCls}-content"] = new CSSObject()
                        {
                            PointerEvents = "none",
                        },
                        [$"{componentCls}-mask"] = new CSSObject()
                        {
                            ["..."] = Box("fixed"),
                            ZIndex = token.ZIndexPopupBase,
                            Height = "100%",
                            BackgroundColor = token.ColorBgMask,
                            PointerEvents = "none",
                            [$"{componentCls}-hidden"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        [$"{componentCls}-wrap"] = new CSSObject()
                        {
                            ["..."] = Box("fixed"),
                            ZIndex = token.ZIndexPopupBase,
                            Overflow = "auto",
                            Outline = 0,
                            WebkitOverflowScrolling = "touch",
                            [$"&:has({componentCls}{antCls}-zoom-enter), &:has({componentCls}{antCls}-zoom-appear)"] = new CSSObject()
                            {
                                PointerEvents = "none",
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-root"] = InitFadeMotion(token)
                },
            };
        }

        public static CSSInterpolation GenModalStyle(ModalToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-root"] = new CSSObject()
                    {
                        [$"{componentCls}-wrap-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                        },
                        [$"{componentCls}-centered"] = new CSSObject()
                        {
                            TextAlign = "center",
                            ["&::before"] = new CSSObject()
                            {
                                Display = "inline-block",
                                Width = 0,
                                Height = "100%",
                                VerticalAlign = "middle",
                                Content = "\"\"",
                            },
                            [componentCls] = new CSSObject()
                            {
                                Top = 0,
                                Display = "inline-block",
                                PaddingBottom = 0,
                                TextAlign = "start",
                                VerticalAlign = "middle",
                            },
                        },
                        [$"@media (max-width: {token.ScreenSMMax})"] = new CSSObject()
                        {
                            [componentCls] = new CSSObject()
                            {
                                MaxWidth = "calc(100vw - 16px)",
                                Margin = @$"{token.MarginXS} auto",
                            },
                            [$"{componentCls}-centered"] = new CSSObject()
                            {
                                [componentCls] = new CSSObject()
                                {
                                    Flex = 1,
                                },
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        PointerEvents = "none",
                        Position = "relative",
                        Top = 100,
                        Width = "auto",
                        MaxWidth = @$"calc(100vw - {token.Margin * 2}px)",
                        Margin = "0 auto",
                        PaddingBottom = token.PaddingLG,
                        [$"{componentCls}-title"] = new CSSObject()
                        {
                            Margin = 0,
                            Color = token.TitleColor,
                            FontWeight = token.FontWeightStrong,
                            FontSize = token.TitleFontSize,
                            LineHeight = token.TitleLineHeight,
                            WordWrap = "break-word",
                        },
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Position = "relative",
                            BackgroundColor = token.ContentBg,
                            BackgroundClip = "padding-box",
                            Border = 0,
                            BorderRadius = token.BorderRadiusLG,
                            BoxShadow = token.BoxShadow,
                            PointerEvents = "auto",
                            Padding = @$"{token.PaddingMD}px {token.PaddingContentHorizontalLG}px",
                        },
                        [$"{componentCls}-close"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = (token.ModalHeaderHeight - token.ModalCloseBtnSize) / 2,
                            InsetInlineEnd = (token.ModalHeaderHeight - token.ModalCloseBtnSize) / 2,
                            ZIndex = token.ZIndexPopupBase + 10,
                            Padding = 0,
                            Color = token.ModalCloseIconColor,
                            FontWeight = token.FontWeightStrong,
                            LineHeight = 1,
                            TextDecoration = "none",
                            Background = "transparent",
                            BorderRadius = token.BorderRadiusSM,
                            Width = token.ModalCloseBtnSize,
                            Height = token.ModalCloseBtnSize,
                            Border = 0,
                            Outline = 0,
                            Cursor = "pointer",
                            Transition = @$"color {token.MotionDurationMid}, background-color {token.MotionDurationMid}",
                            ["&-x"] = new CSSObject()
                            {
                                Display = "flex",
                                FontSize = token.FontSizeLG,
                                FontStyle = "normal",
                                LineHeight = @$"{token.ModalCloseBtnSize}px",
                                JustifyContent = "center",
                                TextTransform = "none",
                                TextRendering = "auto",
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                Color = token.ModalIconHoverColor,
                                BackgroundColor = token.Wireframe ? "transparent" : token.ColorFillContent,
                                TextDecoration = "none",
                            },
                            ["&:active"] = new CSSObject()
                            {
                                BackgroundColor = token.Wireframe ? "transparent" : token.ColorFillContentHover
                            },
                            ["..."] = GenFocusStyle(token)
                        },
                        [$"{componentCls}-header"] = new CSSObject()
                        {
                            Color = token.ColorText,
                            Background = token.HeaderBg,
                            BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
                            MarginBottom = token.MarginXS,
                        },
                        [$"{componentCls}-body"] = new CSSObject()
                        {
                            FontSize = token.FontSize,
                            LineHeight = token.LineHeight,
                            WordWrap = "break-word",
                        },
                        [$"{componentCls}-footer"] = new CSSObject()
                        {
                            TextAlign = "end",
                            Background = token.FooterBg,
                            MarginTop = token.MarginSM,
                            [$"{token.AntCls}-btn + {token.AntCls}-btn:not({token.AntCls}-dropdown-trigger)"] = new CSSObject()
                            {
                                MarginBottom = 0,
                                MarginInlineStart = token.MarginXS,
                            },
                        },
                        [$"{componentCls}-open"] = new CSSObject()
                        {
                            Overflow = "hidden",
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-pure-panel"] = new CSSObject()
                    {
                        Top = "auto",
                        Padding = 0,
                        Display = "flex",
                        FlexDirection = "column",
                        [$"{componentCls}-content,{componentCls}-body,{componentCls}-confirm-body-wrapper"] = new CSSObject()
                        {
                            Display = "flex",
                            FlexDirection = "column",
                            Flex = "auto",
                        },
                        [$"{componentCls}-confirm-body"] = new CSSObject()
                        {
                            MarginBottom = "auto",
                        },
                    },
                },
            };
        }

        public static CSSObject GenWireframeStyle(ModalToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var confirmComponentCls = @$"{componentCls}-confirm";
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Padding = 0,
                    },
                    [$"{componentCls}-header"] = new CSSObject()
                    {
                        Padding = token.ModalHeaderPadding,
                        BorderBottom = @$"{token.ModalHeaderBorderWidth}px {token.ModalHeaderBorderStyle} {token.ModalHeaderBorderColorSplit}",
                        MarginBottom = 0,
                    },
                    [$"{componentCls}-body"] = new CSSObject()
                    {
                        Padding = token.ModalBodyPadding,
                    },
                    [$"{componentCls}-footer"] = new CSSObject()
                    {
                        Padding = @$"{token.ModalFooterPaddingVertical}px {token.ModalFooterPaddingHorizontal}px",
                        BorderTop = @$"{token.ModalFooterBorderWidth}px {token.ModalFooterBorderStyle} {token.ModalFooterBorderColorSplit}",
                        BorderRadius = @$"0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px",
                        MarginTop = 0,
                    },
                },
                [confirmComponentCls] = new CSSObject()
                {
                    [$"{antCls}-modal-body"] = new CSSObject()
                    {
                        Padding = @$"{token.Padding * 2}px {token.Padding * 2}px {token.PaddingLG}px",
                    },
                    [$"{confirmComponentCls}-body > {token.IconCls}"] = new CSSObject()
                    {
                        MarginInlineEnd = token.Margin,
                    },
                    [$"{confirmComponentCls}-btns"] = new CSSObject()
                    {
                        MarginTop = token.MarginLG,
                    },
                },
            };
        }

        public static CSSObject GenRTLStyle(ModalToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-root"] = new CSSObject()
                {
                    [$"{componentCls}-wrap-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                        [$"{componentCls}-confirm-body"] = new CSSObject()
                        {
                            Direction = "rtl",
                        },
                    },
                },
            };
        }

        public static ModalToken PrepareToken(TokenWithCommonCls token)
        {
            var headerPaddingVertical = token.Padding;
            var headerFontSize = token.FontSizeHeading5;
            var headerLineHeight = token.LineHeightHeading5;
            var modalToken = MergeToken(
                token,
                new ModalToken()
                {
                    ModalBodyPadding = token.PaddingLG,
                    ModalHeaderPadding = @$"{headerPaddingVertical}px {token.PaddingLG}px",
                    ModalHeaderBorderWidth = token.LineWidth,
                    ModalHeaderBorderStyle = token.LineType,
                    ModalHeaderBorderColorSplit = token.ColorSplit,
                    ModalHeaderHeight = headerLineHeight * headerFontSize + headerPaddingVertical * 2,
                    ModalFooterBorderColorSplit = token.ColorSplit,
                    ModalFooterBorderStyle = token.LineType,
                    ModalFooterPaddingVertical = token.PaddingXS,
                    ModalFooterPaddingHorizontal = token.Padding,
                    ModalFooterBorderWidth = token.LineWidth,
                    ModalIconHoverColor = token.ColorIconHover,
                    ModalCloseIconColor = token.ColorIcon,
                    ModalCloseBtnSize = token.FontSize * token.LineHeight,
                    ModalConfirmIconSize = token.FontSize * token.LineHeight,
                });
            return modalToken;
        }

        public static ModalToken PrepareComponentToken(GlobalToken token)
        {
            return new ModalToken()
            {
                FooterBg = "transparent",
                HeaderBg = token.ColorBgElevated,
                TitleLineHeight = token.LineHeightHeading5,
                TitleFontSize = token.FontSizeHeading5,
                ContentBg = token.ColorBgElevated,
                TitleColor = token.ColorTextHeading,
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Modal",
                (token) =>
                {
                    var modalToken = PrepareToken(token);
                    return new CSSInterpolation[]
                    {
                        GenModalStyle(modalToken),
                        GenRTLStyle(modalToken),
                        GenModalMaskStyle(modalToken),
                        token.Wireframe ? GenWireframeStyle(modalToken) : null,
                        InitZoomMotion(modalToken, "zoom"),
                    };
                },
                PrepareComponentToken);
        }

    }
}
