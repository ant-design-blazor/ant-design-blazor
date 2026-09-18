using System.Security.Cryptography;
using System.Text.Json;
using AntDesign.Theming;

static string Argument(string[] values, string name)
{
    var index = Array.IndexOf(values, name);
    if (index < 0 || index + 1 >= values.Length) throw new ArgumentException($"Missing {name}.");
    return values[index + 1];
}

static string? OptionalArgument(string[] values, string name)
{
    var index = Array.IndexOf(values, name);
    return index >= 0 && index + 1 < values.Length ? values[index + 1] : null;
}

static int FirstDifference(string expected, string actual)
{
    var length = Math.Min(expected.Length, actual.Length);
    for (var index = 0; index < length; index++)
        if (expected[index] != actual[index]) return index;
    return expected.Length == actual.Length ? -1 : length;
}

try
{
    var input = Argument(args, "--react-css");
    var output = Argument(args, "--output");
    var revision = Argument(args, "--revision");
    var css = File.ReadAllText(input);
    var artifact = CssLayerCompiler.ImportCompiledStaticCss(css);
    Directory.CreateDirectory(Path.Combine(output, "static"));
    File.WriteAllText(Path.Combine(output, "static", "antd.css"), artifact.Css);
    var manifest = new
    {
        schemaVersion = 1,
        upstream = new { repository = "ant-design", revision, version = "6.6.4" },
        generator = new { name = "ant-design-blazor-dotnet-static-import", version = "0.1.0" },
        protocol = new { encoding = "utf-8", lineEnding = "lf", trailingNewline = artifact.Css.EndsWith("\n") ? "one" : "preserve" },
        artifacts = new[] { new
        {
            id = "antd.light.static", path = "static/antd.css", layer = "static", component = "antd", theme = "light",
            encoding = "utf-8", lineEnding = "lf", trailingNewline = artifact.Css.EndsWith("\n") ? "one" : "none",
            sha256 = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(artifact.Css))).ToLowerInvariant(),
            byteLength = System.Text.Encoding.UTF8.GetByteCount(artifact.Css)
        }}
    };
    await File.WriteAllTextAsync(Path.Combine(output, "manifest.json"), JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true }) + "\n");
    string? themeOutput = null;
    themeOutput = OptionalArgument(args, "--theme-css-output");
    if (themeOutput != null)
    {
        Directory.CreateDirectory(themeOutput);
        foreach (var definition in GeneratedPreviewThemes.All)
        {
            var theme = PreviewThemeConfigFactory.Create(definition);
            var result = new StaticStyleExporter().Export(new StaticStyleRequest
            {
                Theme = theme,
                StaticCss = artifact.Css,
                IncludeStyleTag = false
            });
            await File.WriteAllTextAsync(Path.Combine(themeOutput, $"{definition.Key}.css"), result.Css);
        }
    }
    var reactThemeOutput = OptionalArgument(args, "--react-theme-css-dir");
    if (reactThemeOutput != null && themeOutput == null)
        throw new ArgumentException("--react-theme-css-dir requires --theme-css-output.");
    if (reactThemeOutput != null)
    {
        var comparisons = GeneratedPreviewThemes.All.Select(definition =>
        {
            var expectedPath = Path.Combine(reactThemeOutput, $"{definition.Key}.css");
            var actualPath = Path.Combine(themeOutput!, $"{definition.Key}.css");
            if (!File.Exists(expectedPath)) return new { theme = definition.Key, equal = false, kind = "missing-react", firstDifference = -1 };
            var expected = File.ReadAllText(expectedPath);
            var actual = File.ReadAllText(actualPath);
            return new { theme = definition.Key, equal = string.Equals(expected, actual, StringComparison.Ordinal), kind = "full-css", firstDifference = FirstDifference(expected, actual) };
        }).ToList();
        await File.WriteAllTextAsync(Path.Combine(output, "theme-comparison.json"), JsonSerializer.Serialize(comparisons, new JsonSerializerOptions { WriteIndented = true }) + "\n");
        if (comparisons.Any(item => !item.equal))
            throw new InvalidOperationException($"React/Blazor theme CSS mismatch: {string.Join(", ", comparisons.Where(item => !item.equal).Select(item => item.theme))}");
    }
    Console.WriteLine(JsonSerializer.Serialize(new { output, themeOutput, reactThemeOutput, themes = GeneratedPreviewThemes.All.Count, revision, bytes = artifact.Css.Length }));
}
catch (Exception error)
{
    Console.Error.WriteLine($"antdesign-style-dotnet: {error}");
    Environment.ExitCode = 1;
}
