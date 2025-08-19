// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public class ColumnContext
    {
        public IList<IColumn> Columns { get; set; } = new List<IColumn>();

        public IList<IColumn> HeaderColumns { get; set; } = new List<IColumn>();

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
            HeaderColumns.Add(column);
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
                if (string.IsNullOrEmpty(totalWidth))
                {
                    totalWidth = "0px";
                }
                foreach (var col in Columns.Where(x => x.Width == null))
                {
                    col.Width = $"calc(({(CssSizeLength)_table.ScrollX} - ({totalWidth}) ) / {zeroWidthCols.Length})";
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

            if (_table.AutoColIndexes)
            {
                column.ColIndex = CurrentColIndex;
            }

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

        internal void HeaderColumnInitialized(IColumn column)
        {
            var shouldInvokeInitialized = _table.HasHeaderTemplate
                ? column.ColIndex == Columns.Count - 1
                : HeaderColumns.Count == Columns.Count;

            if (shouldInvokeInitialized)
            {
                // Header columns have all been initialized, then we can invoke the first change.
                _table.OnColumnInitialized();
            }
        }
    }
}
