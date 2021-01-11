using System;
using System.Linq.Expressions;
using AntDesign.Core.Reflection;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

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
        public Func<TData, TData, int> Comparer { get; set; }

        [Parameter]
        public bool ShowSorterTooltip { get; set; } = true;

        private PropertyReflector? _propertyReflector;

        public string DisplayName => _propertyReflector?.DisplayName;

        public string FieldName => _propertyReflector?.PropertyName;

        public ITableSortModel SortModel { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (FieldExpression != null)
            {
                _propertyReflector = PropertyReflector.Create(FieldExpression);
                if (Sortable)
                {
                    SortModel = new SortModel<TData>(_propertyReflector.Value.PropertyInfo, 1, Sort, Comparer);
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
                    return Table.Locale.CancelSort;
                }
                else if (next == SortType.Ascending)
                {
                    return Table.Locale.TriggerAsc;
                }
                else
                {
                    return Table.Locale.TriggerDesc;
                }
            }
        }

        private void ToggleTreeNode()
        {
            RowData.Expanded = !RowData.Expanded;
            Table?.Refresh();
        }
    }
}
