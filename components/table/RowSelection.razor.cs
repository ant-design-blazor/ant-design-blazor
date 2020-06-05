using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class RowSelection<TData> : AntDomComponentBase, IRowSelection
    {
        [CascadingParameter(Name = "IsHeader")] public bool IsHeader { get; set; }

        [CascadingParameter(Name = "Index")] public int Index { get; set; }

        [CascadingParameter] public ITable Table { get; set; }

        [CascadingParameter] public TData RowData { get; set; }

        [Parameter] public string Type { get; set; }

        [Parameter] public IEnumerable<TData> SelectedRows { get; set; } = Enumerable.Empty<TData>();

        [Parameter] public EventCallback<IEnumerable<TData>> SelectedRowsChanged { get; set; }

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

        private void HandleCheckedChange(bool @checked)
        {
            this.Checked = @checked;
            if (this.IsHeader)
            {
                RowSelections.ForEach(x => x.Check(@checked));
            }
            else
            {
                if (Type == "radio")
                {
                    Table?.HeaderSelection.RowSelections.Where(x => x.Index != this.Index).ForEach(x => x.Check(false));
                }

                Table?.HeaderSelection.Check(@checked);
            }
        }

        void IRowSelection.Check(bool @checked)
        {
            this.Checked = @checked;
            StateHasChanged();
        }
    }
}
