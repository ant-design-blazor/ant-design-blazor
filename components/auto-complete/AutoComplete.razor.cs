// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Autocomplete function of input field.</para>
     
    <h2>When To Use</h2>
    When there is a need for autocomplete functionality.
    </summary>
    <inheritdoc />
    <seealso cref="AutoCompleteOption" />
    <seealso cref="TriggerBoundaryAdjustMode"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/qtJm4yt45/AutoComplete.svg", Title = "AutoComplete", SubTitle = "自动完成")]
    public partial class AutoComplete<TOption> : AntInputComponentBase<string>, IAutoCompleteRef
    {
        /// <summary>
        /// Input element placeholder
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// Disable
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Make first option active by default or not
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool DefaultActiveFirstOption { get; set; } = true;

        /// <summary>
        /// Backfill selected item into the input when using keyboard to select items
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Backfill { get; set; } = false;

        /// <summary>
        /// Delays the processing of the KeyUp event until the user has stopped
        /// typing for a predetermined amount of time
        /// </summary>
        /// <default value="250"/>
        [Parameter]
        public int DebounceMilliseconds { get; set; } = 250;

        /// <summary>
        /// List object collection
        /// </summary>
        private List<AutoCompleteOption> AutoCompleteOptions { get; set; } = new List<AutoCompleteOption>();

        /// <summary>
        /// List data collection
        /// </summary>
        private List<AutoCompleteDataItem<TOption>> _optionDataItems = new List<AutoCompleteDataItem<TOption>>();

        /// <summary>
        /// List bound data source collection
        /// </summary>
        private IEnumerable<TOption> _options;

        /// <summary>
        /// Options to display in dropdown
        /// </summary>
        [Parameter]
        public IEnumerable<TOption> Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
                _optionDataItems = _options?.Select(x => new AutoCompleteDataItem<TOption>(x, x.ToString())).ToList() ?? new List<AutoCompleteDataItem<TOption>>();
            }
        }

        /// <summary>
        /// Bind the data source of the list data item format
        /// </summary>
        [Parameter]
        public IEnumerable<AutoCompleteDataItem<TOption>> OptionDataItems
        {
            get
            {
                return _optionDataItems;
            }
            set
            {
                _optionDataItems = value.ToList();
            }
        }

        /// <summary>
        /// Callback executed when selection changes
        /// </summary>
        [Parameter]
        public EventCallback<AutoCompleteOption> OnSelectionChange { get; set; }

        /// <summary>
        /// Callback executed when active item changes
        /// </summary>
        [Parameter]
        public EventCallback<AutoCompleteOption> OnActiveChange { get; set; }

        /// <summary>
        /// Callback executed when input changes
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Callback executed when panel visibility changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnPanelVisibleChange { get; set; }

        /// <summary>
        /// Content for dropdown
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Option template
        /// </summary>
        [Parameter]
        public RenderFragment<AutoCompleteDataItem<TOption>> OptionTemplate { get; set; }

        /// <summary>
        /// Formatting options, you can customize the display format
        /// </summary>
        [Parameter]
        public Func<AutoCompleteDataItem<TOption>, string> OptionFormat { get; set; }

        /// <summary>
        /// All option templates
        /// </summary>
        [Parameter]
        public RenderFragment OverlayTemplate { get; set; }

        /// <summary>
        /// Contrast, used to compare whether two objects are the same
        /// </summary>
        [Parameter]
        public Func<object, object, bool> CompareWith { get; set; } = (o1, o2) => o1?.ToString() == o2?.ToString();

        /// <summary>
        /// Filter expression
        /// </summary>
        [Parameter]
        public Func<AutoCompleteDataItem<TOption>, string, bool> FilterExpression { get; set; } = (option, value) => string.IsNullOrEmpty(value) ? false : option.Label.Contains(value, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Allow filtering
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool AllowFilter { get; set; }

        /// <summary>
        /// Width of input, pixels when an int is given, full value given to CSS width property when a string is given
        /// </summary>
        [Parameter]
        public OneOf<int?, string> Width { get; set; }

        /// <summary>
        /// Class name passed to overlay
        /// </summary>
        [Parameter]
        public string OverlayClassName { get; set; }

        /// <summary>
        /// Style passed to overlay
        /// </summary>
        [Parameter]
        public string OverlayStyle { get; set; }

        /// <summary>
        /// Container selector for the popup
        /// </summary>
        /// <default value="body" />
        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        /// <summary>
        /// Selected item from dropdown
        /// </summary>
        [Parameter]
        public AutoCompleteOption SelectedItem { get; set; }

        /// <summary>
        /// Overlay adjustment strategy (when for example browser resize is happening). Check 
        /// </summary>
        /// <default value="TriggerBoundaryAdjustMode.InView"/>
        [Parameter]
        public TriggerBoundaryAdjustMode BoundaryAdjustMode { get; set; } = TriggerBoundaryAdjustMode.InView;

        /// <summary>
        /// Display options dropdown
        /// </summary>
        /// <default value="false" />
        [Parameter]
        [Obsolete("This property is useless, please remove it.")]
        public bool ShowPanel { get; set; } = false;

        [Inject] private IDomEventListener DomEventListener { get; set; }
        object IAutoCompleteRef.SelectedValue { get => _selectedValue; set => _selectedValue = value; }
        object IAutoCompleteRef.ActiveValue { get => _activeValue; set => _activeValue = value; }

        private bool _isOptionsZero = true;

        private IAutoCompleteInput _inputComponent;

        private IList<AutoCompleteDataItem<TOption>> _filteredOptions;

        private string _minWidth = "";
        private bool _parPanelVisible = false;

        private OverlayTrigger _overlayTrigger;

        private object _selectedValue;
        private object _activeValue;
        private bool _isFocused = false;

        private bool _isOpened = false;

        void IAutoCompleteRef.SetInputComponent(IAutoCompleteInput input)
        {
            _inputComponent = input;
        }

        #region 子控件触发事件 / Child controls trigger events

        async Task IAutoCompleteRef.InputFocus(FocusEventArgs e)
        {
            _isFocused = true;
            if (!_isOptionsZero)
            {
                await this.OpenPanel();
            }
        }

        Task IAutoCompleteRef.InputBlur(FocusEventArgs e)
        {
            _isFocused = false;
            return Task.CompletedTask;
        }

        async Task IAutoCompleteRef.InputInput(ChangeEventArgs args)
        {
            if (OnInput.HasDelegate) await OnInput.InvokeAsync(args);
        }

        async Task IAutoCompleteRef.InputValueChange(string value)
        {
            _selectedValue = value;
            UpdateFilteredOptions();
            await ResetActiveItem();
        }

        async Task IAutoCompleteRef.InputKeyDown(KeyboardEventArgs args)
        {
            var key = args.Key;

            if (this._isOpened)
            {
                if (key == "Escape" || key == "Tab")
                {
                    await this.ClosePanel();
                }
                else if (key == "Enter" && this._activeValue != null)
                {
                    await SetSelectedItem(GetActiveItem());
                }
            }
            else if (!_isOptionsZero)
            {
                await this.OpenPanel();
            }

            if (key == "ArrowUp")
            {
                this.SetPreviousItemActive();
            }
            else if (key == "ArrowDown")
            {
                this.SetNextItemActive();
            }
        }

        #endregion 子控件触发事件 / Child controls trigger events

        protected override void OnInitialized()
        {
            _isOptionsZero = Options?.Any() != true;
            base.OnInitialized();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await SetOverlayWidth();
            await DomEventListener.AddResizeObserver(_overlayTrigger.RefBack.Current, UpdateWidth);
            await base.OnFirstAfterRenderAsync();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var optionsChanged = parameters.IsParameterChanged(nameof(Options), Options)
                || parameters.IsParameterChanged(nameof(OptionDataItems), OptionDataItems);

            await base.SetParametersAsync(parameters);

            if (optionsChanged)
            {
                UpdateFilteredOptions();
            }
        }

        void IAutoCompleteRef.AddOption(AutoCompleteOption option)
        {
            AutoCompleteOptions.Add(option);
        }

        void IAutoCompleteRef.RemoveOption(AutoCompleteOption option)
        {
            if (AutoCompleteOptions?.Contains(option) == true)
                AutoCompleteOptions?.Remove(option);
        }

        private IList<AutoCompleteDataItem<TOption>> GetOptionItems()
        {
            if (_optionDataItems != null)
            {
                if (FilterExpression != null && AllowFilter == true && !string.IsNullOrEmpty(_selectedValue?.ToString()))
                    return _optionDataItems.Where(x => FilterExpression(x, _selectedValue?.ToString())).ToList();
                else
                    return _optionDataItems;
            }
            else
            {
                return new List<AutoCompleteDataItem<TOption>>();
            }
        }

        /// <summary>
        /// Open panel
        /// </summary>
        private async Task OpenPanel()
        {
            if (this._isOpened == false)
            {
                this._isOpened = true;

                await _overlayTrigger.Show();

                await ResetActiveItem();
                StateHasChanged();
            }
        }

        /// <summary>
        /// Close panel
        /// </summary>
        private async Task ClosePanel()
        {
            if (this._isOpened == true)
            {
                this._isOpened = false;

                await _overlayTrigger.Close();

                StateHasChanged();
            }
        }

        private AutoCompleteOption GetActiveItem()
        {
            return AutoCompleteOptions.FirstOrDefault(x => CompareWith(x.Value, this._activeValue));
        }

        //设置高亮的对象
        //Set the highlighted object
        private void SetActiveItem(AutoCompleteOption item)
        {
            this._activeValue = item == null ? default(TOption) : item.Value;
            if (OnActiveChange.HasDelegate) OnActiveChange.InvokeAsync(item);
            StateHasChanged();
        }

        //设置下一个激活
        //Set the next activation
        private void SetNextItemActive()
        {
            var opts = AutoCompleteOptions.Where(x => x.Disabled == false).ToList();
            var nextItem = opts.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == opts.Count - 1)
                SetActiveItem(opts.FirstOrDefault());
            else
                SetActiveItem(opts[nextItem + 1]);

            if (Backfill)
                _inputComponent.SetValue(this._activeValue);
        }

        //设置上一个激活
        //Set last activation
        private void SetPreviousItemActive()
        {
            var opts = AutoCompleteOptions.Where(x => x.Disabled == false).ToList();
            var nextItem = opts.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == 0)
                SetActiveItem(opts.LastOrDefault());
            else
                SetActiveItem(opts[nextItem - 1]);

            if (Backfill)
                _inputComponent.SetValue(this._activeValue);
        }

        private void UpdateFilteredOptions()
        {
            _filteredOptions = GetOptionItems();
            _isOptionsZero = _filteredOptions.Count == 0;
            if (_isFocused && !_isOptionsZero)
            {
                _ = OpenPanel();
            }
        }

        private async Task ResetActiveItem()
        {
            if (_overlayTrigger != null)
            {
                // if options count == 0 then close overlay
                if (_isOptionsZero && _overlayTrigger.IsOverlayShow())
                {
                    await ClosePanel();
                }
                // if options count > 0 then open overlay
                else if (!_isOptionsZero && !_overlayTrigger.IsOverlayShow())
                {
                    await OpenPanel();
                }
            }

            var items = _filteredOptions;

            if (items.Any(x => CompareWith(x.Value, this._activeValue)) == false)
            {
                // 如果当前激活项找在列表中不存在，那么我们需要做一些处理
                // If the current activation item does not exist in the list, then we need to do some processing
                if (items.Any(x => CompareWith(x.Value, this._selectedValue)))
                {
                    this._activeValue = this._selectedValue;
                }
                else if (DefaultActiveFirstOption == true && items.Count > 0)
                {
                    this._activeValue = items.FirstOrDefault()?.Value.ToString();
                }
                else
                {
                    this._activeValue = null;
                }
            }

        }

        private async Task SetSelectedItem(AutoCompleteOption item)
        {
            if (item != null)
            {
                this._selectedValue = item.Value;
                this.SelectedItem = item;
                _inputComponent?.SetValue(this.SelectedItem.Label);

                if (OnSelectionChange.HasDelegate) await OnSelectionChange.InvokeAsync(this.SelectedItem);
            }
            this.ClosePanel();
        }

        private async Task OnOverlayTriggerVisibleChange(bool visible)
        {
            if (OnPanelVisibleChange.HasDelegate && _parPanelVisible != visible)
            {
                _parPanelVisible = visible;
                await OnPanelVisibleChange.InvokeAsync(visible);
            }

            if (this._isOpened != visible)
            {
                this._isOpened = visible;
            }
        }

        private async Task UpdateWidth(List<ResizeObserverEntry> entry)
        {
            await SetOverlayWidth();
            InvokeStateHasChanged();
        }

        private async Task SetOverlayWidth()
        {
            HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _overlayTrigger.RefBack.Current);
            _minWidth = $"min-width:{element.ClientWidth}px";
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.RemoveResizeObserver(_overlayTrigger.RefBack.Current, UpdateWidth);
            base.Dispose(disposing);
        }

        void IAutoCompleteRef.SetActiveItem(AutoCompleteOption item)
        {
            SetActiveItem(item);
        }

        async Task IAutoCompleteRef.SetSelectedItem(AutoCompleteOption item)
        {
            await SetSelectedItem(item);
        }
    }

    public class AutoCompleteDataItem<TOption>
    {
        public AutoCompleteDataItem()
        {
        }

        public AutoCompleteDataItem(TOption value, string label)
        {
            Value = value;
            Label = label;
        }

        public TOption Value { get; set; }
        public string Label { get; set; }

        public bool IsDisabled { get; set; }
    }
}
