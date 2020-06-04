using System.Collections.Generic;

namespace AntDesign
{
    public interface ITable
    {
        void AddColumn(ITableColumn column);

        IRowSelection HeaderSelection { get; set; }

        //void Selected(IRowSelection selection, bool @checked);

        //void SelectAll();

        //void UnSelectAll();

        //IEnumerable<IRowSelection> SelectedRows { get; }
    }
}
