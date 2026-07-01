// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string Prefix { get; set; } = DefaultPrefix;

        /// <summary>
        /// Triggered when searching with prefix
        /// </summary>
        [PublicApi("1.5.0")]
        [Parameter]
        public EventCallback<(string searchValue, string prefix)> OnSearch { get; set; }

        private const string DefaultPrefix = "@";
        private static readonly string[] _defaultPrefixes = [DefaultPrefix];
        private static readonly Regex _defaultPrefixesRegex = new($@"({Regex.Escape(DefaultPrefix)})([^{Regex.Escape(DefaultPrefix)}\s]+)\s", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private CancellationTokenSource _cancellationTokenSource;
        private string[] _prefixes;
        private string _currentPrefix;
        private Regex _prefixsRegex;

        internal List<MentionsDynamicOption> OriginalOptions { get; set; } = [];
        internal List<MentionsDynamicOption> ShowOptions { get; } = [];

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
            var p = Prefix;
            if (p == DefaultPrefix || string.IsNullOrWhiteSpace(p))
            {
                _prefixes = _defaultPrefixes;
                _prefixsRegex = _defaultPrefixesRegex;
                return;
            }

#if NET5_0_OR_GREATER
            _prefixes = [.. p.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .OrderByDescending(s => s.Length)
                .ThenBy(s => s, StringComparer.InvariantCulture)];
#else
            _prefixes = [.. p.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .OrderByDescending(s => s.Length)
                .ThenBy(s => s, StringComparer.InvariantCulture)];
#endif
            if (_prefixes.Length == 0 || _prefixes.SequenceEqual(_defaultPrefixes))
            {
                _prefixes = _defaultPrefixes;
                _prefixsRegex = _defaultPrefixesRegex;
                return;
            }

            var alternation = string.Join('|', _prefixes.Select(Regex.Escape));
            var forbiddenChars = Regex.Escape(string.Concat(_prefixes)); // chars to exclude in name char class
            var pattern = $@"({alternation})([^{forbiddenChars}\s]+)\s";
            // Compile once for reuse. If prefixes change frequently, consider removing Compiled.
            _prefixsRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        internal void AddOption(MentionsOption option)
        {
            if (option == null) return;

            var options = OriginalOptions;
            if (!options.Exists(x => x.Value == option.Value))
            {
                options.Add(new()
                {
                    Value = option.Value,
                    Display = option.ChildContent
                });
            }
        }

        public List<(string name, string prefix)> GetMentionNames()
        {
            var result = new List<(string name, string prefix)>();
            foreach (Match m in _prefixsRegex.Matches(Value))
            {
                // group1 = prefix, group2 = name
                result.Add((m.Groups[2].Value, m.Groups[1].Value));
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

                var lastPrefix = _prefixes
                    .Select(p => (Prefix: p, Index: Value.AsSpan(0, focusPosition).LastIndexOf(p)))
                    .Where(x => x.Index != -1)
                    .OrderByDescending(x => x.Index)
                    .FirstOrDefault();

                // If no prefix found, hide options
                if (lastPrefix is not { Prefix.Length: > 0 })
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
                var searchTextSpan = Value.AsSpan(0, focusPosition)[(lastPrefixIndex + _currentPrefix.Length)..];
                // If search text contains space, selection is complete
                if (searchTextSpan.Contains([' '], StringComparison.Ordinal))
                {
                    await HideOverlay();
                    return;
                }
                // Load and show matching options
                await LoadItems(new(searchTextSpan));

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

#if NET10_0_OR_GREATER
            // Refresh the overlay component to detect changes in ShowOptions list (.NET 10 compatibility)
            _overlayTrigger?.GetOverlayComponent()?.RefreshComponentState();
#endif
        }

        internal async Task ItemClick(string optionValue)
        {
            try
            {
                var focusPosition = await JS.InvokeAsync<int>(JSInteropConstants.GetProp, _overlayTrigger.Ref, "selectionStart");
                var lastPrefixIndex = Value.AsSpan(0, focusPosition).LastIndexOf(_currentPrefix);
                var preText = lastPrefixIndex >= 0
                    ? Value.AsSpan(0, lastPrefixIndex)
                    : Value.AsSpan(0, focusPosition);
                if (preText.EndsWith([' '])) preText = preText[..^1];
                var nextText = Value.AsSpan(focusPosition);
                if (nextText.StartsWith([' '])) nextText = nextText[1..];
                var tailLength = nextText.Length;

                Value = new StringBuilder()
                    .Append(preText)
                    .Append(' ').Append(_currentPrefix).Append(optionValue).Append(' ')
                    .Append(nextText)
                    .ToString();
                await ValueChanged.InvokeAsync(Value);

                var pos = Value.Length - tailLength;
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
