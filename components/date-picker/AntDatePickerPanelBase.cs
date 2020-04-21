using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    internal class AntDatePickerPanelBase : AntDomComponentBase
    {
        [CascadingParameter]
        public AntDatePicker DatePicker { get; set; }

        [Parameter]
        public EventCallback<DateTime> OnSelect { get; set; }


        protected void OnSelectDate(DateTime date)
        {
            OnSelect.InvokeAsync(date);
        }

        protected void OnSelectYear(DateTime date)
        {
            DateTime selectDate = new DateTime(
                date.Year,
                DatePicker.CurrentSelectDate.Month,
                DatePicker.CurrentSelectDate.Day
            );

            OnSelect.InvokeAsync(selectDate);
        }

        protected void OnSelectMonth(DateTime date)
        {
            DateTime selectDate = new DateTime(
                DatePicker.CurrentSelectDate.Year,
                date.Month,
                DatePicker.CurrentSelectDate.Day
            );

            OnSelect.InvokeAsync(selectDate);
        }

        protected void OnSelectDay(DateTime date)
        {
            DateTime selectDate = new DateTime(
                DatePicker.CurrentSelectDate.Year,
                DatePicker.CurrentSelectDate.Month,
                date.Day
            );

            OnSelect.InvokeAsync(selectDate);
        }

    }
}
