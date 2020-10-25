using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// Represents a parameter descriptor for a component
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    class ParameterDescriptor<TComponent>
    {
        private readonly bool _isParameter;

        private readonly HashCodeProvider _hashCodeProvider;

        private readonly Func<TComponent, object> _getter;

        /// <summary>
        /// Gets a description of all the parameters of the component
        /// </summary>
        public static readonly ParameterDescriptor<TComponent>[] Descriptors
            = typeof(TComponent)
                .GetProperties()
                .Select(item => new ParameterDescriptor<TComponent>(item))
                .Where(item => item._isParameter)
                .ToArray();

        /// <summary>
        /// A parameter descriptor for a component
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
        /// Check whether it is of type EventCallback
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
        /// Create the get delegate for the property
        /// </summary>
        /// <param name="property">Property</param>
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
        /// Returns the hash of the parameter value
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
