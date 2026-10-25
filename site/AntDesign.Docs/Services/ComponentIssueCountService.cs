using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AntDesign.Docs.Services;

public interface IComponentIssueCountService
{
    Task<int?> GetCountAsync(string component);
}

// A dedicated client: never reuse the site's development certificate handler or
// send credentials to GitHub. Cache shared work, including failures, across circuits.
public sealed class ComponentIssueCountService : IComponentIssueCountService, IDisposable
{
    public const string Repository = "ant-design-blazor/ant-design-blazor";
    private readonly HttpClient _client;
    private readonly Func<DateTimeOffset> _utcNow;
    private readonly Dictionary<string, (DateTimeOffset Expires, Task<int?> Request)> _cache = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _gate = new();
    private DateTimeOffset _retryAfter;

    public ComponentIssueCountService(HttpClient client, Func<DateTimeOffset> utcNow = null)
    {
        _client = client;
        _utcNow = utcNow ?? (() => DateTimeOffset.UtcNow);
    }

    public static string TitleQuery(string component) => $"is:issue is:open {component} in:title";
    public static string SearchUrl(string component) => $"https://github.com/{Repository}/issues?q=" + Uri.EscapeDataString(TitleQuery(component));

    public Task<int?> GetCountAsync(string component)
    {
        if (!Regex.IsMatch(component ?? "", "\\A[A-Za-z][A-Za-z0-9-]{0,63}\\z")) return Task.FromResult<int?>(null);
        lock (_gate)
        {
            var now = _utcNow();
            if (_cache.TryGetValue(component, out var cached) && cached.Expires > now) return cached.Request;
            if (_retryAfter > now) return Task.FromResult<int?>(null);
            var request = FetchAsync(component);
            _cache[component] = (now.AddMinutes(5), request);
            return request;
        }
    }

    private async Task<int?> FetchAsync(string component)
    {
        try
        {
            var query = $"repo:{Repository} {TitleQuery(component)}";
            using var request = new HttpRequestMessage(HttpMethod.Get,
                "https://api.github.com/search/issues?per_page=1&q=" + Uri.EscapeDataString(query));
            request.Headers.UserAgent.ParseAdd("AntDesignBlazorDocs/1.0");
            request.Headers.Accept.ParseAdd("application/vnd.github+json");
            using var response = await _client.SendAsync(request).ConfigureAwait(false);
            if ((int)response.StatusCode is 403 or 429)
            {
                lock (_gate) _retryAfter = _utcNow().AddMinutes(5);
                return null;
            }
            if (!response.IsSuccessStatusCode) return null;
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var root = json.RootElement;
            if (root.TryGetProperty("incomplete_results", out var incomplete) && incomplete.ValueKind == JsonValueKind.True) return null;
            return root.TryGetProperty("total_count", out var total) && total.TryGetInt32(out var count) && count >= 0 ? count : null;
        }
        catch (Exception error) when (error is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException)
        {
            return null;
        }
    }

    public void Dispose() => _client.Dispose();
}
