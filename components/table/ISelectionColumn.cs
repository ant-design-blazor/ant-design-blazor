using System.Collections.Generic;

namespace AntDesign
{
    public interface ISelectionColumn : IColumn
    {
        public bool Disabled { get; }

        public string Key { get; }

        public IList<ISelectionColumn> RowSelections { get; }

        public bool Check(bool @checked);

        public void StateHasChanged();
    }
}
