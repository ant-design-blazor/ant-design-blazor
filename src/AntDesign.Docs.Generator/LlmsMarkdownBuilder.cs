using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AntDesign.Docs.Generator;

// Pure compile-time builder; no file/network/reflection or theme execution.
public static class LlmsMarkdownBuilder
{
    private static JsonElement[] Array(JsonElement value) => value.ValueKind == JsonValueKind.Array ? value.EnumerateArray().ToArray() : System.Array.Empty<JsonElement>();
    private static JsonElement Property(JsonElement value, string name) => value.ValueKind == JsonValueKind.Object && value.TryGetProperty(name, out var result) ? result : default;
    private static string Text(JsonElement value, string name) => Property(value, name).ValueKind == JsonValueKind.String ? Property(value, name).GetString() ?? "" : "";
    private static bool Flag(JsonElement value, string name) => Property(value, name).ValueKind == JsonValueKind.True;
    private static string Cell(string value) => (value ?? "").Replace("|", "\\|").Replace("\r", "").Replace("\n", "<br>");
    public static string Build(JsonElement doc, string language, JsonElement semantics, JsonElement tokens)
    {
        var title = Text(doc, "Title");
        var cn = language == "zh-CN";
        var key = Regex.Replace(Regex.Replace(title, "([A-Z]+)([A-Z][a-z])", "$1-$2"), "([a-z0-9])([A-Z])", "$1-$2").ToLowerInvariant();
        var output = new StringBuilder().Append("# ").AppendLine(title).AppendLine()
            .AppendLine("> Ant Design Blazor — C# / Razor documentation, not the React API.").AppendLine()
            .AppendLine("```razor").AppendLine(title == "Icon" ? "@using AntDesign.Icons" : "@using AntDesign").AppendLine("```")
            .AppendLine().AppendLine(FromHtml(Text(doc, "Desc")));
        output.AppendLine().AppendLine("## API").AppendLine().AppendLine(FromHtml(Text(doc, "ApiDoc")));
        if (Text(doc, "Faq").Length > 0) output.AppendLine().AppendLine("## FAQ").AppendLine().AppendLine(FromHtml(Text(doc, "Faq")));
        output.AppendLine().AppendLine("## Semantic DOM").AppendLine()
            .AppendLine("| Name | Selector | Description | Version |").AppendLine("| --- | --- | --- | --- |");
        var slots = Array(Property(semantics, "components")).FirstOrDefault(c => Text(c, "name") == key);
        foreach (var node in Array(Property(slots, "nodes")))
            output.AppendLine($"| {Cell(Text(node, "name"))} | {Cell(Text(node, "selector"))} | {Cell(Text(node, "description"))} | {Cell(Text(node, "version"))} |");
        output.AppendLine().AppendLine("## Design Token").AppendLine()
            .AppendLine("| Token | Description | Type | Default | CSS variable |").AppendLine("| --- | --- | --- | --- | --- |");
        foreach (var token in Array(Property(tokens, "componentTokens")).Where(t => Text(t, "Component") == key && Flag(t, "Documented") && !Flag(t, "Reserved")).OrderBy(t => Text(t, "Name"), StringComparer.Ordinal))
            output.AppendLine($"| {Cell(Text(token, "Name"))} | {Cell(Text(token, cn ? "Description" : "DescriptionEn"))} | {Cell(Text(token, "Type"))} | {Cell(Text(token, "DefaultValue"))} | {Cell(Text(token, "CssVariable"))} |");
        output.AppendLine().AppendLine("### Global tokens used by this component").AppendLine()
            .AppendLine("| Token | Description | Type | Default |").AppendLine("| --- | --- | --- | --- |");
        var globals = Array(Property(tokens, "globalTokens")).ToDictionary(t => Text(t, "Name"), StringComparer.Ordinal);
        foreach (var name in Array(Property(Property(tokens, "globalUsage"), key.Replace("-", ""))).Select(n => n.GetString() ?? ""))
        {
            if (!globals.TryGetValue(name, out var token)) continue;
            output.AppendLine($"| {Cell(name)} | {Cell(Text(token, cn ? "Description" : "DescriptionEn"))} | {Cell(Text(token, "Type"))} | {Cell(Text(Property(tokens, "defaultValues"), name))} |");
        }
        output.AppendLine().AppendLine(cn ? "## 代码演示" : "## Examples");
        foreach (var demo in Array(Property(doc, "DemoList")).Where(d => !Flag(d, "Debug")))
        {
            output.AppendLine().Append("### ").AppendLine(Text(demo, "Title")).AppendLine().AppendLine(FromHtml(Text(demo, "Description")));
            AppendCode(output, Text(demo, "Code"), "razor");
            var files = Property(demo, "AdditionalFiles");
            if (files.ValueKind == JsonValueKind.Object)
                foreach (var file in files.EnumerateObject().OrderBy(f => f.Name, StringComparer.Ordinal))
                {
                    output.AppendLine().Append("#### ").AppendLine(file.Name);
                    AppendCode(output, file.Value.GetString() ?? "", file.Name.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ? "csharp" : "razor");
                }
        }
        return output.ToString().Replace("\r\n", "\n");
    }

