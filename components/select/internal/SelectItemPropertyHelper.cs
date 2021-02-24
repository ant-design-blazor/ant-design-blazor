// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Select.Internal
{
    internal static class SelectItemPropertyHelper
    {
        private const string ToStringMethodName = "ToString";

        private const string Nullable_HasValue = "HasValue";

        private const string Nullable_Value = "Value";

        internal static Func<TItem, TItemValue> CreateGetValueFunc<TItem, TItemValue>(string valueName)
        {
            var itemParamExp = Expression.Parameter(typeof(TItem));
            var labelExp = Expression.Property(itemParamExp, valueName);
            var labelPropertyType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{valueName}' should be Property");
            if (labelPropertyType.IsClass)
            {
                var funExp = Expression.Lambda<Func<TItem, TItemValue>>(labelExp, itemParamExp);
                return funExp.Compile();
            }

            var labelIsNullable = labelPropertyType.IsValueType && Nullable.GetUnderlyingType(labelPropertyType) != null;
            if (labelIsNullable)
            {
                var test = Expression.IsTrue(Expression.Property(labelExp, Nullable_HasValue));
                var trueResult = Expression.Convert(Expression.Property(labelExp, Nullable_Value), labelExp.Type);
                var falseResult = Expression.Constant(null, labelExp.Type);
                var resultExp = Expression.Condition(test, trueResult, falseResult);
                var funExp = Expression.Lambda<Func<TItem, TItemValue>>(resultExp, itemParamExp);
                return funExp.Compile();
            }
            else
            {
                var valueIsNullable = typeof(TItemValue).IsValueType && Nullable.GetUnderlyingType(typeof(TItemValue)) != null;
                Expression valueExp = valueIsNullable ? Expression.New(typeof(Nullable<>).MakeGenericType(labelExp.Type).GetConstructors()[0], labelExp) : labelExp;
                var funExp = Expression.Lambda<Func<TItem, TItemValue>>(valueExp, itemParamExp);
                return funExp.Compile();
            }
        }

        internal static Func<TItem, string> CreateGetLabelFunc<TItem>(string labelName)
        {
            var itemParamExp = Expression.Parameter(typeof(TItem));
            var labelExp = Expression.Property(itemParamExp, labelName);
            var labelPropertyType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{labelName}' should be Property");
            if (labelPropertyType == typeof(string))
            {
                var funExp = Expression.Lambda<Func<TItem, string>>(labelExp, itemParamExp);
                return funExp.Compile();
            }
            else
            {
                var resultExp = Expression.Call(labelExp, labelPropertyType.GetMethod(ToStringMethodName) ?? throw new MissingMethodException(labelPropertyType.Name, ToStringMethodName));
                var funExp = Expression.Lambda<Func<TItem, string>>(resultExp, itemParamExp);
                return funExp.Compile();
            }
        }

        internal static Func<TItem, string> CreateGetGroupFunc<TItem>(string groupName)
        {
            var itemParamExp = Expression.Parameter(typeof(TItem));
            var labelExp = Expression.Property(itemParamExp, groupName);
            var labelPropertyType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{groupName}' should be Property");
            if (labelPropertyType == typeof(string))
            {
                var funExp = Expression.Lambda<Func<TItem, string>>(labelExp, itemParamExp);
                return funExp.Compile();
            }
            else
            {
                var resultExp = Expression.Call(labelExp, labelPropertyType.GetMethod(ToStringMethodName) ?? throw new MissingMethodException(labelPropertyType.Name, ToStringMethodName));
                var funExp = Expression.Lambda<Func<TItem, string>>(resultExp, itemParamExp);
                return funExp.Compile();
            }
        }

        internal static Func<TItem, bool> CreateGetDisabledFunc<TItem>(string disabledName)
        {
            var itemParamExp = Expression.Parameter(typeof(TItem));
            var labelExp = Expression.Property(itemParamExp, disabledName);
            var labelPropertyType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{disabledName}' should be Property");
            if (labelPropertyType == typeof(bool))
            {
                var funExp = Expression.Lambda<Func<TItem, bool>>(labelExp, itemParamExp);
                return funExp.Compile();
            }
            else
            {
                throw new InvalidOperationException($"Member '{disabledName}' should be type of 'bool'");
            }
        }

        internal static Action<TItem, string> CreateSetLabelFunc<TItem>(string labelName)
        {
            var itemParamExp = Expression.Parameter(typeof(TItem));
            var labelNameParamExp = Expression.Parameter(typeof(string));
            var labelExp = Expression.Property(itemParamExp, labelName);
            var setLabelExp = Expression.Assign(labelExp, labelNameParamExp);
            var funExp = Expression.Lambda<Action<TItem, string>>(setLabelExp, itemParamExp, labelNameParamExp);
            return funExp.Compile();
        }

        internal static Action<TItem, TItemValue> CreateSetValueFunc<TItem, TItemValue>(string valueName)
        {
            var itemParamExp = Expression.Parameter(typeof(TItem));
            var valueParamExp = Expression.Parameter(typeof(TItemValue));
            var labelExp = Expression.Property(itemParamExp, valueName);
            var labelPropertyType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{valueName}' should be Property");

            if (labelPropertyType.IsClass)
            {
                var setLabelExp = Expression.Assign(labelExp, valueParamExp);
                var funExp = Expression.Lambda<Action<TItem, TItemValue>>(setLabelExp, itemParamExp, valueParamExp);
                return funExp.Compile();
            }

            var labelIsNullable = labelPropertyType.IsValueType && Nullable.GetUnderlyingType(labelPropertyType) != null;
            var valueIsNullable = valueParamExp.Type.IsValueType && Nullable.GetUnderlyingType(valueParamExp.Type) != null;
            if (labelIsNullable)
            {
                Expression nullableValueExp = valueIsNullable
                                                  ? valueParamExp
                                                  : Expression.New(labelPropertyType.GetConstructors()[0], valueParamExp);
                var setLabelExp = Expression.Assign(labelExp, nullableValueExp);
                var funExp = Expression.Lambda<Action<TItem, TItemValue>>(setLabelExp, itemParamExp, valueParamExp);
                return funExp.Compile();
            }
            else
            {
                var setLabelExp = valueIsNullable
                                      ? Expression.Assign(labelExp, Expression.Property(valueParamExp, Nullable_Value))
                                      : Expression.Assign(labelExp, valueParamExp);
                var funExp = Expression.Lambda<Action<TItem, TItemValue>>(setLabelExp, itemParamExp, valueParamExp);
                return funExp.Compile();
            }
        }
    }
}
