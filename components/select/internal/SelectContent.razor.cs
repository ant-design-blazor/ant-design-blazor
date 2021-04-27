using System;
using System.Collections.Generic;
using System.Text.Json;
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
    public partial class SelectContent<TItemValue, TItem>: IDisposable
    {
        [CascadingParameter(Name = "ParentSelect")] internal Select<TItemValue, TItem> ParentSelect { get; set; }
        [CascadingParameter(Name = "ParentLabelTemplate")] internal RenderFragment<TItem> ParentLabelTemplate { get; set; }
        [CascadingParameter(Name = "ParentMaxTagPlaceholerTemplate")] internal RenderFragment<IEnumerable<TItem>> ParentMaxTagPlaceholerTemplate { get; set; }
        [CascadingParameter(Name = "ShowSearchIcon")] internal bool ShowSearchIcon { get; set; }
        [CascadingParameter(Name = "ShowArrowIcon")] internal bool ShowArrowIcon { get; set; }
        [Parameter]
        public string Prefix
        {
            get { return _prefix; }
            set {
                _prefix = value;
                if (_isInitialized)
                    SetInputWidth();
            }
        }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool IsOverlayShow { get; set; }
        [Parameter] public bool ShowPlaceholder { get; set; }
        [Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClearClick { get; set; }
        [Parameter] public EventCallback<SelectOptionItem<TItemValue, TItem>> OnRemoveSelected { get; set; }
        [Parameter] public string SearchValue { get; set; }
        [Parameter] public ForwardRef RefBack { get; set; } = new ForwardRef();
        [Inject] protected IJSRuntime Js { get; set; }
        [Inject] private DomEventService DomEventService { get; set; }
        protected ElementReference Ref
        {
            get { return _ref; }
            set
            {
                _ref = value;
                RefBack?.Set(value);
            }
        }

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
                if (ParentSelect.EnableSearch)
                {
                    DomEventService.AddEventListener("window", "beforeunload", Reloading, false);
                    await Js.InvokeVoidAsync(JSInteropConstants.AddPreventKeys, ParentSelect._inputRef, new[] { "ArrowUp", "ArrowDown" });
                    await Js.InvokeVoidAsync(JSInteropConstants.AddPreventEnterOnOverlayVisible, ParentSelect._inputRef, ParentSelect.DropDownRef);
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
                    await DomEventService.AddResizeObserver(_overflow, OnOveralyResize);
                    await CalculateResponsiveTags();
                }
                DomEventService.AddEventListener(ParentSelect._inputRef, "focusout", OnBlurInternal, true);
                DomEventService.AddEventListener(ParentSelect._inputRef, "focus", OnFocusInternal, true);
            }
            else if (_currentItemCount != ParentSelect.SelectedOptionItems.Count)
            {
                _currentItemCount = ParentSelect.SelectedOptionItems.Count;
                _aggregateTagElement = await Js.InvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _aggregateTag);
                await CalculateResponsiveTags(_refocus);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async void OnOveralyResize(List<ResizeObserverEntry> entries)
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
            decimal accumulatedWidth = _prefixElement.Width + _suffixElement.Width + (4 + (SearchValue?.Length ?? 0) * 8);
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

        private void SetInputWidth()
        {
            _inputWidth = string.Empty;
            if (ParentSelect.PrefixIcon != null && ParentSelect.SelectMode == SelectMode.Default)
            {
                _inputWidth = "left: 22px;";
            }
            if (ParentSelect.SelectMode != SelectMode.Default)
            {
                if (!string.IsNullOrWhiteSpace(SearchValue))
                {
                    _inputWidth = $"{_inputWidth}width: {4 + SearchValue.Length * 8}px;";
                    if (ParentSelect.IsResponsive && _lastInputWidth != SearchValue.Length)
                    {
                        _lastInputWidth = SearchValue.Length;
                        InvokeAsync(async() => await CalculateResponsiveTags());
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
                    _inputStyle = "caret-color: transparent;";
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

        protected void OnKeyPressEventHandler(KeyboardEventArgs _)
        {
            if (!ParentSelect.IsSearchEnabled)
                SearchValue = string.Empty;
            else if (ParentSelect.IsResponsive)
            {

            }
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

        private void Reloading(JsonElement jsonElement) => _isReloading = true;

        internal async Task RemovedItem()
        {
            if (ParentSelect.IsResponsive)
            {
                _refocus = true;
                await CalculateResponsiveTags();
            }
        }

        private async Task OnClearClickAsync(MouseEventArgs args)
        {
            await OnClearClick.InvokeAsync(args);
        }

        //TODO: Use built in @onfocus once https://github.com/dotnet/aspnetcore/issues/30070 is solved
        private async void OnFocusInternal(JsonElement e) => await OnFocus.InvokeAsync(new());

        //TODO: Use built in @onblur once https://github.com/dotnet/aspnetcore/issues/30070 is solved
        private async void OnBlurInternal(JsonElement e) => await OnBlur.InvokeAsync(new());


        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isReloading)
            {
                _ = InvokeAsync(async () =>
                {
                    await Task.Delay(100);
                    if (ParentSelect.IsResponsive)
                        await DomEventService.DisposeResizeObserver(_overflow);
                    await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventKeys, ParentSelect._inputRef);
                    await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventEnterOnOverlayVisible, ParentSelect._inputRef);
                });
            }
            DomEventService.RemoveEventListerner<JsonElement>(ParentSelect._inputRef, "focus", OnFocusInternal);
            DomEventService.RemoveEventListerner<JsonElement>(ParentSelect._inputRef, "focusout", OnBlurInternal);
            DomEventService.RemoveEventListerner<JsonElement>("window", "beforeunload", Reloading);

            if (IsDisposed) return;

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SelectContent()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
