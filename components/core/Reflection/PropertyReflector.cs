using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Core.Reflection
{
    internal struct PropertyReflector
    {
        public PropertyInfo PropertyInfo { get; }

        public RequiredAttribute RequiredAttribute { get; set; }

        public string DisplayName { get; set; }

        public string PropertyName { get; set; }

        internal PropertyReflector(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.RequiredAttribute = propertyInfo.GetCustomAttribute<RequiredAttribute>(true);
            this.DisplayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName ?? propertyInfo.Name;
            this.PropertyName = PropertyInfo.Name;
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

            if (!(accessorBody is MemberExpression memberExpression))
            {
                throw new ArgumentException($"The provided expression contains a {accessorBody.GetType().Name} which is not supported. {nameof(PropertyReflector)} only supports simple member accessors (fields, properties) of an object.");
            }

            var property = memberExpression.Member as PropertyInfo;

            return new PropertyReflector(property);
        }
    }
}
