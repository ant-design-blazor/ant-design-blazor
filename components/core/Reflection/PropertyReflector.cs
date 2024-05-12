﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Core.Reflection
{
    internal struct PropertyReflector
    {
        public RequiredAttribute RequiredAttribute { get; set; }

        public string DisplayName { get => _displayName ?? _getDisplayName?.Invoke(); set => _displayName = value; }

        public string PropertyName { get; set; }

        public Func<object, object> GetValueDelegate { get; set; }

        private Func<string> _getDisplayName;

        private string _displayName;

        public PropertyReflector(MemberInfo propertyInfo)
        {
            this.RequiredAttribute = propertyInfo?.GetCustomAttribute<RequiredAttribute>(true);

            if (propertyInfo?.GetCustomAttribute<DisplayNameAttribute>(true) is DisplayNameAttribute displayNameAttribute && !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
            {
                _displayName = displayNameAttribute.DisplayName;
            }
            else if (propertyInfo?.GetCustomAttribute<DisplayAttribute>(true) is DisplayAttribute displayAttribute)
            {
                _getDisplayName = displayAttribute.GetName;
            }

            this.PropertyName = propertyInfo?.Name;

            if (propertyInfo is PropertyInfo property)
            {
                GetValueDelegate = property.GetValue;
            }
        }

        public static PropertyReflector Create<TField>(Expression<Func<TField>> accessor)
        {
            if (accessor == null)
            {
                throw new ArgumentNullException(nameof(accessor));
            }

            var accessorBody = accessor.Body;

            return Create(accessorBody);
        }

        public static PropertyReflector Create(Expression accessorBody)
        {
            if (accessorBody is UnaryExpression unaryExpression
               && unaryExpression.NodeType == ExpressionType.Convert
               && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (accessorBody is MemberExpression memberExpression)
            {
                return new PropertyReflector(memberExpression.Member);
            }

            return new PropertyReflector();
        }
    }
}
