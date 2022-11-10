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
        [Parameter] public int Rows { get; set; } = 3;
        [Parameter] public bool Focused { get; set; }
        [Parameter] public bool Readonly { get; set; }
        [Parameter] public bool Loading { get; set; }


        [Parameter] public Dictionary<string, object> Attributes { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string Value { get; set; } = String.Empty;
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        internal List<MentionsOption> OriginalOptions { get; set; } = new List<MentionsOption>();
        internal List<MentionsOption> ShowOptions { get; } = new List<MentionsOption>();
        private OverlayTrigger _overlayTrigger;
        internal string ActiveOptionValue { get; set; }
        internal int ActiveOptionIndex => ShowOptions.FindIndex(x => x.Value == ActiveOptionValue);

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => this.Disable)
                .If($"{prefixCls}-focused", () => this.Focused)
                .If($"{prefixCls}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
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
            if (firstRender)
            {
                ShowOptions.Clear();
                ShowOptions.AddRange(OriginalOptions);
                await JsInvokeAsync(JSInteropConstants.SetEditorKeyHandler, DotNetObjectReference.Create(this), _overlayTrigger.RefBack.Current);
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        [JSInvokable]
        public void UpOption()
        {
            var index = Math.Max(0, ActiveOptionIndex - 1);
            ActiveOptionValue = ShowOptions[index].Value;
            StateHasChanged();
        }
        [JSInvokable]
        public void NextOption()
        {
            var index = Math.Min(ActiveOptionIndex + 1, ShowOptions.Count - 1);
            ActiveOptionValue = ShowOptions[index].Value;
            StateHasChanged();
        }
        [JSInvokable]
        public async Task EnterOption()
        {
            await ItemClick(ActiveOptionValue);
        }

        async Task HideOverlay()
        {
            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, false);
            await _overlayTrigger.Hide();
        }

        async Task ShowOverlay(bool resetOptions, bool reCalcPosition)
        {
            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, true);
            if (resetOptions)
            {
                ShowOptions.Clear();
                ShowOptions.AddRange(OriginalOptions);
            }
            ActiveOptionValue = ShowOptions.First().Value;
            if (reCalcPosition)
            {
                var pos = await JS.InvokeAsync<double[]>(JSInteropConstants.GetCursorXY, _overlayTrigger.RefBack.Current);
                var x = (int)Math.Round(pos[0]);
                var y = (int)Math.Round(pos[1]);
                await _overlayTrigger.Show(x, y);
            }
            else
            {
                await _overlayTrigger.Show();
            }
            await InvokeStateHasChangedAsync();
        }
        async void OnKeyDown(KeyboardEventArgs args)
        {   //↑、↓、回车键只能放进js里判断，不然在Sever异步模式下无法拦截原键功能
            //开启浮窗的判断放在oninput里，不然会有问题
            if (args.Key == "Escape") await HideOverlay();
        }

        async Task OnInput(ChangeEventArgs args)
        {
            Value = args.Value.ToString();
            await ValueChanged.InvokeAsync(Value);
            if (Value.EndsWith("@"))
            {
                await ShowOverlay(true, true);
                return;
            }
            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            if (focusPosition == 0) return;
            var v = Value.Substring(0, focusPosition);  //从光标处切断,向前找匹配项
            var lastIndex = v.LastIndexOf("@");
            if (lastIndex >= 0)
            {
                var lastOption = v.Substring(lastIndex + 1);
                ShowOptions.Clear();
                ShowOptions.AddRange(OriginalOptions.Where(x => x.Value.Contains(lastOption, StringComparison.OrdinalIgnoreCase)).ToList());
                if (ShowOptions.Count > 0)
                {
                    await ShowOverlay(false, true);
                }
            }
            StateHasChanged();
        }
        internal async Task ItemClick(string optionValue)
        {
            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            var preText = Value.Substring(0, focusPosition);
            preText = preText.LastIndexOf("@") >= 0 ? Value.Substring(0, preText.LastIndexOf("@")) : preText;
            preText = preText.Trim(' ');
            var nextText = Value.Substring(focusPosition);
            nextText = nextText.Trim(' ');
            var option = " @" + optionValue + " ";
            Value = preText + option + nextText;
            ActiveOptionValue = optionValue;
            await HideOverlay();
            await InvokeStateHasChangedAsync();
        }
    }
}
