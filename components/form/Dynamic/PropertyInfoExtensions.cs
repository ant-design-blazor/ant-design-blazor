// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Form.Dynamic
{
    internal static class PropertyInfoExtensions
    {
        internal static RangeAttribute? RangeAttribute(this PropertyInfo prop)
        {
            return prop.GetCustomAttribute<RangeAttribute>();
        }

        internal static RadioAttribute? RadioAttribute(this PropertyInfo prop)
        {
            return prop.GetCustomAttribute<RadioAttribute>();
        }

        internal static bool IsCheckbox(this PropertyInfo prop)
        {
            return prop.PropertyType == typeof(bool);
        }

        internal static bool IsEditable(this PropertyInfo prop)
        {
            var isSetMethodNull = prop.SetMethod is null;
            var isInitOnly = prop.IsInitOnly();

            if (isSetMethodNull || isInitOnly)
                return false;

            var editableAttribute = prop.GetCustomAttribute<EditableAttribute>();
            return editableAttribute?.AllowEdit ?? true;
        }

        internal static Type GetInputComponentType(this PropertyInfo prop)
        {
            var type = prop.PropertyType;
            var dType = prop.GetCustomAttribute<DataTypeAttribute>();

            if (type == typeof(bool?))
                throw new InvalidOperationException("Nullable bools are not supported, please just use a regular bool");

            if (type is { IsEnum: true, IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                throw new InvalidOperationException("Nullable enums are not supported, please just use a regular enum");

            foreach (var (predicate, componentType) in _inputTypes)
                if (predicate(type, dType?.DataType))
                    return componentType;

            if (type.IsEnum)
            {
                var isRadio = prop.GetCustomAttribute<RadioAttribute>() is not null;
                if (isRadio)
                {
                    // TODO implement this one day
                    // return typeof(InputEnumRadio<>).MakeGenericType(prop.PropertyType);
                    throw new InvalidOperationException("RadioAttribute is not implemented yet.");
                }

                var isFlagsEnum = prop.PropertyType.IsDefined(typeof(FlagsAttribute), inherit: true);
                if (isFlagsEnum)
                {
                    // TODO flags multi choice select
                }

                return typeof(EnumSelect<>).MakeGenericType(prop.PropertyType);
            }

            return typeof(Input<>).MakeGenericType(prop.PropertyType);
        }

        internal static object? GetHtmlInputType(this PropertyInfo prop)
        {
            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            var dataTypeAttribute = prop.GetCustomAttribute<DataTypeAttribute>();

            // handle custom DataTypes
            if (dataTypeAttribute?.DataType == DataType.Custom
                && !string.IsNullOrEmpty(dataTypeAttribute.CustomDataType))
                return dataTypeAttribute.CustomDataType;
#if NET6_0_OR_GREATER
            // handle dates
            if (type == typeof(DateTime) || type == typeof(DateTimeOffset)

                || type == typeof(DateOnly) || type == typeof(TimeOnly)

                )
                if (prop.GetCustomAttribute<DateTypeAttribute>() is { InputDateType: var inputDateType })
                    return inputDateType;
#endif
            // handle other types
            foreach ((var pred, object? htmlInputType) in _htmlTypeAttributes)
                if (pred(type, dataTypeAttribute?.DataType))
                    return htmlInputType;

            return null;
        }

        private static readonly Dictionary<Func<Type, DataType?, bool>, Type> _inputTypes = new()
        {
            { (t, _) => t == typeof(bool), typeof(Checkbox) },

            { (t, _) => t == typeof(short), typeof(InputNumber<short>) },
            { (t, _) => t == typeof(int), typeof(InputNumber<int>) },
            { (t, _) => t == typeof(long), typeof(InputNumber<long>) },
            { (t, _) => t == typeof(float), typeof(InputNumber<float>) },
            { (t, _) => t == typeof(double), typeof(InputNumber<double>) },
            { (t, _) => t == typeof(decimal), typeof(InputNumber<decimal>) },

            { (t, _) => t == typeof(short?), typeof(InputNumber<short?>) },
            { (t, _) => t == typeof(int?), typeof(InputNumber<int?>) },
            { (t, _) => t == typeof(long?), typeof(InputNumber<long?>) },
            { (t, _) => t == typeof(float?), typeof(InputNumber<float?>) },
            { (t, _) => t == typeof(double?), typeof(InputNumber<double?>) },
            { (t, _) => t == typeof(decimal?), typeof(InputNumber<decimal?>) },

            { (t, dt) => t == typeof(string) && dt is DataType.MultilineText, typeof(TextArea) },

            { (t, _) => t == typeof(DateTime), typeof(DatePicker<DateTime>) },
            { (t, _) => t == typeof(DateTimeOffset), typeof(DatePicker<DateTimeOffset>) },
            { (t, _) => t == typeof(DateTime?), typeof(DatePicker<DateTime?>) },
            { (t, _) => t == typeof(DateTimeOffset?), typeof(DatePicker<DateTimeOffset?>) },

            #if NET6_0_OR_GREATER
            { (t, _) => t == typeof(DateOnly), typeof(DatePicker<DateOnly>) },
            { (t, _) => t == typeof(TimeOnly), typeof(DatePicker<TimeOnly>) },
            { (t, _) => t == typeof(DateOnly?), typeof(DatePicker<DateOnly?>) },
            { (t, _) => t == typeof(TimeOnly?), typeof(DatePicker<TimeOnly?>) },
            #endif
        };

        private static readonly Dictionary<Func<Type, DataType?, bool>, object?> _htmlTypeAttributes = new()
        {
            { (t, _) => t == typeof(bool), "checkbox" },
            { (t, dt) => t == typeof(string) && dt is DataType.Date, "date" },
            { (t, dt) => t == typeof(string) && dt is DataType.Time, "time" },
            { (t, dt) => t == typeof(string) && dt is DataType.DateTime, "datetime-local" },
            { (t, dt) => t == typeof(string) && dt is DataType.EmailAddress, "email" },
            { (t, dt) => t == typeof(string) && dt is DataType.Password, "password" },
            { (t, dt) => t == typeof(string) && dt is DataType.PhoneNumber, "tel" },
            { (t, dt) => t == typeof(string) && dt is DataType.Url or DataType.ImageUrl, "url" },

            #if NET6_0_OR_GREATER

            { (t, _) => t == typeof(DateTime), InputDateType.DateTimeLocal },
            { (t, _) => t == typeof(DateTimeOffset), InputDateType.DateTimeLocal },

            { (t, _) => t == typeof(DateOnly), InputDateType.Date },
            { (t, _) => t == typeof(TimeOnly), InputDateType.Time },
            #endif
        };

        internal static bool IsInitOnly(this PropertyInfo property)
        {
            if (!property.CanWrite)
                return false;

            var setMethod = property.SetMethod;

            // Get the modifiers applied to the return parameter.
            var setMethodReturnParameterModifiers = setMethod?.ReturnParameter.GetRequiredCustomModifiers();

            // Init-only properties are marked with the IsExternalInit type.
            return setMethodReturnParameterModifiers?.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit)) ?? false;
        }
    }
}
