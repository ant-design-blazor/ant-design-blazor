using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn
    {
        [Parameter] public string Type { get; set; } = "checkbox";

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string Key { get; set; }

        [Parameter] public bool CheckStrictly { get; set; }

        bool ISelectionColumn.Checked
        {
            get => _checked;
            set => _checked = value;
        }

        private bool _checked;

        private bool Indeterminate => IsHeader
                                      && this.RowSelections.Where(x => !x.Disabled).Any(x => x.Checked)
                                      && !this.RowSelections.Where(x => !x.Disabled).All(x => x.Checked);

        public IList<ISelectionColumn> RowSelections { get; set; } = new List<ISelectionColumn>();

        private int[] _selectedIndexes;

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

        private void HandleCheckedChange(bool @checked)
        {
            if (this.IsHeader)
            {
                RowSelections.Where(x => !x.Disabled).ForEach(x => x.RowData.SetSelected(@checked));
                InvokeSelectedRowsChange();
            }
            else if (IsBody)
            {
                RowData.SetSelected(@checked);

                if (Type == "radio")
                {
                    Table?.Selection.RowSelections.Where(x => x.RowData.CacheKey != this.RowData.CacheKey).ForEach(x => x.RowData.SetSelected(false));
                }

                Table?.Selection.InvokeSelectedRowsChange();
            }
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
                StateHasChanged();

                return true;
            }

            return false;
        }

        public void InvokeSelectedRowsChange()
        {
            if (IsHeader)
            {
                Table.SelectionChanged();
            }
        }

        public void ChangeSelection()
        {
            this.Table.Selection.RowSelections.ForEach(x => x.Check(x.RowData.Selected));
            this.Table.Selection.StateHasChanged();
        }

        public void SetSelection(string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                this.Table.Selection.RowSelections.ForEach(x => x.RowData.SetSelected(false));
                this.Table.Selection.InvokeSelectedRowsChange();
            }
            else
            {
                this.Table.Selection.RowSelections.ForEach(x => x.RowData.SetSelected(x.Key.IsIn(keys)));
                this.Table.Selection.InvokeSelectedRowsChange();
            }
        }

        public void ChangeOnPaging()
        {
            this.ChangeSelection();
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (IsHeader)
            {
                _checked = this.RowSelections.Any() && this.RowSelections.Where(x => !x.Disabled).All(x => x.Checked);
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
