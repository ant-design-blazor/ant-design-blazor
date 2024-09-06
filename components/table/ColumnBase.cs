// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
        internal bool IsHeaderTemplate { get; set; }

        [CascadingParameter(Name = "IsRowTemplate")]
        internal bool IsRowTemplate { get; set; }

        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        internal bool Blocked { get; set; }

        [CascadingParameter(Name = "RowData")]
        public RowData RowData { get; set; }

        /// <summary>
        /// Title for column header
        /// </summary>
        [Parameter]
        public virtual string Title { get; set; }

        /// <summary>
        /// Title content for column header
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Width for column
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Style for the header cell
        /// </summary>
        [Parameter]
        public string HeaderStyle { get; set; }

        /// <summary>
        /// Row span
        /// </summary>
        /// <default value="1" />
        [Parameter]
        public int RowSpan { get; set; } = 1;

        /// <summary>
        /// Column span
        /// </summary>
        /// <default value="1" />
        [Parameter]
        public int ColSpan { get; set; } = 1;

        /// <summary>
        /// Header column span
        /// </summary>
        /// <default value="1" />
        [Parameter]
        public int HeaderColSpan { get; set; } = 1;

        /// <summary>
        /// Fix a column
        /// </summary>
        [Parameter]
        public string Fixed { get; set; }

        /// <summary>
        /// Content of the column
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Cut off header title with ellipsis when set to true
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Ellipsis { get; set; }

        /// <summary>
        /// If the column is hidden or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Hidden { get; set; }

        /// <summary>
        /// Alignment for column contents
        /// </summary>
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

        /// <summary>
        /// Index of this column in the table
        /// </summary>
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

        private string _fixedStyle;
        protected string FixedStyle => _fixedStyle;

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
            if (Table?.RebuildColumns(true) ?? false)
            {
                return;
            }

            if (IsHeaderTemplate)
            {
                Context?.AddHeaderColumn(this);
            }
            else if (IsRowTemplate)
            {
                Context?.AddRowColumn(this);
            }
            else
            {
                Context?.AddColumn(this);
            }

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

            _fixedStyle = CalcFixedStyle();

            SetClass();

            base.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsHeaderTemplate && !IsRowTemplate)
            {
                //Context?.RemoveColumn(this);
            }
            //Context?.Columns.Remove(this);
            Table?.RebuildColumns(false);

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

            Fixed ??= Context.Columns.FirstOrDefault(x => x.Fixed != null && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex)?.Fixed;
            if (string.IsNullOrWhiteSpace(Fixed))
            {
                return cssStyleBuilder.Build();
            }

            var fixedWidths = Array.Empty<string>();

            if (Fixed == "left" && Context?.Columns.Count >= ColIndex)
            {
                for (int i = 0; i < ColIndex; i++)
                {
                    if (Context?.Columns[i].Fixed != null)
                    {
                        fixedWidths = fixedWidths.Append($"{(CssSizeLength)Context?.Columns[i].Width}");
                    }
                }
            }
            else if (Fixed == "right")
            {
                for (int i = (Context?.Columns.Count ?? 1) - 1; i > ColIndex; i--)
                {
                    if (Context?.Columns[i].Fixed != null)
                    {
                        fixedWidths = fixedWidths.Append($"{(CssSizeLength)Context?.Columns[i].Width}");
                    }
                }
            }

            if (IsHeaderTemplate && Table.ScrollY != null && Table.ScrollX != null && Fixed == "right")
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

        void IColumn.UpdateFixedStyle()
        {
            _fixedStyle = CalcFixedStyle();
            StateHasChanged();
        }
    }
}
