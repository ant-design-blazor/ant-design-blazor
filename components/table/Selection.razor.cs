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

        //private bool _checked;

        private bool Indeterminate => !Table.AllSelected
                                      && Table.AnySelected;

        public IList<ISelectionColumn> RowSelections { get; set; } = new List<ISelectionColumn>();

        //private int[] _selectedIndexes;

        private bool IsHeaderDisabled => RowSelections.All(x => x.Disabled);

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

            Table?.Selection?.RowSelections.Add(this);
            Context.HeaderColumnInitialed(this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            //if (IsHeader && Type == "radio" && RowSelections.Count(x => x.Checked) > 1)
            //{
            //    var first = RowSelections.FirstOrDefault(x => x.Checked);
            //    if (first != null)
            //    {
            //        Table?.Selection.RowSelections.Where(x => x.ColIndex != first.ColIndex).ForEach(x => x.RowData.SetSelected(false));
            //    }
            //}
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
