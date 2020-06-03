using System;

namespace AntDesign
{
    public interface IDatePicker
    {
        public DateTime CurrentDate { get; set; }

        void ChangePickerType(string type);
        void ChangePickerType(string type, int index);
        void Close();
    }
}
