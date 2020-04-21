using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

        private DateTime CombineNewShowDate(int? year = null, int? month = null, int? day = null)
        {
            return new DateTime(
                year ?? DatePicker.CurrentShowDate.Year,
                month ?? DatePicker.CurrentShowDate.Month,
                day ?? DatePicker.CurrentShowDate.Day
            );
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
