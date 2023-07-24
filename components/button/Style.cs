using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public class ComponentToken
    {
    }

    public partial class ButtonToken : TokenWithCommonCls
    {
        public string ColorOutlineDefault { get; set; }

        public int ButtonPaddingHorizontal { get; set; }

        public int ButtonIconOnlyFontSize { get; set; }

        public int ButtonFontWeight { get; set; }

    }

    public partial class Button
    {
        public CSSObject GenSharedButtonStyle(ButtonToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var buttonFontWeight = token.ButtonFontWeight;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Outline = "none",
                    Position = "relative",
                    Display = "inline-block",
                    FontWeight = buttonFontWeight,
                    WhiteSpace = "nowrap",
                    TextAlign = "center",
                    BackgroundImage = "none",
                    BackgroundColor = "transparent",
                    Border = @$"{token.LineWidth}px {token.LineType} transparent",
                    Cursor = "pointer",
                    Transition = @$"all {token.MotionDurationMid} {token.MotionEaseInOut}",
                    UserSelect = "none",
                    TouchAction = "manipulation",
                    LineHeight = token.LineHeight,
                    Color = token.ColorText,
                    ["> span"] = new CSSObject()
                    {
                        Display = "inline-block",
                    },
                    [$"{componentCls}-icon"] = new CSSObject()
                    {
                        LineHeight = 0,
                    },
                    [$"&:not({componentCls}-icon-only) > {componentCls}-icon"] = new CSSObject()
                    {
                        [$"&{componentCls}-loading-icon, &:not(:last-child)"] = new CSSObject()
                        {
                            MarginInlineEnd = token.MarginXS,
                        },
                    },
                    [$"> span + {iconCls}"] = new CSSObject()
                    {
                        MarginInlineStart = token.MarginXS,
                    },
                    ["> a"] = new CSSObject()
                    {
                        Color = "currentColor",
                    },
                    ["&:not(:disabled)"] = new CSSObject()
                    {
                        ["..."] = GenFocusStyle(token)
                    },
                    [$"&-icon-only{componentCls}-compact-item"] = new CSSObject()
                    {
                        Flex = "none",
                    },
                    [$"&-compact-item{componentCls}-primary"] = new CSSObject()
                    {
                        [$"&:not([disabled]) + {componentCls}-compact-item{componentCls}-primary:not([disabled])"] = new CSSObject()
                        {
                            Position = "relative",
                            ["&:before"] = new CSSObject()
                            {
                                Position = "absolute",
                                Top = -token.LineWidth,
                                InsetInlineStart = -token.LineWidth,
                                Display = "inline-block",
                                Width = token.LineWidth,
                                Height = @$"calc(100% + {token.LineWidth * 2}px)",
                                BackgroundColor = token.ColorPrimaryHover,
                                Content = "\"\"",
                            },
                        },
                    },
                    ["&-compact-vertical-item"] = new CSSObject()
                    {
                        [$"&{componentCls}-primary"] = new CSSObject()
                        {
                            [$"&:not([disabled]) + {componentCls}-compact-vertical-item{componentCls}-primary:not([disabled])"] = new CSSObject()
                            {
                                Position = "relative",
                                ["&:before"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Top = -token.LineWidth,
                                    InsetInlineStart = -token.LineWidth,
                                    Display = "inline-block",
                                    Width = @$"calc(100% + {token.LineWidth * 2}px)",
                                    Height = token.LineWidth,
                                    BackgroundColor = token.ColorPrimaryHover,
                                    Content = "\"\"",
                                },
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenHoverActiveButtonStyle(CSSObject hoverStyle = default, CSSObject activeStyle = default)
        {
            return new CSSObject()
            {
                ["&:not(:disabled)"] = new CSSObject()
                {
                    ["&:hover"] = hoverStyle,
                    ["&:active"] = activeStyle,
                },
            };
        }

        public CSSObject GenCircleButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                MinWidth = token.ControlHeight,
                PaddingInlineStart = 0,
                PaddingInlineEnd = 0,
                BorderRadius = "50%",
            };
        }

        public CSSObject GenRoundButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                BorderRadius = token.ControlHeight,
                PaddingInlineStart = token.ControlHeight / 2,
                PaddingInlineEnd = token.ControlHeight / 2,
            };
        }

        public CSSObject GenDisabledStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                Cursor = "not-allowed",
                BorderColor = token.ColorBorder,
                Color = token.ColorTextDisabled,
                BackgroundColor = token.ColorBgContainerDisabled,
                BoxShadow = "none",
            };
        }

        public CSSObject GenGhostButtonStyle(string btnCls, string textColor, string borderColor, string textColorDisabled, string borderColorDisabled, CSSObject hoverStyle = default, CSSObject activeStyle = default)
        {
            return new CSSObject()
            {
                [$"&{btnCls}-background-ghost"] = new CSSObject()
                {
                    Color = textColor,
                    BackgroundColor = "transparent",
                    BorderColor = borderColor,
                    BoxShadow = "none",
                    ["..."] = GenHoverActiveButtonStyle(
                        new CSSObject()
                        {
                            BackgroundColor = "transparent",
                            ["..."] = hoverStyle,
                        },
                        new CSSObject()
                        {
                            BackgroundColor = "transparent",
                            ["..."] = activeStyle,
                        }),
                    ["&:disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        Color = textColorDisabled,
                        BorderColor = borderColorDisabled,
                    },
                },
            };
        }

        public CSSObject GenSolidDisabledButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["&:disabled"] = new CSSObject()
                {
                    ["..."] = GenDisabledStyle(token)
                },
            };
        }

        public CSSObject GenSolidButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenSolidDisabledButtonStyle(token)
            };
        }

        public CSSObject GenPureDisabledButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["&:disabled"] = new CSSObject()
                {
                    Cursor = "not-allowed",
                    Color = token.ColorTextDisabled,
                },
            };
        }

        public CSSObject GenDefaultButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenSolidButtonStyle(token),
                BackgroundColor = token.ColorBgContainer,
                BorderColor = token.ColorBorder,
                BoxShadow = @$"0 {token.ControlOutlineWidth}px 0 {token.ControlTmpOutline}",
                ["..."] = GenHoverActiveButtonStyle(
                    new CSSObject()
                    {
                        Color = token.ColorPrimaryHover,
                        BorderColor = token.ColorPrimaryHover,
                    },
                    new CSSObject()
                    {
                        Color = token.ColorPrimaryActive,
                        BorderColor = token.ColorPrimaryActive,
                    }),
                ["..."] = GenGhostButtonStyle(token.ComponentCls, token.ColorBgContainer, token.ColorBgContainer, token.ColorTextDisabled, token.ColorBorder),
                [$"&{token.ComponentCls}-dangerous"] = new CSSObject()
                {
                    Color = token.ColorError,
                    BorderColor = token.ColorError,
                    ["..."] = GenHoverActiveButtonStyle(
                        new CSSObject()
                        {
                            Color = token.ColorErrorHover,
                            BorderColor = token.ColorErrorBorderHover,
                        },
                        new CSSObject()
                        {
                            Color = token.ColorErrorActive,
                            BorderColor = token.ColorErrorActive,
                        }),
                    ["..."] = GenGhostButtonStyle(token.ComponentCls, token.ColorError, token.ColorError, token.ColorTextDisabled, token.ColorBorder),
                    ["..."] = GenSolidDisabledButtonStyle(token)
                },
            };
        }

        public CSSObject GenPrimaryButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenSolidButtonStyle(token),
                Color = token.ColorTextLightSolid,
                BackgroundColor = token.ColorPrimary,
                BoxShadow = @$"0 {token.ControlOutlineWidth}px 0 {token.ControlOutline}",
                ["..."] = GenHoverActiveButtonStyle(
                    new CSSObject()
                    {
                        Color = token.ColorTextLightSolid,
                        BackgroundColor = token.ColorPrimaryHover,
                    },
                    new CSSObject()
                    {
                        Color = token.ColorTextLightSolid,
                        BackgroundColor = token.ColorPrimaryActive,
                    }),
                ["..."] = GenGhostButtonStyle(
                    token.ComponentCls,
                    token.ColorPrimary,
                    token.ColorPrimary,
                    token.ColorTextDisabled,
                    token.ColorBorder,
                    new CSSObject()
                    {
                        Color = token.ColorPrimaryHover,
                        BorderColor = token.ColorPrimaryHover,
                    },
                    new CSSObject()
                    {
                        Color = token.ColorPrimaryActive,
                        BorderColor = token.ColorPrimaryActive,
                    }),
                [$"&{token.ComponentCls}-dangerous"] = new CSSObject()
                {
                    BackgroundColor = token.ColorError,
                    BoxShadow = @$"0 {token.ControlOutlineWidth}px 0 {token.ColorErrorOutline}",
                    ["..."] = GenHoverActiveButtonStyle(
                        new CSSObject()
                        {
                            BackgroundColor = token.ColorErrorHover,
                        },
                        new CSSObject()
                        {
                            BackgroundColor = token.ColorErrorActive,
                        }),
                    ["..."] = GenGhostButtonStyle(
                        token.ComponentCls,
                        token.ColorError,
                        token.ColorError,
                        token.ColorTextDisabled,
                        token.ColorBorder,
                        new CSSObject()
                        {
                            Color = token.ColorErrorHover,
                            BorderColor = token.ColorErrorHover,
                        },
                        new CSSObject()
                        {
                            Color = token.ColorErrorActive,
                            BorderColor = token.ColorErrorActive,
                        }),
                    ["..."] = GenSolidDisabledButtonStyle(token)
                },
            };
        }

        public CSSObject GenDashedButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenDefaultButtonStyle(token),
                BorderStyle = "dashed",
            };
        }

        public CSSObject GenLinkButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                Color = token.ColorLink,
                ["..."] = GenHoverActiveButtonStyle(
                    new CSSObject()
                    {
                        Color = token.ColorLinkHover,
                    },
                    new CSSObject()
                    {
                        Color = token.ColorLinkActive,
                    }),
                ["..."] = GenPureDisabledButtonStyle(token),
                [$"&{token.ComponentCls}-dangerous"] = new CSSObject()
                {
                    Color = token.ColorError,
                    ["..."] = GenHoverActiveButtonStyle(
                        new CSSObject()
                        {
                            Color = token.ColorErrorHover,
                        },
                        new CSSObject()
                        {
                            Color = token.ColorErrorActive,
                        }),
                    ["..."] = GenPureDisabledButtonStyle(token)
                },
            };
        }

        public CSSObject GenTextButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenHoverActiveButtonStyle(
                    new CSSObject()
                    {
                        Color = token.ColorText,
                        BackgroundColor = token.ColorBgTextHover,
                    },
                    new CSSObject()
                    {
                        Color = token.ColorText,
                        BackgroundColor = token.ColorBgTextActive,
                    }),
                ["..."] = GenPureDisabledButtonStyle(token),
                [$"&{token.ComponentCls}-dangerous"] = new CSSObject()
                {
                    Color = token.ColorError,
                    ["..."] = GenPureDisabledButtonStyle(token),
                    ["..."] = GenHoverActiveButtonStyle(
                        new CSSObject()
                        {
                            Color = token.ColorErrorHover,
                            BackgroundColor = token.ColorErrorBg,
                        },
                        new CSSObject()
                        {
                            Color = token.ColorErrorHover,
                            BackgroundColor = token.ColorErrorBg,
                        })
                },
            };
        }

        public CSSObject GenDisabledButtonStyle(ButtonToken token)
        {
            return new CSSObject()
            {
                ["..."] = GenDisabledStyle(token),
                [$"&{token.ComponentCls}:hover"] = new CSSObject()
                {
                    ["..."] = GenDisabledStyle(token)
                },
            };
        }

        public CSSObject GenTypeButtonStyle(ButtonToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-default"] = GenDefaultButtonStyle(token),
                [$"{componentCls}-primary"] = GenPrimaryButtonStyle(token),
                [$"{componentCls}-dashed"] = GenDashedButtonStyle(token),
                [$"{componentCls}-link"] = GenLinkButtonStyle(token),
                [$"{componentCls}-text"] = GenTextButtonStyle(token),
                [$"{componentCls}-disabled"] = GenDisabledButtonStyle(token)
            };
        }

        public CSSInterpolation[] GenSizeButtonStyle(ButtonToken token, string sizePrefixCls = "")
        {
            var componentCls = token.ComponentCls;
            var controlHeight = token.ControlHeight;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var lineWidth = token.LineWidth;
            var borderRadius = token.BorderRadius;
            var buttonPaddingHorizontal = token.ButtonPaddingHorizontal;
            var iconCls = token.IconCls;
            var paddingVertical = Math.Max(0, (controlHeight - fontSize * lineHeight) / 2 - lineWidth);
            var paddingHorizontal = buttonPaddingHorizontal - lineWidth;
            var iconOnlyCls = @$"{componentCls}-icon-only";
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}{sizePrefixCls}"] = new CSSObject()
                    {
                        FontSize = fontSize,
                        Height = controlHeight,
                        Padding = @$"{paddingVertical}px {paddingHorizontal}px",
                        BorderRadius = borderRadius,
                        [$"&{iconOnlyCls}"] = new CSSObject()
                        {
                            Width = controlHeight,
                            PaddingInlineStart = 0,
                            PaddingInlineEnd = 0,
                            [$"&{componentCls}-round"] = new CSSObject()
                            {
                                Width = "auto",
                            },
                            [iconCls] = new CSSObject()
                            {
                                FontSize = token.ButtonIconOnlyFontSize,
                            },
                        },
                        [$"&{componentCls}-loading"] = new CSSObject()
                        {
                            Opacity = token.OpacityLoading,
                            Cursor = "default",
                        },
                        [$"{componentCls}-loading-icon"] = new CSSObject()
                        {
                            Transition = @$"width {token.MotionDurationSlow} {token.MotionEaseInOut}, opacity {token.MotionDurationSlow} {token.MotionEaseInOut}",
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}{componentCls}-circle{sizePrefixCls}"] = GenCircleButtonStyle(token)
                },
                new CSSObject()
                {
                    [$"{componentCls}{componentCls}-round{sizePrefixCls}"] = GenRoundButtonStyle(token)
                },
            };
        }

        public CSSInterpolation GenSizeBaseButtonStyle(ButtonToken token)
        {
            return GenSizeButtonStyle(token);
        }

        public CSSInterpolation GenSizeSmallButtonStyle(ButtonToken token)
        {
            var smallToken = MergeToken(
                token,
                new ButtonToken()
                {
                    ControlHeight = token.ControlHeightSM,
                    Padding = token.PaddingXS,
                    ButtonPaddingHorizontal = 8,
                    BorderRadius = token.BorderRadiusSM,
                    ButtonIconOnlyFontSize = token.FontSizeLG - 2,
                });
            return GenSizeButtonStyle(smallToken, $"{token.ComponentCls}-sm");
        }

        public CSSInterpolation GenSizeLargeButtonStyle(ButtonToken token)
        {
            var largeToken = MergeToken(
                token,
                new ButtonToken()
                {
                    ControlHeight = token.ControlHeightLG,
                    FontSize = token.FontSizeLG,
                    BorderRadius = token.BorderRadiusLG,
                    ButtonIconOnlyFontSize = token.FontSizeLG + 2,
                });
            return GenSizeButtonStyle(largeToken, $"{token.ComponentCls}-lg");
        }

        public CSSObject GenBlockButtonStyle(ButtonToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    [$"&{componentCls}-block"] = new CSSObject()
                    {
                        Width = "100%",
                    },
                },
            };
        }

        public CSSInterpolation GenComponentStyleHook(TokenWithCommonCls token)
        {
            var controlTmpOutline = token.ControlTmpOutline;
            var paddingContentHorizontal = token.PaddingContentHorizontal;
            var buttonToken = MergeToken(
                token,
                new ButtonToken()
                {
                    ColorOutlineDefault = controlTmpOutline,
                    ButtonPaddingHorizontal = paddingContentHorizontal,
                    ButtonIconOnlyFontSize = token.FontSizeLG,
                    ButtonFontWeight = 400,
                });
            return new CSSInterpolation[]
            {
                GenSharedButtonStyle(buttonToken),
                GenSizeSmallButtonStyle(buttonToken),
                GenSizeBaseButtonStyle(buttonToken),
                GenSizeLargeButtonStyle(buttonToken),
                GenBlockButtonStyle(buttonToken),
                GenTypeButtonStyle(buttonToken),
                GenGroupStyle(buttonToken),
                GenCompactItemStyle(token),
                GenCompactItemVerticalStyle(token)
            };
        }

        public CSSObject GenButtonBorderStyle(string buttonTypeCls, string borderColor)
        {
            return new CSSObject()
            {
                [$"> span, > {buttonTypeCls}"] = new CSSObject()
                {
                    ["&:not(:last-child)"] = new CSSObject()
                    {
                        [$"&, & > {buttonTypeCls}"] = new CSSObject()
                        {
                            ["&:not(:disabled)"] = new CSSObject()
                            {
                                BorderInlineEndColor = borderColor,
                            },
                        },
                    },
                    ["&:not(:first-child)"] = new CSSObject()
                    {
                        [$"&, & > {buttonTypeCls}"] = new CSSObject()
                        {
                            ["&:not(:disabled)"] = new CSSObject()
                            {
                                BorderInlineStartColor = borderColor,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenGroupStyle(ButtonToken token)
        {
            var componentCls = token.ComponentCls;
            var fontSize = token.FontSize;
            var lineWidth = token.LineWidth;
            var colorPrimaryHover = token.ColorPrimaryHover;
            var colorErrorHover = token.ColorErrorHover;
            return new CSSObject()
            {
                [$"{componentCls}-group"] = new CSSInterpolation[]
                {
                    new CSSObject()
                    {
                        Position = "relative",
                        Display = "inline-flex",
                        [$"> span, > {componentCls}"] = new CSSObject()
                        {
                            ["&:not(:last-child)"] = new CSSObject()
                            {
                                [$"&, & > {componentCls}"] = new CSSObject()
                                {
                                    BorderStartEndRadius = 0,
                                    BorderEndEndRadius = 0,
                                },
                            },
                            ["&:not(:first-child)"] = new CSSObject()
                            {
                                MarginInlineStart = -lineWidth,
                                [$"&, & > {componentCls}"] = new CSSObject()
                                {
                                    BorderStartStartRadius = 0,
                                    BorderEndStartRadius = 0,
                                },
                            },
                        },
                        [componentCls] = new CSSObject()
                        {
                            Position = "relative",
                            ZIndex = 1,
                            ["&:hover,&:focus,&:active"] = new CSSObject()
                            {
                                ZIndex = 2,
                            },
                            ["&[disabled]"] = new CSSObject()
                            {
                                ZIndex = 0,
                            },
                        },
                        [$"{componentCls}-icon-only"] = new CSSObject()
                        {
                            FontSize = fontSize,
                        },
                    },
                    GenButtonBorderStyle($"{componentCls}-primary", colorPrimaryHover),
                    GenButtonBorderStyle($"{componentCls}-danger", colorErrorHover)
                }
            };
        }

    }

}
