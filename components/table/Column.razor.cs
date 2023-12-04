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
using System.Text.Json;
using AntDesign.Core.Helpers;
using AntDesign.Filters;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using Microsoft.JSInterop;

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
        public RenderFragment FilterDropdown { get; set; }

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

        [Parameter]
        public bool Grouping { get; set; }

        [Parameter]
        public virtual Func<TData, object> GroupBy { get; set; }

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
        public IEnumerable<TableFilter> DefaultFilters { get; set; }

        [Parameter]
        public bool FilterMultiple { get; set; } = true;

        [Parameter]
        public IFieldFilterType FieldFilterType { get; set; }

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
        private IFieldFilterType _fieldFilterType;

        private Type _columnDataType;

        public string DisplayName { get; private set; }

        public string FieldName { get; private set; }

        public ITableSortModel SortModel { get; private set; }

        public ITableFilterModel FilterModel { get; private set; }

        private SortDirection _sortDirection;

        protected Func<RowData, TData> GetValue { get; set; }

        protected LambdaExpression GetFieldExpression { get; set; }

        void IFieldColumn.ClearSorter()
        {
            SetSorter(SortDirection.None);
            if (FieldExpression == null)
            {
                StateHasChanged();
            }
        }

        private static readonly EventCallbackFactory _callbackFactory = new();

        private bool _filterOpened;

        private bool _hasFilterSelected;

        private string[] _selectedFilterValues;

        private RenderFragment _renderDefaultFilterDropdown;

        private bool IsFiexedEllipsis => Ellipsis && Fixed is "left" or "right";

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
                    SortModel = new SortModel<TData>(this, GetFieldExpression, FieldName, SorterMultiple, DefaultSortOrder, SorterCompare);
                }

                if (Grouping)
                {
                    Table.AddGroupColumn(this);
                }
                else
                {
                    Table.RemoveGroupColumn(this);
                }
            }
            else if (IsBody)
            {
                if (!Table.HasRowTemplate)
                {
                    SortModel = (Context.HeaderColumns.LastOrDefault(x => x.ColIndex == ColIndex) as IFieldColumn)?.SortModel;
                }

                if (DataIndex != null)
                {
                    (GetValue, _) = ColumnDataIndexHelper<TData>.GetDataIndexConfig(this);
                }

                if (RowData.IsGrouping)
                {
                    ColSpan = ColIndex == Table.TreeExpandIconColumnIndex ? Context.Columns.Count + 1 : 0;
                }
            }

            SortDirections ??= Table.SortDirections;

            Sortable = Sortable || SortModel != null;
            _sortDirection = SortModel?.SortDirection ?? DefaultSortOrder ?? SortDirection.None;

            _filterable = _filterable || FilterDropdown != null;

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
                    _columnFilterType = TableFilterType.FieldType;

                    if (FieldFilterType is null)
                    {
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

                            _filters = EnumHelper<TData>.GetValueLabelList().Select(item =>
                            {
                                var filterOption = GetNewFilter();
                                filterOption.Text = item.Label;
                                filterOption.Value = item.Value;
                                return filterOption;
                            }).ToList();
                        }
                    }

                    if (_columnFilterType == TableFilterType.List && THelper.IsTypeNullable<TData>())
                    {
                        var nullFilterOption = GetNewFilter();
                        nullFilterOption.Text = Table.Locale.FilterOptions.Operator(TableFilterCompareOperator.IsNull);
                        nullFilterOption.Value = null;
                        ((List<TableFilter>)_filters).Add(nullFilterOption);
                    }
                }

                Context.HeaderColumnInitialed(this);

                _renderDefaultFilterDropdown = RenderDefaultFilterDropdown;
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
                if (_columnFilterType == TableFilterType.FieldType && _fieldFilterType == null && Filterable)
                {
                    _fieldFilterType = FieldFilterType ?? Table.FieldFilterTypeResolver.Resolve<TData>();
                    if (DefaultFilters is null)
                        ResetFieldFilters();
                    else
                    {
                        _filters = DefaultFilters;
                        _hasFilterSelected = DefaultFilters.Any(f => f.Selected);
                    }
                }

                FilterModel = _filterable && _filters?.Any(x => x.Selected) == true ?
                    new FilterModel<TData>(this, GetFieldExpression, FieldName, OnFilter, _filters.Where(x => x.Selected).ToList(), _columnFilterType, _fieldFilterType) :
                    null;
            }
        }

        protected override bool ShouldRender()
        {
            if (Blocked) return false;
            return true;
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

        IQueryable<IGrouping<object, TItem>> IFieldColumn.Group<TItem>(IQueryable<TItem> source)
        {
            var param = Expression.Parameter(typeof(TItem), "item");

            Expression field = Expression.Invoke(GetFieldExpression, param);

            if (GroupBy != null)
            {
                var instance = Expression.Constant(GroupBy.Target);
                field = Expression.Call(instance, GroupBy.Method, field);
            }

            var body = Expression.Convert(field, typeof(object));
            var lambda = Expression.Lambda<Func<TItem, object>>(body, param);

            return source.GroupBy(lambda);
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
            FilterModel = _hasFilterSelected
                ? new FilterModel<TData>(this, GetFieldExpression, FieldName, OnFilter,
                    _filters.Where(x => x.Selected).ToList(), _columnFilterType, _fieldFilterType)
                : null;

            Table?.ColumnFilterChange();
        }

        private void ResetFilters()
        {
            if (_columnFilterType == TableFilterType.List)
            {
                _filters.ForEach(x => x.Selected = false);
            }
            else
            {
                ResetFieldFilters();
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
                    FilterCompareOperator = _fieldFilterType.DefaultCompareOperator
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

        private void ResetFieldFilters()
        {
            _filters = new List<TableFilter>() { GetNewFilter() };
        }

        void IFieldColumn.ClearFilters() => ResetFilters();

        void IFieldColumn.SetFilterModel(ITableFilterModel filterModel)
        {
            foreach (var filter in filterModel.Filters)
            {
                if (filter.Value is JsonElement jsonElement)
                {
                    filter.Value = JsonElementHelper<TData>.GetValue(jsonElement);
                }
            }

            if (_columnFilterType == TableFilterType.List)
            {
                foreach (var filter in _filters.Where(f => filterModel.Filters.Any(x => x.Value.Equals(f.Value))))
                {
                    filter.Selected = true;
                }
            }
            else
            {
                _filters = filterModel.Filters;
            }

            FilterModel = new FilterModel<TData>(this, GetFieldExpression, FieldName, OnFilter,
                _filters.Where(x => x.Selected).ToList(), _columnFilterType, _fieldFilterType);

            _hasFilterSelected = true;
        }

        void IFieldColumn.SetSortModel(ITableSortModel sortModel)
        {
            SortModel = new SortModel<TData>(this, GetFieldExpression, FieldName, SorterMultiple, SortDirection.Parse(sortModel.Sort), SorterCompare);
            this.SetSorter(SortDirection.Parse(sortModel.Sort));
        }

        protected object _filterInputRef;

        private void FilterDropdownOnVisibleChange(bool visible)
        {
#if NET5_0_OR_GREATER
            if (!visible ||
                _filterInputRef is not AntDomComponentBase baseDomComponent ||
                baseDomComponent.GetType().GetGenericTypeDefinition() != typeof(Input<>) ||
                baseDomComponent.Ref.Context == null) return;

            _ = Task.Run(async () =>
            {
                var filterInputFocused = false;
                var attemptCount = 0;

                do
                {
                    await Task.Delay(50);
                    try
                    {
                        await Js.FocusAsync(baseDomComponent.Ref, FocusBehavior.FocusAtLast);
                        await Task.Delay(50);
                        filterInputFocused = await JsInvokeAsync<string>(JSInteropConstants.GetActiveElement) == baseDomComponent.Ref.Id;
                    }
                    catch (JSException jsex)
                    {
                        Console.WriteLine(jsex.ToString());
                    }
                    catch
                    {
                        throw;
                    }

                } while (!filterInputFocused || attemptCount++ >= 3);
            });
#endif
        }
    }
}
