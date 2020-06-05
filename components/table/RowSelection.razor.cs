using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class RowSelection : AntDomComponentBase, IRowSelection
    {
        [CascadingParameter(Name = "IsHeader")] public bool IsHeader { get; set; }

        [CascadingParameter(Name = "Index")] public int Index { get; set; }

        [CascadingParameter] public ITable Table { get; set; }

        [Parameter] public string Type { get; set; } = "checkbox";

        [Parameter] public bool Disabled { get; set; }

        public bool Checked { get; set; }

        private bool Indeterminate => IsHeader
                                      && this.RowSelections.Any(x => x.Checked)
                                      && !this.RowSelections.All(x => x.Checked);

        public IList<IRowSelection> RowSelections { get; set; } = new List<IRowSelection>();

        protected override void OnInitialized()
        {
            if (Table != null)
            {
                if (IsHeader)
                {
                    Table.HeaderSelection = this;
                }
                else
                {
                    Table?.HeaderSelection.RowSelections.Add(this);
                }
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (IsHeader && Type == "radio" && RowSelections.Count(x => x.Checked) > 1)
            {
                var first = RowSelections.FirstOrDefault(x => x.Checked);
                if (first != null)
                {
                    Table?.HeaderSelection.RowSelections.Where(x => x.Index != first.Index).ForEach(x => x.Check(false));
                }
            }
        }

        private void HandleCheckedChange(bool @checked)
        {
            this.Checked = @checked;

            if (this.IsHeader)
            {
                RowSelections.Where(x => !x.Disabled).ForEach(x => x.Check(@checked));
            }
            else
            {
                if (Type == "radio")
                {
                    Table?.HeaderSelection.RowSelections.Where(x => x.Index != this.Index).ForEach(x => x.Check(false));
                }

                Table?.HeaderSelection.Check(@checked);
            }

            InvokeSelectedRowsChange();
        }

        void IRowSelection.Check(bool @checked)
        {
            this.Checked = @checked;
            StateHasChanged();

            InvokeSelectedRowsChange();
        }

        private void InvokeSelectedRowsChange()
        {
            if (IsHeader)
            {
                var checkedIndex = RowSelections.Where(x => x.Checked).Select(x => x.Index).ToArray();
                Table.OnSelectionChanged(checkedIndex);
            }
        }
    }
}
