// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.Core.Helpers.MemberPath
{
    internal class GetItemExpressionReplacer : ExpressionVisitor
    {
        private const string GetItemMethod = "get_Item";

        private const string SetItemMethod = "set_Item";

        public readonly Expression ValueArgumentExpression;

        public GetItemExpressionReplacer(Expression valueArgumentExpression)
        {
            ValueArgumentExpression = valueArgumentExpression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == GetItemMethod && node.Object != null)
            {
                var valueExp = ValueArgumentExpression;
                if (node.Type != valueExp.Type)
                {
                    valueExp = Expression.Convert(valueExp, node.Type);
                }

                return Expression.Call(node.Object, node.Object.Type.GetMethod(SetItemMethod, BindingFlags.Public | BindingFlags.Instance)!, new[] { node.Arguments[0], valueExp });
            }

            return node;
        }
    }
}
