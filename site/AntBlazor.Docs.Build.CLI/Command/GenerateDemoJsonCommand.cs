using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using AntBlazor.Docs.Build.CLI.Utils;
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

                GenerateFiles(demoDirectory, output);

                return 0;
            });
        }

        public void GenerateFiles(string demoDirectory, string output)
        {
            var demoDirectoryInfo = new DirectoryInfo(demoDirectory);
            if (demoDirectoryInfo.Attributes != FileAttributes.Directory)
            {
                Console.WriteLine("{0} is not a directory", demoDirectory);
                return;
            }

            Dictionary<string, DemoComponent> ComponentList = new Dictionary<string, DemoComponent>();

            foreach (var component in demoDirectoryInfo.GetFileSystemInfos())
            {
                if (component is DirectoryInfo componentDirectory)
                {
                    var componentName = component.Name;
                    var docDir = componentDirectory.GetFileSystemInfos("doc")[0];
                    var demoDir = componentDirectory.GetFileSystemInfos("demo")[0];

                    foreach (var docItem in (docDir as DirectoryInfo).GetFileSystemInfos())
                    {
                        var language = docItem.Name.Replace("doc_", "").Replace($"{docItem.Extension}", "");
                        var content = File.ReadAllText(docItem.FullName);
                        var docData = DocParser.ParseDoc(content);

                        ComponentList.Add(language, new DemoComponent()
                        {
                            Name = docData.Meta["title"],
                            Type = docData.Meta["type"],
                            Doc = docData.Content,
                        });
                    }

                    foreach (var demo in (demoDir as DirectoryInfo).GetFileSystemInfos().GroupBy(x => x.Name.Replace(x.Extension, "")))
                    {
                        var showCaseFiles = demo.ToList();
                        var razorFile = showCaseFiles.FirstOrDefault(x => x.Extension == ".razor");
                        var descriptionFile = showCaseFiles.FirstOrDefault(x => x.Extension == ".md");
                        var code = razorFile != null ? File.ReadAllText(razorFile.FullName) : null;
                        var descriptionContent = descriptionFile != null ? DocParser.ParseDescription(File.ReadAllText(descriptionFile.FullName)) : default;

                        foreach (var title in descriptionContent.Meta.Title)
                        {
                            var list = ComponentList[title.Key].DemoList ??= new List<DemoItem>();
                            list.Add(new DemoItem()
                            {
                                Title = title.Value,
                                Order = descriptionContent.Meta.Order,
                                Code = code,
                                Description = descriptionContent.Descriptions[title.Key],
                                Type = $"{demoDirectoryInfo.Name}.{component.Name}.{demoDir.Name}.{razorFile.Name.Replace(razorFile.Extension, "")}"
                            });
                        }
                    }
                }
            }

            foreach (var componentDic in ComponentList.GroupBy(x => x.Key))
            {
                var components = componentDic.Select(x => x.Value);
                var json = JsonSerializer.Serialize(components, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                var configFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), output);
                if (!Directory.Exists(configFileDirectory))
                {
                    Directory.CreateDirectory(configFileDirectory);
                }

                var configFilePath = Path.Combine(configFileDirectory, $"demo_{componentDic.Key}.json");
                Console.WriteLine(json);

                if (File.Exists(configFilePath))
                {
                    File.Delete(configFilePath);
                }

                File.WriteAllText(configFilePath, json);
                Console.WriteLine("Generate demo file to {0}", configFilePath);
            }
        }
    }
}