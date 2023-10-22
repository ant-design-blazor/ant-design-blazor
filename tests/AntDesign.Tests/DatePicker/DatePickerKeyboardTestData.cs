using System;
using System.Collections.Generic;

namespace AntDesign.Tests.DatePicker;
public static class DatePickerKeyboardTestData
{
    private const string DefaultDateFormat = "yyyy-MM-dd";
    private const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    public static IEnumerable<object[]> KeyedInputs
    {
        get
        {
            yield return new object[] { "Enter", DateTime.MinValue, new DateTime(2020, 1, 1), new DateTime(2020, 1, 2, 10, 30, 5), DefaultDateTimeFormat, true };
            yield return new object[] { "Enter", DateTime.MinValue, new DateTime(2020, 1, 1), new DateTime(2020, 1, 2), DefaultDateFormat, false };
            yield return new object[] { "Tab", DateTime.MinValue, new DateTime(2020, 1, 1), new DateTime(2020, 1, 2, 10, 30, 5), DefaultDateTimeFormat, true };
            yield return new object[] { "Tab", DateTime.MinValue, new DateTime(2020, 1, 1), new DateTime(2020, 1, 2), DefaultDateFormat, false };

            yield return new object[] { "Enter", DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 10, 30, 5, TimeSpan.FromHours(5)), DefaultDateTimeFormat, true };
            yield return new object[] { "Enter", DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)), DefaultDateFormat, false };
            yield return new object[] { "Tab", DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 10, 30, 5, TimeSpan.FromHours(5)), DefaultDateTimeFormat, true };
            yield return new object[] { "Tab", DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)), DefaultDateFormat, false };


#if NET6_0_OR_GREATER            
            yield return new object[] { "Enter", DateOnly.MinValue, new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2), DefaultDateFormat, false };
            yield return new object[] { "Tab", DateOnly.MinValue, new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2), DefaultDateFormat, false };
#endif
        }
    }

    public static IEnumerable<object[]> InvalidKeyedInputs
    {
        get
        {
            yield return new object[] { DateTime.MinValue, new DateTime(2020, 1, 1), DefaultDateFormat, "2022-90-1" };
            yield return new object[] { DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 10, 30, 0, TimeSpan.FromHours(5)), DefaultDateFormat, "2022-90-1" };
#if NET6_0_OR_GREATER                  
            yield return new object[] { DateOnly.MinValue, new DateOnly(2020, 1, 1), DefaultDateFormat, "2022-90-1" };
#endif
            yield return new object[] { DateTime.MinValue, new DateTime(2020, 1, 1), DefaultDateFormat, "202-01-01" };
            yield return new object[] { DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 10, 30, 0, TimeSpan.FromHours(5)), DefaultDateFormat, "202-01-01" };
#if NET6_0_OR_GREATER                  
            yield return new object[] { DateOnly.MinValue, new DateOnly(2020, 1, 1), DefaultDateFormat, "202-01-01" };
#endif
        }
    }

    public static IEnumerable<object[]> ValidKeyedInputs
    {
        get
        {
            yield return new object[] { DateTime.MinValue, new DateTime(2020, 1, 1), DefaultDateFormat, "2022-01-03" };
            yield return new object[] { DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 10, 30, 0, TimeSpan.FromHours(5)), DefaultDateFormat, "2022-01-03" };
#if NET6_0_OR_GREATER                  
            yield return new object[] { DateOnly.MinValue, new DateOnly(2020, 1, 1), DefaultDateFormat, "2022-01-03" };
#endif
            yield return new object[] { DateTime.MinValue, new DateTime(2020, 1, 1), DefaultDateFormat, "2022-01-03" };
            yield return new object[] { DateTimeOffset.MinValue, new DateTimeOffset(2020, 1, 1, 10, 30, 0, TimeSpan.FromHours(5)), DefaultDateFormat, "2022-01-03" };
#if NET6_0_OR_GREATER                  
            yield return new object[] { DateOnly.MinValue, new DateOnly(2020, 1, 1), DefaultDateFormat, "2022-01-03" };
#endif
        }
    }

    public static IEnumerable<object[]> KeyedInputsWithMaxValue
    {
        get
        {
            yield return new object[] { "Enter", DateTime.MinValue, new DateTime(2023, 02, 24), new DateTime(2023, 02, 24, 10, 30, 5).AddDays(-5), DefaultDateTimeFormat, true };
            yield return new object[] { "Enter", DateTime.MinValue, new DateTime(2023, 02, 24), new DateTime(2023, 02, 24).AddDays(-5), DefaultDateFormat, false };
            yield return new object[] { "Tab", DateTime.MinValue, new DateTime(2023, 02, 24), new DateTime(2023, 02, 24, 10, 30, 5).AddDays(-5), DefaultDateTimeFormat, true };
            yield return new object[] { "Tab", DateTime.MinValue, new DateTime(2023, 02, 24), new DateTime(2023, 02, 24).AddDays(-5), DefaultDateFormat, false };

            yield return new object[] { "Enter", DateTimeOffset.MinValue, new DateTimeOffset(2023, 02, 24, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2023, 02, 24, 10, 30, 5, TimeSpan.FromHours(5)).AddDays(-5), DefaultDateTimeFormat, true };
            yield return new object[] { "Enter", DateTimeOffset.MinValue, new DateTimeOffset(2023, 02, 24, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2023, 02, 24, 0, 0, 0, TimeSpan.FromHours(5)).AddDays(-5), DefaultDateFormat, false };
            yield return new object[] { "Tab", DateTimeOffset.MinValue, new DateTimeOffset(2023, 02, 24, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2023, 02, 24, 10, 30, 5, TimeSpan.FromHours(5)).AddDays(-5), DefaultDateTimeFormat, true };
            yield return new object[] { "Tab", DateTimeOffset.MinValue, new DateTimeOffset(2023, 02, 24, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2023, 02, 24, 0, 0, 0, TimeSpan.FromHours(5)).AddDays(-5), DefaultDateFormat, false };


#if NET6_0_OR_GREATER            
            yield return new object[] { "Enter", DateOnly.MinValue, new DateOnly(2023, 02, 24), new DateOnly(2023, 02, 24).AddDays(-5), DefaultDateFormat, false };
            yield return new object[] { "Tab", DateOnly.MinValue, new DateOnly(2023, 02, 24), new DateOnly(2023, 02, 24).AddDays(-5), DefaultDateFormat, false };
#endif
        }
    }

    public static IEnumerable<object[]> DateTimeOffsetData
    {
        get
        {
            yield return new object[] { new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2023, 2, 2, 0, 0, 0, TimeSpan.FromHours(5)), DefaultDateFormat };
            yield return new object[] { new DateTimeOffset(2022, 2, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2023, 2, 2, 0, 0, 0, TimeSpan.Zero), DefaultDateFormat };
            yield return new object[] { new DateTimeOffset(DateTime.Today), DateTime.Today.AddDays(1), DefaultDateFormat };
        }
    }
}
