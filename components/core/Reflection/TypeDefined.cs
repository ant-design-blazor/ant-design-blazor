using System;

namespace AntDesign.Core.Reflection
{
    internal static class TypeDefined<T>
    {
        public static bool IsNullable { get; }

        public static bool IsGenericType { get; }

        public static Type NullableType { get; }

        static TypeDefined()
        {
            IsNullable = IsNullableType(typeof(T));
            NullableType = GetNullableGenericType(typeof(T));
            IsGenericType = typeof(T).IsGenericType;
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
