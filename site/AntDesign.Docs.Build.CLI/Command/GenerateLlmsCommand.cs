using System;
using System.IO;
using System.Linq;
using System.Text;
using AntDesign.Docs.Services;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Command;

public sealed class GenerateLlmsCommand : IAppCommand
{
    public string Name => "llms";

    public void Execute(CommandLineApplication command)
    {
        command.Description = "Export compiled Blazor component Markdown for static hosting (build docs metadata first).";
        var output = command.Argument("output", "Static website root directory");
        command.OnExecute(() => {
            if (string.IsNullOrWhiteSpace(output.Value)) return 1;
            var root = Path.GetFullPath(output.Value);
            var index = new StringBuilder("# Ant Design Blazor\n\n");
            foreach (var language in new[] { "en-US", "zh-CN" })
            {
                var directory = Path.Combine(root, language, "components");
                Directory.CreateDirectory(directory);
                index.AppendLine("## " + language).AppendLine();
                foreach (var name in ComponentMarkdown.Components(language))
                {
                    var filename = name.ToLowerInvariant() + ".md";
                    File.WriteAllText(Path.Combine(directory, filename), ComponentMarkdown.Get(name, language), new UTF8Encoding(false));
                    index.AppendLine($"- [{name}]({language}/components/{filename})");
                }
                index.AppendLine();
                var docsDirectory = Path.Combine(root, language, "docs");
                Directory.CreateDirectory(docsDirectory);
                foreach (var name in ComponentMarkdown.Docs(language))
                {
                    File.WriteAllText(Path.Combine(docsDirectory, name + ".md"), ComponentMarkdown.GetDoc(name, language), new UTF8Encoding(false));
                    index.AppendLine($"- [{name}]({language}/docs/{name}.md)");
                }
            }
            File.WriteAllText(Path.Combine(root, "llms.txt"), index.ToString(), new UTF8Encoding(false));
            Console.WriteLine("Exported compiled component Markdown to " + root);
            return 0;
        });
    }
}
