using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Docs.Shared;

public partial class DocsSearch
{
    [Inject] private ILocalizationService LocalizationService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string _query = string.Empty;
    private bool _focused;
    private bool _popoverVisible;
    private int _activeIndex = -1;
    private IReadOnlyList<SearchResult> _results = Array.Empty<SearchResult>();

    private string Language => LocalizationService.CurrentCulture.Name;
    private string Placeholder => Language == "zh-CN" ? "输入关键词搜索..." : "Type keywords...";
    private string EmptyText => Language == "zh-CN" ? "没有找到结果" : "No results found";

    private sealed record SearchResult(string Type, string Title, string Description, string PageTitle, string Url, int Score);

    private void OnSearchFocus(FocusEventArgs _) { _focused = true; UpdatePopover(); }

    private async Task OnSearchBlur(FocusEventArgs _)
    {
        await Task.Delay(120);
        _focused = false;
        UpdatePopover();
    }

    private void OnSearchInput(ChangeEventArgs args)
    {
        _query = args.Value?.ToString() ?? string.Empty;
        _focused = true;
        _activeIndex = -1;
        var terms = _query.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (terms.Length == 0) _results = Array.Empty<SearchResult>();
        else
        {
            _results = GeneratedSearchIndex.All
                .Where(item => item.Language == Language)
                .Select(item =>
                {
                    var title = item.Title ?? string.Empty;
                    var description = item.Description ?? string.Empty;
                    var pageTitle = item.PageTitle ?? string.Empty;
                    var searchable = $"{title} {description} {pageTitle}";
                    var score = terms.Sum(term => title.Contains(term, StringComparison.OrdinalIgnoreCase) ? 12
                        : pageTitle.Contains(term, StringComparison.OrdinalIgnoreCase) ? 8
                        : searchable.Contains(term, StringComparison.OrdinalIgnoreCase) ? 2 : 0);
                    return new SearchResult(item.Type, title, CreateSnippet(description, terms), pageTitle, item.Url, score);
                })
                .Where(item => item.Score > 0 && terms.All(term => $"{item.Title} {item.Description} {item.PageTitle}".Contains(term, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(item => item.Score)
                .ThenBy(item => item.Title, StringComparer.OrdinalIgnoreCase)
                .Take(20)
                .ToArray();
        }
        UpdatePopover();
    }

    private void OnSearchKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Escape")
        {
            _query = string.Empty;
            _results = Array.Empty<SearchResult>();
            _focused = false;
            _activeIndex = -1;
            UpdatePopover();
        }
        else if (args.Key == "ArrowDown" && _results.Count > 0) _activeIndex = (_activeIndex + 1) % _results.Count;
        else if (args.Key == "ArrowUp" && _results.Count > 0) _activeIndex = (_activeIndex + _results.Count - 1) % _results.Count;
        else if (args.Key == "Enter" && _activeIndex >= 0 && _activeIndex < _results.Count) NavigateToResult(_results[_activeIndex].Url);
    }

    private static string CreateSnippet(string description, string[] terms)
    {
        const int maxLength = 110;
        if (description.Length <= maxLength) return description;

        var matchIndex = terms
            .Select(term => description.IndexOf(term, StringComparison.OrdinalIgnoreCase))
            .Where(index => index >= 0)
            .DefaultIfEmpty(0)
            .Min();
        var start = Math.Max(0, matchIndex - 28);
        if (start + maxLength > description.Length) start = Math.Max(0, description.Length - maxLength);
        var length = Math.Min(maxLength, description.Length - start);
        return (start > 0 ? "..." : string.Empty)
            + description.Substring(start, length)
            + (start + length < description.Length ? "..." : string.Empty);
    }

    private RenderFragment Highlight(string value) => builder =>
    {
        var terms = _query.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (terms.Length == 0 || string.IsNullOrEmpty(value))
        {
            builder.AddContent(0, value);
            return;
        }

        var expression = new Regex(string.Join("|", terms.Select(Regex.Escape)), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        var cursor = 0;
        var sequence = 0;
        foreach (Match match in expression.Matches(value))
        {
            if (match.Index > cursor) builder.AddContent(sequence++, value[cursor..match.Index]);
            builder.OpenElement(sequence++, "mark");
            builder.AddContent(sequence++, match.Value);
            builder.CloseElement();
            cursor = match.Index + match.Length;
        }

        if (cursor < value.Length) builder.AddContent(sequence, value[cursor..]);
    };

    private static RenderFragment SearchIcon(string type) => builder =>
    {
        var path = type switch
        {
            "page" => "M9.402 0h14.78L30 6.16V24.5c0 1.933-1.71 3.5-3.589 3.5H9.401C7.524 28 6 26.433 6 24.5v-21C6 1.567 7.523 0 9.402 0ZM23 2v4.183c0 .451.366.817.817.817H28l-5-5Zm3.333 24c.92 0 1.667-.768 1.667-1.714V8.857h-5c-.92 0-1.667-.767-1.667-1.714V2H9.667C8.747 2 8 2.768 8 3.714v20.572C8 25.232 8.746 26 9.667 26h16.666Z",
            "section" => "M6.12 14.589h6.628l1.52 4.004h2.485l-5.938-15.19H8.053L2.115 18.732H4.6l1.52-4.143ZM8.88 6.855c.139-.414.277-.828.415-1.38h.138c0 .138.138.414.414 1.104 0 .138.138.276.138.276 0 .138.829 2.072 2.21 5.938H6.672c1.519-3.866 2.21-5.8 2.21-5.938Zm8.148 2.348h12.705v1.933H17.029V9.203ZM2.115 20.665h27.619v1.933H2.114v-1.933Zm14.914-5.662h12.705v1.933H17.029v-1.933ZM2.115 26.327h27.619v1.933H2.114v-1.933ZM17.029 3.54h12.705v1.934H17.029V3.54Z",
            "demo" => "M28 6h-5a5 5 0 0 0-10 0H8a2 2 0 0 0-2 2v5a5 5 0 0 0 0 10v5a2 2 0 0 0 2 2h7v-2a3 3 0 0 1 6 0v2h7a2 2 0 0 0 2-2v-7h-2a3 3 0 0 1 0-6h2V8a2 2 0 0 0-2-2Zm-5 12a5 5 0 0 0 5 5v5h-5a5 5 0 0 0-10 0H8v-7H6a3 3 0 0 1 0-6h2V8h7V6a3 3 0 0 1 6 0v2h7v5a5 5 0 0 0-5 5Z",
            _ => "M5.333 10.667h21.334c.889 0 1.333.444 1.333 1.333s-.444 1.333-1.333 1.333H5.333C4.444 13.333 4 12.89 4 12s.444-1.333 1.333-1.333Z M13.207 2.667h.126a1.206 1.206 0 0 1 1.2 1.326l-2.413 24.14a1.333 1.333 0 0 1-1.327 1.2h-.126a1.206 1.206 0 0 1-1.2-1.326l2.413-24.14c.068-.682.642-1.2 1.327-1.2Zm8 0h.126a1.206 1.206 0 0 1 1.2 1.326l-2.413 24.14a1.333 1.333 0 0 1-1.327 1.2h-.126a1.206 1.206 0 0 1-1.2-1.326l2.413-24.14c.068-.682.642-1.2 1.327-1.2Z M5.333 18.667h21.334c.889 0 1.333.444 1.333 1.333s-.444 1.333-1.333 1.333H5.333C4.444 21.333 4 20.89 4 20s.444-1.333 1.333-1.333Z"
        };

        builder.OpenElement(0, "svg");
        builder.AddAttribute(1, "viewBox", "0 0 32 32");
        builder.AddAttribute(2, "xmlns", "http://www.w3.org/2000/svg");
        builder.AddAttribute(3, "aria-hidden", "true");
        builder.OpenElement(4, "path");
        builder.AddAttribute(5, "d", path);
        builder.CloseElement();
        builder.CloseElement();
    };

    private void OnPopoverVisibleChange(bool visible) => _popoverVisible = visible && _focused && !string.IsNullOrWhiteSpace(_query);
    private void UpdatePopover() => _popoverVisible = _focused && !string.IsNullOrWhiteSpace(_query);

    private void NavigateToResult(string url)
    {
        _focused = false;
        _query = string.Empty;
        _results = Array.Empty<SearchResult>();
        UpdatePopover();
        NavigationManager.NavigateTo(url);
    }
}
