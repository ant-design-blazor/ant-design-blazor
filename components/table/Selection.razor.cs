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

        public IList<ISelectionColumn> RowSelections { get; set; } = new List<ISelectionColumn>();

        //private int[] _selectedIndexes;

        private bool IsHeaderDisabled => RowSelections.Any() && RowSelections.All(x => x.Disabled);

        public bool Selected => DataItem.Selected;

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
                Context.HeaderColumnInitialed(this);
            }
            else if (IsBody)
            {
                Table?.Selection?.RowSelections.Add(this);
                DataItem.Disabled = Disabled;
            }
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

        void ISelectionColumn.StateHasChanged()
        {
            if (IsHeader && Type == SelectionType.Radio)
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsHeader)
            {
                Table?.Selection?.RowSelections?.Remove(this);
            }

            base.Dispose(disposing);
        }
    }
}
