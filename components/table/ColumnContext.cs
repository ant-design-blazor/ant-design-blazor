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

        public IList<IColumn> ColumnDefs => Columns.Where(x => x.IsInitialize).ToList();

        private int _nextColumnIndex;

        private int _headerColIndex = -1;

        private int _bodyColIndex = -1;

        private int _colGroupIndex = -1;

        private readonly HashSet<IColumn> _registeredColumns = new();

        private int[] _headerColIndexesOccupied;

        private int[] _bodyColIndexesOccupied;

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

            if (!_registeredColumns.Add(column))
            {
                return;
            }

            column.ColIndex = _nextColumnIndex++;
            Columns.Add(column);
        }

        public void RemoveColumn(IColumn column)
        {
            if (column == null || !_registeredColumns.Remove(column))
            {
                return;
            }

            Columns.Remove(column);
            HeaderColumns.Remove(column);
            for (var i = 0; i < Columns.Count; i++)
            {
                Columns[i].ColIndex = i;
            }
            _nextColumnIndex = Columns.Count;
            _headerColIndexesOccupied = null;
            _bodyColIndexesOccupied = null;
        }

        public void BeginHeaderRow()
        {
            _headerColIndex = -1;
            AdvanceOccupiedColumns(_headerColIndexesOccupied);
        }

        public void BeginBodyRow()
        {
            _bodyColIndex = -1;
            AdvanceOccupiedColumns(_bodyColIndexesOccupied);
        }

        public void AddHeaderColumn(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            if (Columns.Count == 0)
            {
                return;
            }

            var columnSpan = column.HeaderColSpan;
            if (column.RowSpan == 0) columnSpan = 0;

            _headerColIndex = FindNextColumnIndex(_headerColIndex, _headerColIndexesOccupied);

            column.ColIndex = _headerColIndex;
            if (!HeaderColumns.Contains(column))
            {
                HeaderColumns.Add(column);
            }
            _headerColIndex += columnSpan - 1;

            if (column.RowSpan > 1)
            {
                _headerColIndexesOccupied ??= new int[Columns.Count];
                for (var i = column.ColIndex; i <= _headerColIndex; i++)
                {
                    _headerColIndexesOccupied[i] = column.RowSpan;
                }
            }
        }

        public void AddColGroup(IColumn column)
        {
            if (column == null)
            {
                return;
            }

            if (Columns.Count == 0)
            {
                return;
            }

            if (++_colGroupIndex >= Columns.Count) _colGroupIndex = 0;

            column.ColIndex = _colGroupIndex;

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

            if (Columns.Count == 0)
            {
                return;
            }

            var rowSpan = column.GetRowSpan(column.RowData);
            var columnSpan = column.GetColSpan(column.RowData);
            if (rowSpan == 0) columnSpan = 0;

            _bodyColIndex = FindNextColumnIndex(_bodyColIndex, _bodyColIndexesOccupied);

            if (_table.AutoColIndexes)
            {
                column.ColIndex = _bodyColIndex;
            }

            _bodyColIndex += columnSpan - 1;

            if (rowSpan > 1)
            {
                _bodyColIndexesOccupied ??= new int[Columns.Count];
                for (var i = column.ColIndex; i <= _bodyColIndex; i++)
                {
                    _bodyColIndexesOccupied[i] = rowSpan;
                }
            }
        }

        private static void AdvanceOccupiedColumns(int[] occupiedColumns)
        {
            if (occupiedColumns == null)
            {
                return;
            }

            foreach (ref var item in occupiedColumns.AsSpan())
            {
                if (item > 0) item--;
            }
        }

        private int FindNextColumnIndex(int currentIndex, int[] occupiedColumns)
        {
            for (var offset = 1; offset <= Columns.Count; offset++)
            {
                var candidate = (currentIndex + offset) % Columns.Count;
                if (occupiedColumns == null || occupiedColumns[candidate] == 0)
                {
                    return candidate;
                }
            }

            // Invalid spans can occupy every registered slot. Keep rendering
            // bounded so a malformed template cannot hang the whole table.
            return (currentIndex + 1) % Columns.Count;
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
