using System.Collections.Generic;
using System.Linq;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn
    {
        [Parameter] public string Type { get; set; } = "checkbox";

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string Key { get; set; }

        [Parameter] public bool CheckStrictly { get; set; }

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        //private bool _checked;

        private bool Indeterminate => IsHeader
                                      && !Table.AllSelected
                                      && Table.AnySelected;

        public IList<ISelectionColumn> RowSelections { get; set; } = new List<ISelectionColumn>();

        //private int[] _selectedIndexes;

        private bool IsHeaderDisabled => RowSelections.All(x => x.Disabled);

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
                if (Type == "radio")
                {
                    Table.SetSelection(this);
                }
                else
                {
                    RowData.Selected = selected;
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

            if (IsHeader)
            {
                Table.Selection = this;
                Context.HeaderColumnInitialed(this);
            }
            else if (IsBody)
            {
                Table?.Selection?.RowSelections.Add(this);
            }
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
            if (IsHeader && Type == "checkbox")
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
