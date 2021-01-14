using AntDesign.Select.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#pragma warning disable 1591 // Disable missing XML comment
#pragma warning disable CA1716 // Disable Select name warning
#pragma warning disable CA1305 // IFormatProvider warning

namespace AntDesign
{
    public partial class Select<TItemValue, TItem>
    {
        #region Parameters

        [Parameter] public bool AllowClear { get; set; }
        [Parameter] public bool AllowCustomTags { get; set; }
        [Parameter] public bool AutoClearSearchValue { get; set; } = true;
        [Parameter] public bool Bordered { get; set; } = true;
        [Parameter] public Action<string> OnCreateCustomTag { get; set; }
        [Parameter] public bool DefaultActiveFirstItem { get; set; } = false;
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public string DisabledName { get; set; }
        [Parameter] public Func<RenderFragment, RenderFragment> DropdownRender { get; set; }
        [Parameter] public bool EnableSearch { get; set; }
        [Parameter] public string GroupName { get; set; } = string.Empty;
        [Parameter] public bool HideSelected { get; set; }
        [Parameter] public bool IgnoreItemChanges { get; set; } = true;
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

        /// <summary>
        /// LabelInValue can only be used if the SelectOption is not created by DataSource.
        /// </summary>
        [Parameter] public bool LabelInValue { get; set; }

        [Parameter] public string LabelName { get; set; }
        [Parameter] public RenderFragment<TItem> LabelTemplate { get; set; }
        [Parameter] public bool Loading { get; set; }
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
        [Parameter] public bool ShowArrowIcon { get; set; } = true;
        [Parameter] public bool ShowSearchIcon { get; set; } = true;
        [Parameter] public SortDirection SortByGroup { get; set; } = SortDirection.None;
        [Parameter] public SortDirection SortByLabel { get; set; } = SortDirection.None;
        [Parameter] public RenderFragment SuffixIcon { get; set; }
        [Parameter] public char[] TokenSeparators { get; set; }
        [Parameter] public override EventCallback<TItemValue> ValueChanged { get; set; }
        [Parameter] public string ValueName { get; set; }
        [Parameter] public EventCallback<IEnumerable<TItemValue>> ValuesChanged { get; set; }

