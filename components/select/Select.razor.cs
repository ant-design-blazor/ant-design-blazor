using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using AntDesign.Select;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

#pragma warning disable 1591 // Disable missing XML comment
#pragma warning disable CA1716 // Disable Select name warning
#pragma warning disable CA1305 // IFormatProvider warning

namespace AntDesign
{
    public partial class Select<TItemValue, TItem> : AntInputComponentBase<TItemValue>
    {
        #region Parameters

        [Parameter] public bool AllowClear { get; set; }
        [Parameter] public bool AutoClearSearchValue { get; set; } = true;
        [Parameter] public bool Bordered { get; set; } = true;
        [Parameter] public Action<string> OnCreateCustomTag { get; set; }

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

        [Parameter] public bool Disabled { get; set; }

        [Parameter]
        public string DisabledName
        {
            get => _disabledName;
            set
            {
                _getDisabled = string.IsNullOrWhiteSpace(value) ? null : SelectItemPropertyHelper.CreateGetDisabledFunc<TItem>(value);
                _disabledName = value;
            }
        }

        [Parameter] public Func<RenderFragment, RenderFragment> DropdownRender { get; set; }
        [Parameter] public bool EnableSearch { get; set; }

        [Parameter]
        public string GroupName
        {
            get => _groupName;
            set
            {
                _getGroup = string.IsNullOrWhiteSpace(value) ? null : SelectItemPropertyHelper.CreateGetGroupFunc<TItem>(value);
                _groupName = value;
            }
        }

        [Parameter] public bool HideSelected { get; set; }
        [Parameter] public bool IgnoreItemChanges { get; set; } = true;
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

        /// <summary>
        /// LabelInValue can only be used if the SelectOption is not created by DataSource.
        /// </summary>
        [Parameter] public bool LabelInValue { get; set; }

        [Parameter]
        public string LabelName
        {
            get => _labelName;
            set
            {
                _getLabel = SelectItemPropertyHelper.CreateGetLabelFunc<TItem>(value);
                if (SelectMode == SelectMode.Tags)
                {
                    _setLabel = SelectItemPropertyHelper.CreateSetLabelFunc<TItem>(value);
                }
                _labelName = value;
            }
        }

        [Parameter] public RenderFragment<TItem> LabelTemplate { get; set; }
        [Parameter] public bool Loading { get; set; }

        /// <summary>
        /// How long (number of characters) a tag will be.
        /// Only for Mode = "multiple" or Mode = "tags"
        /// </summary>
        /// <value>
        /// The maximum length of the tag text.
        /// </value>
        [Parameter] public int MaxTagTextLength { get; set; }

        private OneOf<int, ResponsiveTag> _maxTagCount;
        [Parameter]
        public OneOf<int, ResponsiveTag> MaxTagCount
        {
            get { return _maxTagCount; }
            set {
                _maxTagCount = value;

                value.Switch(intValue =>
                {
                    IsResponsive = false;
                    HasTagCount = intValue > 0;
                }, enumValue =>
                {
                    IsResponsive = enumValue == ResponsiveTag.Responsive;
                    HasTagCount = false;
                });
            }
        }
        internal bool IsResponsive { get; set; }
        internal bool HasTagCount { get; set; }
        [Parameter] public RenderFragment<IEnumerable<TItem>> MaxTagPlaceholder { get; set; }
        [Parameter] public string Mode { get; set; } = "default";
        [Parameter] public RenderFragment NotFoundContent { get; set; }
        [Parameter] public Action OnBlur { get; set; }
        [Parameter] public Action OnClearSelected { get; set; }

        /// <summary>
        /// The OnDataSourceChanged event occurs after the DataSource property changes.
        /// </summary>
        /// <remarks>
        /// It does not trigger if a value inside the IEnumerable&lt;TItem&gt; changes.
        /// </remarks>
        [Parameter] public Action OnDataSourceChanged { get; set; }

