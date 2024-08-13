// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign
{
    public static class EnumHelper<T>
    {
        private static readonly MethodInfo _enumHasFlag = typeof(Enum).GetMethod(nameof(Enum.HasFlag));

        private static readonly Func<T, T, T> _aggregateFunction;
        private static readonly Func<T, T, bool> _hasFlagFunction;

        private static readonly IEnumerable<T> _valueList;
        private static readonly IEnumerable<(T Value, string Label)> _valueLabelList;
        private static readonly Type _enumType;
        private static readonly bool _isFlags;

        public static bool IsFlags => _isFlags;

        static EnumHelper()
        {
            _enumType = THelper.GetUnderlyingType<T>();
            _aggregateFunction = BuildAggregateFunction();
            _valueList = Enum.GetValues(_enumType).Cast<T>();
            _valueLabelList = _valueList.Select(value => (value, EnumHelper.GetDisplayName(_enumType, value)));
            _isFlags = _enumType.GetCustomAttribute<FlagsAttribute>() != null;
            _hasFlagFunction = BuildHasFlagFunction();
        }

        // There is no constraint or type check for type parameter T, be sure that T is an enumeration type
        public static object Combine(IEnumerable<T> enumValues)
        {
            if (enumValues?.Any() == true)
            {
                return enumValues.Aggregate(_aggregateFunction);
            }
            return default(T);
        }

        public static IEnumerable<T> Split(object enumValue)
        {
            if (enumValue == null)
            {
                return Array.Empty<T>();
            }
            if (enumValue is string enumString)
            {
                return _valueList.Where(value => enumString.Split(",").Contains(Enum.GetName(_enumType, value)));
            }
            return _valueList.Where(value => _hasFlagFunction((T)enumValue, value));
        }

        public static IEnumerable<T> GetValueList()
        {
            return _valueList;
        }

        public static IEnumerable<(T Value, string Label)> GetValueLabelList()
        {
            return _valueLabelList;
        }

        public static string GetDisplayName<TEnum>(TEnum item)
        {
            return EnumHelper.GetDisplayName(_enumType, item);
        }

        private static Func<T, T, T> BuildAggregateFunction()
        {
            var type = typeof(T);
            var underlyingType = Enum.GetUnderlyingType(_enumType);
            var param1 = Expression.Parameter(type);
            var param2 = Expression.Parameter(type);
            var body = Expression.Convert(
                Expression.Or(
                    Expression.Convert(param1, underlyingType),
                    Expression.Convert(param2, underlyingType)),
                type);
            return Expression.Lambda<Func<T, T, T>>(body, param1, param2).Compile();
        }

        private static Func<T, T, bool> BuildHasFlagFunction()
        {
            var type = typeof(T);
            var param1 = Expression.Parameter(type);
            var param2 = Expression.Parameter(type);

            if (THelper.IsTypeNullable(type))
            {
                Expression notNull = Expression.NotEqual(param1, Expression.Constant(null));
                var param1Value = Expression.MakeMemberAccess(param1, param1.Type.GetMember(nameof(Nullable<int>.Value))[0]);
                var param1ValueHasFlags = Expression.Call(param1Value, _enumHasFlag, Expression.Convert(param2, typeof(Enum)));
                var notNullAndParam1ValueHasFlags = Expression.AndAlso(notNull, param1ValueHasFlags);

                return Expression.Lambda<Func<T, T, bool>>(notNullAndParam1ValueHasFlags, param1, param2).Compile();
            }
            else
            {
                var body = Expression.Call(param1, _enumHasFlag, Expression.Convert(param2, typeof(Enum)));
                return Expression.Lambda<Func<T, T, bool>>(body, param1, param2).Compile();
            }
        }
    }

    internal static class EnumHelper
    {
        public static string GetDisplayName(Type enumType, object enumValue)
        {
            var enumName = Enum.GetName(enumType, enumValue);
            var fieldInfo = enumType.GetField(enumName);
            return fieldInfo.GetCustomAttribute<DisplayAttribute>(true)?.GetName() ?? enumName;
        }
    }
}
