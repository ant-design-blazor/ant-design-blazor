// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    public class GenerateDemoJsonCommand : IAppCommand
    {
        public string Name => "demo2json";

        public void Execute(CommandLineApplication command)
        {
            command.Description = "Generate json file of demos";
            command.HelpOption();

            CommandArgument directoryArgument = command.Argument(
                "source", "[Required] The directory of demo files.");

            CommandArgument outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            command.OnExecute(() =>
            {
                string source = directoryArgument.Value;
                string output = outputArgument.Value;

                if (string.IsNullOrEmpty(source) || !Directory.Exists(source))
                {
                    Console.WriteLine($"Invalid source: {source}");
                    return 1;
                }

                if (string.IsNullOrEmpty(output))
                {
                    output = "./";
                }

                string demoDirectory = Path.Combine(Directory.GetCurrentDirectory(), source);

                try
                {
                    GenerateFiles(demoDirectory, output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return -1;
                }

                return 0;
            });
        }

        private void GenerateFiles(string demoDirectory, string output)
        {
            DirectoryInfo demoDirectoryInfo = new DirectoryInfo(demoDirectory);
            if (!demoDirectoryInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                Console.WriteLine("{0} is not a directory", demoDirectory);
                return;
            }

            IList<Dictionary<string, DemoComponent>> componentList = null;
            IList<string> demoTypes = null;

            var directories = demoDirectoryInfo.GetFileSystemInfos()
                .SelectMany(x => (x as DirectoryInfo).GetFileSystemInfos());

            foreach (FileSystemInfo component in directories)
            {
                if (!(component is DirectoryInfo componentDirectory)) continue;

                FileSystemInfo docDir = componentDirectory.GetFileSystemInfos("doc")?.FirstOrDefault();
                FileSystemInfo demoDir = componentDirectory.GetFileSystemInfos("demo")?.FirstOrDefault();

                Dictionary<string, DemoComponent> componentDic = new Dictionary<string, DemoComponent>();

                if (docDir != null && docDir.Exists)
                {
                    foreach (FileSystemInfo docItem in (docDir as DirectoryInfo).GetFileSystemInfos())
                    {
                        string language = docItem.Name.Replace("index.", "").Replace(docItem.Extension, "");
                        string content = File.ReadAllText(docItem.FullName);
                        (Dictionary<string, string> Meta, string desc, string apiDoc) docData = DocParser.ParseDemoDoc(content);

                        componentDic.Add(language, new DemoComponent()
                        {
                            Category = docData.Meta["category"],
                            Title = docData.Meta["title"],
                            SubTitle = docData.Meta.TryGetValue("subtitle", out string subtitle) ? subtitle : null,
                            Type = docData.Meta["type"],
                            Desc = docData.desc,
                            ApiDoc = docData.apiDoc.Replace("<h2>API</h2>", $"<h2 id=\"API\"><span>API</span><a href=\"{language}/components/{docData.Meta["title"].ToLower()}#API\" class=\"anchor\">#</a></h2>"),
                            Cols = docData.Meta.TryGetValue("cols", out var cols) ? int.Parse(cols) : (int?)null,
                            Cover = docData.Meta.TryGetValue("cover", out var cover) ? cover : null,
                        });
                    }
                }

                if (demoDir != null && demoDir.Exists)
                {
                    foreach (IGrouping<string, FileSystemInfo> demo in (demoDir as DirectoryInfo).GetFileSystemInfos()
                        .GroupBy(x => x.Name
                            .Replace(x.Extension, "")
                            .Replace("-", "")
                            .Replace("_", "")
                            .Replace("Demo", "")
                            .ToLower()))
                    {
                        List<FileSystemInfo> showCaseFiles = demo.ToList();
                        FileSystemInfo razorFile = showCaseFiles.FirstOrDefault(x => x.Extension == ".razor");
                        FileSystemInfo descriptionFile = showCaseFiles.FirstOrDefault(x => x.Extension == ".md");
                        string code = razorFile != null ? File.ReadAllText(razorFile.FullName) : null;

                        string demoType = $"{demoDirectoryInfo.Name}{razorFile.FullName.Replace(demoDirectoryInfo.FullName, "").Replace(Path.DirectorySeparatorChar, '.').Replace(razorFile.Extension, "")}";

                        (DescriptionYaml Meta, string Style, Dictionary<string, string> Descriptions) descriptionContent = descriptionFile != null ? DocParser.ParseDescription(File.ReadAllText(descriptionFile.FullName)) : default;

                        demoTypes ??= new List<string>();
                        demoTypes.Add(demoType);

                        foreach (KeyValuePair<string, string> title in descriptionContent.Meta.Title)
                        {
                            string language = title.Key;
                            List<DemoItem> list = componentDic[language].DemoList ??= new List<DemoItem>();

                            list.Add(new DemoItem()
                            {
                                Title = title.Value,
                                Order = descriptionContent.Meta.Order,
                                Iframe = descriptionContent.Meta.Iframe,
                                Link = descriptionContent.Meta.Link,
                                Code = code,
                                Description = descriptionContent.Descriptions[title.Key],
                                Name = descriptionFile?.Name.Replace(".md", ""),
                                Style = descriptionContent.Style,
                                Debug = descriptionContent.Meta.Debug,
                                Docs = descriptionContent.Meta.Docs,
                                Type = demoType
                            });
                        }
                    }
                }

                componentList ??= new List<Dictionary<string, DemoComponent>>();
                componentList.Add(componentDic);
            }

            if (componentList == null)
                return;

            string configFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), output);

            if (!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }

            IEnumerable<IGrouping<string, KeyValuePair<string, DemoComponent>>> componentI18N = componentList
                .SelectMany(x => x).GroupBy(x => x.Key);

            foreach (IGrouping<string, KeyValuePair<string, DemoComponent>> componentDic in componentI18N)
            {
                IEnumerable<DemoComponent> components = componentDic.Select(x => x.Value);
                string componentJson = JsonSerializer.Serialize(components, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                string configFilePath = Path.Combine(configFileDirectory, $"components.{componentDic.Key}.json");

                if (File.Exists(configFilePath))
                {
                    File.Delete(configFilePath);
                }

                File.WriteAllText(configFilePath, componentJson);

                Console.WriteLine("Generate demo file to {0}", configFilePath);
            }

            var demoJson = JsonSerializer.Serialize(demoTypes, new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            string demoFilePath = Path.Combine(configFileDirectory, $"demoTypes.json");
            if (File.Exists(demoFilePath))
            {
                File.Delete(demoFilePath);
            }
            File.WriteAllText(demoFilePath, demoJson);
        }
    }
}
