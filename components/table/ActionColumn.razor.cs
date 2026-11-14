// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

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

        RenderFragment IQuickRenderColumn.RenderQuickBody(RowData rowData)
        {
            if (Hidden || RowSpan == 0 || ColSpan == 0)
            {
                return builder => { };
            }

            var cellData = new CellData(rowData);
            return builder =>
            {
                builder.OpenElement(0, "td");
                builder.AddMultipleAttributes(1, GetAdditionalAttributes(null));
                builder.AddAttribute(2, "class", ClassMapper.Class);
                builder.AddAttribute(3, "style", $"{QuickFixedStyle} {Style}");
                builder.AddAttribute(4, "rowspan", RowSpan > 1 ? RowSpan : null);
                builder.AddAttribute(5, "colspan", ColSpan > 1 ? ColSpan : null);
                if (CellRender != null)
                {
                    builder.AddContent(6, CellRender(cellData));
                }
                else if (ChildContent != null)
                {
                    builder.AddContent(6, ChildContent);
                }
                builder.CloseElement();
            };
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
