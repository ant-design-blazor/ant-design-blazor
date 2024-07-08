using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Slide;
using static AntDesign.Move;

namespace AntDesign
{
    public partial class SelectToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public string OptionSelectedColor
        {
            get => (string)_tokens["optionSelectedColor"];
            set => _tokens["optionSelectedColor"] = value;
        }

        public double OptionSelectedFontWeight
        {
            get => (double)_tokens["optionSelectedFontWeight"];
            set => _tokens["optionSelectedFontWeight"] = value;
        }

        public string OptionSelectedBg
        {
            get => (string)_tokens["optionSelectedBg"];
            set => _tokens["optionSelectedBg"] = value;
        }

        public string OptionActiveBg
        {
            get => (string)_tokens["optionActiveBg"];
            set => _tokens["optionActiveBg"] = value;
        }

        public string OptionPadding
        {
            get => (string)_tokens["optionPadding"];
            set => _tokens["optionPadding"] = value;
        }

        public double OptionFontSize
        {
            get => (double)_tokens["optionFontSize"];
            set => _tokens["optionFontSize"] = value;
        }

        public double OptionLineHeight
        {
            get => (double)_tokens["optionLineHeight"];
            set => _tokens["optionLineHeight"] = value;
        }

        public double OptionHeight
        {
            get => (double)_tokens["optionHeight"];
            set => _tokens["optionHeight"] = value;
        }

        public string SelectorBg
        {
            get => (string)_tokens["selectorBg"];
            set => _tokens["selectorBg"] = value;
        }

        public string ClearBg
        {
            get => (string)_tokens["clearBg"];
            set => _tokens["clearBg"] = value;
        }

        public double SingleItemHeightLG
        {
            get => (double)_tokens["singleItemHeightLG"];
            set => _tokens["singleItemHeightLG"] = value;
        }

        public string MultipleItemBg
        {
            get => (string)_tokens["multipleItemBg"];
            set => _tokens["multipleItemBg"] = value;
        }

        public string MultipleItemBorderColor
        {
            get => (string)_tokens["multipleItemBorderColor"];
            set => _tokens["multipleItemBorderColor"] = value;
        }

        public double MultipleItemHeight
        {
            get => (double)_tokens["multipleItemHeight"];
            set => _tokens["multipleItemHeight"] = value;
        }

        public double MultipleItemHeightLG
        {
            get => (double)_tokens["multipleItemHeightLG"];
            set => _tokens["multipleItemHeightLG"] = value;
        }

        public string MultipleSelectorBgDisabled
        {
            get => (string)_tokens["multipleSelectorBgDisabled"];
            set => _tokens["multipleSelectorBgDisabled"] = value;
        }

        public string MultipleItemColorDisabled
        {
            get => (string)_tokens["multipleItemColorDisabled"];
            set => _tokens["multipleItemColorDisabled"] = value;
        }

