using System.Linq;

namespace AntDesign.TableModels
{
    public interface ITableSortModel
    {
        public SortType SortType { get; }

        public int Priority { get; }

        public string FieldName { get; }

        IOrderedQueryable<TItem> Sort<TItem>(IQueryable<TItem> source);

        internal void SetSortType(SortType sortType);

        internal void SetSortType(string sortType);

        internal void SwitchSortType();
    }
}
