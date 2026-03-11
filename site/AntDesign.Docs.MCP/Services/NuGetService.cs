using System.Net.Http.Json;

namespace AntDesign.Docs.MCP.Services;

public static class NuGetService
{
    private static readonly HttpClient _http = new HttpClient();
    private static string? _cachedLatest;

    // Retrieves latest version string from NuGet flat container index.json
    public static async Task<string?> GetLatestVersionAsync(string packageId)
    {
        try
        {
            var id = packageId.ToLowerInvariant();
            var url = $"https://api.nuget.org/v3-flatcontainer/{id}/index.json";
            var doc = await _http.GetFromJsonAsync<NuGetIndex>(url);
            if (doc?.Versions == null || doc.Versions.Length == 0) return null;
            return doc.Versions.Last();
        }
        catch
        {
            return null;
        }
    }

    // Start background check and cache the result
    public static async Task StartBackgroundCheckAsync(string packageId)
    {
        try
        {
            var latest = await GetLatestVersionAsync(packageId);
            if (!string.IsNullOrEmpty(latest))
            {
                _cachedLatest = latest;
            }
        }
        catch { /* ignore */ }
    }

    public static string? GetCachedLatest()
    {
        // Test override from env for CI/local testing
        var overrideValue = Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_TEST_LATEST_VERSION");
        if (!string.IsNullOrEmpty(overrideValue)) return overrideValue;
        return _cachedLatest;
    }

    private class NuGetIndex
    {
        public string[] Versions { get; set; } = Array.Empty<string>();
    }
}