        [Parameter] public Action<bool> OnDropdownVisibleChange { get; set; }
        [Parameter] public Action OnFocus { get; set; }
        [Parameter] public Action OnMouseEnter { get; set; }
        [Parameter] public Action OnMouseLeave { get; set; }
        [Parameter] public Action<string> OnSearch { get; set; }
        [Parameter] public Action<TItem> OnSelectedItemChanged { get; set; }
        [Parameter] public Action<IEnumerable<TItem>> OnSelectedItemsChanged { get; set; }
        [Parameter] public bool Open { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string PopupContainerMaxHeight { get; set; } = "256px";
        [Parameter] public string PopupContainerSelector { get; set; } = "body";
        [Parameter] public OneOf<bool, string> DropdownMatchSelectWidth { get; set; } = true;
        [Parameter] public string DropdownMaxWidth { get; set; } = "auto";

        private bool _showArrowIconChanged;
        [Parameter]
        public bool ShowArrowIcon
        {
            get { return _showArrowIcon; }
            set {
                _showArrowIcon = value;
                _showArrowIconChanged = true;
            }
        }
        [Parameter] public bool ShowSearchIcon { get; set; } = true;
        [Parameter] public SortDirection SortByGroup { get; set; } = SortDirection.None;
        [Parameter] public SortDirection SortByLabel { get; set; } = SortDirection.None;
        [Parameter] public RenderFragment SuffixIcon { get; set; }
        [Parameter] public RenderFragment PrefixIcon { get; set; }
        [Parameter] public char[] TokenSeparators { get; set; }
        [Parameter] public override EventCallback<TItemValue> ValueChanged { get; set; }

        [Parameter]
        public string ValueName
        {
            get => _valueName;
            set
            {
                _getValue = SelectItemPropertyHelper.CreateGetValueFunc<TItem, TItemValue>(value);
                _setValue = SelectItemPropertyHelper.CreateSetValueFunc<TItem, TItemValue>(value);
                _valueName = value;
            }
        }

        [Parameter] public EventCallback<IEnumerable<TItemValue>> ValuesChanged { get; set; }

        /// <summary>
        /// Converts custom tag (a string) to TItemValue type.
        /// </summary>
        [Parameter]
        public Func<string, TItemValue> CustomTagLabelToValue { get; set; } =
            (label) => (TItemValue)TypeDescriptor.GetConverter(typeof(TItemValue)).ConvertFromInvariantString(label);

        [Parameter]
        public IEnumerable<TItem> DataSource
        {
            get => _datasource;
            set
            {
                if (value == null && _datasource == null)
                {
                    return;
                }

                if (value == null && _datasource != null)
                {
                    if (!_isInitialized)
                    {
                        _selectedValue = default;
                    }
                    else
                    {
                        SelectOptionItems.Clear();
                        SelectedOptionItems.Clear();
                        Value = default;

                        _datasource = null;

                        OnDataSourceChanged?.Invoke();
                    }
                    return;
                }

                if (value != null && !value.Any() && SelectOptionItems.Any())
                {
                    SelectOptionItems.Clear();
                    SelectedOptionItems.Clear();

                    Value = default;

                    _datasource = value;

                    OnDataSourceChanged?.Invoke();

                    return;
                }

                if (value != null)
                {
                    bool hasChanged;

                    if (_datasource == null)
                    {
                        hasChanged = true;
                    }
                    else
                    {
                        hasChanged = !value.SequenceEqual(_datasource);
                    }

                    if (hasChanged)
                    {
                        OnDataSourceChanged?.Invoke();

                        _datasource = value;
                    }
                }
            }
        }

        [Parameter]
        public override TItemValue Value
        {
            get => _selectedValue;
            set
            {
                var hasChanged = !EqualityComparer<TItemValue>.Default.Equals(value, _selectedValue);
                if (hasChanged)
                {
                    _selectedValue = value;
                    if (_isInitialized)
                    {
                        OnValueChange(value);
                        if (Form?.ValidateOnChange == true)
                        {
                            EditContext?.NotifyFieldChanged(FieldIdentifier);
                        }
                    }
                }
            }
        }

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

        [Parameter]
        public IEnumerable<TItemValue> Values
        {
            get => _selectedValues;
            set
            {
                if (value != null && _selectedValues != null)
                {
                    var hasChanged = !value.SequenceEqual(_selectedValues);

                    if (!hasChanged)
                        return;

                    _selectedValues = value;
                    _ = OnValuesChangeAsync(value);
                }
                else if (value != null && _selectedValues == null)
                {
                    _selectedValues = value;

                    _ = OnValuesChangeAsync(value);
                }
                else if (value == null && _selectedValues != null)
                {
                    _selectedValues = default;

                    _ = OnValuesChangeAsync(default);
                }
            }
        }

        [Parameter]
        public IEnumerable<TItemValue> DefaultValues
        {
            get => _defaultValues;
            set
            {
                if (value != null && _defaultValues != null)
                {
                    var hasChanged = !value.SequenceEqual(_defaultValues);

                    if (!hasChanged)
                        return;

                    _defaultValuesHasItems = value.Any();
                    _defaultValues = value;
                }
                else if (value != null && _defaultValues == null)
                {
                    _defaultValuesHasItems = value.Any();
                    _defaultValues = value;
                }
                else if (value == null && _defaultValues != null)
                {
                    _defaultValuesHasItems = false;
                    _defaultValues = default;
                }
            }
        }

        [Parameter] public RenderFragment SelectOptions { get; set; }

        #endregion Parameters

        [Inject] private DomEventService DomEventService { get; set; }

        #region Properties

        private const string ClassPrefix = "ant-select";
        private const string DefaultWidth = "width: 100%;";

        /// <summary>
        /// Determines if SelectOptions has any selected items
        /// </summary>
        /// <returns>true if SelectOptions has any selected Items, otherwise false</returns>
        internal bool HasValue
        {
            get => SelectOptionItems.Where(x => x.IsSelected).Any() || (AddedTags?.Any() ?? false);
        }

        /// <summary>
        /// Returns a true/false if the placeholder should be displayed or not.
        /// </summary>
        /// <returns>true if SelectOptions has no values and the searchValue is empty; otherwise false </returns>
        protected bool ShowPlaceholder
        {
            get
            {
                return !HasValue && string.IsNullOrEmpty(_searchValue);
            }
        }

        /// <summary>
        /// Returns the value of EnableSearch parameter
        /// </summary>
        /// <returns>true if search is enabled</returns>
        internal bool IsSearchEnabled
        {
            get => EnableSearch || SelectMode == SelectMode.Tags;
        }

        /// <summary>
        /// Indicates if the GroupName is used. When this value is True, the SelectOptions will be rendered in group mode.
        /// </summary>
        internal bool IsGroupingEnabled
        {
            get => !string.IsNullOrWhiteSpace(GroupName);
        }

        internal ElementReference DropDownRef => _dropDown.GetOverlayComponent().Ref;

        internal SelectMode SelectMode => Mode.ToSelectMode();
        internal bool Focused { get; private set; }
        private string _searchValue = string.Empty;
        private string _prevSearchValue = string.Empty;
        private string _dropdownStyle = string.Empty;
        private TItemValue _selectedValue;
        private TItemValue _defaultValue;
        private bool _defaultValueIsNotNull;
        private IEnumerable<TItem> _datasource;
        private IEnumerable<TItemValue> _selectedValues;
        private IEnumerable<TItemValue> _defaultValues;
        private bool _defaultValuesHasItems;
        private bool _isInitialized;
        private bool _defaultValueApplied;
        private bool _defaultActiveFirstOptionApplied;
        private bool _waittingStateChange;
        private bool _isPrimitive;
        internal ElementReference _inputRef;
        protected OverlayTrigger _dropDown;
        protected SelectContent<TItemValue, TItem> _selectContent;
        private bool _isToken;
        private SelectOptionItem<TItemValue, TItem> _activeOption;
        private bool _defaultActiveFirstOption;

        internal HashSet<SelectOptionItem<TItemValue, TItem>> SelectOptionItems { get; } = new HashSet<SelectOptionItem<TItemValue, TItem>>();
        internal List<SelectOptionItem<TItemValue, TItem>> SelectedOptionItems { get; } = new List<SelectOptionItem<TItemValue, TItem>>();
        internal List<SelectOptionItem<TItemValue, TItem>> AddedTags { get; } = new List<SelectOptionItem<TItemValue, TItem>>();
        internal SelectOptionItem<TItemValue, TItem> CustomTagSelectOptionItem { get; set; }

        /// <summary>
        /// Currently active (highlighted) option.
        /// It does not have to be equal to selected option.
        /// </summary>
        internal SelectOptionItem<TItemValue, TItem> ActiveOption
        {
            get { return _activeOption; }
            set
            {
                if (_activeOption != value)
                {
                    if (_activeOption != null && _activeOption.IsActive)
                        _activeOption.IsActive = false;
                    _activeOption = value;
                    if (_activeOption != null && !_activeOption.IsActive)
                        _activeOption.IsActive = true;
                }
            }
        }

        private string _labelName;

        private Func<TItem, string> _getLabel;

        private Action<TItem, string> _setLabel;

        private string _groupName = string.Empty;

        private Func<TItem, string> _getGroup;

        private string _disabledName;

        private Func<TItem, bool> _getDisabled;

        private string _valueName;

        private Func<TItem, TItemValue> _getValue;

        private Action<TItem, TItemValue> _setValue;
        private bool _disableSubmitFormOnEnter;
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
                    typeof(Guid)
                }.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }

