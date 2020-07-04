using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public partial class Column<TData> : ColumnBase, IFieldColumn
    {
        [Parameter]
        public EventCallback<TData> FieldChanged { get; set; }

        [Parameter]
        public Expression<Func<TData>> FieldExpression { get; set; }

        [Parameter]
        public bool Sort { get; set; }

        [Parameter]
        public RenderFragment<TData> CellRender { get; set; }

        [Parameter]
        public TData Field { get; set; }

        [Parameter]
        public string Format { get; set; }

        private FieldIdentifier? _fieldIdentifier;

        public string DisplayName => _fieldIdentifier?.GetDisplayName();

        public string FieldName => _fieldIdentifier?.FieldName;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (FieldExpression != null)
            {
                _fieldIdentifier = FieldIdentifier.Create(FieldExpression);
            }
        }
    }
}
