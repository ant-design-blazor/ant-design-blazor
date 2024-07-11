using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.InputStyle;

namespace AntDesign
{
    public partial class MentionsToken
    {
        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }

        public double DropdownHeight
        {
            get => (double)_tokens["dropdownHeight"];
            set => _tokens["dropdownHeight"] = value;
        }

        public double ControlItemWidth
        {
            get => (double)_tokens["controlItemWidth"];
            set => _tokens["controlItemWidth"] = value;
        }

    }

    public partial class MentionsToken : InputToken
    {
    }

    public partial class MentionsStyle
    {
        public static CSSObject GenMentionsStyle(MentionsToken token)
        {
            var componentCls = token.ComponentCls;
            var colorTextDisabled = token.ColorTextDisabled;
            var controlItemBgHover = token.ControlItemBgHover;
            var controlPaddingHorizontal = token.ControlPaddingHorizontal;
            var colorText = token.ColorText;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineHeight = token.LineHeight;
            var controlHeight = token.ControlHeight;
            var paddingInline = token.PaddingInline;
            var paddingBlock = token.PaddingBlock;
            var fontSize = token.FontSize;
            var colorBgElevated = token.ColorBgElevated;
            var paddingXXS = token.PaddingXXS;
            var borderRadius = token.BorderRadius;
            var borderRadiusLG = token.BorderRadiusLG;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var itemPaddingVertical = Math.Round((token.ControlHeight - token.FontSize * token.LineHeight) / 2);
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
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
                    ["&-disabled"] = new CSSObject()
                    {
                        ["> textarea"] = new CSSObject()
                        {
                            ["..."] = GenDisabledStyle(token)
                        },
                    },
                    [$"&-affix-wrapper {componentCls}-suffix"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = paddingInline,
                        Bottom = 0,
                        ZIndex = 1,
                        Display = "inline-flex",
                        AlignItems = "center",
                        Margin = "auto",
                    },
                    [$"> textarea, {componentCls}-measure"] = new CSSObject()
                    {
                        Color = colorText,
                        BoxSizing = "border-box",
                        MinHeight = controlHeight - 2,
                        Margin = 0,
                        Padding = @$"{paddingBlock}px {paddingInline}px",
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
                    ["> textarea"] = new CSSObject()
                    {
                        Width = "100%",
                        Border = "none",
                        Outline = "none",
                        Resize = "none",
                        BackgroundColor = "inherit",
                        ["..."] = GenPlaceholderStyle(token.ColorTextPlaceholder)
                    },
                    [$"{componentCls}-measure"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = 0,
                        Bottom = 0,
                        InsetInlineStart = 0,
                        ZIndex = -1,
                        Color = "transparent",
                        PointerEvents = "none",
                        ["> span"] = new CSSObject()
                        {
                            Display = "inline-block",
                            MinHeight = "1em",
                        },
                    },
                    ["&-dropdown"] = new CSSObject()
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
                        ["&-hidden"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"{componentCls}-dropdown-menu"] = new CSSObject()
                        {
                            MaxHeight = token.DropdownHeight,
                            Margin = 0,
                            PaddingInlineStart = 0,
                            Overflow = "auto",
                            ListStyle = "none",
                            Outline = "none",
                            ["&-item"] = new CSSObject()
                            {
                                ["..."] = TextEllipsis,
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
                                ["&:hover"] = new CSSObject()
                                {
                                    BackgroundColor = controlItemBgHover,
                                },
                                ["&-disabled"] = new CSSObject()
                                {
                                    Color = colorTextDisabled,
                                    Cursor = "not-allowed",
                                    ["&:hover"] = new CSSObject()
                                    {
                                        Color = colorTextDisabled,
                                        BackgroundColor = controlItemBgHover,
                                        Cursor = "not-allowed",
                                    },
                                },
                                ["&-selected"] = new CSSObject()
                                {
                                    Color = colorText,
                                    FontWeight = token.FontWeightStrong,
                                    BackgroundColor = controlItemBgHover,
                                },
                                ["&-active"] = new CSSObject()
                                {
                                    BackgroundColor = controlItemBgHover,
                                },
                            },
                        },
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Mentions",
                (token) =>
                {
                    var mentionsToken = MergeToken<MentionsToken>(
                        token,
                        InitInputToken(token));
                    return new CSSInterpolation[]
                    {
                        GenMentionsStyle(mentionsToken),
                    };
                },
                (token) =>
                {
                    return new MentionsToken()
                    {
                        ["..."] = InitComponentToken(token),
                        DropdownHeight = 250,
                        ControlItemWidth = 100,
                        ZIndexPopup = token.ZIndexPopupBase + 50,
                    };
                });
        }

    }

}
