using System.Collections.Generic;

namespace AntDesign
{
    internal interface ISelectionColumn : IColumn
    {
        public bool Disabled { get; }

        public string Key { get; }

        public IList<ISelectionColumn> RowSelections { get; }

        public void StateHasChanged();
    }
}
