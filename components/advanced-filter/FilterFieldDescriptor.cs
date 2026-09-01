// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AntDesign.Filters;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Describes a filterable property of TItem, including its type, display name,
    /// resolved filter type, and the expression to access it.
    /// </summary>
    public class FilterFieldDescriptor
    {
        public string PropertyName { get; set; }

        public string DisplayName { get; set; }

        public Type PropertyType { get; set; }

        public Type UnderlyingType { get; set; }

        public IFieldFilterType FilterType { get; set; }

        public LambdaExpression PropertyAccess { get; set; }

        public AdvancedFilterValueInputKind? InputComponent { get; set; }

        public IReadOnlyList<AdvancedFilterOption> ValueOptions { get; set; }

        public RenderFragment<AdvancedFilterInputContext> CustomInput { get; set; }
    }
}
