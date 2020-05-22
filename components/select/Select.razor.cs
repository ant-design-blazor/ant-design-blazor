using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
#pragma warning disable 1591
#pragma warning disable CA1716
// ReSharper disable once CheckNamespace

namespace AntBlazor
{
    public partial class Select : AntDomComponentBase
    {
        #region Private
        #region Constants
        private const string RootSelector = "body";
        private const string ClassPrefix = "ant-select";
        #endregion

        #region Fields
        private bool _rootListened = false;
        private bool _isSelectOpened = false;

        private ElementReference _selectRef;
        private ElementReference _dropdownRef;

        private string _dropdownStyle;
        private readonly ClassMapper _dropdownClassMapper = new ClassMapper();
        #endregion

        #region Properties
        [Inject]
        private DomEventService DomEventService { get; set; }
        #endregion
        #endregion

        #region Protected
        #region Fields
        protected List<string> Values { get; } = new List<string>();
        #endregion

        #region Properties
        protected SelectMode SelectMode => Mode.ToSelectMode();

        protected List<SelectOption> SelectOptions { get; } = new List<SelectOption>();

        protected List<SelectOption> SelectedOptions { get; } = new List<SelectOption>();
        #endregion

        #region Methods
        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add($"{ClassPrefix}")
                .Add($"{ClassPrefix}-single")
                .If($"{ClassPrefix}-show-arrow", () => ShowArrow)
                .If($"{ClassPrefix}-show-search", () => ShowSearch)
                .If($"{ClassPrefix}-bordered", () => Bordered)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled);

