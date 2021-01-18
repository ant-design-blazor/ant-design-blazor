using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.core.Helpers;
using AntDesign.Core.Reflection;
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
        public bool ShowSorterTooltip { get; set; } = true;

        private PropertyReflector? _propertyReflector;

        public string DisplayName => _propertyReflector?.DisplayName;

        public string FieldName => _propertyReflector?.PropertyName;

        public ITableSortModel SortModel { get; private set; }

        public Func<RowData, TData> GetValue { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (FieldExpression != null)
            {
                _propertyReflector = PropertyReflector.Create(FieldExpression);
                if (Sortable)
                {
                    SortModel = new SortModel<TData>(_propertyReflector.Value.PropertyInfo, 1, Sort, SorterCompare);
                }
            }
            else
            {
                var properties = DataIndex?.Split(".");
                if (properties is {Length: >0})
                {
                    var isNullable = typeof(TData).IsValueType && Nullable.GetUnderlyingType(typeof(TData)) != null;
                    var rowDataType = typeof(RowData);
                    var rowData1Type = typeof(RowData<>).MakeGenericType(ItemType);
                    var rowDataExp = Expression.Parameter(rowDataType);
                    var rowData1Exp = Expression.TypeAs(rowDataExp, rowData1Type);
                    var dataMemberExp = Expression.Property(rowData1Exp, nameof(RowData<object>.Data));

                    MemberExpression member;

                    if (isNullable) // TData is Nullable<T> 
                    {
                        var memberExp = PropertyAccessHelper.AccessNullableProperty(dataMemberExp, properties);
                        member = memberExp.IfFalse switch
                        {
                            UnaryExpression ue => ((MemberExpression)ue.Operand),
                            MemberExpression me => me,
                            _ => throw new ArgumentException($"unexpected type: {memberExp.IfFalse.GetType().Name}")
                        };
                    }
                    else
                    {
                        member = PropertyAccessHelper.AccessProperty(dataMemberExp, properties);
                    }

                    GetValue = Expression.Lambda<Func<RowData, TData>>(member, rowDataExp).Compile();
                    var propertyInfo = (PropertyInfo)member.Member;
                    _propertyReflector = new PropertyReflector(propertyInfo);

                    //propertyInfo = (PropertyInfo)((MemberExpression)propertySelector.Body).Member;
                    if (Sortable)
                    {
                        var propertySelector = isNullable
                                                   ? PropertyAccessHelper.BuildNullablePropertyAccessExpression(ItemType, properties)
                                                   : PropertyAccessHelper.BuildPropertyAccessExpression(ItemType, properties);
                        SortModel = new DataIndexSortModel<TData>(propertyInfo, propertySelector, 1, Sort, SorterCompare);
                    }
                }
            }

            ClassMapper
               .If("ant-table-column-has-sorters", () => Sortable)
               .If($"ant-table-column-sort", () => Sortable && SortModel != null && SortModel.SortType.IsIn(SortType.Ascending, SortType.Descending));
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
