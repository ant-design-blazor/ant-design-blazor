// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class DatePickerDisabledTime
    {
        internal int[] _disabledHours;
        internal int[] _disabledMinutes;
        internal int[] _disabledSeconds;

        public DatePickerDisabledTime(
            int[] disabledHours, int[] disabledMinutes, int[] disabledSeconds)
        {
            _disabledHours = disabledHours;
            _disabledMinutes = disabledMinutes;
            _disabledSeconds = disabledSeconds;
        }
    }
}
