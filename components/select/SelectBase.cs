﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#region using block

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.Select;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

#endregion

namespace AntDesign
{
    public abstract class SelectBase<TItemValue, TItem> : AntInputComponentBase<TItemValue>
    {
        protected const string DefaultWidth = "width: 100%;";
        protected bool TypeDefaultExistsAsSelectOption { get; set; } = false; //this is to indicate that value was set outside - basically to monitor for scenario when Value is set to default(Value)
        private SelectOptionItem<TItemValue, TItem> _selectOptionEqualToTypeDefault;
        private SelectOptionItem<TItemValue, TItem> _activeOption;
        protected OverlayTrigger _dropDown;

        internal ElementReference _dropDownRef;

        internal ElementReference _inputRef;
        protected bool _isInitialized;


        protected bool _isPrimitive;

        /// <summary>
        ///     How long (number of characters) a tag will be.
        ///     Only for Mode = "multiple" or Mode = "tags"
        /// </summary>
        /// <value>
        ///     The maximum length of the tag text.
        /// </value>
        private OneOf<int, ResponsiveTag> _maxTagCount;

        protected int _maxTagCountAsInt;
        protected string _prevSearchValue = string.Empty;
        protected string _searchValue = string.Empty;


        protected SelectContent<TItemValue, TItem> _selectContent;

        protected IEnumerable<TItemValue> _selectedValues;

        protected Action<TItem, string> _setLabel;

        protected Action<TItem, TItemValue> _setValue;
        /// <summary>
        /// Show clear button. Has no effect if <see cref="AntInputComponentBase{TValue}.Value"/> type default 
        /// is also in the list of <see cref="SelectOption{TItemValue, TItem}"/>, 
        /// unless used with <see cref="ValueOnClear"/>.
        /// </summary>
        [Parameter] public bool AllowClear { get; set; }
        /// <summary>
        /// Whether the current search will be cleared on selecting an item.
        /// </summary>
        [Parameter] public bool AutoClearSearchValue { get; set; } = true;
        /// <summary>
        /// Whether the Select component is disabled.
        /// </summary>
        [Parameter] public bool Disabled { get; set; }
        /// <summary>
        /// Set mode of Select - default | multiple | tags
        /// </summary>
        [Parameter] public string Mode { get; set; } = "default";
        /// <summary>
        /// Indicates whether the search function is active or not. Always true for mode tags.
        /// </summary>
        [Parameter] public bool EnableSearch { get; set; }
        /// <summary>
        /// Show loading indicator. You have to write the loading logic on your own.
        /// </summary>
        [Parameter] public bool Loading { get; set; }
        /// <summary>
        /// Controlled open state of dropdown.
        /// </summary>
        [Parameter] public bool Open { get; set; }
        /// <summary>
        /// Placeholder of select.
        /// </summary>
        [Parameter] public string Placeholder { get; set; }
        /// <summary>
        /// Called when focus.
        /// </summary>
        [Parameter] public Action OnFocus { get; set; }
        /// <summary>
        /// The name of the property to be used as a group indicator. 
        /// If the value is set, the entries are displayed in groups. 
        /// Use additional SortByGroup and SortByLabel.
        /// </summary>
        [Parameter] public SortDirection SortByGroup { get; set; } = SortDirection.None;
        /// <summary>
        /// Sort items by label value. None | Ascending | Descending
        /// </summary>
        [Parameter] public SortDirection SortByLabel { get; set; } = SortDirection.None;
        /// <summary>
        /// Hides the selected items when they are selected.
        /// </summary>
        [Parameter] public bool HideSelected { get; set; }
        /// <summary>
        /// Used for the two-way binding.
        /// </summary>
        [Parameter] public override EventCallback<TItemValue> ValueChanged { get; set; }
        /// <summary>
        /// Used for the two-way binding.
        /// </summary>
        [Parameter] public EventCallback<IEnumerable<TItemValue>> ValuesChanged { get; set; }
        /// <summary>
        /// The custom suffix icon.
        /// </summary>
        [Parameter] public RenderFragment SuffixIcon { get; set; }
        /// <summary>
        /// The custom prefix icon.
        /// </summary>
        [Parameter] public RenderFragment PrefixIcon { get; set; }

        protected IEnumerable<TItemValue> _defaultValues;
        protected bool _defaultValuesHasItems;
        /// <summary>
        /// Used when Mode =  multiple | tags - The values are used during initialization and when pressing the Reset button within Forms.
        /// </summary>
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

