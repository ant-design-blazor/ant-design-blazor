using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class SwitchToken : TokenWithCommonCls
    {
        public int SwitchMinWidth { get; set; }

        public int SwitchHeight { get; set; }

        public string SwitchDuration { get; set; }

        public string SwitchColor { get; set; }

        public int SwitchDisabledOpacity { get; set; }

        public int SwitchInnerMarginMin { get; set; }

        public int SwitchInnerMarginMax { get; set; }

        public int SwitchPadding { get; set; }

        public int SwitchPinSize { get; set; }

        public string SwitchBg { get; set; }

        public int SwitchMinWidthSM { get; set; }

        public int SwitchHeightSM { get; set; }

        public int SwitchInnerMarginMinSM { get; set; }

        public int SwitchInnerMarginMaxSM { get; set; }

        public int SwitchPinSizeSM { get; set; }

        public string SwitchHandleShadow { get; set; }

        public int SwitchLoadingIconSize { get; set; }

        public string SwitchLoadingIconColor { get; set; }

        public string SwitchHandleActiveInset { get; set; }

    }

    public partial class Switch
    {
        public Unknown_1 GenSwitchSmallStyle(Unknown_6 token)
        {
            var componentCls = token.ComponentCls;
            var switchInnerCls = @$"{componentCls}-inner";
            return new Unknown_7()
            {
                [componentCls] = new Unknown_8()
                {
                    [$"&{componentCls}-small"] = new Unknown_9()
                    {
                        MinWidth = token.SwitchMinWidthSM,
                        Height = token.SwitchHeightSM,
                        LineHeight = @$"{token.SwitchHeightSM}px",
                        [$"{componentCls}-inner"] = new Unknown_10()
                        {
                            PaddingInlineStart = token.SwitchInnerMarginMaxSM,
                            PaddingInlineEnd = token.SwitchInnerMarginMinSM,
                            [$"{switchInnerCls}-checked"] = new Unknown_11()
                            {
                                MarginInlineStart = @$"calc(-100% + {
              token.SwitchPinSizeSM + token.SwitchPadding * 2
            }px - {token.SwitchInnerMarginMaxSM * 2}px)",
                                MarginInlineEnd = @$"calc(100% - {token.SwitchPinSizeSM + token.SwitchPadding * 2}px + {
              token.SwitchInnerMarginMaxSM * 2
            }px)",
                            },
                            [$"{switchInnerCls}-unchecked"] = new Unknown_12()
                            {
                                MarginTop = -token.SwitchHeightSM,
                                MarginInlineStart = 0,
                                MarginInlineEnd = 0,
                            },
                        },
                        [$"{componentCls}-handle"] = new Unknown_13()
                        {
                            Width = token.SwitchPinSizeSM,
                            Height = token.SwitchPinSizeSM,
                        },
                        [$"{componentCls}-loading-icon"] = new Unknown_14()
                        {
                            Top = (token.SwitchPinSizeSM - token.SwitchLoadingIconSize) / 2,
                            FontSize = token.SwitchLoadingIconSize,
                        },
                        [$"&{componentCls}-checked"] = new Unknown_15()
                        {
                            [$"{componentCls}-inner"] = new Unknown_16()
                            {
                                PaddingInlineStart = token.SwitchInnerMarginMinSM,
                                PaddingInlineEnd = token.SwitchInnerMarginMaxSM,
                                [$"{switchInnerCls}-checked"] = new Unknown_17()
                                {
                                    MarginInlineStart = 0,
                                    MarginInlineEnd = 0,
                                },
                                [$"{switchInnerCls}-unchecked"] = new Unknown_18()
                                {
                                    MarginInlineStart = @$"calc(100% - {
                token.SwitchPinSizeSM + token.SwitchPadding * 2
              }px + {token.SwitchInnerMarginMaxSM * 2}px)",
                                    MarginInlineEnd = @$"calc(-100% + {
                token.SwitchPinSizeSM + token.SwitchPadding * 2
              }px - {token.SwitchInnerMarginMaxSM * 2}px)",
                                },
                            },
                            [$"{componentCls}-handle"] = new Unknown_19()
                            {
                                InsetInlineStart = @$"calc(100% - {token.SwitchPinSizeSM + token.SwitchPadding}px)",
                            },
                        },
                        [$"&:not({componentCls}-disabled):active"] = new Unknown_20()
                        {
                            [$"&:not({componentCls}-checked) {switchInnerCls}"] = new Unknown_21()
                            {
                                [$"{switchInnerCls}-unchecked"] = new Unknown_22()
                                {
                                    MarginInlineStart = token.MarginXXS / 2,
                                    MarginInlineEnd = -token.MarginXXS / 2,
                                },
                            },
                            [$"&{componentCls}-checked {switchInnerCls}"] = new Unknown_23()
                            {
                                [$"{switchInnerCls}-checked"] = new Unknown_24()
                                {
                                    MarginInlineStart = -token.MarginXXS / 2,
                                    MarginInlineEnd = token.MarginXXS / 2,
                                },
                            },
                        },
                    },
                },
            };
        }

        public Unknown_2 GenSwitchLoadingStyle(Unknown_25 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_26()
            {
                [componentCls] = new Unknown_27()
                {
                    [$"{componentCls}-loading-icon{token.IconCls}"] = new Unknown_28()
                    {
                        Position = "relative",
                        Top = (token.SwitchPinSize - token.FontSize) / 2,
                        Color = token.SwitchLoadingIconColor,
                        VerticalAlign = "top",
                    },
                    [$"&{componentCls}-checked {componentCls}-loading-icon"] = new Unknown_29()
                    {
                        Color = token.SwitchColor,
                    },
                },
            };
        }

        public Unknown_3 GenSwitchHandleStyle(Unknown_30 token)
        {
            var componentCls = token.ComponentCls;
            var motion = token.Motion;
            var switchHandleCls = @$"{componentCls}-handle";
            return new Unknown_31()
            {
                [componentCls] = new Unknown_32()
                {
                    [switchHandleCls] = new Unknown_33()
                    {
                        Position = "absolute",
                        Top = token.SwitchPadding,
                        InsetInlineStart = token.SwitchPadding,
                        Width = token.SwitchPinSize,
                        Height = token.SwitchPinSize,
                        Transition = @$"all {token.SwitchDuration} ease-in-out",
                        ["&::before"] = new Unknown_34()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineEnd = 0,
                            Bottom = 0,
                            InsetInlineStart = 0,
                            BackgroundColor = token.ColorWhite,
                            BorderRadius = token.SwitchPinSize / 2,
                            BoxShadow = token.SwitchHandleShadow,
                            Transition = @$"all {token.SwitchDuration} ease-in-out",
                            Content = "\"\"",
                        },
                    },
                    [$"&{componentCls}-checked {switchHandleCls}"] = new Unknown_35()
                    {
                        InsetInlineStart = @$"calc(100% - {token.SwitchPinSize + token.SwitchPadding}px)",
                    },
                    [$"&:not({componentCls}-disabled):active"] = motion
        ? {
            [@$"{switchHandleCls}::before"]: {
              insetInlineEnd: token.SwitchHandleActiveInset,
              insetInlineStart: 0,
            },

            [`&{componentCls}-checked {switchHandleCls}::before`]: {
              insetInlineEnd: 0,
              insetInlineStart: token.SwitchHandleActiveInset,
            },
          }
        : /* istanbul ignore next */
          {},
                },
            };
        }

        public Unknown_4 GenSwitchInnerStyle(Unknown_36 token)
        {
            var componentCls = token.ComponentCls;
            var switchInnerCls = @$"{componentCls}-inner";
            return new Unknown_37()
            {
                [componentCls] = new Unknown_38()
                {
                    [switchInnerCls] = new Unknown_39()
                    {
                        Display = "block",
                        Overflow = "hidden",
                        BorderRadius = 100,
                        Height = "100%",
                        PaddingInlineStart = token.SwitchInnerMarginMax,
                        PaddingInlineEnd = token.SwitchInnerMarginMin,
                        Transition = @$"padding-inline-start {token.SwitchDuration} ease-in-out, padding-inline-end {token.SwitchDuration} ease-in-out",
                        [$"{switchInnerCls}-checked, {switchInnerCls}-unchecked"] = new Unknown_40()
                        {
                            Display = "block",
                            Color = token.ColorTextLightSolid,
                            FontSize = token.FontSizeSM,
                            Transition = @$"margin-inline-start {token.SwitchDuration} ease-in-out, margin-inline-end {token.SwitchDuration} ease-in-out",
                            PointerEvents = "none",
                        },
                        [$"{switchInnerCls}-checked"] = new Unknown_41()
                        {
                            MarginInlineStart = @$"calc(-100% + {token.SwitchPinSize + token.SwitchPadding * 2}px - {
            token.SwitchInnerMarginMax * 2
          }px)",
                            MarginInlineEnd = @$"calc(100% - {token.SwitchPinSize + token.SwitchPadding * 2}px + {
            token.SwitchInnerMarginMax * 2
          }px)",
                        },
                        [$"{switchInnerCls}-unchecked"] = new Unknown_42()
                        {
                            MarginTop = -token.SwitchHeight,
                            MarginInlineStart = 0,
                            MarginInlineEnd = 0,
                        },
                    },
                    [$"&{componentCls}-checked {switchInnerCls}"] = new Unknown_43()
                    {
                        PaddingInlineStart = token.SwitchInnerMarginMin,
                        PaddingInlineEnd = token.SwitchInnerMarginMax,
                        [$"{switchInnerCls}-checked"] = new Unknown_44()
                        {
                            MarginInlineStart = 0,
                            MarginInlineEnd = 0,
                        },
                        [$"{switchInnerCls}-unchecked"] = new Unknown_45()
                        {
                            MarginInlineStart = @$"calc(100% - {token.SwitchPinSize + token.SwitchPadding * 2}px + {
            token.SwitchInnerMarginMax * 2
          }px)",
                            MarginInlineEnd = @$"calc(-100% + {token.SwitchPinSize + token.SwitchPadding * 2}px - {
            token.SwitchInnerMarginMax * 2
          }px)",
                        },
                    },
                    [$"&:not({componentCls}-disabled):active"] = new Unknown_46()
                    {
                        [$"&:not({componentCls}-checked) {switchInnerCls}"] = new Unknown_47()
                        {
                            [$"{switchInnerCls}-unchecked"] = new Unknown_48()
                            {
                                MarginInlineStart = token.SwitchPadding * 2,
                                MarginInlineEnd = -token.SwitchPadding * 2,
                            },
                        },
                        [$"&{componentCls}-checked {switchInnerCls}"] = new Unknown_49()
                        {
                            [$"{switchInnerCls}-checked"] = new Unknown_50()
                            {
                                MarginInlineStart = -token.SwitchPadding * 2,
                                MarginInlineEnd = token.SwitchPadding * 2,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenSwitchStyle(SwitchToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "inline-block",
                    BoxSizing = "border-box",
                    MinWidth = token.SwitchMinWidth,
                    Height = token.SwitchHeight,
                    LineHeight = @$"{token.SwitchHeight}px",
                    VerticalAlign = "middle",
                    Background = token.ColorTextQuaternary,
                    Border = "0",
                    BorderRadius = 100,
                    Cursor = "pointer",
                    Transition = @$"all {token.MotionDurationMid}",
                    UserSelect = "none",
                    [$"&:hover:not({componentCls}-disabled)"] = new CSSObject()
                    {
                        Background = token.ColorTextTertiary,
                    },
                    ["..."] = GenFocusStyle(token),
                    [$"&{componentCls}-checked"] = new CSSObject()
                    {
                        Background = token.SwitchColor,
                        [$"&:hover:not({componentCls}-disabled)"] = new CSSObject()
                        {
                            Background = token.ColorPrimaryHover,
                        },
                    },
                    [$"&{componentCls}-loading, &{componentCls}-disabled"] = new CSSObject()
                    {
                        Cursor = "not-allowed",
                        Opacity = token.SwitchDisabledOpacity,
                        ["*"] = new CSSObject()
                        {
                            BoxShadow = "none",
                            Cursor = "not-allowed",
                        },
                    },
                    [$"&{componentCls}-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                },
            };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_51 token)
        {
            var switchHeight = token.FontSize * token.LineHeight;
            var switchHeightSM = token.ControlHeight / 2;
            var switchPadding = 2;
            var switchPinSize = switchHeight - switchPadding * 2;
            var switchPinSizeSM = switchHeightSM - switchPadding * 2;
            var switchToken = MergeToken(
                token,
                new Unknown_52()
                {
                    SwitchMinWidth = switchPinSize * 2 + switchPadding * 4,
                    SwitchHeight = switchHeight,
                    SwitchDuration = token.MotionDurationMid,
                    SwitchColor = token.ColorPrimary,
                    SwitchDisabledOpacity = token.OpacityLoading,
                    SwitchInnerMarginMin = switchPinSize / 2,
                    SwitchInnerMarginMax = switchPinSize + switchPadding + switchPadding * 2,
                    SwitchPadding = switchPadding,
                    SwitchPinSize = switchPinSize,
                    SwitchBg = token.ColorBgContainer,
                    SwitchMinWidthSM = switchPinSizeSM * 2 + switchPadding * 2,
                    SwitchHeightSM = switchHeightSM,
                    SwitchInnerMarginMinSM = switchPinSizeSM / 2,
                    SwitchInnerMarginMaxSM = switchPinSizeSM + switchPadding + switchPadding * 2,
                    SwitchPinSizeSM = switchPinSizeSM,
                    SwitchHandleShadow = @$"0 2px 4px 0 {new TinyColor("#00230b").setAlpha(0.2).toRgbString()}",
                    SwitchLoadingIconSize = token.FontSizeIcon * 0.75,
                    SwitchLoadingIconColor = @$"rgba(0, 0, 0, {token.OpacityLoading})",
                    SwitchHandleActiveInset = "-30%",
                });
            return new Unknown_53
            {
                GenSwitchStyle(switchToken),
                GenSwitchInnerStyle(switchToken),
                GenSwitchHandleStyle(switchToken),
                GenSwitchLoadingStyle(switchToken),
                GenSwitchSmallStyle(switchToken)
            };
        }

    }

}