            _dropdownClassMapper
                .Add($"{ClassPrefix}-placement-bottomLeft")
                .Add($"{ClassPrefix}-dropdown-hidden");
        }

        protected override void OnInitialized()
        {
            SetClassMap();

            #region Init Values
            if (DefaultValue.IsT0)
            {
                Values.Add(DefaultValue.AsT0);
            }

            if (DefaultValue.IsT1)
            {
                Values.AddRange(DefaultValue.AsT1);
            }
            #endregion 

            base.OnInitialized();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            if (!_rootListened)
            {
                var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _selectRef);
                var left = Math.Round(domRect.x);
                var top = Math.Round(domRect.y + domRect.height + 4);

                _dropdownStyle = $"min-width: {domRect.width}px; width: {domRect.width}px; left: {left}px; top: {top}px;";
                DomEventService.AddEventListener(RootSelector, "click", OnSelectHideClick);
                await InvokeAsync(StateHasChanged);

                _rootListened = true;
            }
        }

        protected bool TryGetSelectOption(out SelectOption selectOption)
        {
            selectOption = null;
            if (DefaultValue.IsT0)
            {
                var defaultValue = DefaultValue.AsT0;
                selectOption = SelectOptions.FirstOrDefault(option => option.Value == defaultValue);
            }
            if (DefaultValue.IsT1)
            {
                var defaultValues = DefaultValue.AsT1;
                selectOption = SelectOptions.FirstOrDefault(option => defaultValues.Contains(option.Value));
            }
            else
            {
                return false;
            }

            return selectOption != null;
        }

        protected string GetAriaSelected(SelectOption selectOption)
        {
            return SelectedOptions.Contains(selectOption) ? "true" : "false";
        }

        protected void SetHideClass()
        {
            if (_isSelectOpened)
            {
                _dropdownClassMapper.Clear()
                    .Add($"{ClassPrefix}-dropdown-hidden")
                    .Add($"{ClassPrefix}-placement-bottomLeft");
                _isSelectOpened = false;
            }
        }
        #endregion

        #region  Events
        protected async Task OnSelectOpenClick(MouseEventArgs _)
        {
            if (!_isSelectOpened && !Disabled)
            {
                await JsInvokeAsync(JSInteropConstants.addElementToBody, _dropdownRef);

                _dropdownClassMapper.Clear()
                    .Add($"{ClassPrefix}-placement-bottomLeft");
                await InvokeAsync(StateHasChanged);
                _isSelectOpened = true;
            }
        }

        protected async void OnSelectHideClick(JsonElement _)
        {
            SetHideClass();
            await InvokeAsync(StateHasChanged);
        }

        protected async Task OnClearClick(MouseEventArgs _)
        {
            Values.Clear();
            SelectedOptions.Clear();
            await InvokeAsync(StateHasChanged);
        }
        #endregion
        #endregion

        #region Public
        #region Properties
        #region Generals
        [Inject]
        public IconService IconService { get; set; }
        #endregion

        #region Parameters
        #region Boolean(13)
        [Parameter] public bool AllowClear { get; set; } = false;

        [Parameter] public bool AutoClearSearchValue { get; set; } = true;//Only model is multiple or tags

        [Parameter] public bool AutoFocus { get; set; } = false;

        [Parameter] public bool DefaultActiveFirstOption { get; set; } = true;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool LabelInValue { get; set; } = false;

        [Parameter] public bool ShowArrow { get; set; } = true;

        [Parameter] public bool ShowSearch { get; set; } = false;

        [Parameter] public bool Virtual { get; set; } = true;

        [Parameter] public bool Loading { get; set; } = false;

        [Parameter] public bool Bordered { get; set; } = true;

        [Parameter] public bool? Open { get; set; } = true;

        [Parameter] public bool? DefaultOpen { get; set; } = true;
        #endregion

        #region String(11)
        [Parameter] public string Mode { get; set; }

        [Parameter] public string Size { get; set; } = AntSizeLDSType.Default;

        [Parameter] public string Placeholder { get; set; }

        [Parameter] public string OptionLabelProp { get; set; }

        [Parameter] public string OptionFilterProp { get; set; }

        [Parameter] public string DropdownClassName { get; set; }

        [Parameter] public string SuffixIcon { get; set; }

        [Parameter] public string RemoveIcon { get; set; }

        [Parameter] public string ClearIcon { get; set; }

        [Parameter] public string MenuItemSelectedIcon { get; set; }

        [Parameter] public string DropdownStyle { get; set; }
        #endregion

        #region Number(3)
        [Parameter] public int ListHeight { get; set; } = 256;

        [Parameter] public int? MaxTagCount { get; set; }

        [Parameter] public int? MaxTagTextLength { get; set; }
        #endregion

        #region Complex
        [Parameter] public OneOf<string, string[]> DefaultValue { get; set; }

        [Parameter] public OneOf<bool, int> DropdownMatchSelectWidth { get; set; }

        [Parameter] public OneOf<bool, Func<string, SelectOption, bool>> FilterOption { get; set; } = false;
        #endregion

        #region Fragment
        [Parameter] public RenderFragment ChildContent { get; set; }
        #endregion

        #region Event(11)
        [Parameter]
        public EventCallback OnBlur { get; set; }

        [Parameter]
        public EventCallback OnSearch { get; set; }

        [Parameter]
        public EventCallback OnSelect { get; set; }

        [Parameter]
        public EventCallback OnPopupScroll { get; set; }

        [Parameter]
        public EventCallback OnDropdownVisibleChange { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnDeselect { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnInputKeyDown { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseEnter { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseLeave { get; set; }
        #endregion
        #endregion
        #endregion

        #region Methods
        public void Blur()
        {
        }

        public void Focus()
        {
        }

        public void AddOption(SelectOption selectOption)
        {
            SelectOptions.Add(selectOption);

            switch (SelectMode)
            {
                case SelectMode.Default:
                    var value = Values.FirstOrDefault();
                    if (value != null && value == selectOption?.Value)
                    {
                        SelectedOptions.Add(selectOption);
                        StateHasChanged();
                    }
                    break;
                default:
                    if (Values.Contains(selectOption?.Value))
                    {
                        SelectedOptions.Add(selectOption);
                        StateHasChanged();
                    }
                    break;
            }
        }

        public bool OptionIsSelected(string value)
        {
            return Values.Contains(value);
        }

        public async Task AddOrSetValue(string value)
        {
            switch (SelectMode)
            {
                case SelectMode.Default:
                    SetHideClass();
                    Values.Clear();
                    Values.Add(value);

                    SelectedOptions.Clear();
                    SelectedOptions.Add(SelectOptions.First(option => option.Value == value));

                    break;
                default:
                    break;
            }

            await InvokeAsync(StateHasChanged);
        }
        #endregion
        #endregion
    }
}
