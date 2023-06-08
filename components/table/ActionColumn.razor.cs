using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using AntDesign.Table;

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

            Context.HeaderColumnInitialed(this);

        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }
    }
}
