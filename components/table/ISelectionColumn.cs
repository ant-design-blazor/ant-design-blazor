using System.Collections.Generic;

namespace AntDesign
{
    public interface ISelectionColumn : IColumn
    {
        public bool Disabled { get; }

        public string Key { get; }

        public bool Selected { get; }

        public IList<ISelectionColumn> RowSelections { get; }

        public void StateHasChanged();
    }
}
