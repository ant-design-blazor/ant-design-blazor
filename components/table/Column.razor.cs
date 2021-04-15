using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Core.Reflection;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using System.Globalization;

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
        public TData Field
        {
            get
            {
                return GetValue != null ? GetValue(RowData) : _field;
            }
            set
            {
                if (GetValue == null)
                {
                    _field = value;
                }
            }
        }

        private TData _field;

        [Parameter]
        public string DataIndex { get; set; }

        [Parameter]
        public string Format { get; set; }

        [Parameter]
        public bool Sortable { get; set; }

        [Parameter]
        public Func<TData, TData, int> SorterCompare { get; set; }

        [Parameter]
        public int SorterMultiple { get; set; }

        [Parameter]
        public bool ShowSorterTooltip { get; set; } = true;

        [Parameter]
        public SortDirection[] SortDirections { get; set; }

        [Parameter]
        public SortDirection DefaultSortOrder { get; set; }

        [Parameter]
        public Func<RowData, Dictionary<string, object>> OnCell { get; set; }

        [Parameter]
        public Func<Dictionary<string, object>> OnHeaderCell { get; set; }

        [Parameter]
        public bool Filterable { get; set; }

        [Parameter]
        public IEnumerable<TableFilter<TData>> Filters { get; set; }

        [Parameter]
        public bool FilterMultiple { get; set; } = true;

        /// <summary>
        /// Function that determines if the row is displayed when filtered
        /// <para>
        /// Parameter 1: The value of the filter item
        /// </para>
        /// <para>
        /// Parameter 2: The value of the column
        /// </para>
        /// </summary>
        [Parameter]
        public Expression<Func<TData, TData, bool>> OnFilter { get; set; }

        private TableFilterType _columnFilterType;

        private PropertyReflector? _propertyReflector;

        private Type _columnDataType;

        public string DisplayName { get; private set; }

        public string FieldName { get; private set; }

        public ITableSortModel SortModel { get; private set; }

        public ITableFilterModel FilterModel { get; private set; }

        private SortDirection _sortDirection;

        public Func<RowData, TData> GetValue { get; private set; }

        public LambdaExpression GetFieldExpression { get; private set; }

        void IFieldColumn.ClearSorter() => SetSorter(SortDirection.None);

        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _filterOpened;

        private bool _hasFilterSelected;

        private string[] _selectedFilterValues;

        private ElementReference _filterTriggerRef;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Sortable = Sortable || SorterMultiple != default || SorterCompare != default || DefaultSortOrder != default || SortDirections?.Any() == true;

            if (IsHeader)
            {
                if (FieldExpression != null)
                {
                    _propertyReflector = PropertyReflector.Create(FieldExpression);
                    var paramExp = Expression.Parameter(ItemType);
                    var member = ColumnExpressionHelper.GetReturnMemberInfo(FieldExpression);
                    var bodyExp = Expression.MakeMemberAccess(paramExp, member);
                    GetFieldExpression = Expression.Lambda(bodyExp, paramExp);
                }
                else if (DataIndex != null)
                {
                    (_, GetFieldExpression) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
                }

                if (Sortable && GetFieldExpression != null)
                {
                    SortModel = new SortModel<TData>(GetFieldExpression, SorterMultiple, DefaultSortOrder, SorterCompare);
                }

                if (GetFieldExpression != null)
                {
                    var member = ColumnExpressionHelper.GetReturnMemberInfo(GetFieldExpression);
                    DisplayName = member.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName ?? member.Name;
                    FieldName = member.Name;
                }
            }
            else if (IsBody)
            {
                SortModel = Context.HeaderColumns[ColIndex] is IFieldColumn fieldColumn ? fieldColumn.SortModel : null;

                (GetValue, _) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
            }

            SortDirections ??= Table.SortDirections;

            Sortable = Sortable || SortModel != null;
            _sortDirection = SortModel?.SortDirection ?? DefaultSortOrder ?? SortDirection.None;
            _columnDataType = THelper.GetUnderlyingType<TData>();
            if (_columnDataType == typeof(bool) && Filters?.Any() != true)
            {
                Filters = new List<TableFilter<TData>>();

                var trueFilterOption = GetNewFilter();
                trueFilterOption.Text = Table.Locale.FilterOptions.True;
                trueFilterOption.Value = THelper.ChangeType<TData>(true); //(TData)Convert.ChangeType(true, typeof(TData));
                ((List<TableFilter<TData>>)Filters).Add(trueFilterOption);
                var falseFilterOption = GetNewFilter();
                falseFilterOption.Text = Table.Locale.FilterOptions.False;
                falseFilterOption.Value = THelper.ChangeType<TData>(false);
                ((List<TableFilter<TData>>)Filters).Add(falseFilterOption);
            }

            if (Filters?.Any() == true)
            {
                Filterable = true;
                _columnFilterType = TableFilterType.List;
            }
            else if (Filterable)
            {
                _columnFilterType = TableFilterType.FeildType;
                InitFilters();
            }

            ClassMapper
               .If("ant-table-column-has-sorters", () => Sortable)
               .If($"ant-table-column-sort", () => Sortable && SortModel != null && SortModel.SortDirection.IsIn(SortDirection.Ascending, SortDirection.Descending));
        }

        private string NumberFormatter(TData value)
        {
            if (value == null) return null;
            return Convert.ToDouble(value).ToString(Format);
        }

        private void HandleSort()
        {
            if (Sortable)
            {
                SetSorter(NextSortDirection());
                Table.ColumnSorterChange(this);
            }
        }

        private string SorterTooltip
        {
            get
            {
                var next = NextSortDirection();
                return next?.Value switch
                {
                    0 => Table.Locale.CancelSort,
                    1 => Table.Locale.TriggerAsc,
                    2 => Table.Locale.TriggerDesc,
                    _ => Table.Locale.CancelSort
                };
            }
        }

        private SortDirection NextSortDirection()
        {
            if (_sortDirection == SortDirection.None)
            {
                return SortDirections[0];
            }
            else
            {
                var index = Array.IndexOf(SortDirections, _sortDirection);
                if (index >= SortDirections.Length - 1)
                {
                    return SortDirection.None;
                }

                return SortDirections[index + 1];
            }
        }

        private void SetSorter(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
            SortModel.SetSortDirection(sortDirection);
        }

        private void ToggleTreeNode()
        {
            bool expandValueBeforeChange = RowData.Expanded;
            RowData.Expanded = !RowData.Expanded;
            Table?.OnExpandChange(RowData.CacheKey);
            if (RowData.Expanded != expandValueBeforeChange)
                Table?.Refresh();
        }

        private void SetFilterCompareOperator(TableFilter<TData> filter, TableFilterCompareOperator compareOperator)
        {
            filter.FilterCompareOperator = compareOperator;
            if (compareOperator == TableFilterCompareOperator.IsNull || compareOperator == TableFilterCompareOperator.IsNotNull) filter.Selected = true;
        }

        private void SetFilterCondition(TableFilter<TData> filter, TableFilterCondition filterCondition)
        {
            filter.FilterCondition = filterCondition;
        }

        private void SetFilterValue(TableFilter<TData> filter, TData value)
        {
            filter.Value = value;
            filter.Selected = true;
        }

        private void SetFilterValue(TableFilter<TData> filter, DateTime? value)
        {
            if (value == null)
            {
                filter.Value = default;
                filter.Selected = false;
            }
            else
            {
                filter.Value = (TData)Convert.ChangeType(value, typeof(DateTime), CultureInfo.InvariantCulture);
                filter.Selected = true;
            }
        }

        private DateTime? FilterDateTimeValue(TableFilter<TData> filter)
        {
            if (EqualityComparer<TData>.Default.Equals(filter.Value, default(TData)))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(filter.Value);
            }
        }

        private void FilterSelected(TableFilter<TData> filter)
        {
            if (_columnFilterType == TableFilterType.FeildType) return;
            if (!FilterMultiple)
            {
                Filters.ForEach(x => x.Selected = false);
                filter.Selected = true;
            }
            else
            {
                filter.Selected = !filter.Selected;
            }

            _selectedFilterValues = Filters.Where(x => x.Selected).Select(x => x.Value.ToString()).ToArray();
            StateHasChanged();
        }

        private void FilterConfirm(bool isReset = false)
        {
            _filterOpened = false;
            if (!isReset && _columnFilterType == TableFilterType.FeildType) Filters?.ForEach(f => { if (!f.Selected && f.Value != null) f.Selected = true; });
            _hasFilterSelected = Filters?.Any(x => x.Selected) == true;
            FilterModel = _hasFilterSelected && _propertyReflector != null ? new FilterModel<TData>(_propertyReflector.Value.PropertyInfo, OnFilter, Filters.Where(x => x.Selected).ToList(), _columnFilterType) : null;

            Table?.ReloadAndInvokeChange();
        }

        private void ResetFilters()
        {
            if (_columnFilterType == TableFilterType.List)
            {
                Filters.ForEach(x => x.Selected = false);
            }
            else
            {
                InitFilters();
            }
            FilterConfirm(true);
        }

        private void AddFilter()
        {
            ((List<TableFilter<TData>>)Filters).Add(GetNewFilter());
        }

        private void RemoveFilter(TableFilter<TData> filter)
        {
            ((List<TableFilter<TData>>)Filters).Remove(filter);
        }

        private TableFilter<TData> GetNewFilter()
        {
            return new TableFilter<TData>()
            {
                FilterCondition = TableFilterCondition.And,
                FilterCompareOperator = _columnDataType == typeof(string) ? TableFilterCompareOperator.Contains : TableFilterCompareOperator.Equals
            };
        }

        private void InitFilters()
        {
            Filters = new List<TableFilter<TData>>() { GetNewFilter() };
        }
    }
}
