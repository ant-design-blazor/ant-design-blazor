using System;
using System.Linq.Expressions;
using AntDesign.Core.Reflection;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Column<TData> : ColumnBase, IFieldColumn
    {
        [CascadingParameter(Name = "ItemType")]
        public Type ItemType { get; set; }

        [Parameter]
        public EventCallback<TData> FieldChanged { get; set; }

        [Parameter]
        public Expression<Func<TData>> FieldExpression { get; set; }

        [Parameter]
        public RenderFragment<TData> CellRender { get; set; }

        [Parameter]
        public TData Field { get; set; }

        [Parameter]
        public string DataIndex { get; set; }

        [Parameter]
        public string Format { get; set; }

        [Parameter]
        public bool Sortable { get; set; }

        [Parameter]
        public string Sort { get; set; }

        [Parameter]
        public Func<TData, TData, int> SorterCompare { get; set; }

        [Parameter]
        public int SorterMultiple { get; set; }

        [Parameter]
        public bool ShowSorterTooltip { get; set; } = true;

        private PropertyReflector? _propertyReflector;

        public string DisplayName => _propertyReflector?.DisplayName;

        public string FieldName => _propertyReflector?.PropertyName;

        public ITableSortModel SortModel { get; private set; }

        public Func<RowData, TData> GetValue { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!Sortable)
            {
                Sortable = SorterMultiple != default || SorterCompare != default || Sort != default;
            }

            if (IsHeader)
            {
                if (FieldExpression != null)
                {
                    _propertyReflector = PropertyReflector.Create(FieldExpression);
                    if (Sortable)
                    {
                        SortModel = new SortModel<TData>(_propertyReflector.Value.PropertyInfo, SorterMultiple, Sort, SorterCompare);
                    }
                }
                else
                {
                    (GetValue, SortModel) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
                }
            }
            else if (IsBody)
            {
                SortModel = Context.HeaderColumns[ColIndex] is IFieldColumn fieldColumn ? fieldColumn.SortModel : null;
                (GetValue, _) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
            }

            ClassMapper
               .If("ant-table-column-has-sorters", () => Sortable)
               .If($"ant-table-column-sort", () => Sortable && SortModel != null && SortModel.SortType.IsIn(SortType.Ascending, SortType.Descending));
        }

        private void HandelHeaderClick()
        {
            if (Sortable)
            {
                var currenttype = SortModel.SortType;
                Table.SwithSortModelBySortWay();
                SortModel.SetSortType(currenttype);

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
