using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Core.Reflection
{
    internal struct PropertyReflector
    {
        public RequiredAttribute RequiredAttribute { get; set; }

        public string DisplayName { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public PropertyReflector(PropertyInfo propertyInfo)
        {
            this.RequiredAttribute = propertyInfo?.GetCustomAttribute<RequiredAttribute>(true);
            this.DisplayName = propertyInfo?.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName ??
                propertyInfo?.GetCustomAttribute<DisplayAttribute>(true)?.GetName();

            this.PropertyName = propertyInfo?.Name;
            this.PropertyType = propertyInfo?.PropertyType;
        }

        public static PropertyReflector Create<TField>(Expression<Func<TField>> accessor)
        {
            if (accessor == null)
            {
                throw new ArgumentNullException(nameof(accessor));
            }

            var accessorBody = accessor.Body;

            if (accessorBody is UnaryExpression unaryExpression
               && unaryExpression.NodeType == ExpressionType.Convert
               && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (accessorBody is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo)
            {
                return new PropertyReflector(propertyInfo);
            }

            return new PropertyReflector();
        }
    }
}
