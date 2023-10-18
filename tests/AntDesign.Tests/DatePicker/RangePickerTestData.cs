using System;
using System.Collections.Generic;

namespace AntDesign.Tests.DatePicker;

public static class RangePickerTestData
{
    private const string DefaultDateFormat = "yyyy-MM-dd";
    private const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    public static IEnumerable<object?[]> NullableData
    {
        get
        {
            yield return new object?[] { new[] { new DateTime?(new DateTime(2022, 6, 8, 20, 20, 0)), new DateTime(2022, 6, 8, 20, 20, 30) } };
            yield return new object?[] { new[] { new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5))), new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 30, TimeSpan.FromHours(5))) } };

#if NET6_0_OR_GREATER
            yield return new object[] { new[] { new DateOnly?(DateOnly.FromDateTime(new DateTime(2022, 6, 8))), new DateOnly?(DateOnly.FromDateTime(new DateTime(2022, 6, 8))) } };
#endif
        }
    }

    public static IEnumerable<object?[]> NullData
    {
        get
        {
            yield return new object?[] { new[] { new DateTime?(), new DateTime?() } };
            yield return new object?[] { new[] { new DateTimeOffset?(), new DateTimeOffset?() } };

#if NET6_0_OR_GREATER
            yield return new object?[] { new[] { new DateOnly?(), new DateOnly?() } };
#endif
        }
    }

    public static IEnumerable<object?[]> SameDateRanges
    {
        get
        {
            yield return new object?[] { new[] { new DateTime?(new DateTime(2022, 6, 8, 20, 20, 0)), new DateTime(2022, 6, 8, 20, 20, 30) } };
            yield return new object[] { new[] { new DateTime(2022, 6, 8, 20, 20, 0), new DateTime(2022, 6, 8, 20, 20, 30) } };
            yield return new object?[] { new[] { new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5))), new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 30, TimeSpan.FromHours(5))) } };
            yield return new object[] { new[] { new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2022, 6, 8, 20, 20, 30, TimeSpan.FromHours(5)) } };
        }
    }

    public static IEnumerable<object?[]> NotSameDateRanges
    {
        get
        {
            yield return new object?[] { new[] { new DateTime?(new DateTime(2022, 6, 8, 20, 20, 0)), new DateTime?(new DateTime(2022, 6, 9, 10, 25, 15)) } };
            yield return new object[] { new[] { new DateTime(2022, 6, 8, 20, 20, 0), new DateTime(2022, 6, 9, 10, 25, 15) } };
            yield return new object?[] { new[] { new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5))), new DateTimeOffset?(new DateTimeOffset(2022, 6, 9, 10, 25, 15, TimeSpan.FromHours(5))) } };
            yield return new object[] { new[] { new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2022, 6, 9, 10, 25, 15, TimeSpan.FromHours(5)) } };
        }
    }

    public static IEnumerable<object?[]> InvalidKeyedInput
    {
        get
        {
            yield return new object?[] { 0, new[] { new DateTime?(new DateTime(2022, 6, 8, 20, 20, 0)), new DateTime(2022, 6, 8, 20, 20, 30) }, "2022-06-08 20:25:35", DefaultDateTimeFormat, true };
            yield return new object?[] { 0, new[] { new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5))), new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 30, TimeSpan.FromHours(5))) }, "2022-06-08 20:25:35", DefaultDateTimeFormat, true };
            yield return new object?[] { 1, new[] { new DateTime?(new DateTime(2022, 6, 8, 20, 20, 0)), new DateTime(2022, 6, 8, 20, 20, 30) }, "2022-06-07 12:25:35", DefaultDateTimeFormat, true };
            yield return new object?[] { 1, new[] { new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 0, TimeSpan.FromHours(5))), new DateTimeOffset?(new DateTimeOffset(2022, 6, 8, 20, 20, 30, TimeSpan.FromHours(5))) }, "2022-06-07 12:25:35", DefaultDateTimeFormat, true };

#if NET6_0_OR_GREATER
            yield return new object[] { 0, new[] { new DateOnly?(DateOnly.FromDateTime(new DateTime(2022, 6, 8))), new DateOnly?(DateOnly.FromDateTime(new DateTime(2022, 6, 8))) }, "2022-06-09", DefaultDateFormat, false };
            yield return new object[] { 1, new[] { new DateOnly?(DateOnly.FromDateTime(new DateTime(2022, 6, 8))), new DateOnly?(DateOnly.FromDateTime(new DateTime(2022, 6, 8))) }, "2022-06-07", DefaultDateFormat, false };

#endif
        }
    }

    public static IEnumerable<object[]> ValidKeyedInput
    {
        get
        {
            yield return new object[] { "Enter", new[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) }, new[] { new DateTime(2020, 1, 1, 10, 30, 5), new DateTime(2020, 1, 2, 8, 20, 15) }, DefaultDateTimeFormat, true };
            yield return new object[] { "Enter", new[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) }, new[] { new DateTime(2020, 1, 2), new DateTime(2020, 1, 5) }, DefaultDateFormat, false };
            yield return new object[] { "Tab", new[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) }, new[] { new DateTime(2020, 1, 1, 10, 30, 5), new DateTime(2020, 1, 2, 8, 20, 15) }, DefaultDateTimeFormat, true };
            yield return new object[] { "Tab", new[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) }, new[] { new DateTime(2020, 1, 2), new DateTime(2020, 1, 5) }, DefaultDateFormat, false };

            yield return new object[] { "Enter", new[] { new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)) }, new[] { new DateTimeOffset(2020, 1, 1, 10, 30, 5, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 8, 20, 15, TimeSpan.FromHours(5)) }, DefaultDateTimeFormat, true };
            yield return new object[] { "Enter", new[] { new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)) }, new[] { new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 5, 0, 0, 0, TimeSpan.FromHours(5)) }, DefaultDateFormat, false };
            yield return new object[] { "Tab", new[] { new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)) }, new[] { new DateTimeOffset(2020, 1, 1, 10, 30, 5, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 8, 20, 15, TimeSpan.FromHours(5)) }, DefaultDateTimeFormat, true };
            yield return new object[] { "Tab", new[] { new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)) }, new[] { new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 5, 0, 0, 0, TimeSpan.FromHours(5)) }, DefaultDateFormat, false };


#if NET6_0_OR_GREATER            
            yield return new object[] { "Enter", new[] { new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2) }, new[] { new DateOnly(2020, 2, 2), new DateOnly(2020, 2, 3) }, DefaultDateFormat, false };
            yield return new object[] { "Tab", new[] { new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2) }, new[] { new DateOnly(2020, 1, 3), new DateOnly(2020, 1, 5) }, DefaultDateFormat, false };
#endif
        }
    }

    public static IEnumerable<object[]> DateTimeOffsetKeyedInput
    {
        get
        {
            yield return new object[] { new[] { new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.FromHours(5)) }, DefaultDateTimeFormat, true };
            yield return new object[] { new[] { new DateTimeOffset(2022, 2, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2022, 2, 6, 0, 0, 0, TimeSpan.Zero) }, DefaultDateTimeFormat, true };
            yield return new object[] { new[] { new DateTimeOffset(DateTime.Today), new DateTimeOffset(DateTime.Today.AddDays(1)) }, DefaultDateTimeFormat, true };

        }
    }
}
