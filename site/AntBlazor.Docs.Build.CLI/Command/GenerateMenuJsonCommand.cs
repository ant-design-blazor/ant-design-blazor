using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using AntBlazor.Docs.Build.CLI.Extensions;
using AntBlazor.Docs.Build.CLI.Utils;
using Microsoft.Extensions.CommandLineUtils;

namespace AntBlazor.Docs.Build.CLI.Command
{
    public class GenerateMenuJsonCommand : IAppCommand

    {
        public string Name => "menu2json";

        public void Execute(CommandLineApplication command)
        {
            command.Description = "Generate json file for menu";
            command.HelpOption();

            CommandArgument demoDirArgument = command.Argument(
                "demoDir", "[Required] The directory of docs files.");

            CommandArgument docsDirArgument = command.Argument(
                "docsDir", "[Required] The directory of docs files.");

            CommandArgument outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            command.OnExecute(() =>
            {
                string demoDir = demoDirArgument.Value;
                string docsDir = docsDirArgument.Value;
                string output = outputArgument.Value;

                if (string.IsNullOrEmpty(demoDir) || !Directory.Exists(demoDir))
                {
                    Console.WriteLine("Invalid demoDir.");
                    return 1;
                }

                if (string.IsNullOrEmpty(docsDir) || !Directory.Exists(docsDir))
                {
                    Console.WriteLine("Invalid docsDir.");
                    return 1;
                }

                if (string.IsNullOrEmpty(output))
                {
                    output = "./";
                }

                string demoDirectory = Path.Combine(Directory.GetCurrentDirectory(), demoDir);
                string docsDirectory = Path.Combine(Directory.GetCurrentDirectory(), docsDir);

                GenerateFiles(demoDirectory, docsDirectory, output);

                return 0;
            });
        }

