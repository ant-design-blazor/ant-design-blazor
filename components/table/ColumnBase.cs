using System;
using System.ComponentModel;
using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class ColumnBase : AntDomComponentBase, IColumn
    {
        [CascadingParameter]
        internal ColumnContext Context { get; set; }

        [CascadingParameter(Name = "IsHeaderTemplate")]
        public bool IsHeaderTemplate { get; set; }

        [CascadingParameter(Name = "IsRowTemplate")]
        public bool IsRowTemplate { get; set; }

        [CascadingParameter]
        public RowData RowData { get; set; }

        [Parameter]
        public virtual string Title { get; set; }

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

        [Parameter]
        public bool Hidden { get; set; }

        [Parameter]
        public ColumnAlign Align
        {
            get => _align;
            set
            {
                if (_align == value)
                    return;

                _align = value;
            }
        }

        public int ColIndex { get; set; }
        public ITable Table => Context?.Table;

        protected bool AppendExpandColumn => Table.HasExpandTemplate && ColIndex == (Table.TreeMode ? Table.TreeExpandIconColumnIndex : Table.ExpandIconColumnIndex);
        protected bool IsFiexedEllipsis => Ellipsis && Fixed is "left" or "right";
        //protected string _fixedStyle;
        //protected string _headerFixedStyle;

        private ColumnAlign _align = ColumnAlign.Left;

        private int ColEndIndex => ColIndex + ColSpan;

        private int HeaderColEndIndex => ColIndex + HeaderColSpan;

        protected readonly ClassMapper HeaderMapper = new();

        protected void SetClass()
        {
            ClassMapper
                .Add("ant-table-cell")
                .If("ant-table-cell-fix-right", () => Context.Columns.Any(x => x.Fixed == "right" && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex))
                .If("ant-table-cell-fix-left", () => Context.Columns.Any(x => x.Fixed == "left" && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex))
                .If($"ant-table-cell-fix-right-first", () => Context?.Columns.FirstOrDefault(x => x.Fixed == "right") is var column && column?.ColIndex >= ColIndex && column?.ColIndex < ColEndIndex)
                .If($"ant-table-cell-fix-left-last", () => Context?.Columns.LastOrDefault(x => x.Fixed == "left") is var column && column?.ColIndex >= ColIndex && column?.ColIndex < ColEndIndex)
                .If($"ant-table-cell-with-append", () => Table.TreeMode && Table.TreeExpandIconColumnIndex >= ColIndex && Table.TreeExpandIconColumnIndex < ColEndIndex)
                .If($"ant-table-cell-ellipsis", () => Ellipsis)
                ;

            HeaderMapper
                .Add("ant-table-cell")
                .If("ant-table-cell-fix-right", () => Context.Columns.Any(x => x.Fixed == "right" && x.ColIndex >= ColIndex && x.ColIndex < HeaderColEndIndex))
                .If("ant-table-cell-fix-left", () => Context.Columns.Any(x => x.Fixed == "left" && x.ColIndex >= ColIndex && x.ColIndex < HeaderColEndIndex))
                .If($"ant-table-cell-fix-right-first", () => Context?.Columns.FirstOrDefault(x => x.Fixed == "right") is var column && column?.ColIndex >= ColIndex && column?.ColIndex < HeaderColEndIndex)
                .If($"ant-table-cell-fix-left-last", () => Context?.Columns.LastOrDefault(x => x.Fixed == "left") is var column && column?.ColIndex >= ColIndex && column?.ColIndex < HeaderColEndIndex)
                .If($"ant-table-cell-ellipsis", () => Ellipsis)
                ;
        }

        protected override void OnInitialized()
        {
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

            SetClass();

            base.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsHeaderTemplate && !IsRowTemplate)
            {
                Context?.RemoveColumn(this);
            }
            base.Dispose(disposing);
        }

        protected string CalcFixedStyle(bool isHeader = false)
        {
            CssStyleBuilder cssStyleBuilder = new CssStyleBuilder();
            if (Align != ColumnAlign.Left)
            {
                string alignment = Align switch
                {
                    ColumnAlign.Center => "center",
                    ColumnAlign.Right => "right",
                    _ => ""
                };

                if (!string.IsNullOrEmpty(alignment))
                    cssStyleBuilder.AddStyle("text-align", alignment);
            }

            Fixed ??= Context?.Columns.FirstOrDefault(x => x.Fixed != null && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex)?.Fixed;

            if (Fixed != null)
            {
                var fixedWidths = Array.Empty<string>();

                if (Fixed == "left" && Context?.Columns.Count >= ColIndex)
                {
                    for (int i = 0; i < ColIndex; i++)
                    {
                        fixedWidths = fixedWidths.Append($"{(CssSizeLength)Context?.Columns[i].Width}");
                    }
                }
                else if (Fixed == "right")
                {
                    for (int i = (Context?.Columns.Count ?? 1) - 1; i > ColIndex; i--)
                    {
                        fixedWidths = fixedWidths.Append($"{(CssSizeLength)Context?.Columns[i].Width}");
                    }
                }

                if (isHeader && Table.ScrollY != null && Table.ScrollX != null && Fixed == "right")
                {
                    fixedWidths = fixedWidths.Append($"{(CssSizeLength)Table.ScrollBarWidth}");
                }

                var fixedWidth = fixedWidths.Length switch
                {
                    > 1 => $"calc({string.Join(" + ", fixedWidths)})",
                    1 => fixedWidths[0],
                    _ => "0px"
                };

                cssStyleBuilder
                    .AddStyle("position", "sticky")
                    .AddStyle(Fixed, fixedWidth);
            }

            return cssStyleBuilder.Build();
        }

        protected void ToggleTreeNode(RowData rowData)
        {
            rowData.Expanded = !rowData.Expanded;
            Table?.OnExpandChange(rowData);
        }

        public void Load()
        {
            //StateHasChanged();
        }
    }
}
