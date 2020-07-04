using System;
using System.Globalization;

namespace AntDesign
{
    public interface IDatePicker
    {
        public DateTime CurrentDate { get; set; }
        public CultureInfo CultureInfo { get; set; }

        internal DateTime? HoverDateTime { get; set; }

        int GetOnFocusPickerIndex();

        void ChangePickerType(string type);
        void ChangePickerType(string type, int index);
        void InvokeStateHasChanged();
        void Close();
    }
}
