// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Core.Helpers.MemberPath;
using AntDesign.JsInterop;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Select component to select value from options.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>A dropdown menu for displaying choices - an elegant alternative to the native HTML <c>select</c> element.</item>
        <item>Utilizing <see cref="Radio{TValue}"/> is recommended when there are fewer total options (less than 5).</item>
    </list>
    </summary>
    <seealso cref="SelectOption{TItemValue, TItem}"/>
    <seealso cref="EnumSelect{TEnum}" />
    <inheritdoc/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/_0XzgOis7/Select.svg", Title = "Select", SubTitle = "选择器")]
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
    [CascadingTypeParameter(nameof(TItemValue))]
#endif

    public partial class Select<TItemValue, TItem> : SelectBase<TItemValue, TItem>
    {
        private IReadOnlyDictionary<string, string> _resolvedClassNames;
        private IReadOnlyDictionary<string, string> _resolvedStyles;

        /// <summary>
        /// Computes semantic classes from the current Select instance. This is the Blazor callback form of React's function-valued <c>classNames</c>.
        /// </summary>
        [Parameter]
        public Func<Select<TItemValue, TItem>, IReadOnlyDictionary<string, string>> ClassNamesFunc { get; set; }

        /// <summary>
        /// Computes semantic inline styles from the current Select instance. This is the Blazor callback form of React's function-valued <c>styles</c>.
        /// </summary>
        [Parameter]
        public Func<Select<TItemValue, TItem>, IReadOnlyDictionary<string, string>> StylesFunc { get; set; }

        internal override IReadOnlyDictionary<string, string> ResolvedClassNames => _resolvedClassNames;
        internal override IReadOnlyDictionary<string, string> ResolvedStyles => _resolvedStyles;

        protected override string ImmediatePopupWidthMode => DropdownMatchSelectWidth.IsT0
            ? (DropdownMatchSelectWidth.AsT0 ? "match-trigger" : "content")
            : DropdownMatchSelectWidth.IsT1 ? "min-width" : "custom";
        protected override string ImmediatePopupWidth => DropdownMatchSelectWidth.IsT1
            ? $"{DropdownMatchSelectWidth.AsT1.ToString("0.###", CultureInfo.InvariantCulture)}px"
            : DropdownMatchSelectWidth.IsT2 ? DropdownMatchSelectWidth.AsT2 : string.Empty;
        protected override string ImmediatePopupMaxWidth => DropdownMaxWidth;

        #region Parameters

        private Func<TItem, object> _getNestedOptions;
        private Func<TItem, string> _getGroupLabel;
        private Func<TItem, string> _getGroupClassName;
        private Func<TItem, string> _getGroupTitle;
        private Func<TItem, string> _getTitle;

        /// <summary>
        /// Maps React-compatible option fields, including nested option groups.
        /// </summary>
        [Parameter]
        public SelectFieldNames FieldNames
        {
            get => _fieldNames;
            set
            {
                _fieldNames = value;
                if (value == null) return;

                LabelName = value.Label;
                ValueName = value.Value;
                _getTitle = string.IsNullOrWhiteSpace(value.Title)
                    ? null
                    : PathHelper.GetDelegate<TItem, string>(value.Title);
                _getNestedOptions = null;
                if (!string.IsNullOrWhiteSpace(value.Options))
                {
                    try
                    {
                        _getNestedOptions = PathHelper.GetDelegate<TItem, object>(value.Options);
                    }
                    catch (ArgumentException) when (string.Equals(value.Options, "options", StringComparison.Ordinal))
                    {
                        // React's default field map includes `options`, while
                        // flat Blazor option records often have no such
                        // property. Treat only the missing default as a flat
                        // data source; preserve errors for explicit bad maps.
                    }
                }
                _getGroupLabel = string.IsNullOrWhiteSpace(value.GroupLabel)
                    ? null
                    : PathHelper.GetDelegate<TItem, string>(value.GroupLabel);
                _getGroupClassName = string.IsNullOrWhiteSpace(value.GroupClassName)
                    ? null
                    : PathHelper.GetDelegate<TItem, string>(value.GroupClassName);
                _getGroupTitle = string.IsNullOrWhiteSpace(value.GroupTitle)
                    ? null
                    : PathHelper.GetDelegate<TItem, string>(value.GroupTitle);
            }
        }

        private SelectFieldNames _fieldNames;

        /// <summary>
        /// Enables search, or supplies React v6 search settings such as <c>optionFilterProp</c>.
        /// </summary>
        [Parameter]
        public object ShowSearch { get; set; }

        private SelectShowSearchOptions ShowSearchOptions => ShowSearch switch
        {
            SelectShowSearchOptions options => options,
            OneOf<bool?, SelectShowSearchOptions> legacy when legacy.IsT1 => legacy.AsT1,
            _ => null
        };

        internal override bool? SearchEnabledOverride => ShowSearch switch
        {
            bool enabled => enabled,
            SelectShowSearchOptions => true,
            OneOf<bool?, SelectShowSearchOptions> legacy when legacy.IsT0 => legacy.AsT0,
            OneOf<bool?, SelectShowSearchOptions> => true,
            _ => null
        };
        protected override bool ShouldAutoClearSearchValue => ShowSearchOptions?.AutoClearSearchValue
            ?? AutoClearSearchValue;
        internal override RenderFragment SearchIconContent => ShowSearchOptions?.SearchIcon;
        internal override Func<SelectOptionItem<TItemValue, TItem>, SelectOptionItem<TItemValue, TItem>, string, int> SearchFilterSort =>
            ShowSearchOptions is SelectShowSearchOptions<TItemValue, TItem> typedOptions
                ? typedOptions.FilterSort ?? FilterSort
                : FilterSort;
        private string EffectiveControlledSearchValue => ShowSearchOptions?.SearchValue != null
            ? ShowSearchOptions.SearchValue
            : SearchValue;

        /// <summary>
        /// Legacy border toggle retained for backward compatibility. Use <see cref="Variant"/> to select the selector style.
        /// </summary>
        /// <default value="true"/>
        [Obsolete("Use Variant instead. This parameter is retained for backward compatibility.", false)]
        [Parameter]
        public bool Bordered { get; set; } = true;

        /// <summary>
        /// Visual style of the selector. When omitted, the legacy <see cref="Bordered"/> value is used.
        /// </summary>
        [Parameter]
        public InputVariant? Variant { get; set; }

        private InputVariant EffectiveVariant => Variant ?? (Bordered ? InputVariant.Outlined : InputVariant.Borderless);

        /// <summary>
        /// Validation status of the selector. When omitted, the status is inherited from the containing FormItem.
        /// </summary>
        [Parameter]
        public string Status { get; set; }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Whether to enable the virtual option list, only works for .NET 5 and higher.
        /// Enabled by default to match Ant Design React; set to false to render all options directly.
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool EnableVirtualization
        {
            get => _enableVirtualization;
            set => _enableVirtualization = value;
        }

        private bool _enableVirtualization = true;
#endif

        private bool _dataSourceHasChanged = false;
        private IEnumerable<TItem> _dataSourceCopy;
        private IEnumerable<TItem> _dataSourceShallowCopy;

        /// <summary>
        /// MethodInfo will contain attached MemberwiseClone protected
        /// method. Due to its protection level, it has to be accessed
        /// using reflection. It will be used during generation of
        /// the DataSource shallow copy (which is a new list of DataSource
        /// items with shallow copy of each item).
        /// </summary>
        private MethodInfo _dataSourceItemShallowCopyMehtod;

        private MethodInfo GetDataSourceItemCloneMethod()
        {
            if (_dataSourceItemShallowCopyMehtod is null)
            {
                _dataSourceItemShallowCopyMehtod = typeof(TItem)
                    .GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                if (DataSourceEqualityComparer is null)
                {
                    DataSourceEqualityComparer = this;
                }
            }
            return _dataSourceItemShallowCopyMehtod;
        }

        /// <summary>
        /// The datasource for this component.
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        /// <summary>
        /// Select options. React-compatible name for <see cref="DataSource"/>.
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> Options
        {
            get => DataSource;
            set => DataSource = value;
        }


        /// <summary>
        /// Used for rendering select options manually.
        /// </summary>
        [Parameter]
        public RenderFragment SelectOptions { get; set; }

        /// <summary>
        /// Used for rendering select options manually, alias for <see cref="SelectOptions"/>.
        /// </summary>
        [Parameter]
        [PublicApi("1.5.0")]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// EqualityComparer that will be used during DataSource change
        /// detection. If no comparer set, default .Net is going to be
        /// used.
        /// </summary>
        [Parameter]
        public IEqualityComparer<TItem> DataSourceEqualityComparer { get; set; }

        /// <summary>
        /// Whether to activate the first enabled option when the dropdown opens.
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool DefaultActiveFirstOption
        {
            get { return _defaultActiveFirstOption; }
            set
            {
                _defaultActiveFirstOption = value;
                if (!_defaultActiveFirstOption)
                {
                    _defaultActiveFirstOptionApplied = true;
                }
            }
        }

        /// <summary>
        /// The name of the property to be used as a disabled indicator.
        /// </summary>
        [Parameter]
        public string DisabledName
        {
            get => _disabledName;
            set
            {
                _getDisabledValue = string.IsNullOrWhiteSpace(value) ? null : PathHelper.GetDelegate<TItem, object>(value);
                _getDisabled = _getDisabledValue == null
                    ? null
                    : item => _getDisabledValue(item) is bool disabled && disabled;
                _disabledName = value;
            }
        }

        /// <summary>
        /// Controls the dropdown width. <c>true</c> matches the Select trigger width;
        /// <c>false</c> fits the widest dropdown item while keeping the trigger width as a minimum;
        /// a <c>number</c> sets the popup's minimum width in pixels; a <c>string</c>
        /// sets an explicit CSS width (for example, <c>250px</c>).
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public OneOf<bool, double, string> DropdownMatchSelectWidth { get; set; } = true;

        /// <summary>
        /// Controls whether the popup matches the Select width. React-compatible name for <see cref="DropdownMatchSelectWidth"/>.
        /// </summary>
        [Parameter]
        public OneOf<bool, double, string> PopupMatchSelectWidth
        {
            get => DropdownMatchSelectWidth;
            set => DropdownMatchSelectWidth = value;
        }

        /// <summary>
        /// Will not allow dropdown width to grow above stated in here value (eg. "768px")
        /// </summary>
        /// <default value="auto"/>
        [Parameter]
        public string DropdownMaxWidth { get; set; } = "auto";

        /// <summary>
        /// The name of the property to be used as a group indicator.
        /// If the value is set, the entries are displayed in groups.
        /// Use additional <see cref="SelectBase{TItemValue, TItem}.SortByGroup"/> and <see cref="SelectBase{TItemValue, TItem}.SortByLabel"/>.
        /// </summary>
        [Parameter]
        public string GroupName
        {
            get => _groupName;
            set
            {
                _getGroup = string.IsNullOrWhiteSpace(value) ? null : PathHelper.GetDelegate<TItem, string>(value);
                _groupName = value;
            }
        }

        /// <summary>
        /// Is used to increase the speed. If you expect changes to the label name,
        /// group name or disabled indicator, disable this property.
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool IgnoreItemChanges { get; set; } = true;

        /// <summary>
        /// Is used to customize the item style.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        /// <summary>
        /// Customize how each dropdown option is rendered. The context exposes
        /// the option and its zero-based index in the visible, sorted list,
        /// matching React Select's <c>optionRender(option, { index })</c>.
        /// </summary>
        [Parameter]
        public RenderFragment<SelectOptionRenderInfo<TItemValue, TItem>> OptionRender { get; set; }

        internal RenderFragment<TItem> EffectiveItemTemplate => ItemTemplate;

        /// <summary>
        /// Specify content to show when no result matches.
        /// </summary>
        [Parameter]
        public RenderFragment NotFoundContent { get; set; }

        /// <summary>
        /// Called when custom tag is created.
        /// </summary>

        [Parameter]
        public Action<string> OnCreateCustomTag { get; set; }

        /// <summary>
        /// Called when the datasource object/reference changes.
        /// It does not trigger if a value inside the datasource changes.
        /// </summary>
        [Parameter]
        public Action OnDataSourceChanged { get; set; }

        /// <summary>
        /// Called when the dropdown visibility changes. Use <see cref="OnOpenChange"/> instead.
        /// </summary>
        [Obsolete("Use OnOpenChange instead.")]
        [Parameter]
        public Action<bool> OnDropdownVisibleChange { get; set; }

        /// <summary>
        /// Callback fired when the search input changes. Configure this callback in
        /// <see cref="ShowSearch"/> instead.
        /// </summary>
        [Obsolete("Configure OnSearch in ShowSearch instead.")]
        [Parameter]
        public Action<string> OnSearch { get; set; }

        [Parameter] public string PopupContainerMaxHeight { get; set; } = "256px";

        /// <summary>
        /// Configures the maximum height of the option list. React-compatible alias for <see cref="PopupContainerMaxHeight"/>.
        /// </summary>
        /// <default value="256"/>
        [Parameter]
        public int? ListHeight
        {
            get
            {
                var value = PopupContainerMaxHeight?.Trim();
                if (value != null && value.EndsWith("px", StringComparison.OrdinalIgnoreCase))
                    value = value.Substring(0, value.Length - 2);
                return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var height) ? height : null;
            }
            set
            {
                if (value.HasValue)
                    PopupContainerMaxHeight = $"{value.Value.ToString(CultureInfo.InvariantCulture)}px";
            }
        }

        /// <summary>
        /// Whether to show the drop-down arrow
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public override bool ShowArrowIcon
        {
            get { return _showArrowIcon; }
            set
            {
                _showArrowIcon = value;
            }
        }

        /// <summary>
        /// Whether to show the dropdown arrow. React-compatible name for <c>ShowArrowIcon</c>.
        /// </summary>
        [Parameter]
        public bool ShowArrow
        {
            get => ShowArrowIcon;
            set => ShowArrowIcon = value;
        }

        /// <summary>
        /// Define token separators for newly created tags. Pass a character array for
        /// delimiter-based tokenization or a callback to parse the current input, matching
        /// React Select v6.
        /// </summary>
        [Parameter]
        public OneOf<char[], Func<string, IEnumerable<string>>> TokenSeparators { get; set; }

        private char[] TokenSeparatorCharacters => TokenSeparators.IsT0 ? TokenSeparators.AsT0 : null;
        private Func<string, IEnumerable<string>> TokenSeparatorTokenizer => TokenSeparators.IsT1 ? TokenSeparators.AsT1 : Tokenize;

