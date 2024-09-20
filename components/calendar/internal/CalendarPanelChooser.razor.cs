// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class CalendarPanelChooser : AntDomComponentBase
    {
        [Parameter]
        public Calendar Calendar { get; set; }

        [Parameter]
        public Action<DateTime, int> OnSelect { get; set; }

        [Parameter]
        public int PickerIndex { get; set; }

        private bool IsShowDatePanel()
        {
            return Calendar.Picker == DatePickerType.Date;
        }

        private bool IsShowQuarterPanel()
        {
            return Calendar.Picker == DatePickerType.Quarter;
        }

        private bool IsShowWeekPanel()
        {
            return Calendar.Picker == DatePickerType.Week;
        }

        private bool IsShowMonthPanel()
        {
            return Calendar.Picker == DatePickerType.Month;
        }

        private bool IsShowYearPanel()
        {
            return Calendar.Picker == DatePickerType.Year;
        }

        private bool IsShowDecadePanel()
        {
            return Calendar.Picker == DatePickerType.Decade;
        }
    }
}
