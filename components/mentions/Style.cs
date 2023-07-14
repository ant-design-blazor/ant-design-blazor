using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class MentionsToken
    {
        public int ZIndexPopup { get; set; }

        public int DropdownHeight { get; set; }

        public int ControlItemWidth { get; set; }

    }

    public partial class MentionsToken : TokenWithCommonCls
    {
    }

    public partial class Mentions
    {
        public Unknown_1 GenMentionsStyle(Unknown_4 token)
        {
            var componentCls = token.ComponentCls;
            var colorTextDisabled = token.ColorTextDisabled;
            var controlItemBgHover = token.ControlItemBgHover;
            var controlPaddingHorizontal = token.ControlPaddingHorizontal;
            var colorText = token.ColorText;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineHeight = token.LineHeight;
            var controlHeight = token.ControlHeight;
            var inputPaddingHorizontal = token.InputPaddingHorizontal;
            var inputPaddingVertical = token.InputPaddingVertical;
            var fontSize = token.FontSize;
            var colorBgElevated = token.ColorBgElevated;
            var paddingXXS = token.PaddingXXS;
            var borderRadius = token.BorderRadius;
            var borderRadiusLG = token.BorderRadiusLG;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var itemPaddingVertical = Math.Round((token.ControlHeight - token.FontSize * token.LineHeight) / 2);
            return new Unknown_5()
            {
                [componentCls] = new Unknown_6()
                {
                    ["..."] = ResetComponent(token),
                    ["..."] = GenBasicInputStyle(token),
                    Position = "relative",
                    Display = "inline-block",
                    Height = "auto",
                    Padding = 0,
                    Overflow = "hidden",
                    LineHeight = lineHeight,
                    WhiteSpace = "pre-wrap",
                    VerticalAlign = "bottom",
                    ["..."] = GenStatusStyle(token, componentCls),
                    ["&-disabled"] = new Unknown_7()
                    {
                        ["> textarea"] = new Unknown_8()
                        {
                            ["..."] = GenDisabledStyle(token)
                        },
                    },
                    ["&-focused"] = new Unknown_9()
                    {
                        ["..."] = GenActiveStyle(token)
                    },
                    [$"&-affix-wrapper {componentCls}-suffix"] = new Unknown_10()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = inputPaddingHorizontal,
                        Bottom = 0,
                        ZIndex = 1,
                        Display = "inline-flex",
                        AlignItems = "center",
                        Margin = "auto",
                    },
                    [$"> textarea, {componentCls}-measure"] = new Unknown_11()
                    {
                        Color = colorText,
                        BoxSizing = "border-box",
                        MinHeight = controlHeight - 2,
                        Margin = 0,
                        Padding = @$"{inputPaddingVertical}px {inputPaddingHorizontal}px",
                        Overflow = "inherit",
                        OverflowX = "hidden",
                        OverflowY = "auto",
                        FontWeight = "inherit",
                        FontSize = "inherit",
                        FontFamily = "inherit",
                        FontStyle = "inherit",
                        FontVariant = "inherit",
                        FontSizeAdjust = "inherit",
                        FontStretch = "inherit",
                        LineHeight = "inherit",
                        Direction = "inherit",
                        LetterSpacing = "inherit",
                        WhiteSpace = "inherit",
                        TextAlign = "inherit",
                        VerticalAlign = "top",
                        WordWrap = "break-word",
                        WordBreak = "inherit",
                        TabSize = "inherit",
                    },
                    ["> textarea"] = new Unknown_12()
                    {
                        Width = "100%",
                        Border = "none",
                        Outline = "none",
                        Resize = "none",
                        BackgroundColor = "inherit",
                        ["..."] = GenPlaceholderStyle(token.ColorTextPlaceholder)
                    },
                    [$"{componentCls}-measure"] = new Unknown_13()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = 0,
                        Bottom = 0,
                        InsetInlineStart = 0,
                        ZIndex = -1,
                        Color = "transparent",
                        PointerEvents = "none",
                        ["> span"] = new Unknown_14()
                        {
                            Display = "inline-block",
                            MinHeight = "1em",
                        },
                    },
                    ["&-dropdown"] = new Unknown_15()
                    {
                        ["..."] = ResetComponent(token),
                        Position = "absolute",
                        Top = -9999,
                        InsetInlineStart = -9999,
                        ZIndex = token.ZIndexPopup,
                        BoxSizing = "border-box",
                        FontSize = fontSize,
                        FontVariant = "initial",
                        Padding = paddingXXS,
                        BackgroundColor = colorBgElevated,
                        BorderRadius = borderRadiusLG,
                        Outline = "none",
                        BoxShadow = boxShadowSecondary,
                        ["&-hidden"] = new Unknown_16()
                        {
                            Display = "none",
                        },
                        [$"{componentCls}-dropdown-menu"] = new Unknown_17()
                        {
                            MaxHeight = token.DropdownHeight,
                            Margin = 0,
                            PaddingInlineStart = 0,
                            Overflow = "auto",
                            ListStyle = "none",
                            Outline = "none",
                            ["&-item"] = new Unknown_18()
                            {
                                ["..."] = textEllipsis,
                                Position = "relative",
                                Display = "block",
                                MinWidth = token.ControlItemWidth,
                                Padding = @$"{itemPaddingVertical}px {controlPaddingHorizontal}px",
                                Color = colorText,
                                BorderRadius = borderRadius,
                                FontWeight = "normal",
                                LineHeight = lineHeight,
                                Cursor = "pointer",
                                Transition = @$"background {motionDurationSlow} ease",
                                ["&:hover"] = new Unknown_19()
                                {
                                    BackgroundColor = controlItemBgHover,
                                },
                                ["&-disabled"] = new Unknown_20()
                                {
                                    Color = colorTextDisabled,
                                    Cursor = "not-allowed",
                                    ["&:hover"] = new Unknown_21()
                                    {
                                        Color = colorTextDisabled,
                                        BackgroundColor = controlItemBgHover,
                                        Cursor = "not-allowed",
                                    },
                                },
                                ["&-selected"] = new Unknown_22()
                                {
                                    Color = colorText,
                                    FontWeight = token.FontWeightStrong,
                                    BackgroundColor = controlItemBgHover,
                                },
                                ["&-active"] = new Unknown_23()
                                {
                                    BackgroundColor = controlItemBgHover,
                                },
                            },
                        },
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_24 token)
        {
            var mentionsToken = InitInputToken(token);
            return new Unknown_25 { GenMentionsStyle(mentionsToken) };
        }

        public Unknown_3 GenComponentStyleHook(Unknown_26 token)
        {
            return new Unknown_27()
            {
                DropdownHeight = 250,
                ControlItemWidth = 100,
                ZIndexPopup = token.ZIndexPopupBase + 50,
            };
        }

    }

}