// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Form.Dynamic
{
    internal record InputComponentExpressionContainer(LambdaExpression ValueExpression, object? ValueChanged)
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly MethodInfo _eventCallbackFactoryCreate = GetEventCallbackFactoryCreate();

        private static MethodInfo GetEventCallbackFactoryCreate()
        {
            return typeof(EventCallbackFactory)
                .GetMethods()
                .Single(m =>
                {
                    if (m.Name != "Create" || !m.IsPublic || m.IsStatic || !m.IsGenericMethod)
                        return false;

                    var generic = m.GetGenericArguments();
                    if (generic.Length != 1)
                        return false;

                    var args = m.GetParameters();
                    return args.Length == 2
                           && args[0].ParameterType == typeof(object)
                           && args[1].ParameterType.IsGenericType
                           && args[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<>);
                });
        }

        public static InputComponentExpressionContainer Create<TEntity>(FormField<TEntity> formField)
            where TEntity : class, new()
        {
            // () => Owner.Property
            var access = Expression.Property(
                Expression.Constant(formField.Owner, typeof(TEntity)),
                formField.PropertyInfo);
            var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(formField.PropertyType), access);

            // Create(object receiver, Action<object>) callback
            var method = _eventCallbackFactoryCreate.MakeGenericMethod(formField.PropertyType);

            // value => Field.Value = value;
            var changeHandlerParameter = Expression.Parameter(formField.PropertyType);

            var body = Expression.Assign(
                Expression.Property(Expression.Constant(formField), nameof(formField.Value)),
                Expression.Convert(changeHandlerParameter, typeof(object)));

            var changeHandlerLambda = Expression.Lambda(
                typeof(Action<>).MakeGenericType(formField.PropertyType),
                body,
                changeHandlerParameter);

            var changeHandler = method.Invoke(
                EventCallback.Factory,
                new object[] { formField, changeHandlerLambda.Compile() });

            return new InputComponentExpressionContainer(lambda, changeHandler);
        }
    }
}
