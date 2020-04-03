using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AntBlazor.Docs.Build.Extensions;
using Microsoft.Extensions.CommandLineUtils;

namespace AntBlazor.Docs.Build.Command
{
    public class GenerateDemoJsonCommand : IAppCommand
    {
        public string Name => "demo2json";

        public void Execute(CommandLineApplication command)
        {
            command.Description = "Generate json file of demos";
            command.HelpOption();

            var directoryArgument = command.Argument(
                "source", "[Required] The directory of demo files.");

            var outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            command.OnExecute(() =>
            {
                var source = directoryArgument.Value;
                var output = outputArgument.Value;

                if (string.IsNullOrEmpty(source) || !Directory.Exists(source))
                {
                    Console.WriteLine("Invalid source.");
                    return 1;
                }

                if (string.IsNullOrEmpty(output))
                {
                    output = "./";
                }

                var demoDirectory = Path.Combine(Directory.GetCurrentDirectory(), source);

                var json = new DemoFileItem(new DirectoryInfo(demoDirectory)).ToJson();
                Console.WriteLine(json);

                var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), output, "demo.json");

                if (File.Exists(configFilePath))
                {
                    File.Delete(configFilePath);
                }

                File.WriteAllText(configFilePath, json);

                Console.WriteLine("Generate demo file to {0}", configFilePath);

                return 0;
            });
        }
    }

    public class DemoFileItem
    {
        public string Title { get; set; }

        public bool IsFolder { get; set; }

        public string Key { get; set; }

        public IEnumerable<DemoFileItem> Children { get; set; }

        public DemoFileItem(FileSystemInfo fsi)
        {
            Title = fsi.Name;

            if (fsi.Attributes == FileAttributes.Directory)
            {
                IsFolder = true;
                Children = from FileSystemInfo f in (fsi as DirectoryInfo)?.GetFileSystemInfos()
                           select new DemoFileItem(f);
            }
            else
            {
                IsFolder = false;
            }
            Key = Title.Replace(" ", "").ToLower();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true
            });
        }
    }
}