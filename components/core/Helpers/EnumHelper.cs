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
        private static readonly Func<T, T, T> _aggregateFunction;

        private static readonly IEnumerable<T> _valueList;
        private static readonly IEnumerable<(T Value, string Label)> _valueLabelList;
        private static readonly Type _enumType;

        static EnumHelper()
        {
            _enumType = THelper.GetUnderlyingType<T>();
            _aggregateFunction = BuildAggregateFunction();
            _valueList = Enum.GetValues(_enumType).Cast<T>();
            _valueLabelList = _valueList.Select(value => (value, GetDisplayName(value)));
        }

        // There is no constraint or type check for type parameter T, be sure that T is an enumeration type
        public static object Combine(IEnumerable<T> enumValues)
        {
            if (enumValues?.Any() == true)
            {
                return enumValues.Aggregate(_aggregateFunction);
            }
            return null;
        }

        public static IEnumerable<T> Split(object enumValue)
        {
            var str = enumValue?.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return Array.Empty<T>();
            }
            return str.Split(',').Select(x => (T)Enum.Parse(_enumType, x)).ToArray();
        }

        public static IEnumerable<T> GetValueList()
        {
            return _valueList;
        }

        public static IEnumerable<(T Value, string Label)> GetValueLabelList()
        {
            return _valueLabelList;
        }

        public static string GetDisplayName(T enumValue)
        {
            var enumName = Enum.GetName(_enumType, enumValue);
            var fieldInfo = _enumType.GetField(enumName);
            return fieldInfo.GetCustomAttribute<DisplayAttribute>(true)?.GetName() ?? enumName;
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
    }
}
