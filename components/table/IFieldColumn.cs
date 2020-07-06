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

        public ITableSortModel SortModel { get; }
    }
}
