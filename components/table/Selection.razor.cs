using System.Collections.Generic;
using System.Linq;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using AntDesign.Table;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn, IRenderColumn
    {
        [Parameter] public string Type { get; set; } = "checkbox";

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string Key { get; set; }

        [Parameter] public bool CheckStrictly { get; set; } = true;

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        private bool Indeterminate => !Table.AllSelected && Table.AnySelected;

        private bool Indeterminate => IsHeader
                                   && Table.AnySelected
                                   && !Table.AllSelected;

        private bool IsHeaderDisabled => _rowSelections.Any() && _rowSelections.All(x => x.Disabled);

        bool ISelectionColumn.Disabled => Disabled;

        private bool IsHeaderDisabled => RowSelections.Any() && RowSelections.All(x => x.Disabled);

        public bool Selected => DataItem.Selected;

        private bool? _selected;

        private void OnCheckedChange(bool selected)
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
                    Table.SetSelection(this);
                }
                else
                {
                    RowData.SetSelected(selected, CheckStrictly);
                }
            }
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (Type == "checkbox")
            {
                StateHasChanged();
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
            if (IsHeader && Type == "checkbox")
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            Table?.Selection?.RowSelections?.Remove(this);
            base.Dispose(disposing);
        }
    }
}
