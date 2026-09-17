using System;
using System.Globalization;
using System.Threading;

namespace AntDesign.Docs.Demos.Components.DatePicker.demo;

public static class ChineseLunisolarCalendarFormatter
{
    private static int _isRegistered;
    private static readonly string[] CelestialStems = ["甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸"];
    private static readonly string[] TerrestrialBranches = ["子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥"];
    private static readonly string[] MonthNames = ["正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "冬", "腊"];
    private static readonly string[] DayTens = ["初", "十", "廿", "三"];
    private static readonly string[] DayDigits = ["", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"];

    public static void Register()
    {
        if (Interlocked.Exchange(ref _isRegistered, 1) == 1) return;

        CalendarFormatter.Register(new CalendarFormatterRegistration
        {
            CultureNames = ["zh-CN", "zh-SG"],
            CanFormat = (calendar, _) => calendar is ChineseLunisolarCalendar,
            FormatDate = FormatDate,
            FormatYear = FormatYear,
            FormatMonth = FormatMonth,
            FormatDay = FormatDay,
        });
    }

    public static string FormatDate(DateTime date, string _, CultureInfo __, System.Globalization.Calendar calendar)
        => $"{FormatYear(date, string.Empty, null, calendar)}年{FormatMonth(date, string.Empty, null, calendar)}月{FormatDay(date, calendar, null)}";

    public static string FormatYear(DateTime date, string _, CultureInfo __, System.Globalization.Calendar calendar)
    {
        var chineseCalendar = (ChineseLunisolarCalendar)calendar;
        int sexagenaryYear = chineseCalendar.GetSexagenaryYear(date);
        return $"{CelestialStems[chineseCalendar.GetCelestialStem(sexagenaryYear) - 1]}{TerrestrialBranches[chineseCalendar.GetTerrestrialBranch(sexagenaryYear) - 1]}";
    }

    public static string FormatMonth(DateTime date, string _, CultureInfo __, System.Globalization.Calendar calendar)
    {
        var chineseCalendar = (ChineseLunisolarCalendar)calendar;
        int year = chineseCalendar.GetYear(date);
        int month = chineseCalendar.GetMonth(date);
        int leapMonth = chineseCalendar.GetLeapMonth(year);
        int normalMonth = leapMonth > 0 && month > leapMonth ? month - 1 : month;
        return $"{(leapMonth == month ? "闰" : string.Empty)}{MonthNames[normalMonth - 1]}";
    }

    public static string FormatDay(DateTime date, System.Globalization.Calendar calendar, CultureInfo _)
    {
        int day = ((ChineseLunisolarCalendar)calendar).GetDayOfMonth(date);
        return day switch { 10 => "初十", 20 => "二十", 30 => "三十", _ => $"{DayTens[day / 10]}{DayDigits[day % 10]}" };
    }
}
