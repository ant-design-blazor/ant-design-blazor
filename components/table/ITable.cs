// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Filters;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public interface ITable
    {
        /// <summary>
        /// Gets the root wrapper element of the table.
        /// </summary>
        ElementReference NativeElement { get; }

        string GetSemanticClass(string slot, string baseClass = null);

        string GetSemanticStyle(string slot, string baseStyle = null);

        internal TableColumnDefaults Column { get; }

        internal System.Func<ElementReference, ElementReference> GetPopupContainer { get; }

        void ReloadData();

        void ReloadData(int? pageIndex, int? pageSize = null);

        void ReloadData(QueryModel queryModel);

        void ResetData();

        QueryModel GetQueryModel();

        /// <summary>
        /// Scrolls the table to a row or vertical offset.
        /// </summary>
        Task ScrollTo(int index);

        /// <summary>
        /// Scrolls the table to a row or vertical offset.
        /// </summary>
        Task ScrollTo(TableScrollToOptions options);

        void SetSelection(ICollection<string> keys);

        void SelectAll();

        void UnselectAll();

        void ExpandAll();

        void CollapseAll();

        internal TableLocale Locale { get; }

        internal ISelectionColumn Selection { get; set; }

        internal RenderFragment<RowData> GroupTitleTemplate { get; }

        internal bool TreeMode { get; }

        internal bool Responsive { get; }

        internal bool IsResponsiveColumnHidden(IColumn column);

        internal int IndentSize { get; }

        internal string ScrollX { get; }

        internal string ScrollY { get; }

        internal string ScrollBarWidth { get; }

        internal int ExpandIconColumnIndex { get; }

        internal int? ExpandedRowOffset { get; }

        internal int TreeExpandIconColumnIndex { get; }

        internal bool HasExpandTemplate { get; }

        internal bool ShouldShowExpandAll { get; }

        internal bool AreCurrentPageRowsExpanded { get; }

        internal bool CanExpandCurrentPage { get; }

        internal Task ToggleExpandAllAsync();

        internal SortDirection[] SortDirections { get; }

        internal OneOf<bool, TableSorterTooltipOptions> ShowSorterTooltip { get; }

        internal void SetSelection(ISelectionColumn selectItem);

        internal void UnSelectSelection(ISelectionColumn selectItem);

        internal bool AllSelected { get; }

        internal bool AnySelected { get; }

        internal bool HasHeaderTemplate { get; }

        internal bool HasRowTemplate { get; }

        //internal void SelectionChanged();

        internal void OnExpandChange(RowData rowData);

        internal void Refresh();

        internal void ColumnFilterChange();

        internal void HasFixLeft();

        internal void HasFixRight();

        internal void TableLayoutIsFixed();

        internal void ColumnSorterChange(IFieldColumn column);

        internal bool RowExpandable(RowData rowData);

        internal void AddSummaryRow(SummaryRow summaryRow);

        internal void OnColumnInitialized();

        IFieldFilterTypeResolver FieldFilterTypeResolver { get; }

        internal void AddGroupColumn(IFieldColumn column);

        internal void RemoveGroupColumn(IFieldColumn column);

        internal bool RebuildColumns(bool add);

        internal bool AutoColIndexes { get; }

        internal bool IsSticky { get; }

        internal bool IsVirtual { get; }

        internal string VirtualWidth { get; }

        internal bool ServerSide { get; }
    }
}
