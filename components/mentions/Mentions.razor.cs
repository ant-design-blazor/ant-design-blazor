// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    /**
    <summary>
    <para>Mention component.</para>

    <h2>When To Use</h2>

    <para>When need to mention someone or something.</para>
    </summary>
    <seealso cref="MentionsOption" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/jPE-itMFM/Mentions.svg", Title = "Mentions", SubTitle = "提及")]
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

#if NET7_0_OR_GREATER
        [GeneratedRegex("@([^@\\s]+)\\s")]
        private static partial Regex MentionNamesRegex();
#else
        private static readonly Regex _mentionNamesRegex = new("@([^@\\s]+)\\s");
#endif

        public List<string> GetMentionNames()
        {
#if NET7_0_OR_GREATER
            var matches = MentionNamesRegex().Matches(Value);
#else
            var matches = _mentionNamesRegex.Matches(Value);
#endif
            var names = new List<string>(matches.Count);
            foreach (Match m in matches)
            {
                names.Add(m.Groups[1].Value);
            }
            return names;
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
            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, false);
            await _overlayTrigger.Hide();
        }

        private async Task ShowOverlay(bool reCalcPosition)
        {
            await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, true);

            // 总是选择第一个选项
            ActiveOptionValue = ShowOptions.FirstOrDefault()?.Value;

            // 每次显示时都重新计算位置
            var pos = await JS.InvokeAsync<double[]>(JSInteropConstants.GetCursorXY, _overlayTrigger.RefBack.Current);
            var x = (int)Math.Round(pos[0]);
            var y = (int)Math.Round(pos[1]);
            await _overlayTrigger.Show(x, y);

            await InvokeStateHasChangedAsync();
        }

        private async Task OnKeyDown(KeyboardEventArgs args)
        {
            //↑、↓、回车键只能放进js里判断，不然在Sever异步模式下无法拦截原键功能
            if (args.Key == "Escape")
            {
                await HideOverlay();
                return;
            }
        }

        private async Task OnInput(ChangeEventArgs args)
        {
            Value = args.Value.ToString();
            await ValueChanged.InvokeAsync(Value);

            var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
            if (focusPosition == 0)
            {
                await HideOverlay();
                return;
            }

            // 检查光标前的文本中是否存在 @ 符号
            var textBeforeCursor = Value.Substring(0, focusPosition);
            var lastAtIndex = textBeforeCursor.LastIndexOf(Prefix);

            // 如果没有 @ 符号，隐藏选项
            if (lastAtIndex == -1)
            {
                await HideOverlay();
                return;
            }

            // 如果光标正好在 @ 后面，显示所有选项
            if (lastAtIndex == focusPosition - 1)
            {
                await LoadItems(string.Empty);
                if (ShowOptions.Count > 0)
                {
                    await ShowOverlay(true);
                }
                return;
            }

            // 获取 @ 后面的搜索文本
            var searchText = textBeforeCursor.Substring(lastAtIndex + 1);

            // 如果搜索文本中包含空格，说明已经选择完成，隐藏选项
            if (searchText.Contains(" "))
            {
                await HideOverlay();
                return;
            }

            // 加载并显示匹配的选项
            await LoadItems(searchText);
            if (ShowOptions.Count > 0)
            {
                await ShowOverlay(true);
            }
            else
            {
                await HideOverlay();
            }
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
