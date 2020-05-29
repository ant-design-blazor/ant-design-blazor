using System;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public partial class Column<TItem> : AntComponentBase, ITableColumn
    {
        [Parameter]
        public TItem Field { get; set; }

        [Parameter]
        public EventCallback<TItem> FieldChanged { get; set; }

        [Parameter]
        public Expression<Func<TItem>> FieldExpression { get; set; }

        [Parameter]
        public bool Sort { get; set; }

        [Parameter]
        public RenderFragment<TItem> CellTemplate { get; set; }

        [CascadingParameter]
        public ITable Table { get; set; }

        [CascadingParameter(Name = "IsHeader")]
        public bool IsHeader { get; set; }

        private FieldIdentifier? _fieldIdentifier;

        public string DisplayName => _fieldIdentifier?.GetDisplayName();

        public string FieldName => _fieldIdentifier?.FieldName;

        protected override void OnInitialized()
        {
            if (FieldExpression != null)
            {
                _fieldIdentifier = FieldIdentifier.Create(FieldExpression);
            }

            Table?.AddColumn(this);
        }
    }
}
