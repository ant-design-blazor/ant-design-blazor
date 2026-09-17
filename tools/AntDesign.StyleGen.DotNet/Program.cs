using System.Security.Cryptography;
using System.Text.Json;
using AntDesign.Theming;

static string Argument(string[] values, string name)
{
    var index = Array.IndexOf(values, name);
    if (index < 0 || index + 1 >= values.Length) throw new ArgumentException($"Missing {name}.");
    return values[index + 1];
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
    Console.WriteLine(JsonSerializer.Serialize(new { output, revision, bytes = artifact.Css.Length }));
}
catch (Exception error)
{
    Console.Error.WriteLine($"antdesign-style-dotnet: {error.Message}");
    Environment.ExitCode = 1;
}
