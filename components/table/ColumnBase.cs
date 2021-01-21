using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ColumnBase : AntDomComponentBase, IColumn
    {
        [CascadingParameter]
        public ITable Table { get; set; }

        [CascadingParameter(Name = "IsInitialize")]
        public bool IsInitialize { get; set; }

        [CascadingParameter(Name = "IsHeader")]
        public bool IsHeader { get; set; }

        [CascadingParameter(Name = "IsColGroup")]
        public bool IsColGroup { get; set; }

        [CascadingParameter(Name = "IsPlaceholder")]
        public bool IsPlaceholder { get; set; }

        [CascadingParameter]
        public ColumnContext Context { get; set; }

        [CascadingParameter(Name = "RowData")]
        public RowData RowData { get; set; }

        [CascadingParameter(Name = "IsMeasure")]
        public bool IsMeasure { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public string HeaderStyle { get; set; }

        [Parameter]
        public int RowSpan { get; set; } = 1;

        [Parameter]
        public int ColSpan { get; set; } = 1;

        [Parameter]
        public int HeaderColSpan { get; set; } = 1;

        [Parameter]
        public string Fixed { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Ellipsis { get; set; }

        public int ColIndex { get; set; }

        protected bool AppendExpandColumn => Table.HasExpandTemplate && ColIndex == (Table.TreeMode ? Table.TreeExpandIconColumnIndex : Table.ExpandIconColumnIndex);

        protected string FixedStyle
        {
            get
            {
                if (Fixed == null || Context == null)
                {
                    return "";
                }

                var fixedWidth = 0;

                if (Fixed == "left" && Context?.Columns.Count >= ColIndex)
                {
                    for (int i = 0; i < ColIndex; i++)
                    {
                        fixedWidth += ((CssSizeLength)Context?.Columns[i].Width).Value;
                    }
                }
                else if (Fixed == "right")
                {
                    for (int i = (Context?.Columns.Count ?? 1) - 1; i > ColIndex; i--)
                    {
                        fixedWidth += ((CssSizeLength)Context?.Columns[i].Width).Value;
                    }
                }

                if (IsHeader && Table.ScrollY != null && Table.ScrollX != null && Fixed == "right")
                {
                    fixedWidth += Table.ScrollBarWidth;
                }

                return $"position: sticky; {Fixed}: {(CssSizeLength)fixedWidth};";
            }
        }

        private void SetClass()
        {
            ClassMapper
                .Add("ant-table-cell")
                .GetIf(() => $"ant-table-cell-fix-{Fixed}", () => Fixed.IsIn("right", "left"))
                .If($"ant-table-cell-fix-right-first", () => Fixed == "right" && Context?.Columns.FirstOrDefault(x => x.Fixed == "right")?.ColIndex == this.ColIndex)
                .If($"ant-table-cell-fix-left-last", () => Fixed == "left" && Context?.Columns.LastOrDefault(x => x.Fixed == "left")?.ColIndex == this.ColIndex)
                .If($"ant-table-cell-with-append", () => ColIndex == Table.ExpandIconColumnIndex && Table.TreeMode)
                .If($"ant-table-cell-ellipsis", () => Ellipsis)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Render Pipeline: Initialize -> ColGroup -> Header ...
            if (IsInitialize)
            {
                Context?.AddColumn(this);

                if (Fixed == "left")
                {
                    Table?.HasFixLeft();
                }
                else if (Fixed == "right")
                {
                    Table?.HasFixRight();
                }

                if (Ellipsis)
                {
                    Table?.TableLayoutIsFixed();
                }
            }
            else if (IsColGroup && Width == null)
            {
                Context?.AddColGroup(this);
            }
            else if (IsHeader)
            {
                Context?.AddHeaderColumn(this);
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
