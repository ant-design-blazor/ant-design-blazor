using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
                new[] {toExpression},
                Expression.Assign(toExpression, fromExpression)
            );

            return Expression.Lambda<Func<TFrom, TTo>>(blockExpression, fromExpression).Compile()(fromValue);
        }
    }
}
