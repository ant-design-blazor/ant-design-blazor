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

            column.Table = _table;
            column.ColIndex = CurrentColIndex++;
            Columns.Add(column);
        }

        public void AddHeaderColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            var columnSpan = column.HeaderColSpan;
            if (column.RowSpan == 0) columnSpan = 0;

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

            column.ColIndex = CurrentColIndex;
            CurrentColIndex += columnSpan - 1;

            if (column.RowSpan > 1)
            {
                ColIndexOccupied ??= new int[Columns.Count];
                for (var i = column.ColIndex; i <= CurrentColIndex; i++)
                {
                    ColIndexOccupied[i] = column.RowSpan;
                }
            }
        }

        public void AddRowColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            var columnSpan = column.ColSpan;
            if (column.RowSpan == 0) columnSpan = 0;

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

            column.ColIndex = CurrentColIndex;
            CurrentColIndex += columnSpan - 1;

            if (column.RowSpan > 1)
            {
                ColIndexOccupied ??= new int[Columns.Count];
                for (var i = column.ColIndex; i <= CurrentColIndex; i++)
                {
                    ColIndexOccupied[i] = column.RowSpan;
                }
            }
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
