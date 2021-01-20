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

        private static ConcurrentDictionary<ColumnCacheKey, ColumnCacheItem> _dataIndexCache = new();

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
                var cacheKey = ColumnCacheKey.Create(this);
                (GetValue, _propertyReflector, SortModel) = _dataIndexCache.GetOrAdd(cacheKey, CreateDataIndex);
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

        private struct ColumnCacheKey
        {
            public Type ItemType;

            public Type PropType;

            public string DataIndex;

            public bool Sortable;

            public string Sort;

            public Func<TData, TData, int> SorterCompare;

            internal static ColumnCacheKey Create(Column<TData> column)
            {
                return new(column.ItemType, typeof(TData), column.DataIndex, column.Sortable, column.Sort, column.SorterCompare);
            }

            public ColumnCacheKey(Type itemType, Type propType, string dataIndex, bool sortable, string sort, Func<TData, TData, int> sorterCompare)
            {
                ItemType = itemType;
                PropType = propType;
                DataIndex = dataIndex;
                Sortable = sortable;
                Sort = sort;
                SorterCompare = sorterCompare;
            }

            public void Deconstruct(out Type itemType, out Type propType, out string dataIndex, out bool sortable, out string sort, out Func<TData, TData, int> sorterCompare)
            {
                itemType = ItemType;
                propType = PropType;
                dataIndex = DataIndex;
                sortable = Sortable;
                sort = Sort;
                sorterCompare = SorterCompare;
            }
        }

        private struct ColumnCacheItem
        {
            public Func<RowData, TData> GetValue;

            public PropertyReflector? PropertyReflector;

            public ITableSortModel SortModel;

            public void Deconstruct(out Func<RowData, TData> getValue, out PropertyReflector? propertyReflector, out ITableSortModel sortModel)
            {
                getValue = GetValue;
                propertyReflector = PropertyReflector;
                sortModel = SortModel;
            }
        }

        private static ColumnCacheItem CreateDataIndex(ColumnCacheKey key)
        {
            var (itemType, propType, dataIndex, sortable, sort, sorterCompare) = key;
            var item = new ColumnCacheItem();
            var properties = dataIndex?.Split(".");
            if (properties is {Length: >0})
            {
                var isNullable = propType.IsValueType && Nullable.GetUnderlyingType(propType) != null;
                var rowDataType = typeof(RowData);
                var rowData1Type = typeof(RowData<>).MakeGenericType(itemType);
                var rowDataExp = Expression.Parameter(rowDataType);
                var rowData1Exp = Expression.TypeAs(rowDataExp, rowData1Type);
                var dataMemberExp = Expression.Property(rowData1Exp, nameof(RowData<object>.Data));

                MemberExpression member;

                if (isNullable) // TData is Nullable<T> 
                {
                    var nullableMemberExp = PropertyAccessHelper.AccessNullableProperty(dataMemberExp, properties);
                    member = nullableMemberExp.IfFalse switch
                    {
                        UnaryExpression ue => (MemberExpression)ue.Operand,
                        MemberExpression me => me,
                        _ => throw new ArgumentException($"unexpected type: {nullableMemberExp.IfFalse.GetType().Name}")
                    };
                }
                else
                {
                    member = PropertyAccessHelper.AccessProperty(dataMemberExp, properties);
                }

                item.GetValue = Expression.Lambda<Func<RowData, TData>>(member, rowDataExp).Compile();
                var propertyInfo = (PropertyInfo)member.Member;
                item.PropertyReflector = new PropertyReflector(propertyInfo);

                if (sortable)
                {
                    var propertySelector = isNullable
                                               ? PropertyAccessHelper.BuildNullablePropertyAccessExpression(itemType, properties)
                                               : PropertyAccessHelper.BuildPropertyAccessExpression(itemType, properties);
                    item.SortModel = new DataIndexSortModel<TData>(propertyInfo, propertySelector, 1, sort, sorterCompare);
                }
            }

            return item;
        }
    }
}
