// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using AntDesign.Core.Documentation;
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

        [CascadingParameter(Name = "IsHeaderColGroup")]
        public bool IsHeaderColGroup { get; set; }


        [CascadingParameter(Name = "IsPlaceholder")]
        public bool IsPlaceholder { get; set; }

        [CascadingParameter(Name = "IsBody")]
        public bool IsBody { get; set; }

        [CascadingParameter]
        public ColumnContext Context { get; set; }

        [CascadingParameter(Name = "RowData")]
        public RowData RowData { get; set; }

        protected TableDataItem DataItem => RowData?.TableDataItem;

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
        /// CSS class applied to this column's header and body cells. This is
        /// equivalent to the React Table column `className` property.
        /// </summary>
        [Parameter]
        public string ClassName { get; set; }

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
        /// Defines the scope for cells in this column, such as `row` or `rowgroup`.
        /// </summary>
        [Parameter]
        public string RowScope { get; set; }

        /// <summary>
        /// Fix a column
        /// </summary>
        [Parameter]
        public ColumnFixPlacement? Fixed { get; set; }

        /// <summary>
        /// Content of the column
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Cut off content with ellipsis when set to true
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Ellipsis
        {
            get => _ellipsis;
            set
            {
                _ellipsisSpecified = true;
                _ellipsis = value;
            }
        }

        /// <summary>
        /// Whether to show native title attribute (true)
        /// Setting this property will automatically enable ellipsis.
        /// </summary>
        /// <default value="null" />
        [Parameter]
        public bool? EllipsisShowTitle { get; set; }

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
                _alignSpecified = true;
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
        [PublicApi("1.1.0")]
        public int ColIndex { get; set; }

        protected bool AppendExpandColumn => Table.HasExpandTemplate && ColIndex == (Table.TreeMode ? Table.TreeExpandIconColumnIndex : Table.ExpandIconColumnIndex);

        protected int VirtualColumnIndex
        {
            get
            {
                var expandColumnIndex = Table.TreeMode ? Table.TreeExpandIconColumnIndex : Table.ExpandIconColumnIndex;
                return Table.HasExpandTemplate && ColIndex >= expandColumnIndex ? ColIndex + 1 : ColIndex;
            }
        }

        internal virtual bool RequiresColGroup => Width != null || Fixed != null || AppendExpandColumn;

        protected bool RowExpandable => Table.RowExpandable(RowData);

        private string _fixedStyle;

        private ColumnAlign _align = ColumnAlign.Left;
        private bool _alignSpecified;
        private bool _ellipsis;
        private bool _ellipsisSpecified;
        private bool _columnInitialized;

        protected string FixedStyle => _fixedStyle;

        private int ActualColumnSpan => IsHeader ? HeaderColSpan : ColSpan;

        private int ColEndIndex => ColIndex + ActualColumnSpan;

        protected bool ShouldShowEllipsis => Ellipsis || EllipsisShowTitle.HasValue;

        private bool IsFixRight => Context.Columns.Any(x => x.Fixed == ColumnFixPlacement.Right && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex);

        private bool IsFixLeft => Context.Columns.Any(x => x.Fixed == ColumnFixPlacement.Left && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex)
            || IsAutoFixedSelection(this);

        private bool IsAutoFixedSelection(IColumn column) => column is ISelectionColumn
            && Context?.Columns.Any(x => !ReferenceEquals(x, column)
                && x.ColIndex > column.ColIndex
                && x.Fixed == ColumnFixPlacement.Left) == true;

        private void SetClass()
        {
            var isFixedLeftShadow = IsFixLeft && !Context.Columns.Any(x =>
                x.Fixed == ColumnFixPlacement.Left && x.ColIndex == ColEndIndex);
            var isFixedRightShadow = IsFixRight && !Context.Columns.Any(x =>
                x.Fixed == ColumnFixPlacement.Right && x.ColIndex + Math.Max(x.ColSpan, 1) == ColIndex);

            ClassMapper
                .Clear()
                .Add("ant-table-cell")
                .Add(ClassName)
                .If("ant-table-cell-fix", () => IsFixLeft || IsFixRight)
                .If("ant-table-cell-fix-start", () => IsFixLeft)
                .If("ant-table-cell-fix-end", () => IsFixRight)
                .If("ant-table-cell-fix-start-shadow", () => isFixedLeftShadow)
                .If("ant-table-cell-fix-end-shadow", () => isFixedRightShadow)
                .If("ant-table-cell-fix-end-shadow-show", () => isFixedRightShadow && Table.ScrollX != null)
                .If($"ant-table-cell-with-append", () => IsBody && Table.TreeMode && Table.TreeExpandIconColumnIndex >= ColIndex && Table.TreeExpandIconColumnIndex < ColEndIndex)
                .If($"ant-table-cell-ellipsis", () => ShouldShowEllipsis)
                .If("ant-table-cell-fix-sticky", () => Table.IsSticky && (IsFixRight || IsFixLeft))
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

                if (Fixed == ColumnFixPlacement.Left)
                {
                    Table?.HasFixLeft();
                }
                else if (Fixed == ColumnFixPlacement.Right)
                {
                    Table?.HasFixRight();
                }

                if (ShouldShowEllipsis)
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
            _columnInitialized = true;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var defaults = Table?.Column;
            var effectiveAlign = _alignSpecified ? _align : defaults?.Align ?? ColumnAlign.Left;
            var effectiveEllipsis = _ellipsisSpecified ? _ellipsis : defaults?.Ellipsis ?? false;
            var changed = _align != effectiveAlign || _ellipsis != effectiveEllipsis;

            _align = effectiveAlign;
            _ellipsis = effectiveEllipsis;

            if ((changed || this is ISelectionColumn) && _columnInitialized)
            {
                _fixedStyle = CalcFixedStyle();
                SetClass();
                if (ShouldShowEllipsis)
                    Table?.TableLayoutIsFixed();
            }
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
            if (Align != ColumnAlign.Left || _alignSpecified)
            {
                string alignment = Align switch
                {
                    ColumnAlign.Left => "left",
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

            var fixedPlacement = Fixed;
            if (this is ISelectionColumn && !fixedPlacement.HasValue && IsAutoFixedSelection(this))
            {
                fixedPlacement = ColumnFixPlacement.Left;
            }
            else
            {
                fixedPlacement ??= Context.Columns.FirstOrDefault(x => x.Fixed != null && x.ColIndex >= ColIndex && x.ColIndex < ColEndIndex)?.Fixed;
            }

            if (!fixedPlacement.HasValue)
            {
                return cssStyleBuilder.Build();
            }

            var fixedWidths = Array.Empty<string>();

            if (fixedPlacement == ColumnFixPlacement.Left && Context?.Columns.Count >= ColIndex)
            {
                for (int i = 0; i < ColIndex; i++)
                {
                    var column = Context?.Columns[i];
                    if (column?.Fixed != null || (column is ISelectionColumn && IsAutoFixedSelection(column)))
                    {
                        var width = column.Width ?? (column is ISelectionColumn ? "32px" : null);
                        if (width != null)
                            fixedWidths = fixedWidths.Append($"{(CssSizeLength)width}");
                    }
                }
            }
            else if (fixedPlacement == ColumnFixPlacement.Right)
            {
                for (int i = (Context?.Columns.Count ?? 1) - 1; i > ColIndex; i--)
                {
                    if (Context?.Columns[i].Fixed != null)
                    {
                        fixedWidths = fixedWidths.Append($"{(CssSizeLength)Context?.Columns[i].Width}");
                    }
                }
            }

            if (IsHeader && Table.ScrollY != null && Table.ScrollX != null && fixedPlacement == ColumnFixPlacement.Right)
            {
                fixedWidths = fixedWidths.Append($"{(CssSizeLength)Table.ScrollBarWidth}");
            }

            var fixedWidth = fixedWidths.Length switch
            {
                > 1 => $"calc({string.Join(" + ", fixedWidths)})",
                1 => fixedWidths[0],
                _ => "0px"
            };


            var columnCount = Context.Columns.Count;
            if (fixedPlacement == ColumnFixPlacement.Left)
            {
                cssStyleBuilder.AddStyle("inset-inline-start", fixedWidth);
                cssStyleBuilder.AddStyle("--z-offset", (2 * columnCount - ColIndex).ToString());
                cssStyleBuilder.AddStyle("--z-offset-reverse", (columnCount + ColIndex).ToString());
            }
            else if (fixedPlacement == ColumnFixPlacement.Right)
            {
                cssStyleBuilder.AddStyle("inset-inline-end", fixedWidth);
                cssStyleBuilder.AddStyle("--z-offset", ColIndex.ToString());
                cssStyleBuilder.AddStyle("--z-offset-reverse", (columnCount - ColIndex).ToString());
            }

            return cssStyleBuilder.Build().TrimEnd();
        }

        protected void ToggleTreeNode()
        {
            RowData.Expanded = !RowData.Expanded;
            Table?.OnExpandChange(RowData);
        }

        void IColumn.UpdateFixedStyle()
        {
            _fixedStyle = CalcFixedStyle();
            SetClass();
            StateHasChanged();
        }
    }
}
