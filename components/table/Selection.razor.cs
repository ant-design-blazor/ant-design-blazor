// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using AntDesign.Table;
using System.Collections.Generic;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn, IRenderColumn
    {
        [Parameter] public string Type { get; set; } = "checkbox";

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string Key { get; set; }

        [Parameter] public bool CheckStrictly { get; set; }

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        private bool Indeterminate => !Table.AllSelected && Table.AnySelected;

        private IList<ISelectionColumn> _rowSelections = new List<ISelectionColumn>();

        private bool IsHeaderDisabled => _rowSelections.Any() && _rowSelections.All(x => x.Disabled);

        bool ISelectionColumn.Disabled => Disabled;

        string ISelectionColumn.Key => Key;

        IList<ISelectionColumn> ISelectionColumn.RowSelections => _rowSelections;

        private bool? _selected;

        private void OnCheckedChange(bool selected, RowData rowData = null, bool isHeader = false)
        {
            if (isHeader)
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
            else
            {
                if (Type == "radio")
                {
                    Table.SetSelection(new[] { Key });
                }
                else
                {
                    rowData.Selected = selected;
                    Table.Selection.StateHasChanged();
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

            Table.Selection = this;

            Table.Selection.RowSelections.Add(this);
        }

        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3312
        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3417
        private void HandleSelected(TableDataItem dataItem)
        {
            // avoid check the disabled one but allow default checked
            if (Disabled && _selected.HasValue)
            {
                dataItem.SetSelected(_selected.Value);
            }

            _selected = dataItem.Selected;
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (Type == "checkbox")
            {
                StateHasChanged();
            }
        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            Table?.Selection?.RowSelections?.Remove(this);
            base.Dispose(disposing);
        }
    }
}
