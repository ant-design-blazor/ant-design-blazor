// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DatePickerPanelBase<TValue> : PickerPanelBase
    {
        [CascadingParameter]
        internal IDatePicker DatePicker { get; set; }

        [Parameter]
        public string PrefixCls { get; set; }

        [Parameter]
        public DatePickerType Picker { get; set; }

        [Parameter]
        public bool IsRange { get; set; } = false;

        [Parameter]
        public bool IsCalendar { get; set; } = false;

        [Parameter]
        public bool IsShowHeader { get; set; } = true;

        /// <summary>
        /// Used only by DatePickerWithTimePanel
        /// </summary>
        [Parameter]
        public bool IsShowTime { get; set; } = false;

        [Parameter]
        public DatePickerLocale Locale { get; set; }

        [Parameter]
        public CultureInfo CultureInfo { get; set; }

        [Parameter]
        public Action ClosePanel { get; set; }

        [Parameter]
        public Action<DateTime, int?> ChangePickerValue { get; set; } //nullable int as picker index is no longer needed here unless forced

        [Parameter]
        public Action<DateTime, int> ChangeValue { get; set; }

        [Parameter]
        public Action<DatePickerType, int> ChangePickerType { get; set; }

        [Parameter]
        public Func<int, DateTime> GetIndexPickerValue { get; set; }

        [Parameter]
        public Func<int, DateTime?> GetIndexValue { get; set; }

        [Parameter]
        public Func<DateTime, bool> DisabledDate { get; set; } = null;

        /// <summary>
        /// for Calendar.DateFullCellRender、DatePicker.DateRender
        /// </summary>
        [Parameter]
        public Func<DateTime, DateTime, RenderFragment> DateRender { get; set; }

        /// <summary>
        /// for Calendar.MonthFullCellRender、DatePicker.MonthCellRender
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> MonthCellRender { get; set; }

        /// <summary>
        /// for Calendar.DateCellRender
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> CalendarDateRender { get; set; }

        /// <summary>
        /// for Calendar.MonthCellRender
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> CalendarMonthCellRender { get; set; }

        [Parameter]
        public RenderFragment RenderExtraFooter { get; set; }

        [Parameter]
        public bool Use12Hours { get; set; }

        [Parameter]
        public bool ShowWeek { get; set; }

        protected Dictionary<string, object> GetAttributes() => new Dictionary<string, object>()
            {
                { "PrefixCls", PrefixCls },
                { "Picker", Picker },
                { "Locale", Locale },
                { "CultureInfo", CultureInfo },
                { "ClosePanel", ClosePanel },
                { "ChangePickerValue", ChangePickerValue },
                { "ChangeValue", ChangeValue },
                { "ChangePickerType", ChangePickerType },
                { "GetIndexPickerValue", GetIndexPickerValue },
                { "GetIndexValue", GetIndexValue },
                { "DisabledDate", DisabledDate },
                { "DateRender", DateRender },
                { "MonthCellRender", MonthCellRender },
                { "CalendarDateRender", CalendarDateRender },
                { "CalendarMonthCellRender", CalendarMonthCellRender },
                { "RenderExtraFooter", RenderExtraFooter },
                { "IsRange", IsRange },
                { "PickerIndex", PickerIndex },
                { "IsCalendar", IsCalendar },
                { "IsShowHeader", IsShowHeader },
                { "IsShowTime", IsShowTime },
                { "ShowWeek", ShowWeek} 
            };

        protected static Dictionary<DatePickerType, string> PickerTypeMap = new()
        {
            [DatePickerType.Date] = "date",
            [DatePickerType.Decade] = "decade",
            [DatePickerType.Month] = "month",
            [DatePickerType.Quarter] = "quarter",
            [DatePickerType.Time] = "time",
            [DatePickerType.Week] = "week",
            [DatePickerType.Year] = "year"
        };

        protected void OnSelectTime(DateTime date) => OnSelect?.Invoke(date, GetPickerIndex());

        protected void OnSelectDate(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(year: date.Year, month: date.Month, day: date.Day), GetPickerIndex());

        protected void OnSelectYear(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(year: date.Year), GetPickerIndex());

        protected void OnSelectQuarter(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(month: date.Month), GetPickerIndex());

        protected void OnSelectMonth(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(month: date.Month), GetPickerIndex());

        protected void OnSelectDay(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(day: date.Day), GetPickerIndex());

        protected void OnSelectHour(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(hour: date.Hour), GetPickerIndex());

        protected void OnSelectMinute(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(minute: date.Minute), GetPickerIndex());

        protected void OnSelectSecond(DateTime date) => OnSelect?.Invoke(CombineNewShowDate(second: date.Second), GetPickerIndex());

        protected void OnSelectShowYear(DateTime date) => ChangePickerValue(CombineNewShowDate(year: date.Year), GetPickerIndex());

        protected void OnSelectShowMonth(DateTime date) => ChangePickerValue(CombineNewShowDate(month: date.Month), GetPickerIndex());

        protected void OnSelectShowDay(DateTime date) => ChangePickerValue(CombineNewShowDate(day: date.Day), GetPickerIndex());

        protected DateTime CombineNewShowDate(
            int? year = null,
            int? month = null,
            int? day = null,
            int? hour = null,
            int? minute = null,
            int? second = null) => DateHelper.CombineNewDate(
                PickerValue,
                year,
                month,
                day,
                hour,
                minute,
                second
                );

        protected void ChangePickerYearValue(int interval)
        {
            DateTime baseDate;
            if (IsShowTime || PickerIndex == 0)
            {
                baseDate = PickerValue;
            }
            else
            {
                baseDate = Picker switch
                {
                    DatePickerType.Date => PickerValue.AddMonths(-1),
                    DatePickerType.Week => PickerValue.AddMonths(-1),
                    DatePickerType.Year => PickerValue.AddYears(-10),
                    _ => PickerValue.AddYears(-1)
                };
            }
            ChangePickerValue(DateHelper.AddYearsSafely(baseDate, interval), null);
        }

        protected void ChangePickerMonthValue(int interval)
        {
            DateTime baseDate;
            if (IsShowTime || PickerIndex == 0)
                baseDate = PickerValue;
            else
                baseDate = PickerValue.AddMonths(-1);
            ChangePickerValue(DateHelper.AddMonthsSafely(baseDate, interval), null);
        }

        protected void Close() => ClosePanel?.Invoke();

        protected DateTime PickerValue { get => GetIndexPickerValue(PickerIndex); }

        protected DateTime? Value { get => GetIndexValue(GetPickerIndex()); }

        public void PopUpPicker(DatePickerType type) => ChangePickerType(type, PickerIndex);

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add($"{PrefixCls}-panel")
                .If($"{PrefixCls}-panel-rtl", () => RTL);
        }

        protected int GetPickerIndex() => IsRange && !IsShowTime ? DatePicker.GetOnFocusPickerIndex() : PickerIndex;
    }
}
