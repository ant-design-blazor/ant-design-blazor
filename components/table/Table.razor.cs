using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : AntDomComponentBase, ITable
    {
        private static readonly TItem _fieldModel = (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem));

        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        private readonly IList<ITableColumn> _columns = new List<ITableColumn>();

        IRowSelection ITable.HeaderSelection { get; set; }

        public void AddColumn(ITableColumn column)
        {
            _columns.Add(column);
            StateHasChanged();
        }
    }
}
