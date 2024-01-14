// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Helpers.MemberPath;
using AntDesign.JsInterop;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
    [CascadingTypeParameter(nameof(TItemValue))]
#endif

    public partial class Select<TItemValue, TItem> : SelectBase<TItemValue, TItem>
    {
        #region Parameters

        /// <summary>
        /// Toggle the border style.
        /// </summary>
        [Parameter] public bool Bordered { get; set; } = true;

#if NET5_0_OR_GREATER
        /// <summary>
        /// Whether to enable virtualization feature or not, only works for .NET 5 and higher
        /// </summary>
        [Parameter]
        public bool EnableVirtualization { get; set; }
#endif

        private bool _dataSourceHasChanged = false;
        private IEnumerable<TItem> _dataSourceCopy;
        private IEnumerable<TItem> _dataSourceShallowCopy;
        //private bool? _isTItemPrimitive;
        //private bool IsTItemPrimitive
        //{
        //    get
        //    {
        //        if (_isTItemPrimitive is null)
        //        {
        //            _isTItemPrimitive = IsSimpleType(typeof(TItem));
        //        }
        //        return _isTItemPrimitive!.Value;
        //    }
        //}

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
                    DataSourceEqualityComparer = new DataSourceEqualityComparer<TItemValue, TItem>(this);
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
        /// EqualityComparer that will be used during DataSource change
        /// detection. If no comparer set, default .Net is going to be
        /// used.
        /// </summary>
        [Parameter]
        public IEqualityComparer<TItem> DataSourceEqualityComparer { get; set; }

        /// <summary>
        /// Activates the first item that is not deactivated.
        /// </summary>
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
                _getDisabled = string.IsNullOrWhiteSpace(value) ? null : PathHelper.GetDelegate<TItem, bool>(value);
                _disabledName = value;
            }
        }

        /// <summary>
        /// Will match drowdown width:
        /// - for boolean: true - with widest item in the dropdown list
        /// - for string: with value (e.g.: "256px")
        /// </summary>
        [Parameter] public OneOf<bool, string> DropdownMatchSelectWidth { get; set; } = true;

        /// <summary>
        /// Will not allow dropdown width to grow above stated in here value (eg. "768px")
        /// </summary>
        [Parameter] public string DropdownMaxWidth { get; set; } = "auto";

        /// <summary>
        /// Customize dropdown content.
        /// </summary>
        [Parameter] public Func<RenderFragment, RenderFragment> DropdownRender { get; set; }

        /// <summary>
        /// The name of the property to be used as a group indicator.
        /// If the value is set, the entries are displayed in groups.
        /// Use additional SortByGroup and SortByLabel.
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
        [Parameter] public bool IgnoreItemChanges { get; set; } = true;

        /// <summary>
        /// Is used to customize the item style.
        /// </summary>
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

        /// <summary>
        /// The name of the property to be used for the label.
        /// </summary>
        [Parameter]
        public string LabelName
        {
            get => _labelName;
            set
            {
                _getLabel = string.IsNullOrWhiteSpace(value) ? null : PathHelper.GetDelegate<TItem, string>(value);
                if (SelectMode == SelectMode.Tags)
                {
                    _setLabel = string.IsNullOrWhiteSpace(value) ? null : PathHelper.SetDelegate<TItem, string>(value);
                }
                _labelName = value;
            }
        }

        /// <summary>
        /// Is used to customize the label style.
        /// </summary>
        [Parameter] public RenderFragment<TItem> LabelTemplate { get; set; }

        /// <summary>
        /// Placeholder for hidden tags. If used with ResponsiveTag.Responsive, implement your own handling logic.
        /// </summary>
        [Parameter] public RenderFragment<IEnumerable<TItem>> MaxTagPlaceholder { get; set; }

        /// <summary>
        /// Specify content to show when no result matches.
        /// </summary>
        [Parameter] public RenderFragment NotFoundContent { get; set; }

        /// <summary>
        /// Called when blur.
        /// </summary>
        [Parameter] public Action OnBlur { get; set; }

        /// <summary>
        /// Called when custom tag is created.
        /// </summary>

        [Parameter] public Action<string> OnCreateCustomTag { get; set; }

        /// <summary>
        /// Called when the datasource changes. From null to <see cref="IEnumerable{TItem}"/>,
        /// from <see cref="IEnumerable{TItem}"/> to <see cref="IEnumerable{TItem}"/>
        /// or from <see cref="IEnumerable{TItem}"/> to null.
        /// It does not trigger if a value inside the <see cref="IEnumerable{TItem}"/> changes.
        /// </summary>
        [Parameter] public Action OnDataSourceChanged { get; set; }

        /// <summary>
        /// Called when the dropdown visibility changes.
        /// </summary>
        [Parameter] public Action<bool> OnDropdownVisibleChange { get; set; }

        /// <summary>
        /// Called when mouse enter.
        /// </summary>
        [Parameter] public Action OnMouseEnter { get; set; }

        /// <summary>
        /// Called when mouse leave.
        /// </summary>
        [Parameter] public Action OnMouseLeave { get; set; }

        /// <summary>
        /// Callback function that is fired when input changed.
        /// </summary>
        [Parameter] public Action<string> OnSearch { get; set; }

        [Parameter] public string PopupContainerMaxHeight { get; set; } = "256px";

        /// <summary>
        /// Use this to fix overlay problems e.g. #area
        /// </summary>
        [Parameter] public string PopupContainerSelector { get; set; } = "body";

        private bool _showArrowIconChanged;

        /// <summary>
        /// Whether to show the drop-down arrow
        /// </summary>
        [Parameter]
        public bool ShowArrowIcon
        {
            get { return _showArrowIcon; }
            set
            {
                _showArrowIcon = value;
                _showArrowIconChanged = true;
            }
        }

        /// <summary>
        /// Whether show search input in single mode.
        /// </summary>
        [Parameter] public bool ShowSearchIcon { get; set; } = true;

        /// <summary>
        /// Define what characters will be treated as token separators for newly created tags.
        /// Useful when creating new tags using only keyboard.
        /// </summary>
        [Parameter] public char[] TokenSeparators { get; set; }

        /// <summary>
        /// Used for the two-way binding.
        /// </summary>
        [Parameter] public override EventCallback<TItemValue> ValueChanged { get; set; }

        /// <summary>
        /// The name of the property to be used for the value.
        /// </summary>
        [Parameter]
        public string ValueName
        {
            get => _valueName;
            set
            {
                _getValue = string.IsNullOrWhiteSpace(value) ? null : PathHelper.GetDelegate<TItem, TItemValue>(value);
                _setValue = string.IsNullOrWhiteSpace(value) ? null : PathHelper.SetDelegate<TItem, TItemValue>(value);
                _valueName = value;
            }
        }

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
        [Parameter] public Func<TItem, string> LabelProperty { get => _getLabel; set => _getLabel = value; }

        /// <summary>
        /// Specifies the value property in the option object. If use this property, should not use <see cref="ValueName"/>
        /// </summary>
        [Parameter] public Func<TItem, TItemValue> ValueProperty { get => _getValue; set => _getValue = value; }

        /// <summary>
        /// Specifies predicate for disabled options
        /// </summary>
        [Parameter] public Func<TItem, bool> DisabledPredicate { get => _getDisabled; set => _getDisabled = value; }
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

        #endregion Parameters

        [Inject] private IDomEventListener DomEventListener { get; set; }

        #region Properties

        private const string ClassPrefix = "ant-select";

        /// <summary>
        /// Indicates if the GroupName is used. When this value is True, the SelectOptions will be rendered in group mode.
        /// </summary>
        internal bool IsGroupingEnabled
        {
            get => !string.IsNullOrWhiteSpace(GroupName);
        }

        internal ElementReference DropDownRef => _dropDown.GetOverlayComponent().Ref;
        private ElementReference _scrollableSelectDiv;

        private string _dropdownStyle = string.Empty;
        private TItemValue _selectedValue;
        private TItemValue _defaultValue;
        private bool _defaultValueIsNotNull;
        private IEnumerable<TItem> _datasource;
        private bool _afterFirstRender;
        private bool _optionsHasInitialized;
        private bool _defaultValueApplied;
        private bool _defaultActiveFirstOptionApplied;
        private bool _waitingForStateChange;
        private bool _isValueEnum;
        private bool _isToken;
        private bool _defaultActiveFirstOption;

        private string _labelName;

        internal Func<TItem, string> _getLabel;

        private string _groupName = string.Empty;

        private Func<TItem, string> _getGroup;

        private string _disabledName;

        private Func<TItem, bool> _getDisabled;

        private string _valueName;

        internal Func<TItem, TItemValue> _getValue;

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

        protected override void OnInitialized()
        {
            if (SelectOptions == null && typeof(TItemValue) != typeof(TItem) && ValueProperty == null && string.IsNullOrWhiteSpace(ValueName))
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
                if (!_showArrowIconChanged && SelectMode != SelectMode.Default)
                    _showArrowIcon = SuffixIcon != null;
            }
            _isInitialized = true;

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            EvaluateDataSourceChange();
            if (SelectOptions == null)
            {
                if (!_optionsHasInitialized || _dataSourceHasChanged)
                {
                    CreateDeleteSelectOptions();
                    _optionsHasInitialized = true;
                    _dataSourceHasChanged = false;
                }
            }

            if (_valueHasChanged && _optionsHasInitialized)
            {
                _valueHasChanged = false;
                if (Form?.ValidateOnChange == true)
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
                else
                {
                    OnValueChange(_selectedValue);
                }
            }
            base.OnParametersSet();
        }

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
        internal async Task ProcessSelectedSelectOptions()
        {
            if (_isInitialized && _afterFirstRender)
            {
                if (Mode == "default")
                {
                    if (LastValueBeforeReset is not null)
                    {
                        OnValueChange(LastValueBeforeReset);
                        LastValueBeforeReset = default;
                    }
                    else
                    {
                        OnValueChange(Value);
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
                _defaultValueApplied = !(_defaultValueIsNotNull || _defaultValuesHasItems);

                await SetInitialValuesAsync();

                DomEventListener.AddShared<JsonElement>("window", "resize", OnWindowResize);
                await SetDropdownStyleAsync();
                _defaultActiveFirstOptionApplied = !(_defaultActiveFirstOption && SelectedOptionItems.Count == 0);
            }
            if (!_defaultValueApplied || !_defaultActiveFirstOptionApplied)
            {
                if (SelectMode == SelectMode.Default)
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
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
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
            if (_datasource == null)
                return;

            Dictionary<TItem, SelectOptionItem<TItemValue, TItem>> dataStoreToSelectOptionItemsMatch = new();
            // Compare items of SelectOptions and the datastore
            if (SelectOptionItems.Any())
            {
                // Delete items from SelectOptions if it is no longer in the datastore
                foreach (var selectOption in SelectOptionItems)
                {
                    if (!selectOption.IsAddedTag)
                    {
                        var exists = _datasource.FirstOrDefault(x => x.Equals(selectOption.Item));

                        if (exists is null)
                        {
                            SelectOptionItems.Remove(selectOption);
                            RemoveEqualityToNoValue(selectOption);

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
                var groupName = string.Empty;
                var label = _getLabel == null ? GetLabel(item) : _getLabel(item);

                bool isSelected = false;
                if (processedSelectedCount > 0)
                {
                    if (SelectMode == SelectMode.Default)
                        isSelected = ReferenceEquals(value, _selectedValue) || value?.Equals(_selectedValue) == true;
                    else
                        isSelected = _selectedValues.Contains(value);
                }

                if (_getDisabled != default)
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
                    AddEqualityToNoValue(newItem);
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
                        SelectedOptionItems.Add(tag);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the CSS classes to change the visual style
        /// </summary>
        protected override void SetClassMap()
        {
            ClassMapper
                .Add($"{ClassPrefix}")
                .If($"{ClassPrefix}-open", () => _dropDown?.IsOverlayShow() ?? false)
                .If($"{ClassPrefix}-focused", () => Focused)
                .If($"{ClassPrefix}-single", () => SelectMode == SelectMode.Default)
                .If($"{ClassPrefix}-multiple", () => SelectMode != SelectMode.Default)
                .If($"{ClassPrefix}-sm", () => Size == AntSizeLDSType.Small)
                .If($"{ClassPrefix}-lg", () => Size == AntSizeLDSType.Large)
                .If($"{ClassPrefix}-borderless", () => !Bordered)
                .If($"{ClassPrefix}-show-arrow", () => ShowArrowIcon)
                .If($"{ClassPrefix}-show-search", () => IsSearchEnabled)
                .If($"{ClassPrefix}-bordered", () => Bordered)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled)
                .If($"{ClassPrefix}-rtl", () => RTL)

                ;
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
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _selectContent.Ref);
            var width = domRect.Width.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
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

        /// <summary>
        /// Scrolls to the item via JavaScript.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private async Task ElementScrollIntoViewAsync(ElementReference element)
        {
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
                    {
                        SelectedOptionItems.Add(firstEnabled);
                    }
                    else
                    {
                        SelectedOptionItems[0] = firstEnabled;
                    }

                    if (SelectMode == SelectMode.Default)
                    {
                        Value = firstEnabled.Value;
                        firstEnabled.IsSelected = true;
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

                    _waitingForStateChange = true;
                    if (SelectedOptionItems.Count == 0)
                    {
                        SelectedOptionItems.Add(result);
                    }
                    else
                        SelectedOptionItems[0] = result;
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
                    _waitingForStateChange = true;

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
                if (_selectedValue != null || TypeDefaultExistsAsSelectOption)
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
                        await OnSelectedItemChanged.InvokeAsync(result.Item);
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

                    await OnSelectedItemsChanged.InvokeAsync(newSelectedItems);
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

        protected virtual string GetLabel(TItem item)
        {
            return item.ToString();
        }

        #region Events

        /// <summary>
        /// When newly set Value is not found in SelectOptionItems, it is reset to
        /// default. This property holds the value before reset. It may be needed
        /// to be reaplied (for example when new Value is set at the same time
        /// as new SelectOption is added, but Value in the component is set
        /// before new SelectOptionItem has been created).
        /// </summary>
        internal TItemValue LastValueBeforeReset { get; set; }

        /// <summary>
        /// The Method is called every time if the value of the @bind-Value was changed by the two-way binding.
        /// </summary>
        protected override void OnValueChange(TItemValue value)
        {
            if (!_optionsHasInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
                return;

            if (!_isValueEnum && !TypeDefaultExistsAsSelectOption && EqualityComparer<TItemValue>.Default.Equals(value, default))
            {
                _ = InvokeAsync(() => OnInputClearClickAsync(new()));
                return;
            }

            var result = SelectOptionItems.FirstOrDefault(x => EqualityComparer<TItemValue>.Default.Equals(x.Value, value));

            if (result == null)
            {
                if (SelectOptions is not null)
                {
                    LastValueBeforeReset = value;
                }

                if (!AllowClear)
                {
                    _ = TrySetDefaultValueAsync();
                }
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
            //cover special case scenario when SelectItem with Value == default(TItemValue) is requested
            if (TypeDefaultExistsAsSelectOption && ActiveOption != null && EqualityComparer<TItemValue>.Default.Equals(value, default))
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
                if (!SelectedOptionItems[0].Value.Equals(value))
                {
                    SelectedOptionItems[0].IsSelected = false;
                    SelectedOptionItems[0] = optionItem;
                }
            }
            else
            {
                SelectedOptionItems.Add(optionItem);
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

            if (!string.IsNullOrEmpty(_searchValue))
            {
                FilterOptionItems(_searchValue);
            }
            else
            {
                UnhideSelectOptions();
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

                        item.IsHidden = item.IsSelected && HideSelected;
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
                        {
                            item.IsActive = false;
                        }

                        item.IsHidden = item.IsSelected && HideSelected;
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
                        _setLabel?.Invoke(CustomTagSelectOptionItem.Item, _searchValue);
                        _setValue?.Invoke(CustomTagSelectOptionItem.Item, value);
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
                (IsSearchEnabled || AllowClear))
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

                //SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Blur, _inputRef);

                OnBlur?.Invoke();
            }
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

                //it can happen that Ref.Id is null when for example DataSource
                //is set in first OnAfterRender() after initialization
                if (currentSelected.Ref.Id is null)
                {
                    await InvokeAsync(StateHasChanged);
                }
                await ElementScrollIntoViewAsync(currentSelected.Ref);
            }
            else if (ActiveOption == null)//position on first element in the list
            {
                var selectionCandidate = SelectOptionItems.FirstOrDefault();
                if (selectionCandidate != null)
                {
                    ActiveOption = selectionCandidate;
                }
            }
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
