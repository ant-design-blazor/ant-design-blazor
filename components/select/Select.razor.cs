using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

#pragma warning disable 1591
#pragma warning disable CA1716
#pragma warning disable CA1062

namespace AntDesign
{
    public partial class Select : SelectBase
    {
        #region Private
        #region Constants
        private const string ClassPrefix = "ant-select";
        private const string InputDefaultWidth = "4px";
        #endregion

        #region Fields
        private string _inputWidth = InputDefaultWidth;

        private string _searchValue;
        private string _dropdownStyle;
        private bool _focused;

        private bool _hasInitDropdownStyle = false;
        private SelectOption _searchOption;
        internal ElementReference _inputRef;

        private readonly List<SelectOption> _tagSelectOptions = new List<SelectOption>();
        private readonly SortedSet<string> _tokenSelectOptions = new SortedSet<string>();

        protected OverlayTrigger _dropDown;
        #endregion

        #region Properties
        [Inject]
        private DomEventService DomEventService { get; set; }

        private bool IsTagMode => SelectMode == SelectMode.Tags;

        private bool IsDefaultMode => SelectMode == SelectMode.Default;

        private bool IsComplexMode => SelectMode != SelectMode.Default;
        #endregion

        #region Methods
        private static bool DefaultFilterOption(string value, SelectOption option)
        {
            if (option.Children == null)
            {
                return false;
            }

            var optionContent = option.Children.ToUpperInvariant();
            return optionContent.Contains(value, StringComparison.OrdinalIgnoreCase);
        }
        #endregion
        #endregion

        #region Properties
        public string Title
        {
            get
            {
                if (SelectMode == SelectMode.Default && SelectedOptions.Any())
                {
                    return SelectedOptions.First().Title;
                }
                return null;
            }
        }

        internal bool HasValue
        {
            get
            {
                if (SelectedValues.HasValue)
                {
                    if (SelectMode != SelectMode.Default)
                    {
                        return LabelInValue ? SelectedValues.Value.AsT3.Any() : SelectedValues.Value.AsT1.Any();
                    }

                    return true;
                }

                return false;
            }
        }

        protected bool ShowPlaceholder
        {
            get
            {
                return !HasValue && string.IsNullOrEmpty(_searchValue);
            }
        }

        protected List<SelectOption> SelectOptions { get; } = new List<SelectOption>();

        internal List<SelectOption> SelectedOptions { get; } = new List<SelectOption>();

        protected bool IsShowSearch()
        {
            if (SelectMode == SelectMode.Default)
            {
                return ShowSearch;
            }
            return true;
        }
        #endregion

