// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace AntDesign
{
    public static class THelper
    {
        public static T ChangeType<T>(object value)
        {
            return ChangeType<T>(value, null);
        }

        public static T ChangeType<T>(object value, IFormatProvider provider)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            if (provider == null) return (T)Convert.ChangeType(value, t);
            return (T)Convert.ChangeType(value, t, provider);
        }

        public static bool IsTypeNullable<T>()
        {
            return IsTypeNullable(typeof(T));
        }

        public static bool IsTypeNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static Type GetNullableType<T>()
        {
            var type = typeof(T);
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            else
                return type;
        }

        public static Type GetUnderlyingType<T>()
        {
            var type = typeof(T);
            Type targetType;
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(type);
            }
            else
            {
                targetType = type;
            }
            return targetType;
        }

        public static Type GetUnderlyingType(this Type type)
        {
            Type targetType;
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(type);
            }
            else
            {
                targetType = type;
            }
            return targetType;
        }

        public static bool IsNumericType(this Type type)
        {
            return type != null
                && Type.GetTypeCode(type)
                       is TypeCode.Byte
                       or TypeCode.Decimal
                       or TypeCode.Double
                       or TypeCode.Int16
                       or TypeCode.Int32
                       or TypeCode.Int64
                       or TypeCode.SByte
                       or TypeCode.Single
                       or TypeCode.UInt16
                       or TypeCode.UInt32
                       or TypeCode.UInt64;
        }
    }
}
