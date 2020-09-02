using System;
using System.Linq.Expressions;

namespace AntDesign.core.Extensions
{
    public static class DataConvertionExtensions
    {
        public static TTo Convert<TFrom, TTo>(TFrom fromValue)
        {
            // Creating a parameter expression
            ParameterExpression fromExpression = Expression.Parameter(typeof(TFrom), "from");

            // Creating a parameter express
            ParameterExpression toExpression = Expression.Parameter(typeof(TTo), "to");

            // Creating a method body
            BlockExpression blockExpression = Expression.Block(
                new[] { toExpression },
                Expression.Assign(toExpression, fromExpression)
                );

            return Expression.Lambda<Func<TFrom, TTo>>(blockExpression, fromExpression).Compile()(fromValue);
        }
    }
}
