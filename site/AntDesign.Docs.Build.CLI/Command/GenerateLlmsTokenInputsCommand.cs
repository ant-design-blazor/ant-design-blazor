using System;
using System.IO;
using System.Text.Json;
using AntDesign.Theming;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Command;

public sealed class GenerateLlmsTokenInputsCommand : IAppCommand
{
    public string Name => "llms-tokens";
    public void Execute(CommandLineApplication command)
    {
        var output = command.Argument("output", "Token input JSON consumed by AntDesign.Docs.Generator");
        command.OnExecute(() => {
            if (string.IsNullOrWhiteSpace(output.Value)) return 1;
            var data = new {
                schemaVersion = 1,
                componentTokens = GeneratedComponentTokens.All,
                globalTokens = GeneratedGlobalTokens.All,
                globalUsage = GeneratedComponentGlobalTokens.All,
                defaultValues = new ThemeConfig().GetResolvedTokenValues(),
            };
            File.WriteAllText(output.Value, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine("Generated LLM token inputs: " + output.Value);
            return 0;
        });
    }
}
