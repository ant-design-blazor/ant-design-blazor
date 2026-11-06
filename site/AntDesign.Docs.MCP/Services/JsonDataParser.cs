using System.Text.Json;
using AntDesign.Docs.MCP.Models;

namespace AntDesign.Docs.MCP.Services;

internal static class JsonDataParser
{
    public static List<ComponentModel> ParseComponents(string json)
    {
        try
        {
            // If the JSON directly maps to ComponentModel, prefer deserialization
            var direct = JsonSerializer.Deserialize<List<ComponentModel>>(json);
            if (direct != null && direct.Count > 0)
            {
                return direct;
            }
        }
        catch { /* ignore and try the generic parser below */ }

        var list = new List<ComponentModel>();
        using var doc = JsonDocument.Parse(json);
        foreach (var el in doc.RootElement.EnumerateArray())
        {
            var title = el.GetProperty("Title").GetString() ?? string.Empty;
            var category = el.TryGetProperty("Category", out var c) ? c.GetString() ?? string.Empty : string.Empty;
            var desc = el.TryGetProperty("Description", out var d) ? d.GetString() ?? string.Empty : string.Empty;
            list.Add(new ComponentModel
            {
                Title = title,
                Category = category,
                Description = desc
            });
        }

        return list;
    }

    public static List<DemoModel> ParseDemos(string json)
    {
        var results = new List<DemoModel>();
        try
        {
            using var doc = JsonDocument.Parse(json);
            foreach (var comp in doc.RootElement.EnumerateArray())
            {
                var component = comp.GetProperty("Title").GetString() ?? string.Empty;
                if (!comp.TryGetProperty("DemoList", out var demoList)) continue;
                foreach (var demo in demoList.EnumerateArray())
                {
                    var title = demo.GetProperty("Title").GetString() ?? string.Empty;
                    var desc = demo.TryGetProperty("Description", out var d) ? d.GetString() ?? string.Empty : string.Empty;
                    var code = demo.TryGetProperty("Code", out var c) ? c.GetString() ?? string.Empty : string.Empty;
                    results.Add(new DemoModel
                    {
                        Component = component,
                        Scenario = title,
                        Description = desc,
                        Source = code
                    });
                }
            }
        }
        catch
        {
            // If parsing fails, return empty list
        }

        return results;
    }
}
