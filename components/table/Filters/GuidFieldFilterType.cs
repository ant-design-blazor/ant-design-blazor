// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Filters
{
    public class GuidFieldFilterType : BaseFieldFilterType
    {
        public override RenderFragment<TableFilterInputRenderOptions> FilterInput => FilterInputs.Instance.GuidInput;
    }
}
