using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.DatePickerStyle;

namespace AntDesign
{
    public partial class CalendarToken : PickerPanelToken
    {
        public double YearControlWidth
        {
            get => (double)_tokens["yearControlWidth"];
            set => _tokens["yearControlWidth"] = value;
        }

        public double MonthControlWidth
        {
            get => (double)_tokens["monthControlWidth"];
            set => _tokens["monthControlWidth"] = value;
        }

        public double MiniContentHeight
        {
            get => (double)_tokens["miniContentHeight"];
            set => _tokens["miniContentHeight"] = value;
        }

        public string FullBg
        {
            get => (string)_tokens["fullBg"];
            set => _tokens["fullBg"] = value;
        }

        public string FullPanelBg
        {
            get => (string)_tokens["fullPanelBg"];
            set => _tokens["fullPanelBg"] = value;
        }

        public string ItemActiveBg
        {
            get => (string)_tokens["itemActiveBg"];
            set => _tokens["itemActiveBg"] = value;
        }

    }

    public partial class CalendarToken
    {
        public string CalendarCls
        {
            get => (string)_tokens["calendarCls"];
            set => _tokens["calendarCls"] = value;
        }

        public double DateValueHeight
        {
            get => (double)_tokens["dateValueHeight"];
            set => _tokens["dateValueHeight"] = value;
        }

        public double WeekHeight
        {
            get => (double)_tokens["weekHeight"];
            set => _tokens["weekHeight"] = value;
        }

        public double DateContentHeight
        {
            get => (double)_tokens["dateContentHeight"];
            set => _tokens["dateContentHeight"] = value;
        }

    }

