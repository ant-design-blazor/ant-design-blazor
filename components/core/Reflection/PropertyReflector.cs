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
        public RequiredAttribute RequiredAttribute { get; set; }

        public string DisplayName { get; set; }

        public string PropertyName { get; set; }

        private PropertyReflector(MemberInfo propertyInfo)
        {
            this.RequiredAttribute = propertyInfo?.GetCustomAttribute<RequiredAttribute>(true);
            this.DisplayName = propertyInfo?.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName ??
                propertyInfo?.GetCustomAttribute<DisplayAttribute>(true)?.GetName();

            this.PropertyName = propertyInfo?.Name;
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

            if (accessorBody is MemberExpression memberExpression)
            {
                return new PropertyReflector(memberExpression.Member);
            }

            return new PropertyReflector();
        }
    }
}
