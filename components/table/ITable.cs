using System.Collections.Generic;

namespace AntDesign
{
    public interface ITable
    {
        //IList<ITableColumn> Columns { get; }

        //void AddColumn(ITableColumn column);

        ISelectionColumn HeaderSelection { get; set; }

        internal void OnSelectionChanged(int[] checkedIndex);

        internal void Refresh();
    }
}
