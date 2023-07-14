using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class CalendarToken : TokenWithCommonCls
    {
        public int YearControlWidth { get; set; }

        public int MonthControlWidth { get; set; }

        public int MiniContentHeight { get; set; }

    }

    public partial class CalendarToken
    {
        public string CalendarCls { get; set; }

        public string CalendarFullBg { get; set; }

        public string CalendarFullPanelBg { get; set; }

        public string CalendarItemActiveBg { get; set; }

        public int DateValueHeight { get; set; }

        public int WeekHeight { get; set; }

        public int DateContentHeight { get; set; }

    }

    public partial class Calendar
    {
        public CSSObject GenCalendarStyles(CalendarToken token)
        {
            var calendarCls = token.CalendarCls;
            var componentCls = token.ComponentCls;
            var calendarFullBg = token.CalendarFullBg;
            var calendarFullPanelBg = token.CalendarFullPanelBg;
            var calendarItemActiveBg = token.CalendarItemActiveBg;
            return new CSSObject()
            {
                [calendarCls] = new CSSObject()
                {
                    ["..."] = GenPanelStyle(token),
                    ["..."] = ResetComponent(token),
                    Background = calendarFullBg,
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
                    Background = calendarFullPanelBg,
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
                        Background = calendarFullBg,
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
                                Background = calendarItemActiveBg,
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

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var calendarCls = @$"{token.ComponentCls}-calendar";
            var calendarToken = MergeToken(
                initInputToken<FullToken<"Calendar">>(token),
                initPickerPanelToken(token),
                new CalendarToken()
                {
                    CalendarCls = calendarCls,
                    PickerCellInnerCls = @$"{token.ComponentCls}-cell-inner",
                    CalendarFullBg = token.ColorBgContainer,
                    CalendarFullPanelBg = token.ColorBgContainer,
                    CalendarItemActiveBg = token.ControlItemBgActive,
                    DateValueHeight = token.ControlHeightSM,
                    WeekHeight = token.ControlHeightSM * 0.75,
                    DateContentHeight = (token.FontSizeSM * token.LineHeightSM + token.MarginXS) * 3 + token.LineWidth * 2,
                });
            return new CSSInterpolation[] { GenCalendarStyles(calendarToken) };
        }

    }

}