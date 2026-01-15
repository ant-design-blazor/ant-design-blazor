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
        try
        {
            var json = await _httpClient.GetStringAsync(ComponentsUrl);
            _components = JsonSerializer.Deserialize<List<ComponentModel>>(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading components: {ex.Message}");
            throw;
        }
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