namespace AntDesign.Docs.Services;

/// <summary>Serves Markdown prepared by AntDesign.Docs.Generator at compilation.</summary>
public static class ComponentMarkdown
{
    public static string Get(string component, string language) => GeneratedLlmsMarkdown.Get("components", component ?? "", language);
    public static string[] Components(string language) => GeneratedLlmsMarkdown.Names("components", language);
    public static string GetDoc(string name, string language) => GeneratedLlmsMarkdown.Get("docs", name ?? "", language);
    public static string[] Docs(string language) => GeneratedLlmsMarkdown.Names("docs", language);
}