        internal bool IsDropdownShown() => _dropDown.IsOverlayShow();
        protected override void OnInitialized()
        {
            SetClassMap();

            if (string.IsNullOrWhiteSpace(Style))
                Style = DefaultWidth;

            if (!_isInitialized)
            {
                _isPrimitive = IsSimpleType(typeof(TItem));
                if (!_showArrowIconChanged && SelectMode != SelectMode.Default)
                    _showArrowIcon = SuffixIcon != null;
            }
            _isInitialized = true;

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (SelectOptions == null)
                CreateDeleteSelectOptions();

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetInitialValuesAsync();

                DomEventService.AddEventListener("window", "resize", OnWindowResize, false);
                await SetDropdownStyleAsync();

                _defaultValueApplied = !(_defaultValueIsNotNull || _defaultValuesHasItems);
                _defaultActiveFirstOptionApplied = !_defaultActiveFirstOption;
            }

            if (!_defaultValueApplied || !_defaultActiveFirstOptionApplied)
            {
                if (SelectMode == SelectMode.Default)
                {
                    if (_defaultValueIsNotNull && !HasValue && SelectOptionItems.Any()
                        || DefaultActiveFirstOption && !HasValue && SelectOptionItems.Any())
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

            if (_isInitialized && SelectOptions == null)
                CreateDeleteSelectOptions();

            if (_waittingStateChange)
            {
                _waittingStateChange = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>("window", "resize", OnWindowResize);
            base.Dispose(disposing);
        }

        protected async void OnWindowResize(JsonElement element)
        {
            await SetDropdownStyleAsync();
        }

        /// <summary>
        /// Create or delete SelectOption when the datasource changed
        /// </summary>
        private void CreateDeleteSelectOptions()
        {
            if (string.IsNullOrWhiteSpace(ValueName)) throw new ArgumentNullException(nameof(ValueName));
            if (string.IsNullOrWhiteSpace(LabelName)) throw new ArgumentNullException(nameof(LabelName));

            if (_datasource == null)
                return;

            Dictionary<TItem, SelectOptionItem<TItemValue, TItem>> dataStoreToSelectOptionItemsMatch = new();
            // Compare items of SelectOptions and the datastore
            if (SelectOptionItems.Any())
            {
                // Delete items from SelectOptions if it is no longer in the datastore
                for (var i = SelectOptionItems.Count - 1; i >= 0; i--)
                {
                    var selectOption = SelectOptionItems.ElementAt(i);
                    if (!selectOption.IsAddedTag)
                    {
                        var exists = _datasource.Where(x => x.Equals(selectOption.Item)).FirstOrDefault();

                        if (exists is null)
                        {
                            SelectOptionItems.Remove(selectOption);
                            if (selectOption.IsSelected)
                                SelectedOptionItems.Remove(selectOption);
                        }
                        else
                            dataStoreToSelectOptionItemsMatch.Add(exists, selectOption);
                    }
                }
            }

            //A simple approach to avoid unnecessary scanning through _selectedValues once
            //all of SelectOptionItem where already marked as selected
            int processedSelectedCount = 0;
            if (SelectMode == SelectMode.Default && _selectedValue != null)
                processedSelectedCount = 1;
            else if (SelectMode != SelectMode.Default && _selectedValues != null)
                processedSelectedCount = _selectedValues.Count();

            foreach (var item in _datasource)
            {
                TItemValue value = _getValue(item);

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
                var groupName = string.Empty;
                var label = _getLabel(item);

                bool isSelected = false;
                if (processedSelectedCount > 0)
                {
                    if (SelectMode == SelectMode.Default)
                        isSelected = value.Equals(_selectedValue);
                    else
                        isSelected = _selectedValues.Contains(value);
                }

                if (!string.IsNullOrWhiteSpace(DisabledName))
                    disabled = _getDisabled(item);

                if (!string.IsNullOrWhiteSpace(GroupName))
                    groupName = _getGroup(item);

                if (!exists)
                {
                    var newItem = new SelectOptionItem<TItemValue, TItem>
                    {
                        Label = label,
                        GroupName = groupName,
                        IsDisabled = disabled,
                        Item = item,
                        Value = value,
                        IsSelected = isSelected,
                        IsHidden = isSelected && HideSelected
                    };

                    SelectOptionItems.Add(newItem);
                    if (isSelected)
                    {
                        processedSelectedCount--;
                        SelectedOptionItems.Add(newItem);
                    }
                }
                else if (exists && !IgnoreItemChanges)
                {
                    updateSelectOption.Label = label;
                    updateSelectOption.IsDisabled = disabled;
                    updateSelectOption.GroupName = groupName;
                    updateSelectOption.IsHidden = isSelected && HideSelected;
                    if (isSelected)
                    {
                        if (!updateSelectOption.IsSelected)
                        {
                            updateSelectOption.IsSelected = isSelected;
                            SelectedOptionItems.Add(updateSelectOption);
                        }
                        processedSelectedCount--;
                    }
                }
            }
        }

        /// <summary>
        /// Sorted list of SelectOptionItems
        /// </summary>
        protected internal IEnumerable<SelectOptionItem<TItemValue, TItem>> SortedSelectOptionItems
        {
            get
            {
                var selectOption = SelectOptionItems;

                if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.None)
                {
                    return selectOption.OrderBy(g => g.GroupName);
                }
                else if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.None)
                {
                    return selectOption.OrderByDescending(g => g.GroupName);
                }
                else if (SortByGroup == SortDirection.None && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderBy(l => l.Label);
                }
                else if (SortByGroup == SortDirection.None && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderByDescending(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderBy(g => g.GroupName).ThenBy(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderBy(g => g.GroupName).OrderByDescending(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderByDescending(g => g.GroupName).ThenBy(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderByDescending(g => g.GroupName).OrderByDescending(l => l.Label);
                }
                else if (SelectMode == SelectMode.Tags)
                {
                    if (CustomTagSelectOptionItem != null)
                    {
                        return selectOption.OrderByDescending(g => g.Equals(CustomTagSelectOptionItem));
                    }
                    return selectOption;
                }
                else
                {
                    return selectOption;
                }
            }
        }

        /// <summary>
        /// Sets the CSS classes to change the visual style
        /// </summary>
        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add($"{ClassPrefix}")
                .If($"{ClassPrefix}-open", () => _dropDown?.IsOverlayShow() ?? false)
                .If($"{ClassPrefix}-focused", () => Focused)
                .If($"{ClassPrefix}-single", () => SelectMode == SelectMode.Default)
                .If($"{ClassPrefix}-multiple", () => SelectMode != SelectMode.Default)
                .If($"{ClassPrefix}-sm", () => Size == AntSizeLDSType.Small)
                .If($"{ClassPrefix}-lg", () => Size == AntSizeLDSType.Large)
                .If($"{ClassPrefix}-borderless", () => !Bordered)
                .If($"{ClassPrefix}-show-arrow", () => ShowArrowIcon)
                .If($"{ClassPrefix}-show-search", () => EnableSearch || SelectMode == SelectMode.Tags)
                .If($"{ClassPrefix}-bordered", () => Bordered)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled)
                .If($"{ClassPrefix}-rtl", () => RTL);
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

        /// <summary>
        /// Gets the BoundingClientRect of Ref (JSInvoke) and set the min-width and width in px.
        /// </summary>
        protected async Task SetDropdownStyleAsync()
        {
            string maxWidth = "", minWidth = "", definedWidth = "";
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
            var width = domRect.width.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            minWidth = $"min-width: {width}px;";
            if (DropdownMatchSelectWidth.IsT0 && DropdownMatchSelectWidth.AsT0)
            {
                definedWidth = $"width: {width}px;";
            }
            else if (DropdownMatchSelectWidth.IsT1)
            {
                definedWidth = $"width: {DropdownMatchSelectWidth.AsT1};";
            }
            if (!DropdownMaxWidth.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                maxWidth = $"max-width: {DropdownMaxWidth};";
            _dropdownStyle = minWidth + definedWidth + maxWidth;
        }

        protected async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            OnDropdownVisibleChange?.Invoke(visible);

            if (visible)
            {
                await SetDropdownStyleAsync();

                await SetInputFocusAsync();

                await ScrollToFirstSelectedItemAsync();
            }
            else
            {
                OnOverlayHide();
            }
        }

        protected void OnOverlayHide()
        {
            if (!IsSearchEnabled)
                return;

            if (!AutoClearSearchValue)
                return;

            if (string.IsNullOrWhiteSpace(_searchValue))
                return;

            _searchValue = string.Empty;
            _prevSearchValue = string.Empty;

            if (SelectMode != SelectMode.Default && HideSelected)
            {
                SelectOptionItems.Where(x => !x.IsSelected && x.IsHidden)
                    .ForEach(i => i.IsHidden = false);
            }
            else
            {
                if (CustomTagSelectOptionItem is not null)
                {
                    SelectOptionItems.Remove(CustomTagSelectOptionItem);
                    CustomTagSelectOptionItem = null;
                }
                SelectOptionItems.Where(x => x.IsHidden)
                    .ForEach(i => i.IsHidden = false);
            }
        }

        /// <summary>
        /// Scrolls to the item via JavaScript.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private async Task ElementScrollIntoViewAsync(ElementReference element)
        {
            await JsInvokeAsync(JSInteropConstants.ElementScrollIntoView, element);
        }

        /// <summary>
        /// Close the overlay
        /// </summary>
        /// <returns></returns>
        internal async Task CloseAsync()
        {
            await _dropDown.Hide(true);
        }

        /// <summary>
        /// Called by the Form reset method
        /// </summary>
        internal override void ResetValue()
        {
            _ = ClearSelectedAsync();
        }

        /// <summary>
        /// The method is called every time if the user select/de-select a item by mouse or keyboard.
        /// Don't change the IsSelected property outside of this function.
        /// </summary>
        protected internal async Task SetValueAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));

            if (SelectMode == SelectMode.Default)
            {
                if (SelectedOptionItems.Count > 0)
                {
                    SelectedOptionItems[0].IsSelected = false;
                    SelectedOptionItems[0] = selectOption;
                }
                else
                    SelectedOptionItems.Add(selectOption);

                selectOption.IsSelected = true;
                await ValueChanged.InvokeAsync(selectOption.Value);
                InvokeOnSelectedItemChanged(selectOption);
            }
            else
            {
                selectOption.IsSelected = !selectOption.IsSelected;

                if (selectOption.IsSelected)
                {
                    if (HideSelected && !selectOption.IsHidden)
                        selectOption.IsHidden = true;

                    if (IsSearchEnabled && !string.IsNullOrWhiteSpace(_searchValue))
                        ClearSearch();

                    if (selectOption.IsAddedTag)
                    {
                        CustomTagSelectOptionItem = null;
                        AddedTags.Add(selectOption);
                        SelectOptionItems.Add(selectOption);
                    }
                }
                else
                {
                    if (selectOption.IsHidden)
                        selectOption.IsHidden = false;
                    if (selectOption.IsAddedTag)
                    {
                        SelectOptionItems.Remove(selectOption);
                        SelectedOptionItems.Remove(selectOption);
                        if (selectOption.IsAddedTag && SelectOptions != null)
                        {
                            AddedTags.Remove(selectOption);
                        }
                    }
                    if (IsResponsive)
                        await _selectContent.RemovedItem();
                }
                if (EnableSearch || SelectMode == SelectMode.Tags)
                    await SetInputFocusAsync();
                await InvokeValuesChanged(selectOption);
                await UpdateOverlayPositionAsync();
            }
        }

