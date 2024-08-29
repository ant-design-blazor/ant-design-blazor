// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using AntDesign.Table;

namespace AntDesign
{
    public partial class ActionColumn : ColumnBase, IRenderColumn
    {
        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }
    }
}
