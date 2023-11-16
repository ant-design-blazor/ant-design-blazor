using System;
using System.Collections.Generic;

namespace AntDesign.Tests.DatePicker;

public static class DatePickerTestData
{
    public static IEnumerable<object?[]> Data
    {
        get
        {
            yield return new object?[] { new DateTime?() };
            yield return new object?[] { new DateTime?(DateTime.Now) };
            yield return new object[] { DateTime.Now };
            yield return new object?[] { new DateTimeOffset?() };
            yield return new object?[] { new DateTimeOffset?(DateTimeOffset.Now) };
            yield return new object[] { DateTimeOffset.Now };

#if NET6_0_OR_GREATER
            yield return new object?[] { new DateOnly?() };
            yield return new object[] { DateOnly.FromDateTime(DateTime.Now) };
            yield return new object[] { new DateOnly?(DateOnly.FromDateTime(DateTime.Now)) };
            yield return new object[] { TimeOnly.FromDateTime(DateTime.Now) };
            yield return new object[] { new TimeOnly(0, 0, 0) };
#endif
        }
    }

    public static IEnumerable<object?[]> NullableData
    {
        get
        {
            yield return new object?[] { new DateTime?(DateTime.Now) };
            yield return new object?[] { new DateTimeOffset?(DateTimeOffset.Now) };

#if NET6_0_OR_GREATER
            yield return new object[] { new DateOnly?(DateOnly.FromDateTime(DateTime.Now)) };
#endif
        }
    }

    public static IEnumerable<object?[]> NullData
    {
        get
        {
            yield return new object?[] { new DateTime?() };
            yield return new object?[] { new DateTimeOffset?() };

#if NET6_0_OR_GREATER
            yield return new object?[] { new DateOnly?() };
#endif
        }
    }
    public static IEnumerable<object[]> CultureData
    {
        get
        {
            yield return new object[] { new DateTime(2020, 4, 5), "en-US", "2020-04-05" };
            yield return new object[] { new DateTime(2020, 4, 5), "en-GB", "5/4/2020" };
            yield return new object[] { new DateTime(2020, 4, 5), "th-TH", "5/4/2563" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "en-US", "2020-04-05" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "en-GB", "5/4/2020" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "th-TH", "5/4/2563" };

#if NET6_0_OR_GREATER
            yield return new object[] { new DateOnly(2020, 4, 5), "en-US", "2020-04-05" };
            yield return new object[] { new DateOnly(2020, 4, 5), "en-GB", "5/4/2020" };
            yield return new object[] { new DateOnly(2020, 4, 5), "th-TH", "5/4/2563" };
#endif
        }
    }

    public static IEnumerable<object[]> FormatData
    {
        get
        {
            yield return new object[] { new DateTime(2020, 4, 5), "yyyy-MM-dd", "2020-04-05" };
            yield return new object[] { new DateTime(2020, 4, 5), "dd/MM/yyyy", "05/04/2020" };
            yield return new object[] { new DateTime(2020, 4, 5), "MM/dd/yyyy", "04/05/2020" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "yyyy-MM-dd", "2020-04-05" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "dd/MM/yyyy", "05/04/2020" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "MM/dd/yyyy", "04/05/2020" };

#if NET6_0_OR_GREATER
            yield return new object[] { new DateOnly(2020, 4, 5), "yyyy-MM-dd", "2020-04-05" };
            yield return new object[] { new DateOnly(2020, 4, 5), "dd/MM/yyyy", "05/04/2020" };
            yield return new object[] { new DateOnly(2020, 4, 5), "MM/dd/yyyy", "04/05/2020" };
#endif
        }
    }

    public static IEnumerable<object[]> FormatCultureData
    {
        get
        {
            yield return new object[] { new DateTime(2020, 4, 5), "yyyy-MM-dd", "en-GB", "2020-04-05" };
            yield return new object[] { new DateTime(2020, 4, 5), "dd/MM/yyyy", "en-US", "05/04/2020" };
            yield return new object[] { new DateTime(2020, 4, 5), "MM/dd/yyyy", "en-US", "04/05/2020" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "yyyy-MM-dd", "en-GB", "2020-04-05" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "dd/MM/yyyy", "en-US", "05/04/2020" };
            yield return new object[] { new DateTimeOffset(2020, 4, 5, 0, 0, 0, TimeSpan.FromHours(5)), "MM/dd/yyyy", "en-US", "04/05/2020" };

#if NET6_0_OR_GREATER
            yield return new object[] { new DateOnly(2020, 4, 5), "yyyy-MM-dd", "en-GB", "2020-04-05" };
            yield return new object[] { new DateOnly(2020, 4, 5), "dd/MM/yyyy", "en-US", "05/04/2020" };
            yield return new object[] { new DateOnly(2020, 4, 5), "MM/dd/yyyy", "en-US", "04/05/2020" };
#endif
        }
    }

    public static IEnumerable<object?[]> TimePickerData
    {
        get
        {
            yield return new object[] { DateTime.Now };
            yield return new object[] { DateTimeOffset.Now };

#if NET6_0_OR_GREATER
            yield return new object[] { TimeOnly.FromDateTime(DateTime.Now) };
            yield return new object[] { new TimeOnly(0, 0, 0) };
#endif
        }
    }
}
