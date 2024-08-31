// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    internal class ColumnContext
    {
        public IReadOnlyList<IColumn> Columns => _columns.Where(x => !x.Hidden).ToList();

        private IList<IColumn> _columns = new List<IColumn>();

        private int _currentColIndex;

        private int[] _colIndexOccupied;

        public ITable Table { get; }

        private bool _collectingColumns;

        public ColumnContext(ITable table)
        {
            _collectingColumns = true;
            Table = table;
        }

        public void AddColumn(IColumn column)
        {
            if (!_collectingColumns)
            {
                return;
            }
            if (column == null)
            {
                return;
            }

            column.ColIndex = _currentColIndex++;
            _columns.Add(column);
        }

        public void RemoveColumn(IColumn column)
        {
            _columns.Remove(column);
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
                if (++_currentColIndex >= _columns.Count)
                {
                    _currentColIndex = 0;
                    if (_colIndexOccupied != null)
                    {
                        foreach (ref var item in _colIndexOccupied.AsSpan())
                        {
                            if (item > 0) item--;
                        }
                    }
                }
            }
            while (_colIndexOccupied != null && _colIndexOccupied[_currentColIndex] > 0);

            column.ColIndex = _currentColIndex;
            _currentColIndex += columnSpan - 1;

            if (column.RowSpan > 1)
            {
                _colIndexOccupied ??= new int[_columns.Count];
                for (var i = column.ColIndex; i <= _currentColIndex; i++)
                {
                    _colIndexOccupied[i] = column.RowSpan;
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
                if (++_currentColIndex >= _columns.Count)
                {
                    _currentColIndex = 0;
                    if (_colIndexOccupied != null)
                    {
                        foreach (ref var item in _colIndexOccupied.AsSpan())
                        {
                            if (item > 0) item--;
                        }
                    }
                }
            }
            while (_colIndexOccupied != null && _colIndexOccupied[_currentColIndex] > 0);

            column.ColIndex = _currentColIndex;
            _currentColIndex += columnSpan - 1;

            if (column.RowSpan > 1)
            {
                _colIndexOccupied ??= new int[_columns.Count];
                for (var i = column.ColIndex; i <= _currentColIndex; i++)
                {
                    _colIndexOccupied[i] = column.RowSpan;
                }
            }
        }

        internal void StartCollectingColumns()
        {
            _columns.Clear();
            _currentColIndex = 0;
            _collectingColumns = true;
        }

        internal void HeaderColumnInitialed()
        {
            if (Table.ScrollX != null && _columns.Any(x => x.Width == null))
            {
                var zeroWidthCols = _columns.Where(x => x.Width == null).ToArray();
                var totalWidth = string.Join(" + ", _columns.Where(x => x.Width != null).Select(x => (CssSizeLength)x.Width));
                foreach (var col in zeroWidthCols)
                {
                    col.Width = $"calc(({(CssSizeLength)Table.ScrollX} - ({totalWidth}) + 3px) / {zeroWidthCols.Length})";
                }
            }
            _collectingColumns = false;
            // Header columns have all been initialized, then we can invoke the first change.
            Table.OnColumnInitialized();

            _columns.ForEach(x => x.Load());
        }
    }
}
