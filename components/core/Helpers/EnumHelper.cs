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

        private static IEnumerable<T> _valueList;

        static EnumHelper()
        {
            _aggregateFunction = BuildAggregateFunction();
            _valueList = Enum.GetValues(typeof(T)).Cast<T>();
        }

        // There is no constraint or type check for type parameter T, be sure that T is an enumeration type  
        public static object Combine(IEnumerable<T> enumValues)
        {
            if (enumValues?.Count() > 0)
            {
                return enumValues.Aggregate(_aggregateFunction);
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<T> GetValueList()
        {
            return _valueList;
        }

        public static string GetDisplayName(T t)
        {
            var fieldInfo = typeof(T).GetField(t.ToString());
            return fieldInfo.GetCustomAttribute<DisplayAttribute>(true)?.Name ??
                 fieldInfo.Name;

        }

        private static Func<T, T, T> BuildAggregateFunction()
        {
            var type = typeof(T);
            var enumType = THelper.GetUnderlyingType<T>();
            var underlyingType = Enum.GetUnderlyingType(enumType);
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
