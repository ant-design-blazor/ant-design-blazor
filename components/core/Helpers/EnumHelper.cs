using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign
{
    public static class EnumHelper
    {
        private static readonly ConcurrentDictionary<Type, Delegate> _aggregateFunctionCache = new();

        public static T Combine<T>(IEnumerable<T> enumValues)
        {
            var type = typeof(T);
            var enumType = THelper.GetUnderlyingType<T>();
            if (enumType.IsEnum == false)
            {
                throw new ArgumentException("Type parameter T should be of enumeration types");
            }
            if (enumValues?.Count() > 0)
            {
                return enumValues.Aggregate((Func<T, T, T>)_aggregateFunctionCache.GetOrAdd(type, t => BuildAggregateFunction(t, enumType)));
            }
            else
            {
                return default;
            }
        }

        private static Delegate BuildAggregateFunction(Type type, Type enumType)
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var param1 = Expression.Parameter(type);
            var param2 = Expression.Parameter(type);
            var body = Expression.Convert(
                Expression.Or(
                    Expression.Convert(param1, underlyingType),
                    Expression.Convert(param2, underlyingType)),
                type);
            return Expression.Lambda(body, param1, param2).Compile();
        }
    }
}
