using System.Collections.Generic;

namespace AntDesign
{
    public interface ITable
    {
        void AddColumn(ITableColumn column);

        IRowSelection HeaderSelection { get; set; }

        internal void OnSelectionChanged(int[] checkedIndex);
    }
}
