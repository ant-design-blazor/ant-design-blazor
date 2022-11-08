using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    public partial class Mentions
    {

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool Disable { get; set; }
        [Parameter] public bool Focused { get; set; }
        [Parameter] public string DefaultValue { get; set; }
        [Parameter] public Dictionary<string, object> Attributes { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string Value { get; set; }
        internal List<MentionsOption> OriginalOptions { get; set; } = new List<MentionsOption>();
        internal List<MentionsOption> ShowOptions { get; set; } = new List<MentionsOption>();
        private OverlayTrigger _overlayTrigger;
        private int ActiveOptionIndex { get; set; }
        internal MentionsOption ActiveOption
        {
            get
            {
                return ShowOptions[ActiveOptionIndex];
            }
            set
            {
                var index = ShowOptions.FindIndex(0, x => x == value);
                if (index >= 0)
                {
                    ActiveOptionIndex = index;
                    InvokeStateHasChanged();
                }
            }
        }
        private void SetClassMap()
        {
            var prefixCls = "ant-mentions";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .Add("ant-mentions-status-success")
                .If($"{prefixCls}-disable", () => this.Disable)
                .If($"{prefixCls}-focused", () => this.Focused)
                .If($"{prefixCls}-rtl", () => RTL)
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
        }

        internal void RemoveOption(MentionsOption option)
        {
            OriginalOptions.Remove(option);
        }

        internal void AddOption(MentionsOption option)
        {
            if (option == null) return;
            var opt = OriginalOptions.Find(x => x.Value == option.Value);
            if (opt == null)
            {
                OriginalOptions.Add(option);
            }
        }
        public List<string> GetMentionNames()
        {
            var r = new List<string>();
            var regex = new System.Text.RegularExpressions.Regex("@([^@\\s]+)\\s");
            regex.Matches(Value).ToList().ForEach(m =>
            {
                var name = m.Groups[1].Value;
                r.Add(name);
            });
            return r;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ShowOptions.Clear();
            ShowOptions.AddRange(OriginalOptions);
            if (firstRender)
            {
                await JsInvokeAsync(JSInteropConstants.SetEditorKeyHandler, DotNetObjectReference.Create(this), Ref);
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        [JSInvokable]
        public void UpOption()
        {
            ActiveOptionIndex = Math.Max(0, ActiveOptionIndex - 1);
            StateHasChanged();
        }
        [JSInvokable]
        public void NextOption()
        {
            ActiveOptionIndex = Math.Min(ActiveOptionIndex + 1, ShowOptions.Count - 1);
            StateHasChanged();
        }
        [JSInvokable]
        public async Task EnterOption()
        {
            await ItemClick(ShowOptions[ActiveOptionIndex]);
            StateHasChanged();
        }
        async Task OnkeyDown(KeyboardEventArgs args)
        {   //↑、↓、回车键只能放进js里判断，不然在Sever异步模式下无法拦截原键功能
            //开启浮窗的判断放在oninput里，不然会有问题
            if (args.Key == "Escape") await _overlayTrigger.Hide(true);
        }
        async Task ShowOverLay(bool reset)
        {
            if (reset)
            {
                ShowOptions.Clear();
                ShowOptions.AddRange(OriginalOptions);
            }
            var pos = await JS.InvokeAsync<double[]>(JSInteropConstants.GetCursorXY, _overlayTrigger.Ref);
            await _overlayTrigger.Show((int)Math.Round(pos[0]), (int)Math.Round(pos[1]));
            await InvokeStateHasChangedAsync();
        }
        async Task OnInput(ChangeEventArgs args)
        {
            if (args.Value!.ToString()!.EndsWith("@"))
            {
                await ShowOverLay(true);
                return;
            }

            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            if (focusPosition == 0) return;
            var showoverlay = false;
            var v = args.Value!.ToString()!.Substring(0, focusPosition);  //从光标处切断,向前找匹配项
            var lastIndex = v.LastIndexOf("@");
            if (lastIndex >= 0)
            {
                var lastOption = v.Substring(lastIndex + 1);
                ShowOptions = OriginalOptions.Where(x => x.Value.Contains(lastOption, StringComparison.OrdinalIgnoreCase)).ToList();
                if (ShowOptions.Count > 0)
                {
                    showoverlay = true;
                }
            }
            if (showoverlay)
            {
                ActiveOptionIndex = 0;
                await ShowOverLay(false);
            }
            await InvokeStateHasChangedAsync();
        }
        internal async Task ItemClick(MentionsOption item)
        {
            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            var preText = Value.Substring(0, focusPosition);
            preText = preText.LastIndexOf("@") >= 0 ? Value.Substring(0, preText.LastIndexOf("@")) : preText;
            preText = preText.Trim();
            var nextText = Value.Substring(focusPosition);
            nextText = nextText.Trim();
            var option = " @" + item + " ";
            Value = preText + option + nextText;
            ActiveOptionIndex = ShowOptions.FindIndex(0, y => y == item);
            await _overlayTrigger.Hide(true);
            await InvokeStateHasChangedAsync();
        }
    }
}