        /// <summary>
        /// Called when the user clears the selection.
        /// </summary>
        [Parameter] public Action OnClearSelected { get; set; }

        internal bool IsResponsive { get; set; }

        internal HashSet<SelectOptionItem<TItemValue, TItem>> SelectOptionItems { get; } = new();
        internal List<SelectOptionItem<TItemValue, TItem>> SelectedOptionItems { get; } = new();
        /// <summary>
        /// Called when the selected item changes.
        /// </summary>
        [Parameter] public Action<TItem> OnSelectedItemChanged { get; set; }
        /// <summary>
        /// Called when the selected items changes.
        /// </summary>
        [Parameter] public Action<IEnumerable<TItem>> OnSelectedItemsChanged { get; set; }

        internal virtual SelectMode SelectMode => Mode.ToSelectMode();


        /// <summary>
        ///     Currently active (highlighted) option.
        ///     It does not have to be equal to selected option.
        /// </summary>
        internal SelectOptionItem<TItemValue, TItem> ActiveOption
        {
            get => _activeOption;
            set
            {
                if (_activeOption != value)
                {
                    if (_activeOption != null && _activeOption.IsActive)
                    {
                        _activeOption.IsActive = false;
                    }

                    _activeOption = value;
                    if (_activeOption != null && !_activeOption.IsActive)
                    {
                        _activeOption.IsActive = true;
                    }
                }
            }
        }

        /// <summary>
        /// Get or set the selected values.
        /// </summary>
        [Parameter]
        public virtual IEnumerable<TItemValue> Values
        {
            get => _selectedValues;
            set
            {
                if (value != null && _selectedValues != null)
                {
                    var hasChanged = !value.SequenceEqual(_selectedValues);

                    if (!hasChanged)
                    {
                        return;
                    }

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

                if (_isNotifyFieldChanged && Form?.ValidateOnChange == true)
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }
        }

        /// <summary>
        ///     Converts custom tag (a string) to TItemValue type.
        /// </summary>
        [Parameter]
        public Func<string, TItemValue> CustomTagLabelToValue { get; set; } =
            label => (TItemValue)TypeDescriptor.GetConverter(typeof(TItemValue)).ConvertFromInvariantString(label);


        /// <summary>
        ///     Determines if SelectOptions has any selected items
        /// </summary>
        /// <returns>true if SelectOptions has any selected Items, otherwise false</returns>
        internal bool HasValue => SelectOptionItems.Any(x => x.IsSelected);

        /// <summary>
        ///     Returns whether the user can input a pattern to search matched items
        /// </summary>
        /// <returns>true if search is enabled</returns>
        internal bool IsSearchEnabled => EnableSearch || SelectMode == SelectMode.Tags;

        /// <summary>
        ///     Sorted list of SelectOptionItems
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

                if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.None)
                {
                    return selectOption.OrderByDescending(g => g.GroupName);
                }

                if (SortByGroup == SortDirection.None && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderBy(l => l.Label);
                }

                if (SortByGroup == SortDirection.None && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderByDescending(l => l.Label);
                }

                if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderBy(g => g.GroupName).ThenBy(l => l.Label);
                }

