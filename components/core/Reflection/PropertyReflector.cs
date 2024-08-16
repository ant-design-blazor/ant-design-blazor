﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Core.Reflection
{
    internal class PropertyReflector
    {
        public RequiredAttribute RequiredAttribute { get; set; }

        public ValidationAttribute[] ValidationAttributes { get; set; }

        public string DisplayName { get => _displayName ?? _getDisplayName?.Invoke(); set => _displayName = value; }

        public string PropertyName { get; set; }

        public Func<object, object> GetValueDelegate { get; set; }

        private Func<string> _getDisplayName;

        private string _displayName;

        private PropertyReflector ParentReflector { get; set; }

        public PropertyReflector() { }

        public PropertyReflector(MemberInfo propertyInfo, PropertyReflector parentReflector = null)
        {
            ParentReflector = parentReflector;

            ValidationAttributes = propertyInfo?.GetCustomAttributes<ValidationAttribute>(true).ToArray();
            if (parentReflector?.ValidationAttributes?.Length > 0)
            {
                ValidationAttributes = [.. parentReflector.ValidationAttributes, .. ValidationAttributes];
            }

            RequiredAttribute = ValidationAttributes.OfType<RequiredAttribute>().FirstOrDefault() ?? ParentReflector?.RequiredAttribute;

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
            PropertyReflector parentProperty = default;

            if (accessorBody is UnaryExpression unaryExpression
               && unaryExpression.NodeType == ExpressionType.Convert
               && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (accessorBody is MemberExpression memberExpression)
            {
                if (memberExpression.Expression is MemberExpression parentMemberExpression)
                {
                    parentProperty = Create(parentMemberExpression);
                }

                return new PropertyReflector(memberExpression.Member, parentProperty);
            }

            if (accessorBody.NodeType == ExpressionType.ArrayIndex)
            {
                var parameterExpression = Expression.Parameter(typeof(object), "parameter");
                var func = Expression.Lambda<Func<object, object>>(Expression.Convert(accessorBody, typeof(object)), parameterExpression).Compile();
                return new PropertyReflector() { GetValueDelegate = func };
            }

            return new PropertyReflector();
        }
    }
}
