// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign
{
    internal static class AdvancedFilterExpressionBuilder
    {
        public static Expression<Func<TItem, bool>> Build<TItem>(
            FilterConditionNode rootNode,
            IReadOnlyDictionary<string, FilterFieldDescriptor> fields)
        {
            if (rootNode == null)
                return _ => true;

            var parameter = Expression.Parameter(typeof(TItem), "item");
            var body = BuildNode(rootNode, fields, parameter);
            if (body == null)
                return _ => true;

            return Expression.Lambda<Func<TItem, bool>>(body, parameter);
        }

        private static Expression BuildNode(
            FilterConditionNode node,
            IReadOnlyDictionary<string, FilterFieldDescriptor> fields,
            ParameterExpression parameter)
        {
            if (node.NodeType == FilterNodeType.Condition)
                return BuildCondition(node, fields, parameter);

            return BuildGroup(node, fields, parameter);
        }

        private static Expression BuildCondition(
            FilterConditionNode node,
            IReadOnlyDictionary<string, FilterFieldDescriptor> fields,
            ParameterExpression parameter)
        {
            if (string.IsNullOrEmpty(node.PropertyName) || !fields.TryGetValue(node.PropertyName, out var field))
                return null;

            if (field.FilterType == null)
                return null;

            // IsNull/IsNotNull don't need a value
            var op = node.CompareOperator;
            if (op != TableFilterCompareOperator.IsNull && op != TableFilterCompareOperator.IsNotNull && node.Value == null)
                return null;

            // Replace the original parameter with ours
            var propertyExpr = ReplaceParameter(field.PropertyAccess.Body, field.PropertyAccess.Parameters[0], parameter);

            if (IsCollectionCompare(op, node.Value))
                return BuildCollectionCondition(op, propertyExpr, node.Value);

            var valueExpr = Expression.Constant(node.Value, node.Value?.GetType() ?? typeof(object));

            return field.FilterType.GetFilterExpression(op, propertyExpr, valueExpr);
        }

        private static bool IsCollectionCompare(TableFilterCompareOperator compareOperator, object value)
        {
            return value is IEnumerable
                && value is not string
                && compareOperator is TableFilterCompareOperator.Equals
                    or TableFilterCompareOperator.NotEquals
                    or TableFilterCompareOperator.Contains
                    or TableFilterCompareOperator.NotContains;
        }

        private static Expression BuildCollectionCondition(TableFilterCompareOperator compareOperator, Expression propertyExpr, object value)
        {
            var values = ToTypedArray(value, propertyExpr.Type);
            if (values.Length == 0)
                return null;

            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .Single(method => method.Name == nameof(Enumerable.Contains)
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(propertyExpr.Type);

            var containsExpr = Expression.Call(containsMethod, Expression.Constant(values), propertyExpr);
            return compareOperator is TableFilterCompareOperator.NotEquals or TableFilterCompareOperator.NotContains
                ? Expression.Not(containsExpr)
                : containsExpr;
        }

        private static Array ToTypedArray(object value, Type targetType)
        {
            var convertedValues = ((IEnumerable)value)
                .Cast<object>()
                .Where(item => item != null)
                .Select(item => ConvertValue(item, targetType))
                .ToArray();

            var array = Array.CreateInstance(targetType, convertedValues.Length);
            for (var i = 0; i < convertedValues.Length; i++)
            {
                array.SetValue(convertedValues[i], i);
            }

            return array;
        }

        private static object ConvertValue(object value, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (underlyingType.IsInstanceOfType(value))
                return value;

            if (underlyingType.IsEnum)
                return value is string stringValue
                    ? Enum.Parse(underlyingType, stringValue)
                    : Enum.ToObject(underlyingType, value);

            if (underlyingType == typeof(Guid))
                return value is Guid ? value : Guid.Parse(value.ToString());

            return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
        }

        private static Expression BuildGroup(
            FilterConditionNode node,
            IReadOnlyDictionary<string, FilterFieldDescriptor> fields,
            ParameterExpression parameter)
        {
            var validChildren = node.Children?
                .Select(c => BuildNode(c, fields, parameter))
                .Where(e => e != null)
                .ToList();

            if (validChildren == null || validChildren.Count == 0)
                return null;

            var combined = validChildren[0];
            for (int i = 1; i < validChildren.Count; i++)
            {
                combined = node.LogicalOperator == TableFilterCondition.And
                    ? Expression.AndAlso(combined, validChildren[i])
                    : Expression.OrElse(combined, validChildren[i]);
            }

            return combined;
        }

        private static Expression ReplaceParameter(Expression body, ParameterExpression oldParam, ParameterExpression newParam)
        {
            return new ParameterReplacer(oldParam, newParam).Visit(body);
        }

        private sealed class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;

            public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _oldParam ? _newParam : base.VisitParameter(node);
        }
    }
}