        #region Methods
        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add($"{ClassPrefix}")
                .If($"{ClassPrefix}-open", () => _dropDown != null ? _dropDown.Visible : false)
                .If($"{ClassPrefix}-focused", () => _focused)
                .If($"{ClassPrefix}-single", () => SelectMode == SelectMode.Default)
                .If($"{ClassPrefix}-multiple", () => SelectMode != SelectMode.Default)
                .If($"{ClassPrefix}-sm", () => Size == AntSizeLDSType.Small)
                .If($"{ClassPrefix}-lg", () => Size == AntSizeLDSType.Large)
                .If($"{ClassPrefix}-borderless", () => !Bordered)
                .If($"{ClassPrefix}-show-arrow", () => ShowArrow)
                .If($"{ClassPrefix}-show-search", () => IsShowSearch())
                .If($"{ClassPrefix}-bordered", () => Bordered)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled);
        }

        protected bool IsEmptyOnSearch()
        {
            if (IsTagMode || (IsDefaultMode && !ShowSearch))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(_searchValue))
            {
                foreach (var option in SelectOptions)
                {
                    if (IsShowOption(option))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        protected bool IsEmptyOnHideSelected()
        {
            if (SelectMode == SelectMode.Multiple && 
                HideSelected && 
                SelectedOptions.Count == SelectOptions.Count)
            {
                return true;
            }

            return false;
        }

        protected bool IsCreatedTagOption()
        {
            if (SelectMode == SelectMode.Tags &&
                !string.IsNullOrEmpty(_searchValue) &&
                !SelectOptions.Any(option => option.Value == _searchValue))
            {
                return true;
            }
            return false;
        }

        protected async Task SetDropdownStyle()
        {
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
 
            _dropdownStyle = $"min-width: {domRect.width}px; width: {domRect.width}px;";
        }

        protected IEnumerable<string> GetTagOptions()
        {
            if (SelectMode == SelectMode.Tags && SelectedValues.HasValue)
            {
                var optionValues = SelectOptions.Select(option => option.Value).ToHashSet();
                if (LabelInValue)
                {
                    var values = SelectedValues.Value.AsT3.ToList();
                    return values.Where(v => !optionValues.Contains(v.Key)).Select(v => v.Key);
                }
                else
                {
                    var values = SelectedValues.Value.AsT1.ToList();
                    return values.Where(v => !optionValues.Contains(v));
                }
            }
            return Array.Empty<string>();
        }

        protected string GetAriaSelected(SelectOption selectOption)
        {
            return SelectedOptions.Contains(selectOption) ? "true" : "false";
        }

        protected OneOf<string, RenderFragment> GetShowValue(SelectOption option)
        {
            if (option.IsSearch || option.IsTag)
            {
                return option.Value;
            }

            return OptionLabelProp switch
            {
                "children" => option?.ChildContent,
                "value" => option?.Value ?? option.Label,
                "label" => option?.Label ?? option.Value,
                _ => SelectMode == SelectMode.Default ? OneOf<string, RenderFragment>.FromT1((option?.ChildContent)) : option?.Label ?? option.Value,
            };
        }

        internal async Task ResetState()
        {
            await Task.Delay(1);
            CurrentValue = null;
            Value = null;
            SelectedValues = null;
            SelectedOptions.Clear();
            OnChange?.Invoke(default, default(SelectOption));
        }
        #endregion

        #region  Events
        protected override void OnInitialized()
        {
            SetClassMap();

            #region Init Values
            if (DefaultValue.HasValue)
            {
                // Set default value
                switch (SelectMode)
                {
                    case SelectMode.Default:
                        if (LabelInValue)
                        {
                            if (DefaultValue.Value.IsT2)
                            {
                                SelectedValues = DefaultValue.Value;

                                // Value 赋值
                                CurrentValueAsString = DefaultValue.Value.AsT2.Key;
                            }
                        }
                        else
                        {
                            if (DefaultValue.Value.IsT0)
                            {
                                SelectedValues = DefaultValue.Value;

                                // Value 赋值
                                CurrentValueAsString = DefaultValue.Value.AsT0;
                            }
                        }
                        break;
                    default:
                        if (LabelInValue)
                        {
                            if (DefaultValue.Value.IsT3)
                            {
                                SelectedValues = DefaultValue.Value;

                                // Value 赋值
                                CurrentValueAsString = string.Join(",", DefaultValue.Value.AsT3.Select(s => s.Key));
                            }
                        }
                        else
                        {
                            if (DefaultValue.Value.IsT1)
                            {
                                SelectedValues = DefaultValue.Value;

                                // Value 赋值
                                CurrentValueAsString = string.Join(",", DefaultValue.Value.AsT1);
                            }
                        }
                        break;
                }
            }
            #endregion 

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            if ((ModalCompleteShow && InModal) && !_hasInitDropdownStyle)
            {
                StateHasChanged();
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if ((ModalCompleteShow || !InModal) && !_hasInitDropdownStyle)
            {
                _hasInitDropdownStyle = true;
                await SetDropdownStyle();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (IsTagMode && _tokenSelectOptions.Any())
            {
                _searchValue = string.Empty;
                _tokenSelectOptions.Clear();
                StateHasChanged();
            }
        }

        protected async Task OnVisibleChange(bool visible)
        {
            OnDropdownVisibleChange?.Invoke(visible);

            SetClassMap();

            if (visible)
            {
                OnFocus?.Invoke();

                _focused = true;

                if (IsShowSearch())
                {
                    await JsInvokeAsync(JSInteropConstants.Focus, _inputRef);

                    StateHasChanged();
                }
            }
            else
            {
                OnOverlayHide();

                _focused = false;
            }
        }

        protected async void OnOverlayHide()
        {
            await TransSearchValueToTag();
        }

        internal async Task TransSearchValueToTag()
        {
            if (IsShowSearch())
            {
                if (IsTagMode && !string.IsNullOrEmpty(_searchValue) && !OptionIsSelected(_searchValue))
                {
                    await ToggleOrSetValue(_searchValue, true);
                }

                if (AutoClearSearchValue)
                {
                    _searchValue = string.Empty;
                }
            }

            OnBlur?.Invoke();
        }

        protected async void OnInput(ChangeEventArgs e)
        {
            if (IsShowSearch())
            {
                _searchValue = e?.Value.ToString();

                if (string.IsNullOrEmpty(_searchValue))
                {
                    _inputWidth = InputDefaultWidth;
                }
                else
                {
                    _inputWidth = $"{4 + _searchValue.Length * 8}px";
                }

                if (!string.IsNullOrEmpty(_searchValue))
                {
                    if (_tokenSeparators != null && _tokenSeparators.Any())
                    {
                        var tokens = _searchValue.Split(_tokenSeparators, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens[0] != _searchValue)
                        {
                            _searchOption = null;
                            foreach (var token in tokens)
                            {
                                if (!OptionIsSelected(token))
                                {
                                    await ToggleOrSetValue(token, false);
                                }
                            }
                        }
                    }
                }

                OnSearch?.Invoke(_searchValue);
            }
        }

        protected async void OnRemoveSelected(SelectOption option)
        {
            var value = option?.Value;
            if (option.IsTag)
            {
                _tagSelectOptions.Remove(option);
            }
            SelectedOptions.Remove(option);

            if (LabelInValue)
            {
                var values = SelectedValues.Value.AsT3 as List<LabeledValue>;
                values.RemoveAll(item => item.Key == value);

                // Value 赋值
                CurrentValueAsString = string.Join(",", values.Select(s => s.Key));
            }
            else
            {
                if (!(SelectedValues.Value.AsT1 is List<string> values))
                {
                    values = new List<string>(SelectedValues.Value.AsT1);
                    SelectedValues = values;
                }
                values.Remove(value);

                // Value 赋值
                CurrentValueAsString = string.Join(",", values);
            }

            OnChange?.Invoke(SelectedValues.Value, SelectedOptions);
        }
        #endregion 

        #region Public
        #region Properties

        #endregion

        #region Methods
        public void RemoveOption(SelectOption selectOption)
        {
            SelectOptions.Remove(selectOption);
        }

        public void AddOption(SelectOption selectOption)
        {
            if (selectOption.IsSearch)
            {
                //_searchOption = selectOption;
                return;
            }

            if (selectOption.IsTag)
            {
                if (!_tagSelectOptions.Contains(selectOption))
                {
                    AddSelectedOption(selectOption);
                    _tagSelectOptions.Add(selectOption);

                    StateHasChanged();
                }

                return;
            }

            SelectOptions.Add(selectOption);
            if (SelectedValues.HasValue)
            {
                switch (SelectMode)
                {
                    case SelectMode.Default:
                        if (LabelInValue)
                        {
                            var value = SelectedValues.Value.AsT2;
                            if (value?.Key == selectOption?.Value)
                            {
                                AddSelectedOption(selectOption);
                            }
                        }
                        else
                        {
                            var value = SelectedValues.Value.AsT0;
                            if (value == selectOption?.Value)
                            {
                                AddSelectedOption(selectOption);
                            }
                        }
                        break;
                    default:
                        if (LabelInValue)
                        {
                            var values = SelectedValues.Value.AsT3;
                            if (values != null && values.Any(value => value.Key == selectOption?.Value))
                            {
                                AddSelectedOption(selectOption);
                            }
                        }
                        else
                        {
                            var values = SelectedValues.Value.AsT1;
                            if (values != null && values.Any(value => value == selectOption?.Value))
                            {
                                AddSelectedOption(selectOption);
                            }
                        }
                        break;
                }
            }

            StateHasChanged();
        }

        private void AddSelectedOption(SelectOption selectOption)
        {
            if (!SelectedOptions.Contains(selectOption))
            {
                SelectedOptions.Add(selectOption);
            }
        }

        public bool OptionIsSelected(string value)
        {
            if (SelectedValues.HasValue)
            {
                switch (SelectMode)
                {
                    case SelectMode.Default:
                        if (LabelInValue)
                        {
                            return SelectedValues.Value.AsT2.Key == value;
                        }
                        else
                        {
                            return SelectedValues.Value.AsT0 == value;
                        }
                    default:
                        if (LabelInValue)
                        {
                            return SelectedValues.Value.AsT3.Any(item => item.Key == value);
                        }
                        else
                        {
                            return SelectedValues.Value.AsT1.Any(item => item == value);
                        }
                }
            }

            return false;
        }

        public async Task ToggleOrSetValue(string value, bool isRrender = true)
        {
            await Task.Delay(1);
            var currentOption = SelectOptions.FirstOrDefault(option => option.Value == value);
            if (IsDefaultMode)
            {
                if (!LabelInValue)
                {
                    SelectedValues = value;
                }
                else
                {
                    SelectedValues = new LabeledValue(value, currentOption.Label);
                }

                // Value 赋值
                CurrentValueAsString = value;

                SelectedOptions.Clear();
                SelectedOptions.Add(currentOption);
            }
            else
            {
                currentOption ??= _tagSelectOptions.FirstOrDefault(option => option.Value == value);
                if (LabelInValue)
                {
                    if (SelectedValues.HasValue)
                    {
                        if (!(SelectedValues.Value.AsT3 is List<LabeledValue>))
                        {
                            SelectedValues = new List<LabeledValue>(SelectedValues.Value.AsT3);
                        }
                    }
                    else
                    {
                        SelectedValues = new List<LabeledValue>();
                    }

                    var values = SelectedValues.Value.AsT3 as List<LabeledValue>;
                    var existValue = values.FirstOrDefault(item => item.Key == value);

                    if (existValue != null)
                    {
                        values.Remove(existValue);
                        SelectedOptions.Remove(currentOption);
                        _tagSelectOptions.Remove(currentOption);
                    }
                    else
                    {
                        values.Add(new LabeledValue(value, currentOption?.Label ?? value));
                        if (currentOption != null)
                        {
                            SelectedOptions.Add(currentOption);
                        }
                        else if (_searchOption != null)
                        {
                            _searchOption.SearchToTag();
                            _tagSelectOptions.Add(_searchOption);
                            SelectedOptions.Add(_searchOption);
                        }
                        else
                        {
                            _tokenSelectOptions.Add(value);
                        }
                    }

                    // Value 赋值
                    CurrentValueAsString = string.Join(",", values.Select(s => s.Key));
                }
                else
                {
                    if (!SelectedValues.HasValue)
                    {
                        SelectedValues = new List<string>();
                    }
                    else if (!(SelectedValues.Value.AsT1 is List<string>))
                    {
                        SelectedValues = new List<string>(SelectedValues.Value.AsT1);
                    }

                    var values = SelectedValues.Value.AsT1 as List<string>;
                    var existValue = values.FirstOrDefault(item => item == value);
                    if (existValue != null)
                    {
                        values.Remove(existValue);
                        SelectedOptions.Remove(currentOption);
                        _tagSelectOptions.Remove(currentOption);
                    }
                    else
                    {
                        values.Add(value);
                        if (currentOption != null)
                        {
                            SelectedOptions.Add(currentOption);
                        }
                        else if (_searchOption != null)
                        {
                            _searchOption.SearchToTag();
                            _tagSelectOptions.Add(_searchOption);
                            SelectedOptions.Add(_searchOption);
                        }
                        else
                        {
                            _tokenSelectOptions.Add(value);
                        }
                    }

                    // Value 赋值
                    CurrentValueAsString = string.Join(",", values);
                }

                // 多选模式换行时, 动态调整下拉框位置
                await SetDropdownStyle();
            }

            OnChange?.Invoke(SelectedValues.Value, currentOption);
            if (isRrender)
            {
                _searchValue = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        public bool IsShowOption(SelectOption option)
        {
            if (HideSelected && SelectedOptions.Any(item => item.Value == option.Value))
            {
                return false;
            }

            if (IsShowSearch() && !string.IsNullOrEmpty(_searchValue))
            {
                if (FilterOption.IsT0 && FilterOption.AsT0)
                {
                    return DefaultFilterOption(_searchValue, option);
                }
                else if (FilterOption.IsT1)
                {
                    return FilterOption.AsT1.Invoke(_searchValue, option);
                }
            }
            return true;
        }

        public async Task ClearAll()
        {
            await ResetState();
        }

        #endregion
        #endregion

        internal async Task Close()
        {
            await _dropDown.Hide(true);
        }
    }
}
