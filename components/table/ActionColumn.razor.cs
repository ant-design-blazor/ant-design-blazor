using AntDesign.Table.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ActionColumn : ColumnBase, IRenderColumn
    {
        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        public bool Blocked { get; set; }

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

        RenderFragment IRenderColumn.RenderBody(RowData rowData)
        {
            return BodyRender;
        }

        RenderFragment IRenderColumn.RenderColGroup()
        {
            return ColGroupRender;
        }

        RenderFragment IRenderColumn.RenderHeader()
        {
            return HeaderRender;
        }

        RenderFragment IRenderColumn.RenderMeasure()
        {
            return MEASURE;
        }

        RenderFragment IRenderColumn.RenderPlaceholder()
        {
            return PLACHOLDER;
        }
    }
}
