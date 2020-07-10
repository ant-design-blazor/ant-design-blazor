using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    public partial class Mentions
    {
        [Parameter] public bool AutoFocus { get; set; }
        [Parameter] public bool Disable { get; set; }
        [Parameter] public bool Readonly { get; set; }
        [Parameter] public string Prefix { get; set; } = "@,#";   //“@，#”
        [Parameter] public string Split { get; set; } = " ";
        [Parameter] public string DefaultValue { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public string Placement { get; set; } = "bottom";
        [Parameter] public string Direction { get; set; } = "ltr";
        [Parameter] public int Rows { get; set; } = 1;
        [Parameter] public bool Loading { get; set; } = false;
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<string> ValueChange { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<EventArgs> OnSearch { get; set; }

        [Parameter] public RenderFragment NoFoundContent { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        [Inject] private DomEventService DomEventService { get; set; }
        internal List<MentionsOption> LstOriginalOptions { get; set; } = new List<MentionsOption>();

        private string DropdownStyle { get; set; }
        private bool Focused { get; set; }
        private bool ShowSuggestions { get; set; }

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => this.Disable)
                .If($"{prefixCls}-focused", () => this.Focused)
                .If($"{prefixCls}-rtl", () => this.Direction == "rtl")
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
            if (string.IsNullOrEmpty(Value))
            {
                Value = DefaultValue;
            }

            //DomEventService.AddEventListener(Ref, "keyup", OnTextAreaKeyup);
            // DomEventService.AddEventListener(Ref, "onmouseup", OnTextAreaMouseUp);
        }

        internal bool FirstTime { get; set; } = true;

        protected override Task OnFirstAfterRenderAsync()
        {
            base.OnFirstAfterRenderAsync();
            FirstTime = false;
            return Task.CompletedTask;
        }

        /// <summary>
        ///  After Press keyUP, if a key  is prefix, to popup the suggestion
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (args == null) return;
            if (Prefix.Contains(args.Key, StringComparison.Ordinal))
            {
                await SetDropdownStyle();
                ShowSuggestions = Prefix.Contains(args.Key, StringComparison.Ordinal);
                await InvokeStateHasChangedAsync();
            }

            if (!string.IsNullOrEmpty(Value))
            {
                var suggestions = this.Value.Split(Split);
            }

            await ValueChange.InvokeAsync(this.Value);
        }

        private async Task OnKeyDown(KeyboardEventArgs args)
        {
            await InvokeStateHasChangedAsync();
        }

        DotNetObjectReference<Mentions> _reference;
        const string ReferenceName = "mentions";

        protected async Task SetDropdownStyle()
        {
            if (_reference == null)
            {
                _reference = DotNetObjectReference.Create<Mentions>(this);
            }
            //get current dom element infomation
            var domRect = await JsInvokeAsync<double[]>(JSInteropConstants.getCursorXY, Ref, _reference);

            var left = Math.Round(domRect[0]);
            var top = Math.Round(domRect[1]);
            //DropdownStyle = $"display:inline-flex;position:fixed;top:{top}px;left:{left}px;transform:translate({cursorPoint.x}px, {cursorPoint.y}0px);";
            DropdownStyle = $"display:inline-flex;position:fixed;top:{top}px;left:{left}px;transform:translate(0px, 0px);";
        }

        /// <summary>
        ///  open or close the list of Suggestion
        /// </summary>
        /// <param name="show"></param>
        /// <returns></returns>
        internal async Task ShowSuggestion(bool show)
        {
            ShowSuggestions = show;
            await InvokeStateHasChangedAsync();
        }

        internal async Task GetSuggestion()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        internal async Task OnOptionClick(MentionsOption opt)
        {
            // update to status of  all of suggestion to un-selected state
            foreach (var item in LstOriginalOptions)
            {
                item.Selected = false;
            }

            // when the suggestion clicked is not null and enable, then change the value of textarea.
            if (opt != null && !opt.Disable)
            {
                if (string.IsNullOrEmpty(this.Value))
                {
                    this.Value = opt.Value + this.Split;
                }
                else
                {
                    this.Value = this.Value + opt.Value + this.Split;
                }
            }

            ShowSuggestions = false;

            await InvokeStateHasChangedAsync();
        }

        private void OnKeyPress(KeyboardEventArgs args)
        {
            Console.WriteLine(args.Key);

            if (args == null) return;
            if (ValueChange.HasDelegate)
                ValueChange.InvokeAsync(args.Code);

            return;
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        /// <summary>
        /// return a  new list of mentionsOption, when all item included the specificed text
        /// </summary>
        /// <param name="textValue"></param>
        /// <returns></returns>
        private List<MentionsOption> IsIncluded(string textValue)
        {
            if (string.IsNullOrEmpty(textValue))
            {
                return LstOriginalOptions;
            }

            List<MentionsOption> lst = new List<MentionsOption>();
            foreach (var item in LstOriginalOptions)
            {
                if (item.Value.Contains(textValue, StringComparison.Ordinal))
                {
                    lst.Add(item);
                }
            }
            return lst;
        }

        public void AddOption(MentionsOption option)
        {
            if (option == null) return;
            var opt = LstOriginalOptions.Find(x => x.Value == option.Value);
            if (opt == null)
            {
                LstOriginalOptions.Add(option);
            }
        }

        internal async void Onfocus(FocusEventArgs args)
        {
            if (args == null) return;

            Focused = true;
            if (OnFocus.HasDelegate)
                await OnFocus.InvokeAsync(args);

            await JsInvokeAsync(JSInteropConstants.focus, Ref);
            StateHasChanged();
        }

        internal async void Onblur(FocusEventArgs args)
        {
            if (args == null) return;
            Focused = false;
            if (OnBlur.HasDelegate)
                await OnBlur.InvokeAsync(args);
            await JsInvokeAsync(JSInteropConstants.blur, Ref);
            StateHasChanged();
        }

        [JSInvokable]
        public void CloseMentionsDropDown()
        {
            ShowSuggestions = false;
            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            _reference = null;
            _ = JsInvokeAsync(JSInteropConstants.disposeObj, ReferenceName);
            base.Dispose(disposing);
        }
    }
}
