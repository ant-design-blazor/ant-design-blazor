// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
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
    public partial class Mentions : AntDomComponentBase
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
        /// Dynamically load mention options for display when the user types a value after the prefix symbol
        /// </summary>
        [Parameter]
        public Func<string, CancellationToken, Task<IEnumerable<MentionsDynamicOption>>> LoadOptions { get; set; }

        [Parameter]
        public RenderFragment<MentionsTextareaTemplateOptions> TextareaTemplate { get; set; }

        /// <summary>
        /// Gets or sets the prefix used to identify special commands or keywords, split by ',' (e.g. "@,#").
        /// </summary>
        [Parameter]
        public string Prefix { get; set; } = "@";

        /// <summary>
        /// Triggered when searching with prefix
        /// </summary>
        [PublicApi("1.5.0")]
        [Parameter]
        public EventCallback<(string searchValue, string prefix)> OnSearch { get; set; }

        private CancellationTokenSource _cancellationTokenSource;
        private string[] _prefixes;
        private string _currentPrefix;

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
                .If($"{prefixCls}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
            InitializePrefixes();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            InitializePrefixes();
        }

        private void InitializePrefixes()
        {
            _prefixes = (Prefix ?? "@")
#if NET5_0_OR_GREATER
                .Split([','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
#else
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
#endif
                .ToArray();

            if (_prefixes.Length == 0)
            {
                _prefixes = ["@"];
            }
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

        public List<(string name, string prefix)> GetMentionNames()
        {
            var result = new List<(string name, string prefix)>();
            foreach (var prefix in _prefixes)
            {
                var pattern = $"{Regex.Escape(prefix)}([^{Regex.Escape(string.Join("", _prefixes))}\\s]+)\\s";
                var matches = Regex.Matches(Value, pattern);
                foreach (Match m in matches)
                {
                    result.Add((m.Groups[1].Value, prefix));
                }
            }
            return result;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    await JsInvokeAsync(JSInteropConstants.SetEditorKeyHandler, DotNetObjectReference.Create(this), _overlayTrigger.RefBack.Current);
                }
                catch (JSException)
                {
                    // Ignore JS exceptions during test scenarios
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public void PrevOption()
        {
            var index = Math.Max(0, ActiveOptionIndex - 1);
            if (index < ShowOptions.Count)
            {
                ActiveOptionValue = ShowOptions[index].Value;
                StateHasChanged();
            }
        }

        [JSInvokable]
        public void NextOption()
        {
            var index = Math.Min(ActiveOptionIndex + 1, ShowOptions.Count - 1);
            if (index >= 0 && index < ShowOptions.Count)
            {
                ActiveOptionValue = ShowOptions[index].Value;
                StateHasChanged();
            }
        }

        [JSInvokable]
        public async Task EnterOption()
        {
            if (!string.IsNullOrEmpty(ActiveOptionValue))
            {
                await ItemClick(ActiveOptionValue);
            }
        }

        private async Task HideOverlay()
        {
            try
            {
                await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, false);
                await _overlayTrigger.Hide();
            }
            catch (JSException)
            {
                // Ignore JS exceptions during test scenarios
            }
        }

        private async Task ShowOverlay(bool reCalcPosition)
        {
            try
            {
                await JS.InvokeAsync<double[]>(JSInteropConstants.SetPopShowFlag, true);

                // Always select the first option
                ActiveOptionValue = ShowOptions.FirstOrDefault()?.Value;

                // Recalculate position each time shown
                var pos = await JS.InvokeAsync<double[]>(JSInteropConstants.GetCursorXY, _overlayTrigger.RefBack.Current);
                var x = (int)Math.Round(pos[0]);
                var y = (int)Math.Round(pos[1]);
                await _overlayTrigger.Show(x, y);

                await InvokeStateHasChangedAsync();
            }
            catch (JSException)
            {
                // Ignore JS exceptions during test scenarios
            }
        }

        private async Task OnKeyDown(KeyboardEventArgs args)
        {
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

            try
            {
                var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
                if (focusPosition == 0)
                {
                    await HideOverlay();
                    return;
                }

                // Get text before cursor
                var textBeforeCursor = Value.Substring(0, focusPosition);

                // Find the last prefix that appears in the text
                var lastPrefix = _prefixes
                    .Select(p => new { Prefix = p, Index = textBeforeCursor.LastIndexOf(p) })
                    .Where(x => x.Index != -1)
                    .OrderByDescending(x => x.Index)
                    .FirstOrDefault();

                // If no prefix found, hide options
                if (lastPrefix == null)
                {
                    await HideOverlay();
                    return;
                }

                _currentPrefix = lastPrefix.Prefix;
                var lastPrefixIndex = lastPrefix.Index;

                // If cursor is right after the prefix, show all options
                if (lastPrefixIndex == focusPosition - _currentPrefix.Length)
                {
                    await LoadItems(string.Empty);
                    if (ShowOptions.Count > 0)
                    {
                        await ShowOverlay(true);
                    }
                    return;
                }

                // Get search text after the prefix
                var searchText = textBeforeCursor.Substring(lastPrefixIndex + _currentPrefix.Length);

                // If search text contains space, selection is complete
                if (searchText.Contains(' '))
                {
                    await HideOverlay();
                    return;
                }

                // Load and show matching options
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
            catch (JSException)
            {
                // Ignore JS exceptions during test scenarios
            }
        }

        private async Task LoadItems(string searchValue)
        {
            if (OnSearch.HasDelegate)
            {
                await OnSearch.InvokeAsync((searchValue, _currentPrefix));
            }

            if (LoadOptions is null)
            {
                var optionsToShow = OriginalOptions.Where(x => x.Value.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                SetOptionsToShow(optionsToShow);
            }
            else
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    var options = await LoadOptions.Invoke(searchValue, _cancellationTokenSource.Token).ConfigureAwait(false);
                    SetOptionsToShow(options);
                }
                catch (OperationCanceledException)
                {
                    // Operation was cancelled, ignore
                }
            }
        }

        private void SetOptionsToShow(IEnumerable<MentionsDynamicOption> optionsToShow)
        {
            ShowOptions.Clear();
            ShowOptions.AddRange(optionsToShow);

            // Set active option to first option if available
            if (ShowOptions.Count > 0)
            {
                ActiveOptionValue = ShowOptions[0].Value;
            }
            else
            {
                ActiveOptionValue = null;
            }
        }

        internal async Task ItemClick(string optionValue)
        {
            try
            {
                var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
                var preText = Value.Substring(0, focusPosition);
                preText = preText.LastIndexOf(_currentPrefix) >= 0 ? Value.Substring(0, preText.LastIndexOf(_currentPrefix)) : preText;
                if (preText.EndsWith(' ')) preText = preText.Substring(0, preText.Length - 1);
                var nextText = Value.Substring(focusPosition);
                if (nextText.StartsWith(' ')) nextText = nextText.Substring(1);
                var option = $" {_currentPrefix}{optionValue} ";

                Value = preText + option + nextText;
                await ValueChanged.InvokeAsync(Value);

                var pos = preText.Length + option.Length;
                var js = $"document.querySelector('[_bl_{_overlayTrigger.Ref.Id}]').selectionStart = {pos};";
                js += $"document.querySelector('[_bl_{_overlayTrigger.Ref.Id}]').selectionEnd = {pos}";
                await JS.InvokeVoidAsync("eval", js);

                await HideOverlay();
                await InvokeStateHasChangedAsync();
            }
            catch (JSException)
            {
                // Ignore JS exceptions during test scenarios
            }
        }
    }
}
