using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Mentions : AntInputComponentBase<string>
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public int Rows { get; set; } = 1;
        [Parameter] public Dictionary<string, object> Attributes { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Parameter] public string Placeholder { get; set; }

        internal List<MentionsOption> OriginalOptions { get; set; } = new List<MentionsOption>();
        internal List<MentionsOption> ShowOptions { get; } = new List<MentionsOption>();

        internal string ActiveOptionValue { get; set; }
        internal int ActiveOptionIndex => ShowOptions.FindIndex(x => x.Value == ActiveOptionValue);

        private OverlayTrigger _overlayTrigger;
        public bool _focused;

        public Mentions()
        {
            BindOnInput = true;
        }

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions";
            this.ClassMapper
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => this.Disabled)
                .If($"{prefixCls}-focused", () => this._focused)
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
        public void PrevOption()
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

        private async Task HideOverlay()
        {
            ActiveOptionValue = null;
            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, false);
            await _overlayTrigger.Hide();
        }

        private async Task ShowOverlay(bool resetOptions, bool reCalcPosition)
        {
            if (resetOptions)
            {
                ShowOptions.Clear();
                ShowOptions.AddRange(OriginalOptions);
            }

            if (ActiveOptionIndex > 0)
            {
                await InvokeStateHasChangedAsync();
                return;
            }

            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, true);
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

        private async void OnKeyDown(KeyboardEventArgs args)
        {   //↑、↓、回车键只能放进js里判断，不然在Sever异步模式下无法拦截原键功能
            //开启浮窗的判断放在oninput里，不然会有问题
            if (args.Key == "Escape") await HideOverlay();
        }

        protected override async Task OnBindAsync()
        {
            if (_inputString is not { Length: > 0 })
            {
                await HideOverlay();
                return;
            }
            if (_inputString?.EndsWith("@") == true)
            {
                await ShowOverlay(true, true);
                return;
            }
            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            if (focusPosition == 0)
            {
                await HideOverlay();
                return;
            };
            var showPop = false;
            var v = _inputString.Substring(0, focusPosition);  //从光标处切断,向前找匹配项
            var lastIndex = v.LastIndexOf("@");
            if (lastIndex >= 0)
            {
                var lastOption = v.Substring(lastIndex + 1);
                ShowOptions.Clear();
                ShowOptions.AddRange(OriginalOptions.Where(x => x.Value.Contains(lastOption, StringComparison.OrdinalIgnoreCase)).ToList());
                if (ShowOptions.Count > 0) showPop = true;
            }

            if (showPop)
            {
                await ShowOverlay(false, true);
            }
            else
            {
                await HideOverlay();
            }
        }

        internal async Task ItemClick(string optionValue)
        {
            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            var preText = _inputString.Substring(0, focusPosition);
            preText = preText.LastIndexOf("@") >= 0 ? _inputString.Substring(0, preText.LastIndexOf("@")) : preText;
            if (preText.EndsWith(' ')) preText = preText.Substring(0, preText.Length - 1);
            var nextText = _inputString.Substring(focusPosition);
            if (nextText.StartsWith(' ')) nextText = nextText.Substring(1);
            var option = " @" + optionValue + " ";

            CurrentValueAsString = preText + option + nextText;

            var pos = preText.Length + option.Length;
            var js = $"document.querySelector('[_bl_{_overlayTrigger.Ref.Id}]').selectionStart = {pos};";
            js += $"document.querySelector('[_bl_{_overlayTrigger.Ref.Id}]').selectionEnd = {pos}";
            await JS.InvokeVoidAsync("eval", js);

            await HideOverlay();
            await InvokeStateHasChangedAsync();
        }
    }
}