        [Parameter]
        public IEnumerable<TItem> DataSource
        {
            get => _datasource;
            set
            {
                if (value == null && _datasource == null)
                    return;

                if (value == null && _datasource != null)
                {
                    SelectOptionItems.Clear();

                    Value = default;
                    if (ValueChanged.HasDelegate)
                        _ = ValueChanged.InvokeAsync(default);

                    _datasource = null;

                    OnDataSourceChanged?.Invoke();

                    return;
                }

                if (value != null && !value.Any() && SelectOptionItems.Any())
                {
                    SelectOptionItems.Clear();

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

                    OnValueChange(value);
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

        #region Properties

        private const string ClassPrefix = "ant-select";
        private const string DefaultWidth = "width: 100%;";

        /// <summary>
        /// Determines if SelectOptions has any selected items
        /// </summary>
        /// <returns>true if SelectOptions has any selected Items, otherwise false</returns>
        internal bool HasValue
        {
            get => SelectOptionItems.Where(x => x.IsSelected).Any();
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
            get => EnableSearch;
        }

        /// <summary>
        /// Indicates if the GroupName is used. When this value is True, the SelectOptions will be rendered in group mode.
        /// </summary>
        internal bool IsGroupingEnabled
        {
            get => !string.IsNullOrWhiteSpace(GroupName);
        }

        internal SelectMode SelectMode => Mode.ToSelectMode();
        internal bool Focused { get; private set; }
        private string _searchValue = string.Empty;
        private string _dropdownStyle = string.Empty;
        private TItemValue _selectedValue;
        private TItemValue _defaultValue;
        private bool _defaultValueIsNotNull;
        private IEnumerable<TItem> _datasource;
        private IEnumerable<TItemValue> _selectedValues;
        private IEnumerable<TItemValue> _defaultValues;
        private bool _defaultValuesHasItems;
        private bool _isInitialized;
        private bool _waittingStateChange;
        internal ElementReference _inputRef;
        protected OverlayTrigger _dropDown;

        internal HashSet<SelectOptionItem<TItemValue, TItem>> SelectOptionItems { get; } = new HashSet<SelectOptionItem<TItemValue, TItem>>();

        #endregion Properties

        protected override void OnInitialized()
        {
            SetClassMap();

            if (string.IsNullOrWhiteSpace(Style))
                Style = DefaultWidth;

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

                await SetDropdownStyleAsync();
            }

            //
            if (_isInitialized && SelectOptions == null)
                CreateDeleteSelectOptions();

            if (SelectMode == SelectMode.Default)
            {
                // Try to set the default value each render cycle if _selectedValue has no value
                if (_defaultValueIsNotNull && !HasValue && SelectOptionItems.Any()
                    || DefaultActiveFirstItem && !HasValue && SelectOptionItems.Any())
                {
                    await TrySetDefaultValueAsync();
                }
            }
            else
            {
                // Try to set the default value each render cycle if _selectedValue has no value
                if (_defaultValuesHasItems && !HasValue && SelectOptionItems.Any()
                    || DefaultActiveFirstItem && !HasValue && SelectOptionItems.Any())
                {
                    await TrySetDefaultValuesAsync();
                }
            }

            if (_waittingStateChange)
            {
                _waittingStateChange = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
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

            // Compare items of SelectOptions and the datastore
            if (SelectOptionItems.Any())
            {
                // Delete items from SelectOptions if it is no longer in the datastore
                for (var i = SelectOptionItems.Count - 1; i >= 0; i--)
                {
                    var selectOption = SelectOptionItems.ElementAt(i);
                    var exists = _datasource.Contains(selectOption.Item);

                    if (!exists)
                    {
                        SelectOptionItems.Remove(selectOption);
                    }
                }
            }

            foreach (var item in _datasource)
            {
                TItemValue value = GetPropertyValueAsTItemValue(item, ValueName);

                var exists = false;
                SelectOptionItem<TItemValue, TItem> updateSelectOption = null;

                foreach (var selectOption in SelectOptionItems)
                {
                    var result = EqualityComparer<TItemValue>.Default.Equals(selectOption.Value, value);

                    if (result)
                    {
                        exists = true;
                        updateSelectOption = selectOption;
                        continue;
                    }
                }

                if (!exists)
                {
                    var disabled = false;
                    var groupName = string.Empty;

                    var label = GetPropertyValueAsObject(item, LabelName)?.ToString();

                    if (!string.IsNullOrWhiteSpace(DisabledName))
                        disabled = (bool)GetPropertyValueAsObject(item, DisabledName);

                    if (!string.IsNullOrWhiteSpace(GroupName))
                        groupName = GetPropertyValueAsObject(item, GroupName)?.ToString();

                    var newItem = new SelectOptionItem<TItemValue, TItem>
                    {
                        Label = label,
                        GroupName = groupName,
                        IsDisabled = disabled,
                        Item = item,
                        Value = value
                    };

                    SelectOptionItems.Add(newItem);
                }
                else if (exists && !IgnoreItemChanges)
                {
                    updateSelectOption.Label = GetPropertyValueAsObject(item, LabelName)?.ToString();

                    if (!string.IsNullOrWhiteSpace(DisabledName))
                        updateSelectOption.IsDisabled = (bool)GetPropertyValueAsObject(item, DisabledName);

                    if (!string.IsNullOrWhiteSpace(GroupName))
                        updateSelectOption.GroupName = GetPropertyValueAsObject(item, GroupName)?.ToString();
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
                .If($"{ClassPrefix}-show-search", () => EnableSearch)
                .If($"{ClassPrefix}-bordered", () => Bordered)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled);
        }

        /// <summary>
        /// Returns True if the parameter IsHidden is set to true for all entries in the SelectOptions list
        /// </summary>
        /// <returns>true if all items are set to IsHidden(true)</returns>
        protected bool AllOptionsHidden()
        {
            return SelectOptionItems.All(x => x.IsHidden);
        }

        /// <summary>
        /// Gets the BoundingClientRect of Ref (JSInvoke) and set the min-width and width in px.
        /// </summary>
        protected async Task SetDropdownStyleAsync()
        {
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);

            _dropdownStyle = $"min-width: {domRect.width}px; width: {domRect.width}px;";
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

            if (SelectMode != SelectMode.Default && HideSelected)
            {
                SelectOptionItems.Where(x => !x.IsSelected && x.IsHidden)
                    .ForEach(i => i.IsHidden = false);
            }
            else
            {
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
                SelectOptionItems.Where(x => x.IsSelected)
                    .ForEach(i => i.IsSelected = false);

                selectOption.IsSelected = true;

                await ValueChanged.InvokeAsync(selectOption.Value);
            }
            else
            {
                selectOption.IsSelected = !selectOption.IsSelected;

                if (selectOption.IsSelected)
                {
                    if (HideSelected && !selectOption.IsHidden)
                        selectOption.IsHidden = true;

                    if (IsSearchEnabled)
                    {
                        if (!string.IsNullOrWhiteSpace(_searchValue))
                        {
                            ClearSearch();

                            await SetInputFocusAsync();
                        }
                    }
                }
                else
                {
                    if (selectOption.IsHidden)
                        selectOption.IsHidden = false;
                }

                await InvokeValuesChanged();

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
            }
            else
            {
                OnSelectedItemsChanged?.Invoke(default);
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

                    if (SelectMode == SelectMode.Default)
                    {
                        Value = firstEnabled.Value;
                        if (!ValueChanged.HasDelegate)
                            await InvokeStateHasChangedAsync();
                        else
                            await ValueChanged.InvokeAsync(firstEnabled.Value);
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
                    Value = result.Value;
                        
                    if (!ValueChanged.HasDelegate)
                        await InvokeStateHasChangedAsync();
                    else
                        await ValueChanged.InvokeAsync(result.Value);
                }
                else
                {
                    await SetDefaultActiveFirstItemAsync();
                }
            }
            else if (DefaultActiveFirstItem)
            {
                await SetDefaultActiveFirstItemAsync();
            }
            else
            {
                await ClearSelectedAsync();
            }
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
                    if (DefaultActiveFirstItem)
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
            else if (DefaultActiveFirstItem)
            {
                await SetDefaultActiveFirstItemAsync();
            }
            else
            {
                await ClearSelectedAsync();
            }
        }

        /// <summary>
        /// Sets the initial values after initialization, the method should only called once.
        /// </summary>
        private async Task SetInitialValuesAsync()
        {
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

                        if (HideSelected)
                            result.IsHidden = true;

                        OnSelectedItemChanged?.Invoke(result.Item);
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
                        });

                    OnSelectedItemsChanged?.Invoke(newSelectedItems);
                }
            }
        }

