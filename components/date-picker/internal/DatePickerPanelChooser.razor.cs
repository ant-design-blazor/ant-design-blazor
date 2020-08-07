using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class DatePickerPanelChooser<TValue> : AntDomComponentBase
    {
        [Parameter] public DatePickerBase<TValue> DatePicker { get; set; }

        [Parameter] public Action<DateTime, int> OnSelect { get; set; }

        [Parameter] public int PickerIndex { get; set; }

        private bool IsShowDatePanel()
        {
            if (DatePicker.IsShowTime && PickerIndex != DatePicker.GetOnFocusPickerIndex())
            {
                return false;
            }

            return DatePicker.Picker == DatePickerType.Date;
        }

        private bool IsShowQuarterPanel()
        {
            return DatePicker.Picker == DatePickerType.Quarter;
        }

        private bool IsShowWeekPanel()
        {
            return DatePicker.Picker == DatePickerType.Week;
        }

        private bool IsShowMonthPanel()
        {
            return DatePicker.Picker == DatePickerType.Month;
        }

        private bool IsShowYearPanel()
        {
            return DatePicker.Picker == DatePickerType.Year;
        }

        private bool IsShowDecadePanel()
        {
            return DatePicker.Picker == DatePickerType.Decade;
        }

        private bool IsShowTimePanel()
        {
            return DatePicker.Picker == DatePickerType.Time;
        }
    }
}
