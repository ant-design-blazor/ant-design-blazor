using System;
using System.Collections.Generic;
using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntBlazor
{
    public sealed partial class AntTable<TData> : IAntTable
    {
        [Parameter]
        public IList<TData> DataSource { get; set; }

        [Parameter]
        public RenderFragment<TData> ChildContent { get; set; }

        [Parameter]
        public RowSelection<TData> RowSelection { get; set; }
    }
}