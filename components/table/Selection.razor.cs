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

        [CascadingParameter(Name = "AntDesign.Selection.OnChange")]
        internal EventCallback<bool> OnChange { get; set; }

        [CascadingParameter(Name = "AntDesign.Selection.TableRow")]
        internal ITableRow TableRow { get; set; }

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        private bool _checked;

        private bool Indeterminate => IsHeader
                                      && this.RowSelections.Where(x => !x.Disabled).Any(x => x.RowData.Selected)
                                      && !this.RowSelections.Where(x => !x.Disabled).All(x => x.RowData.Selected);

        public IList<ISelectionColumn> RowSelections { get; set; } = new List<ISelectionColumn>();

        //private int[] _selectedIndexes;

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
                TableRow.Selection = this;
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

        bool ISelectionColumn.Check(bool @checked)
        {
            return this.Check(@checked);
        }

        private bool Check(bool @checked)
        {
            if (this._checked != @checked)
            {
                this._checked = @checked;
                //StateHasChanged();

                return true;
            }

            return false;
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (IsHeader && Type == "checkbox")
            {
                _checked = this.RowSelections.Any() && this.RowSelections.Where(x => !x.Disabled).All(x => x.RowData.Selected);
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsHeader)
            {
                Table.Selection.RowSelections.Remove(this);
            }

            base.Dispose(disposing);
        }
    }
}
