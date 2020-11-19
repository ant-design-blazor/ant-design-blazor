using System;
using System.Collections.Generic;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DatePickerPanelBase<TValue> : PickerPanelBase
    {
        [CascadingParameter]
        public IDatePicker DatePicker { get; set; }

        [Parameter]
        public string PrefixCls { get; set; }

        [Parameter]
        public string Picker { get; set; }

        [Parameter]
        public bool IsRange { get; set; } = false;

        [Parameter]
        public bool IsCalendar { get; set; } = false;

        [Parameter]
        public bool IsShowHeader { get; set; } = true;

        [Parameter]
        public Action ClosePanel { get; set; }

        [Parameter]
        public Action<DateTime, int> ChangePickerValue { get; set; }

        [Parameter]
        public Action<DateTime, int> ChangeValue { get; set; }

        [Parameter]
        public Action<string, int> ChangePickerType { get; set; }

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

        protected Dictionary<string, object> GetAttritubes()
        {
            return new Dictionary<string, object>()
            {
                { "PrefixCls", PrefixCls },
                { "Picker", Picker },
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
            };
        }

        protected void OnSelectTime(DateTime date)
        {
            OnSelect?.Invoke(date, PickerIndex);
        }

        protected void OnSelectDate(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(year: date.Year, month: date.Month, day: date.Day), PickerIndex);
        }

        protected void OnSelectYear(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(year: date.Year), PickerIndex);
        }

        protected void OnSelectQuarter(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(month: date.Month), PickerIndex);
        }

        protected void OnSelectMonth(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(month: date.Month), PickerIndex);
        }

        protected void OnSelectDay(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(day: date.Day), PickerIndex);
        }

        protected void OnSelectHour(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(hour: date.Hour), PickerIndex);
        }

        protected void OnSelectMinute(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(minute: date.Minute), PickerIndex);
        }

        protected void OnSelectSecond(DateTime date)
        {
            OnSelect?.Invoke(CombineNewShowDate(second: date.Second), PickerIndex);
        }

        protected void OnSelectShowYear(DateTime date)
        {
            ChangePickerValue(CombineNewShowDate(year: date.Year), PickerIndex);
        }

        protected void OnSelectShowMonth(DateTime date)
        {
            ChangePickerValue(CombineNewShowDate(month: date.Month), PickerIndex);
        }

        protected void OnSelectShowDay(DateTime date)
        {
            ChangePickerValue(CombineNewShowDate(day: date.Day), PickerIndex);
        }

        protected DateTime CombineNewShowDate(
            int? year = null,
            int? month = null,
            int? day = null,
            int? hour = null,
            int? minute = null,
            int? second = null)
        {
            return DateHelper.CombineNewDate(
                PickerValue,
                year,
                month,
                day,
                hour,
                minute,
                second
                );
        }

        protected void ChangePickerYearValue(int interval)
        {
            ChangePickerValue(PickerValue.AddYears(interval), PickerIndex);
        }

        protected void ChangePickerMonthValue(int interval)
        {
            ChangePickerValue(PickerValue.AddMonths(interval), PickerIndex);
        }

        protected void Close()
        {
            ClosePanel?.Invoke();
        }

        protected DateTime PickerValue { get => GetIndexPickerValue(PickerIndex); }

        protected DateTime Value { get => GetIndexValue(PickerIndex) ?? DateTime.Now; }

        public void PopUpPicker(string type)
        {
            ChangePickerType(type, PickerIndex);
        }
    }
}
