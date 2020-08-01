using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class ColumnContext
    {
        public IList<IColumn> Columns { get; set; } = new List<IColumn>();

        private int CurrentColIndex { get; set; }

        public void AddHeaderColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            column.ColIndex = CurrentColIndex++;
            Columns.Add(column);
        }

        public void AddRowColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            if (++CurrentColIndex >= Columns.Count)
            {
                CurrentColIndex = 0;
            }

            column.ColIndex = CurrentColIndex;
        }
    }
}
