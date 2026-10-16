// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;

namespace AntDesign
{
    public static class AdvancedFilterTableFilterConverter
    {
        public static FilterConditionNode FromTableFilterModels(
            IEnumerable<ITableFilterModel> filterModels,
            IReadOnlyList<FilterFieldDescriptor> fields = null)
        {
            var root = FilterConditionNode.CreateGroup(TableFilterCondition.And);
            root.Children.Clear();

            if (filterModels == null)
                return root;

            foreach (var filterModel in filterModels.Where(model => model?.Filters?.Any() == true))
            {
                var node = ToNode(filterModel);
                if (node != null)
                    root.Children.Add(node);
            }

            return fields == null ? root : AdvancedFilterStateSerializer.Clone(root, fields);
        }

        private static FilterConditionNode ToNode(ITableFilterModel filterModel)
        {
            var filters = filterModel.Filters?.ToList();
            if (filters?.Count > 0 != true)
                return null;

            if (filters.Count == 1)
                return ToCondition(filterModel.FieldName, filters[0]);

            if (GetTableFilterType(filterModel) == TableFilterType.List)
            {
                var listGroup = FilterConditionNode.CreateGroup(TableFilterCondition.Or);
                listGroup.Children = filters.Select(filter => ToCondition(filterModel.FieldName, filter)).ToList();
                return listGroup;
            }

            return ToFieldTypeGroup(filterModel.FieldName, filters);
        }

        private static FilterConditionNode ToFieldTypeGroup(string fieldName, IReadOnlyList<TableFilter> filters)
        {
            if (filters.Count == 1)
                return ToCondition(fieldName, filters[0]);

            var conditions = filters.Select(filter => ToCondition(fieldName, filter)).ToList();
            var conditionsAfterFirst = filters.Skip(1).Select(filter => filter.FilterCondition).Distinct().ToList();
            if (conditionsAfterFirst.Count <= 1)
            {
                var group = FilterConditionNode.CreateGroup(conditionsAfterFirst.FirstOrDefault(TableFilterCondition.And));
                group.Children = conditions;
                return group;
            }

            var current = conditions[0];
            for (var i = 1; i < conditions.Count; i++)
            {
                current = new FilterConditionNode
                {
                    NodeType = FilterNodeType.Group,
                    LogicalOperator = filters[i].FilterCondition,
                    Children = new List<FilterConditionNode> { current, conditions[i] },
                };
            }

            return current;
        }

        private static FilterConditionNode ToCondition(string fieldName, TableFilter filter)
        {
            return new FilterConditionNode
            {
                NodeType = FilterNodeType.Condition,
                PropertyName = fieldName,
                CompareOperator = filter.FilterCompareOperator == default
                    ? TableFilterCompareOperator.Equals
                    : filter.FilterCompareOperator,
                Value = filter.Value,
            };
        }

        private static TableFilterType GetTableFilterType(ITableFilterModel filterModel)
        {
            var property = filterModel.GetType().GetProperty(nameof(FilterModel<object>.FilterType));
            return property?.GetValue(filterModel) is TableFilterType filterType
                ? filterType
                : TableFilterType.FieldType;
        }
    }
}
