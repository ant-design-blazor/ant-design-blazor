// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using AntDesign.Docs.MCP.Models;

namespace AntDesign.Docs.MCP.Services;

public class ComponentService
{
    private readonly HttpClient _httpClient;
    private List<ComponentModel>? _components;
    private const string ComponentsUrl = "https://antblazor.com/_content/AntDesign.Docs/meta/components.en-US.json";

    public ComponentService()
    {
        _httpClient = new HttpClient();
    }

    public async Task LoadComponentsAsync()
    {
        if (_components != null) return;

        // Allow forcing offline mode for testing via environment variable
        var forceOffline = string.Equals(Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_FORCE_OFFLINE"), "1", StringComparison.OrdinalIgnoreCase)
                           || string.Equals(Environment.GetEnvironmentVariable("ANT_DESIGN_DOCS_FORCE_OFFLINE"), "true", StringComparison.OrdinalIgnoreCase);

        // Try downloading components JSON first (unless forced offline)
        if (!forceOffline)
        {
            try
            {
                var json = await _httpClient.GetStringAsync(ComponentsUrl);
                var parsed = JsonDataParser.ParseComponents(json);
                if (parsed.Count > 0)
                {
                    _components = parsed;
                    return;
                }
            }
            catch (Exception ex)
            {
                // fallback to local file
                Console.Error.WriteLine($"Warning: failed to download components from {ComponentsUrl}: {ex.Message}");
            }
        }

        // Fallback: try to read components JSON from local data files shipped with package
        try
        {
            var baseDir = AppContext.BaseDirectory;

            // If forced offline, prefer files that indicate offline (e.g. *.offline.json)
            var offlineFiles = Directory.GetFiles(baseDir, "*.offline.json", SearchOption.AllDirectories);
            string[] files;
            if (offlineFiles.Length > 0)
            {
                files = offlineFiles;
            }
            else
            {
                files = Directory.GetFiles(baseDir, "components*.json", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    // also try any json produced (legacy)
                    files = Directory.GetFiles(baseDir, "*.json", SearchOption.AllDirectories);
                }
            }

            foreach (var f in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(f);
                    var parsed = JsonDataParser.ParseComponents(json);
                    if (parsed != null && parsed.Count > 0)
                    {
                        _components = parsed;
                        return;
                    }
                }
                catch { /* ignore and try next file */ }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading local components JSON: {ex.Message}");
        }

        throw new InvalidOperationException("Components data not available (remote download failed and no local data found).");
    }

    public ComponentModel? FindComponent(string name)
    {
        if (_components == null)
        {
            throw new InvalidOperationException("Components not loaded. Call LoadComponentsAsync first.");
        }

        return _components.FirstOrDefault(c => 
            c.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<string> ListComponents()
    {
        if (_components == null)
        {
            throw new InvalidOperationException("Components not loaded. Call LoadComponentsAsync first.");
        }

        return _components.Select(c => c.Title);
    }
}