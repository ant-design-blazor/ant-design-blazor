// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class YearPicker<TValue> : DatePicker<TValue>
    {
        public YearPicker()
        {
            Picker = DatePickerType.Year;
        }
    }
}