        /// <summary>
        /// Append a label item in tag mode
        /// </summary>
        /// <param name="label"></param>
        private void AppendLabelValue(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return;

            SelectOptionItems.Add(new SelectOptionItem<TItemValue, TItem>() { Label = label, IsActive = true, IsSelected = true });
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

        protected async Task InvokeValuesChanged()
        {
            var newSelectedValues = new List<TItemValue>();

            SelectOptionItems.Where(x => x.IsSelected)
                .ForEach(i =>
                {
                    newSelectedValues.Add(i.Value);
                });

            await ValuesChanged.InvokeAsync(newSelectedValues);
        }

        /// <summary>
        /// Inform the Overlay to update the position.
        /// </summary>
        internal async Task UpdateOverlayPositionAsync()
        {
            await _dropDown.GetOverlayComponent().UpdatePosition();
        }

        private static object GetPropertyValueAsObject(object obj, string propertyName)
        {
            return obj.GetType().GetProperties()
                .Single(p => p.Name == propertyName)
                .GetValue(obj, null);
        }

        private static TItemValue GetPropertyValueAsTItemValue(object obj, string propertyName)
        {
            var result = obj.GetType().GetProperties()
                .Single(p => p.Name == propertyName)
                .GetValue(obj, null);

            return (TItemValue)TypeDescriptor.GetConverter(typeof(TItemValue)).ConvertFromInvariantString(result.ToString());
        }

        #region Events

        /// <summary>
        /// The Method is called every time if the value of the @bind-Value was changed by the two-way binding.
        /// </summary>
        protected override void OnValueChange(TItemValue value)
        {
            if (!_isInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
            {
                _waittingStateChange = true;
                return;
            }

            SelectOptionItems.Where(x => x.IsSelected)
                .ForEach(i => i.IsSelected = false);

            if (EqualityComparer<TItemValue>.Default.Equals(value, default))
            {
                OnSelectedItemChanged?.Invoke(default);
                return;
            }

            var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, value));

            if (result == null)
            {
                _ = TrySetDefaultValueAsync();

                return;
            }

            if (result.IsDisabled)
            {
                _ = TrySetDefaultValueAsync();

                return;
            }

            result.IsSelected = true;

            if (HideSelected)
                result.IsHidden = true;

            InvokeOnSelectedItemChanged(result);
        }

        /// <summary>
        /// The Method is called every time if the value of the @bind-Values was changed by the two-way binding.
        /// </summary>
        protected async Task OnValuesChangeAsync(IEnumerable<TItemValue> values)
        {
            if (!_isInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
            {
                _waittingStateChange = true;
                return;
            }

            if (!SelectOptionItems.Any())
                return;

            SelectOptionItems.Where(x => x.IsSelected)
                .ForEach(i => i.IsSelected = false);

            if (values == null)
            {
                OnSelectedItemsChanged?.Invoke(default);
                return;
            }

            var valueList = values.ToList();

            foreach (var item in valueList)
            {
                var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, item));

                if (result != null && !result.IsDisabled)
                {
                    result.IsSelected = true;
                }
            }

