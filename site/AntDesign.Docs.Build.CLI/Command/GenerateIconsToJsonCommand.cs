using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using AntDesign.Docs.Build.CLI.Extensions;
using AntDesign.Docs.Build.CLI.Utils;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Command
{
    public class GenerateIconsToJsonCommand : IAppCommand
    {
        public string Name => "icons2json";

        public void Execute(CommandLineApplication command)
        {
            command.Description = "Generate json file of icons";
            command.HelpOption();

            CommandArgument directoryArgument = command.Argument(
                "source", "[Required] The directory of icon files.");

            CommandArgument outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            command.OnExecute(() =>
            {
                string source = directoryArgument.Value;
                string output = outputArgument.Value;

                if (string.IsNullOrEmpty(source) || !Directory.Exists(source))
                {
                    Console.WriteLine("Invalid source.");
                    return 1;
                }

                if (string.IsNullOrEmpty(output))
                {
                    output = "./";
                }

                string iconDirectory = Path.Combine(Directory.GetCurrentDirectory(), source);

                GenerateFiles(iconDirectory, output);

                return 0;
            });
        }

        private void GenerateFiles(string iconDirectory, string output)
        {
            DirectoryInfo iconDirectoryInfo = new DirectoryInfo(iconDirectory);

            IDictionary<string, string[]> iconList = new Dictionary<string, string[]>();

            if (!iconDirectoryInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                Console.WriteLine("{0} is not a directory", iconDirectory);
                return;
            }

            var themes = iconDirectoryInfo.GetFileSystemInfos().Where(x => x.Attributes.HasFlag(FileAttributes.Directory));

            foreach (DirectoryInfo themeDirectory in themes)
            {
                var theme = themeDirectory.Name;
                iconList[theme] = themeDirectory.GetFileSystemInfos().Select(x => x.Name.Replace($"{x.Extension}", "")).ToArray();
            }

            var demoJson = JsonSerializer.Serialize(iconList, new JsonSerializerOptions()
            {
                WriteIndented = false,
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            string configFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), output);

            string demoFilePath = Path.Combine(configFileDirectory, $"icons.json");
            if (File.Exists(demoFilePath))
            {
                File.Delete(demoFilePath);
            }
            File.WriteAllText(demoFilePath, demoJson);
        }
    }
}
