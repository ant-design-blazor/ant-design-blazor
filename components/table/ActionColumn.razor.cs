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
            RenderBody(builder, rowData, quick: true);
        }

        void IQuickRenderColumn.RenderQuickHeader(RenderTreeBuilder builder)
        {
            var initialize = !QuickHeaderInitialized;
            InitializeQuickHeader();
            if (initialize)
            {
                Context.HeaderColumnInitialized(this);
            }
            if (Hidden || HeaderColSpan == 0)
            {
                return;
            }
            builder.OpenElement(0, "th");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "class", ClassMapper.Class);
            builder.AddAttribute(3, "style", $"{FixedStyle} {HeaderStyle}");
            builder.AddAttribute(4, "colspan", HeaderColSpan);
            if (RowSpan > 1) builder.AddAttribute(5, "rowspan", RowSpan);
            builder.AddContent(6, TitleTemplate ?? (RenderFragment)(contentBuilder => contentBuilder.AddContent(0, Title)));
            builder.CloseElement();
        }

        void IQuickRenderColumn.RenderQuickColGroup(RenderTreeBuilder builder)
        {
            RenderQuickColGroup(builder);
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
