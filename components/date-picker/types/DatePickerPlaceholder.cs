﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    internal class DatePickerPlaceholder
    {
        public static string GetPlaceholderByType(DatePickerType pickerType, DatePickerLocale locale)
        {
            var placeholder = pickerType.Name switch
            {
                DatePickerType.DATE => locale.Lang.DateSelect,
                DatePickerType.WEEK => locale.Lang.WeekSelect,
                DatePickerType.MONTH => locale.Lang.MonthSelect,
                DatePickerType.QUARTER => locale.Lang.QuarterSelect,
                DatePickerType.YEAR => locale.Lang.YearSelect,
                DatePickerType.TIME => locale.Lang.TimeSelect,
                _ => locale.Lang.DateSelect,
            };

            return placeholder;
        }

        public static (string, string) GetRangePlaceHolderByType(DatePickerType pickerType, DatePickerLocale locale)
        {
            var placeholder = pickerType.Name switch
            {
                DatePickerType.DATE => (locale.Lang.StartDate, locale.Lang.EndDate),
                DatePickerType.WEEK => (locale.Lang.StartWeek, locale.Lang.EndWeek),
                DatePickerType.MONTH => (locale.Lang.StartMonth, locale.Lang.EndMonth),
                DatePickerType.YEAR => (locale.Lang.StartYear, locale.Lang.EndYear),
                DatePickerType.TIME => (locale.Lang.StartDate, locale.Lang.EndDate),
                DatePickerType.QUARTER => (locale.Lang.StartQuarter, locale.Lang.EndQuarter),
                _ => (locale.Lang.StartDate, locale.Lang.EndDate),
            };
            return placeholder;
        }
    }
}
