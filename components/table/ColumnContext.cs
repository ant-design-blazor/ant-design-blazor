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

        private int[] ColIndexOccupied { get; set; }

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

            if (column.RowSpan > 1)
            {
                ColIndexOccupied ??= new int[Columns.Count];
                for (var i = column.ColIndex; i <= CurrentColIndex; i++)
                {
                    ColIndexOccupied[i] = column.RowSpan;
                }
            }

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


            var columnSpan = column.ColSpan;
            if (column.RowSpan == 0) columnSpan = 0;

            column.ColIndex = CurrentColIndex;
            CurrentColIndex += columnSpan - 1;

            do
            {
                if (++CurrentColIndex >= Columns.Count)
                {
                    CurrentColIndex = 0;
                    if (ColIndexOccupied != null)
                    {
                        foreach (ref var item in ColIndexOccupied.AsSpan())
                        {
                            if (item > 0) item--;
                        }
                    }
                }
            }
            while (ColIndexOccupied != null && ColIndexOccupied[CurrentColIndex] > 0);

        }

        internal void HeaderColumnInitialed(IColumn column)
        {
            if (column.ColIndex == Columns.Count - 1)
            {
            }


            // Header columns have all been initialized, then we can invoke the first change.
            _table.OnColumnInitialized();
        }
    }
}
