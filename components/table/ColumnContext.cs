using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntDesign
{
    public class ColumnContext
    {
        public IList<IColumn> Columns { get; set; } = new List<IColumn>();

        public IList<IColumn> HeaderColumns { get; set; } = new List<IColumn>();

        private int CurrentColIndex { get; set; }

        private ITable _table;

        public ColumnContext(ITable table)
        {
            _table = table;
        }

        public void AddColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            column.ColIndex = CurrentColIndex++;
            Columns.Add(column);
        }

        public void AddHeaderColumn(IColumn column)
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
            HeaderColumns.Add(column);
        }

        public void AddColGroup(IColumn column)
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

            if (_table.ScrollX != null && Columns.Any(x => x.Width == null))
            {
                var zeroWidthCols = Columns.Where(x => x.Width == null).ToArray();
                var totalWidth = string.Join(" + ", Columns.Where(x => x.Width != null).Select(x => (CssSizeLength)x.Width));
                foreach (var col in Columns.Where(x => x.Width == null))
                {
                    col.Width = $"calc(({(CssSizeLength)_table.ScrollX} - ({totalWidth}) + 3px) / {zeroWidthCols.Length})";
                }
            }

            if (column.Width == null)
            {
                var col = Columns.FirstOrDefault(x => x.ColIndex == column.ColIndex);
                if (col != null)
                {
                    column.Width = col.Width;
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
