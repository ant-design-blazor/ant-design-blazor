// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign.Filters;

namespace AntDesign
{
    public class AdvancedFilterInputContext
    {
        public FilterFieldDescriptor Field { get; set; }

        public FilterConditionNode Node { get; set; }

        public AdvancedFilterValueInputKind InputKind { get; set; }

        public TableFilter TableFilter { get; set; }

        public IReadOnlyList<AdvancedFilterOption> Options { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        public Action<object> ValueChanged { get; set; }

        public Action Confirm { get; set; }

        public object Value
        {
            get => Node?.Value;
            set => ValueChanged?.Invoke(value);
        }

        public TableFilterInputRenderOptions ToTableFilterInputRenderOptions()
        {
            return new TableFilterInputRenderOptions(
                TableFilter,
                Attributes ?? new(),
                null,
                _ => Confirm?.Invoke(),
                ValueChanged);
        }
    }
}
