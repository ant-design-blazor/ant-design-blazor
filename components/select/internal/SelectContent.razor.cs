// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign.Select.Internal
{
    public partial class SelectContent<TItemValue, TItem> : AntDomComponentBase
    {
        [CascadingParameter(Name = "ParentSelect")] internal SelectBase<TItemValue, TItem> ParentSelect { get; set; }
        [CascadingParameter(Name = "ParentLabelTemplate")] internal RenderFragment<TItem> ParentLabelTemplate { get; set; }
        [CascadingParameter(Name = "ParentMaxTagPlaceholerTemplate")] internal RenderFragment<IEnumerable<TItem>> ParentMaxTagPlaceholerTemplate { get; set; }
        [CascadingParameter(Name = "ShowSearchIcon")] internal bool ShowSearchIcon { get; set; }
        [CascadingParameter(Name = "ShowArrowIcon")] internal bool ShowArrowIcon { get; set; }

        [Parameter]
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
            }
        }

        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool IsOverlayShow { get; set; }

        [Parameter]
        public int MaxTagCount
        {
            get { return _maxTagCount; }
            set
            {
                if (_maxTagCount != value)
                {
                    _maxTagCount = value;
                    _calculatedMaxCount = _maxTagCount;
                }
            }
        }

        [Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClearClick { get; set; }
        [Parameter] public EventCallback<SelectOptionItem<TItemValue, TItem>> OnRemoveSelected { get; set; }
        [Parameter] public string SearchValue { get; set; }
        [Parameter] public int SearchDebounceMilliseconds { get; set; }
        [Inject] private IDomEventListener DomEventListener { get; set; }

        private const char Ellipse = (char)0x2026;
        private const int ItemMargin = 4; //taken from each tag item
        private string _inputStyle = string.Empty;
        private string _inputWidth;
        private bool _suppressInput;
        private bool _isInitialized;
        private string _prefix;
        private int _calculatedMaxCount;
        private int _lastInputWidth;

        private ElementReference _ref;
        private ElementReference _overflow;
        private ElementReference _aggregateTag;
        private ElementReference _prefixRef;
        private ElementReference _suffixRef;
        private DomRect _overflowElement;
        private DomRect _aggregateTagElement;
        private DomRect _prefixElement = new();
        private DomRect _suffixElement = new();
        private int _currentItemCount;
        private Guid _internalId = Guid.NewGuid();
        private bool _refocus;
        private Timer _debounceTimer;
        private string _inputString;
        private bool _firstRender;

        private bool _compositionInputting;

        protected override void OnInitialized()
        {
            if (!_isInitialized)
            {
                SetInputWidth();
            }
            _isInitialized = true;
            SetSuppressInput();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            SetSuppressInput();
            if (firstRender)
            {
                _firstRender = true;
                if (ParentSelect.IsSearchEnabled)
                {
                    DomEventListener.AddShared<JsonElement>("window", "beforeunload", Reloading);
                    await Js.InvokeVoidAsync(JSInteropConstants.AddPreventKeys, ParentSelect._inputRef, new[] { "ArrowUp", "ArrowDown" });
                    await Js.InvokeVoidAsync(JSInteropConstants.AddPreventEnterOnOverlayVisible, ParentSelect._inputRef, ParentSelect._dropDownRef);
                }
                if (ParentSelect.IsResponsive)
                {
                    _currentItemCount = ParentSelect.SelectedOptionItems.Count;
                    //even though it is run in OnAfterRender, it may happen that the browser
                    //did not manage to render yet the element; force a continuous check
                    //until the element gets the id
                    while (_aggregateTag.Id is null)
                    {
                        await Task.Delay(5);
                    }

                    _aggregateTagElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _aggregateTag, _aggregateTag.Id, true);

                    if (_prefixRef.Id != default)
                    {
                        _prefixElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _prefixRef);
                        _prefixElement.Width += ItemMargin;
                    }
                    if (_suffixRef.Id != default)
                    {
                        _suffixElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _suffixRef);
                        _suffixElement.Width += 7;
                    }
                    await DomEventListener.AddResizeObserver(_overflow, OnOveralyResize);
                    await CalculateResponsiveTags();
                }

                DomEventListener.AddExclusive<JsonElement>(ParentSelect._inputRef, "compositionstart", OnCompositionStart);
                DomEventListener.AddExclusive<JsonElement>(ParentSelect._inputRef, "compositionend", OnCompositionEndAsync);
                DomEventListener.AddExclusive<JsonElement>(ParentSelect._inputRef, "focusout", OnBlurInternal);
                DomEventListener.AddExclusive<JsonElement>(ParentSelect._inputRef, "focus", OnFocusInternal);
            }
            else if (_firstRender && _currentItemCount != ParentSelect.SelectedOptionItems.Count)
            {
                _currentItemCount = ParentSelect.SelectedOptionItems.Count;
                _aggregateTagElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _aggregateTag);
                await CalculateResponsiveTags(_refocus);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task OnOveralyResize(List<ResizeObserverEntry> entries)
        {
            await CalculateResponsiveTags(false, entries[0].ContentRect);
        }

        internal async Task CalculateResponsiveTags(bool forceInputFocus = false, DomRect entry = null)
        {
            if (!ParentSelect.IsResponsive)
                return;

            if (entry is null)
                _overflowElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _overflow);
            else
                _overflowElement = entry;

            //distance between items is margin-inline-left=4px
            decimal accumulatedWidth = _prefixElement.Width + _suffixElement.Width + (4 + (_inputString?.Length ?? 0) * 8);
            int i = 0;
            bool overflowing = false;
            bool renderAgain = false;
            foreach (var item in ParentSelect.SelectedOptionItems)
            {
                if (item.Width == 0)
                {
                    var itemElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, item.SelectedTagRef);
                    item.Width = itemElement.Width;
                }

                if (!overflowing)
                {
                    if (accumulatedWidth + item.Width > _overflowElement.Width)
                    {
                        //current item will overflow; check if with aggregateTag will overflow
                        if (accumulatedWidth + _aggregateTagElement.Width > _overflowElement.Width)
                        {
                            if (_calculatedMaxCount != Math.Max(0, i - 1))
                            {
                                _calculatedMaxCount = Math.Max(0, i - 1);
                                renderAgain = true;
                            }
                        }
                        else //aggregateTag will not overflow, so start aggregating from current item
                        {
                            if (_calculatedMaxCount != i)
                            {
                                _calculatedMaxCount = i;
                                renderAgain = true;
                            }
                        }
                        overflowing = true;
                    }
                    else
                    {
                        accumulatedWidth += item.Width;
                    }
                    i++;
                }
            }
            if (!overflowing && _calculatedMaxCount != i)
            {
                _calculatedMaxCount = i;
                renderAgain = true;
            }
            if (renderAgain)
                StateHasChanged();

            //force focus on cursor
            if (ParentSelect.IsDropdownShown() || forceInputFocus)
            {
                var isFocused = await Js.InvokeAsync<bool>(JSInteropConstants.HasFocus, ParentSelect._inputRef);
                if (!isFocused)
                {
                    await Js.FocusAsync(ParentSelect._inputRef);
                }
            }
        }

        private async Task OnInputChange(ChangeEventArgs e)
        {
            _inputString = e.Value.ToString();
            SetInputWidth();

            if (_compositionInputting)
            {
                return; // Don't trigger search during IME composition
            }

            if (SearchDebounceMilliseconds == 0)
            {
                await InvokeOnInput(e);
                return;
            }

            await DebounceInputChange(e);
        }

        private async Task DebounceInputChange(ChangeEventArgs e, bool ignoreDebounce = false)
        {
            if (ignoreDebounce is false)
            {
                DebounceInput(e);
                return;
            }

            if (_debounceTimer != null)
            {
                await _debounceTimer.DisposeAsync();
                _debounceTimer = null;
            }

            await InvokeOnInput(e);
        }

        private async Task InvokeOnInput(ChangeEventArgs e)
        {
            if (_compositionInputting)
            {
                return; // Double check to prevent search during IME composition
            }

            await OnInput.InvokeAsync(e);
        }

        private void DebounceInput(ChangeEventArgs e)
        {
            _debounceTimer?.Dispose();
            _debounceTimer = new Timer(DebounceTimerIntervalOnTick, e, SearchDebounceMilliseconds, SearchDebounceMilliseconds);
        }

        private void DebounceTimerIntervalOnTick(object state)
        {
            if (_compositionInputting)
            {
                return; // Don't trigger search if still in IME composition
            }

            InvokeAsync(async () => await DebounceInputChange((ChangeEventArgs)state, true));
        }

        internal virtual void OnCompositionStart(JsonElement e)
        {
            _compositionInputting = true;
        }

        internal virtual Task OnCompositionEndAsync(JsonElement e)
        {
            _compositionInputting = false;

            // Trigger search with current input value after IME composition ends
            if (!string.IsNullOrEmpty(_inputString))
            {
                var changeArgs = new ChangeEventArgs { Value = _inputString };
                return OnInputChange(changeArgs);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// clear input value after the dropdown is closed if AutoClearSearchValue=true
        /// </summary>
        internal void ClearInput()
        {
            _inputString = string.Empty;
        }

        /// <summary>
        /// discovery search value after the dropdown is opened
        /// </summary>
        internal void DiscoverySearch()
        {
            _inputString = SearchValue;
        }

        private void SetInputWidth()
        {
            _inputWidth = string.Empty;
            if (ParentSelect.PrefixIcon != null && ParentSelect.Mode == SelectMode.Default)
            {
                _inputWidth = "left: 22px;";
            }
            if (ParentSelect.Mode != SelectMode.Default)
            {
                if (!string.IsNullOrWhiteSpace(_inputString))
                {
                    _inputWidth = $"{_inputWidth}width: {_inputString.Length}em;";
                    if (ParentSelect.IsResponsive && _lastInputWidth != _inputString.Length)
                    {
                        _lastInputWidth = _inputString.Length;
                        InvokeAsync(async () => await CalculateResponsiveTags());
                    }
                }
                else
                {
                    if (ParentSelect.HasValue)
                    {
                        _inputWidth = $"{_inputWidth}width: 4px;"; //ToDo fix class
                    }
                    else if (ParentSelect.PrefixIcon != null)
                    {
                        _inputWidth = $"{_inputWidth}width: 4px; margin-left: 0px;"; //ToDo fix class
                    }
                    else
                    {
                        _inputWidth = $"{_inputWidth}width: 4px; margin-left: 10px;"; //ToDo fix class
                    }
                }
            }
        }

        private void SetSuppressInput()
        {
            if (!ParentSelect.IsSearchEnabled)
            {
                if (!_suppressInput)
                {
                    _suppressInput = true;
                    _inputStyle = "opacity: 0;";
                }
            }
            else
            {
                if (_suppressInput)
                {
                    _suppressInput = false;
                    _inputStyle = string.Empty;
                }
            }
        }

        private string OverflowStyle(int order)
        {
            string width = "max-width: 98%;";
            if (order == 0)
                width = $"max-width: {GetFirstItemMaxWidth()}%;";
            if (ParentSelect.HasTagCount || ParentSelect.IsResponsive)
            {
                if (_calculatedMaxCount < order + 1)
                {
                    return $"opacity: 0.2; order: {order}; height: 0px; overflow-y: hidden; pointer-events: none; {width}";
                }
                return $"opacity: 1; order: {order}; {width}";
            }
            return "opacity: 1;" + width;
        }

        private string FormatLabel(string label)
        {
            if (ParentSelect.MaxTagTextLength > 0)
            {
                return label.Length <= ParentSelect.MaxTagTextLength ? label : label.Substring(0, ParentSelect.MaxTagTextLength) + Ellipse;
            }
            return label;
        }

        private Dictionary<string, object> AdditonalAttributes()
        {
            var dict = new Dictionary<string, object>();

            if (ParentSelect.Disabled)
                dict.Add("tabindex", "-1");

            return dict;
        }

        /// <summary>
        /// Any item may overflow. In case of first item, when there
        /// are any other elements inside SelectContent (prefix, suffix, clear btn, etc)
        /// default MaxWidth will force th SelectContent to grow. Changing the MaxWidth
        /// allows the overflowing item to fit in a single line.
        /// TODO: use relative units
        /// </summary>
        /// <returns></returns>
        private int GetFirstItemMaxWidth()
        {
            int percentValue = 98;
            if (ParentSelect.PrefixIcon != null)
            {
                if (ShowArrowIcon || ShowSearchIcon)
                {
                    if (ParentSelect.AllowClear)
                    {
                        percentValue = 90;
                    }
                    else
                    {
                        percentValue = 93;
                    }
                }
                else
                {
                    percentValue = 94;
                }
            }
            else if (ShowArrowIcon || ShowSearchIcon)
            {
                if (ParentSelect.AllowClear)
                {
                    percentValue = 94;
                }
                else
                {
                    percentValue = 96;
                }
            }
            return percentValue;
        }

        /// <summary>
        /// Indicates that a page is being refreshed
        /// </summary>
        private bool _isReloading;

        private int _maxTagCount;

        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        internal async Task RemovedItem()
        {
            if (ParentSelect.IsResponsive)
            {
                _refocus = true;
                await CalculateResponsiveTags();
            }
        }

        private async Task RemoveClicked(MouseEventArgs e, SelectOptionItem<TItemValue, TItem> selectedOption)
        {
            if (e.Button == 0)
            {
                await OnRemoveSelected.InvokeAsync(selectedOption);
            }
        }

        //TODO: Use built in @onfocus once https://github.com/dotnet/aspnetcore/issues/30070 is solved
        private async Task OnFocusInternal(JsonElement e) => await OnFocus.InvokeAsync(new());

        //TODO: Use built in @onblur once https://github.com/dotnet/aspnetcore/issues/30070 is solved
        private async Task OnBlurInternal(JsonElement e)
        {
            if (_compositionInputting)
            {
                _compositionInputting = false;
            }
            await OnBlur.InvokeAsync(new());
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isReloading)
            {
                _ = InvokeAsync(async () =>
                {
                    await Task.Delay(100);
                    if (ParentSelect.IsResponsive)
                        await DomEventListener.DisposeResizeObserver(_overflow);
                    await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventKeys, ParentSelect._inputRef);
                    await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventEnterOnOverlayVisible, ParentSelect._inputRef);
                });
            }
            DomEventListener?.Dispose();
        }
    }
}
