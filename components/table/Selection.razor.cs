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

        [Parameter] public bool CheckStrictly { get; set; }

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        private bool Indeterminate => !Table.AllSelected && Table.AnySelected;

        private IList<ISelectionColumn> _rowSelections = new List<ISelectionColumn>();

        private bool IsHeaderDisabled => _rowSelections.All(x => x.Disabled);

        bool ISelectionColumn.Disabled => Disabled;

        string ISelectionColumn.Key => Key;

        IList<ISelectionColumn> ISelectionColumn.RowSelections => _rowSelections;

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
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (Type == "checkbox")
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
