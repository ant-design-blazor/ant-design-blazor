// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace AntDesign
{
    /// <summary>Formats dates displayed by picker components for standard .NET calendars.</summary>
    public static class CalendarFormatter
    {
        public static string FormatDate(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => DateHelper.FormatDate(date, format, culture, calendar);

        public static string FormatYear(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => DateHelper.FormatDate(date, format, culture, calendar);

        public static string FormatMonth(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => DateHelper.FormatDate(date, format, culture, calendar);

        public static string FormatDay(DateTime date, System.Globalization.Calendar calendar, CultureInfo culture)
            => calendar.GetDayOfMonth(date).ToString(culture);
    }

    /// <summary>Formats dates for <see cref="ChineseLunisolarCalendar"/>.</summary>
    public static class ChineseLunisolarCalendarFormatter
    {
        private static readonly string[] CelestialStems = ["甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸"];
        private static readonly string[] TerrestrialBranches = ["子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥"];
        private static readonly string[] MonthNames = ["正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "冬", "腊"];
        private static readonly string[] DayTens = ["初", "十", "廿", "三"];
        private static readonly string[] DayDigits = ["", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"];

        public static string FormatDate(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => $"{FormatYear(date, format, culture, calendar)}年{FormatMonth(date, format, culture, calendar)}月{FormatDay(date, calendar, culture)}";

        public static string FormatYear(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
        {
            var chineseCalendar = (ChineseLunisolarCalendar)calendar;
            int sexagenaryYear = chineseCalendar.GetSexagenaryYear(date);
            return $"{CelestialStems[chineseCalendar.GetCelestialStem(sexagenaryYear) - 1]}{TerrestrialBranches[chineseCalendar.GetTerrestrialBranch(sexagenaryYear) - 1]}";
        }

        public static string FormatMonth(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
        {
            var chineseCalendar = (ChineseLunisolarCalendar)calendar;
            int year = chineseCalendar.GetYear(date);
            int month = chineseCalendar.GetMonth(date);
            int leapMonth = chineseCalendar.GetLeapMonth(year);
            bool isLeapMonth = leapMonth == month;
            int normalMonth = leapMonth > 0 && month > leapMonth ? month - 1 : month;
            return $"{(isLeapMonth ? "闰" : string.Empty)}{MonthNames[normalMonth - 1]}";
        }

        public static string FormatDay(DateTime date, System.Globalization.Calendar calendar, CultureInfo culture)
        {
            var chineseCalendar = (ChineseLunisolarCalendar)calendar;
            int day = chineseCalendar.GetDayOfMonth(date);
            return day switch
            {
                10 => "初十",
                20 => "二十",
                30 => "三十",
                _ => $"{DayTens[day / 10]}{DayDigits[day % 10]}",
            };
        }
    }

    internal static class CalendarFormatterProvider
    {
        public static string FormatDate(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => calendar is ChineseLunisolarCalendar
                ? ChineseLunisolarCalendarFormatter.FormatDate(date, format, culture, calendar)
                : CalendarFormatter.FormatDate(date, format, culture, calendar);

        public static string FormatYear(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => calendar is ChineseLunisolarCalendar
                ? ChineseLunisolarCalendarFormatter.FormatYear(date, format, culture, calendar)
                : CalendarFormatter.FormatYear(date, format, culture, calendar);

        public static string FormatMonth(DateTime date, string format, CultureInfo culture, System.Globalization.Calendar calendar)
            => calendar is ChineseLunisolarCalendar
                ? ChineseLunisolarCalendarFormatter.FormatMonth(date, format, culture, calendar)
                : CalendarFormatter.FormatMonth(date, format, culture, calendar);

        public static string FormatDay(DateTime date, System.Globalization.Calendar calendar, CultureInfo culture)
            => calendar is ChineseLunisolarCalendar
                ? ChineseLunisolarCalendarFormatter.FormatDay(date, calendar, culture)
                : CalendarFormatter.FormatDay(date, calendar, culture);
    }
}
