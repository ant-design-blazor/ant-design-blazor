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
