using System;
using System.Linq;

namespace AntDesign.Internal;

internal static class InternalConvert
{
    public static DateTimeOffset? ToDateTimeOffset<TValue>(TValue input)
    {
        if (input is null)
        {
            return null;
        }

        var type = input.GetType();

        if (type.IsAssignableFrom(typeof(DateTime)))
        {
            return ConvertFromDateTime(THelper.ChangeType<DateTime>(input));
        }

        if (type.IsAssignableFrom(typeof(DateTime?)))
        {
            return ConvertFromDateTime(THelper.ChangeType<DateTime?>(input).Value);
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset)))
        {
            return THelper.ChangeType<DateTimeOffset>(input);
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset?)))
        {
            return THelper.ChangeType<DateTimeOffset?>(input);
        }

#if NET6_0_OR_GREATER

        if (type.IsAssignableFrom(typeof(DateOnly)))
        {
            return ConvertFromDateTime(THelper.ChangeType<DateOnly>(input)
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Local));
        }

        if (type.IsAssignableFrom(typeof(DateOnly?)))
        {
            return ConvertFromDateTime(THelper.ChangeType<DateOnly?>(input).Value
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Local));
        }

        if (type.IsAssignableFrom(typeof(TimeOnly)))
        {
            return ConvertFromDateTime(DateOnly.FromDateTime(DateTime.MinValue)
                    .ToDateTime(THelper.ChangeType<TimeOnly>(input), DateTimeKind.Local));
        }

        if (type.IsAssignableFrom(typeof(TimeOnly?)))
        {
            return ConvertFromDateTime(DateOnly.FromDateTime(DateTime.MinValue)
                    .ToDateTime(THelper.ChangeType<TimeOnly?>(input).Value, DateTimeKind.Local));
        }

#endif
        throw new NotSupportedException($"{type.FullName} not supported");
    }

    public static TValue FromDateTimeOffset<TValue>(DateTimeOffset input)
    {
        var type = typeof(TValue);

        if (type.IsAssignableFrom(typeof(DateTime)) || type.IsAssignableFrom(typeof(DateTime?))
            || type.IsAssignableFrom(typeof(DateTime[])) || type.IsAssignableFrom(typeof(DateTime?[])))
        {
            return THelper.ChangeType<TValue>(ConvertFromDateTimeOffset(input));
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset)) || type.IsAssignableFrom(typeof(DateTimeOffset?))
            || type.IsAssignableFrom(typeof(DateTimeOffset[])) || type.IsAssignableFrom(typeof(DateTimeOffset?[])))
        {
            return THelper.ChangeType<TValue>(input);
        }

#if NET6_0_OR_GREATER

        if (type.IsAssignableFrom(typeof(DateOnly)) || type.IsAssignableFrom(typeof(DateOnly?))
           || type.IsAssignableFrom(typeof(DateOnly[])) || type.IsAssignableFrom(typeof(DateOnly?[])))
        {
            return THelper.ChangeType<TValue>(DateOnly.FromDateTime(ConvertFromDateTimeOffset(input)));
        }

        if (type.IsAssignableFrom(typeof(TimeOnly)) || type.IsAssignableFrom(typeof(TimeOnly?))
         || type.IsAssignableFrom(typeof(TimeOnly[])) || type.IsAssignableFrom(typeof(TimeOnly?[])))
        {
            return THelper.ChangeType<TValue>(TimeOnly.FromDateTime(ConvertFromDateTimeOffset(input)));
        }
#endif
        throw new NotSupportedException($"{type.FullName} not supported");
    }

    public static DateTime? ToDateTime<TValue>(TValue input)
    {
        if (input is null)
        {
            return null;
        }

        var type = input.GetType();

        if (type.IsAssignableFrom(typeof(DateTime)))
        {
            return THelper.ChangeType<DateTime>(input);
        }

        if (type.IsAssignableFrom(typeof(DateTime?)))
        {
            return THelper.ChangeType<DateTime?>(input);
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset)))
        {
            return ConvertFromDateTimeOffset(THelper.ChangeType<DateTimeOffset>(input));
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset?)))
        {
            return ConvertFromDateTimeOffset(THelper.ChangeType<DateTimeOffset?>(input).Value);
        }

#if NET6_0_OR_GREATER

        if (type.IsAssignableFrom(typeof(DateOnly)))
        {
            return THelper.ChangeType<DateOnly>(input)
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Local);
        }

        if (type.IsAssignableFrom(typeof(DateOnly?)))
        {
            return THelper.ChangeType<DateOnly?>(input).Value
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Local);
        }

        if (type.IsAssignableFrom(typeof(TimeOnly)))
        {
            return DateOnly.FromDateTime(DateTime.MinValue)
                    .ToDateTime(THelper.ChangeType<TimeOnly>(input), DateTimeKind.Local);
        }

        if (type.IsAssignableFrom(typeof(TimeOnly?)))
        {
            return DateOnly.FromDateTime(DateTime.MinValue)
                    .ToDateTime(THelper.ChangeType<TimeOnly?>(input).Value, DateTimeKind.Local);
        }

