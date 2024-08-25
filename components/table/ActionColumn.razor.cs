// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ActionColumn : ColumnBase
    {
        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        public bool Blocked { get; set; }

        /// <summary>
        /// Column content for a row. Takes priority over <see cref="ColumnBase.ChildContent"/>
        /// </summary>
        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (IsHeader)
            {
                Context.HeaderColumnInitialed(this);
            }
        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }
    }
}
