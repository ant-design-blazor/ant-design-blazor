// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Core.Helpers
{
    internal static class PropertyAccessHelper
    {
        public const string DefaultPathSeparator = ".";

        private const string Nullable_HasValue = "HasValue";

        private const string Nullable_Value = "Value";

        private const string CountPropertyName = "Count";

        private const string GetItemMethodName = "get_Item";

        #region Build not Nullable delegate

        public static LambdaExpression BuildAccessPropertyLambdaExpression([NotNull] this Type type, [NotNull] string propertyPath)
        {
            return BuildAccessPropertyLambdaExpression(type, propertyPath.Split(DefaultPathSeparator));
        }

        public static LambdaExpression BuildAccessPropertyLambdaExpression([NotNull] this Type type, [NotNull] string[] properties)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));

            var parameterExpression = Expression.Parameter(type);
            var expression = AccessProperty(parameterExpression, properties);
            return Expression.Lambda(expression, parameterExpression);
        }

        public static Expression<Func<TParam, TReturn>> BuildAccessPropertyLambdaExpression<TParam, TReturn>([NotNull] this Type type, [NotNull] string propertyPath)
        {
            return BuildAccessPropertyLambdaExpression<TParam, TReturn>(type, propertyPath.Split(DefaultPathSeparator));
        }

        public static Expression<Func<TParam, TReturn>> BuildAccessPropertyLambdaExpression<TParam, TReturn>([NotNull] this Type type, [NotNull] string[] properties)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));

            var parameterExpression = Expression.Parameter(type);
            var expression = AccessProperty(parameterExpression, properties);
            return Expression.Lambda<Func<TParam, TReturn>>(expression, parameterExpression);
        }

        public static Expression<Func<TParam, TReturn>> BuildAccessPropertyLambdaExpression<TParam, TReturn>(
            [NotNull] this ParameterExpression parameterExpression,
            [NotNull] string[] properties)
        {
            ArgumentNotNull(parameterExpression, nameof(parameterExpression));

            var expression = AccessProperty(parameterExpression, properties);
            return Expression.Lambda<Func<TParam, TReturn>>(expression, parameterExpression);
        }

        #endregion Build not Nullable delegate

        #region Build Nullable delegate

        public static LambdaExpression BuildAccessNullablePropertyLambdaExpression([NotNull] this Type type, [NotNull] string propertyPath)
        {
            return BuildAccessNullablePropertyLambdaExpression(type, propertyPath.Split(DefaultPathSeparator));
        }

        public static LambdaExpression BuildAccessNullablePropertyLambdaExpression([NotNull] this Type type, [NotNull] string[] properties)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));
            var parameterExpression = Expression.Parameter(type);
            var expression = AccessNullableProperty(parameterExpression, properties);
            return Expression.Lambda(expression, parameterExpression);
        }

        public static Expression<Func<TParam, TReturn>> BuildAccessNullablePropertyLambdaExpression<TParam, TReturn>([NotNull] this Type type, [NotNull] string propertyPath)
        {
            return BuildAccessNullablePropertyLambdaExpression<TParam, TReturn>(type, propertyPath.Split(DefaultPathSeparator));
        }

        public static Expression<Func<TParam, TReturn>> BuildAccessNullablePropertyLambdaExpression<TParam, TReturn>([NotNull] this Type type, [NotNull] string[] properties)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));
            var parameterExpression = Expression.Parameter(type);
            var expression = AccessNullableProperty(parameterExpression, properties);
            return Expression.Lambda<Func<TParam, TReturn>>(expression, parameterExpression);
        }

        public static Expression<Func<TParam, TReturn>> BuildAccessNullablePropertyLambdaExpression<TParam, TReturn>(
            [NotNull] this ParameterExpression parameterExpression,
            [NotNull] string[] properties)
        {
            ArgumentNotNull(parameterExpression, nameof(parameterExpression));

            var expression = AccessNullableProperty(parameterExpression, properties);
            return Expression.Lambda<Func<TParam, TReturn>>(expression, parameterExpression);
        }

        #endregion Build Nullable delegate

        #region Extension method

        public static Delegate ToDelegate([NotNull] this Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            return ToLambdaExpression(expression).Compile();
        }

        public static LambdaExpression ToLambdaExpression([NotNull] this Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            return Expression.Lambda(expression, GetRootParameterExpression(expression));
        }

        public static Expression<Func<TItem, TProp>> ToFuncExpression<TItem, TProp>([NotNull] this Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            return Expression.Lambda<Func<TItem, TProp>>(expression, GetRootParameterExpression(expression));
        }

        #endregion Extension method

        #region DefaultValue handle

        public static Expression AccessPropertyDefaultIfNull<TValue>([NotNull] this Type type, [NotNull] string properties, [NotNull] string separator, TValue defaultValue)
        {
            return AccessPropertyDefaultIfNull(type, properties.Split(separator), defaultValue);
        }

        public static Expression AccessPropertyDefaultIfNull<TValue>([NotNull] this Type type, [NotNull] string properties, TValue defaultValue)
        {
            return AccessPropertyDefaultIfNull(type, properties.Split(DefaultPathSeparator), defaultValue);
        }

        public static Expression AccessPropertyDefaultIfNull<TValue>([NotNull] this Type type, [NotNull] string[] properties, TValue defaultValue)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));

            var valueType = typeof(TValue);
            var propertyExp = AccessNullableProperty(type, properties); // will get Nullable<T> or class
            if (propertyExp.Type.IsValueType)
            {
                // Nullable Value Type
                var defaultValueUnderlyingType = Nullable.GetUnderlyingType(valueType);
                var defaultValueTypeIsNullable = defaultValueUnderlyingType != null;
                if (defaultValueTypeIsNullable)
                {
                    var propertyUnderlyingType = Nullable.GetUnderlyingType(propertyExp.Type);
                    if (propertyUnderlyingType != defaultValueUnderlyingType)
                    {
                        throw new InvalidOperationException($"default value type doesn't match the property type: property type '{propertyUnderlyingType?.Name}', default value type '{defaultValueUnderlyingType?.Name}'");
                    }
                }

                if (propertyExp is UnaryExpression ue)
                {
                    var test = Expression.IsTrue(Expression.Property(ue, Nullable_HasValue));
                    Expression trueResult = defaultValueTypeIsNullable ? ue : Expression.Property(ue, Nullable_Value);
                    var falseResult = Expression.Constant(defaultValue, valueType);
                    return Expression.Condition(test, trueResult, falseResult);
                }
                else if (propertyExp is ConditionalExpression ce)
                {
                    var test = Expression.IsTrue(Expression.Property(ce, Nullable_HasValue));
                    var trueResult = defaultValueTypeIsNullable ? ce.IfTrue : Expression.Property(ce.IfTrue, Nullable_Value);
                    var falseResult = defaultValueTypeIsNullable ? ce.IfFalse : Expression.Constant(defaultValue, valueType);
                    return Expression.Condition(test, trueResult, falseResult);
                }
                else if (propertyExp is MemberExpression me)
                {
                    var test = Expression.IsTrue(Expression.Property(me, Nullable_HasValue));
                    Expression trueResult = defaultValueTypeIsNullable ? me : Expression.Property(me, Nullable_Value);
                    var falseResult = Expression.Constant(defaultValue, valueType);
                    return Expression.Condition(test, trueResult, falseResult);
                }
                else
                {
                    throw new NotImplementedException($"Unexpected expression type {propertyExp.GetType().Name}");
                }
            }
            else
            {
                // Class
                var defaultValueType = typeof(TValue);
                var propertyType = propertyExp.Type;
                if (defaultValueType != propertyType)
                {
                    throw new InvalidOperationException($"default value type doesn't match the property type: property type '{propertyType?.Name}', default value type '{defaultValueType.Name}'");
                }

                var test = Expression.NotEqual(propertyExp, Expression.Constant(null, propertyType));
                var trueResult = propertyExp;
                var falseResult = Expression.Constant(defaultValue, defaultValueType);
                return Expression.Condition(test, trueResult, falseResult);
            }
        }

        #endregion DefaultValue handle

        #region Access nullable property

        public static Expression AccessNullableProperty([NotNull] this Type type, [NotNull] string propertyPath)
        {
            return AccessNullableProperty(type, propertyPath.Split(DefaultPathSeparator));
        }

        public static Expression AccessNullableProperty([NotNull] this Type type, [NotNull] string propertyPath, [NotNull] string separator)
        {
            return AccessNullableProperty(type, propertyPath.Split(separator));
        }

        public static Expression AccessNullableProperty([NotNull] this Type type, [NotNull] string[] properties)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));

            var paramExp = Expression.Parameter(type);
            return AccessNullableProperty(paramExp, properties);
        }

        public static Expression AccessNullableProperty([NotNull] this Expression parameterExpression, [NotNull] string propertyPath)
        {
            return AccessNullableProperty(parameterExpression, propertyPath.Split(DefaultPathSeparator));
        }

        public static Expression AccessNullableProperty([NotNull] this Expression parameterExpression, [NotNull] string propertyPath, [NotNull] string separator)
        {
            return AccessNullableProperty(parameterExpression, propertyPath.Split(separator));
        }

        public static Expression AccessNullableProperty([NotNull] this Expression parameterExpression, [NotNull] string[] properties)
        {
            ArgumentNotNull(parameterExpression, nameof(parameterExpression));
            ArgumentNotEmpty(properties, nameof(properties));

            Expression access = parameterExpression;
            foreach (var property in properties)
            {
                var indexAccess = ParseIndexAccess(property);
                access = indexAccess.HasValue
                             ? AccessIndex(access, indexAccess.Value.propertyName, indexAccess.Value.indexes)
                             : AccessNext(access, property);
            }

            static Expression AccessIndex(Expression member, string propertyName, Expression[] indexes)
            {
                switch (propertyName)
                {
                    case { Length: 0 }:
                        {
                            foreach (var index in indexes)
                            {
                                member = member.IndexableGetNullableItem(index);
                            }

                            return member;
                        }
                    default:
                        {
                            member = AccessNext(member, propertyName);
                            foreach (var index in indexes)
                            {
                                member = member.IndexableGetNullableItem(index);
                            }

                            return member;
                        }
                }
            }

            static Expression AccessNext(Expression member, string property)
            {
                if (member.Type.IsValueType)
                {
                    if (Nullable.GetUnderlyingType(member.Type) == null)
                    {
                        // Value Type
                        return member.ValueTypeGetProperty(property);
                    }
                    else
                    {
                        // Nullable Value Type
                        return member.NullableTypeGetPropOrNull(property);
                    }
                }
                else
                {
                    // Class
                    return member.ClassGetPropertyOrNull(property);
                }
            }

            return TryConvertToNullable(access);
        }

        #endregion Access nullable property

        #region Access not null property

        public static Expression AccessProperty([NotNull] this Type type, [NotNull] string propertyPath)
        {
            return AccessProperty(type, propertyPath.Split(DefaultPathSeparator));
        }

        public static Expression AccessProperty([NotNull] this Type type, [NotNull] string propertyPath, [NotNull] string separator)
        {
            return AccessProperty(type, propertyPath.Split(separator));
        }

        public static Expression AccessProperty([NotNull] this Type type, [NotNull] string[] properties)
        {
            ArgumentNotNull(type, nameof(type));
            ArgumentNotEmpty(properties, nameof(properties));

            var paramExp = Expression.Parameter(type);
            return AccessProperty(paramExp, properties);
        }

        public static Expression AccessProperty([NotNull] this Expression parameterExpression, [NotNull] string propertyPath)
        {
            return AccessProperty(parameterExpression, propertyPath.Split(DefaultPathSeparator));
        }

        public static Expression AccessProperty([NotNull] this Expression parameterExpression, [NotNull] string propertyPath, [NotNull] string separator)
        {
            return AccessProperty(parameterExpression, propertyPath.Split(separator));
        }

        public static Expression AccessProperty(this Expression parameterExpression, string[] properties)
        {
            ArgumentNotNull(parameterExpression, nameof(parameterExpression));
            ArgumentNotEmpty(properties, nameof(properties));

            Expression access = parameterExpression;
            foreach (var property in properties)
            {
                var indexAccess = ParseIndexAccess(property);
                if (!indexAccess.HasValue)
                {
                    access = AccessNext(access, property);
                }
                else
                {
                    access = AccessIndex(access, indexAccess.Value.propertyName, indexAccess.Value.indexes);
                }
            }

            static Expression AccessIndex(Expression member, string propertyName, Expression[] indexes)
            {
                switch (propertyName)
                {
                    case { Length: 0 }:
                        {
                            return indexes.Aggregate(member, (current, index) => current.IndexableGetItem(index));
                        }
                    default:
                        {
                            member = AccessNext(member, propertyName);
                            return indexes.Aggregate(member, (current, index) => current.IndexableGetItem(index));
                        }
                }
            }

            static Expression AccessNext(Expression member, string property)
            {
                // Not index access
                if (member.Type.IsValueType)
                {
                    return Nullable.GetUnderlyingType(member.Type) == null
                               ? member.ValueTypeGetProperty(property)     // Not Nullable
                               : member.NullableTypeGetProperty(property); // Is Nullable
                }
                else
                {
                    // Class
                    return member.ClassGetProperty(property);
                }
            }

            return access;
        }

        #endregion Access not null property

        //
        // Branch
        // 1. class : C?
        // 2. not null value type : V
        // 3. nullable value type : N?
        //
        // C!.Prop
        // C?.Prop
        //
        // V.Prop
        //
        // N!.Value
        // N!.Value.Prop
        // N?.Value
        //

        #region Property Access

        /// <summary>
        /// C.Prop
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="property"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private static MemberExpression ClassGetProperty(this Expression expression, string property)
        {
            IsClass(expression);

            if (!expression.Type.IsClass)
            {
                throw new InvalidOperationException($"{nameof(expression)} {expression.Type.Name} must be class");
            }

            var exp = Expression.Property(expression, property); // C.Prop
            return exp;
        }

        /// <summary>
        /// C?.Prop
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static ConditionalExpression ClassGetPropertyOrNull([NotNull] this Expression expression, string property)
        {
            ArgumentNotNull(expression, nameof(expression));

            IsClass(expression);
            var test = Expression.NotEqual(expression, Expression.Constant(null, expression.Type)); // E: C == null
            var propExp = Expression.Property(expression, property);                                // C.Prop
            var propIsValueType = propExp.Type.IsValueType && Nullable.GetUnderlyingType(propExp.Type) == null;
            Expression trueResult = propIsValueType
                                        ? Expression.Convert(propExp, typeof(Nullable<>).MakeGenericType(propExp.Type)) // T: Prop is VT: (Nullable<Prop>)C.Prop
                                        : propExp;                                                                      // T: Prop is class: C.Prop
            var falseResult = Expression.Constant(null, trueResult.Type);                                               // F: null
            var exp = Expression.Condition(test, trueResult, falseResult);                                              // E ? T : F;
            return exp;
        }

        /// <summary>
        /// V.Prop
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static MemberExpression ValueTypeGetProperty([NotNull] this Expression expression, string property)
        {
            ArgumentNotNull(expression, nameof(expression));

            IsValueType(expression);
            var exp = Expression.Property(expression, property); // V.Prop
            return exp;
        }

        /// <summary>
        /// NV?.Value
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static ConditionalExpression NullableTypeGetValueOrNull([NotNull] this Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            IsNullableTypeOrThrow(expression);
            var test = Expression.IsTrue(Expression.Property(expression, Nullable_HasValue));                      // E: NV.HasValue == true
            var trueResult = Expression.Convert(Expression.Property(expression, Nullable_Value), expression.Type); // T: (Nullable<T>)NV.Value
            var falseResult = Expression.Constant(null, expression.Type);                                          // F: (Nullable<T>)null
            var exp = Expression.Condition(test, trueResult, falseResult);                                         // E ? T : F
            return exp;
        }

        /// <summary>
        /// NV!.Value, maybe InvalidOperationException for no value
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static MemberExpression NullableTypeGetValue([NotNull] this Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            IsNullableTypeOrThrow(expression);
            var exp = Expression.Property(expression, nameof(Nullable<bool>.Value)); // NV!.Value
            return exp;
        }

        /// <summary>
        /// NV?.Value.Prop
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static ConditionalExpression NullableTypeGetPropOrNull([NotNull] this Expression expression, string property)
        {
            ArgumentNotNull(expression, nameof(expression));

            IsNullableTypeOrThrow(expression);
            var valueExp = NullableTypeGetValueOrNull(expression);                              // NV?.Value
            var test = Expression.NotEqual(valueExp, Expression.Constant(null, valueExp.Type)); // E: NV?.Value != null
            var propExp = NullableTypeGetProperty(valueExp.IfTrue, property);                   // Expression.Property(Expression.Property(valueExp.IfTrue, VTValue), property); // NV!.Value.Prop
            var propIsValueType = propExp.Type.IsValueType && Nullable.GetUnderlyingType(propExp.Type) == null;
            Expression trueResult = propIsValueType
                                        ? Expression.Convert(propExp, typeof(Nullable<>).MakeGenericType(propExp.Type)) // T: Prop is VT: (Nullable<Prop>)NV!.Value.Prop
                                        : propExp;                                                                      // T: Prop is class: NV!.Value.Prop
            var falseResult = Expression.Constant(null, trueResult.Type);                                               // F: (Nullable<Prop>)null
            var exp = Expression.Condition(test, trueResult, falseResult);                                              //  NV?.Value != null ? NV!.Value.Prop : null
            return exp;
        }

        /// <summary>
        /// NV!.Value.Prop, maybe InvalidOperationException for no value
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static MemberExpression NullableTypeGetProperty([NotNull] this Expression expression, string property)
        {
            ArgumentNotNull(expression, nameof(expression));

            IsNullableTypeOrThrow(expression);
            var nvValue = expression.NullableTypeGetValue();  // NV!.Value
            var exp = Expression.Property(nvValue, property); // NV!.Value.Prop
            return exp;
        }

        private static Expression IndexableGetItem([NotNull] this Expression expression, Expression index)
        {
            var getItemMethod = expression.Type.GetMethod(GetItemMethodName, BindingFlags.Public | BindingFlags.Instance);
            if (getItemMethod != null)
            {
                var getItemMethodCall = Expression.Call(expression, getItemMethod, index);
                return getItemMethodCall;
            }
            else if (expression.Type.IsArray)
            {
                var indexAccess = Expression.ArrayIndex(expression, index);
                return indexAccess;
            }

            throw new InvalidOperationException($"Not supported type '{expression.Type.Name}' for index access");
        }

        private static Expression IndexableGetNullableItem([NotNull] this Expression expression, [NotNull] Expression index)
        {
            // Array
            if (expression.Type.IsArray)
            {
                if (index.Type != typeof(int))
                {
                    throw new InvalidOperationException($"Array must be indexed by 'int', but get '{expression.Type.Name}'");
                }

                var lengthAccess = Expression.ArrayLength(expression);
                var test = Expression.AndAlso(Expression.LessThan(index, lengthAccess), Expression.GreaterThanOrEqual(index, Expression.Constant(0, typeof(int))));
                var trueResult = Expression.ArrayIndex(expression, index);
                var falseResult = Expression.Constant(null, trueResult.Type);
                return Expression.Condition(test, trueResult, falseResult);
            }

            // Dictionary<,> like, types that have the 'ContainsKey' method and the 'get_Item' method.
            {
                var getItemMethod = expression.Type.GetMethod(GetItemMethodName, BindingFlags.Public | BindingFlags.Instance);
                var containsKeyMethod = expression.Type.GetMethod(nameof(IDictionary<int, int>.ContainsKey), BindingFlags.Public | BindingFlags.Instance);
                if (getItemMethod != null && containsKeyMethod != null)
                {
                    var containsKeyCall = Expression.Call(expression, containsKeyMethod, index);
                    var test = Expression.IsTrue(containsKeyCall);
                    var trueResult = Expression.Call(expression, getItemMethod, index);
                    var falseResult = Expression.Constant(null, trueResult.Type);
                    return Expression.Condition(test, trueResult, falseResult);
                }
            }

            // Lisk like, types that have the 'get_Item' method and the 'Count' property.
            {
                var getItemMethod = expression.Type.GetMethod(GetItemMethodName, BindingFlags.Public | BindingFlags.Instance);
                var countProperty = expression.Type.GetProperty(CountPropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (getItemMethod != null && countProperty != null)
                {
                    if (index.Type != typeof(int))
                    {
                        throw new InvalidOperationException($"{expression.Type.Name} must be indexable by 'int', but get '{index.Type.Name}'");
                    }

                    var countAccess = Expression.Property(expression, countProperty);
                    var test = Expression.AndAlso(Expression.LessThan(index, countAccess), Expression.GreaterThanOrEqual(index, Expression.Constant(0, typeof(int))));
                    var trueResult = Expression.Call(expression, getItemMethod, index);
                    var falseResult = Expression.Constant(null, trueResult.Type);
                    return Expression.Condition(test, trueResult, falseResult);
                }
            }

            throw new InvalidOperationException($"Not supported type '{expression.Type.Name}' for index accessing");
        }

        #endregion Property Access

        #region Type Validate

        /// <summary>
        /// Check if expression.Type is class, otherwise throw and exception
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static void IsClass([NotNull] Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            if (!expression.Type.IsClass)
            {
                throw new InvalidOperationException($"{nameof(expression)} {expression.Type.Name} must be class");
            }
        }

        /// <summary>
        /// Check if expression.Type is ValueType and not Nullable&lt;T&gt;, otherwise throw and exception
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static void IsValueType([NotNull] Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            if (expression.Type.IsValueType && Nullable.GetUnderlyingType(expression.Type) != null)
            {
                throw new InvalidOperationException($"{nameof(expression)} {expression.Type.Name} must be value type");
            }
        }

        /// <summary>
        /// Check if expression.Type is Nullable&lt;T&gt;, otherwise throw and exception
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static void IsNullableTypeOrThrow([NotNull] Expression expression)
        {
            ArgumentNotNull(expression, nameof(expression));

            if (Nullable.GetUnderlyingType(expression.Type) == null)
            {
                throw new InvalidOperationException($"{nameof(expression)} {expression.Type.Name} must be typeof Nullable<T>");
            }
        }

        #endregion Type Validate

        #region Utils

        private static void ArgumentNotNull<T>(in T arg, string argName)
            where T : class
        {
            if (arg == default)
            {
                throw new ArgumentNullException(argName);
            }
        }

        private static void ArgumentNotEmpty<T>(in T[] arg, string argName)
            where T : class
        {
            if (arg == default || arg is { Length: 0 })
            {
                throw new ArgumentException("Value cannot be an empty collection or null.", argName);
            }
        }

        /// <summary>
        /// expression should be like: ParameterExpression->MemberExpression1->MemberExpression2... ,
        /// if the root for 'expression' is not ParameterExpression, this will return null.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static ParameterExpression GetRootParameterExpression([NotNull] Expression expression)
        {
            return expression switch
            {
                MemberExpression { Expression: { } } memberExp => GetRootParameterExpression(memberExp.Expression),
                ConditionalExpression conditionalExp => GetRootParameterExpression(conditionalExp.IfTrue),
                UnaryExpression unaryExp => GetRootParameterExpression(unaryExp.Operand),
                ParameterExpression paramExp => paramExp,
                _ => throw new NullReferenceException()
            };
        }

        /// <summary>
        /// Try convert Expression type to Nullable type, only Non-Nullable ValueType can be converted
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Expression TryConvertToNullable([NotNull] Expression expression)
        {
            if (expression.Type.IsValueType && Nullable.GetUnderlyingType(expression.Type) == null)
            {
                return Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));
            }

            return expression;
        }

        /// <summary>
        /// Check if property string has index operation and parse to Expression
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static (string propertyName, Expression[] indexes)? ParseIndexAccess(string property)
        {
            const string IndexAccessErrorTemplate = "Invalid index property: {0}, index must be like 'prop[key]' or 'prop[key][key]...'";
            if (!property.EndsWith(']'))
            {
                return null;
            }

            property = property.Replace(' ', '\0');
            var sp = property.AsSpan();
            var begin = 0;
            var end = sp.IndexOf('[');
            if (end == -1)
            {
                throw new InvalidOperationException(string.Format(IndexAccessErrorTemplate, property));
            }

            var propertyName = sp.Slice(begin, end).ToString(); // from 0 to index of first '[' treat as property name
            sp = sp.Slice(end);
            var leftCount = 0;
            var rightCount = 0;
            for (int i = 0; i < sp.Length; i++)
            {
                if (sp[i] == '[')
                {
                    leftCount++;
                }
                else if (sp[i] == ']')
                {
                    rightCount++;
                }
            }

            if (leftCount != rightCount)
            {
                throw new InvalidOperationException(string.Format(IndexAccessErrorTemplate, property));
            }

            var indexes = new Expression[leftCount];
            var count = 0;
            while (sp.Length != 0)
            {
                var indexBegin = sp.IndexOf('[');
                var indexEnd = sp.IndexOf(']');
                if (indexBegin == -1 || indexEnd == -1 || indexBegin + 1 == indexEnd) // ']' not found or found "[]" which is incorrect
                {
                    throw new InvalidOperationException();
                }

                var idxSpan = sp.Slice(indexBegin + 1, indexEnd - indexBegin - 1);
                Expression indexExp;
                if (idxSpan[0] == '"' && idxSpan[^1] == '"')
                {
                    // is string key
                    var key = idxSpan.Slice(1, idxSpan.Length - 2).ToString();
                    indexExp = Expression.Constant(key, typeof(string));
                }
                else
                {
                    // is int key
                    if (!int.TryParse(idxSpan, out var indexNum))
                    {
                        throw new InvalidOperationException(string.Format(IndexAccessErrorTemplate, property));
                    }

                    indexExp = Expression.Constant(indexNum, typeof(int));
                }

                indexes[count] = indexExp;
                count++;
                sp = sp.Slice(indexEnd + 1);
            }

            return (propertyName, indexes);
        }

        #endregion Utils
    }
}
