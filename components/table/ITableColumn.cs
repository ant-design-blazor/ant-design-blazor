using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface ITableColumn
    {
        public string DisplayName { get; }

        public string FieldName { get; }
    }
}
