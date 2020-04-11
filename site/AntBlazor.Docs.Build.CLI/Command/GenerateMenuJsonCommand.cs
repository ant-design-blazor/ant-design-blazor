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

            var demoDirArgument = command.Argument(
                "demoDir", "[Required] The directory of docs files.");

            var docsDirArgument = command.Argument(
                "docsDir", "[Required] The directory of docs files.");

            var outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            command.OnExecute(() =>
            {
                var demoDir = demoDirArgument.Value;
                var docsDir = docsDirArgument.Value;
                var output = outputArgument.Value;

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

                var demoDirectory = Path.Combine(Directory.GetCurrentDirectory(), demoDir);
                var docsDirectory = Path.Combine(Directory.GetCurrentDirectory(), docsDir);

                GenerateFiles(demoDirectory, docsDirectory, output);

                return 0;
            });
        }

        public void GenerateFiles(string demoDirectory, string docsDirectory, string output)
        {
            var demoDirectoryInfo = new DirectoryInfo(demoDirectory);
            if (demoDirectoryInfo.Attributes != FileAttributes.Directory)
            {
                Console.WriteLine("{0} is not a directory", demoDirectory);
                return;
            }

            var docsDirectoryInfo = new DirectoryInfo(docsDirectory);
            if (docsDirectoryInfo.Attributes != FileAttributes.Directory)
            {
                Console.WriteLine("{0} is not a directory", docsDirectory);
                return;
            }

            IList<Dictionary<string, DemoComponent>> componentList = null;

            IList<Dictionary<string, MenuItem>> menuList = null;

            var sortMap = new Dictionary<string, int>()
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

            foreach (var component in demoDirectoryInfo.GetFileSystemInfos())
            {
                if (!(component is DirectoryInfo componentDirectory))
                    continue;

                var componentDic = new Dictionary<string, DemoComponent>();

                var docDir = componentDirectory.GetFileSystemInfos("doc")[0];

                foreach (var docItem in (docDir as DirectoryInfo).GetFileSystemInfos())
                {
                    var language = docItem.Name.Replace("index.", "").Replace(docItem.Extension, "");
                    var content = File.ReadAllText(docItem.FullName);
                    var docData = DocParser.ParseDemoDoc(content);

                    componentDic.Add(language, new DemoComponent()
                    {
                        Title = docData.Meta["title"],
                        SubTitle = docData.Meta.TryGetValue("subtitle", out var subtitle) ? subtitle : null,
                        Type = docData.Meta["type"],
                        Doc = docData.Content,
                    });
                }

                componentList ??= new List<Dictionary<string, DemoComponent>>();
                componentList.Add(componentDic);
            }

            if (componentList == null)
                return;

            var componentMenuList = new List<Dictionary<string, MenuItem>>();

            var componentI18N = componentList
                .SelectMany(x => x);

            foreach (var group in componentI18N.GroupBy(x => x.Value.Type))
            {
                var menu = new Dictionary<string, MenuItem>();

                foreach (var component in group.GroupBy(x => x.Key))
                {
                    menu.Add(component.Key, new MenuItem()
                    {
                        Order = sortMap[group.Key],
                        Title = group.Key,
                        Type = "itemGroup",
                        Children = group.Select(x => new MenuItem()
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

            foreach (var docItem in docsDirectoryInfo.GetFileSystemInfos())
            {
                if (docItem.Extension != ".md")
                    continue;

                var segments = docItem.Name.Split('.');
                if (segments.Length != 3)
                    continue;

                var language = segments[1];
                var content = File.ReadAllText(docItem.FullName);
                var docData = DocParser.ParseHeader(content);

                menuList ??= new List<Dictionary<string, MenuItem>>();
                menuList.Add(new Dictionary<string, MenuItem>()
                {
                    [language] = new MenuItem()
                    {
                        Order = int.TryParse(docData["order"], out var order) ? order : 0,
                        Title = docData["title"],
                        Url = $"docs/{segments[0]}",
                        Type = "menuItem"
                    }
                });
            }

            if (menuList == null)
                return;

            var menuI18N = menuList
                .SelectMany(x => x).GroupBy(x => x.Key);

            var componentMenuI18N = componentMenuList
                .SelectMany(x => x).GroupBy(x => x.Key);

            foreach (var menuGroup in menuI18N)
            {
                var menu = menuGroup.Select(x => x.Value).OrderBy(x => x.Order).ToList();

                var components = componentMenuI18N.Where(x => x.Key == menuGroup.Key)
                    .SelectMany(x => x)
                    .Select(x => x.Value)
                    .OrderBy(x => x.Order);

                menu.Add(new MenuItem()
                {
                    Order = 999,
                    Title = menuGroup.Key == "zh-CN" ? "组件" : "Components",
                    Type = "subMenu",
                    Children = components.ToArray()
                });

                var json = JsonSerializer.Serialize(menu, new JsonSerializerOptions()
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

                var configFilePath = Path.Combine(configFileDirectory, $"menu.{menuGroup.Key}.json");
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