using System;
using System.Linq;
using System.Linq.Expressions;
using AntDesign.TableModels;
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
        public RenderFragment<TData> CellRender { get; set; }

        [Parameter]
        public TData Field { get; set; }

        [Parameter]
        public string Format { get; set; }

        [Parameter]
        public bool Sortable { get; set; }

        [Parameter]
        public string Sort { get; set; }

        [Parameter]
        public bool ShowSorterTooltip { get; set; } = true;

        private FieldIdentifier? _fieldIdentifier;

        public string DisplayName => _fieldIdentifier?.GetDisplayName();

        public string FieldName => _fieldIdentifier?.FieldName;

        public ITableSortModel SortModel { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (FieldExpression != null)
            {
                _fieldIdentifier = FieldIdentifier.Create(FieldExpression);

                if (Sortable)
                {
                    SortModel = new SortModel<TData>(_fieldIdentifier.Value, 1, Sort);
                }
            }

            ClassMapper
                .If("ant-table-column-has-sorters", () => Sortable)
                .If($"ant-table-column-sort", () => Sortable && SortModel.SortType.IsIn(SortType.Ascending, SortType.Descending));
        }

        private void HandelHeaderClick()
        {
            if (Sortable)
            {
                SortModel.SwitchSortType();
                Table.ReloadAndInvokeChange();
            }
        }

        private string SorterTooltip
        {
            get
            {
                var next = SortModel.NextType();
                if (next == SortType.None)
                {
                    return "取消排序";
                }
                else if (next == SortType.Ascending)
                {
                    return "点击升序";
                }
                else
                {
                    return "点击降序";
                }
            }
        }
    }
}
