using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SelectToken
    {
        public int ZIndexPopup { get; set; }

    }

    public partial class SelectToken : TokenWithCommonCls
    {
        public string RootPrefixCls { get; set; }

        public int InputPaddingHorizontalBase { get; set; }

    }

    public partial class Select
    {
        public Unknown_1 GenSelectorStyle(Unknown_9 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_10()
            {
                Position = "relative",
                BackgroundColor = token.ColorBgContainer,
                Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                Transition = @$"all {token.MotionDurationMid} {token.MotionEaseInOut}",
                Input = new Unknown_11()
                {
                    Cursor = "pointer",
                },
                [$"{componentCls}-show-search&"] = new Unknown_12()
                {
                    Cursor = "text",
                    Input = new Unknown_13()
                    {
                        Cursor = "auto",
                        Color = "inherit",
                    },
                },
                [$"{componentCls}-disabled&"] = new Unknown_14()
                {
                    Color = token.ColorTextDisabled,
                    Background = token.ColorBgContainerDisabled,
                    Cursor = "not-allowed",
                    [$"{componentCls}-multiple&"] = new Unknown_15()
                    {
                        Background = token.ColorBgContainerDisabled,
                    },
                    Input = new Unknown_16()
                    {
                        Cursor = "not-allowed",
                    },
                },
            };
        }

        public CSSObject GenStatusStyle(string rootSelectCls, {
    componentCls: string;
    antCls: string;
    borderHoverColor: string;
    outlineColor: string;
    controlOutlineWidth: number;
  } token, bool overwriteDefaultBorder = false)
        {
            var componentCls = token.ComponentCls;
            var borderHoverColor = token.BorderHoverColor;
            var outlineColor = token.OutlineColor;
            var antCls = token.AntCls;
            var overwriteStyle = overwriteDefaultBorder
    ? {
        [@$"{componentCls}-selector"]: {
          borderColor: borderHoverColor,
        },
      }
    : {};
            return new CSSObject()
            {
                [rootSelectCls] = new CSSObject()
                {
                    [$"&:not({componentCls}-disabled):not({componentCls}-customize-input):not({antCls}-pagination-size-changer)"] = new CSSObject()
                    {
                        ["..."] = overwriteStyle,
                        [$"{componentCls}-focused& {componentCls}-selector"] = new CSSObject()
                        {
                            BorderColor = borderHoverColor,
                            BoxShadow = @$"0 0 0 {token.ControlOutlineWidth}px {outlineColor}",
                            Outline = 0,
                        },
                        [$"&:hover {componentCls}-selector"] = new CSSObject()
                        {
                            BorderColor = borderHoverColor,
                        },
                    },
                },
            };
        }

        public Unknown_2 GetSearchInputWithoutBorderStyle(Unknown_17 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_18()
            {
                [$"{componentCls}-selection-search-input"] = new Unknown_19()
                {
                    Margin = 0,
                    Padding = 0,
                    Background = "transparent",
                    Border = "none",
                    Outline = "none",
                    Appearance = "none",
                    ["&::-webkit-search-cancel-button"] = new Unknown_20()
                    {
                        Display = "none",
                        ["-webkit-appearance"] = "none",
                    },
                },
            };
        }

        public Unknown_3 GenBaseStyle(Unknown_21 token)
        {
            var componentCls = token.ComponentCls;
            var inputPaddingHorizontalBase = token.InputPaddingHorizontalBase;
            var iconCls = token.IconCls;
            return new Unknown_22()
            {
                [componentCls] = new Unknown_23()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    Cursor = "pointer",
                    [$"&:not({componentCls}-customize-input) {componentCls}-selector"] = new Unknown_24()
                    {
                        ["..."] = GenSelectorStyle(token),
                        ["..."] = GetSearchInputWithoutBorderStyle(token)
                    },
                    [$"{componentCls}-selection-item"] = new Unknown_25()
                    {
                        Flex = 1,
                        FontWeight = "normal",
                        ["..."] = textEllipsis,
                        ["> *"] = new Unknown_26()
                        {
                            LineHeight = "inherit",
                            ["..."] = textEllipsis,
                        },
                    },
                    [$"{componentCls}-selection-placeholder"] = new Unknown_27()
                    {
                        ["..."] = textEllipsis,
                        Flex = 1,
                        Color = token.ColorTextPlaceholder,
                        PointerEvents = "none",
                    },
                    [$"{componentCls}-arrow"] = new Unknown_28()
                    {
                        ["..."] = ResetIcon(),
                        Position = "absolute",
                        Top = "50%",
                        InsetInlineStart = "auto",
                        InsetInlineEnd = inputPaddingHorizontalBase,
                        Height = token.FontSizeIcon,
                        MarginTop = -token.FontSizeIcon / 2,
                        Color = token.ColorTextQuaternary,
                        FontSize = token.FontSizeIcon,
                        LineHeight = 1,
                        TextAlign = "center",
                        PointerEvents = "none",
                        Display = "flex",
                        AlignItems = "center",
                        [iconCls] = new Unknown_29()
                        {
                            VerticalAlign = "top",
                            Transition = @$"transform {token.MotionDurationSlow}",
                            ["> svg"] = new Unknown_30()
                            {
                                VerticalAlign = "top",
                            },
                            [$"&:not({componentCls}-suffix)"] = new Unknown_31()
                            {
                                PointerEvents = "auto",
                            },
                        },
                        [$"{componentCls}-disabled &"] = new Unknown_32()
                        {
                            Cursor = "not-allowed",
                        },
                        ["> *:not(:last-child)"] = new Unknown_33()
                        {
                            MarginInlineEnd = 8,
                        },
                    },
                    [$"{componentCls}-clear"] = new Unknown_34()
                    {
                        Position = "absolute",
                        Top = "50%",
                        InsetInlineStart = "auto",
                        InsetInlineEnd = inputPaddingHorizontalBase,
                        ZIndex = 1,
                        Display = "inline-block",
                        Width = token.FontSizeIcon,
                        Height = token.FontSizeIcon,
                        MarginTop = -token.FontSizeIcon / 2,
                        Color = token.ColorTextQuaternary,
                        FontSize = token.FontSizeIcon,
                        FontStyle = "normal",
                        LineHeight = 1,
                        TextAlign = "center",
                        TextTransform = "none",
                        Background = token.ColorBgContainer,
                        Cursor = "pointer",
                        Opacity = 0,
                        Transition = @$"color {token.MotionDurationMid} ease, opacity {token.MotionDurationSlow} ease",
                        TextRendering = "auto",
                        ["&:before"] = new Unknown_35()
                        {
                            Display = "block",
                        },
                        ["&:hover"] = new Unknown_36()
                        {
                            Color = token.ColorTextTertiary,
                        },
                    },
                    ["&:hover"] = new Unknown_37()
                    {
                        [$"{componentCls}-clear"] = new Unknown_38()
                        {
                            Opacity = 1,
                        },
                    },
                },
                [$"{componentCls}-has-feedback"] = new Unknown_39()
                {
                    [$"{componentCls}-clear"] = new Unknown_40()
                    {
                        InsetInlineEnd = inputPaddingHorizontalBase + token.FontSize + token.PaddingXXS,
                    },
                },
            };
        }

        public Unknown_4 GenSelectStyle(Unknown_41 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_42
            {
                new Unknown_43()
                {
                    [componentCls] = new Unknown_44()
                    {
                        [$"&-borderless {componentCls}-selector"] = new Unknown_45()
                        {
                            BackgroundColor = @$"transparent !important",
                            BorderColor = @$"transparent !important",
                            BoxShadow = @$"none !important",
                        },
                        [$"&{componentCls}-in-form-item"] = new Unknown_46()
                        {
                            Width = "100%",
                        },
                    },
                },
                GenBaseStyle(token),
                GenSingleStyle(token),
                GenMultipleStyle(token),
                GenDropdownStyle(token),
                new Unknown_47()
                {
                    [$"{componentCls}-rtl"] = new Unknown_48()
                    {
                        Direction = "rtl",
                    },
                },
                GenStatusStyle(
      componentCls,
      mergeToken<any>(token, {
        borderHoverColor: token.ColorPrimaryHover,
        outlineColor: token.ControlOutline,
      }),
    ),
                GenStatusStyle(
      $"{componentCls}-status-error",
      mergeToken<any>(token, {
        borderHoverColor: token.ColorErrorHover,
        outlineColor: token.ColorErrorOutline,
      }),
      true,
    ),
                GenStatusStyle(
      $"{componentCls}-status-warning",
      mergeToken<any>(token, {
        borderHoverColor: token.ColorWarningHover,
        outlineColor: token.ColorWarningOutline,
      }),
      true,
    ),
                GenCompactItemStyle(token, {
      borderElCls: $"{componentCls}-selector",
      focusElCls: `{componentCls}-focused`,
    })
            };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_49 token, Unknown_50 args)
        {
            var selectToken = MergeToken(
                token,
                new Unknown_51()
                {
                    RootPrefixCls = rootPrefixCls,
                    InputPaddingHorizontalBase = token.PaddingSM - 1,
                });
            return new Unknown_52 { GenSelectStyle(selectToken) };
        }

        public Unknown_6 GenComponentStyleHook(Unknown_53 token)
        {
            return new Unknown_54()
            {
                ZIndexPopup = token.ZIndexPopupBase + 50,
            };
        }

        public Unknown_7 GenItemStyle(Unknown_55 token)
        {
            var controlPaddingHorizontal = token.ControlPaddingHorizontal;
            return new Unknown_56()
            {
                Position = "relative",
                Display = "block",
                MinHeight = token.ControlHeight,
                Padding = @$"{
      (token.ControlHeight - token.FontSize * token.LineHeight) / 2
    }px {controlPaddingHorizontal}px",
                Color = token.ColorText,
                FontWeight = "normal",
                FontSize = token.FontSize,
                LineHeight = token.LineHeight,
                BoxSizing = "border-box",
            };
        }

        public Unknown_8 GenSingleStyle(Unknown_57 token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var selectItemCls = @$"{componentCls}-item";
            return new Unknown_58
            {
                new Unknown_59()
                {
                    [$"{componentCls}-dropdown"] = new Unknown_60()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        Top = -9999,
                        ZIndex = token.ZIndexPopup,
                        BoxSizing = "border-box",
                        Padding = token.PaddingXXS,
                        Overflow = "hidden",
                        FontSize = token.FontSize,
                        FontVariant = "initial",
                        BackgroundColor = token.ColorBgElevated,
                        BorderRadius = token.BorderRadiusLG,
                        Outline = "none",
                        BoxShadow = token.BoxShadowSecondary,
                        [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-bottomLeft"] = new Unknown_61()
                        {
                            AnimationName = slideUpIn,
                        },
                        [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-topLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-topLeft"] = new Unknown_62()
                        {
                            AnimationName = slideDownIn,
                        },
                        [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-bottomLeft"] = new Unknown_63()
                        {
                            AnimationName = slideUpOut,
                        },
                        [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-topLeft"] = new Unknown_64()
                        {
                            AnimationName = slideDownOut,
                        },
                        ["&-hidden"] = new Unknown_65()
                        {
                            Display = "none",
                        },
                        [$"{selectItemCls}"] = new Unknown_66()
                        {
                            ["..."] = GenItemStyle(token),
                            Cursor = "pointer",
                            Transition = @$"background {token.MotionDurationSlow} ease",
                            BorderRadius = token.BorderRadiusSM,
                            ["&-group"] = new Unknown_67()
                            {
                                Color = token.ColorTextDescription,
                                FontSize = token.FontSizeSM,
                                Cursor = "default",
                            },
                            ["&-option"] = new Unknown_68()
                            {
                                Display = "flex",
                                ["&-content"] = new Unknown_69()
                                {
                                    Flex = "auto",
                                    ["..."] = textEllipsis,
                                    ["> *"] = new Unknown_70()
                                    {
                                        ["..."] = textEllipsis,
                                    },
                                },
                                ["&-state"] = new Unknown_71()
                                {
                                    Flex = "none",
                                },
                                [$"&-active:not({selectItemCls}-option-disabled)"] = new Unknown_72()
                                {
                                    BackgroundColor = token.ControlItemBgHover,
                                },
                                [$"&-selected:not({selectItemCls}-option-disabled)"] = new Unknown_73()
                                {
                                    Color = token.ColorText,
                                    FontWeight = token.FontWeightStrong,
                                    BackgroundColor = token.ControlItemBgActive,
                                    [$"{selectItemCls}-option-state"] = new Unknown_74()
                                    {
                                        Color = token.ColorPrimary,
                                    },
                                },
                                ["&-disabled"] = new Unknown_75()
                                {
                                    [$"&{selectItemCls}-option-selected"] = new Unknown_76()
                                    {
                                        BackgroundColor = token.ColorBgContainerDisabled,
                                    },
                                    Color = token.ColorTextDisabled,
                                    Cursor = "not-allowed",
                                },
                                ["&-grouped"] = new Unknown_77()
                                {
                                    PaddingInlineStart = token.ControlPaddingHorizontal * 2,
                                },
                            },
                        },
                        ["&-rtl"] = new Unknown_78()
                        {
                            Direction = "rtl",
                        },
                    },
                },
                InitSlideMotion(token, "slide-up"),
                InitSlideMotion(token, "slide-down"),
                InitMoveMotion(token, "move-up"),
                InitMoveMotion(token, "move-down")
            };
        }

        public readonly [number, number] GetSelectItemStyle(Unknown_79 args)
        {
            var selectItemDist = (controlHeight - controlHeightSM) / 2 - borderWidth;
            var selectItemMargin = Math.Ceil(selectItemDist / 2);
        }

        public CSSObject GenSizeStyle(SelectToken token, string suffix)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var selectOverflowPrefixCls = @$"{componentCls}-selection-overflow";
            var selectItemHeight = token.ControlHeightSM;
            var [selectItemDist] = GetSelectItemStyle(token);
            var suffixCls = suffix ? @$"{componentCls}-{suffix}" : "";
            return new CSSObject()
            {
                [$"{componentCls}-multiple{suffixCls}"] = new CSSObject()
                {
                    FontSize = token.FontSize,
                    [selectOverflowPrefixCls] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "flex",
                        Flex = "auto",
                        FlexWrap = "wrap",
                        MaxWidth = "100%",
                        ["&-item"] = new CSSObject()
                        {
                            Flex = "none",
                            AlignSelf = "center",
                            MaxWidth = "100%",
                            Display = "inline-flex",
                        },
                    },
                    [$"{componentCls}-selector"] = new CSSObject()
                    {
                        Display = "flex",
                        FlexWrap = "wrap",
                        AlignItems = "center",
                        Padding = @$"{selectItemDist - FIXED_ITEM_MARGIN}px {FIXED_ITEM_MARGIN * 2}px",
                        BorderRadius = token.BorderRadius,
                        [$"{componentCls}-show-search&"] = new CSSObject()
                        {
                            Cursor = "text",
                        },
                        [$"{componentCls}-disabled&"] = new CSSObject()
                        {
                            Background = token.ColorBgContainerDisabled,
                            Cursor = "not-allowed",
                        },
                        ["&:after"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = 0,
                            Margin = @$"{FIXED_ITEM_MARGIN}px 0",
                            LineHeight = @$"{selectItemHeight}px",
                            Content = '"\\a0"',
                        },
                    },
                    [$"&{componentCls}-show-arrow{componentCls}-selector,&{componentCls}-allow-clear{componentCls}-selector"] = new CSSObject()
                    {
                        PaddingInlineEnd = token.FontSizeIcon + token.ControlPaddingHorizontal,
                    },
                    [$"{componentCls}-selection-item"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "flex",
                        Flex = "none",
                        BoxSizing = "border-box",
                        MaxWidth = "100%",
                        Height = selectItemHeight,
                        MarginTop = FIXED_ITEM_MARGIN,
                        MarginBottom = FIXED_ITEM_MARGIN,
                        LineHeight = @$"{selectItemHeight - token.LineWidth * 2}px",
                        Background = token.ColorFillSecondary,
                        BorderRadius = token.BorderRadiusSM,
                        Cursor = "default",
                        Transition = @$"font-size {token.MotionDurationSlow}, line-height {token.MotionDurationSlow}, height {token.MotionDurationSlow}",
                        UserSelect = "none",
                        MarginInlineEnd = FIXED_ITEM_MARGIN * 2,
                        PaddingInlineStart = token.PaddingXS,
                        PaddingInlineEnd = token.PaddingXS / 2,
                        [$"{componentCls}-disabled&"] = new CSSObject()
                        {
                            Color = token.ColorTextDisabled,
                            Cursor = "not-allowed",
                        },
                        ["&-content"] = new CSSObject()
                        {
                            Display = "inline-block",
                            MarginInlineEnd = token.PaddingXS / 2,
                            Overflow = "hidden",
                            WhiteSpace = "pre",
                            TextOverflow = "ellipsis",
                        },
                        ["&-remove"] = new CSSObject()
                        {
                            ["..."] = ResetIcon(),
                            Display = "inline-block",
                            Color = token.ColorIcon,
                            FontWeight = "bold",
                            FontSize = 10,
                            LineHeight = "inherit",
                            Cursor = "pointer",
                            [$"> {iconCls}"] = new CSSObject()
                            {
                                VerticalAlign = "-0.2em",
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                Color = token.ColorIconHover,
                            },
                        },
                    },
                    [$"{selectOverflowPrefixCls}-item + {selectOverflowPrefixCls}-item"] = new CSSObject()
                    {
                        [$"{componentCls}-selection-search"] = new CSSObject()
                        {
                            MarginInlineStart = 0,
                        },
                    },
                    [$"{componentCls}-selection-search"] = new CSSObject()
                    {
                        Display = "inline-flex",
                        Position = "relative",
                        MaxWidth = "100%",
                        MarginInlineStart = token.InputPaddingHorizontalBase - selectItemDist,
                        ["&-input,&-mirror"] = new CSSObject()
                        {
                            Height = selectItemHeight,
                            FontFamily = token.FontFamily,
                            LineHeight = @$"{selectItemHeight}px",
                            Transition = @$"all {token.MotionDurationSlow}",
                        },
                        ["&-input"] = new CSSObject()
                        {
                            Width = "100%",
                            MinWidth = 4.1f,
                        },
                        ["&-mirror"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = 0,
                            InsetInlineEnd = "auto",
                            ZIndex = 999,
                            WhiteSpace = "pre",
                            Visibility = "hidden",
                        },
                    },
                    [$"{componentCls}-selection-placeholder "] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = "50%",
                        InsetInlineStart = token.InputPaddingHorizontalBase,
                        InsetInlineEnd = token.InputPaddingHorizontalBase,
                        Transform = "translateY(-50%)",
                        Transition = @$"all {token.MotionDurationSlow}",
                    },
                },
            };
        }

        public CSSInterpolation GenMultipleStyle(SelectToken token)
        {
            var componentCls = token.ComponentCls;
            var smallToken = MergeToken(
                token,
                new CSSInterpolation()
                {
                    ControlHeight = token.ControlHeightSM,
                    ControlHeightSM = token.ControlHeightXS,
                    BorderRadius = token.BorderRadiusSM,
                    BorderRadiusSM = token.BorderRadiusXS,
                });
            var largeToken = MergeToken(
                token,
                new CSSInterpolation()
                {
                    FontSize = token.FontSizeLG,
                    ControlHeight = token.ControlHeightLG,
                    ControlHeightSM = token.ControlHeight,
                    BorderRadius = token.BorderRadiusLG,
                    BorderRadiusSM = token.BorderRadius,
                });
            var [, smSelectItemMargin] = GetSelectItemStyle(token);
            return new CSSInterpolation
            {
                GenSizeStyle(token),
                GenSizeStyle(smallToken, "sm"),
                new Unknown_80()
                {
                    [$"{componentCls}-multiple{componentCls}-sm"] = new Unknown_81()
                    {
                        [$"{componentCls}-selection-placeholder"] = new Unknown_82()
                        {
                            InsetInline = token.ControlPaddingHorizontalSM - token.LineWidth,
                        },
                        [$"{componentCls}-selection-search"] = new Unknown_83()
                        {
                            MarginInlineStart = smSelectItemMargin,
                        },
                    },
                },
                GenSizeStyle(largeToken, "lg")
            };
        }

        public CSSObject GenSizeStyle(SelectToken token, string suffix)
        {
            var componentCls = token.ComponentCls;
            var inputPaddingHorizontalBase = token.InputPaddingHorizontalBase;
            var borderRadius = token.BorderRadius;
            var selectHeightWithoutBorder = token.ControlHeight - token.LineWidth * 2;
            var selectionItemPadding = Math.Ceil(token.FontSize * 1.25);
            var suffixCls = suffix ? @$"{componentCls}-{suffix}" : "";
            return new CSSObject()
            {
                [$"{componentCls}-single{suffixCls}"] = new CSSObject()
                {
                    FontSize = token.FontSize,
                    [$"{componentCls}-selector"] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        Display = "flex",
                        BorderRadius = borderRadius,
                        [$"{componentCls}-selection-search"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = inputPaddingHorizontalBase,
                            InsetInlineEnd = inputPaddingHorizontalBase,
                            Bottom = 0,
                            ["&-input"] = new CSSObject()
                            {
                                Width = "100%",
                            },
                        },
                        [$"{componentCls}-selection-item,{componentCls}-selection-placeholder"] = new CSSObject()
                        {
                            Padding = 0,
                            LineHeight = @$"{selectHeightWithoutBorder}px",
                            Transition = @$"all {token.MotionDurationSlow}, visibility 0s",
                            ["@supports (-moz-appearance: meterbar)"] = new CSSObject()
                            {
                                LineHeight = @$"{selectHeightWithoutBorder}px",
                            },
                        },
                        [$"{componentCls}-selection-item"] = new CSSObject()
                        {
                            Position = "relative",
                            UserSelect = "none",
                        },
                        [$"{componentCls}-selection-placeholder"] = new CSSObject()
                        {
                            Transition = "none",
                            PointerEvents = "none",
                        },
                        [[$"&:after",/*For""valuebaselinealign*/"{componentCls}-selection-item:after",/*Forundefinedvaluebaselinealign*/"{componentCls}-selection-placeholder:after",].join(",")] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = 0,
                            Visibility = "hidden",
                            Content = '"\\a0"',
                        },
                    },
                    [$"&{componentCls}-show-arrow{componentCls}-selection-item,&{componentCls}-show-arrow{componentCls}-selection-placeholder"] = new CSSObject()
                    {
                        PaddingInlineEnd = selectionItemPadding,
                    },
                    [$"&{componentCls}-open {componentCls}-selection-item"] = new CSSObject()
                    {
                        Color = token.ColorTextPlaceholder,
                    },
                    [$"&:not({componentCls}-customize-input)"] = new CSSObject()
                    {
                        [$"{componentCls}-selector"] = new CSSObject()
                        {
                            Width = "100%",
                            Height = token.ControlHeight,
                            Padding = @$"0 {inputPaddingHorizontalBase}px",
                            [$"{componentCls}-selection-search-input"] = new CSSObject()
                            {
                                Height = selectHeightWithoutBorder,
                            },
                            ["&:after"] = new CSSObject()
                            {
                                LineHeight = @$"{selectHeightWithoutBorder}px",
                            },
                        },
                    },
                    [$"&{componentCls}-customize-input"] = new CSSObject()
                    {
                        [$"{componentCls}-selector"] = new CSSObject()
                        {
                            ["&:after"] = new CSSObject()
                            {
                                Display = "none",
                            },
                            [$"{componentCls}-selection-search"] = new CSSObject()
                            {
                                Position = "static",
                                Width = "100%",
                            },
                            [$"{componentCls}-selection-placeholder"] = new CSSObject()
                            {
                                Position = "absolute",
                                InsetInlineStart = 0,
                                InsetInlineEnd = 0,
                                Padding = @$"0 {inputPaddingHorizontalBase}px",
                                ["&:after"] = new CSSObject()
                                {
                                    Display = "none",
                                },
                            },
                        },
                    },
                },
            };
        }

        public CSSInterpolation GenSingleStyle(SelectToken token)
        {
            var componentCls = token.ComponentCls;
            var inputPaddingHorizontalSM = token.ControlPaddingHorizontalSM - token.LineWidth;
            return new CSSInterpolation
            {
                GenSizeStyle(token),
                GenSizeStyle(
      mergeToken<any>(token, {
        controlHeight: token.ControlHeightSM,
        borderRadius: token.BorderRadiusSM,
      }),
      "sm",
    ),
                new Unknown_84()
                {
                    [$"{componentCls}-single{componentCls}-sm"] = new Unknown_85()
                    {
                        [$"&:not({componentCls}-customize-input)"] = new Unknown_86()
                        {
                            [$"{componentCls}-selection-search"] = new Unknown_87()
                            {
                                InsetInlineStart = inputPaddingHorizontalSM,
                                InsetInlineEnd = inputPaddingHorizontalSM,
                            },
                            [$"{componentCls}-selector"] = new Unknown_88()
                            {
                                Padding = @$"0 {inputPaddingHorizontalSM}px",
                            },
                            [$"&{componentCls}-show-arrow {componentCls}-selection-search"] = new Unknown_89()
                            {
                                InsetInlineEnd = inputPaddingHorizontalSM + token.FontSize * 1.5,
                            },
                            [$"&{componentCls}-show-arrow{componentCls}-selection-item,&{componentCls}-show-arrow{componentCls}-selection-placeholder"] = new Unknown_90()
                            {
                                PaddingInlineEnd = token.FontSize * 1.5,
                            },
                        },
                    },
                },
                GenSizeStyle(
      mergeToken<any>(token, {
        controlHeight: token.ControlHeightLG,
        fontSize: token.FontSizeLG,
        borderRadius: token.BorderRadiusLG,
      }),
      "lg",
    )
            };
        }

    }

}