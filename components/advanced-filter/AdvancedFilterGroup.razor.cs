// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AdvancedFilterGroup : AntDomComponentBase
    {
        [Parameter]
        public IReadOnlyList<FilterFieldDescriptor> Fields { get; set; }

        [Parameter]
        public IReadOnlyDictionary<string, FilterFieldDescriptor> FieldMap { get; set; }

        [Parameter]
        public FilterConditionNode Node { get; set; }

        [Parameter]
        public FilterOptionsLocale FilterOptionsLocale { get; set; }

        [Parameter]
        public AdvancedFilterInputResolver InputResolver { get; set; }

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public EventCallback OnChanged { get; set; }

        [Parameter]
        public EventCallback OnRemoveGroup { get; set; }

        private AdvancedFilterLocale _locale = new();

        private void AddCondition()
        {
            Node.Children.Add(FilterConditionNode.CreateCondition());
            OnChanged.InvokeAsync();
        }

        private void RemoveChild(FilterConditionNode child)
        {
            Node.Children.Remove(child);
            OnChanged.InvokeAsync();
        }

        private void OnLogicalOperatorChanged(TableFilterCondition value)
        {
            Node.LogicalOperator = value;
            OnChanged.InvokeAsync();
        }
    }
}
