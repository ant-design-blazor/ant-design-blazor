// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign.core.Helpers
{
    public static class PropertyAccessHelper
    {
        /// <summary>
        /// Build property access expression, using property path string to identify the property.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="propertyPath"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static MemberExpression AccessProperty(Expression expression, string propertyPath, string separator)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (propertyPath == null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            return AccessProperty(expression, propertyPath.Split(separator));
        }

        public static MemberExpression AccessProperty<TItem>(string propertyPath, string separator)
        {
            if (propertyPath == null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            return AccessProperty(Expression.Parameter(typeof(TItem)), propertyPath.Split(separator));
        }

        /// <summary>
        /// Build property access expression, using property path string to identify the property.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="separatedPropertyPath"></param>
        /// <returns></returns>
        public static MemberExpression AccessProperty(Expression expression, string[] separatedPropertyPath)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (separatedPropertyPath == null)
            {
                throw new ArgumentNullException(nameof(separatedPropertyPath));
            }

            if (separatedPropertyPath.Length == 0)
            {
                throw new ArgumentException($"{nameof(separatedPropertyPath)} should not be empty");
            }

            return (MemberExpression)separatedPropertyPath.Aggregate(expression, Expression.Property);
        }

        public static ConditionalExpression AccessNullableProperty(Expression expression, string[] separatedPropertyPath)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (separatedPropertyPath == null)
            {
                throw new ArgumentNullException(nameof(separatedPropertyPath));
            }

            if (separatedPropertyPath.Length == 0)
            {
                throw new ArgumentException($"{nameof(separatedPropertyPath)} should not be empty");
            }

            return (ConditionalExpression)separatedPropertyPath.Aggregate(expression, CreateNullConditionalPropertyAccess);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="separatedPropertyPath"></param>
        /// <returns></returns>
        public static LambdaExpression BuildNullablePropertyAccessExpression(Type itemType, string[] separatedPropertyPath)
        {
            var expression = Expression.Parameter(itemType);
            var accessExpression = AccessNullableProperty(expression, separatedPropertyPath);
            return Expression.Lambda(accessExpression, expression);
        }

        /// <summary>
        /// return itemType t => t.A.B.C.D;
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="separatedPropertyPath"></param>
        /// <returns></returns>
        public static LambdaExpression BuildPropertyAccessExpression(Type itemType, string[] separatedPropertyPath)
        {
            var expression = Expression.Parameter(itemType);
            var accessExpression = AccessProperty(expression, separatedPropertyPath);
            return Expression.Lambda(accessExpression, expression);
        }

        /// <summary>
        /// return itemType t => t.A.B.C.D;
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="propertyPath"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LambdaExpression BuildPropertyAccessExpression(Type itemType, string propertyPath, string separator)
        {
            if (propertyPath == null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            return BuildPropertyAccessExpression(itemType, propertyPath.Split(separator));
        }

        /// <summary>
        /// return TItem t => t.A.B.C.D; D is TProp
        /// </summary>
        /// <param name="separatedPropertyPath"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Expression<Func<TItem, TProp>> BuildPropertyAccessExpression<TItem, TProp>(string[] separatedPropertyPath)
        {
            if (separatedPropertyPath == null)
            {
                throw new ArgumentNullException(nameof(separatedPropertyPath));
            }

            if (separatedPropertyPath.Length == 0)
            {
                throw new ArgumentException($"{nameof(separatedPropertyPath)} should not be empty");
            }

            var expression = Expression.Parameter(typeof(TItem));
            var accessExpression = AccessProperty(expression, separatedPropertyPath);
            return Expression.Lambda<Func<TItem, TProp>>(accessExpression, expression);
        }

        /// <summary>
        /// return TItem t => t.A.B.C.D; D is TProp
        /// </summary>
        /// <param name="propertyPath"></param>
        /// <param name="separator"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Expression<Func<TItem, TProp>> BuildPropertyAccessExpression<TItem, TProp>(string propertyPath, string separator)
        {
            if (propertyPath == null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            return BuildPropertyAccessExpression<TItem, TProp>(propertyPath.Split(separator));
        }

        public static ConditionalExpression CreateNullConditionalPropertyAccess(Expression expression, string property)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            Expression propertyAccess = Expression.Property(expression, property);

            var propertyType = propertyAccess.Type;

            if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null)
            {
                propertyAccess = Expression.Convert(propertyAccess, typeof(Nullable<>).MakeGenericType(propertyType));
            }

            var nullResult = Expression.Default(propertyAccess.Type);

            var condition = Expression.Equal(expression, Expression.Constant(null, expression.Type));

            return Expression.Condition(condition, nullResult, propertyAccess);
        }

        /// <summary>
        /// expression should be like: ParameterExpression->MemberExpression1->MemberExpression2... ,
        /// it will return root ParameterExpression, otherwise return null.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ParameterExpression GetRootParameterExpression(Expression expression)
        {
            while (true)
            {
                if (expression is MemberExpression {Expression: { }} m)
                {
                    expression = m.Expression;
                }
                else if (expression is ParameterExpression parameterExpression)
                {
                    return parameterExpression;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