#if NET5_0_OR_GREATER
        /// <summary>
        /// Whether to use virtual scrolling. React-compatible name for <see cref="EnableVirtualization"/>.
        /// </summary>
        [Parameter]
        public bool Virtual
        {
            get => EnableVirtualization;
            set => EnableVirtualization = value;
        }
#endif

        /// <summary>
        /// Converts the current search text into committed values for tags or multiple mode.
        /// Return an empty sequence while the text is incomplete.
        /// </summary>
        [Parameter]
        public Func<string, IEnumerable<string>> Tokenize { get; set; }

        /// <summary>
        /// Used for the two-way binding.
        /// </summary>
        [Parameter]
        public override EventCallback<TItemValue> ValueChanged { get; set; }

        private bool _valueHasChanged;

        /// <summary>
        /// Get or set the selected value.
        /// </summary>
        [Parameter]
        public override TItemValue Value
        {
            get => _selectedValue;
            set
            {
                _valueHasChanged = !EqualityComparer<TItemValue>.Default.Equals(value, _selectedValue);
                if (_valueHasChanged)
                {
                    _selectedValue = value;
                    _valueHasChanged = _isInitialized;
                }
            }
        }

        /// <summary>
        /// Specifies the label property in the option object. If use this property, should not use <see cref="LabelName"/>
        /// </summary>
        [Obsolete("Use ItemLabel instead")]
        [Parameter]
        public Func<TItem, string> LabelProperty { get => _getLabel; set => _getLabel = value; }

        /// <summary>
        /// Specifies the value property in the option object. If use this property, should not use <see cref="ValueName"/>
        /// </summary>
        [Obsolete("Use ItemValue instead")]
        [Parameter]
        public Func<TItem, TItemValue> ValueProperty { get => _getValue; set => _getValue = value; }

        /// <summary>
        /// Specifies predicate for disabled options
        /// </summary>
        [Parameter] public Func<TItem, bool> DisabledPredicate
        {
            get => _getDisabled;
            set
            {
                _getDisabled = value;
                _getDisabledValue = null;
            }
        }

        /// <summary>
        /// Used when Mode =  default - The value is used during initialization and when pressing the Reset button within Forms.
        /// </summary>
        [Parameter]
        public TItemValue DefaultValue
        {
            get => _defaultValue;
            set
            {
                var hasChanged = !EqualityComparer<TItemValue>.Default.Equals(value, _defaultValue);
                if (hasChanged)
                {
                    _defaultValueIsNotNull = !EqualityComparer<TItemValue>.Default.Equals(value, default);
                    _defaultValue = value;
                }
            }
        }

        [Parameter] public string ListboxStyle { get; set; } = "display: flex; flex-direction: column;";

        /// <summary>
        /// Custom filter expression to filter options based on search value.
        /// </summary>
        [Parameter]
        public Func<SelectOptionItem<TItemValue, TItem>, string, bool> FilterExpression { get; set; }

        /// <summary>
        /// Legacy top-level alias for <see cref="SelectShowSearchOptions.OptionFilterProp"/>.
        /// </summary>
        [Obsolete("Set OptionFilterProp inside ShowSearch to match React v6.", false)]
        [Parameter]
        public OneOf<string, string[]> OptionFilterProp { get; set; } = "value";

        internal OneOf<string, string[]> EffectiveOptionFilterProp => ShowSearchOptions != null
            ? ShowSearchOptions.OptionFilterProp
            : OptionFilterProp;

        /// <summary>
        /// Whether to filter options by the search text. Set to false to keep all options visible;
        /// use <see cref="FilterExpression"/> to customize the matching rule.
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool? FilterOption { get; set; } = true;

        internal bool EffectiveFilterOption => ShowSearchOptions != null
            ? GetShowSearchFilterOptionEnabled() ?? ShowSearchOptions.FilterOption ?? FilterOption != false
            : FilterOption != false;

        private bool? GetShowSearchFilterOptionEnabled()
        {
            if (ShowSearchOptions is SelectShowSearchOptions<TItemValue, TItem> typedOptions)
            {
                if (typedOptions.FilterOptionEnabled.HasValue)
                {
                    return typedOptions.FilterOptionEnabled;
                }

                return typedOptions.FilterOption != null ? true : null;
            }

            return null;
        }

        private Func<string, SelectOptionItem<TItemValue, TItem>, bool> ShowSearchFilterPredicate =>
            ShowSearchOptions is SelectShowSearchOptions<TItemValue, TItem> typedOptions
                && typedOptions.FilterOption != null
                    ? typedOptions.FilterOption
                    : null;

        #endregion Parameters

        #region Properties

        /// <summary>
        /// Indicates if the GroupName is used. When this value is True, the SelectOptions will be rendered in group mode.
        /// </summary>
        internal bool IsGroupingEnabled
        {
            get => !string.IsNullOrWhiteSpace(GroupName) || _getNestedOptions != null;
        }

        internal override bool HasSelectOptions => SelectOptions != null;
        private bool _lastVirtualAccessibility;

        internal override bool UsesVirtualAccessibility
        {
            get
            {
#if NET5_0_OR_GREATER
                // React Select uses the accessibility listbox/proxy whenever
                // virtualization is enabled, even for short option lists.
                // The visual list only needs a recycled window for larger sets.
                return EnableVirtualization
                       && !(DropdownMatchSelectWidth.IsT0 && !DropdownMatchSelectWidth.AsT0)
                       && !IsGroupingEnabled;
#else
                return false;
#endif
            }
        }

        internal ElementReference DropDownRef => _dropDown.GetOverlayComponent().Ref;
        private ElementReference _scrollableSelectDiv;
        private const int VirtualListItemHeight = 32;
        private const int VirtualListRowsRendered = 10;
        private int _virtualListStartIndex;
        private int _virtualListEndIndex = VirtualListRowsRendered;
        private DotNetObjectReference<Select<TItemValue, TItem>> _virtualListReference;
        private ElementReference _attachedVirtualListHolder;
        private bool _virtualListAttached;
        private bool _virtualScrollbarFlashedForOpen;

        internal override bool UsesVirtualWindow => UsesVirtualAccessibility
                                                    && SortedSelectOptionItems.Count(option => !option.IsHidden) > 8;

        internal int VirtualListOptionCount => SortedSelectOptionItems.Count(option => !option.IsHidden);

        internal string VirtualListSpacerStyle
        {
            get
            {
                var height = FormatVirtualListPixels(VirtualListOptionCount * VirtualListItemHeight);
                return $"height: {height}px; position: relative; overflow: hidden;";
            }
        }

        internal string VirtualListInnerStyle
        {
            get
            {
                var baseStyle = ListboxStyle?.Trim() ?? string.Empty;
                if (baseStyle.Length > 0 && !baseStyle.EndsWith(';')) baseStyle += ";";
                var offset = _virtualListStartIndex * VirtualListItemHeight;
                return $"{baseStyle} transform: translateY({offset}px); margin-left: 0px; position: absolute; left: 0px; right: 0px; top: 0px;";
            }
        }

        internal IEnumerable<SelectOptionItem<TItemValue, TItem>> GetVirtualWindowOptions()
        {
            var options = SortedSelectOptionItems.Where(option => !option.IsHidden).ToList();
            var start = Math.Clamp(_virtualListStartIndex, 0, options.Count);
            var end = Math.Clamp(_virtualListEndIndex, start, options.Count);
            return options.Skip(start).Take(end - start);
        }

        private static string FormatVirtualListPixels(int value)
        {
            return value >= 1_000_000
                ? value.ToString("0.0e+00", CultureInfo.InvariantCulture)
                : value.ToString(CultureInfo.InvariantCulture);
        }

        [JSInvokable]
        public Task UpdateVirtualWindow(int startIndex, int endIndex)
        {
            return InvokeAsync(() =>
            {
                if (!UsesVirtualWindow) return;
                var optionCount = VirtualListOptionCount;
                var nextStart = Math.Clamp(startIndex, 0, optionCount);
                var nextEnd = Math.Clamp(endIndex, nextStart, optionCount);
                if (nextStart == _virtualListStartIndex && nextEnd == _virtualListEndIndex) return;

                _virtualListStartIndex = nextStart;
                _virtualListEndIndex = nextEnd;
                StateHasChanged();
                RefreshComponentState();
            });
        }

        [JSInvokable]
        public Task SetActiveVirtualOption(string internalId)
        {
            if (!Guid.TryParse(internalId, out var optionId)) return Task.CompletedTask;

            return InvokeAsync(() =>
            {
                var option = SelectOptionItems.FirstOrDefault(item => item.InternalId == optionId);
                if (option == null || option.IsHidden || option.IsDisabled || ActiveOption == option) return;

                ActiveOption = option;
            });
        }


        private TItemValue _selectedValue;
        private TItemValue _defaultValue;
        private bool _defaultValueIsNotNull;
        private IEnumerable<TItem> _datasource;
        private bool _afterFirstRender;
        private bool _optionsHasInitialized;
        private bool _defaultValueApplied;
        private bool _defaultValueWasCleared;
        private bool _defaultActiveFirstOptionApplied;
        private bool _waitingForStateChange;
        private bool _isValueEnum;
        private bool _isToken;
        private bool _defaultActiveFirstOption = true;

        private string _groupName = string.Empty;

        private Func<TItem, string> _getGroup;

        private string _disabledName;

        private Func<TItem, bool> _getDisabled;
        private Func<TItem, object> _getDisabledValue;


        private bool _showArrowIcon = true;

        #endregion Properties

        private static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid),
