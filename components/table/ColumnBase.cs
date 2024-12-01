// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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

        [CascadingParameter(Name = "IsBody")]
        public bool IsBody { get; set; }

        [CascadingParameter]
        public ColumnContext Context { get; set; }

        [CascadingParameter(Name = "RowData")]
        public RowData RowData { get; set; }

        protected TableDataItem DataItem => RowData.TableDataItem;

        [CascadingParameter(Name = "IsMeasure")]
        public bool IsMeasure { get; set; }

        [CascadingParameter(Name = "IsSummary")]
        public bool IsSummary { get; set; }

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
                _fixedStyle = CalcFixedStyle();
            }
        }

        /// <summary>
        /// Index of this column in the table
        /// </summary>
        [Parameter]
        public int ColIndex { get; set; }

        protected bool AppendExpandColumn => Table.HasExpandTemplate && ColIndex == (Table.TreeMode ? Table.TreeExpandIconColumnIndex : Table.ExpandIconColumnIndex);

        private string _fixedStyle;

        private ColumnAlign _align = ColumnAlign.Left;

        protected string FixedStyle => _fixedStyle;

        private int ActualColumnSpan => IsHeader ? HeaderColSpan : ColSpan;

        private int ColEndIndex => ColIndex + ActualColumnSpan;

        private void SetClass()
        {
            ClassMapper
                .Add("ant-table-cell")
                .If("ant-table-cell-fix-right", () => Context.Columns.Any(x => x.Fixed == "right" && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex))
                .If("ant-table-cell-fix-left", () => Context.Columns.Any(x => x.Fixed == "left" && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex))
                .If($"ant-table-cell-fix-right-first", () => Context?.Columns.FirstOrDefault(x => x.Fixed == "right") is var column && column?.ColIndex >= ColIndex && column?.ColIndex < ColEndIndex)
                .If($"ant-table-cell-fix-left-last", () => Context?.Columns.LastOrDefault(x => x.Fixed == "left") is var column && column?.ColIndex >= ColIndex && column?.ColIndex < ColEndIndex)
                .If($"ant-table-cell-with-append", () => IsBody && Table.TreeMode && Table.TreeExpandIconColumnIndex >= ColIndex && Table.TreeExpandIconColumnIndex < ColEndIndex)
                .If($"ant-table-cell-ellipsis", () => Ellipsis)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Render Pipeline: Initialize -> ColGroup -> Header ...
            if (IsInitialize)
            {
                if (Table?.RebuildColumns(true) ?? false)
                {
                    return;
                }

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
            else if (IsColGroup/* && Width == null*/)
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

            if (IsHeader || IsBody || IsSummary)
            {
                _fixedStyle = CalcFixedStyle();
            }

            SetClass();
        }

        protected override void Dispose(bool disposing)
        {
            //Context?.Columns.Remove(this);
            if (IsInitialize)
            {
                Table?.RebuildColumns(false);
            }
            base.Dispose(disposing);
        }

        private string CalcFixedStyle()
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

            if (Context == null)
            {
                return cssStyleBuilder.Build();
            }

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

            if (IsHeader && Table.ScrollY != null && Table.ScrollX != null && Fixed == "right")
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

        protected void ToggleTreeNode()
        {
            RowData.Expanded = !RowData.Expanded;
            Table?.OnExpandChange(RowData);
        }

        void IColumn.UpdateFixedStyle()
        {
            _fixedStyle = CalcFixedStyle();
            StateHasChanged();
        }
    }
}
