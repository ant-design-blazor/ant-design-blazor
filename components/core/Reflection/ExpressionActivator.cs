using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AntDesign.Core.Reflection
{
    public static class ExpressionActivator<T>
    {
        private static Func<T> _createInstanceFunc;

        static ExpressionActivator()
        {
            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                _createInstanceFunc = () => default;
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