#endif
        throw new NotSupportedException($"{type.FullName} not supported");
    }

    public static bool IsNullable<TValue>()
    {
        Type type = typeof(TValue);
        if (type.IsAssignableFrom(typeof(DateTime?)) || type.IsAssignableFrom(typeof(DateTime?[]))
            || type.IsAssignableFrom(typeof(DateTimeOffset?)) || type.IsAssignableFrom(typeof(DateTimeOffset?[])))
        {
            return true;
        }
#if NET6_0_OR_GREATER
        if (type.IsAssignableFrom(typeof(DateOnly?)) || type.IsAssignableFrom(typeof(DateOnly?[]))
            || type.IsAssignableFrom(typeof(TimeOnly?)) || type.IsAssignableFrom(typeof(TimeOnly?[])))
        {
            return true;
        }
#endif
        return false;
    }

    public static DateTime?[] ToDateTimeArray<TValue>(TValue input)
    {
        if (input is not Array inputArr)
        {
            return null;
        }

        var first = ToDateTime(inputArr.GetValue(0));
        var second = ToDateTime(inputArr.GetValue(1));

        return new DateTime?[] { first, second };
    }

    public static bool SequenceEqual<TValue>(TValue first, TValue second)
    {
        var type = typeof(TValue);

        if (type.IsAssignableFrom(typeof(DateTime[])))
        {
            var firstDateTime = THelper.ChangeType<DateTime[]>(first);
            var secondDateTime = THelper.ChangeType<DateTime[]>(second);
            return Enumerable.SequenceEqual(firstDateTime, secondDateTime);
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset[])))
        {
            var firstDateTimeOffset = THelper.ChangeType<DateTimeOffset[]>(first);
            var secondDateTimeOffset = THelper.ChangeType<DateTimeOffset[]>(second);
            return Enumerable.SequenceEqual(firstDateTimeOffset, secondDateTimeOffset);
        }
        if (type.IsAssignableFrom(typeof(DateTime?[])))
        {
            var firstDateTime = THelper.ChangeType<DateTime?[]>(first);
            var secondDateTime = THelper.ChangeType<DateTime?[]>(second);
            return Enumerable.SequenceEqual(firstDateTime, secondDateTime);
        }

        if (type.IsAssignableFrom(typeof(DateTimeOffset?[])))
        {
            var firstDateTimeOffset = THelper.ChangeType<DateTimeOffset?[]>(first);
            var secondDateTimeOffset = THelper.ChangeType<DateTimeOffset?[]>(second);
            return Enumerable.SequenceEqual(firstDateTimeOffset, secondDateTimeOffset);
        }

#if NET6_0_OR_GREATER

        if (type.IsAssignableFrom(typeof(DateOnly[])))
        {
            var firstDateOnly = THelper.ChangeType<DateOnly[]>(first);
            var secondDateOnly = THelper.ChangeType<DateOnly[]>(second);
            return Enumerable.SequenceEqual(firstDateOnly, secondDateOnly);
        }

        if (type.IsAssignableFrom(typeof(DateOnly?[])))
        {
            var firstDateOnly = THelper.ChangeType<DateOnly?[]>(first);
            var secondDateOnly = THelper.ChangeType<DateOnly?[]>(second);
            return Enumerable.SequenceEqual(firstDateOnly, secondDateOnly);
        }

        if (type.IsAssignableFrom(typeof(TimeOnly[])))
        {
            var firstTimeOnly = THelper.ChangeType<TimeOnly[]>(first);
            var secondTimeOnly = THelper.ChangeType<TimeOnly[]>(second);
            return Enumerable.SequenceEqual(firstTimeOnly, secondTimeOnly);
        }

        if (type.IsAssignableFrom(typeof(TimeOnly?[])))
        {
            var firstTimeOnly = THelper.ChangeType<TimeOnly?[]>(first);
            var secondTimeOnly = THelper.ChangeType<TimeOnly?[]>(second);
            return Enumerable.SequenceEqual(firstTimeOnly, secondTimeOnly);
        }

#endif
        throw new NotSupportedException($"{type.FullName} not supported");
    }

    internal static bool IsDateTimeOffsetType<TValue>()
    {
        var type = typeof(TValue);

        return type.IsAssignableFrom(typeof(DateTimeOffset))
                || type.IsAssignableFrom(typeof(DateTimeOffset?))
                || type.IsAssignableFrom(typeof(DateTimeOffset[]))
                || type.IsAssignableFrom(typeof(DateTimeOffset?[]));
    }

    public static DateTime SetKind<TValue>(DateTime input)
    {
        var type = typeof(TValue);

        if (type.IsAssignableFrom(typeof(DateTimeOffset[])) || type.IsAssignableFrom(typeof(DateTimeOffset?[]))
            || type.IsAssignableFrom(typeof(DateTimeOffset)) || type.IsAssignableFrom(typeof(DateTimeOffset?)))
        {
            return DateTime.SpecifyKind(input, DateTimeKind.Unspecified);
        }
        else
        {
            return DateTime.SpecifyKind(input, DateTimeKind.Local);
        }
    }

    private static DateTime ConvertFromDateTimeOffset(DateTimeOffset input)
    {
        if (input == DateTimeOffset.MinValue)
            return DateTime.MinValue;

        if (input == DateTimeOffset.MaxValue)
            return DateTime.MaxValue;

        if (input.Offset.Equals(TimeSpan.Zero))
            return input.UtcDateTime;

        if (input.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(input.DateTime)))
            return DateTime.SpecifyKind(input.DateTime, DateTimeKind.Local);

        return input.DateTime;
    }

    private static DateTimeOffset ConvertFromDateTime(DateTime input)
    {
        if (input == DateTime.MinValue)
            return DateTimeOffset.MinValue;

        if (input == DateTime.MaxValue)
            return DateTimeOffset.MaxValue;

        if (input.Date == DateTime.MinValue.Date)
            return new DateTimeOffset(DateTime.SpecifyKind(input, DateTimeKind.Unspecified), TimeSpan.Zero);

        if (input.Kind == DateTimeKind.Unspecified)
            return new DateTimeOffset(input, TimeSpan.Zero);

        return new DateTimeOffset(input);
    }

}
