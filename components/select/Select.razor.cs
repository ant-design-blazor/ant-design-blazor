using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

#pragma warning disable 1591
#pragma warning disable CA1716
#pragma warning disable CA1062

namespace AntDesign
{
    public partial class Select : AntDomComponentBase
    {
        #region Private

        #region Constants

        private const string RootSelector = "body";
        private const string ClassPrefix = "ant-select";
        private const string InputDefaultWidth = "4px";
        #endregion

        #region Fields
        private string _inputWidth = InputDefaultWidth;
        private bool _isSelectOpened = false;
        private bool _isPreventPenetration = false;

        private ElementReference _inputRef;
        private ElementReference _dropdownRef;

        private string _searchValue;
        private string _dropdownStyle;
        private readonly ClassMapper _dropdownClassMapper = new ClassMapper();

        private string _currentTag = string.Empty;
        private readonly List<string> _tagOptions = new List<string>();
        private OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>? _value;
        #endregion

        #region Properties

        [Inject]
        private DomEventService DomEventService { get; set; }
        #endregion

        #region Methods
        private static bool DefaultFilterOption(string value, SelectOption option)
        {
            var optionContent = option.Children.ToUpperInvariant();
            return optionContent.Contains(value, StringComparison.OrdinalIgnoreCase);
        }
        #endregion
        #endregion

        #region Protected
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

