using System;
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

        internal Expression<Func<TItem, object>> GetGroupByExpression<TItem>();
    }
}
