// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Internal
{
    internal static class ColumnExpressionHelper
    {
        internal static MemberInfo? GetReturnMemberInfo(LambdaExpression expression)
        {
            var accessorBody = expression.Body;
            while (true)
            {
                if (accessorBody is UnaryExpression unaryExpression)
                {
                    accessorBody = unaryExpression.Operand;
                }
                else if (accessorBody is ConditionalExpression conditionalExpression)
                {
                    accessorBody = conditionalExpression.IfTrue;
                }
                else if (accessorBody is MethodCallExpression methodCallExpression)
                {
                    accessorBody = methodCallExpression.Object;
                }
                else if (accessorBody is BinaryExpression binaryExpression)
                {
                    accessorBody = binaryExpression.Left;
                }
                else
                {
                    break;
                }
            }

            if (accessorBody is not MemberExpression memberExpression)
            {
                return null;
            }

            return memberExpression.Member;
        }
    }
}
