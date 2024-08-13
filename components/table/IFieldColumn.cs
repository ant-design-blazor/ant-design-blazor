using System.Linq;
using AntDesign.TableModels;

namespace AntDesign
{
    public interface IFieldColumn : IColumn
    {
        public string DisplayName { get; }

        public string FieldName { get; }

        public string Format { get; set; }

        public bool Sortable { get; set; }

        public bool Filterable { get; set; }

        public int SorterMultiple { get; set; }

        public bool Grouping { get; set; }

        public ITableSortModel SortModel { get; }

        public ITableFilterModel FilterModel { get; }

        internal void ClearSorter();

        internal void ClearFilters();

        internal void SetFilterModel(ITableFilterModel filterModel);

        internal void SetSortModel(ITableSortModel sortModel);

        internal IQueryable<IGrouping<object, TItem>> Group<TItem>(IQueryable<TItem> source);
    }
}
