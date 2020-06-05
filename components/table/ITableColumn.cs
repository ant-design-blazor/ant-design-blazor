using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface ITableColumn
    {
        public int Index { get; set; }

        public string DisplayName { get; }

        public string FieldName { get; }
    }
}