#if NET6_0_OR_GREATER
                    typeof(DateOnly)
#endif
                }.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }

        protected override bool IsSelectFixed()
        {
            return !IsGroupingEnabled;
        }

        protected override void OnInitialized()
        {
            if (ChildContent != null)
            {
                SelectOptions = ChildContent;
            }

            if (DataSource?.Any() == true && typeof(TItemValue) != typeof(TItem) && ValueProperty == null && string.IsNullOrWhiteSpace(ValueName))
            {
                throw new ArgumentNullException(nameof(ValueName));
            }

            //SetClassMap();

            if (string.IsNullOrWhiteSpace(Style))
                Style = DefaultWidth;

            if (!_isInitialized)
            {
                _isPrimitive = IsSimpleType(typeof(TItem));
                _isValueEnum = typeof(TItemValue).IsEnum;
            }
            _isInitialized = true;

            base.OnInitialized();
        }


        internal override void AddOptionItem(SelectOptionItem<TItemValue, TItem> optionItem)
        {
            base.AddOptionItem(optionItem);

            SetInitialValuesAsync(optionItem);
        }

        protected override async Task OnParametersSetAsync()
        {
            _resolvedClassNames = ClassNamesFunc?.Invoke(this) ?? ClassNames;
            _resolvedStyles = StylesFunc?.Invoke(this) ?? Styles;

            var effectiveSearchValue = EffectiveControlledSearchValue;
            var controlledSearchValue = SearchValueChanged.HasDelegate || effectiveSearchValue != null;
            var searchValueChanged = !_hasSearchValueParameter || !string.Equals(_lastSearchValueParameter, effectiveSearchValue, StringComparison.Ordinal);
            _hasSearchValueParameter = true;
            _lastSearchValueParameter = effectiveSearchValue;
            if (controlledSearchValue && searchValueChanged)
            {
                _prevSearchValue = _searchValue;
                _searchValue = effectiveSearchValue ?? string.Empty;
            }

            var virtualAccessibilityChanged = _lastVirtualAccessibility != UsesVirtualAccessibility;
            _lastVirtualAccessibility = UsesVirtualAccessibility;
            SetClassMap();
            EvaluateDataSourceChange();

            if (SelectOptions == null)
            {
                if (!_optionsHasInitialized || _dataSourceHasChanged)
                {
                    CreateDeleteSelectOptions();
                    _optionsHasInitialized = true;
                    _dataSourceHasChanged = false;
#if NET10_0_OR_GREATER
                    // .NET 10: Force re-render of overlay and parent component after DataSource changes
                    RefreshComponentState();
#endif
                }
            }

            if (_valueHasChanged && _optionsHasInitialized)
            {
                _valueHasChanged = false;
                if (Form?.ValidateOnChange == true && FieldIdentifier is { Model: not null, FieldName: not null })
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }

                await OnValueChangeAsync(_selectedValue);
            }
            if (controlledSearchValue && searchValueChanged && _optionsHasInitialized)
            {
                if (!string.IsNullOrEmpty(_searchValue) && EffectiveFilterOption)
                {
                    FilterOptionItems(_searchValue);
                }
                else
                {
                    UnhideSelectOptions();
                }
                _dropDown?.GetOverlayComponent()?.RefreshComponentState();
            }
            await base.OnParametersSetAsync();
            if (virtualAccessibilityChanged)
            {
                if (!UsesVirtualAccessibility && _scrollableSelectDiv.Id != null)
                    await JsInvokeAsync(JSInteropConstants.SelectHelper.CancelScroll, _scrollableSelectDiv);
                _dropDown?.GetOverlayComponent()?.RefreshComponentState();
            }
        }

        private string _lastSearchValueParameter;
        private bool _hasSearchValueParameter;

        private void EvaluateDataSourceChange()
        {
            if (DataSource == null && _datasource == null)
            {
                return;
            }

            // clear DataSource
            if (DataSource == null && _datasource != null)
            {
                SelectOptionItems.Clear();
                SelectedOptionItems.Clear();
                Value = default;

                _datasource = null;
                _dataSourceCopy = null;
                _dataSourceShallowCopy = null;
                TypeDefaultExistsAsSelectOption = false;

                OnDataSourceChanged?.Invoke();
                return;
            }

            // DataSource maybe changed
            if (DataSource != null)
            {
                if (_datasource == null)
                {
                    _dataSourceHasChanged = true;
                }
                else if (_isPrimitive)
                {
                    _dataSourceHasChanged = !DataSource.SequenceEqual(_dataSourceCopy);
                }
                else if (_getValue is null)
                {
                    _dataSourceHasChanged = !DataSource.SequenceEqual(_dataSourceCopy) ||
                        !DataSource.SequenceEqual(_dataSourceShallowCopy, DataSourceEqualityComparer);
                }
                else
                {
                    _dataSourceHasChanged = !DataSource.SequenceEqual(_dataSourceShallowCopy, DataSourceEqualityComparer);
                }

                if (_dataSourceHasChanged)
                {
                    _datasource = DataSource;
                    if (_isPrimitive)
                    {
                        // Maybe a workaound for issues 2439
                        SelectOptionItems.Clear();
                        SelectedOptionItems.Clear();

                        _dataSourceCopy = _datasource.ToList();
                    }
                    else
                    {
                        if (_getValue is null)
                        {
                            _dataSourceCopy = _datasource.ToList();
                        }
                        var cloneMethod = GetDataSourceItemCloneMethod();
                        _dataSourceShallowCopy = _datasource.Select(x => (TItem)cloneMethod.Invoke(x, null)).ToList();
                    }

                    OnDataSourceChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// Used only when ChildElement SelectOptions is used.
        /// Will run this process if after initalization an item
        /// is added that is also marked as selected.
        /// </summary>
        /// <returns></returns>
        internal override async Task ProcessSelectedSelectOptions()
        {
            if (_isInitialized && _afterFirstRender)
            {
                if (Mode == SelectMode.Default)
                {
                    if (LastValueBeforeReset is not null)
                    {
                        await OnValueChangeAsync(LastValueBeforeReset);
                        LastValueBeforeReset = default;
                    }
                    else
                    {
                        await OnValueChangeAsync(Value);
                    }
                }
                else
                {
                    await OnValuesChangeAsync(Values);
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (SelectOptions != null)
            {
                _optionsHasInitialized = true;
            }

            if (firstRender)
            {
                _defaultValueApplied = _defaultValueWasCleared || !(_defaultValueIsNotNull || _defaultValuesHasItems);

                _defaultActiveFirstOptionApplied = _defaultValueWasCleared || !(_defaultActiveFirstOption && SelectedOptionItems.Count == 0);
            }
            if (!_defaultValueApplied || !_defaultActiveFirstOptionApplied)
            {
                if (Mode == SelectMode.Default)
                {
                    bool hasOptions = SelectOptionItems.Any();
                    if (!HasValue && hasOptions && (_defaultValueIsNotNull || DefaultActiveFirstOption))
                    {
                        await TrySetDefaultValueAsync();
                    }
                }
                else
                {
                    if (_defaultValuesHasItems && !HasValue && SelectOptionItems.Any()
                        || DefaultActiveFirstOption && !HasValue && SelectOptionItems.Any())
                    {
                        await TrySetDefaultValuesAsync();
                    }
                }
            }

            if (_isInitialized && SelectOptions == null && !_optionsHasInitialized)
            {
                CreateDeleteSelectOptions();
                _optionsHasInitialized = true;
            }

            if (_waitingForStateChange)
            {
                _waitingForStateChange = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
            _afterFirstRender = true;

            var virtualListHolderChanged = _virtualListAttached
                                           && _attachedVirtualListHolder.Id != _scrollableSelectDiv.Id;
            if (UsesVirtualWindow && _scrollableSelectDiv.Id != null
                && (!_virtualListAttached || virtualListHolderChanged))
            {
                if (_virtualListAttached)
                {
                    await JsInvokeAsync(JSInteropConstants.SelectHelper.DetachVirtualList, _attachedVirtualListHolder);
                    _virtualListAttached = false;
                }
                _virtualListReference ??= DotNetObjectReference.Create(this);
                await JsInvokeAsync(JSInteropConstants.SelectHelper.AttachVirtualList,
                    _scrollableSelectDiv, _virtualListReference, VirtualListItemHeight, VirtualListRowsRendered, VirtualListOptionCount);
                _attachedVirtualListHolder = _scrollableSelectDiv;
                _virtualListAttached = true;
            }
            else if (!UsesVirtualWindow && _virtualListAttached)
            {
                await JsInvokeAsync(JSInteropConstants.SelectHelper.DetachVirtualList, _attachedVirtualListHolder);
                _virtualListAttached = false;
                _attachedVirtualListHolder = default;
                _virtualListReference?.Dispose();
                _virtualListReference = null;
            }

            if (!IsDropdownShown())
            {
                _virtualScrollbarFlashedForOpen = false;
            }
            else if (UsesVirtualWindow && !_virtualScrollbarFlashedForOpen)
            {
                await JsInvokeAsync(JSInteropConstants.SelectHelper.ShowScrollbar, _scrollableSelectDiv);
                _virtualScrollbarFlashedForOpen = true;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (_virtualListAttached && _attachedVirtualListHolder.Id != null)
            {
                _ = JsInvokeAsync(JSInteropConstants.SelectHelper.DetachVirtualList, _attachedVirtualListHolder);
                _virtualListAttached = false;
                _attachedVirtualListHolder = default;
            }
            _virtualListReference?.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Create or delete SelectOption when the datasource changed
        /// </summary>
        private void CreateDeleteSelectOptions()
        {
            if (_datasource == null)
                return;

            var sourceOptions = FlattenDataSource(_datasource).ToArray();

            Dictionary<TItem, SelectOptionItem<TItemValue, TItem>> dataStoreToSelectOptionItemsMatch = new();
            // Compare items of SelectOptions and the datastore
            if (SelectOptionItems.Any())
            {
                // Delete items from SelectOptions if it is no longer in the datastore
                for (var i = SelectOptionItems.Count - 1; i >= 0; i--)
                {
                    var selectOption = SelectOptionItems[i];
                    if (!selectOption.IsAddedTag)
                    {
                        var exists = sourceOptions.FirstOrDefault(x => EqualityComparer<TItem>.Default.Equals(x.Item, selectOption.Item)).Item;

                        if (exists is null)
                        {
                            if (IgnoreItemChanges)
                            {
                                SelectOptionItems.RemoveAt(i);
                            }
                            RemoveEqualityToNoValue(selectOption);

                            if (selectOption.IsSelected)
                                SelectedOptionItems.Remove(selectOption.Value);
                        }
                        else
                            dataStoreToSelectOptionItemsMatch[exists] = selectOption; // Use indexer to avoid ArgumentException on duplicate keys
                    }
                }
            }
            if (!IgnoreItemChanges)
            {
                SelectOptionItems.Clear();
            }

            //A simple approach to avoid unnecessary scanning through _selectedValues once
            //all of SelectOptionItem where already marked as selected
            int processedSelectedCount = 0;
            if (Mode == SelectMode.Default && _selectedValue != null)
                processedSelectedCount = 1;
            else if (Mode != SelectMode.Default && _selectedValues != null)
                processedSelectedCount = _selectedValues.Count();

            foreach (var sourceOption in sourceOptions)
            {
                var item = sourceOption.Item;
                TItemValue value = _getValue == null ? (TItemValue)(object)item : _getValue(item);

                var exists = false;
                SelectOptionItem<TItemValue, TItem> selectOption;
                SelectOptionItem<TItemValue, TItem> updateSelectOption = null;

                if (dataStoreToSelectOptionItemsMatch.TryGetValue(item, out selectOption))
                {
                    var result = EqualityComparer<TItemValue>.Default.Equals(selectOption.Value, value);

                    if (result)
                    {
                        exists = true;
                        updateSelectOption = selectOption;
                    }
                }

                var disabled = false;
                var groupName = sourceOption.GroupName;
                var groupClassName = sourceOption.GroupClassName;
                var groupTitle = sourceOption.GroupTitle;
                var label = _getLabel == null ? GetLabel(item) : _getLabel(item);

                bool isSelected = false;
                if (processedSelectedCount > 0)
                {
                    if (Mode == SelectMode.Default)
                        isSelected = ReferenceEquals(value, _selectedValue) || value?.Equals(_selectedValue) == true;
                    else
                        isSelected = _selectedValues.Contains(value);
                }

                if (_getDisabled != default)
                    disabled = _getDisabled(item);

                if (string.IsNullOrWhiteSpace(groupName) && !string.IsNullOrWhiteSpace(GroupName))
                {
                    groupName = _getGroup(item);
                    groupClassName = _getGroupClassName?.Invoke(item);
                    groupTitle = _getGroupTitle?.Invoke(item);
                }

                if (!exists)
                {
                    var newItem = new SelectOptionItem<TItemValue, TItem>
                    {
                        Label = label,
                        Title = _getTitle?.Invoke(item),
                        GroupName = groupName,
                        GroupClassName = groupClassName,
                        GroupTitle = groupTitle,
                        IsDisabled = disabled,
                        HasExplicitDisabled = _getDisabledValue?.Invoke(item) != null,
                        Item = item,
                        Value = value,
                        IsSelected = isSelected,
                        IsHidden = isSelected && HideSelected
                    };

                    SelectOptionItems.Add(newItem);
                    AddEqualityToNoValue(newItem);
                    if (isSelected)
                    {
                        processedSelectedCount--;
                        SelectedOptionItems[newItem.Value] = newItem;
                    }
                }
                else if (exists && !IgnoreItemChanges)
                {
                    updateSelectOption.Label = label;
                    updateSelectOption.Title = _getTitle?.Invoke(item);
                    updateSelectOption.IsDisabled = disabled;
                    updateSelectOption.HasExplicitDisabled = _getDisabledValue?.Invoke(item) != null;
                    updateSelectOption.GroupName = groupName;
                    updateSelectOption.GroupClassName = groupClassName;
                    updateSelectOption.GroupTitle = groupTitle;
                    updateSelectOption.IsHidden = isSelected && HideSelected;
                    SelectOptionItems.Add(updateSelectOption);
                    if (isSelected)
                    {
                        if (!updateSelectOption.IsSelected)
                        {
                            //updateSelectOption.IsSelected = isSelected;
                            //SelectedOptionItems.Add(updateSelectOption);
                        }
                        processedSelectedCount--;
                    }
                }
            }

            // add the tag options back in if they were removed since they are separate from the datasource
            foreach (var tag in AddedTags)
            {
                if (!SelectOptionItems.Contains(tag))
                {
                    SelectOptionItems.Add(tag);

                    if (tag.IsSelected)
                    {
                        processedSelectedCount--;
                        SelectedOptionItems[tag.Value] = tag;
                    }
                }
            }
        }

        private IEnumerable<(TItem Item, string GroupName, string GroupClassName, string GroupTitle)> FlattenDataSource(
            IEnumerable<TItem> items,
            string parentGroup = null,
            string parentGroupClassName = null,
            string parentGroupTitle = null)
        {
            foreach (var item in items ?? Enumerable.Empty<TItem>())
            {
                if (_getNestedOptions?.Invoke(item) is IEnumerable nestedOptions && nestedOptions is not string)
                {
                    var groupLabel = _getGroupLabel?.Invoke(item);
                    var childGroup = string.IsNullOrWhiteSpace(groupLabel) ? parentGroup : groupLabel;
                    var groupClassName = _getGroupClassName?.Invoke(item);
                    if (string.IsNullOrWhiteSpace(groupClassName))
                        groupClassName = parentGroupClassName;
                    var groupTitle = _getGroupTitle?.Invoke(item);
                    if (string.IsNullOrWhiteSpace(groupTitle))
                        groupTitle = parentGroupTitle;
                    var children = nestedOptions.Cast<object>().OfType<TItem>();
                    foreach (var child in FlattenDataSource(children, childGroup, groupClassName, groupTitle))
                        yield return child;
                    continue;
                }

                yield return (item, parentGroup, parentGroupClassName, parentGroupTitle);
            }
        }

        /// <summary>
        /// Sets the CSS classes to change the visual style
        /// </summary>
        protected override void SetClassMap()
        {
            var status = GetValidationStatus();
            ClassMapper
                .Clear()
                .Add($"{ClassPrefix}")
                .If($"{ClassPrefix}-sm", () => Size == InputSize.Small)
                .If($"{ClassPrefix}-lg", () => Size == InputSize.Large)
                .If($"{ClassPrefix}-{EffectiveVariant.ToString().ToLowerInvariant()}", () => true)
                .Add($"{ClassPrefix}-css-var")
                .If($"{ClassPrefix}-focused", () => Focused)
                .If($"{ClassPrefix}-single", () => Mode == SelectMode.Default)
                .If($"{ClassPrefix}-multiple", () => Mode != SelectMode.Default)
                .If($"{ClassPrefix}-in-form-item ", () => FormItem != null)
                .If($"{ClassPrefix}-has-feedback", () => FormItem?.HasFeedback == true)
                .If($"{ClassPrefix}-allow-clear", () => !Disabled && IsAllowClearEnabled && HasValueOrUnmatched)
                .If($"{ClassPrefix}-show-arrow", () => ShowArrowIcon)
                .If($"{ClassPrefix}-disabled", () => Disabled)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-open", () => _isDropdownVisible || (_dropDown?.IsOverlayShow() ?? false) || (_dropDown?.IsOverlayOpening() ?? false))
                .If($"{ClassPrefix}-show-search", () => IsSearchEnabled)
                .If($"{ClassPrefix}-rtl", () => IsRTLDirection)
                .GetIf(() => $"{ClassPrefix}-status-{status}", () => status is not null)

                ;
        }

        protected override void OnDefaultValueCleared()
        {
            _defaultValueWasCleared = true;
            _defaultValueApplied = true;
            _defaultActiveFirstOptionApplied = true;
        }

        private string? GetValidationStatus()
        {
            if (!string.IsNullOrWhiteSpace(Status))
                return Status.ToLowerInvariant();

            return FormItem?.ValidateStatus is FormValidateStatus.Error or FormValidateStatus.Warning
                ? FormItem.ValidateStatus.ToString().ToLowerInvariant()
                : null;
        }

        /// <summary>
        /// Returns True if the parameter IsHidden is set to true for all entries in the SelectOptions list
        /// </summary>
        /// <returns>true if all items are set to IsHidden(true)</returns>
        protected bool AllOptionsHidden()
        {
            if (AddedTags.Count > 0)
                return SelectOptionItems.All(x => x.IsHidden) && AddedTags.All(x => x.IsHidden);
            else
                return SelectOptionItems.All(x => x.IsHidden);
        }

        protected override async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            _isDropdownVisible = visible;

            // Focus an interactively opened Select, but don't steal focus when
            // the popup is controlled open (for example, the Semantic DOM
            // preview). Its native focus event also drives the focused style.
            if (visible && !Open && !Focused)
            {
                // The pre-visible callback normally focuses before reveal;
                // retain this fallback for visibility changes from custom triggers.
                await SetInputFocusAsync(refreshState: false);
            }

            OnDropdownVisibleChange?.Invoke(visible);
            await OnOpenChange.InvokeAsync(visible);

            // Overlay visibility changes independently of the Select's bound
            // Open parameter. Refresh the root classes and the cascading
            // IsOverlayShow value so its ARIA state follows the actual popup,
            // including when Select is used without an Open binding.
            SetClassMap();
            await InvokeAsync(StateHasChanged);
            if (visible)
            {
                OnOverlayShow();

                if (UsesVirtualWindow && _scrollableSelectDiv.Id != null)
                {
                    await JsInvokeAsync(JSInteropConstants.SelectHelper.ShowScrollbar, _scrollableSelectDiv);
                    _virtualScrollbarFlashedForOpen = true;
                }

                // React Select does not steal focus when its popup is open
                // through the controlled `Open` parameter. In particular, a
                // controlled-open semantic preview must leave focus on the
                // control that changed its mode instead of focusing the new
                // Select input after its first render.
                if (!Open)
                {
                    FocusIfInSearch();
                }

                await ScrollToFirstSelectedItemAsync();

            }
            else
            {
                if (UsesVirtualAccessibility && _scrollableSelectDiv.Id != null)
                    await JsInvokeAsync(JSInteropConstants.SelectHelper.CancelScroll, _scrollableSelectDiv);
                await OnOverlayHide();
            }
        }


        /// <summary>
        /// Scrolls to the item via JavaScript.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private async Task ElementScrollIntoViewAsync(ElementReference element)
        {
            if (UsesVirtualAccessibility)
            {
                var index = SortedSelectOptionItems.Where(option => !option.IsHidden).ToList()
                    .FindIndex(option => option == ActiveOption);
                if (index >= 0)
                    await JsInvokeAsync(JSInteropConstants.SelectHelper.ScrollToIndex, _scrollableSelectDiv, index, ActiveOption.Label);
                return;
            }
            await JsInvokeAsync(JSInteropConstants.ScrollTo, element, _scrollableSelectDiv);
        }

        /// <summary>
        /// Called by the Form reset method
        /// </summary>
        internal override void ResetValue()
        {
            _ = ClearSelectedAsync();
        }

        /// <summary>
        /// If DefaultActiveFirstOption is true, highlight the first enabled option without selecting it.
        /// </summary>
        private Task SetDefaultActiveFirstItemAsync()
        {
            ActiveOption = SortedSelectOptionItems.FirstOrDefault(x => !x.IsDisabled);
            if (ActiveOption != null)
            {
                _waitingForStateChange = true;
            }
            _defaultActiveFirstOptionApplied = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method invoked by OnAfterRenderAsync if the Value is null(default) and
        /// DefaultValue has a value or DefaultActiveFirstItem is True.
        /// </summary>
        private async Task TrySetDefaultValueAsync()
        {
            if (_defaultValueIsNotNull)
            {
                var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, _defaultValue));

                if (result != null && !result.IsDisabled)
                {
                    result.IsSelected = true;

                    if (HideSelected)
                        result.IsHidden = true;

                    _waitingForStateChange = true;
                    SelectedOptionItems[result.Value] = result;
                    await ValueChanged.InvokeAsync(result.Value);

                    _defaultValueApplied = true;
                    return;
                }
            }
            if (DefaultActiveFirstOption)
            {
                await SetDefaultActiveFirstItemAsync();
            }
            else
            {
                await ClearSelectedAsync();
            }
            _defaultValueApplied = true;
        }

        /// <summary>
        /// Method invoked by OnAfterRenderAsync if the Value is null(default) and
        /// DefaultValues has a values or DefaultActiveFirstItem is True.
        /// </summary>
        private async Task TrySetDefaultValuesAsync()
        {
            if (_defaultValuesHasItems)
            {
                foreach (var defaultValue in _defaultValues)
                {
                    var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, defaultValue));

                    // Disabled options may still be selected by an initial/default value.
                    if (result != null)
                    {
                        result.IsSelected = true;

                        if (HideSelected)
                            result.IsHidden = true;
                    }
                }

                var anySelected = SelectOptionItems.Any(x => x.IsSelected);

                if (!anySelected)
                {
                    if (DefaultActiveFirstOption)
                    {
                        await SetDefaultActiveFirstItemAsync();
                    }
                    else
                    {
                        await ClearSelectedAsync();
                    }
                }
                else
                {
                    _waitingForStateChange = true;

                    InvokeValuesChanged();
                }
            }
            else if (DefaultActiveFirstOption)
            {
                await SetDefaultActiveFirstItemAsync();
            }
            else
            {
                await ClearSelectedAsync();
            }
            _defaultValueApplied = true;
        }

        /// <summary>
        /// Sets the initial values after initialization, the method should only called once.
        /// </summary>
        private void SetInitialValuesAsync(SelectOptionItem<TItemValue, TItem> selectOptionItem)
        {
            if (Mode == SelectMode.Default)
            {
                if (ActiveOption == null && (_selectedValue != null || TypeDefaultExistsAsSelectOption))
                {
                    if (EqualityComparer<TItemValue>.Default.Equals(selectOptionItem.Value, _selectedValue))
                    {
                        if (selectOptionItem.IsDisabled)
                        {
                            //await TrySetDefaultValueAsync();
                            return;
                        }

                        selectOptionItem.IsSelected = true;
                        ActiveOption = selectOptionItem;
                        if (HideSelected)
                            selectOptionItem.IsHidden = true;
                        SelectedOptionItems.Add(selectOptionItem.Value, selectOptionItem);
                    }
                }
            }
            else
            {
                if (_selectedValues != null)
                {
                    foreach (var value in _selectedValues)
                    {
                        var result = EqualityComparer<TItemValue>.Default.Equals(selectOptionItem.Value, value);

                        // A disabled option cannot be newly selected by interaction, but a value
                        // supplied through Value/DefaultValues remains selected (matching React Select).
                        if (result)
                        {
                            selectOptionItem.IsSelected = true;

                            if (HideSelected)
                                selectOptionItem.IsHidden = true;

                            SelectedOptionItems.Add(selectOptionItem.Value, selectOptionItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Append a label item in tag mode
        /// </summary>
        /// <param name="label"></param>
        private SelectOptionItem<TItemValue, TItem> AppendLabelValue(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return default;
            SelectOptionItem<TItemValue, TItem> newItem = CreateSelectOptionItem(label, true);
            SelectOptionItems.Add(newItem);
            return newItem;
        }

        protected virtual string GetLabel(TItem item)
        {
            return item.ToString();
        }

        #region Events

        protected override async Task OnValueChangeAsync(TItemValue value)
        {
            if (!_optionsHasInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
                return;

            // An option can finish registering after an uncontrolled DefaultValue has
            // already been cleared. Ignore that stale initialization callback, while
            // still accepting a new user/parent value once Value has actually changed.
            if (_defaultValueWasCleared
                && !TypeDefaultExistsAsSelectOption
                && !_hasValueOnClear
                && !EqualityComparer<TItemValue>.Default.Equals(value, Value))
            {
                return;
            }
            if (_defaultValueWasCleared && !EqualityComparer<TItemValue>.Default.Equals(value, default))
            {
                _defaultValueWasCleared = false;
            }

            if (!_isValueEnum && !TypeDefaultExistsAsSelectOption && EqualityComparer<TItemValue>.Default.Equals(value, default))
            {
                await InvokeAsync(() => ClearInputAsync(new(), focusInput: false));
                return;
            }

            var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, value));

            if (result == null)
            {
                if (SelectOptions is not null)
                {
                    LastValueBeforeReset = value;
                }

                if (!IsAllowClearEnabled)
                {
                    await TrySetDefaultValueAsync();
                }
                else
                {
                    // Reset value if not found - needed if value changed outside of the component.
                    // In some scenarios (e.g., enum flags handled by EnumSelect) the incoming
                    // Value setter will populate the Values collection which then asynchronously
                    // populates SelectedOptionItems. Yielding once here gives that path a chance
                    // to run and populate SelectedOptionItems so we don't clear them erroneously.
                    await Task.Yield();

                    if (!SelectedOptionItems.Any())
                    {
                        await InvokeAsync(() => ClearInputAsync(new(), focusInput: false));
                    }
                }
                return;
            }

            if (result.IsDisabled)
            {
                await TrySetDefaultValueAsync();
                return;
            }

            result.IsSelected = true;

            EvaluateValueChangedOutsideComponent(result, value);

            if (HideSelected)
                result.IsHidden = true;

            await ValueChanged.InvokeAsync(result.Value);
        }

        /// <summary>
        /// When bind-Value is changed outside of the component, then component
        /// selected items have to be reselected according to new value passed.
        /// </summary>
        /// <param name="optionItem">The option item that has been selected.</param>
        /// <param name="value">The value of the selected option item.</param>
        private void EvaluateValueChangedOutsideComponent(SelectOptionItem<TItemValue, TItem> optionItem, TItemValue value)
        {
            //cover special case scenario when SelectItem with Value == default(TItemValue) is requested
            if (TypeDefaultExistsAsSelectOption
                && ActiveOption != null
                && EqualityComparer<TItemValue>.Default.Equals(value, default)
                && EqualityComparer<TItemValue>.Default.Equals(ActiveOption.Value, value))
            {
                return; //already selected, no need to evaluate further
            }

            if (ActiveOption != null && !ActiveOption.Value.Equals(value))
            {
                ActiveOption.IsSelected = false;
                ActiveOption = optionItem;
            }
            if (SelectedOptionItems.Count > 0)
            {
                var oldSelectedValue = SelectedOptionItems.Values.First();
                if (!oldSelectedValue.Value.Equals(value))
                {
                    oldSelectedValue.IsSelected = false;
                    SelectedOptionItems.Remove(oldSelectedValue.Value);
                }
            }
            SelectedOptionItems[optionItem.Value] = optionItem;
        }

        /// <summary>
        /// Method is called via EventCallBack if the value of the Input element was changed by keyboard
        /// </summary>
        /// <param name="e">Contains the value of the Input element</param>
        protected override async Task OnInputAsync(ChangeEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!IsSearchEnabled)
            {
                return;
            }

            if (!_dropDown.IsOverlayShow())
            {
                await _dropDown.Show();
            }

            bool containsToken = false;
            IEnumerable<string> customTokens = null;
            _prevSearchValue = _searchValue;
            if (_isToken)
                _searchValue = e.Value?.ToString().TrimEnd(TokenSeparatorCharacters);
            else
            {
                _searchValue = e.Value?.ToString();

                if (TokenSeparatorCharacters is { Length: > 0 })
                {
                    containsToken = TokenSeparatorCharacters.Any(t => _searchValue.Contains(t));
                }

                if (TokenSeparatorTokenizer is not null && Mode is SelectMode.Tags or SelectMode.Multiple)
                {
                    var tokenizedInput = _searchValue ?? string.Empty;
                    var parsedTokens = TokenSeparatorTokenizer(tokenizedInput)?.ToArray();
                    var isUnchangedInput = parsedTokens is { Length: 1 }
                        && string.Equals(parsedTokens[0], tokenizedInput, StringComparison.Ordinal);
                    if (parsedTokens is { Length: > 0 } && !isUnchangedInput)
                    {
                        customTokens = parsedTokens.Where(token => !string.IsNullOrWhiteSpace(token)).ToArray();
                    }
                    containsToken |= customTokens?.Any() == true;
                }
            }

            //_inputWidth = string.IsNullOrEmpty(_searchValue) ? InputDefaultWidth : $"{4 + _searchValue.Length * 8}px";

            if (containsToken)
            {
                if (customTokens?.Any() == true)
                {
                    await TokenizeValuesAsync(customTokens);
                    _searchValue = string.Empty;
                    UnhideSelectOptions();
                }
                else
                {
                    await TokenizeSearchedPhrase(_searchValue);
                }
            }

            if (!string.IsNullOrEmpty(_searchValue))
            {
                if (EffectiveFilterOption)
                {
                    FilterOptionItems(_searchValue);
                }
                else
                {
                    UnhideSelectOptions();
                    ActiveOption = SortedSelectOptionItems.FirstOrDefault(option => !option.IsHidden && !option.IsDisabled);
                }
            }
            else
            {
                UnhideSelectOptions();
                if (Mode == SelectMode.Tags && CustomTagSelectOptionItem is not null)
                {
                    SelectOptionItems.Remove(CustomTagSelectOptionItem);
                    CustomTagSelectOptionItem = null;
                }
            }
            OnSearch?.Invoke(_searchValue);
            if (ShowSearchOptions != null)
            {
                ShowSearchOptions.OnSearch?.Invoke(_searchValue);
            }
            await SearchValueChanged.InvokeAsync(_searchValue);
            // The portaled Overlay owns its own render tree. Updating option
            // children alone leaves its list/empty-state fragment stale.
            _dropDown.GetOverlayComponent()?.RefreshComponentState();
        }

        private async Task TokenizeSearchedPhrase(string searchValue)
        {
            await TokenizeValuesAsync(searchValue.Split(TokenSeparatorCharacters));
        }

        private async Task TokenizeValuesAsync(IEnumerable<string> values)
        {
            var tokenItemMatch = values
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Distinct()
                .ToDictionary(item => item, _ => default(SelectOptionItem<TItemValue, TItem>));

            if (Mode == SelectMode.Tags)
            {
                List<SelectOptionItem<TItemValue, TItem>> selectOptionItems;
                if (AddedTags.Count > 0)
                {
                    selectOptionItems = SelectOptionItems.ToList();
                    selectOptionItems.AddRange(AddedTags);
                }
                else
                    selectOptionItems = SelectOptionItems.ToList();

                foreach (var item in selectOptionItems)
                {
                    if (tokenItemMatch.ContainsKey(item.Label))
                    {
                        await SetValueAsync(item);
                        tokenItemMatch[item.Label] = item;
                    }
                }

                foreach (KeyValuePair<string, SelectOptionItem<TItemValue, TItem>> tokenItem in tokenItemMatch)
                {
                    if (tokenItem.Value == null)
                    {
                        tokenItemMatch[tokenItem.Key] = CreateSelectOptionItem(tokenItem.Key, false);
                        SelectOptionItems.Add(tokenItemMatch[tokenItem.Key]);
                        await SetValueAsync(tokenItemMatch[tokenItem.Key]);
                    }
                }
            }
            else
            {
                foreach (var item in SelectOptionItems)
                {
                    if (tokenItemMatch.ContainsKey(item.Label))
                    {
                        await SetValueAsync(item);
                    }
                }
            }
            if (_dropDown.IsOverlayShow())
            {
                await CloseAsync();
            }
            await SetInputBlurAsync();
        }

        private void FilterOptionItems(string searchValue)
        {
            if (Mode != SelectMode.Tags)
            {
                SelectOptionItem<TItemValue, TItem> firstActiveOption = null;
                foreach (var item in SelectOptionItems)
                {
                    bool matches = false;
                    try
                    {
                        matches = MatchesSearchOption(item, searchValue);
                    }
                    catch
                    {
                        // If filter expression fails, default to false
                        matches = false;
                    }

                    if (matches)
                    {
                        item.IsHidden = item.IsSelected && HideSelected;
                        if (firstActiveOption == null && !item.IsHidden && !item.IsDisabled)
                            firstActiveOption = item;
                    }
                    else
                    {
                        if (!item.IsHidden)
                        {
                            item.IsHidden = true;
                        }
                        item.IsActive = false;
                    }
                }

                // Keep the input's aria-activedescendant in sync with the
                // first visible search result, matching React's Select after
                // filtering. Updating only IsActive leaves ActiveOptionId
                // unset even though the option itself appears active.
                ActiveOption = firstActiveOption;
            }
            else
            {
                FilterTagsOptionItems(searchValue);
            }
        }

        private bool MatchesOptionFilter(SelectOptionItem<TItemValue, TItem> option, string searchValue)
        {
            IEnumerable<string> fields;
            var optionFilterProp = EffectiveOptionFilterProp;
            if (optionFilterProp.IsT1)
            {
                fields = optionFilterProp.AsT1;
            }
            else if (!string.IsNullOrWhiteSpace(optionFilterProp.AsT0))
            {
                fields = new[] { optionFilterProp.AsT0 };
            }
            else
            {
                fields = new[] { "value" };
            }

            foreach (var field in fields ?? Array.Empty<string>())
            {
                if (string.IsNullOrWhiteSpace(field))
                {
                    continue;
                }

                object fieldValue;
                if (string.Equals(field, "label", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(field, "children", StringComparison.OrdinalIgnoreCase))
                {
                    fieldValue = option.Label;
                }
                else if (string.Equals(field, "value", StringComparison.OrdinalIgnoreCase))
                {
                    fieldValue = option.Value;
                }
                else
                {
                    fieldValue = PathHelper.GetDelegate<TItem, object>(field)(option.Item);
                }

                if (fieldValue?.ToString()?.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private bool MatchesSearchOption(SelectOptionItem<TItemValue, TItem> option, string searchValue)
        {
            if (ShowSearchFilterPredicate != null)
            {
                return ShowSearchFilterPredicate(searchValue, option);
            }

            return FilterExpression != null
                ? FilterExpression(option, searchValue)
                : MatchesOptionFilter(option, searchValue);
        }

        private void FilterTagsOptionItems(string searchValue)
        {
            SelectOptionItem<TItemValue, TItem> activeCanditate = null;
            List<SelectOptionItem<TItemValue, TItem>> selectOptionItems;
            if (AddedTags.Count > 0)
            {
                selectOptionItems = SelectOptionItems.ToList();
                selectOptionItems.AddRange(AddedTags);
            }
            else
                selectOptionItems = SelectOptionItems.ToList();

            foreach (var item in selectOptionItems)
            {
                if (!(CustomTagSelectOptionItem != null && item.Equals(CustomTagSelectOptionItem))) //ignore if analyzing CustomTagSelectOptionItem
                {
                    bool matches = false;
                    try
                    {
                        matches = MatchesSearchOption(item, searchValue);
                    }
                    catch
                    {
                        // If filter expression fails, default to false
                        matches = false;
                    }

                    if (matches)
                    {
                        if (item.Label.Equals(searchValue, StringComparison.InvariantCulture))
                        {
                            activeCanditate = item;
                            ActiveOption = item;
                            item.IsActive = true;
                            if (CustomTagSelectOptionItem != null)
                            {
                                SelectOptionItems.Remove(CustomTagSelectOptionItem);
                                CustomTagSelectOptionItem = null;
                            }
                        }
                        else
                        {
                            item.IsHidden = false;
                            if (activeCanditate == null)
                            {
                                activeCanditate = item;
                                ActiveOption = item;
                                item.IsActive = true;
                            }
                        }
                    }
                    else
                    {
                        item.IsHidden = true;
                        item.IsActive = false;
                    }
                }
            }

            if (activeCanditate == null && !string.IsNullOrWhiteSpace(searchValue))
            {
                if (Mode == SelectMode.Tags)
                {
                    CustomTagSelectOptionItem = CreateSelectOptionItem(searchValue, true);
                    SelectOptionItems.Add(CustomTagSelectOptionItem);
                    CustomTagSelectOptionItem.IsActive = true;
                    ActiveOption = CustomTagSelectOptionItem;
                }
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the keyboard key is no longer pressed inside the Input element.
        /// </summary>
        /// <param name="e">Contains the key (combination) which was pressed inside the Input element</param>
        protected override async Task OnKeyUpAsync(KeyboardEventArgs e)
        {
            if (e?.Key == null)
            {
                return;
            }

            var key = e.Key.ToUpperInvariant();
            var overlayFirstOpen = false;

            if (_isToken && Mode == SelectMode.Tags)
            {
                if (!_dropDown.IsOverlayShow())
                    return;

                if (!SelectOptionItems.Any())
                    return;

                SelectOptionItem<TItemValue, TItem> firstActive;
                if (ActiveOption.IsAddedTag)
                {
                    firstActive = SelectOptionItems.FirstOrDefault(x => x.Value.Equals(ActiveOption.Value));
                    if (firstActive is null)
                        firstActive = ActiveOption;
                }
                else
                    firstActive = ActiveOption; // SelectOptionItems.FirstOrDefault(x => x.IsActive);

                if (AllOptionsHidden() || firstActive is null)
                {
                    var newItem = AppendLabelValue(_searchValue);

                    await SetValueAsync(newItem);

                    OnCreateCustomTag?.Invoke(_searchValue);
                }
                else if (firstActive != null && !firstActive.IsDisabled)
                {
                    CustomTagSelectOptionItem = null;
                    await SetValueAsync(firstActive);
                }

                await ClearSearch();

                return;
            }

            if (key == "ENTER")
            {
                if (!_dropDown.IsOverlayShow())
                    return;

                if (!SelectOptionItems.Any())
                    return;

                if (Mode == SelectMode.Default)
                {
                    var firstActive = SelectOptionItems.FirstOrDefault(x => x.IsActive);

                    if (firstActive != null)
                    {
                        if (!firstActive.IsDisabled)
                        {
                            await SetValueAsync(firstActive);
                            await CloseAsync();
                        }
                    }

                    return;
                }

                if (Mode == SelectMode.Multiple)
                {
                    if (AllOptionsHidden())
                        return;

                    var firstActive = SelectOptionItems.FirstOrDefault(x => x.IsActive);

                    if (firstActive != null && !firstActive.IsDisabled)
                    {
                        await SetValueAsync(firstActive);
                    }
                    return;
                }

                if (Mode == SelectMode.Tags)
                {
                    SelectOptionItem<TItemValue, TItem> firstActive;
                    if (ActiveOption.IsAddedTag)
                    {
                        firstActive = SelectOptionItems.FirstOrDefault(x => x.Value.Equals(ActiveOption.Value));
                        if (firstActive is null)
                            firstActive = ActiveOption;
                    }
                    else
                        firstActive = ActiveOption;

                    if (AllOptionsHidden() || firstActive is null)
                    {
                        var newItem = AppendLabelValue(_searchValue);
                        await SetValueAsync(newItem);
                        OnCreateCustomTag?.Invoke(_searchValue);
                    }
                    else if (firstActive != null && !firstActive.IsDisabled)
                    {
                        await SetValueAsync(firstActive);
                    }

                    return;
                }
            }

            if (key == "ARROWUP")
            {
                if (!_dropDown.IsOverlayShow() && !Disabled)
                {
                    await _dropDown.Show();
                    overlayFirstOpen = true;
                }

                if (!SelectOptionItems.Any())
                    return;

                var sortedSelectOptionItems = SortedSelectOptionItems.ToList();

                if (overlayFirstOpen)
                {
                    // Check if there is a selected item and set it as active
                    var currentSelected = sortedSelectOptionItems.FirstOrDefault(x => x.IsSelected);

                    if (currentSelected != null)
                    {
                        if (currentSelected.IsActive)
                            return;

                        sortedSelectOptionItems.Where(x => x.IsActive)
                            .ForEach(i => i.IsActive = false);

                        currentSelected.IsActive = true;
                        ActiveOption = currentSelected;

                        await ElementScrollIntoViewAsync(currentSelected.Ref);
                    }

                    return;
                }

                var firstActive = sortedSelectOptionItems.FirstOrDefault(x => x.IsActive && !x.IsHidden && !x.IsDisabled);

                if (firstActive == null)
                {
                    var firstOption = sortedSelectOptionItems.FirstOrDefault(x => !x.IsHidden && !x.IsDisabled);

                    if (firstOption != null)
                    {
                        firstOption.IsActive = true;
                        ActiveOption = firstOption;

                        await ElementScrollIntoViewAsync(firstOption.Ref);
                    }
                }
                else
                {
                    var possibilityCount = sortedSelectOptionItems.Where(x => !x.IsHidden && !x.IsDisabled).Count();

                    if (possibilityCount == 1) // Do nothing if there is only one choice
                        return;

                    var index = sortedSelectOptionItems.FindIndex(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, firstActive.Value));

                    index--;

                    int nextIndex;

                    if (index == -1)
                    {
                        nextIndex = sortedSelectOptionItems.FindLastIndex(x => !x.IsHidden && !x.IsDisabled);
                    }
                    else
                    {
                        nextIndex = sortedSelectOptionItems.FindIndex(index, x => !x.IsHidden && !x.IsDisabled);

                        if (nextIndex != index)
                        {
                            for (var i = index; i >= -1; i--)
                            {
                                if (i < 0)
                                {
                                    nextIndex = sortedSelectOptionItems.FindLastIndex(x => !x.IsHidden && !x.IsDisabled);
                                    break;
                                }

                                if (!sortedSelectOptionItems[i].IsHidden && !sortedSelectOptionItems[i].IsDisabled)
                                {
                                    nextIndex = i;
                                    break;
                                }
                            }
                        }
                    }

                    if (nextIndex == -1)
                        return;

                    // Prevent duplicate active items if search has no value
                    sortedSelectOptionItems.Where(x => x.IsActive)
                        .ForEach(x => x.IsActive = false);

                    sortedSelectOptionItems[nextIndex].IsActive = true;
                    ActiveOption = sortedSelectOptionItems[nextIndex];
                    await ElementScrollIntoViewAsync(sortedSelectOptionItems[nextIndex].Ref);
                }
            }

            if (key == "ARROWDOWN")
            {
                if (!_dropDown.IsOverlayShow() && !Disabled)
                {
                    await _dropDown.Show();
                    overlayFirstOpen = true;
                }

                if (!SelectOptionItems.Any())
                    return;

                var sortedSelectOptionItems = SortedSelectOptionItems.ToList();

                if (overlayFirstOpen)
                {
                    // Check if there is a selected item and set it as active
                    var currentSelected = sortedSelectOptionItems.FirstOrDefault(x => x.IsSelected);

                    if (currentSelected != null)
                    {
                        if (currentSelected.IsActive)
                            return;

                        sortedSelectOptionItems.Where(x => x.IsActive)
                            .ForEach(i => i.IsActive = false);

                        currentSelected.IsActive = true;
                        ActiveOption = currentSelected;

                        await ElementScrollIntoViewAsync(currentSelected.Ref);
                    }

                    return;
                }

                var firstActive = sortedSelectOptionItems.FirstOrDefault(x => x.IsActive && !x.IsHidden && !x.IsDisabled);

                if (firstActive == null)
                {
                    var firstOption = sortedSelectOptionItems.FirstOrDefault(x => !x.IsHidden && !x.IsDisabled);

                    if (firstOption != null)
                    {
                        firstOption.IsActive = true;
                        ActiveOption = firstOption;
                    }
                }
                else
                {
                    var possibilityCount = sortedSelectOptionItems.Count(x => !x.IsHidden && !x.IsDisabled);

                    if (possibilityCount == 1) // Do nothing if there is only one choice
                        return;

                    var index = sortedSelectOptionItems.FindIndex(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, firstActive.Value));

                    index++;

                    var nextIndex = sortedSelectOptionItems.FindIndex(index, x => !x.IsHidden && !x.IsDisabled);

                    if (nextIndex == -1) // Maybe the next item is above the current active item
                    {
                        nextIndex = sortedSelectOptionItems.FindIndex(0, x => !x.IsHidden && !x.IsDisabled); // Try to find the index from the first available item
                    }

                    if (nextIndex == -1)
                        return;

                    // Prevent duplicate active items if search has no value
                    sortedSelectOptionItems.Where(x => x.IsActive)
                        .ForEach(x => x.IsActive = false);

                    sortedSelectOptionItems[nextIndex].IsActive = true;
                    ActiveOption = sortedSelectOptionItems[nextIndex];
                    await ElementScrollIntoViewAsync(sortedSelectOptionItems[nextIndex].Ref);
                }
            }

            if (key == "HOME")
            {
                if (_dropDown.IsOverlayShow())
                {
                    if (!SelectOptionItems.Any())
                        return;

                    var sortedSelectOptionItems = SortedSelectOptionItems.ToList();

                    var index = sortedSelectOptionItems.FindIndex(0, x => !x.IsHidden && !x.IsDisabled);

                    if (index == -1)
                        return;

                    // Prevent duplicate active items if search has no value
                    sortedSelectOptionItems.Where(x => x.IsActive)
                        .ForEach(i => i.IsActive = false);

                    sortedSelectOptionItems[index].IsActive = true;
                    ActiveOption = sortedSelectOptionItems[index];

                    await ElementScrollIntoViewAsync(sortedSelectOptionItems[index].Ref);
                }
            }

            if (key == "END")
            {
                if (_dropDown.IsOverlayShow())
                {
                    if (!SelectOptionItems.Any())
                        return;

                    var sortedSelectOptionItems = SortedSelectOptionItems.ToList();

                    var index = sortedSelectOptionItems.FindLastIndex(x => !x.IsHidden && !x.IsDisabled);

                    if (index == -1)
                        return;

                    // Prevent duplicate active items if search has no value
                    sortedSelectOptionItems.Where(x => x.IsActive)
                        .ForEach(i => i.IsActive = false);

                    sortedSelectOptionItems[index].IsActive = true;
                    ActiveOption = sortedSelectOptionItems[index];
                    await ElementScrollIntoViewAsync(sortedSelectOptionItems[index].Ref);
                }
            }

            if (key == "ESCAPE")
            {
                if (_dropDown.IsOverlayShow())
                {
                    await CloseAsync();
                }
            }

            if (key == "BACKSPACE" && string.IsNullOrEmpty(_searchValue) &&
                (IsSearchEnabled || IsAllowClearEnabled))
            {
                // Don't remove selected items if IME composition is in progress
                if (_selectContent?.IsComposing == true)
                {
                    return;
                }
                
                var orderedSelectedItems = GetOrderedSelectedItems();
                if (string.IsNullOrEmpty(_prevSearchValue) && orderedSelectedItems.Count > 0)
                {
                    var removable = orderedSelectedItems.LastOrDefault(item => !item.IsDisabled);
                    if (!Disabled && removable != null)
                    {
                        await SetValueAsync(removable);
                        FocusIfInSearch();
                    }
                }
                else if (!string.IsNullOrEmpty(_prevSearchValue))
                    _prevSearchValue = _searchValue;
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if a key is pressed inside Input element.
        /// The method is used to get the TAB event if the user press TAB to cycle trough elements.
        /// If a TAB is received, the overlay will be closed and the Input element blures.
        /// </summary>
        protected override async Task OnKeyDownAsync(KeyboardEventArgs e)
        {
            if (e?.Key == null)
            {
                return;
            }

            await OnInputKeyDown.InvokeAsync(e);

            var key = e.Key.ToUpperInvariant();

            if (key == "TAB")
            {
                if (_dropDown.IsOverlayShow())
                {
                    await CloseAsync();
                }

                await SetInputBlurAsync();
            }
            else if (key == "ESCAPE" && _dropDown.IsOverlayShow())
            {
                await CloseAsync();
            }
            else if (TokenSeparatorCharacters is { Length: > 0 })
            {
                _isToken = TokenSeparatorCharacters.Contains(e.Key[0]);
            }
        }

        private Task OnPopupScrollAsync(EventArgs args) => OnPopupScroll.InvokeAsync(args);

        /// <summary>
        /// Check if Focused property is true;  Set the Focused property to false, change the
        /// style and blures the Input element via DOM.
        /// </summary>
        /// <returns></returns>
        protected override async Task SetInputBlurAsync()
        {
            if (Focused)
            {
                Focused = false;
                SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Blur, _inputRef);

            }
        }

        /// <summary>
        /// Search the first selected item, set IsActive to False for all other items and call the scrollIntoView function via JavaScript.
        /// The method is used to scroll to the first selected item after opening the overlay.
        /// </summary>
        protected async Task ScrollToFirstSelectedItemAsync()
        {
            // Check if there is a selected item and set it as active
            // React only scrolls a single selector to its selected option.
            // Multiple selectors must not activate a disabled selected tag.
            var currentSelected = Mode == SelectMode.Default
                ? SortedSelectOptionItems.FirstOrDefault(x => x.IsSelected && !x.IsHidden)
                : null;

            if (currentSelected != null)
            {
                SelectOptionItems.Where(x => x.IsActive)
                    .ForEach(i => i.IsActive = false);

                currentSelected.IsActive = true;
                ActiveOption = currentSelected;

                //it can happen that Ref.Id is null when for example DataSource
                //is set in first OnAfterRender() after initialization
                if (currentSelected.Ref.Id is null)
                {
                    await InvokeAsync(StateHasChanged);
                }
                await ElementScrollIntoViewAsync(currentSelected.Ref);
            }
            else if (UsesVirtualWindow)
            {
                // React opens a virtual multi-select with the first enabled
                // result active, even when the selection contains later items.
                // Keep the accessibility window and visible virtual row in
                // sync with that initial active descendant.
                var firstOption = SortedSelectOptionItems.FirstOrDefault(x => !x.IsHidden && !x.IsDisabled);
                if (firstOption != null)
                    ActiveOption = firstOption;
            }
            else if (ActiveOption == null || ActiveOption.IsHidden || ActiveOption.IsDisabled)
            {
                var selectionCandidate = SortedSelectOptionItems.FirstOrDefault(x => !x.IsHidden && !x.IsDisabled);
                if (selectionCandidate != null)
                {
                    ActiveOption = selectionCandidate;
                }
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the user clicked on the Close icon of a Tag.
        /// </summary>
        protected override async Task OnRemoveSelectedAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));
            if (Disabled || selectOption.IsDisabled) return;
            await SetValueAsync(selectOption);
            FocusIfInSearch();
        }

        protected override SelectOptionItem<TItemValue, TItem> CreateSelectOptionItem(string label, bool isActive)
        {
            TItemValue value = default;
            TItem item = default;

            try
            {
                if (!string.IsNullOrEmpty(label))
                {
                    if (typeof(TItem) == typeof(string))
                    {
                        item = (TItem)(object)label;
                        value = (TItemValue)(object)label;
                    }
                    else if (DataSource != null)
                    {
                        // Try to find matching item from DataSource
                        item = _getLabel != null
                            ? DataSource.FirstOrDefault(x => _getLabel(x).Equals(label, StringComparison.InvariantCultureIgnoreCase))
                            : DataSource.FirstOrDefault(x => x.ToString().Equals(label, StringComparison.InvariantCultureIgnoreCase));

                        if (item != null)
                        {
                            value = _getValue != null ? _getValue(item) :
                                   typeof(TItemValue) == typeof(TItem) ? (TItemValue)(object)item :
                                   default;
                        }
                    }

                    // Try custom tag conversion if no item found
                    if (item == null && Mode == SelectMode.Tags && CustomTagLabelToValue != null)
                    {
                        try
                        {
                            value = CustomTagLabelToValue(label);
                        }
                        catch
                        {
                            value = default;
                        }
                    }
                }
            }
            catch
            {
                value = default;
                item = default;
            }

            return new SelectOptionItem<TItemValue, TItem>
            {
                Label = label ?? string.Empty,
                Value = value,
                Item = item,
                IsActive = isActive,
                IsSelected = false,
                IsAddedTag = true
            };
        }

        #endregion Events
    }
}
