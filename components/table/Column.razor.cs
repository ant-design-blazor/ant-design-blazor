using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Column<TData> : ColumnBase, IFieldColumn
    {
        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        public bool Blocked { get; set; }

        [CascadingParameter(Name = "ItemType")]
        public Type ItemType { get; set; }

        [Parameter]
        public EventCallback<TData> FieldChanged { get; set; }

        [Parameter]
        public Expression<Func<TData>> FieldExpression { get; set; }


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

        public override string Title { get => base.Title ?? DisplayName ?? FieldName; set => base.Title = value; }

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
        public Func<CellData, Dictionary<string, object>> OnCell { get; set; }

        [Parameter]
        public Func<Dictionary<string, object>> OnHeaderCell { get; set; }

        private bool _filterable;

        private bool _hasFilterableAttribute;

        [Parameter]
        public bool Filterable
        {
            get => _filterable;
            set
            {
                _filterable = value;
                _hasFilterableAttribute = true;
            }
        }

        private IEnumerable<TableFilter> _filters;

        private bool _hasFiltersAttribute;

        [Parameter]
        public IEnumerable<TableFilter<TData>> Filters
        {
            get => _filters as IEnumerable<TableFilter<TData>>;
            set
            {
                _filters = value;
                _hasFiltersAttribute = true;
            }
        }

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

        [Parameter]
        public virtual RenderFragment<CellData<TData>> CellRender { get; set; }

        private TableFilterType _columnFilterType;

        private Type _columnDataType;

        public string DisplayName { get; private set; }

        public string FieldName { get; private set; }

        public ITableSortModel SortModel { get; private set; }

        public ITableFilterModel FilterModel { get; private set; }

        private SortDirection _sortDirection;

        public Func<RowData, TData> GetValue { get; private set; }

        public LambdaExpression GetFieldExpression { get; private set; }

        void IFieldColumn.ClearSorter()
        {
            SetSorter(SortDirection.None);
            if (FieldExpression == null)
            {
                StateHasChanged();
            }
        }

        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _filterOpened;

        private bool _hasFilterSelected;

        private string[] _selectedFilterValues;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Sortable = Sortable || SorterMultiple != default || SorterCompare != default || DefaultSortOrder != default || SortDirections?.Any() == true;

            if (IsHeader)
            {
                if (FieldExpression != null)
                {
                    if (FieldExpression.Body is not MemberExpression memberExp)
                    {
                        throw new ArgumentException("'Field' parameter must be child member");
                    }

                    var paramExp = Expression.Parameter(ItemType);
                    var bodyExp = Expression.MakeMemberAccess(paramExp, memberExp.Member);
                    GetFieldExpression = Expression.Lambda(bodyExp, paramExp);
                }
                else if (DataIndex != null)
                {
                    (_, GetFieldExpression) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
                }

                if (GetFieldExpression != null)
                {
                    var member = ColumnExpressionHelper.GetReturnMemberInfo(GetFieldExpression);
                    DisplayName = member?.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName
                               ?? member?.GetCustomAttribute<DisplayAttribute>(true)?.GetName()
                               ?? member?.Name;
                    FieldName = DataIndex ?? member?.Name;
                }

                if (Sortable && GetFieldExpression != null)
                {
                    SortModel = new SortModel<TData>(GetFieldExpression, FieldName, SorterMultiple, DefaultSortOrder, SorterCompare);
                }
            }
            else if (IsBody)
            {
                SortModel = Context.HeaderColumns[ColIndex] is IFieldColumn fieldColumn ? fieldColumn.SortModel : null;

                if (DataIndex != null)
                {
                    (GetValue, _) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
                }
            }

            SortDirections ??= Table.SortDirections;

            Sortable = Sortable || SortModel != null;
            _sortDirection = SortModel?.SortDirection ?? DefaultSortOrder ?? SortDirection.None;

            if (IsHeader)
            {
                if (_hasFiltersAttribute)
                {
                    if (!_hasFilterableAttribute) Filterable = true;
                    _columnFilterType = TableFilterType.List;
                }
                else if (_hasFilterableAttribute)
                {
                    _columnDataType = THelper.GetUnderlyingType<TData>();
                    if (_columnDataType == typeof(bool))
                    {
                        _columnFilterType = TableFilterType.List;

                        _filters = new List<TableFilter>();

                        var trueFilterOption = GetNewFilter();
                        trueFilterOption.Text = Table.Locale.FilterOptions.True;
                        trueFilterOption.Value = true;
                        ((List<TableFilter>)_filters).Add(trueFilterOption);
                        var falseFilterOption = GetNewFilter();
                        falseFilterOption.Text = Table.Locale.FilterOptions.False;
                        falseFilterOption.Value = false;
                        ((List<TableFilter>)_filters).Add(falseFilterOption);
                    }
                    else if (_columnDataType.IsEnum && _columnDataType.GetCustomAttribute<FlagsAttribute>() == null)
                    {
                        _columnFilterType = TableFilterType.List;

                        _filters = new List<TableFilter>();

                        foreach (var enumValue in Enum.GetValues(_columnDataType))
                        {
                            var enumName = Enum.GetName(_columnDataType, enumValue);
                            var filterOption = GetNewFilter();
                            // use DisplayAttribute only, DisplayNameAttribute is not valid for enum values
                            filterOption.Text = _columnDataType.GetMember(enumName)[0].GetCustomAttribute<DisplayAttribute>()?.Name ?? enumName;
                            filterOption.Value = enumValue;
                            ((List<TableFilter>)_filters).Add(filterOption);
                        }
                    }
                    else
                    {
                        _columnFilterType = TableFilterType.FieldType;
                        InitFilters();
                    }

                    if (_columnFilterType == TableFilterType.List && THelper.IsTypeNullable<TData>())
                    {
                        var nullFilterOption = GetNewFilter();
                        nullFilterOption.Text = Table.Locale.FilterOptions.IsNull;
                        nullFilterOption.Value = null;
                        ((List<TableFilter>)_filters).Add(nullFilterOption);
                    }
                }

                Context.HeaderColumnInitialed(this);
            }

            ClassMapper
               .If("ant-table-column-has-sorters", () => Sortable)
               .If($"ant-table-column-sort", () => Sortable && SortModel != null && SortModel.SortDirection.IsIn(SortDirection.Ascending, SortDirection.Descending));
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (IsHeader)
            {
                FilterModel = _filterable && _filters?.Any(x => x.Selected) == true ?
                    new FilterModel<TData>(GetFieldExpression, FieldName, OnFilter, _filters.Where(x => x.Selected).ToList(), _columnFilterType) :
                    null;
            }
        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
        }

        private string NumberFormatter(object value)
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
            SortModel?.SetSortDirection(sortDirection);
        }

        private void SetFilterCompareOperator(TableFilter filter, TableFilterCompareOperator compareOperator)
        {
            filter.FilterCompareOperator = compareOperator;
        }

        private void SetFilterCondition(TableFilter filter, TableFilterCondition filterCondition)
        {
            filter.FilterCondition = filterCondition;
        }

        private void SetFilterValue(TableFilter filter, object value)
        {
            filter.Value = value;
        }

        private void FilterSelected(TableFilter filter)
        {
            if (_columnFilterType == TableFilterType.FieldType) return;
            if (!FilterMultiple)
            {
                _filters.ForEach(x => x.Selected = false);
                filter.Selected = true;
            }
            else
            {
                filter.Selected = !filter.Selected;
            }

            _selectedFilterValues = _filters.Where(x => x.Selected).Select(x => x.Value?.ToString()).ToArray();
            StateHasChanged();
        }

        private void FilterConfirm(bool isReset = false)
        {
            _filterOpened = false;
            if (!isReset && _columnFilterType == TableFilterType.FieldType)
            {
                _filters?.ForEach(f =>
                {
                    f.Selected =
                        f.Value != null ||
                        f.FilterCompareOperator == TableFilterCompareOperator.IsNotNull ||
                        f.FilterCompareOperator == TableFilterCompareOperator.IsNull;
                });
            }
            _hasFilterSelected = _filters?.Any(x => x.Selected) == true;
            FilterModel = _hasFilterSelected ? new FilterModel<TData>(GetFieldExpression, FieldName, OnFilter, _filters.Where(x => x.Selected).ToList(), _columnFilterType) : null;

            Table?.ReloadAndInvokeChange();
        }

        private void ResetFilters()
        {
            if (_columnFilterType == TableFilterType.List)
            {
                _filters.ForEach(x => x.Selected = false);
            }
            else
            {
                InitFilters();
            }
            FilterConfirm(true);
        }

        private void AddFilter()
        {
            ((List<TableFilter>)_filters).Add(GetNewFilter());
        }

        private void RemoveFilter(TableFilter filter)
        {
            ((List<TableFilter>)_filters).Remove(filter);
        }

        private TableFilter GetNewFilter()
        {
            if (_columnFilterType == TableFilterType.FieldType)
            {
                return new TableFilter()
                {
                    FilterCondition = TableFilterCondition.And,
                    FilterCompareOperator = _columnDataType == typeof(string) ? TableFilterCompareOperator.Contains : TableFilterCompareOperator.Equals
                };
            }
            else
            {
                return new TableFilter()
                {
                    FilterCondition = TableFilterCondition.Or,
                    FilterCompareOperator = TableFilterCompareOperator.Equals
                };
            }
        }

        private void InitFilters()
        {
            _filters = new List<TableFilter>() { GetNewFilter() };
        }
    }
}