                if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderBy(g => g.GroupName).OrderByDescending(l => l.Label);
                }

                if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderByDescending(g => g.GroupName).ThenBy(l => l.Label);
                }

                if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderByDescending(g => g.GroupName).OrderByDescending(l => l.Label);
                }

                return selectOption;
            }
        }

        /// <summary>
        /// Used for rendering select options manually.
        /// </summary>
        [Parameter] public RenderFragment SelectOptions { get; set; }


        internal List<SelectOptionItem<TItemValue, TItem>> AddedTags { get; } = new();

        internal SelectOptionItem<TItemValue, TItem> CustomTagSelectOptionItem { get; set; }

        internal bool Focused { get; set; }
        internal bool HasTagCount { get; set; }

        /// <summary>
        /// How long (number of characters) a tag will be.
        /// Only for Mode = "multiple" or Mode = "tags"
        /// </summary>
        /// <value>
        /// The maximum length of the tag text.
        /// </value>
        [Parameter] public int MaxTagTextLength { get; set; }

        /// <summary>
        /// Whether to embed label in value, turn the format of value from TItemValue to string (JSON) 
        /// e.g. { "value": TItemValue, "label": "Label value" }
        /// </summary>
        [Parameter]
        public bool LabelInValue { get; set; }

        /// <summary>
        ///     Max tag count to show. responsive will cost render performance.
        /// </summary>
        [Parameter]
        public OneOf<int, ResponsiveTag> MaxTagCount
        {
            get => _maxTagCount;
            set
            {
                _maxTagCount = value;

                value.Switch(intValue =>
                {
                    IsResponsive = false;
                    HasTagCount = intValue > 0;
                    _maxTagCountAsInt = intValue;
                }, enumValue =>
                {
                    IsResponsive = enumValue == ResponsiveTag.Responsive;
                    HasTagCount = false;
                });
            }
        }

        private bool _hasValueOnClear;
        private TItemValue _valueOnClear;
        /// <summary>
        /// When Clear button is pressed, Value will be set to
        /// whatever is set in ValueOnClear
        /// </summary>
        [Parameter]
        public TItemValue ValueOnClear
        {
            get => _valueOnClear;
            set
            {
                _hasValueOnClear = true;
                _valueOnClear = value;
            }
        }

        /// <summary>
        ///     Returns a true/false if the placeholder should be displayed or not.
        /// </summary>
        /// <returns>true if SelectOptions has no values and the searchValue is empty; otherwise false </returns>
        protected bool ShowPlaceholder => !HasValue && string.IsNullOrEmpty(_searchValue);


        /// <summary>
        ///     The Method is called every time if the value of the @bind-Values was changed by the two-way binding.
        /// </summary>
        protected async Task OnValuesChangeAsync(IEnumerable<TItemValue> values)
        {
            if (
                !_isInitialized) // This is important because otherwise the initial value is overwritten by the EventCallback of ValueChanged and would be NULL.
            {
                return;
            }

            if (!SelectOptionItems.Any())
            {
                return;
            }

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
        ///     When bind-Values is changed outside of the component, then component
        ///     selected items have to be reselected according to new values passed.
        ///     TODO: (Perf) Consider using hash to identify if the passed values are different from currently selected.
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
                    result = SelectOptionItems.FirstOrDefault(x =>
                        !x.IsSelected && EqualityComparer<TItemValue>.Default.Equals(x.Value, value));
                    if (result != null && !result.IsDisabled)
                    {
                        result.IsSelected = true;
                        SelectedOptionItems.Add(result);
                    }

                    deselectList.Remove(value);
                }
                else
                {
                    result = SelectOptionItems.FirstOrDefault(x =>
                        EqualityComparer<TItemValue>.Default.Equals(x.Value, value));
                    if (result is null) //tag delivered from outside, needs to be added to the list of options
                    {
                        result = CreateSelectOptionItem(value.ToString(), true);
                        result.IsSelected = true;
                        AddedTags.Add(result);
                        SelectOptionItems.Add(result);
                        AddEqualityToNoValue(result);
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
                    RemoveEqualityToNoValue(item.Value);
                    if (item.Value.IsAddedTag)
                    {
                        SelectOptionItems.Remove(item.Value);
                        AddedTags.Remove(item.Value);
                    }
                }
            }
        }

        /// <summary>
        ///     Creates the select option item. Mostly meant to create new tags, that is why IsAddedTag is hardcoded to true.
        /// </summary>
        /// <param name="label">Creation based on passed label</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        /// <returns></returns>
        protected SelectOptionItem<TItemValue, TItem> CreateSelectOptionItem(string label, bool isActive)
        {
            var value = CustomTagLabelToValue.Invoke(label);
            TItem item;
            if (_isPrimitive)
            {
                item = (TItem)TypeDescriptor.GetConverter(typeof(TItem)).ConvertFromInvariantString(_searchValue);
            }
            else
            {
                if (_setValue == null)
                {
                    item = THelper.ChangeType<TItem>(value);
                }
                else
                {
                    item = Activator.CreateInstance<TItem>();
                    _setValue(item, value);
                }

                _setLabel?.Invoke(item, _searchValue);
            }

            return new SelectOptionItem<TItemValue, TItem>
            {
                Label = label,
                Value = value,
                Item = item,
                IsActive = isActive,
                IsSelected = false,
                IsAddedTag = true
            };
        }

        protected bool IsOptionEqualToNoValue(SelectOptionItem<TItemValue, TItem> option)
            => EqualityComparer<TItemValue>.Default.Equals(option.Value, default);

        internal void RemoveEqualityToNoValue(SelectOptionItem<TItemValue, TItem> option)
        {
            if (TypeDefaultExistsAsSelectOption)
            {
                if (IsOptionEqualToNoValue(option))
                {
                    TypeDefaultExistsAsSelectOption = false;
                } // Same as TypeDefaultExistsAsSelectOption = !IsOptionEqualToNoValue(option); since TypeDefaultExistsAsSelectOption is already true
                if (!TypeDefaultExistsAsSelectOption)
                {
                    _selectOptionEqualToTypeDefault = null;
                }
            }
        }

        internal void AddEqualityToNoValue(SelectOptionItem<TItemValue, TItem> option)
        {
            if (!TypeDefaultExistsAsSelectOption)
            {
                TypeDefaultExistsAsSelectOption = IsOptionEqualToNoValue(option);
                if (TypeDefaultExistsAsSelectOption)
                {
                    _selectOptionEqualToTypeDefault = option;
                }
            }
        }

        internal bool IsDropdownShown()
        {
            return _dropDown.IsOverlayShow();
        }

        protected override void OnInitialized()
        {
            SetClassMap();

            if (string.IsNullOrWhiteSpace(Style))
            {
                Style = DefaultWidth;
            }

            _isInitialized = true;

            base.OnInitialized();
        }

        protected void OnOverlayHide()
        {
            if (!IsSearchEnabled)
            {
                return;
            }

            if (!AutoClearSearchValue)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_searchValue))
            {
                return;
            }

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
        ///     A separate method to invoke ValuesChanged and OnSelectedItemsChanged to reduce code duplicates.
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
                    var valueLabel = new ValueLabel<TItemValue>
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
                if (TypeDefaultExistsAsSelectOption && IsOptionEqualToNoValue(selectOptionItem))
                {
                    StateHasChanged();
                }
            }
        }


        /// <summary>
        ///     The method is called every time if the user select/de-select a item by mouse or keyboard.
        ///     Don't change the IsSelected property outside of this function.
        /// </summary>
        protected internal async Task SetValueAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null)
            {
                throw new ArgumentNullException(nameof(selectOption));
            }

            if (SelectMode == SelectMode.Default)
            {
                if (SelectedOptionItems.Count > 0)
                {
                    SelectedOptionItems[0].IsSelected = false;
                    SelectedOptionItems[0] = selectOption;
                }
                else
                {
                    SelectedOptionItems.Add(selectOption);
                }
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
                    {
                        selectOption.IsHidden = true;
                    }

                    if (IsSearchEnabled && !string.IsNullOrWhiteSpace(_searchValue))
                    {
                        ClearSearch();
                    }

                    if (selectOption.IsAddedTag)
                    {
                        CustomTagSelectOptionItem = null;
                        AddedTags.Add(selectOption);
                        if (!SelectOptionItems.Any(x => x.Value.Equals(selectOption.Value)))
                        {
                            SelectOptionItems.Add(selectOption);
                        }
                    }
                }
                else
                {
                    if (selectOption.IsHidden)
                    {
                        selectOption.IsHidden = false;
                    }

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
                    {
                        await _selectContent.RemovedItem();
                    }
                }

                if (IsSearchEnabled)
                {
                    await SetInputFocusAsync();
                }

                await InvokeValuesChanged(selectOption);
                await UpdateOverlayPositionAsync();
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
            {
                await ValuesChanged.InvokeAsync(newSelectedValues);
            }
            else
            {
                Values = newSelectedValues;
                StateHasChanged();
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
                        if (HideSelected && !item.IsSelected || !HideSelected)
                        {
                            item.IsHidden = false;
                        }
                    }
                }

                foreach (var item in AddedTags)
                {
                    if (item.IsHidden)
                    {
                        if (HideSelected && !item.IsSelected || !HideSelected)
                        {
                            item.IsHidden = false;
                        }
                    }
                }
            }

            _searchValue = string.Empty;
            _prevSearchValue = string.Empty;
        }


        /// <summary>
        ///     Method is called via EventCallBack after the user clicked on the Clear icon inside the Input element.
        ///     Set the IsSelected and IsHidden properties for all items to False. It updates the overlay position if
        ///     the SelectMode is Tags or Multiple. Invoke the OnClearSelected Action. Set the Value(s) to default.
        /// </summary>
        protected async Task OnInputClearClickAsync(MouseEventArgs _)
        {
            if (SelectMode == SelectMode.Default)
            {
                await ClearDefaultMode();
            }
            else
            {
                await ClearMultipleMode();
            }
            OnClearSelected?.Invoke();
        }

        private async Task ClearMultipleMode()
        {
            List<SelectOptionItem<TItemValue, TItem>> tagItems = new();
            SelectOptionItems.Where(c => c.IsSelected)
                .ForEach(i =>
                {
                    i.IsSelected = false;
                    i.IsHidden = false;
                    if (i.IsAddedTag)
                    {
                        tagItems.Add(i);
                    }
                });
            //When clearing, also remove all added tags that are kept after adding in SelectOptionItems
            if (tagItems.Count > 0)
            {
                foreach (var item in tagItems)
                {
                    SelectOptionItems.Remove(item);
                }
            }

            await ClearSelectedAsync();

            AddedTags.Clear();
            ActiveOption = SelectOptionItems.FirstOrDefault();
            CustomTagSelectOptionItem = null;
            SelectedOptionItems.Clear();

            await Task.Delay(1); // Todo - Workaround because UI does not refresh
            await UpdateOverlayPositionAsync();
            StateHasChanged(); // Todo - Workaround because UI does not refresh
        }

        private async Task ClearDefaultMode()
        {
            if (_hasValueOnClear && EqualityComparer<TItemValue>.Default.Equals(Value, _valueOnClear))
            {
                return; //nothing to do, already cleared; mostly to avoid redoing OnInputClearClickAsync when issued from OnParameterSet() => OnValueChange() => OnInputClearClickAsync()
            }
            if (!TypeDefaultExistsAsSelectOption && !_hasValueOnClear)
            {
                SelectedOptionItems[0].IsSelected = false;
                SelectedOptionItems[0].IsHidden = false;

                ActiveOption = SelectOptionItems.FirstOrDefault();
                SelectedOptionItems.Clear();
                Value = default;
                await ClearSelectedAsync();
            }
            else if (_hasValueOnClear)
            {
                var selectOption = SelectOptionItems.Where(o => EqualityComparer<TItemValue>.Default.Equals(o.Value, _valueOnClear)).FirstOrDefault();
                if (selectOption != null)
                {
                    Value = selectOption.Value;
                    await SetValueAsync(selectOption);
                }
                else
                {
                    SelectedOptionItems[0].IsSelected = false;
                    SelectedOptionItems[0].IsHidden = false;

                    ActiveOption = SelectOptionItems.FirstOrDefault();
                    SelectedOptionItems.Clear();
                    Value = _valueOnClear;
                    await ValueChanged.InvokeAsync(_valueOnClear);
                }
            }
            else
            {
                if (SelectedOptionItems[0].InternalId != _selectOptionEqualToTypeDefault.InternalId)
                {
                    SelectedOptionItems[0].IsSelected = false;
                    SelectedOptionItems[0] = _selectOptionEqualToTypeDefault;
                    SelectedOptionItems[0].IsSelected = true;
                    Value = _selectOptionEqualToTypeDefault.Value;
                    await ValueChanged.InvokeAsync(_valueOnClear);
                }
                else
                {
                    return; //ValueOnClear already selected, no need to do anything;
                }
            }
            ActiveOption = _selectOptionEqualToTypeDefault;
        }

        /// <summary>
        ///     Check if Focused property is False; Set the Focused property to true, change the
        ///     style and set the Focus on the Input element via DOM. It also invoke the OnFocus Action.
        /// </summary>
        protected async Task SetInputFocusAsync()
        {
            if (!Focused)
            {
                Focused = true;

                SetClassMap();

                await FocusAsync(_inputRef);

                OnFocus?.Invoke();
            }
        }

        /// <summary>
        ///     Inform the Overlay to update the position.
        /// </summary>
        internal async Task UpdateOverlayPositionAsync()
        {
            if (_dropDown.Visible)
            {
                await _dropDown.GetOverlayComponent().UpdatePosition();
            }
        }


        internal async Task OnArrowClick(MouseEventArgs args)
        {
            await _dropDown.OnClickDiv(args);
        }

        /// <summary>
        ///     Close the overlay
        /// </summary>
        /// <returns></returns>
        internal async Task CloseAsync()
        {
            await _dropDown.Hide(true);
        }

        /// <summary>
        ///     Called by the Form reset method
        /// </summary>
        internal override void ResetValue()
        {
            _ = ClearSelectedAsync();
        }


        /// <summary>
        ///     Clears the selectValue(s) property and send the null(default) value back through the two-way binding.
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

        protected abstract void SetClassMap();
    }
}
