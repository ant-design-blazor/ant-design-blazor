// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using AntDesign.Docs.MCP.Services;
using AntDesign.Docs.MCP.Models;
using ModelContextProtocol.Server;
using SmartComponents.LocalEmbeddings;
using System.Reflection;

namespace AntDesign.Docs.MCP.Tools;

[McpServerToolType]
public sealed class AntDesignTools
{
    private readonly ComponentService _componentService;
    private readonly DemoService _demoService;

    // NOTE: Keep this in sync with the <PackageVersion> in the project file.
    // Resolve package id and current package version from assembly metadata
    private static string PackageId =>
        typeof(AntDesignTools).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => string.Equals(a.Key, "PackageId", StringComparison.OrdinalIgnoreCase))?.Value
        ?? typeof(AntDesignTools).Assembly.GetName().Name ?? "AntDesign.Docs.MCP";

    private static string CurrentPackageVersion =>
        typeof(AntDesignTools).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? typeof(AntDesignTools).Assembly.GetName().Version?.ToString() ?? "0.0.0";

    // Services are provided via DI. Background prefetch is handled by PrefetchBackgroundService.
    public AntDesignTools(ComponentService componentService, DemoService demoService)
    {
        _componentService = componentService;
        _demoService = demoService;
    }

    private static async Task<string> AppendUpdateNoticeIfAny(string content)
    {
        try
        {
            // Use cached version if available (background check on startup)
            var latest = NuGetService.GetCachedLatest();
            if (string.IsNullOrEmpty(latest))
            {
                // do not block for a network call here; if cached value is unavailable just return original content
                return content;
            }

            if (!string.Equals(latest, CurrentPackageVersion, StringComparison.OrdinalIgnoreCase))
            {
                // Try to compare semver-ish versions. If parsing fails, fall back to string inequality.
                if (Version.TryParse(latest, out var latestV) && Version.TryParse(CurrentPackageVersion, out var currentV))
                {
                    if (latestV > currentV)
                    {
                        var notice = $"⚠️ A newer version ({latest}) of this tool is available. Update with:\n`dotnet tool update -g {PackageId}`";
                        return content + "\n\n" + notice;
                    }
                }
                else
                {
                    // Unknown formats, but versions differ — still notify
                    var notice = $"⚠️ A newer version ({latest}) of this tool may be available. Update with:\n`dotnet tool update -g {PackageId}`";
                    return content + "\n\n" + notice;
                }
            }
        }
        catch { /* ignore */ }

        return content;
    }

    [McpServerTool]
    [Description("Search for multiple Ant Design Blazor components by names")]
    public async Task<string> SearchComponents(
        [Description("Comma-separated list of component names to search for, were splited by ','")] string names)
    {
        await _componentService.LoadComponentsAsync();
        var componentNames = names.Split(',').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n));
        var results = new List<string>();

        foreach (var name in componentNames)
        {
            var component = _componentService.FindComponent(name);
            if (component != null)
            {
                results.Add($"### {name}\n{component}");
            }
            else
            {
                results.Add($"### {name}\nComponent not found.");
            }
        }

        var output = string.Join("\n\n", results);
        return await AppendUpdateNoticeIfAny(output);
    }

    [McpServerTool]
    [Description("List all available Ant Design Blazor components")]
    public async Task<string> ListComponents()
    {
        await _componentService.LoadComponentsAsync();
        var components = _componentService.ListComponents();
        
        var output = $"Available components:\n{string.Join("\n", components.Select(c => $"  {c}"))}";
        return await AppendUpdateNoticeIfAny(output);
    }

    [McpServerTool]
    [Description("Get component information by category")]
    public async Task<string> GetComponentsByCategory(
        [Description("The category to filter components by (e.g. 'Components', 'Feedback', 'Navigation')")] string category)
    {
        await _componentService.LoadComponentsAsync();
        var components = _componentService.ListComponents()
            .Where(c => _componentService.FindComponent(c)?.Category.Equals(category, StringComparison.OrdinalIgnoreCase) ?? false);

        if (!components.Any())
        {
            return $"No components found in category '{category}'.";
        }

        var output = $"Components in category '{category}':\n{string.Join("\n", components.Select(c => $"  {c}"))}";
        return await AppendUpdateNoticeIfAny(output);
    }

    [McpServerTool]
    [Description("Retrieve the full C# source code for Ant Design Blazor demo(s) by specifying component name and optional scenario. Accepts one or more 'Component:Scenario' pairs separated by commas (e.g., 'Button:Icon, Table:Editable'). Scenario can be partial or left blank to automatically choose the most relevant demo. Returns Markdown-formatted code block(s) containing the demo source.")]
    public async Task<string> SearchComponentDemos(
        [Description("Comma-separated list of 'Component:Scenario' pairs (e.g., 'Button:Icon, Table:Editable'). Leave Scenario blank to fetch the best match.")] string queries)
    {

        var queryPairs = queries.Split(',')
            .Select(q => q.Trim())
            .Where(q => !string.IsNullOrEmpty(q))
            .Select(q =>
            {
                var parts = q.Split(':');
                return (Component: parts[0].Trim(), Scenario: parts.Length > 1 ? parts[1].Trim() : "");
            });
        var results = new List<string>();
        using var embedder = new LocalEmbedder();
        var allDemos = await _demoService.LoadDemosAsync();
        foreach (var (component, scenario) in queryPairs)
        {
            var candidates = allDemos.Where(d => d.Component.Equals(component, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!candidates.Any())
            {
                results.Add($"### {component} - {scenario}\nDemo not found.");
                continue;
            }
            if (string.IsNullOrWhiteSpace(scenario))
            {
                var demo = candidates.First();
                results.Add($"### {component} - {demo.Scenario}\n```csharp\n{demo.Source}\n```");
                continue;
            }
            var queryVec = embedder.Embed(scenario);
            var best = candidates
                .Select(d => (Demo: d, Score: queryVec.Similarity(embedder.Embed(d.Scenario + " " + d.Description))))
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (best.Demo != null && best.Score > 0)
            {
                results.Add($"### {component} - {best.Demo.Scenario}\n```csharp\n{best.Demo.Source}\n```");
            }
            else
            {
                results.Add($"### {component} - {scenario}\nDemo not found.");
            }
        }
        var output = string.Join("\n\n", results);
        return await AppendUpdateNoticeIfAny(output);
    }

    [McpServerTool]
    [Description("List every available Ant Design Blazor demo with its component, scenario, and description. Use this command for exploration; afterwards call 'SearchComponentDemos' with 'Component:Scenario' to get the demo's source code.")]
    public async Task<string> ListAllDemos()
    {
        var allDemos = await _demoService.LoadDemosAsync();
        if (allDemos == null || allDemos.Count == 0)
        {
            return "No demos found.";
        }
        var lines = allDemos.Select(d => $"Component: {d.Component}\nScenario: {d.Scenario}\nDescription: {d.Description}\n");
        var output = string.Join("\n", lines);
        return await AppendUpdateNoticeIfAny(output);
    }

    [McpServerTool]
    [Description("List demos for a given component (component name)")]
    public async Task<string> ListDemosForComponent([Description("Component name")] string component)
    {
        var allDemos = await _demoService.LoadDemosAsync();
        var found = allDemos.Where(d => d.Component.Equals(component, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!found.Any())
        {
            return $"No demos found for component '{component}'.";
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Demos for {component}:");
        foreach (var d in found)
        {
            sb.AppendLine($"- {d.Scenario}: {d.Description}");
        }

        var output = sb.ToString();
        return await AppendUpdateNoticeIfAny(output);
    }

    [McpServerTool]
    [Description("Get demo source for component[:scenario]. Returns formatted Markdown code block(s).")]
    public async Task<string> GetDemoSource([Description("Component[:Scenario]")] string query)
    {
        // Reuse existing SearchComponentDemos to keep behavior consistent
        return await SearchComponentDemos(query);
    }
}
