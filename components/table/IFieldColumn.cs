using System.Collections.Generic;
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

        public bool Groupable { get; set; }

        public int SorterMultiple { get; }

        public ITableSortModel SortModel { get; }

        public ITableFilterModel FilterModel { get; }

        public ITableGroupModel GroupModel { get; }

        internal void ClearSorter();

        internal void ClearFilters();

        internal void ClearGroups();

        internal void SetFilterModel(ITableFilterModel filterModel);

        internal void SetSortModel(ITableSortModel sortModel);

        internal void SetGroupModel(ITableGroupModel groupModel);
    }
}
