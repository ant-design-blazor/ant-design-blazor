using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public int SorterMultiple { get; }

        public bool Grouping { get; }

        public ITableSortModel SortModel { get; }

        public ITableFilterModel FilterModel { get; }

        internal void ClearSorter();

        internal void ClearFilters();

        internal void SetFilterModel(ITableFilterModel filterModel);

        internal void SetSortModel(ITableSortModel sortModel);

        internal IQueryable<IGrouping<object, TItem>> Group<TItem>(IQueryable<TItem> source);
    }
}
