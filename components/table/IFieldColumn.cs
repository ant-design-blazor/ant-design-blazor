// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
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

        internal Expression<Func<TItem, object>> GetGroupByExpression<TItem>();

        internal void SetHeaderFilter(Type columnDataType);
    }
}
