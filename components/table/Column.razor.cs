// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using AntDesign.Core.Helpers;
using AntDesign.Filters;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// The column definition, can be used to define a column for a <see cref="Table{TItem}"/>.
    /// <para>
    /// We recommend using <see cref="PropertyColumn{TItem, TProp}"/> instead.
    /// </para>
    /// </summary>
    /// <typeparam name="TData">
    /// The type of a property of the TItem objec. 
    /// </typeparam>
    public partial class Column<TData> : ColumnBase, IFieldColumn
    {
        [CascadingParameter(Name = "AntDesign.Column.Blocked")]
        internal bool Blocked { get; set; }

        [CascadingParameter(Name = "ItemType")]
        internal Type ItemType { get; set; }

        /// <summary>
        /// Expression to get the data for the field
        /// </summary>
        [Parameter]
        [Obsolete]
        public Expression<Func<TData>> FieldExpression { get; set; }

        /// <summary>
        /// Field this column represents
        /// </summary>
        [Parameter]
        public RenderFragment FilterDropdown { get; set; }

        /// <summary>
        /// Use @bind-Field to bind to a property of TItem, we recommend using <see cref="PropertyColumn{TItem, TProp}"/> instead
        /// </summary>
        [Parameter]
        [Obsolete]
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

        /// <summary>
        /// Only used for @bind-Field and get the expression, no other purpose
        /// </summary>
        [Parameter]
        [Obsolete]
        public EventCallback<TData> FieldChanged { get; set; }

        private TData _field;

        /// <summary>
        /// Title of the column. Uses the following order of priority: <see cref="ColumnBase.Title"/>, <see cref="DisplayName"/>, then <see cref="FieldName"/>
        /// </summary>
        public override string Title
        {
            get => base.Title ?? DisplayName ?? FieldName;
            set => base.Title = value;
        }

        /// <summary>
        /// The corresponding path of the column data in the data item, support for querying the nested path through the array
        /// </summary>
        [Parameter]
        public string DataIndex { get; set; }

        /// <summary>
        /// Column data serialization rules, such as DateTime.ToString("XXX")
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// Whether to allow sorting or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Sortable { get; set; }

        /// <summary>
        /// Comparison function for custom sort
        /// </summary>
        [Parameter]
        public Func<TData, TData, int> SorterCompare { get; set; }

        /// <summary>
        /// Number of similtaneous sorts allowed
        /// </summary>
        [Parameter]
        public int SorterMultiple { get; set; }

        /// <summary>
        /// Whether to show tooltip when hovering over sort button or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool ShowSorterTooltip { get; set; } = true;

        /// <summary>
        /// Allowable sort directions
        /// </summary>
        [Parameter]
        public SortDirection[] SortDirections { get; set; }

        /// <summary>
        /// Default sort direction
        /// </summary>
        [Parameter]
        public SortDirection? DefaultSortOrder { get; set; }

        /// <summary>
        /// Set cell attributes
        /// </summary>
        [Parameter]
        public Func<CellData, Dictionary<string, object>> OnCell { get; set; }

        /// <summary>
        /// Set header cell attributes
        /// </summary>
        [Parameter]
        public Func<Dictionary<string, object>> OnHeaderCell { get; set; }

        private bool _filterable;

        private bool _hasFilterableAttribute;

        /// <summary>
        /// Whether the column is filterable or not
        /// </summary>
        /// <default value="false"/>
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

        /// <summary>
        /// Whether the column is used for grouping or not
        /// </summary>
        [Parameter]
        public bool Grouping { get; set; }

        /// <summary>
        /// Specifies the grouping function for the column
        /// </summary>
        [Parameter]
        public virtual Func<TData, object> GroupBy { get; set; }

        private IEnumerable<TableFilter> _filters;

        private bool _hasFiltersAttribute;

        /// <summary>
        /// Filter options for the column
        /// </summary>
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

        /// <summary>
        /// Whether to allow multiple filters or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public IEnumerable<TableFilter> DefaultFilters { get; set; }

        /// <summary>
        /// Whether to allow multiple filters or not
        /// </summary>
        [Parameter]
        public bool FilterMultiple { get; set; } = true;

        /// <summary>
        /// Filter type for the column
        /// </summary>
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

        /// <summary>
        /// Whether the dataSource is filtered. Filter icon will be actived when it is true.
        /// </summary>
        [Parameter]
        public bool Filtered { get; set; }

        /// <summary>
        /// Set the column content to be displayed in the table
        /// </summary>
        [Parameter]
        public virtual RenderFragment<CellData<TData>> CellRender { get; set; }

        private TableFilterType _columnFilterType;
        private IFieldFilterType _fieldFilterType;

        private Type _columnDataType;

        /// <summary>
        /// Display name for the column
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Field name for the column
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Sort model of the column
        /// </summary>
        public ITableSortModel SortModel { get; private set; }

        /// <summary>
        /// Filter model of the column
        /// </summary>
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

        private bool IsFixedEllipsis => ShouldShowEllipsis && Fixed is ColumnFixPlacement.Left or ColumnFixPlacement.Right;

        private bool IsFiltered => _hasFilterSelected || Filtered;

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
                    SortModel = new SortModel<TData>(this, GetFieldExpression, FieldName, SorterMultiple, DefaultSortOrder ?? SortDirection.None, SorterCompare);
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

                Context.HeaderColumnInitialized(this);

                _renderDefaultFilterDropdown = RenderDefaultFilterDropdown;
            }
            // When the column type is object, the filter type is recognized by the type of the value
            else if (IsBody && _hasFilterableAttribute && _fieldFilterType is null && Field is not null)
            {
                var headerColumn = Context.HeaderColumns[ColIndex];
                if (headerColumn is IFieldColumn fieldColumn)
                {
                    var columnDataType = Field.GetType();
                    fieldColumn.SetHeaderFilter(columnDataType);
                }
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

                return next switch
                {
                    SortDirection.None => Table.Locale.CancelSort,
                    SortDirection.Ascending => Table.Locale.TriggerAsc,
                    SortDirection.Descending => Table.Locale.TriggerDesc,
                    _ => Table.Locale.CancelSort,
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

        Expression<Func<TItem, object>> IFieldColumn.GetGroupByExpression<TItem>()
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

            return lambda;
        }

        private void SetSorter(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
            SortModel?.SetSortDirection(sortDirection);
        }

        private void SetFilterCompareOperator(TableFilter filter, TableFilterCompareOperator compareOperator)
        {
            filter.Value = default;
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
                    FilterCompareOperator = _fieldFilterType?.DefaultCompareOperator ?? TableFilterCompareOperator.Equals
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
            SortModel = new SortModel<TData>(this, GetFieldExpression, FieldName, SorterMultiple, sortModel.SortDirection, SorterCompare);
            this.SetSorter(sortModel.SortDirection);
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
                        filterInputFocused = await JsInvokeAsync<string>(JSInteropConstants.GetActiveElement) == baseDomComponent.Id;
                    }
                    catch (JSException jsex)
                    {
                        Console.WriteLine(jsex.ToString());
                        break;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        attemptCount++;
                    }
                    if (attemptCount > 2) break;

                } while (!filterInputFocused);
            });
#endif
        }

        void IFieldColumn.SetHeaderFilter(Type columnDataType)
        {
            if (IsHeader && _fieldFilterType is null)
            {
                _columnDataType = columnDataType;
                _fieldFilterType = Table.FieldFilterTypeResolver.Resolve(columnDataType);

                var getValue = GetValue;
                GetValue = rowData =>
                {
                    var value = getValue(rowData);
                    return (TData)Convert.ChangeType(value, columnDataType);
                };

                var getFieldExpression = GetFieldExpression;

                StateHasChanged();
            }
        }
    }
}
