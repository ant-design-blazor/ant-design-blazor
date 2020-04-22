using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntBlazor
{
    public class AntDatePickerPanelBase : AntDomComponentBase
    {
        [CascadingParameter]
        public AntDatePicker DatePicker { get; set; }

        [Parameter]
        public EventCallback<DateTime> OnSelect { get; set; }

        /// <summary>
        /// 测试期间用：是否在选择日期后自动关闭
        /// </summary>
        [Parameter]
        public bool AutoClose { get; set; } = false;

        protected bool IsClose { get; set; } = false;

        protected Calendar calendar = CultureInfo.InvariantCulture.Calendar;

        protected void OnSelectDate(DateTime date)
        {
            OnSelect.InvokeAsync(date);

            OnSelected();
        }

        protected void OnSelectYear(DateTime date)
        {
            OnSelect.InvokeAsync(CombineNewShowDate(year: date.Year));

            OnSelected();
        }

        protected void OnSelectMonth(DateTime date)
        {
            OnSelect.InvokeAsync(CombineNewShowDate(month: date.Month));

            OnSelected();
        }

        protected void OnSelectDay(DateTime date)
        {
            OnSelect.InvokeAsync(CombineNewShowDate(day: date.Day));

            OnSelected();
        }

        protected void OnSelectShowYear(DateTime date)
        {
            DatePicker.ChangeShowDate(CombineNewShowDate(year: date.Year));

            OnSelected();
        }

        protected void OnSelectShowMonth(DateTime date)
        {
            DatePicker.ChangeShowDate(CombineNewShowDate(month: date.Month));

            OnSelected();
        }

        protected void OnSelectShowDay(DateTime date)
        {
            DatePicker.ChangeShowDate(CombineNewShowDate(day: date.Day));

            OnSelected();
        }

        protected DateTime CombineNewShowDate(int? year = null, int? month = null, int? day = null)
        {
            return new DateTime(
                year ?? DatePicker.CurrentShowDate.Year,
                month ?? DatePicker.CurrentShowDate.Month,
                day ?? DatePicker.CurrentShowDate.Day
            );
        }

        protected bool IsSameDate(DateTime date, DateTime compareDate)
        {
            return date == compareDate;
        }

        protected bool IsSameYear(DateTime date, DateTime compareDate)
        {
            return date.Year == compareDate.Year;
        }

        protected bool IsSameMonth(DateTime date, DateTime compareDate)
        {
            return IsSameYear(date, compareDate)
                && date.Month == compareDate.Month;
        }

        protected bool IsSameDay(DateTime date, DateTime compareDate)
        {
            return calendar.GetDayOfYear(date) == calendar.GetDayOfYear(compareDate);
        }

        protected bool IsSameWeak(DateTime date, DateTime compareDate)
        {
            return GetWeekOfYear(date) == GetWeekOfYear(compareDate);
        }

        protected int GetWeekOfYear(DateTime date)
        {
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        private void OnSelected()
        {
            if (AutoClose)
            {
                IsClose = true;
            }
        }
    }
}
