// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    internal class AdvancedFilterInputRule
    {
        internal Func<FilterFieldDescriptor, bool> PropertyType { get; set; }

        internal Func<TableFilterCompareOperator, bool> CompareOperator { get; set; }

        internal AdvancedFilterValueInputKind InputKind { get; set; }

        internal bool Matches(FilterFieldDescriptor field, TableFilterCompareOperator compareOperator)
        {
            return (PropertyType?.Invoke(field) ?? true)
                && (CompareOperator?.Invoke(compareOperator) ?? true);
        }
    }
}
