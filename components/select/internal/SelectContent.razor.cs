using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
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

        private string _inputStyle = string.Empty;
        private string _inputWidth;
        private bool _suppressInput;
        private ElementReference _ref;
        private bool _isInitialized;
        private string _prefix;

        protected override void OnInitialized()
        {
            if (!_isInitialized)
                SetInputWidth();
            _isInitialized = true;
            SetSuppressInput();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            SetSuppressInput();
            if (firstRender && ParentSelect.EnableSearch)
            {
                DomEventService.AddEventListener("window", "beforeunload", Reloading, false);
                await Js.InvokeVoidAsync(JSInteropConstants.AddPreventKeys, ParentSelect._inputRef, new[] { "ArrowUp", "ArrowDown" });
                await Js.InvokeVoidAsync(JSInteropConstants.AddPreventEnterOnOverlayVisible, ParentSelect._inputRef, ParentSelect.DropDownRef);
            }
            await base.OnAfterRenderAsync(firstRender);
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

        protected void OnKeyPressEventHandler(KeyboardEventArgs _)
        {
            if (!ParentSelect.IsSearchEnabled)
                SearchValue = string.Empty;
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

        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isReloading)
            {
                _ = InvokeAsync(async () =>
                {
                    await Task.Delay(100);
                    await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventKeys, ParentSelect._inputRef);
                    await Js.InvokeVoidAsync(JSInteropConstants.RemovePreventEnterOnOverlayVisible, ParentSelect._inputRef);                    
                });
            }
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
