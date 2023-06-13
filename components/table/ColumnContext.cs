using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntDesign
{
    public class ColumnContext
    {
        public IList<IColumn> Columns { get; set; } = new List<IColumn>();

        private int _currentColIndex;

        //private int[] ColIndexOccupied { get; set; }

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

            column.ColIndex = _currentColIndex++;
            Columns.Add(column);
        }

        internal void HeaderColumnInitialed()
        {
            if (_table.ScrollX != null && Columns.Any(x => x.Width == null))
            {
                var zeroWidthCols = Columns.Where(x => x.Width == null).ToArray();
                var totalWidth = string.Join(" + ", Columns.Where(x => x.Width != null).Select(x => (CssSizeLength)x.Width));
                foreach (var col in zeroWidthCols)
                {
                    col.Width = $"calc(({(CssSizeLength)_table.ScrollX} - ({totalWidth}) + 3px) / {zeroWidthCols.Length})";
                }
            }

            // Header columns have all been initialized, then we can invoke the first change.
            _table.OnColumnInitialized();
        }
    }
}
