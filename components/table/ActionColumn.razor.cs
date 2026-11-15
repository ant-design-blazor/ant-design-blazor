// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public partial class ActionColumn : ColumnBase, IQuickRenderColumn
    {
        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        internal bool Blocked { get; set; }

        /// <summary>
        /// Column content for a row. Takes priority over <see cref="ColumnBase.ChildContent"/>
        /// </summary>
        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        void IQuickRenderColumn.RenderQuickBody(RenderTreeBuilder builder, RowData rowData)
        {
            if (Hidden || RowSpan == 0 || ColSpan == 0)
            {
                return;
            }

            var cellData = new CellData(rowData);
            QuickBody(builder, cellData);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (IsHeader)
            {
                Context.HeaderColumnInitialized(this);
            }
        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }
    }
}
