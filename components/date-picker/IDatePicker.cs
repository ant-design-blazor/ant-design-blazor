// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public interface IDatePicker
    {
        public DateTime CurrentDate { get; set; }
        internal DateTime? HoverDateTime { get; set; }

        internal event EventHandler<bool> OverlayVisibleChanged;
        int GetOnFocusPickerIndex();
        void ChangePlaceholder(string placeholder, int index = 0);
        void ResetPlaceholder(int index = -1);
        string GetFormatValue(DateTime value, int index);
        void ChangePickerType(string type);
        void ChangePickerType(string type, int index);
        void Close();
    }
}
