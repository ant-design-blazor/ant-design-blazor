using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem> : AntDomComponentBase, ITable
    {
        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter]
        public RowSelection<TItem> RowSelection { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        private readonly IList<ITableColumn> _columns = new List<ITableColumn>();

        public void AddColumn(ITableColumn column)
        {
            _columns.Add(column);
            StateHasChanged();
        }

        private static readonly TItem _fieldModel = (TItem)RuntimeHelpers.GetUninitializedObject(typeof(TItem));
    }
}
