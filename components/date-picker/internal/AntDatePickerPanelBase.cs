// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using System;

namespace AntBlazor
{
    public class AntDatePickerPanelBase : AntDomComponentBase
    {
        [CascadingParameter]
        public AntDatePicker DatePicker { get; set; }

        [Parameter]
        public EventCallback<DateTime> OnSelect { get; set; }

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

        protected void OnSelectQuarter(DateTime date)
        {
            OnSelect.InvokeAsync(CombineNewShowDate(month: date.Month));

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
            DatePicker.ChangePickerValue(CombineNewShowDate(year: date.Year));

            OnSelected();
        }

        protected void OnSelectShowMonth(DateTime date)
        {
            DatePicker.ChangePickerValue(CombineNewShowDate(month: date.Month));

            OnSelected();
        }

        protected void OnSelectShowDay(DateTime date)
        {
            DatePicker.ChangePickerValue(CombineNewShowDate(day: date.Day));

            OnSelected();
        }

        protected DateTime CombineNewShowDate(int? year = null, int? month = null, int? day = null)
        {
            return new DateTime(
                year ?? DatePicker.PickerValue.Year,
                month ?? DatePicker.PickerValue.Month,
                day ?? DatePicker.PickerValue.Day
            );
        }


        public void PopUpPicker(string type)
        {
            DatePicker.ChangePickerType(type);
        }

        private void OnSelected()
        {
           
        }
    }
}
