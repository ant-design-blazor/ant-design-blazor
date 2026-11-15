// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn
    {
        internal override bool RequiresColGroup => true;

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
        /// Returns additional attributes for a row selection checkbox, based on its source record.
        /// </summary>
        [Parameter]
        public Func<object, IDictionary<string, object>> GetCheckboxProps { get; set; }

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

        private IDictionary<string, object> CheckboxProps
        {
            get
            {
                if (!IsBody || RowData == null)
                {
                    return null;
                }

                var properties = GetCheckboxProps?.Invoke(RowData.Record)?.ToDictionary(x => x.Key, x => x.Value)
                    ?? new Dictionary<string, object>();
                return properties;
            }
        }

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

            // Ant Design fixes the selection column to the left when a
            // following data column is fixed to the left. Keep the selection
            // column in the fixed-column offset chain as well.
            if (!Fixed.HasValue && Context?.Columns.Any(column =>
                    !ReferenceEquals(column, this)
                    && column.ColIndex > ColIndex
                    && column.Fixed == ColumnFixPlacement.Left) == true)
            {
                Fixed = ColumnFixPlacement.Left;
                ((IColumn)this).UpdateFixedStyle();
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
