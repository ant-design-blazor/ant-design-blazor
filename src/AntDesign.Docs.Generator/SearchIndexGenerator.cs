using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AntDesign.Docs.Generator;

[Generator]
public sealed class SearchIndexGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                && (file.Path.Replace('\\', '/').Contains("/Demos/Components/")
                    || file.Path.Replace('\\', '/').Contains("/docs/")))
            .Select((file, token) => new Input(file.Path.Replace('\\', '/'), file.GetText(token)?.ToString() ?? ""))
            .Collect();

        context.RegisterSourceOutput(files, (output, inputs) =>
        {
            var documents = inputs.OrderBy(file => file.Path, StringComparer.Ordinal).ToArray();
            var entries = new List<Entry>();
            foreach (var doc in documents)
            {
                var name = Path.GetFileName(doc.Path);
                var language = name.EndsWith(".zh-CN.md", StringComparison.Ordinal) ? "zh-CN"
                    : name.EndsWith(".en-US.md", StringComparison.Ordinal) ? "en-US" : null;
                if (language == null) continue;

                if (doc.Path.IndexOf("/Demos/Components/", StringComparison.Ordinal) >= 0 && doc.Path.IndexOf("/doc/index.", StringComparison.Ordinal) >= 0)
                {
                    var frontMatter = ReadFrontMatter(doc.Text);
                    if (!string.Equals(ReadValue(frontMatter, "category"), "Components", StringComparison.OrdinalIgnoreCase)) continue;
                    var title = ReadValue(frontMatter, "title");
                    if (string.IsNullOrWhiteSpace(title)) continue;
                    var slug = title.ToLowerInvariant();
                    var body = RemoveFrontMatter(doc.Text);
                    var api = Regex.Match(body, @"(?im)^##\s+API\s*$");
                    var intro = api.Success ? body.Substring(0, api.Index) : body;
                    var pageUrl = $"/{language}/components/{slug}";
                    entries.Add(new Entry(language, "page", title, PlainText(intro), title, pageUrl));

                    var searchableSections = api.Success ? intro : body;
                    foreach (Match heading in Regex.Matches(searchableSections, @"(?m)^#{2,3}\s+(.+?)\s*$"))
                    {
                        var headingTitle = heading.Groups[1].Value.Trim();
                        var sectionStart = heading.Index + heading.Length;
                        var remaining = searchableSections.Substring(sectionStart);
                        var next = Regex.Match(remaining, @"(?m)^#{2,3}\s+");
                        var end = next.Success ? sectionStart + next.Index : searchableSections.Length;
                        var sectionText = searchableSections.Substring(sectionStart, end - sectionStart);
                        entries.Add(new Entry(language, "section", headingTitle, PlainText(sectionText), title, pageUrl + "#" + headingTitle.ToLowerInvariant().Replace(' ', '-')));
                    }

                    var demoDirectory = doc.Path.Substring(0, doc.Path.LastIndexOf("/doc/", StringComparison.Ordinal)) + "/demo/";
                    foreach (var demo in documents.Where(file => file.Path.StartsWith(demoDirectory, StringComparison.Ordinal)
                        && file.Path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)))
                    {
                        var demoName = Path.GetFileNameWithoutExtension(demo.Path);
                        var demoFrontMatter = ReadFrontMatter(demo.Text);
                        var localizedTitle = ReadLocalizedValue(demoFrontMatter, language);
                        if (string.IsNullOrWhiteSpace(localizedTitle)) continue;
                        var demoBody = RemoveFrontMatter(demo.Text);
                        var localizedSection = Regex.Match(demoBody, $@"(?ims)^##\s+{Regex.Escape(language)}\s*\r?\n(.*?)(?=^##\s+|\z)");
                        var description = localizedSection.Success ? localizedSection.Groups[1].Value : demoBody;
                        entries.Add(new Entry(language, "demo", localizedTitle, PlainText(description), title,
                            pageUrl + "#components-" + slug + "-demo-" + demoName));
                    }
                }
                else if (doc.Path.IndexOf("/docs/", StringComparison.Ordinal) >= 0)
                {
                    var pageKey = name.Substring(0, name.Length - language.Length - 4);
                    var title = ReadValue(ReadFrontMatter(doc.Text), "title");
                    if (string.IsNullOrWhiteSpace(title)) title = Regex.Match(RemoveFrontMatter(doc.Text), @"(?m)^#\s+(.+?)\s*$").Groups[1].Value;
                    if (string.IsNullOrWhiteSpace(title)) title = pageKey;
                    entries.Add(new Entry(language, "page", title, PlainText(RemoveFrontMatter(doc.Text)), title, $"/{language}/docs/{pageKey}"));
                }
            }

            const int entriesPerChunk = 120;
            var chunks = (entries.Count + entriesPerChunk - 1) / entriesPerChunk;
            var source = new StringBuilder("// <auto-generated />\n#nullable enable\nnamespace AntDesign.Docs.Services;\ninternal static class GeneratedSearchIndex\n{\n    internal sealed record Entry(string Language, string Type, string Title, string Description, string PageTitle, string Url);\n    private static readonly string[] Packed = new string[] {\n");
            for (var chunk = 0; chunk < chunks; chunk++)
            {
                var rows = entries.Skip(chunk * entriesPerChunk).Take(entriesPerChunk)
                    .Select(entry => string.Join("\t", entry.Language, entry.Type, entry.Title, entry.Description, entry.PageTitle, entry.Url));
                source.Append("        ").Append(Literal(Pack(string.Join("\n", rows) + "\n"))).AppendLine(",");
            }
            source.AppendLine("    };\n    internal static readonly Entry[] All = Decode();")
                .AppendLine("    private static Entry[] Decode()")
                .AppendLine("    {")
                .AppendLine("        var entries = new System.Collections.Generic.List<Entry>();")
                .AppendLine("        foreach (var packed in Packed)")
                .AppendLine("        {")
                .AppendLine("            using var input = new System.IO.MemoryStream(System.Convert.FromBase64String(packed));")
                .AppendLine("            using var gzip = new System.IO.Compression.GZipStream(input, System.IO.Compression.CompressionMode.Decompress);")
                .AppendLine("            using var reader = new System.IO.StreamReader(gzip, System.Text.Encoding.UTF8);")
                .AppendLine("            string? line;")
                .AppendLine("            while ((line = reader.ReadLine()) != null)")
                .AppendLine("            {")
                .AppendLine("                var fields = line.Split('\\t');")
                .AppendLine("                if (fields.Length == 6) entries.Add(new Entry(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5]));")
                .AppendLine("            }")
                .AppendLine("        }")
                .AppendLine("        return entries.ToArray();")
                .AppendLine("    }\n}");
            output.AddSource("GeneratedSearchIndex.g.cs", SourceText.From(source.ToString(), Encoding.UTF8));
        });
    }

    private static string Literal(string value) => Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(value, true);
    private static string Pack(string value)
    {
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal, true))
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            gzip.Write(bytes, 0, bytes.Length);
        }
        return Convert.ToBase64String(output.ToArray());
    }
    private static string ReadFrontMatter(string text) => Regex.Match(text.TrimStart('\uFEFF'), @"\A---\s*\r?\n(.*?)\r?\n---\s*(?:\r?\n|\z)", RegexOptions.Singleline).Groups[1].Value;
    private static string RemoveFrontMatter(string text) => Regex.Replace(text.TrimStart('\uFEFF'), @"\A---\s*\r?\n.*?\r?\n---\s*(?:\r?\n|\z)", "", RegexOptions.Singleline);
    private static string ReadValue(string yaml, string key) => Regex.Match(yaml, $@"(?m)^{Regex.Escape(key)}:\s*([^\r\n]+)").Groups[1].Value.Trim().Trim('"', '\'');
    private static string ReadLocalizedValue(string yaml, string language) => Regex.Match(yaml, $@"(?ms)^title:\s*\r?\n.*?^\s+{Regex.Escape(language)}:\s*(.+?)\s*$").Groups[1].Value.Trim().Trim('"', '\'');
    private static string PlainText(string value)
    {
        value = Regex.Replace(value, @"(?s)```.*?```|<!--.*?-->", " ");
        value = Regex.Replace(value, @"!?(\[([^\]]*)\])\([^)]*\)", "$2");
        value = Regex.Replace(value, @"`([^`]*)`", "$1");
        value = Regex.Replace(value, @"(?m)^\s{0,3}#{1,6}\s*", "");
        value = Regex.Replace(value, @"[>*_~|]", " ");
        value = Regex.Replace(value, @"\s+", " ").Trim();
        return value.Length > 800 ? value.Substring(0, 797) + "..." : value;
    }

    private sealed class Input
    {
        internal string Path { get; }
        internal string Text { get; }
        internal Input(string path, string text) { Path = path; Text = text; }
    }

    private sealed class Entry
    {
        internal string Language { get; }
        internal string Type { get; }
        internal string Title { get; }
        internal string Description { get; }
        internal string PageTitle { get; }
        internal string Url { get; }
        internal Entry(string language, string type, string title, string description, string pageTitle, string url)
        { Language = language; Type = type; Title = title; Description = description; PageTitle = pageTitle; Url = url; }
    }
}
