// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;
using System;

namespace AntBlazor
{
    public class DatePickerPanelBase : PickerPanelBase
    {
        [CascadingParameter]
        public DatePicker DatePicker { get; set; }

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
            DatePicker.ChangePickerValue(CombineNewShowDate(year: date.Year), PickerIndex);
        }

        protected void OnSelectShowMonth(DateTime date)
        {
            DatePicker.ChangePickerValue(CombineNewShowDate(month: date.Month), PickerIndex);
        }

        protected void OnSelectShowDay(DateTime date)
        {
            DatePicker.ChangePickerValue(CombineNewShowDate(day: date.Day), PickerIndex);
        }

        protected DateTime CombineNewShowDate(
            int? year = null,
            int? month = null,
            int? day = null,
            int? hour = null,
            int? minute = null,
            int? second = null)
        {
            return new DateTime(
                year ?? PickerValue.Year,
                month ?? PickerValue.Month,
                day ?? PickerValue.Day,
                hour ?? PickerValue.Hour,
                minute ?? PickerValue.Minute,
                second ?? PickerValue.Second
            );
        }

        protected void ChangePickerYearValue(int interval)
        {
            DatePicker.ChangePickerValue(PickerValue.AddYears(interval), PickerIndex);
        }

        protected void ChangePickerMonthValue(int interval)
        {
            DatePicker.ChangePickerValue(PickerValue.AddMonths(interval), PickerIndex);
        }

        protected void ChangePickerValue(DateTime date)
        {
            DatePicker.ChangePickerValue(date, PickerIndex);
        }

        protected void ChangeValue(DateTime date)
        {
            DatePicker.ChangeValue(date, PickerIndex);
        }

        protected string Picker { get => DatePicker.Picker; }

        protected DateTime PickerValue { get => DatePicker.GetIndexPickerValue(PickerIndex); }

        protected DateTime Value { get => DatePicker.GetIndexValue(PickerIndex) ?? DateTime.Now; }

        public void PopUpPicker(string type)
        {
            DatePicker.ChangePickerType(type, PickerIndex);
        }
    }
}
