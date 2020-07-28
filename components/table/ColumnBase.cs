using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ColumnBase : AntDomComponentBase, IColumn
    {
        [CascadingParameter]
        public ITable Table { get; set; }

        [CascadingParameter(Name = "IsHeader")]
        public bool IsHeader { get; set; }

        [CascadingParameter(Name = "IsColGroup")]
        public bool IsColGroup { get; set; }

        [CascadingParameter(Name = "IsPlaceholder")]
        public bool IsPlaceholder { get; set; }

        [CascadingParameter]
        public ColumnContext Context { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public string Fixed { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public int Index { get; set; }

        protected string FixedStyle => Fixed != null ? $"position: sticky; {Fixed}: {Width * (Fixed == "left" ? Index : Context.Columns.Count - Index - 1)}px;" : "";

        private void SetClass()
        {
            ClassMapper
                .Add("ant-table-cell")
                .If($"ant-table-cell-fix-{Fixed}", () => Fixed.IsIn("right", "left"))
                .If($"ant-table-cell-fix-right-first", () => Fixed == "right" && Context?.Columns.FirstOrDefault(x => x.Fixed == "right")?.Index == this.Index)
                .If($"ant-table-cell-fix-left-last", () => Fixed == "left" && Context?.Columns.LastOrDefault(x => x.Fixed == "left")?.Index == this.Index)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (IsHeader)
            {
                Context?.AddHeaderColumn(this);
                Table?.Refresh();
            }
            else
            {
                Context?.AddRowColumn(this);
            }

            SetClass();
        }

        protected override void Dispose(bool disposing)
        {
            if (Context != null)
            {
                Context.Columns.Remove(this);
            }
            base.Dispose(disposing);
        }
    }
}
