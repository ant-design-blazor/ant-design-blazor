// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Filters;

namespace AntDesign
{
    public static class FilterFieldResolver
    {
        public static IReadOnlyList<FilterFieldDescriptor> Resolve<TItem>()
        {
            return Resolve(typeof(TItem));
        }

        public static IReadOnlyList<FilterFieldDescriptor> Resolve(Type itemType)
        {
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && IsSupportedType(p.PropertyType));

            var result = new List<FilterFieldDescriptor>();
            var parameter = Expression.Parameter(itemType, "item");

            foreach (var prop in properties)
            {
                var underlyingType = prop.PropertyType.GetUnderlyingType();
                var filterType = ResolveFilterType(prop.PropertyType, underlyingType);
                if (filterType == null)
                    continue;

                var propertyAccess = Expression.Lambda(
                    Expression.Property(parameter, prop),
                    parameter);

                result.Add(new FilterFieldDescriptor
                {
                    PropertyName = prop.Name,
                    DisplayName = GetDisplayName(prop),
                    PropertyType = prop.PropertyType,
                    UnderlyingType = underlyingType,
                    FilterType = filterType,
                    PropertyAccess = propertyAccess,
                });
            }

            return result;
        }

        private static IFieldFilterType ResolveFilterType(Type propertyType, Type underlyingType)
        {
            if (underlyingType == typeof(bool))
                return new BoolFieldFilterType();

            if (underlyingType.IsEnum)
            {
                // Use reflection to create EnumFieldFilterType<T> with the actual property type
                var enumFilterType = typeof(EnumFieldFilterType<>).MakeGenericType(propertyType);
                return (IFieldFilterType)Activator.CreateInstance(enumFilterType);
            }

            return InternalFieldFilterTypeResolver.Resolve(underlyingType);
        }

        private static bool IsSupportedType(Type type)
        {
            var underlying = type.GetUnderlyingType();
            return underlying == typeof(string)
                || underlying == typeof(bool)
                || underlying == typeof(Guid)
                || underlying.IsEnum
                || underlying.IsNumericType()
                || underlying.IsDateType();
        }

        private static string GetDisplayName(PropertyInfo prop)
        {
            var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null)
                return displayAttr.GetName() ?? prop.Name;

            var displayNameAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttr != null)
                return displayNameAttr.DisplayName;

            return prop.Name;
        }
    }
}
