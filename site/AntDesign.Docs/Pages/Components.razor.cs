// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Pages
{
    public partial class Components : ComponentBase, IDisposable
    {
        [Parameter]
        public string Locale { get; set; }
        [Parameter]
        public string Name { get; set; }

        [Inject]
        private DemoService DemoService { get; set; }

        [Inject]
        private ILocalizationService LocalizationService { get; set; }

        [Inject]
        private IStringLocalizer Localizer { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        private DemoComponent _demoComponent;

        private bool _expanded;

        private bool _expandAllCode;

        private string CurrentLanguage => LocalizationService.CurrentCulture.Name;

        private string _filePath;

        private List<string> _filePaths;

        private bool _rendered;

        private bool _changed = true;

        private string EditUrl => $"https://github.com/ant-design-blazor/ant-design-blazor/edit/master/{_filePath}";

        private string ComponentSourceUrl => $"https://github.com/ant-design/ant-design/tree/master/components/{ToKebabCase(_demoComponent?.Title)}";

        private string SemanticComponentKey => ToKebabCase(_demoComponent?.Title);

        private Type? SemanticPreviewType => _demos.Count > 0
            ? DemoService.GetShowCase(_demos[0].Type)
            : null;

        private List<DemoItem> _demos = [];

        private (string Name, string Title)[] _anchors = [];

        private string _currentPageName;
        private string _currentLocale;

        private static string ToKebabCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            var result = new StringBuilder(value.Length + 8);
            for (var index = 0; index < value.Length; index++)
            {
                var character = value[index];
                if (char.IsUpper(character) && index > 0 &&
                    (char.IsLower(value[index - 1]) || char.IsDigit(value[index - 1]) ||
                     (char.IsUpper(value[index - 1]) && index + 1 < value.Length && char.IsLower(value[index + 1]))))
                {
                    result.Append('-');
                }

                result.Append(char.ToLowerInvariant(character));
            }

            return result.ToString();
        }

        protected override async Task OnInitializedAsync()
        {
            LocalizationService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;
            await HandleNavigate(onInitialize: true);
        }

        private void OnLanguageChanged(object sender, CultureInfo args)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                _changed = true;
                InvokeAsync(StateHasChanged);
            }
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _changed = true;
            InvokeAsync(StateHasChanged);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _rendered = true;
                StateHasChanged();
                return;
            }

            if (_rendered && _changed)
            {
                _changed = false;
                _ = HandleNavigate();
            }
        }

        private async Task HandleNavigate(bool onInitialize = false)
        {
            var fullPageName = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            fullPageName = fullPageName.IndexOf('/') > 0 ? fullPageName.Substring(fullPageName.IndexOf('/') + 1) : fullPageName;
            // Query strings are useful for cache busting and browser verification,
            // but they are not part of the metadata key (components/colorpicker).
            var suffixIndex = fullPageName.IndexOfAny(['?', '#']);
            fullPageName = suffixIndex >= 0 ? fullPageName.Substring(0, suffixIndex) : fullPageName;
            if (fullPageName.StartsWith("docs"))
            {
                return;
            }

            if (_currentPageName == fullPageName && _currentLocale == Locale)
            {
                return;
            }

            if (fullPageName.Split("/").Length != 2)
            {
                if (!onInitialize)
                {
                    var menus = await DemoService.GetMenuAsync();
                    var current = menus.FirstOrDefault(x => x.Url == fullPageName.ToLowerInvariant());
                    if (current?.Children is { Length: > 0 })
                    {
                        var subPath = current.Children[0].Type == "menuItem" ? current.Children[0].Url : current.Children[0].Children[0].Url;
                        NavigationManager.NavigateTo($"{CurrentLanguage}/{subPath}");
                    }
                }
            }
            else
            {
                _currentPageName = fullPageName;
                _currentLocale = Locale;
                _demoComponent = await DemoService.GetComponentAsync($"{fullPageName}");
                StateHasChanged();

                _filePath = $"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/doc/index.{CurrentLanguage}.md";
                await LoadDemos(_currentPageName);
                await MainLayout.ChangePrevNextNav(Name);

                if (NavigationManager.Uri.Contains("#"))
                {
                    NavigationManager.NavigateTo(NavigationManager.Uri);
                }
            }
        }

        private async Task LoadDemos(string pageName)
        {
            var showDemos = _demoComponent?.DemoList?.Where(x => !x.Debug && !x.Docs.HasValue).OrderBy(x => x.Order) ?? Enumerable.Empty<DemoItem>();
            _anchors = showDemos.Select(x => (x.Name, x.Title)).ToArray();
            _demos = [];
            _filePaths = new() { _filePath };
            foreach (var item in showDemos)
            {
                // compoennt is changed
                if (pageName != _currentPageName)
                {
                    return;
                }
                _filePaths.Add($"site/AntDesign.Docs/Demos/Components/{_demoComponent?.Title}/demo/{item.Name}.md");
                _filePaths.Add($"site/AntDesign.Docs/{item.Type.Replace(".", "/")}.razor");
                _demos.Add(item);
                StateHasChanged();
                await Task.Delay(100); //Make outgoing threads avoid lagging operations (e.g. switching menus)
            }
        }

        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
