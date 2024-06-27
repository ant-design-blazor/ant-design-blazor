using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AntDesign.Filters;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IFieldColumn : IColumn
    {
        public string DisplayName { get; }

        public string FieldName { get; }

        public string Format { get; }

        public bool Sortable { get; }

        public bool Filterable { get; }

        public int SorterMultiple { get; }

        public bool Grouping { get; }

        public ITableSortModel SortModel { get; }

        public ITableFilterModel FilterModel { get; }

        public IFieldFilterType FieldFilterType { get; }

        internal void ClearSorter();

        internal void ClearFilters();

        internal IEnumerable<TableFilter> Filters { get; }
        internal void SetFilterModel(ITableFilterModel filterModel);

        internal void SetSelectedFilters(IEnumerable<TableFilter> selectedFilters);

        internal void SetSortModel(ITableSortModel sortModel);

        internal IQueryable<IGrouping<object, TItem>> Group<TItem>(IQueryable<TItem> source);
    }
}
