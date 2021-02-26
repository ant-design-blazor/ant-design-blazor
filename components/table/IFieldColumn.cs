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

        public int SorterMultiple { get; }

        public ITableSortModel SortModel { get; }

        public IEnumerable<ITableFilterModel> FilterModel { get; }

        internal void ClearSorter();
    }
}
