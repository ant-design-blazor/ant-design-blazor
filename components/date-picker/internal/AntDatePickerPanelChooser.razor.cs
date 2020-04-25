using System;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Internal
{
    public partial class AntDatePickerPanelChooser : AntDomComponentBase
    {
        [Parameter]
        public AntDatePicker DatePicker { get; set; }

        [Parameter]
        public Action<DateTime, int> OnSelect { get; set; }

        [Parameter]
        public int PickerIndex { get; set; }

        private bool IsShowDatePanel()
        {
            if (DatePicker.IsShowTime && PickerIndex != DatePicker.GetOnFocusPickerIndex())
            {
                return false;
            }
            return DatePicker.Picker == AntDatePickerType.Date;
        }

        private bool IsShowQuarterPanel()
        {
            return DatePicker.Picker == AntDatePickerType.Quarter;
        }

        private bool IsShowWeekPanel()
        {
            return DatePicker.Picker == AntDatePickerType.Week;
        }

        private bool IsShowMonthPanel()
        {
            return DatePicker.Picker == AntDatePickerType.Month;
        }

        private bool IsShowYearPanel()
        {
            return DatePicker.Picker == AntDatePickerType.Year;
        }

        private bool IsShowDecadePanel()
        {
            return DatePicker.Picker == AntDatePickerType.Decade;
        }
    }
}