        public string MultipleItemBorderColorDisabled
        {
            get => (string)_tokens["multipleItemBorderColorDisabled"];
            set => _tokens["multipleItemBorderColorDisabled"] = value;
        }

    }

    public partial class SelectToken : TokenWithCommonCls
    {
        public string RootPrefixCls
        {
            get => (string)_tokens["rootPrefixCls"];
            set => _tokens["rootPrefixCls"] = value;
        }

        public double InputPaddingHorizontalBase
        {
            get => (double)_tokens["inputPaddingHorizontalBase"];
            set => _tokens["inputPaddingHorizontalBase"] = value;
        }

        public double MultipleSelectItemHeight
        {
            get => (double)_tokens["multipleSelectItemHeight"];
            set => _tokens["multipleSelectItemHeight"] = value;
        }

        public double SelectHeight
        {
            get => (double)_tokens["selectHeight"];
            set => _tokens["selectHeight"] = value;
        }

        public string BorderHoverColor
        {
            get => (string)_tokens["borderHoverColor"];
            set => _tokens["borderHoverColor"] = value;
        }

        public string BorderActiveColor
        {
            get => (string)_tokens["borderActiveColor"];
            set => _tokens["borderActiveColor"] = value;
        }

        public string OutlineColor
        {
            get => (string)_tokens["outlineColor"];
            set => _tokens["outlineColor"] = value;
        }
    }

    public class SelectStyle
    {
        private const double FIXED_ITEM_MARGIN = 2;

        public CSSObject GenSelectorStyle(SelectToken token)
        {
            var componentCls = token.ComponentCls;
            var selectorBg = token.SelectorBg;
            return new CSSObject()
            {
                Position = "relative",
                BackgroundColor = selectorBg,
                Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                Transition = @$"all {token.MotionDurationMid} {token.MotionEaseInOut}",
                ["input"] = new CSSObject()
                {
                    Cursor = "pointer",
                },
                [$"{componentCls}-show-search&"] = new CSSObject()
                {
                    Cursor = "text",
                    ["input"] = new CSSObject()
                    {
                        Cursor = "auto",
                        Color = "inherit",
                        Height = "100%",
                    },
                },
                [$"{componentCls}-disabled&"] = new CSSObject()
                {
                    Color = token.ColorTextDisabled,
                    Background = token.ColorBgContainerDisabled,
                    Cursor = "not-allowed",
                    [$"{componentCls}-multiple&"] = new CSSObject()
                    {
                        Background = token.MultipleSelectorBgDisabled,
                    },
                    ["input"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                    },
                },
            };
        }

        public CSSObject GenStatusStyle(string rootSelectCls, SelectToken token, bool overwriteDefaultBorder = false)
        {
            var componentCls = token.ComponentCls;
            var borderHoverColor = token.BorderHoverColor;
            var antCls = token.AntCls;
            var borderActiveColor = token.BorderActiveColor;
            var outlineColor = token.OutlineColor;
            var controlOutlineWidth = token.ControlOutlineWidth;
            var overwriteStyle = overwriteDefaultBorder
                ? new CSSObject()
                {
                    [$"{componentCls}-selector"] = new CSSObject()
                    {
                        BorderColor = borderActiveColor,
                    },
                }
                : new CSSObject()
                {
                };
            return new CSSObject()
            {
                [rootSelectCls] = new CSSObject()
                {
                    [$"&:not({componentCls}-disabled):not({componentCls}-customize-input):not({antCls}-pagination-size-changer)"] = new CSSObject()
                    {
                        ["..."] = overwriteStyle,
                        [$"&:hover {componentCls}-selector"] = new CSSObject()
                        {
                            BorderColor = borderHoverColor,
                        },
                        [$"{componentCls}-focused& {componentCls}-selector"] = new CSSObject()
                        {
                            BorderColor = borderActiveColor,
                            BoxShadow = @$"0 0 0 {controlOutlineWidth}px {outlineColor}",
                            Outline = 0,
                        },
                    },
                },
            };
        }

        public CSSObject GetSearchInputWithoutBorderStyle(SelectToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-selection-search-input"] = new CSSObject()
                {
                    Margin = 0,
                    Padding = 0,
                    Background = "transparent",
                    Border = "none",
                    Outline = "none",
                    Appearance = "none",
                    FontFamily = "inherit",
                    ["&::-webkit-search-cancel-button"] = new CSSObject()
                    {
                        Display = "none",
                        ["-webkit-appearance"] = "none",
                    },
                },
            };
        }

        public CSSObject GenBaseStyle(SelectToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var inputPaddingHorizontalBase = token.InputPaddingHorizontalBase;
            var iconCls = token.IconCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    Cursor = "pointer",
                    [$"&:not({componentCls}-customize-input) {componentCls}-selector"] = new CSSObject()
                    {
                        ["..."] = GenSelectorStyle(token),
                        ["..."] = GetSearchInputWithoutBorderStyle(token)
                    },
                    [$"{componentCls}-selection-item"] = new CSSObject()
                    {
                        Flex = 1,
                        FontWeight = "normal",
                        Position = "relative",
                        UserSelect = "none",
                        ["..."] = TextEllipsis,
                        [$"> {antCls}-typography"] = new CSSObject()
                        {
                            Display = "inline",
                        },
                    },
                    [$"{componentCls}-selection-placeholder"] = new CSSObject()
                    {
                        ["..."] = TextEllipsis,
                        Flex = 1,
                        Color = token.ColorTextPlaceholder,
                        PointerEvents = "none",
                    },
                    [$"{componentCls}-arrow"] = new CSSObject()
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
                        [iconCls] = new CSSObject()
                        {
                            VerticalAlign = "top",
                            Transition = @$"transform {token.MotionDurationSlow}",
                            ["> svg"] = new CSSObject()
                            {
                                VerticalAlign = "top",
                            },
                            [$"&:not({componentCls}-suffix)"] = new CSSObject()
                            {
                                PointerEvents = "auto",
                            },
                        },
                        [$"{componentCls}-disabled &"] = new CSSObject()
                        {
                            Cursor = "not-allowed",
                        },
                        ["> *:not(:last-child)"] = new CSSObject()
                        {
                            MarginInlineEnd = 8,
                        },
                    },
                    [$"{componentCls}-clear"] = new CSSObject()
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
                        Background = token.ClearBg,
                        Cursor = "pointer",
                        Opacity = 0,
                        Transition = @$"color {token.MotionDurationMid} ease, opacity {token.MotionDurationSlow} ease",
                        TextRendering = "auto",
                        ["&:before"] = new CSSObject()
                        {
                            Display = "block",
                        },
                        ["&:hover"] = new CSSObject()
                        {
                            Color = token.ColorTextTertiary,
                        },
                    },
                    ["&:hover"] = new CSSObject()
                    {
                        [$"{componentCls}-clear"] = new CSSObject()
                        {
                            Opacity = 1,
                        },
                    },
                },
                [$"{componentCls}-has-feedback"] = new CSSObject()
                {
                    [$"{componentCls}-clear"] = new CSSObject()
                    {
                        InsetInlineEnd = inputPaddingHorizontalBase + token.FontSize + token.PaddingXS,
                    },
                },
            };
        }

        public CSSInterpolation GenSelectStyle(SelectToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$"&-borderless {componentCls}-selector"] = new CSSObject()
                        {
                            BackgroundColor = @$"transparent !important",
                            BorderColor = @$"transparent !important",
                            BoxShadow = @$"none !important",
                        },
                        [$"&{componentCls}-in-form-item"] = new CSSObject()
                        {
                            Width = "100%",
                        },
                    },
                },
                GenBaseStyle(token),
                GenSingleStyle(token),
                GenMultipleStyle(token),
                GenDropdownStyle(token),
                new CSSObject()
                {
                    [$"{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
                GenStatusStyle(
                    componentCls,
                    MergeToken(
                        token,
                        new SelectToken()
                        {
                            BorderHoverColor = token.ColorPrimaryHover,
                            BorderActiveColor = token.ColorPrimary,
                            OutlineColor = token.ControlOutline,
                        })),
                GenStatusStyle(
                    $"{componentCls}-status-error",
                    MergeToken(
                        token,
                        new SelectToken()
                        {
                            BorderHoverColor = token.ColorErrorHover,
                            BorderActiveColor = token.ColorError,
                            OutlineColor = token.ColorErrorOutline,
                        }),
                    true),
                GenStatusStyle(
                    $"{componentCls}-status-warning",
                    MergeToken(
                        token,
                        new SelectToken()
                        {
                            BorderHoverColor = token.ColorWarningHover,
                            BorderActiveColor = token.ColorWarning,
                            OutlineColor = token.ColorWarningOutline,
                        }),
                    true),
                GenCompactItemStyle(
                    token,
                    new CompactItemOptions()
                    {
                        BorderElCls = @$"{componentCls}-selector",
                        FocusElCls = @$"{componentCls}-focused",
                    }),
            };
        }

        public UseComponentStyleResult ExportDefault()
        {
            return GenComponentStyleHook(
                "Select",
                (token) =>
                {
                    var rootPrefixCls = token.RootPrefixCls;
                    var selectToken = MergeToken(
                        token,
                        new SelectToken()
                        {
                            RootPrefixCls = rootPrefixCls,
                            InputPaddingHorizontalBase = token.PaddingSM - 1,
                            MultipleSelectItemHeight = token.MultipleItemHeight,
                            SelectHeight = token.ControlHeight,
                        });
                    return new CSSInterpolation[]
                    {
                        GenSelectStyle(selectToken),
                    };
                },
                (token) =>
                {
                    var fontSize = token.FontSize;
                    var lineHeight = token.LineHeight;
                    var controlHeight = token.ControlHeight;
                    var controlPaddingHorizontal = token.ControlPaddingHorizontal;
                    var zIndexPopupBase = token.ZIndexPopupBase;
                    var colorText = token.ColorText;
                    var fontWeightStrong = token.FontWeightStrong;
                    var controlItemBgActive = token.ControlItemBgActive;
                    var controlItemBgHover = token.ControlItemBgHover;
                    var colorBgContainer = token.ColorBgContainer;
                    var colorFillSecondary = token.ColorFillSecondary;
                    var controlHeightLG = token.ControlHeightLG;
                    var controlHeightSM = token.ControlHeightSM;
                    var colorBgContainerDisabled = token.ColorBgContainerDisabled;
                    var colorTextDisabled = token.ColorTextDisabled;
                    return new SelectToken()
                    {
                        ZIndexPopup = zIndexPopupBase + 50,
                        OptionSelectedColor = colorText,
                        OptionSelectedFontWeight = fontWeightStrong,
                        OptionSelectedBg = controlItemBgActive,
                        OptionActiveBg = controlItemBgHover,
                        OptionPadding = $"{(controlHeight - fontSize * lineHeight) / 2}px {controlPaddingHorizontal}px",
                        OptionFontSize = fontSize,
                        OptionLineHeight = lineHeight,
                        OptionHeight = controlHeight,
                        SelectorBg = colorBgContainer,
                        ClearBg = colorBgContainer,
                        SingleItemHeightLG = controlHeightLG,
                        MultipleItemBg = colorFillSecondary,
                        MultipleItemBorderColor = "transparent",
                        MultipleItemHeight = controlHeightSM,
                        MultipleItemHeightLG = controlHeight,
                        MultipleSelectorBgDisabled = colorBgContainerDisabled,
                        MultipleItemColorDisabled = colorTextDisabled,
                        MultipleItemBorderColorDisabled = "transparent",
                    };
                });
        }

        public CSSObject GenItemStyle(SelectToken token)
        {
            var optionHeight = token.OptionHeight;
            var optionFontSize = token.OptionFontSize;
            var optionLineHeight = token.OptionLineHeight;
            var optionPadding = token.OptionPadding;
            return new CSSObject()
            {
                Position = "relative",
                Display = "block",
                MinHeight = optionHeight,
                Padding = optionPadding,
                Color = token.ColorText,
                FontWeight = "normal",
                FontSize = optionFontSize,
                LineHeight = optionLineHeight,
                BoxSizing = "border-box",
            };
        }

        public CSSInterpolation GenDropdownStyle(SelectToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var selectItemCls = @$"{componentCls}-item";
            var slideUpEnterActive = @$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active";
            var slideUpAppearActive = @$"&{antCls}-slide-up-appear{antCls}-slide-up-appear-active";
            var slideUpLeaveActive = @$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active";
            var dropdownPlacementCls = @$"{componentCls}-dropdown-placement-";
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-dropdown"] = new CSSObject()
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
                        [$"{slideUpEnterActive}{dropdownPlacementCls}bottomLeft,{slideUpAppearActive}{dropdownPlacementCls}bottomLeft"] = new CSSObject()
                        {
                            AnimationName = SlideUpIn,
                        },
                        [$"{slideUpEnterActive}{dropdownPlacementCls}topLeft,{slideUpAppearActive}{dropdownPlacementCls}topLeft,{slideUpEnterActive}{dropdownPlacementCls}topRight,{slideUpAppearActive}{dropdownPlacementCls}topRight"] = new CSSObject()
                        {
                            AnimationName = SlideDownIn,
                        },
                        [$"{slideUpLeaveActive}{dropdownPlacementCls}bottomLeft"] = new CSSObject()
                        {
                            AnimationName = SlideUpOut,
                        },
                        [$"{slideUpLeaveActive}{dropdownPlacementCls}topLeft,{slideUpLeaveActive}{dropdownPlacementCls}topRight"] = new CSSObject()
                        {
                            AnimationName = SlideDownOut,
                        },
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"{selectItemCls}"] = new CSSObject()
                        {
                            ["..."] = GenItemStyle(token),
                            Cursor = "pointer",
                            Transition = @$"background {token.MotionDurationSlow} ease",
                            BorderRadius = token.BorderRadiusSM,
                            ["&-group"] = new CSSObject()
                            {
                                Color = token.ColorTextDescription,
                                FontSize = token.FontSizeSM,
                                Cursor = "default",
                            },
                            ["&-option"] = new CSSObject()
                            {
                                Display = "flex",
                                ["&-content"] = new CSSObject()
                                {
                                    Flex = "auto",
                                    ["..."] = TextEllipsis,
                                },
                                ["&-state"] = new CSSObject()
                                {
                                    Flex = "none",
                                    Display = "flex",
                                    AlignItems = "center",
                                },
                                [$"&-active:not({selectItemCls}-option-disabled)"] = new CSSObject()
                                {
                                    BackgroundColor = token.OptionActiveBg,
                                },
                                [$"&-selected:not({selectItemCls}-option-disabled)"] = new CSSObject()
                                {
                                    Color = token.OptionSelectedColor,
                                    FontWeight = token.OptionSelectedFontWeight,
                                    BackgroundColor = token.OptionSelectedBg,
                                    [$"{selectItemCls}-option-state"] = new CSSObject()
                                    {
                                        Color = token.ColorPrimary,
                                    },
                                },
                                ["&-disabled"] = new CSSObject()
                                {
                                    [$"&{selectItemCls}-option-selected"] = new CSSObject()
                                    {
                                        BackgroundColor = token.ColorBgContainerDisabled,
                                    },
                                    Color = token.ColorTextDisabled,
                                    Cursor = "not-allowed",
                                },
                                ["&-grouped"] = new CSSObject()
                                {
                                    PaddingInlineStart = token.ControlPaddingHorizontal * 2,
                                },
                            },
                        },
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                        },
                    },
                },
                InitSlideMotion(token, "slide-up"),
                InitSlideMotion(token, "slide-down"),
                InitMoveMotion(token, "move-up"),
                InitMoveMotion(token, "move-down"),
            };
        }

        public (double, double) GetSelectItemStyle(SelectToken args)
        {
            var multipleSelectItemHeight = args.MultipleSelectItemHeight;
            var selectHeight = args.SelectHeight;
            var borderWidth = args.LineWidth;
            var selectItemDist = (selectHeight - multipleSelectItemHeight) / 2 - borderWidth;
            var selectItemMargin = Math.Ceiling(selectItemDist / 2);
            return (selectItemDist, selectItemMargin);
        }

        public CSSObject GenSizeStyle(SelectToken token, string suffix = null)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var selectOverflowPrefixCls = @$"{componentCls}-selection-overflow";
            var selectItemHeight = token.MultipleSelectItemHeight;
            var (selectItemDist, selectItemMargin) = GetSelectItemStyle(token);
            var suffixCls = !string.IsNullOrEmpty(suffix) ? $"{componentCls}-{suffix}" : "";
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
                        Height = "100%",
                        Padding = @$"{selectItemDist - FIXED_ITEM_MARGIN}px {FIXED_ITEM_MARGIN * 2}px",
                        BorderRadius = token.BorderRadius,
                        [$"{componentCls}-show-search&"] = new CSSObject()
                        {
                            Cursor = "text",
                        },
                        [$"{componentCls}-disabled&"] = new CSSObject()
                        {
                            Background = token.MultipleSelectorBgDisabled,
                            Cursor = "not-allowed",
                        },
                        ["&:after"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = 0,
                            Margin = $"{FIXED_ITEM_MARGIN} px 0",
                            LineHeight = @$"{selectItemHeight}px",
                            Visibility = "hidden",
                            Content = "\"\\\\a0\"",
                        },
                    },
                    [$"&{componentCls}-show-arrow{componentCls}-selector,&{componentCls}-allow-clear{componentCls}-selector"] = new CSSObject()
                    {
                        PaddingInlineEnd = token.FontSizeIcon + token.ControlPaddingHorizontal,
                    },
                    [$"{componentCls}-selection-item"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignSelf = "center",
                        Flex = "none",
                        BoxSizing = "border-box",
                        MaxWidth = "100%",
                        Height = selectItemHeight,
                        MarginTop = FIXED_ITEM_MARGIN,
                        MarginBottom = FIXED_ITEM_MARGIN,
                        LineHeight = @$"{selectItemHeight - token.LineWidth * 2}px",
                        Background = token.MultipleItemBg,
                        Border = @$"{token.LineWidth}px {token.LineType} {token.MultipleItemBorderColor}",
                        BorderRadius = token.BorderRadiusSM,
                        Cursor = "default",
                        Transition = @$"font-size {token.MotionDurationSlow}, line-height {token.MotionDurationSlow}, height {token.MotionDurationSlow}",
                        MarginInlineEnd = FIXED_ITEM_MARGIN * 2,
                        PaddingInlineStart = token.PaddingXS,
                        PaddingInlineEnd = token.PaddingXS / 2,
                        [$"{componentCls}-disabled&"] = new CSSObject()
                        {
                            Color = token.MultipleItemColorDisabled,
                            BorderColor = token.MultipleItemBorderColorDisabled,
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
                            Display = "inline-flex",
                            AlignItems = "center",
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
                    [$"{selectOverflowPrefixCls}-item-suffix"] = new CSSObject()
                    {
                        Height = "100%",
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
                    [$"{componentCls}-selection-placeholder"] = new CSSObject()
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
                new SelectToken()
                {
                    SelectHeight = token.ControlHeightSM,
                    MultipleSelectItemHeight = token.ControlHeightXS,
                    BorderRadius = token.BorderRadiusSM,
                    BorderRadiusSM = token.BorderRadiusXS,
                });
            var largeToken = MergeToken(
                token,
                new SelectToken()
                {
                    FontSize = token.FontSizeLG,
                    SelectHeight = token.ControlHeightLG,
                    MultipleSelectItemHeight = token.MultipleItemHeightLG,
                    BorderRadius = token.BorderRadiusLG,
                    BorderRadiusSM = token.BorderRadius,
                });
            var (selectItemDist, smSelectItemMargin) = GetSelectItemStyle(token);
            return new CSSInterpolation[]
            {
                GenSizeStyle(token),
                GenSizeStyle(smallToken, "sm"),
                new CSSObject()
                {
                    [$"{componentCls}-multiple{componentCls}-sm"] = new CSSObject()
                    {
                        [$"{componentCls}-selection-placeholder"] = new CSSObject()
                        {
                            InsetInline = token.ControlPaddingHorizontalSM - token.LineWidth,
                        },
                        [$"{componentCls}-selection-search"] = new CSSObject()
                        {
                            MarginInlineStart = smSelectItemMargin,
                        },
                    },
                },
                GenSizeStyle(largeToken, "lg"),
            };
        }

        public CSSObject GenSingleSizeStyle(SelectToken token, string suffix = null)
        {
            var componentCls = token.ComponentCls;
            var inputPaddingHorizontalBase = token.InputPaddingHorizontalBase;
            var borderRadius = token.BorderRadius;
            var selectHeightWithoutBorder = token.ControlHeight - token.LineWidth * 2;
            var selectionItemPadding = Math.Ceiling(token.FontSize * 1.25);
            var suffixCls = !string.IsNullOrEmpty(suffix) ? $"{componentCls}-{suffix}" : "";
            return new CSSObject()
            {
                [$"{componentCls}-single{suffixCls}"] = new CSSObject()
                {
                    FontSize = token.FontSize,
                    Height = token.ControlHeight,
                    [$"{componentCls}-selector"] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token, true),
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
                                WebkitAppearance = "textfield",
                            },
                        },
                        [$"{componentCls}-selection-item,{componentCls}-selection-placeholder"] = new CSSObject()
                        {
                            Padding = 0,
                            LineHeight = @$"{selectHeightWithoutBorder}px",
                            Transition = @$"all {token.MotionDurationSlow}, visibility 0s",
                            AlignSelf = "center",
                        },
                        [$"{componentCls}-selection-placeholder"] = new CSSObject()
                        {
                            Transition = "none",
                            PointerEvents = "none",
                        },
                        [$"&:after,{componentCls}-selection-item:empty:after,{componentCls}-selection-placeholder:empty:after"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = 0,
                            Visibility = "hidden",
                            Content = "\"\\\\a0\"",
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
                            Height = "100%",
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
            return new CSSInterpolation[]
            {
                GenSingleSizeStyle(token),
                GenSingleSizeStyle(
                    MergeToken(
                        token,
                        new SelectToken()
                        {
                            ControlHeight = token.ControlHeightSM,
                            BorderRadius = token.BorderRadiusSM,
                        }),
                    "sm"),
                new CSSObject()
                {
                    [$"{componentCls}-single{componentCls}-sm"] = new CSSObject()
                    {
                        [$"&:not({componentCls}-customize-input)"] = new CSSObject()
                        {
                            [$"{componentCls}-selection-search"] = new CSSObject()
                            {
                                InsetInlineStart = inputPaddingHorizontalSM,
                                InsetInlineEnd = inputPaddingHorizontalSM,
                            },
                            [$"{componentCls}-selector"] = new CSSObject()
                            {
                                Padding = @$"0 {inputPaddingHorizontalSM}px",
                            },
                            [$"&{componentCls}-show-arrow {componentCls}-selection-search"] = new CSSObject()
                            {
                                InsetInlineEnd = inputPaddingHorizontalSM + token.FontSize * 1.5,
                            },
                            [$"&{componentCls}-show-arrow{componentCls}-selection-item,&{componentCls}-show-arrow{componentCls}-selection-placeholder"] = new CSSObject()
                            {
                                PaddingInlineEnd = token.FontSize * 1.5,
                            },
                        },
                    },
                },
                GenSingleSizeStyle(
                    MergeToken(
                        token,
                        new SelectToken()
                        {
                            ControlHeight = token.SingleItemHeightLG,
                            FontSize = token.FontSizeLG,
                            BorderRadius = token.BorderRadiusLG,
                        }),
                    "lg"),
            };
        }

    }
}
