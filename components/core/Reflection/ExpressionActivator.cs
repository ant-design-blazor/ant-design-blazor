// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace AntDesign.Core.Reflection
{
#if NETCOREAPP3_1_OR_GREATER
    internal static class ExpressionActivator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>
#else
    internal static class ExpressionActivator<T>
#endif
    {
        private static Func<T> _createInstanceFunc;

        static ExpressionActivator()
        {
            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                _createInstanceFunc = () => default;
                return;
            }

            var newExpression = Expression.New(constructor);
            _createInstanceFunc = Expression.Lambda<Func<T>>(newExpression).Compile();
        }

        public static T CreateInstance()
        {
            return _createInstanceFunc();
        }
    }
}
