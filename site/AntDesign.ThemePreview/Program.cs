using AntDesign;
using AntDesign.Theming;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();

var app = builder.Build();
WriteGeneratedV6Css(app.Environment.WebRootPath);
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();

static void WriteGeneratedV6Css(string webRootPath)
{
    var staticLayer = CssLayerCompiler.CompileStatic(new[]
    {
        Entry(".ant-btn", new Dictionary<string, string>
        {
            ["display"] = "inline-flex", ["align-items"] = "center", ["justify-content"] = "center",
            ["min-width"] = "64px", ["height"] = "32px", ["padding"] = "4px 15px",
            ["color"] = "var(--ant-color-text)", ["font-size"] = "14px", ["line-height"] = "1.5715",
            ["background"] = "var(--ant-color-bg-container)", ["border"] = "1px solid var(--ant-color-border)",
            ["border-radius"] = "6px", ["cursor"] = "pointer", ["transition"] = "all .2s"
        }),
        Entry(".ant-btn:hover", new Dictionary<string, string> { ["color"] = "var(--ant-color-primary)", ["border-color"] = "var(--ant-color-primary)" }),
        Entry(".ant-btn-primary", new Dictionary<string, string> { ["color"] = "#fff", ["background"] = "var(--ant-color-primary)", ["border-color"] = "var(--ant-color-primary)" }),
        Entry(".ant-btn-primary:hover", new Dictionary<string, string> { ["color"] = "#fff", ["filter"] = "brightness(1.08)" }),
        Entry(".ant-btn-dashed", new Dictionary<string, string> { ["border-style"] = "dashed" }),
        Entry(".ant-alert", new Dictionary<string, string>
        {
            ["display"] = "flex", ["align-items"] = "flex-start", ["gap"] = "8px", ["padding"] = "8px 12px",
            ["color"] = "var(--ant-color-text)", ["background"] = "color-mix(in srgb, var(--ant-color-primary) 10%, var(--ant-color-bg-container))",
            ["border"] = "1px solid color-mix(in srgb, var(--ant-color-primary) 30%, var(--ant-color-border))", ["border-radius"] = "6px"
        }),
        Entry(".ant-alert-icon", new Dictionary<string, string> { ["flex"] = "0 0 auto", ["color"] = "var(--ant-color-primary)" }),
        Entry(".ant-alert-content", new Dictionary<string, string> { ["min-width"] = "0" }),
        Entry(".ant-alert-message", new Dictionary<string, string> { ["line-height"] = "1.5715" }),
        Entry(".ant-input", new Dictionary<string, string>
        {
            ["width"] = "100%", ["height"] = "32px", ["padding"] = "4px 11px", ["color"] = "var(--ant-color-text)",
            ["font-size"] = "14px", ["line-height"] = "1.5715", ["background"] = "var(--ant-color-bg-container)",
            ["border"] = "1px solid var(--ant-color-border)", ["border-radius"] = "6px", ["outline"] = "none",
            ["transition"] = "border-color .2s, box-shadow .2s"
        }),
        Entry(".ant-input:focus", new Dictionary<string, string>
        {
            ["border-color"] = "var(--ant-color-primary)",
            ["box-shadow"] = "0 0 0 2px color-mix(in srgb, var(--ant-color-primary) 18%, transparent)"
        })
    });
    var tokenLayer = CssLayerCompiler.CompileTokenValues(new[]
    {
        new DesignToken { CssVariable = "--ant-color-primary", Value = "#1677ff" },
        new DesignToken { CssVariable = "--ant-color-bg-container", Value = "#ffffff" },
        new DesignToken { CssVariable = "--ant-color-text", Value = "#1f1f1f" },
        new DesignToken { CssVariable = "--ant-color-border", Value = "#d9d9d9" }
    }, "light", ":root");
    var css = CssLayerCompiler.Compose(staticLayer, tokenLayer);
    Directory.CreateDirectory(webRootPath);
    File.WriteAllText(Path.Combine(webRootPath, "css", "antd-v6.generated.css"), css, new UTF8Encoding(false));
}

static StyleIrEntry Entry(string selector, IDictionary<string, string> declarations)
{
    var entry = new StyleIrEntry { Component = "ThemePreview", Selector = selector, Layer = "static" };
    foreach (var declaration in declarations) entry.Declarations.Add(new StyleDeclaration { Property = declaration.Key, Value = declaration.Value });
    return entry;
}
