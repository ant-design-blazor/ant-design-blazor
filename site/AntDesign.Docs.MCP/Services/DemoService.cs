// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using AntDesign.Docs.MCP.Models;

namespace AntDesign.Docs.MCP.Services;

public class DemoService
{
    private List<DemoModel>? _demos;
    private const string RemoteUrl = "https://antblazor.com/_content/AntDesign.Docs/meta/components.en-US.json";
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<DemoModel>> LoadDemosAsync()
    {
        if (_demos != null) return _demos;

        // Allow forcing offline mode for testing via environment variable
        var forceOffline = string.Equals(Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_FORCE_OFFLINE"), "1", StringComparison.OrdinalIgnoreCase)
                           || string.Equals(Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_FORCE_OFFLINE"), "true", StringComparison.OrdinalIgnoreCase);

        // Try download first (unless forced offline)
        if (!forceOffline)
        {
            try
            {
                var json = await _httpClient.GetStringAsync(RemoteUrl);
                var parsed = JsonDataParser.ParseDemos(json);
                if (parsed.Count > 0)
                {
                    _demos = parsed;
                    return _demos;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Warning: failed to download demos from {RemoteUrl}: {ex.Message}");
            }
        }

        // Fallback to local JSON files shipped with the package
        try
        {
            var baseDir = AppContext.BaseDirectory;

            // Prefer offline-specific files when present (useful for testing)
            var offlineFiles = Directory.GetFiles(baseDir, "*.offline.json", SearchOption.AllDirectories);
            string[] files;
            if (offlineFiles.Length > 0)
            {
                files = offlineFiles;
            }
            else
            {
                files = Directory.GetFiles(baseDir, "demos*.json", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    files = Directory.GetFiles(baseDir, "*.json", SearchOption.AllDirectories);
                }
            }

            foreach (var f in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(f);
                    var parsed = JsonDataParser.ParseDemos(json);
                    if (parsed.Count > 0)
                    {
                        _demos = parsed;
                        return _demos;
                    }
                }
                catch { /* ignore and try next file */ }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading local demos JSON: {ex.Message}");
        }

        throw new InvalidOperationException("Demos data not available (remote download failed and no local data found).");
    }
}