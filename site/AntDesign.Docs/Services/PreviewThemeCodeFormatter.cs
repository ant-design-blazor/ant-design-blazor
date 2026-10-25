using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using AntDesign.Theming;

namespace AntDesign.Docs.Services;

public static class PreviewThemeCodeFormatter
{
    public static string Format(ThemeConfig theme)
    {
        string Quote(string value) => JsonSerializer.Serialize(value);
        var code = new StringBuilder("using AntDesign.Theming;\nusing System.Collections.Generic;\n\nvar theme = new ThemeConfig\n{\n");
        code.AppendLine($"    Key = {Quote(theme.Key)},");
        code.AppendLine($"    Algorithm = {Quote(theme.Algorithm)},");
        code.AppendLine($"    Inherit = {theme.Inherit.ToString().ToLowerInvariant()},");
        code.AppendLine("    Token = new ThemeToken\n    {");
        foreach (var token in theme.GetExplicitTokenValues()) code.AppendLine($"        [{Quote(token.Key)}] = {Quote(token.Value)},");
        code.AppendLine("    },\n    Components = new Dictionary<string, IDictionary<string, string>>\n    {");
        foreach (var component in theme.Components)
        {
            code.AppendLine($"        [{Quote(component.Key)}] = new Dictionary<string, string>\n        {{");
            foreach (var token in component.Value) code.AppendLine($"            [{Quote(token.Key)}] = {Quote(token.Value)},");
            code.AppendLine("        },");
        }
        return code.AppendLine("    },\n};").ToString();
    }
}