    public partial class CalendarStyle
    {
        public static CSSObject GenCalendarStyles(CalendarToken token)
        {
            var calendarCls = token.CalendarCls;
            var componentCls = token.ComponentCls;
            var fullBg = token.FullBg;
            var fullPanelBg = token.FullPanelBg;
            var itemActiveBg = token.ItemActiveBg;
            return new CSSObject()
            {
                [calendarCls] = new CSSObject()
                {
                    ["..."] = GenPanelStyle(token),
                    ["..."] = ResetComponent(token),
                    Background = fullBg,
                    ["&-rtl"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"{calendarCls}-header"] = new CSSObject()
                    {
                        Display = "flex",
                        JustifyContent = "flex-end",
                        Padding = @$"{token.PaddingSM}px 0",
                        [$"{calendarCls}-year-select"] = new CSSObject()
                        {
                            MinWidth = token.YearControlWidth,
                        },
                        [$"{calendarCls}-month-select"] = new CSSObject()
                        {
                            MinWidth = token.MonthControlWidth,
                            MarginInlineStart = token.MarginXS,
                        },
                        [$"{calendarCls}-mode-switch"] = new CSSObject()
                        {
                            MarginInlineStart = token.MarginXS,
                        },
                    },
                },
                [$"{calendarCls} {componentCls}-panel"] = new CSSObject()
                {
                    Background = fullPanelBg,
                    Border = 0,
                    BorderTop = @$"{token.LineWidth}px {token.LineType} {token.ColorSplit}",
                    BorderRadius = 0,
                    [$"{componentCls}-month-panel, {componentCls}-date-panel"] = new CSSObject()
                    {
                        Width = "auto",
                    },
                    [$"{componentCls}-body"] = new CSSObject()
                    {
                        Padding = @$"{token.PaddingXS}px 0",
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Width = "100%",
                    },
                },
                [$"{calendarCls}-mini"] = new CSSObject()
                {
                    BorderRadius = token.BorderRadiusLG,
                    [$"{calendarCls}-header"] = new CSSObject()
                    {
                        PaddingInlineEnd = token.PaddingXS,
                        PaddingInlineStart = token.PaddingXS,
                    },
                    [$"{componentCls}-panel"] = new CSSObject()
                    {
                        BorderRadius = @$"0 0 {token.BorderRadiusLG}px {token.BorderRadiusLG}px",
                    },
                    [$"{componentCls}-content"] = new CSSObject()
                    {
                        Height = token.MiniContentHeight,
                        ["th"] = new CSSObject()
                        {
                            Height = "auto",
                            Padding = 0,
                            LineHeight = @$"{token.WeekHeight}px",
                        },
                    },
                    [$"{componentCls}-cell::before"] = new CSSObject()
                    {
                        PointerEvents = "none",
                    },
                },
                [$"{calendarCls}{calendarCls}-full"] = new CSSObject()
                {
                    [$"{componentCls}-panel"] = new CSSObject()
                    {
                        Display = "block",
                        Width = "100%",
                        TextAlign = "end",
                        Background = fullBg,
                        Border = 0,
                        [$"{componentCls}-body"] = new CSSObject()
                        {
                            ["th, td"] = new CSSObject()
                            {
                                Padding = 0,
                            },
                            ["th"] = new CSSObject()
                            {
                                Height = "auto",
                                PaddingInlineEnd = token.PaddingSM,
                                PaddingBottom = token.PaddingXXS,
                                LineHeight = @$"{token.WeekHeight}px",
                            },
                        },
                    },
                    [$"{componentCls}-cell"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        ["&:hover"] = new CSSObject()
                        {
                            [$"{calendarCls}-date"] = new CSSObject()
                            {
                                Background = token.ControlItemBgHover,
                            },
                        },
                        [$"{calendarCls}-date-today::before"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"&-in-view{componentCls}-cell-selected"] = new CSSObject()
                        {
                            [$"{calendarCls}-date, {calendarCls}-date-today"] = new CSSObject()
                            {
                                Background = itemActiveBg,
                            },
                        },
                        ["&-selected, &-selected:hover"] = new CSSObject()
                        {
                            [$"{calendarCls}-date, {calendarCls}-date-today"] = new CSSObject()
                            {
                                [$"{calendarCls}-date-value"] = new CSSObject()
                                {
                                    Color = token.ColorPrimary,
                                },
                            },
                        },
                    },
                    [$"{calendarCls}-date"] = new CSSObject()
                    {
                        Display = "block",
                        Width = "auto",
                        Height = "auto",
                        Margin = @$"0 {token.MarginXS / 2}px",
                        Padding = @$"{token.PaddingXS / 2}px {token.PaddingXS}px 0",
                        Border = 0,
                        BorderTop = @$"{token.LineWidthBold}px {token.LineType} {token.ColorSplit}",
                        BorderRadius = 0,
                        Transition = @$"background {token.MotionDurationSlow}",
                        ["&-value"] = new CSSObject()
                        {
                            LineHeight = @$"{token.DateValueHeight}px",
                            Transition = @$"color {token.MotionDurationSlow}",
                        },
                        ["&-content"] = new CSSObject()
                        {
                            Position = "static",
                            Width = "auto",
                            Height = token.DateContentHeight,
                            OverflowY = "auto",
                            Color = token.ColorText,
                            LineHeight = token.LineHeight,
                            TextAlign = "start",
                        },
                        ["&-today"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                            [$"{calendarCls}-date-value"] = new CSSObject()
                            {
                                Color = token.ColorText,
                            },
                        },
                    },
                },
                [$"@media only screen and (max-width: {token.ScreenXS}px) "] = new CSSObject()
                {
                    [$"{calendarCls}"] = new CSSObject()
                    {
                        [$"{calendarCls}-header"] = new CSSObject()
                        {
                            Display = "block",
                            [$"{calendarCls}-year-select"] = new CSSObject()
                            {
                                Width = "50%",
                            },
                            [$"{calendarCls}-month-select"] = new CSSObject()
                            {
                                Width = @$"calc(50% - {token.PaddingXS}px)",
                            },
                            [$"{calendarCls}-mode-switch"] = new CSSObject()
                            {
                                Width = "100%",
                                MarginTop = token.MarginXS,
                                MarginInlineStart = 0,
                                ["> label"] = new CSSObject()
                                {
                                    Width = "50%",
                                    TextAlign = "center",
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
                "Calendar",
                (token) =>
                {
                    var calendarCls = @$"{token.ComponentCls}-calendar";
                    var calendarToken = MergeToken(
                        token,
                        // InitPickerPanelToken(token),
                        // InitPanelComponentToken(token),
                        new CalendarToken()
                        {
                            CalendarCls = calendarCls,
                            // PickerCellInnerCls = @$"{token.ComponentCls}-cell-inner",
                            DateValueHeight = token.ControlHeightSM,
                            WeekHeight = token.ControlHeightSM * 0.75,
                            DateContentHeight = (token.FontSizeSM * token.LineHeightSM + token.MarginXS) * 3 + token.LineWidth * 2,
                        });
                    return new CSSInterpolation[]
                    {
                        GenCalendarStyles(calendarToken),
                    };
                },
                (token) =>
                {
                    return new CalendarToken()
                    {
                        FullBg = token.ColorBgContainer,
                        FullPanelBg = token.ColorBgContainer,
                        ItemActiveBg = token.ControlItemBgActive,
                        YearControlWidth = 80,
                        MonthControlWidth = 70,
                        MiniContentHeight = 256,
                    };
                });
        }

    }

}