        private void GenerateFiles(string demoDirectory, string docsDirectory, string output)
        {
            DirectoryInfo demoDirectoryInfo = new DirectoryInfo(demoDirectory);
            if (!demoDirectoryInfo.Exists)
            {
                Console.WriteLine("{0} is not a directory", demoDirectory);
                return;
            }

            DirectoryInfo docsDirectoryInfo = new DirectoryInfo(docsDirectory);
            if (!docsDirectoryInfo.Exists)
            {
                Console.WriteLine("{0} is not a directory", docsDirectory);
                return;
            }

            IList<Dictionary<string, DemoComponent>> componentList = null;

            IList<Dictionary<string, DemoMenuItem>> menuList = null;

            Dictionary<string, int> sortMap = new Dictionary<string, int>()
            {
                ["General"] = 0,
                ["通用"] = 0,
                ["Layout"] = 1,
                ["布局"] = 1,
                ["Navigation"] = 2,
                ["导航"] = 2,
                ["Data Entry"] = 3,
                ["数据录入"] = 3,
                ["Data Display"] = 4,
                ["数据展示"] = 4,
                ["Feedback"] = 5,
                ["反馈"] = 5,
                ["Localization"] = 6,
                ["Other"] = 7,
                ["其他"] = 7
            };

            foreach (FileSystemInfo component in demoDirectoryInfo.GetFileSystemInfos())
            {
                if (!(component is DirectoryInfo componentDirectory))
                    continue;

                Dictionary<string, DemoComponent> componentDic = new Dictionary<string, DemoComponent>();

                FileSystemInfo docDir = componentDirectory.GetFileSystemInfos("doc")[0];

                foreach (FileSystemInfo docItem in (docDir as DirectoryInfo).GetFileSystemInfos())
                {
                    string language = docItem.Name.Replace("index.", "").Replace(docItem.Extension, "");
                    string content = File.ReadAllText(docItem.FullName);
                    (Dictionary<string, string> Meta, string Content) docData = DocParser.ParseDemoDoc(content);

                    componentDic.Add(language, new DemoComponent()
                    {
                        Title = docData.Meta["title"],
                        SubTitle = docData.Meta.TryGetValue("subtitle", out string subtitle) ? subtitle : null,
                        Type = docData.Meta["type"],
                        Doc = docData.Content,
                    });
                }

                componentList ??= new List<Dictionary<string, DemoComponent>>();
                componentList.Add(componentDic);
            }

            if (componentList == null)
                return;

            List<Dictionary<string, DemoMenuItem>> componentMenuList = new List<Dictionary<string, DemoMenuItem>>();

            IEnumerable<KeyValuePair<string, DemoComponent>> componentI18N = componentList
                .SelectMany(x => x);

            foreach (IGrouping<string, KeyValuePair<string, DemoComponent>> group in componentI18N.GroupBy(x => x.Value.Type))
            {
                Dictionary<string, DemoMenuItem> menu = new Dictionary<string, DemoMenuItem>();

                foreach (IGrouping<string, KeyValuePair<string, DemoComponent>> component in group.GroupBy(x => x.Key))
                {
                    menu.Add(component.Key, new DemoMenuItem()
                    {
                        Order = sortMap[group.Key],
                        Title = group.Key,
                        Type = "itemGroup",
                        Children = group.Select(x => new DemoMenuItem()
                        {
                            Title = x.Value.Title,
                            SubTitle = x.Value.SubTitle,
                            Url = $"components/{x.Value.Title.ToLower()}",
                            Type = "menuItem"
                        }).ToArray()
                    });
                }

                componentMenuList.Add(menu);
            }

            foreach (FileSystemInfo docItem in docsDirectoryInfo.GetFileSystemInfos())
            {
                if (docItem.Extension != ".md")
                    continue;

                string[] segments = docItem.Name.Split('.');
                if (segments.Length != 3)
                    continue;

                string language = segments[1];
                string content = File.ReadAllText(docItem.FullName);
                Dictionary<string, string> docData = DocParser.ParseHeader(content);

                menuList ??= new List<Dictionary<string, DemoMenuItem>>();
                menuList.Add(new Dictionary<string, DemoMenuItem>()
                {
                    [language] = new DemoMenuItem()
                    {
                        Order = int.TryParse(docData["order"], out int order) ? order : 0,
                        Title = docData["title"],
                        Url = $"docs/{segments[0]}",
                        Type = "menuItem"
                    }
                });
            }

            if (menuList == null)
                return;

            IEnumerable<IGrouping<string, KeyValuePair<string, DemoMenuItem>>> menuI18N = menuList
                .SelectMany(x => x).GroupBy(x => x.Key);

            IEnumerable<IGrouping<string, KeyValuePair<string, DemoMenuItem>>> componentMenuI18N = componentMenuList
                .SelectMany(x => x).GroupBy(x => x.Key);

            foreach (IGrouping<string, KeyValuePair<string, DemoMenuItem>> menuGroup in menuI18N)
            {
                var children = menuGroup.Select(x => x.Value).OrderBy(x => x.Order).ToArray();
                List<DemoMenuItem> menu = new List<DemoMenuItem>
                {
                    new DemoMenuItem()
                    {
                        Order = 0,
                        Title = menuGroup.Key == "zh-CN" ? "文档" : "Docs",
                        Type = "subMenu",
                        Url = "docs",
                        Children = children
                    }
                };

                var components = componentMenuI18N.Where(x => x.Key == menuGroup.Key)
                    .SelectMany(x => x)
                    .Select(x => x.Value)
                    .OrderBy(x => x.Order)
                    .ToArray();

                menu.Add(new DemoMenuItem()
                {
                    Order = 999,
                    Title = menuGroup.Key == "zh-CN" ? "组件" : "Components",
                    Type = "subMenu",
                    Url = "components",
                    Children = components.ToArray()
                });

                string json = JsonSerializer.Serialize(menu, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                string configFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), output);
                if (!Directory.Exists(configFileDirectory))
                {
                    Directory.CreateDirectory(configFileDirectory);
                }

                string configFilePath = Path.Combine(configFileDirectory, $"menu.{menuGroup.Key}.json");
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
