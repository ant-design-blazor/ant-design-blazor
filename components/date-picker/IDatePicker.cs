﻿using System;

namespace AntDesign
{
    public interface IDatePicker
    {
        public DateTime CurrentDate { get; set; }
        internal DateTime? HoverDateTime { get; set; }

        internal event EventHandler<bool> OverlayVisibleChanged;
        int GetOnFocusPickerIndex();
        void ChangePlaceholder(string placeholder, int index = 0);
        string GetFormatValue(DateTime value, int index);
        void ChangePickerType(string type);
        void ChangePickerType(string type, int index);
        void Close();
    }
}
