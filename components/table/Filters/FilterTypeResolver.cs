// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Filters;

public interface IFieldFilterTypeResolver
{
    public IFieldFilterType Resolve<T>();

    public IFieldFilterType Resolve(Type underlyingType);
}

public class DefaultFieldFilterTypeResolver : IFieldFilterTypeResolver
{
    public IFieldFilterType Resolve<T>()
    {
        var underlyingType = THelper.GetUnderlyingType<T>();

        return underlyingType switch
        {
            _ when underlyingType.IsEnum => new EnumFieldFilterType<T>(),
            _ => Resolve(underlyingType),
        };
    }

    public IFieldFilterType Resolve(Type underlyingType)
    {
        return InternalFieldFilterTypeResolver.Resolve(underlyingType);
    }
}

internal static class InternalFieldFilterTypeResolver
{
    public static IFieldFilterType Resolve<T>()
    {
        var underlyingType = THelper.GetUnderlyingType<T>();

        return underlyingType switch
        {
            _ when underlyingType.IsEnum => new EnumFieldFilterType<T>(),
            _ => Resolve(underlyingType),
        };
    }

    public static IFieldFilterType Resolve(Type underlyingType)
    {
#pragma warning disable format
        return underlyingType switch
        {
            // _ when underlyingType.IsEnum              => new EnumFieldFilterType<T>(),
            _ when underlyingType == typeof(byte)     => new NumberFieldFilterType<byte>(),
            _ when underlyingType == typeof(decimal)  => new NumberFieldFilterType<decimal>(),
            _ when underlyingType == typeof(double)   => new NumberFieldFilterType<double>(),
            _ when underlyingType == typeof(short)    => new NumberFieldFilterType<short>(),
            _ when underlyingType == typeof(int)      => new NumberFieldFilterType<int>(),
            _ when underlyingType == typeof(long)     => new NumberFieldFilterType<long>(),
            _ when underlyingType == typeof(sbyte)    => new NumberFieldFilterType<sbyte>(),
            _ when underlyingType == typeof(float)    => new NumberFieldFilterType<float>(),
            _ when underlyingType == typeof(ushort)   => new NumberFieldFilterType<ushort>(),
            _ when underlyingType == typeof(uint)     => new NumberFieldFilterType<uint>(),
            _ when underlyingType == typeof(ulong)    => new NumberFieldFilterType<ulong>(),
            _ when underlyingType == typeof(DateTime) => new DateTimeFieldFilterType(),
#if NET6_0_OR_GREATER
            _ when underlyingType == typeof(DateOnly) => new DateTimeFieldFilterType<DateOnly>(),
            _ when underlyingType == typeof(TimeOnly) => new DateTimeFieldFilterType<TimeOnly>() { InputAttributes = { [nameof(DatePicker<TimeOnly>.Picker)] = DatePickerType.Time } },
#endif
            _ when underlyingType == typeof(string)   => new StringFieldFilterType(),
            _ when underlyingType == typeof(Guid)     => new GuidFieldFilterType(),
            _                                         => null
        };
#pragma warning restore format
    }
}
