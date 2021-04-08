// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Internal
{
    internal static class ColumnExpressionHelper
    {
        internal static MemberInfo GetReturnMemberInfo(LambdaExpression expression)
        {
            var accessorBody = expression.Body;

            if (accessorBody is UnaryExpression unaryExpression
             && unaryExpression.NodeType == ExpressionType.Convert
             && unaryExpression.Type == typeof(object))
            {
                accessorBody = unaryExpression.Operand;
            }

            if (accessorBody is ConditionalExpression conditionalExpression)
            {
                accessorBody = conditionalExpression.IfTrue;
            }

            if (!(accessorBody is MemberExpression memberExpression))
            {
                throw new ArgumentException($"The type of the provided expression {accessorBody.GetType().Name} is not supported, it should be {nameof(MemberExpression)}.");
            }

            return memberExpression.Member;
        }
    }
}
