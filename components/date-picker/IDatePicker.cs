using System;
using System.Globalization;

namespace AntDesign
{
    public interface IDatePicker
    {
        public DateTime CurrentDate { get; set; }
        public CultureInfo CultureInfo { get; set; }

        void ChangePickerType(string type);
        void ChangePickerType(string type, int index);
        void Close();
    }
}
