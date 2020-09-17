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

        [CascadingParameter(Name = "RowIndex")]
        public int RowIndex { get; set; }

        [CascadingParameter]
        public ColumnContext Context { get; set; }

        [CascadingParameter(Name = "CacheKey")]
        public int CacheKey { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public int RowSpan { get; set; } = 1;

        [Parameter]
        public int ColSpan { get; set; } = 1;

        [Parameter]
        public string Fixed { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public int ColIndex { get; set; }

        protected string FixedStyle => Fixed != null ? $"position: sticky; {Fixed}: {Width * (Fixed == "left" ? ColIndex : Context.Columns.Count - ColIndex - 1)}px;" : "";

        private void SetClass()
        {
            ClassMapper
                .Add("ant-table-cell")
                .If($"ant-table-cell-fix-{Fixed}", () => Fixed.IsIn("right", "left"))
                .If($"ant-table-cell-fix-right-first", () => Fixed == "right" && Context?.Columns.FirstOrDefault(x => x.Fixed == "right")?.ColIndex == this.ColIndex)
                .If($"ant-table-cell-fix-left-last", () => Fixed == "left" && Context?.Columns.LastOrDefault(x => x.Fixed == "left")?.ColIndex == this.ColIndex)
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
