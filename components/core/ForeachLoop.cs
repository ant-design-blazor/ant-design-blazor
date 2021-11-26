// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class ForeachLoop<TItem> : ComponentBase
    {
        [Parameter]
        public IEnumerable<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            foreach (var item in Items)
            {
                ChildContent(item)(builder);
            }
        }
    }
}
