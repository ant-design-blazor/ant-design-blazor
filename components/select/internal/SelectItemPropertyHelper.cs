// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Select.Internal
{
    internal static class SelectItemPropertyHelper
    {
        private const string ToStringMethodName = "ToString";

        private const string Nullable_HasValue = "HasValue";

        private const string Nullable_Value = "Value";

        private static readonly ConcurrentDictionary<DelegateCacheKey, Delegate> _getValueDelegateCache = new(new DelegateCacheKeyComparer());

        private static readonly ConcurrentDictionary<DelegateCacheKey, Delegate> _setValuedDelegateCache = new(new DelegateCacheKeyComparer());

        internal static Func<TItem, TItemValue> CreateGetValueFunc<TItem, TItemValue>(string valueName)
        {
            var key = new DelegateCacheKey(typeof(TItem), typeof(TItemValue), valueName);
            var func = _getValueDelegateCache.GetOrAdd(
                key,
                _ =>
                {
                    var itemParamExp = Expression.Parameter(typeof(TItem));
                    var labelExp = Expression.Property(itemParamExp, valueName);
                    var labelPropType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{valueName}' should be Property");
                    if (labelPropType.IsClass)
                    {
                        var funExp = Expression.Lambda<Func<TItem, TItemValue>>(labelExp, itemParamExp);
                        return funExp.Compile();
                    }

                    var labelIsNullable = labelPropType.IsValueType && Nullable.GetUnderlyingType(labelPropType) != null;
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
                });
            return (Func<TItem, TItemValue>)func;
        }

        internal static Func<TItem, string> CreateGetLabelFunc<TItem>(string labelName)
        {
            var key = new DelegateCacheKey(typeof(TItem), typeof(string), labelName);
            var func = _getValueDelegateCache.GetOrAdd(
                key,
                _ =>
                {
                    var itemParamExp = Expression.Parameter(typeof(TItem));
                    var labelExp = Expression.Property(itemParamExp, labelName);
                    var labelPropType = (labelExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{labelName}' should be Property");
                    if (labelPropType == typeof(string))
                    {
                        var funExp = Expression.Lambda<Func<TItem, string>>(labelExp, itemParamExp);
                        return funExp.Compile();
                    }
                    else
                    {
                        var resultExp = Expression.Call(labelExp, labelPropType.GetMethod(ToStringMethodName, Array.Empty<Type>()) ?? throw new MissingMethodException(labelPropType.Name, ToStringMethodName));
                        var funExp = Expression.Lambda<Func<TItem, string>>(resultExp, itemParamExp);
                        return funExp.Compile();
                    }
                });
            return (Func<TItem, string>)func;
        }

        internal static Func<TItem, string> CreateGetGroupFunc<TItem>(string groupName)
        {
            var key = new DelegateCacheKey(typeof(TItem), typeof(string), groupName);
            var func = _getValueDelegateCache.GetOrAdd(
                key,
                _ =>
                {
                    var itemParamExp = Expression.Parameter(typeof(TItem));
                    var groupExp = Expression.Property(itemParamExp, groupName);
                    var groupPropType = (groupExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{groupName}' should be Property");
                    if (groupPropType == typeof(string))
                    {
                        var funExp = Expression.Lambda<Func<TItem, string>>(groupExp, itemParamExp);
                        return funExp.Compile();
                    }
                    else
                    {
                        var resultExp = Expression.Call(groupExp, groupPropType.GetMethod(ToStringMethodName, Array.Empty<Type>()) ?? throw new MissingMethodException(groupPropType.Name, ToStringMethodName));
                        var funExp = Expression.Lambda<Func<TItem, string>>(resultExp, itemParamExp);
                        return funExp.Compile();
                    }
                });
            return (Func<TItem, string>)func;
        }

        internal static Func<TItem, bool> CreateGetDisabledFunc<TItem>(string disabledName)
        {
            var key = new DelegateCacheKey(typeof(TItem), typeof(bool), disabledName);
            var func = _getValueDelegateCache.GetOrAdd(
                key,
                _ =>
                {
                    var itemParamExp = Expression.Parameter(typeof(TItem));
                    var disabledExp = Expression.Property(itemParamExp, disabledName);
                    var disabledPropType = (disabledExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{disabledName}' should be Property");
                    if (disabledPropType == typeof(bool))
                    {
                        var funExp = Expression.Lambda<Func<TItem, bool>>(disabledExp, itemParamExp);
                        return funExp.Compile();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Member '{disabledName}' should be type of 'bool'");
                    }
                });
            return (Func<TItem, bool>)func;
        }

        internal static Action<TItem, string> CreateSetLabelFunc<TItem>(string labelName)
        {
            var key = new DelegateCacheKey(typeof(TItem), typeof(string), labelName);
            var action = _setValuedDelegateCache.GetOrAdd(
                key,
                _ =>
                {
                    var itemParamExp = Expression.Parameter(typeof(TItem));
                    var labelParamExp = Expression.Parameter(typeof(string));
                    var labelExp = Expression.Property(itemParamExp, labelName);
                    var setLabelExp = Expression.Assign(labelExp, labelParamExp);
                    var funExp = Expression.Lambda<Action<TItem, string>>(setLabelExp, itemParamExp, labelParamExp);
                    return funExp.Compile();
                });
            return (Action<TItem, string>)action;
        }

        internal static Action<TItem, TItemValue> CreateSetValueFunc<TItem, TItemValue>(string valueName)
        {
            var key = new DelegateCacheKey(typeof(TItem), typeof(TItemValue), valueName);
            var action = _setValuedDelegateCache.GetOrAdd(
                key,
                _ =>
                {
                    var itemParamExp = Expression.Parameter(typeof(TItem));
                    var valueParamExp = Expression.Parameter(typeof(TItemValue));
                    var valueExp = Expression.Property(itemParamExp, valueName);
                    var valuePropType = (valueExp.Member as PropertyInfo)?.PropertyType ?? throw new InvalidOperationException($"Member '{valueName}' should be Property");
                    if (valuePropType.IsClass)
                    {
                        var setLabelExp = Expression.Assign(valueExp, valueParamExp);
                        var funExp = Expression.Lambda<Action<TItem, TItemValue>>(setLabelExp, itemParamExp, valueParamExp);
                        return funExp.Compile();
                    }

                    var valueIsNullable = valuePropType.IsValueType && Nullable.GetUnderlyingType(valuePropType) != null;
                    var valueParamIsNullable = valueParamExp.Type.IsValueType && Nullable.GetUnderlyingType(valueParamExp.Type) != null;
                    if (valueIsNullable)
                    {
                        Expression nullableValueExp = valueParamIsNullable
                                                          ? valueParamExp
                                                          : Expression.New(valuePropType.GetConstructors()[0], valueParamExp);
                        var setLabelExp = Expression.Assign(valueExp, nullableValueExp);
                        var funExp = Expression.Lambda<Action<TItem, TItemValue>>(setLabelExp, itemParamExp, valueParamExp);
                        return funExp.Compile();
                    }
                    else
                    {
                        var setLabelExp = valueParamIsNullable
                                              ? Expression.Assign(valueExp, Expression.Property(valueParamExp, Nullable_Value))
                                              : Expression.Assign(valueExp, valueParamExp);
                        var funExp = Expression.Lambda<Action<TItem, TItemValue>>(setLabelExp, itemParamExp, valueParamExp);
                        return funExp.Compile();
                    }
                });
            return (Action<TItem, TItemValue>)action;
        }

        private readonly struct DelegateCacheKey
        {
            public readonly Type ItemType;

            public readonly Type ValueType;

            public readonly string PropertyName;

            public DelegateCacheKey(Type itemType, Type valueType, string propertyName)
            {
                ItemType = itemType;
                ValueType = valueType;
                PropertyName = propertyName;
            }
        }

        private class DelegateCacheKeyComparer : IEqualityComparer<DelegateCacheKey>
        {
            public bool Equals(DelegateCacheKey x, DelegateCacheKey y)
            {
                return x.ItemType == y.ItemType
                    && x.ValueType == y.ValueType
                    && x.PropertyName == y.PropertyName;
            }

            public int GetHashCode(DelegateCacheKey obj)
            {
                return HashCode.Combine(obj.ItemType, obj.ValueType, obj.PropertyName);
            }
        }
    }
}