        protected bool HasValue
        {
            get
            {
                if (Value.HasValue)
                {
                    if (SelectMode != SelectMode.Default)
                    {
                        if (LabelInValue)
                        {
                            return Value.Value.AsT3.Any();
                        }
                        else
                        {
                            return Value.Value.AsT1.Any();
                        }
                    }
                    else
                    {
                        return true;
                    }
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

        protected List<SelectOption> SelectedOptions { get; } = new List<SelectOption>();

        protected Properties GetProperties(SelectOption option)
        {
            Action<MouseEventArgs> action = e => OnRemoveSelected(option);

            return new Properties
            {
                Closable = true,
                Value = option.Value,
                Label = option.Label,
                OnClose = action
            };
        }

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
        protected OneOf<string, RenderFragment> GetShowValue(SelectOption option)
        {
            return OptionLabelProp switch
            {
                "children" => option?.ChildContent,
                "value" => option?.Value ?? option.Label,
                "label" => option?.Label ?? option.Value,
                _ => SelectMode == SelectMode.Default ? OneOf<string, RenderFragment>.FromT1((option?.ChildContent)) : option?.Label ?? option.Value,
            };
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add($"{ClassPrefix}")
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

            _dropdownClassMapper.Clear()
                .Add($"{ClassPrefix}-dropdown-hidden")
                .Add($"{ClassPrefix}-placement-bottomLeft");
        }

        protected void SetHideClass()
        {
            if (_isSelectOpened)
            {
                SetClassMap();
                _isSelectOpened = false;
            }
        }

        protected async Task SetDropdownStyle()
        {
            var scrollPoint = await JsInvokeAsync<Point>(JSInteropConstants.getScroll);
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, Ref);

            var left = Math.Round(domRect.left + scrollPoint.x);
            var top = Math.Round(domRect.top + scrollPoint.y + domRect.height + 4);
            _dropdownStyle = $"min-width: {domRect.width}px; width: {domRect.width}px; left: {left}px; top: {top}px;";
        }

        protected string GetAriaSelected(SelectOption selectOption)
        {
            return SelectedOptions.Contains(selectOption) ? "true" : "false";
        }

        protected IEnumerable<string> GetTagOptions()
        {
            if (SelectMode == SelectMode.Tags && Value.HasValue)
            {
                var optionValues = SelectOptions.Select(option => option.Value).ToHashSet();
                if (LabelInValue)
                {
                    var values = Value.Value.AsT3.ToList();
                    return values.Where(v => !optionValues.Contains(v.Key)).Select(v => v.Key);
                }
                else
                {
                    var values = Value.Value.AsT1.ToList();
                    return values.Where(v => !optionValues.Contains(v));
                }
            }
            return Array.Empty<string>();
        }

        protected bool IsEmptyOnSearch()
        {
            if (SelectMode == SelectMode.Multiple && !string.IsNullOrEmpty(_searchValue))
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
        #endregion

        #region  Events
        protected override void OnInitialized()
        {
            SetClassMap();

            #region Init Values
            if (DefaultValue.HasValue)
            {// Set default value
                switch (SelectMode)
                {
                    case SelectMode.Default:
                        if (LabelInValue)
                        {
                            if (DefaultValue.Value.IsT2)
                            {
                                Value = DefaultValue.Value;
                            }
                        }
                        else
                        {
                            if (DefaultValue.Value.IsT0)
                            {
                                Value = DefaultValue.Value;
                            }
                        }
                        break;
                    default:
                        if (LabelInValue)
                        {
                            if (DefaultValue.Value.IsT3)
                            {
                                Value = DefaultValue.Value;
                            }
                        }
                        else
                        {
                            if (DefaultValue.Value.IsT1)
                            {
                                Value = DefaultValue.Value;
                            }
                        }
                        break;
                }
            }
            #endregion 

            base.OnInitialized();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await SetDropdownStyle();
            DomEventService.AddEventListener(RootSelector, "click", OnSelectHideClick);
            await InvokeAsync(StateHasChanged);
        }

        protected async Task OnSelectOpenClick(MouseEventArgs _)
        {
            if (!_isSelectOpened && !Disabled)
            {
                _isSelectOpened = true;
                ClassMapper
                    .Add($"{ClassPrefix}-open")
                    .If($"{ClassPrefix}-focused", () => IsShowSearch());
                _dropdownClassMapper.Clear()
                    .Add($"{ClassPrefix}-placement-bottomLeft");


                OnFocus?.Invoke();
                await SetDropdownStyle();
                await InvokeAsync(StateHasChanged);
                await JsInvokeAsync(JSInteropConstants.addElementToBody, _dropdownRef);

                if (IsShowSearch())
                {
                    await JsInvokeAsync(JSInteropConstants.focus, _inputRef);
                }
                OnDropdownVisibleChange?.Invoke(true);
            }
        }

        protected async void OnSelectHideClick(JsonElement _)
        {
            if (!IsPreventPenetration)
            {
                if (IsShowSearch() && AutoClearSearchValue)
                {
                    _searchValue = string.Empty;
                }

                SetHideClass();
                OnBlur?.Invoke();
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task OnClearClick(MouseEventArgs _)
        {
            Value = null;
            SelectedOptions.Clear();
            OnChange?.Invoke(default, default(SelectOption));

            await InvokeAsync(StateHasChanged);
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
                OnSearch?.Invoke(_searchValue);
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async void OnRemoveSelected(SelectOption option)
        {
            var value = option?.Value;
            SelectedOptions.Remove(option);
            if (LabelInValue)
            {
                var values = Value.Value.AsT3 as List<LabeledValue>;
                values.RemoveAll(item => item.Key == value);
            }
            else
            {
                if (!(Value.Value.AsT1 is List<string> values))
                {
                    values = new List<string>(Value.Value.AsT1);
                    Value = values;
                }
                values.Remove(value);
            }

            OnChange?.Invoke(Value.Value, SelectedOptions);
            await InvokeAsync(StateHasChanged);
        }

        #endregion Events

        #endregion Protected

        #region Public

        #region Properties
        #region Parameters(51)
        #region Boolean(15)
        [Parameter] public bool AutoFocus { get; set; } = false;

        [Parameter] public bool AllowClear { get; set; } = false;

        [Parameter] public bool AutoClearSearchValue { get; set; } = true;

        [Parameter] public bool DefaultActiveFirstOption { get; set; } = true;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool LabelInValue { get; set; } = false;

        [Parameter] public bool ShowArrow { get; set; } = true;

        [Parameter] public bool ShowSearch { get; set; } = false;

        [Parameter] public bool Virtual { get; set; } = true;

        [Parameter] public bool Loading { get; set; } = false;

        [Parameter] public bool Bordered { get; set; } = true;

        [Parameter] public bool Open { get; set; } = false;

        [Parameter] public bool DefaultOpen { get; set; } = false;

        [Parameter] public bool HideSelected { get; set; } = false;
        #endregion

        #region String(7)
        [Parameter] public string Mode { get; set; }

        [Parameter] public string Placeholder { get; set; }

        [Parameter] public string DropdownStyle { get; set; }

        [Parameter] public string OptionLabelProp { get; set; }

        [Parameter] public string DropdownClassName { get; set; }

        [Parameter] public string OptionFilterProp { get; set; } = "value";

        [Parameter] public string Size { get; set; } = AntSizeLDSType.Default;
        #endregion

        #region Number(3)
        [Parameter] public int? MaxTagCount { get; set; }

        [Parameter] public int? MaxTagTextLength { get; set; }

        [Parameter] public int ListHeight { get; set; } = 256;
        #endregion

        #region Array(2)
        [Parameter] public IEnumerable<LabeledValue> Options { get; set; }

        [Parameter] public IEnumerable<string> TokenSeparators { get; set; }
        #endregion

        #region Complex(5)
        [Parameter] public OneOf<bool, int>? DropdownMatchSelectWidth { get; set; }

        [Parameter]
        public OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>? Value
        {
            get => _value;
            set
            {
                if (value.HasValue)
                {
                    if (SelectMode == SelectMode.Default)
                    {
                        if (LabelInValue)
                        {
                            if (value.Value.AsT2 != null)
                            {
                                _value = value;
                            }
                        }
                        else if (value.Value.AsT0 != null)
                        {
                            _value = value;
                        }
                    }
                    else
                    {
                        if (LabelInValue)
                        {
                            if (value.Value.AsT3 != null)
                            {
                                _value = value;
                            }
                        }
                        else if (value.Value.AsT1 != null)
                        {
                            _value = value;
                        }
                    }
                }
            }
        }

        [Parameter] public OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>? DefaultValue { get; set; }

        [Parameter] public OneOf<bool, Func<string, SelectOption, bool>> FilterOption { get; set; } = true;

        [Parameter] public OneOf<RenderFragment, Func<string[], RenderFragment>>? MaxTagPlaceholder { get; set; }
        #endregion

        #region RenderFragment(6)
        [Parameter] public RenderFragment ClearIcon { get; set; }

        [Parameter] public RenderFragment RemoveIcon { get; set; }

        [Parameter] public RenderFragment SuffixIcon { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment NotFoundContent { get; set; }

        [Parameter] public RenderFragment MenuItemSelectedIcon { get; set; }
        #endregion

        #region Function(14)
        [Parameter] public Action OnBlur { get; set; }

        [Parameter] public Action OnFocus { get; set; }

        [Parameter] public Action OnMouseEnter { get; set; }

        [Parameter] public Action OnMouseLeave { get; set; }

        [Parameter] public Action OnPopupScroll { get; set; }

        [Parameter] public Action OnInputKeyDown { get; set; }

        [Parameter] public Action<string> OnSearch { get; set; }

        [Parameter] public Action<bool> OnDropdownVisibleChange { get; set; }

        [Parameter] public Action<OneOf<string, LabeledValue>> OnDeselect { get; set; }

        [Parameter] public Action<OneOf<string, LabeledValue>, SelectOption> OnSelect { get; set; }

        [Parameter] public Func<Properties, RenderFragment> TagRender { get; set; }

        [Parameter] public Func<ElementReference, ElementReference> GetPopupContainer { get; set; }

        [Parameter] public Func<RenderFragment, Properties, RenderFragment> DropdownRender { get; set; }

        [Parameter] public Action<OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>, OneOf<SelectOption, IEnumerable<SelectOption>>> OnChange { get; set; }
        #endregion
        #endregion

        #region Generals
        public bool IsPreventPenetration
        {
            get
            {
                if (_isPreventPenetration)
                {
                    _isPreventPenetration = false;
                    return true;
                }
                return false;
            }
            set
            {
                _isPreventPenetration = value;
            }
        }

        public SelectMode SelectMode => Mode.ToSelectMode();
        #endregion
        #endregion

        #region Methods

        public void Blur()
        {
            OnSelectHideClick(default);
        }

        public async void Focus()
        {
            await OnSelectOpenClick(null);
        }

        public void AddOption(SelectOption selectOption)
        {
            if (selectOption.IsTagOption)
            {
                _currentTag = selectOption.Value;
            }
            else
            {
                SelectOptions.Add(selectOption);
            }

            if (Value.HasValue)
            {
                switch (SelectMode)
                {
                    case SelectMode.Default:
                        if (LabelInValue)
                        {
                            var value = Value.Value.AsT2;
                            if (value?.Key == selectOption?.Value)
                            {
                                SelectedOptions.Add(selectOption);
                                StateHasChanged();
                            }
                        }
                        else
                        {
                            var value = Value.Value.AsT0;
                            if (value == selectOption?.Value)
                            {
                                SelectedOptions.Add(selectOption);
                                StateHasChanged();
                            }
                        }
                        break;
                    default:
                        if (LabelInValue)
                        {
                            var values = Value.Value.AsT3;
                            if (values != null && values.Any(value => value.Key == selectOption?.Value))
                            {
                                SelectedOptions.Add(selectOption);
                                StateHasChanged();
                            }
                        }
                        else
                        {
                            var values = Value.Value.AsT1;
                            if (values != null && values.Any(value => value == selectOption?.Value))
                            {
                                SelectedOptions.Add(selectOption);
                                StateHasChanged();
                            }
                        }
                        break;
                }
            }
        }

        public bool OptionIsSelected(string value)
        {
            if (Value.HasValue)
            {
                switch (SelectMode)
                {
                    case SelectMode.Default:
                        if (LabelInValue)
                        {
                            return Value.Value.AsT2.Key == value;
                        }
                        else
                        {
                            return Value.Value.AsT0 == value;
                        }
                    default:
                        if (LabelInValue)
                        {
                            return Value.Value.AsT3.Any(item => item.Key == value);
                        }
                        else
                        {
                            return Value.Value.AsT1.Any(item => item == value);
                        }
                }
            }

            return false;
        }

        public async Task ToggleOrSetValue(string value)
        {
            var select = SelectOptions.FirstOrDefault(option => option.Value == value);
            switch (SelectMode)
            {
                case SelectMode.Default:
                    SetHideClass();

                    if (LabelInValue)
                    {
                        Value = new LabeledValue(value, select.Label);
                    }
                    else
                    {
                        Value = value;
                    }

                    SelectedOptions.Clear();
                    SelectedOptions.Add(select);
                    OnChange?.Invoke(Value.Value, select);
                    break;

                default:
                    var tagSelect = _tagOptions.FirstOrDefault(tag => tag == value);
                    if (LabelInValue)
                    {
                        if (Value.HasValue)
                        {
                            if (!(Value.Value.AsT3 is List<LabeledValue>))
                            {
                                Value = new List<LabeledValue>(Value.Value.AsT3);
                            }
                        }
                        else
                        {
                            Value = new List<LabeledValue>();
                        }

                        var values = Value.Value.AsT3 as List<LabeledValue>;
                        var existValue = values.FirstOrDefault(item => item.Key == value);
                        if (existValue != null)
                        {
                            values.Remove(existValue);

                            if (select != null)
                            {
                                SelectedOptions.Remove(select);
                            }
                            else if (tagSelect != null)
                            {
                                _tagOptions.Remove(tagSelect);
                            }
                        }
                        else
                        {
                            values.Add(new LabeledValue(value, select.Label));

                            if (select != null)
                            {
                                SelectedOptions.Add(select);
                            }
                            else if (tagSelect != null)
                            {
                                _tagOptions.Add(tagSelect);
                            }
                        }
                    }
                    else
                    {
                        if (Value.HasValue)
                        {
                            if (!(Value.Value.AsT1 is List<string>))
                            {
                                Value = new List<string>(Value.Value.AsT1);
                            }
                        }
                        else
                        {
                            Value = new List<string>();
                        }

                        var values = Value.Value.AsT1 as List<string>;
                        var existValue = values.FirstOrDefault(item => item == value);
                        if (existValue != null)
                        {
                            values.Remove(existValue);

                            if (select != null)
                            {
                                SelectedOptions.Remove(select);
                            }
                            else if (tagSelect != null)
                            {
                                _tagOptions.Remove(tagSelect);
                            }
                        }
                        else
                        {
                            values.Add(value);
                            if (select != null)
                            {
                                SelectedOptions.Add(select);
                            }
                            else if (tagSelect != null)
                            {
                                _tagOptions.Add(tagSelect);
                            }
                        }
                    }

                    OnChange?.Invoke(Value.Value, select);
                    break;
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
                if (FilterOption.IsT0)
                {
                    if (FilterOption.AsT0)
                    {
                        return DefaultFilterOption(_searchValue, option);
                    }
                }
                else
                {
                    return FilterOption.AsT1.Invoke(_searchValue, option);
                }
            }
            return true;
        }
        #endregion
        #endregion
    }
}
