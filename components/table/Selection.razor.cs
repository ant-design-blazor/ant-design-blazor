// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn, IQuickRenderColumn
    {
        /// <summary>
        /// The type of input to use for the selection column (checkbox or radio)
        /// </summary>
        /// <default value="SelectionType.Checkbox"/>
        [Parameter]
        public SelectionType Type { get; set; } = SelectionType.Checkbox;

        /// <summary>
        /// Whether the selection column is disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// No use now.
        /// </summary>
        [Obsolete("Please use the RowKey of Table instead.")]
        [Parameter]
        public string Key { get; set; }

        /// <summary>
        /// Check table row precisely; parent row and children rows are not associated
        /// </summary>
        [Parameter] public bool CheckStrictly { get; set; }

        /// <summary>
        /// Customize the content of the cell.
        /// </summary>
        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        private bool Indeterminate => IsHeader
                                   && Table.AnySelected
                                   && !Table.AllSelected;

        private IList<ISelectionColumn> _rowSelections = [];

        //private int[] _selectedIndexes;

        private bool IsHeaderDisabled => _rowSelections.Any() && _rowSelections.All(x => x.Disabled);

        public bool Selected => DataItem.Selected;

        IList<ISelectionColumn> ISelectionColumn.RowSelections => _rowSelections;

        private bool? _selected;

        private void OnCheckedChange(bool selected)
        {
            if (IsHeader)
            {
                if (selected)
                {
                    Table.SelectAll();
                }
                else
                {
                    Table.UnselectAll();
                }
            }
            else if (IsBody)
            {
                if (Type == SelectionType.Radio)
                {
                    Table.SetSelection(this);
                }
                else
                {
                    RowData.SetSelected(selected, CheckStrictly);
                }
            }
        }

        void IQuickRenderColumn.RenderQuickBody(RenderTreeBuilder builder, RowData rowData)
        {
            if (Hidden || RowSpan == 0 || ColSpan == 0)
            {
                return;
            }

            var cellData = new CellData(rowData);
            builder.OpenElement(0, "td");
            builder.AddMultipleAttributes(1, GetAdditionalAttributes(null));
            builder.AddAttribute(2, "class", $"{ClassMapper.Class} ant-table-selection-column");
            builder.AddAttribute(3, "style", $"{QuickFixedStyle} {Style}");
            builder.AddAttribute(4, "rowspan", RowSpan > 1 ? RowSpan : null);
            builder.AddAttribute(5, "colspan", ColSpan > 1 ? ColSpan : null);
            if (CellRender != null)
            {
                builder.AddContent(6, CellRender(cellData));
            }
            else if (ChildContent != null)
            {
                builder.AddContent(6, ChildContent);
            }
            else if (Type == SelectionType.Checkbox)
            {
                builder.OpenComponent<Checkbox>(7);
                builder.AddAttribute(8, nameof(Checkbox.Checked), rowData.Selected);
                builder.AddAttribute(9, nameof(Checkbox.Disabled), Disabled || rowData.TableDataItem.Disabled);
                builder.AddAttribute(10, nameof(Checkbox.CheckedChange), EventCallback.Factory.Create<bool>(this, selected =>
                {
                    rowData.SetSelected(selected, CheckStrictly);
                    Table.Refresh();
                }));
                builder.CloseComponent();
            }
            else if (Type == SelectionType.Radio)
            {
                builder.OpenComponent<Radio<bool>>(7);
                builder.AddAttribute(8, nameof(Radio<bool>.Checked), rowData.Selected);
                builder.AddAttribute(9, nameof(Radio<bool>.Disabled), Disabled || rowData.TableDataItem.Disabled);
                builder.AddAttribute(10, nameof(Radio<bool>.CheckedChanged), EventCallback.Factory.Create<bool>(this, selected =>
                {
                    if (selected)
                    {
                        Table.SetSelection(this, rowData);
                        Table.Refresh();
                    }
                }));
                builder.CloseComponent();
            }
            builder.CloseElement();
        }

        void IQuickRenderColumn.RenderQuickHeader(RenderTreeBuilder builder)
        {
            var initialize = !QuickHeaderInitialized;
            InitializeQuickHeader();
            if (initialize)
            {
                Context.HeaderColumnInitialized(this);
                Table.Selection = this;
            }
            if (Hidden || HeaderColSpan == 0)
            {
                return;
            }
            builder.OpenElement(0, "th");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "class", $"{ClassMapper.Class} ant-table-selection-column");
            builder.AddAttribute(3, "style", $"{FixedStyle} {HeaderStyle}");
            builder.SetKey(Key);
            builder.AddAttribute(4, "colspan", HeaderColSpan);
            if (Type == SelectionType.Checkbox)
            {
                builder.OpenComponent<Checkbox>(5);
                builder.AddAttribute(6, nameof(Checkbox.Checked), Table.AllSelected);
                builder.AddAttribute(7, nameof(Checkbox.CheckedChange), EventCallback.Factory.Create<bool>(this, selected =>
                {
                    OnCheckedChange(selected);
                    Table.Refresh();
                }));
                builder.AddAttribute(8, nameof(Checkbox.Indeterminate), Indeterminate);
                builder.AddAttribute(9, nameof(Checkbox.Disabled), IsHeaderDisabled);
                builder.CloseComponent();
            }
            builder.CloseElement();
        }

        void IQuickRenderColumn.RenderQuickColGroup(RenderTreeBuilder builder)
        {
            Context?.AddColGroup(this);
            if (Hidden)
            {
                return;
            }
            builder.OpenElement(0, "col");
            builder.AddAttribute(1, "class", "ant-table-selection-col");
            if (Width != null)
            {
                builder.AddAttribute(2, "style", $"width: {(CssSizeLength)Width}; min-width: {(CssSizeLength)Width};");
            }
            builder.CloseElement();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Table == null)
            {
                return;
            }

            if (IsHeader)
            {
                Table.Selection = this;
                Context.HeaderColumnInitialized(this);
            }
            else if (IsBody)
            {
                Table?.Selection?.RowSelections.Add(this);
                DataItem.Disabled = Disabled;
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (IsBody && DataItem != null)
            {
                DataItem.Disabled = Disabled;
            }
        }

        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/4320
        void ISelectionColumn.ResetSelected()
        {
            _selected = null;
        }

        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3312
        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3417
        private void HandleSelected()
        {
            // avoid check the disabled one but allow default checked
            if (Disabled && _selected.HasValue)
            {
                DataItem.SetSelected(_selected.Value);
            }

            _selected = DataItem.Selected;
        }

        void ISelectionColumn.OnDataSourceChange()
        {
            _rowSelections.ForEach(x => x.ResetSelected());
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (IsHeader && Type == SelectionType.Checkbox)
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsHeader)
            {
                if (Table?.Selection?.Type == SelectionType.Radio && Table.ServerSide != true && RowData != null)
                {
                    Table.UnSelectSelection(this);
                }
                Table?.Selection?.RowSelections?.Remove(this);
            }

            base.Dispose(disposing);
        }
    }
}