            if (_dropDown.IsOverlayShow())
            {
                await UpdateOverlayPositionAsync();
            }

            var newSelectedValues = new List<TItemValue>();
            var newSelectedItems = new List<TItem>();

            SelectOptionItems.Where(x => x.IsSelected)
                .ForEach(i =>
                {
                    newSelectedValues.Add(i.Value);
                    newSelectedItems.Add(i.Item);
                });

            OnSelectedItemsChanged?.Invoke(newSelectedItems);
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

            _searchValue = e.Value?.ToString();

            //_inputWidth = string.IsNullOrEmpty(_searchValue) ? InputDefaultWidth : $"{4 + _searchValue.Length * 8}px";

            if (SelectMode == SelectMode.Default)
            {
                SelectOptionItems.Where(x => x.IsHidden).ForEach(i => i.IsHidden = false);

                if (!string.IsNullOrWhiteSpace(_searchValue))
                {
                    SelectOptionItems
                        .Where(x => !x.Label.Contains(_searchValue, StringComparison.InvariantCultureIgnoreCase))
                        .ForEach(i =>
                        {
                            i.IsHidden = true;
                            i.IsActive = false;
                        });
                }

                OnSearch?.Invoke(_searchValue);
            }
            else if (TokenSeparators?.Any() == true)
            {
                // Automatic tokenization
                _searchValue?.Split(TokenSeparators).ForEach(AppendLabelValue);

                ClearSearch();
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

                    if (firstActive != null)
                    {
                        if (!firstActive.IsDisabled)
                        {
                            await SetValueAsync(firstActive);
                        }
                    }

                    return;
                }

                if (SelectMode == SelectMode.Tags)
                {
                    if (AllowCustomTags)
                    {
                        var anyActiveItems = SelectOptionItems.Any(x => x.IsActive);

                        if (AllOptionsHidden() || !anyActiveItems)
                        {
                            OnCreateCustomTag?.Invoke(_searchValue);

                            ClearSearch();

                            return;
                        }
                    }
                    else
                    {
                        AppendLabelValue(_searchValue);
                        ClearSearch();
                        return;
                    }

                    //var firstActive = SelectOptionItems.FirstOrDefault(x => x.IsActive);

                    //if (firstActive != null)
                    //{
                    //    if (!firstActive.IsDisabled)
                    //    {
                    //        await SetValueAsync(firstActive);
                    //    }
                    //}

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
                        firstOption.IsActive = true;
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
                if (HideSelected)
                {
                    SelectOptionItems.Where(x => x.IsHidden && !x.IsSelected)
                        .ForEach(i => i.IsHidden = false);
                }
                else
                {
                    SelectOptionItems.Where(x => x.IsHidden)
                        .ForEach(i => i.IsHidden = false);
                }

                SelectOptionItems.Where(x => x.IsActive)
                        .ForEach(i => i.IsActive = false);
            }

            _searchValue = string.Empty;
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

                // ToDo: Sometime the element does not scroll, you have to call the function twice
                await ElementScrollIntoViewAsync(currentSelected.Ref);
                await Task.Delay(1);
                await ElementScrollIntoViewAsync(currentSelected.Ref);
            }
        }

        /// <summary>
        /// Method is called via EventCallBack after the user clicked on the Clear icon inside the Input element.
        /// Set the IsSelected and IsHidden properties for all items to False. It updates the overlay position if
        /// the SelectMode is Tags or Multiple. Invoke the OnClearSelected Action. Set the Value(s) to default.
        /// </summary>
        protected async Task OnInputClearClickAsync(MouseEventArgs _)
        {
            SelectOptionItems.Where(c => c.IsSelected)
                .ForEach(i =>
                {
                    i.IsSelected = false;
                    i.IsHidden = false;
                });

            await ClearSelectedAsync();

            if (SelectMode == SelectMode.Default)
            {
                await ValueChanged.InvokeAsync(default);
            }
            else
            {
                await ValuesChanged.InvokeAsync(default);
                await Task.Delay(1); // Todo - Workaround because UI does not refresh
                await UpdateOverlayPositionAsync();
                StateHasChanged(); // Todo - Workaround because UI does not refresh
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

        #endregion Events
    }
}
