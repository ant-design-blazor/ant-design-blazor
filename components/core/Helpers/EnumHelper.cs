using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign
{
    public static class EnumHelper
    {
        public static T Combine<T>(IEnumerable<T> enumValues)
        {
            var enumType = THelper.GetUnderlyingType<T>();
            if (enumType.IsEnum == false)
            {
                throw new ArgumentException("EnumHelper.Combine only supports enumeration types");
            }
            if (enumValues?.Count() > 0)
            {
                var underlyingType = Enum.GetUnderlyingType(enumType);
                var param1Expression = Expression.Parameter(typeof(T));
                var param2Expression = Expression.Parameter(typeof(T));
                var param1ConvertExpression = Expression.Convert(param1Expression, underlyingType);
                var param2ConvertExpression = Expression.Convert(param2Expression, underlyingType);
                var orExpression = Expression.Or(param1ConvertExpression, param2ConvertExpression);
                var bodyExpression = Expression.Convert(orExpression, typeof(T));
                var lambdaExpression = (Expression<Func<T, T, T>>)Expression.Lambda(bodyExpression, param1Expression, param2Expression);
                return enumValues.Aggregate(lambdaExpression.Compile());
            }
            else
            {
                return default;
            }
        }
    }
}
