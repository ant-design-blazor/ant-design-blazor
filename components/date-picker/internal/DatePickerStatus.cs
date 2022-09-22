using System;

namespace AntDesign.Internal
{
    internal class DatePickerStatus
    {
        public string InitPicker { get; set; } = null;
        public bool IsValueSelected { get; set; }
        public DateTime? SelectedValue { get; set; }
    }
}
