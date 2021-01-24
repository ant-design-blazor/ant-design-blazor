using System.Linq;

namespace AntDesign.TableModels
{
    public interface ITableSortModel
    {
        public string Sort { get; }

        public int Priority { get; }

        public string FieldName { get; }

        internal SortDirection SortDirection { get; }

        internal void SetSortDirection(SortDirection sortDirection);

        internal IOrderedQueryable<TItem> SortList<TItem>(IQueryable<TItem> source);
    }
}
