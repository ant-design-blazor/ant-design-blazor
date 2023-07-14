using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class ModalToken
    {
        public string HeaderBg { get; set; }

        public int TitleLineHeight { get; set; }

        public int TitleFontSize { get; set; }

        public string TitleColor { get; set; }

        public string ContentBg { get; set; }

        public string FooterBg { get; set; }

    }

    public partial class ModalToken : TokenWithCommonCls
    {
        public int ModalHeaderHeight { get; set; }

        public int ModalBodyPadding { get; set; }

        public string ModalHeaderPadding { get; set; }

        public int ModalHeaderBorderWidth { get; set; }

        public string ModalHeaderBorderStyle { get; set; }

        public string ModalHeaderBorderColorSplit { get; set; }

        public string ModalFooterBorderColorSplit { get; set; }

        public string ModalFooterBorderStyle { get; set; }

        public int ModalFooterPaddingVertical { get; set; }

        public int ModalFooterPaddingHorizontal { get; set; }

        public int ModalFooterBorderWidth { get; set; }

        public string ModalIconHoverColor { get; set; }

        public string ModalCloseIconColor { get; set; }

        public int ModalCloseBtnSize { get; set; }

        public int ModalConfirmIconSize { get; set; }

    }

    public partial class Modal
    {
        public React.CSSProperties Box(React.CSSProperties['position'] position)
        {
            return new React.CSSProperties()
            {
                Position = position,
                Top = 0,
                InsetInlineEnd = 0,
                Bottom = 0,
                InsetInlineStart = 0,
            };
        }

        public Unknown_1 GenModalMaskStyle(Unknown_8 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            return new Unknown_9
            {
                new Unknown_10()
                {
                    [$"{componentCls}-root"] = new Unknown_11()
                    {
                        [$"{componentCls}{antCls}-zoom-enter, {componentCls}{antCls}-zoom-appear"] = new Unknown_12()
                        {
                            Transform = "none",
                            Opacity = 0,
                            AnimationDuration = token.MotionDurationSlow,
                            UserSelect = "none",
                        },
                        [$"{componentCls}{antCls}-zoom-leave {componentCls}-content"] = new Unknown_13()
                        {
                            PointerEvents = "none",
                        },
                        [$"{componentCls}-mask"] = new Unknown_14()
                        {
                            ["..."] = Box("fixed"),
                            ZIndex = token.ZIndexPopupBase,
                            Height = "100%",
                            BackgroundColor = token.ColorBgMask,
                            [$"{componentCls}-hidden"] = new Unknown_15()
                            {
                                Display = "none",
                            },
                        },
                        [$"{componentCls}-wrap"] = new Unknown_16()
                        {
                            ["..."] = Box("fixed"),
                            Overflow = "auto",
                            Outline = 0,
                            WebkitOverflowScrolling = "touch",
                        },
                    },
                },
                new Unknown_17()
                {
                    [$"{componentCls}-root"] = InitFadeMotion(token)
                },
            };
        }

        public Unknown_2 GenModalStyle(Unknown_18 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_19
            {
                new Unknown_20()
                {
                    [$"{componentCls}-root"] = new Unknown_21()
                    {
                        [$"{componentCls}-wrap"] = new Unknown_22()
                        {
                            ZIndex = token.ZIndexPopupBase,
                            Position = "fixed",
                            Inset = 0,
                            Overflow = "auto",
                            Outline = 0,
                            WebkitOverflowScrolling = "touch",
                        },
                        [$"{componentCls}-wrap-rtl"] = new Unknown_23()
                        {
                            Direction = "rtl",
                        },
                        [$"{componentCls}-centered"] = new Unknown_24()
                        {
                            TextAlign = "center",
                            ["&::before"] = new Unknown_25()
                            {
                                Display = "inline-block",
                                Width = 0,
                                Height = "100%",
                                VerticalAlign = "middle",
                                Content = "\"\"",
                            },
                            [componentCls] = new Unknown_26()
                            {
                                Top = 0,
                                Display = "inline-block",
                                PaddingBottom = 0,
                                TextAlign = "start",
                                VerticalAlign = "middle",
                            },
                        },
                        [$"@media (max-width: {token.ScreenSMMax})"] = new Unknown_27()
                        {
                            [componentCls] = new Unknown_28()
                            {
                                MaxWidth = "calc(100vw - 16px)",
                                Margin = @$"{token.MarginXS} auto",
                            },
                            [$"{componentCls}-centered"] = new Unknown_29()
                            {
                                [componentCls] = new Unknown_30()
                                {
                                    Flex = 1,
                                },
                            },
                        },
                    },
                },
                new Unknown_31()
                {
                    [componentCls] = new Unknown_32()
                    {
                        ["..."] = ResetComponent(token),
                        PointerEvents = "none",
                        Position = "relative",
                        Top = 100,
                        Width = "auto",
                        MaxWidth = @$"calc(100vw - {token.Margin * 2}px)",
                        Margin = "0 auto",
                        PaddingBottom = token.PaddingLG,
                        [$"{componentCls}-title"] = new Unknown_33()
                        {
                            Margin = 0,
                            Color = token.TitleColor,
                            FontWeight = token.FontWeightStrong,
                            FontSize = token.TitleFontSize,
                            LineHeight = token.TitleLineHeight,
                            WordWrap = "break-word",
                        },
                        [$"{componentCls}-content"] = new Unknown_34()
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
                        [$"{componentCls}-close"] = new Unknown_35()
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
                            ["&-x"] = new Unknown_36()
                            {
                                Display = "flex",
                                FontSize = token.FontSizeLG,
                                FontStyle = "normal",
                                LineHeight = @$"{token.ModalCloseBtnSize}px",
                                JustifyContent = "center",
                                TextTransform = "none",
                                TextRendering = "auto",
                            },
                            ["&:hover"] = new Unknown_37()
                            {
                                Color = token.ModalIconHoverColor,
                                BackgroundColor = token.Wireframe ? "transparent" : token.ColorFillContent,
                                TextDecoration = "none",
                            },
                            ["&:active"] = new Unknown_38()
                            {
                                BackgroundColor = token.Wireframe ? "transparent" : token.ColorFillContentHover,
                            },
                            ["..."] = GenFocusStyle(token)
                        },
                        [$"{componentCls}-header"] = new Unknown_39()
                        {
                            Color = token.ColorText,
                            Background = token.HeaderBg,
                            BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
                            MarginBottom = token.MarginXS,
                        },
                        [$"{componentCls}-body"] = new Unknown_40()
                        {
                            FontSize = token.FontSize,
                            LineHeight = token.LineHeight,
                            WordWrap = "break-word",
                        },
                        [$"{componentCls}-footer"] = new Unknown_41()
                        {
                            TextAlign = "end",
                            Background = token.FooterBg,
                            MarginTop = token.MarginSM,
                            [$"{token.AntCls}-btn + {token.AntCls}-btn:not({token.AntCls}-dropdown-trigger)"] = new Unknown_42()
                            {
                                MarginBottom = 0,
                                MarginInlineStart = token.MarginXS,
                            },
                        },
                        [$"{componentCls}-open"] = new Unknown_43()
                        {
                            Overflow = "hidden",
                        },
                    },
                },
                new Unknown_44()
                {
                    [$"{componentCls}-pure-panel"] = new Unknown_45()
                    {
                        Top = "auto",
                        Padding = 0,
                        Display = "flex",
                        FlexDirection = "column",
                        [$"{componentCls}-content,{componentCls}-body,{componentCls}-confirm-body-wrapper"] = new Unknown_46()
                        {
                            Display = "flex",
                            FlexDirection = "column",
                            Flex = "auto",
                        },
                        [$"{componentCls}-confirm-body"] = new Unknown_47()
                        {
                            MarginBottom = "auto",
                        },
                    },
                },
            };
        }

        public Unknown_3 GenModalConfirmStyle(Unknown_48 token)
        {
            var componentCls = token.ComponentCls;
            var confirmComponentCls = @$"{componentCls}-confirm";
            return new Unknown_49()
            {
                [confirmComponentCls] = new Unknown_50()
                {
                    ["&-rtl"] = new Unknown_51()
                    {
                        Direction = "rtl",
                    },
                    [$"{token.AntCls}-modal-header"] = new Unknown_52()
                    {
                        Display = "none",
                    },
                    [$"{confirmComponentCls}-body-wrapper"] = new Unknown_53()
                    {
                        ["..."] = ClearFix()
                    },
                    [$"{confirmComponentCls}-body"] = new Unknown_54()
                    {
                        Display = "flex",
                        FlexWrap = "wrap",
                        AlignItems = "center",
                        [$"{confirmComponentCls}-title"] = new Unknown_55()
                        {
                            Flex = "0 0 100%",
                            Display = "block",
                            Overflow = "hidden",
                            Color = token.ColorTextHeading,
                            FontWeight = token.FontWeightStrong,
                            FontSize = token.TitleFontSize,
                            LineHeight = token.TitleLineHeight,
                            [$"+ {confirmComponentCls}-content"] = new Unknown_56()
                            {
                                MarginBlockStart = token.MarginXS,
                                FlexBasis = "100%",
                                MaxWidth = @$"calc(100% - {token.ModalConfirmIconSize + token.MarginSM}px)",
                            },
                        },
                        [$"{confirmComponentCls}-content"] = new Unknown_57()
                        {
                            Color = token.ColorText,
                            FontSize = token.FontSize,
                        },
                        [$"> {token.IconCls}"] = new Unknown_58()
                        {
                            Flex = "none",
                            MarginInlineEnd = token.MarginSM,
                            FontSize = token.ModalConfirmIconSize,
                            [$"+ {confirmComponentCls}-title"] = new Unknown_59()
                            {
                                Flex = 1,
                            },
                            [$"+ {confirmComponentCls}-title + {confirmComponentCls}-content"] = new Unknown_60()
                            {
                                MarginInlineStart = token.ModalConfirmIconSize + token.MarginSM,
                            },
                        },
                    },
                    [$"{confirmComponentCls}-btns"] = new Unknown_61()
                    {
                        TextAlign = "end",
                        MarginTop = token.MarginSM,
                        [$"{token.AntCls}-btn + {token.AntCls}-btn"] = new Unknown_62()
                        {
                            MarginBottom = 0,
                            MarginInlineStart = token.MarginXS,
                        },
                    },
                },
                [$"{confirmComponentCls}-error {confirmComponentCls}-body > {token.IconCls}"] = new Unknown_63()
                {
                    Color = token.ColorError,
                },
                [$"{confirmComponentCls}-warning{confirmComponentCls}-body>{token.IconCls},{confirmComponentCls}-confirm{confirmComponentCls}-body>{token.IconCls}"] = new Unknown_64()
                {
                    Color = token.ColorWarning,
                },
                [$"{confirmComponentCls}-info {confirmComponentCls}-body > {token.IconCls}"] = new Unknown_65()
                {
                    Color = token.ColorInfo,
                },
                [$"{confirmComponentCls}-success {confirmComponentCls}-body > {token.IconCls}"] = new Unknown_66()
                {
                    Color = token.ColorSuccess,
                },
            };
        }

        public Unknown_4 GenRTLStyle(Unknown_67 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_68()
            {
                [$"{componentCls}-root"] = new Unknown_69()
                {
                    [$"{componentCls}-wrap-rtl"] = new Unknown_70()
                    {
                        Direction = "rtl",
                        [$"{componentCls}-confirm-body"] = new Unknown_71()
                        {
                            Direction = "rtl",
                        },
                    },
                },
            };
        }

        public Unknown_5 GenWireframeStyle(Unknown_72 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var confirmComponentCls = @$"{componentCls}-confirm";
            return new Unknown_73()
            {
                [componentCls] = new Unknown_74()
                {
                    [$"{componentCls}-content"] = new Unknown_75()
                    {
                        Padding = 0,
                    },
                    [$"{componentCls}-header"] = new Unknown_76()
                    {
                        Padding = token.ModalHeaderPadding,
                        BorderBottom = @$"{token.ModalHeaderBorderWidth}px {token.ModalHeaderBorderStyle} {token.ModalHeaderBorderColorSplit}",
                        MarginBottom = 0,
                    },
                    [$"{componentCls}-body"] = new Unknown_77()
                    {
                        Padding = token.ModalBodyPadding,
                    },
                    [$"{componentCls}-footer"] = new Unknown_78()
                    {
                        Padding = @$"{token.ModalFooterPaddingVertical}px {token.ModalFooterPaddingHorizontal}px",
                        BorderTop = @$"{token.ModalFooterBorderWidth}px {token.ModalFooterBorderStyle} {token.ModalFooterBorderColorSplit}",
                        BorderRadius = @$"0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px",
                        MarginTop = 0,
                    },
                },
                [confirmComponentCls] = new Unknown_79()
                {
                    [$"{antCls}-modal-body"] = new Unknown_80()
                    {
                        Padding = @$"{token.Padding * 2}px {token.Padding * 2}px {token.PaddingLG}px",
                    },
                    [$"{confirmComponentCls}-body"] = new Unknown_81()
                    {
                        [$"> {token.IconCls}"] = new Unknown_82()
                        {
                            MarginInlineEnd = token.Margin,
                            [$"+ {confirmComponentCls}-title + {confirmComponentCls}-content"] = new Unknown_83()
                            {
                                MarginInlineStart = token.ModalConfirmIconSize + token.Margin,
                            },
                        },
                    },
                    [$"{confirmComponentCls}-btns"] = new Unknown_84()
                    {
                        MarginTop = token.MarginLG,
                    },
                },
            };
        }

        public Unknown_6 GenComponentStyleHook(Unknown_85 token)
        {
            var headerPaddingVertical = token.Padding;
            var headerFontSize = token.FontSizeHeading5;
            var headerLineHeight = token.LineHeightHeading5;
            var modalToken = MergeToken(
                token,
                new Unknown_86()
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
            return new Unknown_87
            {
                GenModalStyle(modalToken),
                GenModalConfirmStyle(modalToken),
                GenRTLStyle(modalToken),
                GenModalMaskStyle(modalToken),
                Token.Wireframe && genWireframeStyle(modalToken),
                InitZoomMotion(modalToken, "zoom")
            };
        }

        public Unknown_7 GenComponentStyleHook(Unknown_88 token)
        {
            return new Unknown_89()
            {
                FooterBg = "transparent",
                HeaderBg = token.ColorBgElevated,
                TitleLineHeight = token.LineHeightHeading5,
                TitleFontSize = token.FontSizeHeading5,
                ContentBg = token.ColorBgElevated,
                TitleColor = token.ColorTextHeading,
            };
        }

    }

}