using System.Collections.Generic;

namespace AntDesign
{
    public interface ISelectionColumn : IColumn
    {
        public bool Disabled { get; set; }

        public bool Checked { get; set; }

        public void Check(bool @checked);

        public IList<ISelectionColumn> RowSelections { get; set; }
    }
}
