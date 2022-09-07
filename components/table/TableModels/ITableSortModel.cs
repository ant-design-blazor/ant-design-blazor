using System;
using System.Linq;

namespace AntDesign.TableModels
{
    public interface ITableSortModel : ICloneable
    {
        public string Sort { get; }

        public int Priority { get; }

        public string FieldName { get; }

        public int ColumnIndex { get; }

        internal SortDirection SortDirection { get; }

        internal void SetSortDirection(SortDirection sortDirection);

        internal IQueryable<TItem> SortList<TItem>(IQueryable<TItem> source);
    }
}
