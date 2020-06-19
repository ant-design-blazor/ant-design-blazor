using System.Collections.Generic;

namespace AntDesign
{
    public interface ISelectionColumn : IColumn
    {
        public bool Disabled { get; set; }

        internal bool Checked { get; set; }

        public void Check(bool @checked);

        public string Key { get; set; }

        public int RowIndex { get; set; }

        public IList<ISelectionColumn> RowSelections { get; set; }
    }
}