    private static void AppendCode(StringBuilder output, string code, string language)
    {
        if (string.IsNullOrWhiteSpace(code)) return;
        var fence = new string('`', Math.Max(3, Regex.Matches(code, "`+").Cast<Match>().Select(m => m.Length + 1).DefaultIfEmpty(3).Max()));
        output.AppendLine().Append(fence).AppendLine(language).AppendLine(code.TrimEnd()).AppendLine(fence);
    }

    public static string FromHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return "";
        var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
        var preserved = new System.Collections.Generic.List<string>();
        string Preserve(string text) { preserved.Add(text); return "\uE000" + (preserved.Count - 1) + "\uE001"; }
        html = Regex.Replace(html, @"<(script|style)\b[^>]*>.*?</\1>", "", options);
        html = Regex.Replace(html, @"<a\b[^>]*class=""anchor""[^>]*>.*?</a>", "", options);
        html = Regex.Replace(html, @"<table\b[^>]*>(.*?)</table>", table => {
            var rows = Regex.Matches(table.Groups[1].Value, @"<tr\b[^>]*>(.*?)</tr>", options).Cast<Match>()
                .Select(row => Regex.Matches(row.Groups[1].Value, @"<t[hd]\b[^>]*>(.*?)</t[hd]>", options).Cast<Match>()
                    .Select(cell => Cell(FromHtml(cell.Groups[1].Value))).ToArray()).Where(row => row.Length > 0).ToArray();
            if (rows.Length == 0) return "";
            return "\n\n" + Preserve("| " + string.Join(" | ", rows[0]) + " |\n| " + string.Join(" | ", rows[0].Select(_ => "---")) + " |\n"
                + string.Join("\n", rows.Skip(1).Select(row => "| " + string.Join(" | ", row) + " |"))) + "\n\n";
        }, options);
        html = Regex.Replace(html, @"<h([1-6])\b[^>]*>(.*?)</h\1>", m => "\n\n" + new string('#', int.Parse(m.Groups[1].Value)) + " " + FromHtml(m.Groups[2].Value).TrimEnd('#') + "\n\n", options);
        html = Regex.Replace(html, @"<a\b[^>]*href=""([^""]*)""[^>]*>(.*?)</a>", m => "[" + FromHtml(m.Groups[2].Value) + "](" + m.Groups[1].Value + ")", options);
        html = Regex.Replace(html, @"<code\b[^>]*>(.*?)</code>", m => {
            var code = WebUtility.HtmlDecode(m.Groups[1].Value);
            var fence = new string('`', Regex.Matches(code, "`+").Cast<Match>().Select(part => part.Length + 1).DefaultIfEmpty(1).Max());
            return Preserve(fence + code + fence);
        }, options);
        html = Regex.Replace(html, @"</span>", " ", options);
        html = Regex.Replace(html, @"<li\b[^>]*>", "\n- ", options);
        html = Regex.Replace(html, @"</(?:p|div|ul|ol|blockquote)>|<br\s*/?>", "\n\n", options);
        html = Regex.Replace(html, "<[^>]+>", "", options);
        html = WebUtility.HtmlDecode(html);
        html = Regex.Replace(html, "\uE000(\\d+)\uE001", m => preserved[int.Parse(m.Groups[1].Value)]);
        return Regex.Replace(html.Replace("\r", ""), @"\n[\t ]*\n(?:[\t ]*\n)+", "\n\n").Trim();
    }
}
