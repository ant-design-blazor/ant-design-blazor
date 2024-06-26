using System.Collections.Generic;

namespace AntDesign
{
    internal interface ISelectionColumn : IColumn
    {
        public string Type { get; set; }

        public bool Disabled { get; }

        public string Key { get; }

        public bool Selected { get; }

        public bool CheckStrictly { get; set; }

        public IList<ISelectionColumn> RowSelections { get; }

        public void StateHasChanged();
    }
}
