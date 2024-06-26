using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Mentions
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disable { get; set; }

        [Parameter]
        public uint Rows { get; set; } = 3;

        [Parameter]
        public bool Focused { get; set; }

        [Parameter]
        public bool Readonly { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public Dictionary<string, object> Attributes { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public string Value { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Dynamically load mention options for display when the user types a value after the @ symbol
        /// </summary>
        [Parameter]
        public Func<string, CancellationToken, Task<IEnumerable<MentionsDynamicOption>>> LoadOptions { get; set; }

        [Parameter]
        public RenderFragment<MentionsTextareaTemplateOptions> TextareaTemplate { get; set; }

        [Parameter]
        public string Prefix { get; set; } = "@";

        private CancellationTokenSource _cancellationTokenSource;

        internal List<MentionsDynamicOption> OriginalOptions { get; set; } = new List<MentionsDynamicOption>();

        internal List<MentionsDynamicOption> ShowOptions { get; } = new List<MentionsDynamicOption>();

        private OverlayTrigger _overlayTrigger;

        internal string ActiveOptionValue { get; set; }

        internal int ActiveOptionIndex => ShowOptions.FindIndex(x => x.Value == ActiveOptionValue);

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions";
            ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => Disable)
                .If($"{prefixCls}-focused", () => Focused)
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
                OriginalOptions.Add(new MentionsDynamicOption
                {
                    Value = option.Value,
                    Display = option.ChildContent
                });
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

        async Task HideOverlay()
        {
            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, false);
            await _overlayTrigger.Hide();
        }

        async Task ShowOverlay(bool reCalcPosition)
        {
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

        async void OnKeyDown(KeyboardEventArgs args)
        {   //↑、↓、回车键只能放进js里判断，不然在Sever异步模式下无法拦截原键功能
            //开启浮窗的判断放在oninput里，不然会有问题
            if (args.Key == "Escape") await HideOverlay();
        }

        async Task OnInput(ChangeEventArgs args)
        {
            Value = args.Value.ToString();
            await ValueChanged.InvokeAsync(Value);

            if (Value.EndsWith(Prefix))
            {
                await LoadItems(string.Empty);
                await ShowOrHideBasedOnAvailableShowOptions();
                return;
            }

            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            if (focusPosition == 0)
            {
                await HideOverlay();
                return;
            };

            var v = Value.Substring(0, focusPosition);  //从光标处切断,向前找匹配项
            var lastIndex = v.LastIndexOf(Prefix);
            if (lastIndex >= 0)
            {
                var lastOption = v.Substring(lastIndex + 1);
                await LoadItems(lastOption);
            }

            await ShowOrHideBasedOnAvailableShowOptions();
        }

        private async Task LoadItems(string searchValue)
        {
            if (LoadOptions is null)
            {
                var optionsToShow = OriginalOptions.Where(x => x.Value.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();

                SetOptionsToShow(optionsToShow);
            }
            else
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();

                var options = await LoadOptions.Invoke(searchValue, _cancellationTokenSource.Token).ConfigureAwait(false);

                SetOptionsToShow(options);
            }
        }

        private void SetOptionsToShow(IEnumerable<MentionsDynamicOption> optionsToShow)
        {
            ShowOptions.Clear();
            ShowOptions.AddRange(optionsToShow);
        }

        private Task ShowOrHideBasedOnAvailableShowOptions()
        {
            return ShowOptions.Count > 0
                ? ShowOverlay(true)
                : HideOverlay();
        }

        internal async Task ItemClick(string optionValue)
        {
            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            var preText = Value.Substring(0, focusPosition);
            preText = preText.LastIndexOf(Prefix) >= 0 ? Value.Substring(0, preText.LastIndexOf(Prefix)) : preText;
            if (preText.EndsWith(' ')) preText = preText.Substring(0, preText.Length - 1);
            var nextText = Value.Substring(focusPosition);
            if (nextText.StartsWith(' ')) nextText = nextText.Substring(1);
            var option = " @" + optionValue + " ";

            Value = preText + option + nextText;
            await ValueChanged.InvokeAsync(Value);

            var pos = preText.Length + option.Length;
            var js = $"document.querySelector('[_bl_{_overlayTrigger.Ref.Id}]').selectionStart = {pos};";
            js += $"document.querySelector('[_bl_{_overlayTrigger.Ref.Id}]').selectionEnd = {pos}";
            await JS.InvokeVoidAsync("eval", js);

            await HideOverlay();
            await InvokeStateHasChangedAsync();
        }
    }
}
