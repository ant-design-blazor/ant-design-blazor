using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public class SortModelRemote<TField> : ITableSortModel, IComparer<TField>, ICloneable
    {
        public int Priority { get; }
        public string FieldName { get; }
        public string Sort => _sortDirection?.Name;

        SortDirection ITableSortModel.SortDirection => _sortDirection;

        public int ColumnIndex => _columnIndex;

        private readonly Func<TField, TField, int> _comparer;

        private SortDirection _sortDirection;

        private int _columnIndex;


        public SortModelRemote(IFieldColumn column, int priority, SortDirection defaultSortOrder, Func<TField, TField, int> comparer)
        {
            this.Priority = priority;
            this._columnIndex = column.ColIndex;
            this._comparer = comparer;
            this._sortDirection = defaultSortOrder ?? SortDirection.None;
            this.FieldName = column.RemoteFieldName;
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public SortModelRemote(int columnIndex, int priority, string sort)
        {
            this.Priority = priority;
            this._columnIndex = columnIndex;
            this._sortDirection = SortDirection.Parse(sort);
        }

        void ITableSortModel.SetSortDirection(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        IQueryable<TItem> ITableSortModel.SortList<TItem>(IQueryable<TItem> source)
        {
            throw new Exception("SortModelRemote does not implement SortList()");
        }

        /// <inheritdoc />
        public int Compare(TField x, TField y)
        {
            return _comparer?.Invoke(x, y) ?? 0;
        }

        public object Clone()
        {
            return new SortModelRemote<TField>(_columnIndex, Priority, Sort);
        }
    }
}
