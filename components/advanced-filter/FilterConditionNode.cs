// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign
{
    public enum FilterNodeType
    {
        Condition = 1,
        Group = 2
    }

    /// <summary>
    /// Represents a node in a filter expression tree.
    /// Can be either a single condition (property + operator + value)
    /// or a group of conditions joined by AND/OR.
    /// </summary>
    public class FilterConditionNode
    {
        public FilterNodeType NodeType { get; set; }

        // -- Condition node properties --
        public string PropertyName { get; set; }
        public TableFilterCompareOperator CompareOperator { get; set; } = TableFilterCompareOperator.Equals;
        public object Value { get; set; }

        // -- Group node properties --
        public TableFilterCondition LogicalOperator { get; set; } = TableFilterCondition.And;
        public List<FilterConditionNode> Children { get; set; } = new();

        public static FilterConditionNode CreateCondition() => new() { NodeType = FilterNodeType.Condition };

        public static FilterConditionNode CreateGroup(TableFilterCondition logicalOperator = TableFilterCondition.And) => new()
        {
            NodeType = FilterNodeType.Group,
            LogicalOperator = logicalOperator,
            Children = new List<FilterConditionNode> { CreateCondition() }
        };
    }
}