        /// <summary>
        /// Clears the selectValue(s) property and send the null(default) value back through the two-way binding.
        /// </summary>
        protected async Task ClearSelectedAsync()
        {
            if (SelectMode == SelectMode.Default)
            {
                OnSelectedItemChanged?.Invoke(default);
                await ValueChanged.InvokeAsync(default);
            }
            else
            {
                OnSelectedItemsChanged?.Invoke(default);
                await ValuesChanged.InvokeAsync(default);
            }
        }

        /// <summary>
        /// If DefaultActiveFirstItem is True, the first item which is not IsDisabled(True) is set as selected.
        /// If there is no item it falls back to the clear method.
        /// </summary>
        private async Task SetDefaultActiveFirstItemAsync()
        {
            if (SelectOptionItems.Any())
            {
                var firstEnabled = SortedSelectOptionItems.FirstOrDefault(x => !x.IsDisabled);

                if (firstEnabled != null)
                {
                    firstEnabled.IsSelected = true;

                    if (HideSelected)
                        firstEnabled.IsHidden = true;

                    if (SelectedOptionItems.Count == 0)
                        SelectedOptionItems.Add(firstEnabled);
                    else
                        SelectedOptionItems[0] = firstEnabled;

                    if (SelectMode == SelectMode.Default)
                    {
                        await ValueChanged.InvokeAsync(firstEnabled.Value);

                        if (!ValueChanged.HasDelegate)
                            await InvokeStateHasChangedAsync();
                    }
                    else
                    {
                        await InvokeValuesChanged();

                        if (!ValuesChanged.HasDelegate)
                            await InvokeStateHasChangedAsync();
                    }
                }
                else
                {
                    await ClearSelectedAsync();
                }
            }
            else
            {
                await ClearSelectedAsync();
            }
            _defaultActiveFirstOptionApplied = true;
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

                    _waittingStateChange = true;
                    if (SelectedOptionItems.Count == 0)
                        SelectedOptionItems.Add(result);
                    else
                        SelectedOptionItems[0] = result;
                    await ValueChanged.InvokeAsync(result.Value);
                }
                else
                {
                    await SetDefaultActiveFirstItemAsync();
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

                    if (result != null && !result.IsDisabled)
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
                    _waittingStateChange = true;

                    await InvokeValuesChanged();
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
        private async Task SetInitialValuesAsync()
        {
            SelectedOptionItems.Clear();
            if (SelectMode == SelectMode.Default)
            {
                if (_selectedValue != null)
                {
                    var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, _selectedValue));

                    if (result != null)
                    {
                        if (result.IsDisabled)
                        {
                            await TrySetDefaultValueAsync();
                            return;
                        }

                        result.IsSelected = true;
                        ActiveOption = result;
                        if (HideSelected)
                            result.IsHidden = true;
                        SelectedOptionItems.Add(result);
                        OnSelectedItemChanged?.Invoke(result.Item);
                        await ValueChanged.InvokeAsync(result.Value);
                    }
                }
            }
            else
            {
                if (_selectedValues != null)
                {
                    foreach (var value in _selectedValues)
                    {
                        var result = SelectOptionItems.FirstOrDefault(c => EqualityComparer<TItemValue>.Default.Equals(c.Value, value));

                        if (result != null && !result.IsDisabled)
                        {
                            result.IsSelected = true;

                            if (HideSelected)
                                result.IsHidden = true;
                        }
                    }

                    var newSelectedValues = new List<TItemValue>();
                    var newSelectedItems = new List<TItem>();

                    SelectOptionItems.Where(x => x.IsSelected)
                        .ForEach(i =>
                        {
                            newSelectedValues.Add(i.Value);
                            newSelectedItems.Add(i.Item);
                            SelectedOptionItems.Add(i);
                        });

                    OnSelectedItemsChanged?.Invoke(newSelectedItems);
                    await ValuesChanged.InvokeAsync(newSelectedValues);
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

        /// <summary>
        /// Creates the select option item. Mostly meant to create new tags, that is why IsAddedTag is hardcoded to true.
        /// </summary>
        /// <param name="label">Creation based on passed label</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        /// <returns></returns>
        private SelectOptionItem<TItemValue, TItem> CreateSelectOptionItem(string label, bool isActive)
        {
            TItemValue value = CustomTagLabelToValue.Invoke(label);
            TItem item;
            if (_isPrimitive)
            {
                item = (TItem)TypeDescriptor.GetConverter(typeof(TItem)).ConvertFromInvariantString(_searchValue);
            }
            else
            {
                item = Activator.CreateInstance<TItem>();
                _setLabel(item, _searchValue);
                _setValue(item, value);
            }
            return new SelectOptionItem<TItemValue, TItem>() { Label = label, Value = value, Item = item, IsActive = isActive, IsSelected = false, IsAddedTag = true };
        }

        /// <summary>
        /// A separate method to invoke ValuesChanged and OnSelectedItemsChanged to reduce code duplicates.
        /// </summary>

        protected void InvokeOnSelectedItemChanged(SelectOptionItem<TItemValue, TItem> selectOptionItem = null)
        {
            if (selectOptionItem == null)
            {
                OnSelectedItemsChanged?.Invoke(default);
            }
            else
            {
                if (LabelInValue && SelectOptions != null)
                {
                    // Embed the label into the value and return the result as json string.
                    var valueLabel = new Select.Internal.ValueLabel<TItemValue>
                    {
                        Value = selectOptionItem.Value,
                        Label = selectOptionItem.Label
                    };

                    var json = JsonSerializer.Serialize(valueLabel);

                    OnSelectedItemChanged?.Invoke((TItem)Convert.ChangeType(json, typeof(TItem)));
                }
                else
                {
                    OnSelectedItemChanged?.Invoke(selectOptionItem.Item);
                }
            }
        }

        protected async Task InvokeValuesChanged(SelectOptionItem<TItemValue, TItem> newSelection = null)
        {
            List<TItemValue> newSelectedValues;
            if (newSelection is null || Values is null)
            {
                newSelectedValues = new List<TItemValue>();
                SelectedOptionItems.Clear();
                SelectOptionItems.Where(x => x.IsSelected)
                    .ForEach(i =>
                    {
                        newSelectedValues.Add(i.Value);
                        SelectedOptionItems.Add(i);
                    });
            }
            else
            {
                newSelectedValues = Values.ToList();
                if (newSelection.IsSelected)
                {
                    newSelectedValues.Add(newSelection.Value);
                    SelectedOptionItems.Add(newSelection);
                }
                else
                {
                    newSelectedValues.Remove(newSelection.Value);
                    SelectedOptionItems.Remove(newSelection);
                }
            }

            if (ValuesChanged.HasDelegate)
                await ValuesChanged.InvokeAsync(newSelectedValues);
            else
            {
                Values = newSelectedValues;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Inform the Overlay to update the position.
        /// </summary>
        internal async Task UpdateOverlayPositionAsync()
        {
            if (_dropDown.Visible)
                await _dropDown.GetOverlayComponent().UpdatePosition();
        }

        #region Events

        /// <summary>
        /// The Method is called every time if the value of the @bind-Value was changed by the two-way binding.
        /// </summary>
        protected override void OnValueChange(TItemValue value)
        {
            if (!_isInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
                return;

            if (EqualityComparer<TItemValue>.Default.Equals(value, default))
            {
                _ = InvokeAsync(() => OnInputClearClickAsync(new()));
                return;
            }

            var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, value));

            if (result == null)
            {
                if (!AllowClear)
                    _ = TrySetDefaultValueAsync();
                else
                {
                    //Reset value if not found - needed if value changed
                    //outside of the component
                    _ = InvokeAsync(() => OnInputClearClickAsync(new()));
                }
                return;
            }

            if (result.IsDisabled)
            {
                _ = TrySetDefaultValueAsync();

                return;
            }

            result.IsSelected = true;

            EvaluateValueChangedOutsideComponent(result, value);

            if (HideSelected)
                result.IsHidden = true;

            ValueChanged.InvokeAsync(result.Value);
        }

        /// <summary>
        /// When bind-Value is changed outside of the component, then component
        /// selected items have to be reselected according to new value passed.
        /// </summary>
        /// <param name="optionItem">The option item that has been selected.</param>
        /// <param name="value">The value of the selected option item.</param>
        private void EvaluateValueChangedOutsideComponent(SelectOptionItem<TItemValue, TItem> optionItem, TItemValue value)
        {
            if (ActiveOption != null && !ActiveOption.Value.Equals(value))
            {
                ActiveOption.IsSelected = false;
                ActiveOption = optionItem;
            }
            if (SelectedOptionItems.Count > 0)
            {
                if (!SelectedOptionItems[0].Value.Equals(value))
                {
                    SelectedOptionItems[0].IsSelected = false;
                    SelectedOptionItems[0] = optionItem;
                }
            }
            else
                SelectedOptionItems.Add(optionItem);
        }

        /// <summary>
        /// The Method is called every time if the value of the @bind-Values was changed by the two-way binding.
        /// </summary>
        protected async Task OnValuesChangeAsync(IEnumerable<TItemValue> values)
        {
            if (!_isInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
                return;

            if (!SelectOptionItems.Any())
                return;

            if (values == null)
            {
                await ValuesChanged.InvokeAsync(default);
                OnSelectedItemsChanged?.Invoke(default);
                return;
            }

            EvaluateValuesChangedOutsideComponent(values);

            if (_dropDown.IsOverlayShow())
            {
                //A delay forces a refresh better than StateHasChanged().
                //For example when a tag is added that is causing SelectContent to grow,
                //this Task.Delay will actually allow to reposition the Overlay to match
                //new size of SelectContent.
                await Task.Delay(1);
                await UpdateOverlayPositionAsync();
            }

            OnSelectedItemsChanged?.Invoke(SelectedOptionItems.Select(s => s.Item));
            await ValuesChanged.InvokeAsync(Values);
        }

        /// <summary>
        /// When bind-Values is changed outside of the component, then component
        /// selected items have to be reselected according to new values passed.
        /// TODO: (Perf) Consider using hash to identify if the passed values are different from currently selected.
        /// </summary>
        /// <param name="values">The values that need to be selected.</param>
        private void EvaluateValuesChangedOutsideComponent(IEnumerable<TItemValue> values)
        {
            var newSelectedItems = new List<TItem>();
            var deselectList = SelectedOptionItems.ToDictionary(item => item.Value, item => item);
            foreach (var value in values.ToList())
            {
                SelectOptionItem<TItemValue, TItem> result;
                if (SelectMode == SelectMode.Multiple)
                {
                    result = SelectOptionItems.FirstOrDefault(x => !x.IsSelected && EqualityComparer<TItemValue>.Default.Equals(x.Value, value));
                    if (result != null && !result.IsDisabled)
                    {
                        result.IsSelected = true;
                        SelectedOptionItems.Add(result);
                    }
                    deselectList.Remove(value);
                }
                else
                {
                    result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, value));
                    if (result is null) //tag delivered from outside, needs to be added to the list of options
                    {
                        result = CreateSelectOptionItem(value.ToString(), true);
                        result.IsSelected = true;
                        AddedTags.Add(result);
                        SelectOptionItems.Add(result);
                        SelectedOptionItems.Add(result);
                    }
                    else if (result != null && !result.IsSelected && !result.IsDisabled)
                    {
                        result.IsSelected = true;
                        SelectedOptionItems.Add(result);
                    }
                    deselectList.Remove(value);
                }
            }
            if (deselectList.Count > 0)
            {
                foreach (var item in deselectList)
                {
                    item.Value.IsSelected = false;
                    SelectedOptionItems.Remove(item.Value);
                    if (item.Value.IsAddedTag)
                    {
                        SelectOptionItems.Remove(item.Value);
                        AddedTags.Remove(item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the value of the Input element was changed by keyboard
        /// </summary>
        /// <param name="e">Contains the value of the Input element</param>
        protected async void OnInputAsync(ChangeEventArgs e)
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
            _prevSearchValue = _searchValue;
            if (_isToken)
                _searchValue = e.Value?.ToString().TrimEnd(TokenSeparators);
            else
            {
                _searchValue = e.Value?.ToString();

                if (TokenSeparators is not null && TokenSeparators.Length > 0)
                {
                    containsToken = TokenSeparators.Any(t => _searchValue.Contains(t));
                }
            }

            //_inputWidth = string.IsNullOrEmpty(_searchValue) ? InputDefaultWidth : $"{4 + _searchValue.Length * 8}px";

            if (containsToken)
            {
                await TokenizeSearchedPhrase(_searchValue);
            }

            if (!string.IsNullOrWhiteSpace(_searchValue))
            {
                FilterOptionItems(_searchValue);
            }
            else
            {
                SelectOptionItems.Where(x => x.IsHidden).ForEach(i => i.IsHidden = false);
                if (SelectMode == SelectMode.Tags && CustomTagSelectOptionItem is not null)
                {
                    SelectOptionItems.Remove(CustomTagSelectOptionItem);
                    CustomTagSelectOptionItem = null;
                }
            }
            OnSearch?.Invoke(_searchValue);
        }

        private async Task TokenizeSearchedPhrase(string searchValue)
        {
            Dictionary<string, SelectOptionItem<TItemValue, TItem>> tokenItemMatch = new();
            tokenItemMatch = searchValue.Split(TokenSeparators).Distinct().ToDictionary(
                item => item.Trim(),
                _ => default(SelectOptionItem<TItemValue, TItem>));

            if (SelectMode == SelectMode.Tags)
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
            if (SelectMode != SelectMode.Tags)
            {
                bool firstDone = false;
                foreach (var item in SelectOptionItems)
                {
                    if (item.Label.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!firstDone)
                        {
                            item.IsActive = true;
                            firstDone = true;
                        }
                        else if (item.IsActive)
                        {
                            item.IsActive = false;
                        }
                        if (item.IsHidden)
                            item.IsHidden = false;
                    }
                    else
                    {
                        if (!item.IsHidden)
                            item.IsHidden = true;
                        item.IsActive = false;
                    }
                }
            }
            else
            {
                FilterTagsOptionItems(searchValue);
            }
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
                    if (item.Label.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase))
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
                        else if (item.IsActive)
                            item.IsActive = false;
                        if (item.IsHidden)
                            item.IsHidden = false;
                    }
                    else
                    {
                        if (!item.IsHidden)
                            item.IsHidden = true;
                        item.IsActive = false;
                    }
                }
            }

            if (activeCanditate is null)
            {
                //label has to be cast-able to value
                TItemValue value = CustomTagLabelToValue.Invoke(searchValue);
                if (CustomTagSelectOptionItem is null)
                {
                    CustomTagSelectOptionItem = CreateSelectOptionItem(searchValue, true);
                    SelectOptionItems.Add(CustomTagSelectOptionItem);
                    ActiveOption = CustomTagSelectOptionItem;
                }
                else
                {
                    CustomTagSelectOptionItem.Label = searchValue;
                    CustomTagSelectOptionItem.Value = value;
                    if (_isPrimitive)
                    {
                        CustomTagSelectOptionItem.Item = (TItem)TypeDescriptor.GetConverter(typeof(TItem)).ConvertFromInvariantString(_searchValue);
                    }
                    else
                    {
                        _setLabel(CustomTagSelectOptionItem.Item, _searchValue);
                        _setValue(CustomTagSelectOptionItem.Item, value);
                    }
                }
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the keyboard key is no longer pressed inside the Input element.
        /// </summary>
        /// <param name="e">Contains the key (combination) which was pressed inside the Input element</param>
        protected async Task OnKeyUpAsync(KeyboardEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var key = e.Key.ToUpperInvariant();
            var overlayFirstOpen = false;

            if (_isToken && SelectMode == SelectMode.Tags)
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

                ClearSearch();

                return;
            }

            if (key == "ENTER")
            {
                if (!_dropDown.IsOverlayShow())
                    return;

                if (!SelectOptionItems.Any())
                    return;

                if (SelectMode == SelectMode.Default)
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

                if (SelectMode == SelectMode.Multiple)
                {
                    if (AllOptionsHidden())
                        return;

                    var firstActive = SelectOptionItems.FirstOrDefault(x => x.IsActive);

                    if (firstActive != null && !firstActive.IsDisabled)
                    {
                        await SetValueAsync(firstActive);
                        ClearSearch();
                    }
                    return;
                }

                if (SelectMode == SelectMode.Tags)
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

                        // ToDo: Sometime the element does not scroll, you have to call the function twice
                        await ElementScrollIntoViewAsync(currentSelected.Ref);
                        await Task.Delay(1);
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

                        // ToDo: Sometime the element does not scroll, you have to call the function twice
                        await ElementScrollIntoViewAsync(currentSelected.Ref);
                        await Task.Delay(1);
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
                (EnableSearch || SelectMode == SelectMode.Tags || AllowClear))
            {
                if (string.IsNullOrEmpty(_prevSearchValue) && SelectedOptionItems.Count > 0)
                    await OnRemoveSelectedAsync(SelectedOptionItems.Last());
                else if (!string.IsNullOrEmpty(_prevSearchValue))
                    _prevSearchValue = _searchValue;
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the Input element get the focus
        /// </summary>
        protected async Task OnInputFocusAsync(FocusEventArgs _)
        {
            await SetInputFocusAsync();
        }

        /// <summary>
        /// Method is called via EventCallback if a key is pressed inside Input element.
        /// The method is used to get the TAB event if the user press TAB to cycle trough elements.
        /// If a TAB is received, the overlay will be closed and the Input element blures.
        /// </summary>
        protected async Task OnKeyDownAsync(KeyboardEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var key = e.Key.ToUpperInvariant();

            if (key == "TAB")
            {
                if (_dropDown.IsOverlayShow())
                {
                    await CloseAsync();
                }

                await SetInputBlurAsync();
            }
            else if (TokenSeparators is not null && TokenSeparators.Length > 0)
            {
                _isToken = TokenSeparators.Contains(e.Key[0]);
            }
        }

        /// <summary>
        /// Check if Focused property is False; Set the Focused property to true, change the
        /// style and set the Focus on the Input element via DOM. It also invoke the OnFocus Action.
        /// </summary>
        protected async Task SetInputFocusAsync()
        {
            if (!Focused)
            {
                Focused = true;

                SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Focus, _inputRef);

                OnFocus?.Invoke();
            }
        }

        /// <summary>
        /// Method is called via EventCallBack if the Input element loses the focus
        /// </summary>
        protected async Task OnInputBlurAsync(FocusEventArgs _)
        {
            await SetInputBlurAsync();
        }

        /// <summary>
        /// Check if Focused property is true;  Set the Focused property to false, change the
        /// style and blures the Input element via DOM. It also invoke the OnBlur Action.
        /// </summary>
        /// <returns></returns>
        protected async Task SetInputBlurAsync()
        {
            if (Focused)
            {
                Focused = false;

                SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Blur, _inputRef);

                OnBlur?.Invoke();
            }
        }

        protected void ClearSearch()
        {
            if (SelectMode != SelectMode.Default)
            {
                foreach (var item in SelectOptionItems)
                {
                    if (item.IsHidden)
                    {
                        if ((HideSelected && !item.IsSelected) || !HideSelected)
                            item.IsHidden = false;
                    }
                }
                foreach (var item in AddedTags)
                {
                    if (item.IsHidden)
                    {
                        if ((HideSelected && !item.IsSelected) || !HideSelected)
                            item.IsHidden = false;
                    }
                }
            }

            _searchValue = string.Empty;
            _prevSearchValue = string.Empty;
        }

        /// <summary>
        /// Search the first selected item, set IsActive to False for all other items and call the scrollIntoView function via JavaScript.
        /// The method is used to scroll to the first selected item after opening the overlay.
        /// </summary>
        protected async Task ScrollToFirstSelectedItemAsync()
        {
            // Check if there is a selected item and set it as active
            var currentSelected = SelectOptionItems.FirstOrDefault(x => x.IsSelected);

            if (currentSelected != null)
            {
                SelectOptionItems.Where(x => x.IsActive)
                    .ForEach(i => i.IsActive = false);

                currentSelected.IsActive = true;
                ActiveOption = currentSelected;
                // ToDo: Sometime the element does not scroll, you have to call the function twice
                await ElementScrollIntoViewAsync(currentSelected.Ref);
                await Task.Delay(1);
                await ElementScrollIntoViewAsync(currentSelected.Ref);
            }
            else if (ActiveOption == null)//position on first element in the list
            {
                var selectionCandidate = SelectOptionItems.FirstOrDefault();
                if (selectionCandidate != null)
                    ActiveOption = selectionCandidate;
            }
        }

        /// <summary>
        /// Method is called via EventCallBack after the user clicked on the Clear icon inside the Input element.
        /// Set the IsSelected and IsHidden properties for all items to False. It updates the overlay position if
        /// the SelectMode is Tags or Multiple. Invoke the OnClearSelected Action. Set the Value(s) to default.
        /// </summary>
        protected async Task OnInputClearClickAsync(MouseEventArgs _)
        {
            List<SelectOptionItem<TItemValue, TItem>> tagItems = new();

            SelectOptionItems.Where(c => c.IsSelected)
                .ForEach(i =>
                {
                    i.IsSelected = false;
                    i.IsHidden = false;
                    if (i.IsAddedTag)
                        tagItems.Add(i);
                });
            //When clearing, also remove all added tags that are kept after adding in SelectOptionItems
            if (tagItems.Count > 0)
            {
                foreach (var item in tagItems)
                {
                    SelectOptionItems.Remove(item);
                }
            }
            AddedTags.Clear();
            ActiveOption = SelectOptionItems.FirstOrDefault();
            CustomTagSelectOptionItem = null;
            SelectedOptionItems.Clear();

            await ClearSelectedAsync();

            if (SelectMode != SelectMode.Default)
            {
                await Task.Delay(1);    // Todo - Workaround because UI does not refresh
                await UpdateOverlayPositionAsync();
                StateHasChanged();      // Todo - Workaround because UI does not refresh
            }

            OnClearSelected?.Invoke();
        }

        /// <summary>
        /// Method is called via EventCallBack if the user clicked on the Close icon of a Tag.
        /// </summary>
        protected async Task OnRemoveSelectedAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));
            await SetValueAsync(selectOption);
        }

        internal async Task OnArrowClick(MouseEventArgs args)
        {
            await _dropDown.OnClickDiv(args);
        }

        #endregion Events
    }
}