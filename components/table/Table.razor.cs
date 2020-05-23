using System.Collections.Generic;
using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public sealed partial class Table<TData> : ITable
    {
        [Parameter]
        public IList<TData> DataSource { get; set; }

        [Parameter]
        public IList<ColumnType> Columns { get; set; }

        [Parameter]
        public RenderFragment<TData> ChildContent { get; set; }

        [Parameter]
        public RowSelection<TData> RowSelection { get; set; }
    }
}
