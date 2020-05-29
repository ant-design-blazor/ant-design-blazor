using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class ColumnType
    {
        public string Title { get; set; }
        public string DataIndex { get; set; }
        public string Key { get; set; }
    }

    public class ColumnContext<TColumn, TRecord>
    {
        public TColumn Value { get; set; }

        public TRecord Record { get; set; }
    }

    public class ColumnType<TColumn> : ColumnType
    {
        public RenderFragment<ColumnContext<TColumn, object>> Render { get; set; }
    }

    public class ColumnType<TColumn, TRecord> : ColumnType
    {
        public RenderFragment<ColumnContext<TColumn, TRecord>> Render { get; set; }
    }
}
