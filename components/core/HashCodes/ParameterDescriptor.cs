// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// 表示组件的参数描述者
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    class ParameterDescriptor<TComponent>
    {
        private readonly bool _isParameter;

        private readonly HashCodeProvider _hashCodeProvider;

        private readonly Func<TComponent, object> _getter;

        /// <summary>
        /// 获取组件的所有参数描述
        /// </summary>
        public static readonly ParameterDescriptor<TComponent>[] Descriptors
            = typeof(TComponent)
                .GetProperties()
                .Select(item => new ParameterDescriptor<TComponent>(item))
                .Where(item => item._isParameter)
                .ToArray();

        /// <summary>
        /// 组件的参数描述者
        /// </summary>
        /// <param name="property">属性类型</param>
        private ParameterDescriptor(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            this._isParameter = IsEventCallBack(property) == false
                || property.IsDefined(typeof(ParameterAttribute))
                || property.IsDefined(typeof(CascadingParameterAttribute));

            if (this._isParameter == true)
            {
                this._getter = CreateGetFunc(property);
                this._hashCodeProvider = HashCodeProvider.Create(property.PropertyType);
            }
        }

        /// <summary>
        /// 返回是否为EventCallback类型
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsEventCallBack(PropertyInfo property)
        {
            var type = property.PropertyType;
            if (type == typeof(EventCallback))
            {
                return true;
            }

            if (type.IsGenericType == true)
            {
                return type.GetGenericTypeDefinition() == typeof(EventCallback<>);
            }

            return false;
        }

        /// <summary>
        /// 创建属性的获取委托
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        private static Func<TComponent, object> CreateGetFunc(PropertyInfo property)
        {
            // (TComponent component) => (object)(component.Property)
            var componentType = typeof(TComponent);
            var parameter = Expression.Parameter(componentType);
            var member = Expression.Property(parameter, property);
            var body = Expression.Convert(member, typeof(object));
            return Expression.Lambda<Func<TComponent, object>>(body, parameter).Compile();
        }

        /// <summary>
        /// 返回参数值的哈希
        /// </summary>
        /// <param name="component">组件</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public int GetValueHashCode(TComponent component)
        {
            if (this._isParameter == false)
            {
                throw new NotSupportedException();
            }
            var value = this._getter(component);
            return this._hashCodeProvider.GetHashCode(value);
        }
    }
}
