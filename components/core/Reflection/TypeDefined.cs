using System;

namespace AntDesign.Core.Reflection
{
    internal static class TypeDefined<T>
    {
        public static bool IsNullable;

        public static bool IsGenericType => typeof(T).IsGenericType;

        public static Type NullableType;

        static TypeDefined()
        {
            IsNullable = IsNullableType(typeof(T));
            NullableType = GetNullableGenericType(typeof(T));
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Type GetNullableGenericType(Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : null;
        }
    }
}
