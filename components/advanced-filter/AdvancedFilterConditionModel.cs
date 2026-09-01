// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json;

namespace AntDesign
{
    internal class AdvancedFilterConditionModel
    {
        public FilterNodeType NodeType { get; set; }

        public string PropertyName { get; set; }

        public TableFilterCompareOperator CompareOperator { get; set; } = TableFilterCompareOperator.Equals;

        public JsonElement? Value { get; set; }

        public TableFilterCondition LogicalOperator { get; set; } = TableFilterCondition.And;

        public List<AdvancedFilterConditionModel> Children { get; set; } = new();
    }
}
