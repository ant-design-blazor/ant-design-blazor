using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntDesign
{
    public class ColumnContext
    {
        public IList<IColumn> Columns { get; set; } = new List<IColumn>();

        private int CurrentColIndex { get; set; }

        private ITable _table;

        public ColumnContext(ITable table)
        {
            _table = table;
        }

        public void AddHeaderColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            column.ColIndex = CurrentColIndex++;
            Columns.Add(column);

            if (_table.ScrollX != null)
            {
                var zeroWidthCols = Columns.Where(x => x.Width == null).ToArray();
                var totalWidth = Columns.Where(x => x.Width != null).Sum(x => ((CssSizeLength)x.Width).Value);
                foreach (var col in Columns.Where(x => x.Width == null))
                {
                    col.Width = $"{(((CssSizeLength)_table.ScrollX).Value - totalWidth) / zeroWidthCols.Length}";
                }
            }
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
