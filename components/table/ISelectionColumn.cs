using System.Collections.Generic;

namespace AntDesign
{
    public interface ISelectionColumn : IColumn
    {
        public bool Disabled { get; }

        internal bool Checked { get; set; }

        public string Key { get; }

        public IList<ISelectionColumn> RowSelections { get; }

        public bool Check(bool @checked);

        public void ChangeSelection();

        public void SetSelection(string[] keys);

        public void ChangeOnPaging();

        public void InvokeSelectedRowsChange();

        public void StateHasChanged();
    }
}
