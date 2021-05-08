using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign
{
    public static class EnumHelper<T>
    {
        private static readonly Func<T, T, T> _aggregateFunction;

        static EnumHelper()
        {
            _aggregateFunction = BuildAggregateFunction();
        }

        // There is no constraint or type check for type parameter T, be sure that T is an enumeration type  
        public static T Combine(IEnumerable<T> enumValues)
        {
            if (enumValues?.Count() > 0)
            {
                return enumValues.Aggregate(_aggregateFunction);
            }
            else
            {
                return default;
            }
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
