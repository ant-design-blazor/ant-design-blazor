// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using AntDesign.Docs.Build.CLI.Extensions;
using AntDesign.Docs.Build.CLI.Utils;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Command
{
    public class GenerateMenuJsonCommand : IAppCommand
    {
        public string Name => "menu2json";

        private static readonly Dictionary<string, int> _sortMap = new Dictionary<string, int>()
        {
            ["Docs"] = -2,
            ["文档"] = -2,
            ["Overview"] = -1,
            ["组件总览"] = -1,
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
            ["其他"] = 7,
            ["Charts"] = 8,
            ["图表"] = 8,
            ["Experimental"] = 9,
            ["高阶功能"] = 9,
        };

        private static readonly Dictionary<string, string> _demoCategoryMap = new Dictionary<string, string>()
        {
            ["Components"] = "组件",
            ["Charts"] = "图表",
            ["Experimental"] = "高阶功能"
        };

        private static readonly Dictionary<string, string> _typeNameMap = new Dictionary<string, string>()
        {
            ["General"] = "通用",
            ["Layout"] = "布局",
            ["Navigation"] = "导航",
            ["Data Entry"] = "数据录入",
            ["Data Display"] = "数据展示",
            ["Feedback"] = "反馈",
            ["Other"] = "其他",
        };

        private const string LibraryAssemblyName = "AntDesign";

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

                try
                {
                    GenerateFiles(demoDirectory, docsDirectory, output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
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

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            IList<Dictionary<string, DemoMenuItem>> docsMenuList = GetSubMenuList(docsDirectoryInfo, true).ToList();

            Dictionary<string, Dictionary<string, IEnumerable<DemoMenuItem>>> categoryDemoMenuList = new Dictionary<string, Dictionary<string, IEnumerable<DemoMenuItem>>>();
            List<Dictionary<string, DemoMenuItem>> allComponentMenuList = new List<Dictionary<string, DemoMenuItem>>();

            foreach (var subDemoDirectory in demoDirectoryInfo.GetFileSystemInfos())
            {
                if (subDemoDirectory is not DirectoryInfo directory)
                {
                    continue;
                }

                var category = subDemoDirectory.Name;

                IList<Dictionary<string, DemoMenuItem>> componentMenuList = GetSubMenuList(directory, false).ToList();

                allComponentMenuList.AddRange(componentMenuList);

                if (category == "Components")
                {
                    componentMenuList.Add(new Dictionary<string, DemoMenuItem>() { ["zh-CN"] = new DemoMenuItem() { Title = "组件总览", Type = "menuItem", Url = "components/overview", Order = -1 } });
                    componentMenuList.Add(new Dictionary<string, DemoMenuItem>() { ["en-US"] = new DemoMenuItem() { Title = "Components Overview", Type = "menuItem", Url = "components/overview", Order = -1 } });
                }

                var componentMenuI18N = componentMenuList
                    .SelectMany(x => x)
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Select(o => o.Value));

                foreach (var component in componentMenuI18N)
                {
                    if (!categoryDemoMenuList.ContainsKey(component.Key))
                    {
                        categoryDemoMenuList[component.Key] = new Dictionary<string, IEnumerable<DemoMenuItem>>();
                    }
                    categoryDemoMenuList[component.Key].Add(category, component.Value);
                }
            }

            var docsMenuI18N = docsMenuList
                .SelectMany(x => x)
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Value));

            foreach (var lang in new[] { "zh-CN", "en-US" })
            {
                List<DemoMenuItem> menu = new List<DemoMenuItem>();

                var children = docsMenuI18N[lang].OrderBy(x => x.Order).ToArray();

                menu.Add(new DemoMenuItem()
                {
                    Order = 0,
                    Title = lang == "zh-CN" ? "文档" : "Docs",
                    Type = "subMenu",
                    Url = "docs",
                    Children = children
                });

                var categoryComponent = categoryDemoMenuList[lang];

                foreach (var component in categoryComponent)
                {
                    if (!_demoCategoryMap.ContainsKey(component.Key))
                    {
                        continue;
                    }
                    menu.Add(new DemoMenuItem()
                    {
                        Order = Array.IndexOf(_demoCategoryMap.Select(x => x.Key).ToArray(), component.Key) + 1,
                        Title = lang == "zh-CN" ? _demoCategoryMap[component.Key] : component.Key,
                        Type = "subMenu",
                        Url = component.Key.ToLowerInvariant(),
                        Children = component.Value.OrderBy(x => x.Order).ToArray()
                    });
                }

                var json = JsonSerializer.Serialize(menu, jsonOptions);

                var configFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), output);
                if (!Directory.Exists(configFileDirectory))
                {
                    Directory.CreateDirectory(configFileDirectory);
                }

                var configFilePath = Path.Combine(configFileDirectory, $"menu.{lang}.json");

                if (File.Exists(configFilePath))
                {
                    File.Delete(configFilePath);
                }

                File.WriteAllText(configFilePath, json);

                var componentI18N = allComponentMenuList
                    .SelectMany(x => x)
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Select(o => o.Value));

                var demos = componentI18N[lang];

                var demosPath = Path.Combine(configFileDirectory, $"demos.{lang}.json");

                if (File.Exists(demosPath))
                {
                    File.Delete(demosPath);
                }

                json = JsonSerializer.Serialize(demos, jsonOptions);
                File.WriteAllText(demosPath, json);

                var docs = docsMenuI18N[lang];
                var docsPath = Path.Combine(configFileDirectory, $"docs.{lang}.json");

                if (File.Exists(docsPath))
                {
                    File.Delete(docsPath);
                }

                json = JsonSerializer.Serialize(docs, jsonOptions);
                File.WriteAllText(docsPath, json);
            }
        }

        private IEnumerable<Dictionary<string, DemoMenuItem>> GetSubMenuList(DirectoryInfo directory, bool isDocs)
        {
            if (isDocs)
            {
                foreach (FileSystemInfo docItem in directory.GetFileSystemInfos())
                {
                    if (docItem.Extension != ".md")
                        continue;

                    string[] segments = docItem.Name.Split('.');
                    if (segments.Length != 3)
                        continue;

                    string language = segments[1];
                    string content = File.ReadAllText(docItem.FullName);
                    Dictionary<string, string> docData = DocParser.ParseHeader(content);

                    yield return (new Dictionary<string, DemoMenuItem>()
                    {
                        [language] = new DemoMenuItem()
                        {
                            Order = float.TryParse(docData["order"], out var order) ? order : 0,
                            Title = docData["title"],
                            Url = $"docs/{segments[0]}",
                            Type = "menuItem"
                        }
                    });
                }
            }
            else
            {
                var componentI18N = directory.Name == "Components" ? GetComponentSubMenuList() : GetComponentI18N(directory);
                foreach (IGrouping<string, KeyValuePair<string, DemoComponent>> group in componentI18N.GroupBy(x => x.Value.Type))
                {
                    Dictionary<string, DemoMenuItem> menu = new Dictionary<string, DemoMenuItem>();

                    foreach (IGrouping<string, KeyValuePair<string, DemoComponent>> component in group.GroupBy(x => x.Key))
                    {
                        menu.Add(component.Key, new DemoMenuItem()
                        {
                            Order = _sortMap[group.Key],
                            Title = group.Key,
                            Type = "itemGroup",
                            Children = group.Select(x => new DemoMenuItem()
                            {
                                Title = x.Value.Title,
                                SubTitle = x.Value.SubTitle,
                                Url = $"{directory.Name.ToLowerInvariant()}/{x.Value.Title.ToLower()}",
                                Type = "menuItem",
                                Cover = x.Value.Cover,
                            })
                            .OrderBy(x => x.Title)
                            .ToArray(),
                        });
                    }

                    yield return menu;
                }
            }
        }

        private IEnumerable<KeyValuePair<string, DemoComponent>> GetComponentI18N(DirectoryInfo directory)
        {
            IList<Dictionary<string, DemoComponent>> componentList = null;

            foreach (FileSystemInfo component in directory.GetFileSystemInfos())
            {
                if (!(component is DirectoryInfo componentDirectory))
                    continue;

                Dictionary<string, DemoComponent> componentDic = new Dictionary<string, DemoComponent>();

                FileSystemInfo docDir = componentDirectory.GetFileSystemInfos("doc")[0];

                foreach (FileSystemInfo docItem in (docDir as DirectoryInfo).GetFileSystemInfos())
                {
                    string language = docItem.Name.Replace("index.", "").Replace(docItem.Extension, "");
                    string content = File.ReadAllText(docItem.FullName);
                    (Dictionary<string, string> Meta, string Desc, string ApiDoc) docData = DocParser.ParseDemoDoc(content);

                    componentDic.Add(language, new DemoComponent()
                    {
                        Title = docData.Meta["title"],
                        SubTitle = docData.Meta.TryGetValue("subtitle", out var subtitle) ? subtitle : null,
                        Type = docData.Meta["type"],
                        Desc = docData.Desc,
                        ApiDoc = docData.ApiDoc,
                        Cover = docData.Meta.TryGetValue("cover", out var cover) ? cover : null,
                    });
                }

                componentList ??= new List<Dictionary<string, DemoComponent>>();
                componentList.Add(componentDic);
            }

            if (componentList == null)
                return Enumerable.Empty<KeyValuePair<string, DemoComponent>>();

            IEnumerable<KeyValuePair<string, DemoComponent>> componentI18N = componentList
                .SelectMany(x => x).OrderBy(x => _sortMap[x.Value.Type]);

            return componentI18N;
        }

        private List<KeyValuePair<string, DemoComponent>> GetComponentSubMenuList()
        {
            List<KeyValuePair<string, DemoComponent>> componentList = new();

            var components = Assembly.Load(LibraryAssemblyName).GetTypes();

            foreach (var component in components)
            {
                var docAttribute = component.GetCustomAttribute<DocumentationAttribute>();
                if (docAttribute is null)
                {
                    continue;
                }

                componentList.Add(new(Constants.EnglishLanguage, new DemoComponent()
                {
                    Category = docAttribute.Category.ToString(),
                    Title = docAttribute.Title ?? component.Name,
                    // SubTitle = docAttribute.SubTitle,
                    Type = GetDescription(typeof(DocumentationType), docAttribute.Type),
                    Desc = string.Empty,
                    ApiDoc = string.Empty,
                    Cols = docAttribute.Columns,
                    Cover = docAttribute.CoverImageUrl,
                }));

                componentList.Add(new(Constants.ChineseLanguage, new DemoComponent()
                {
                    Category = docAttribute.Category.ToString(),
                    Title = docAttribute.Title ?? component.Name,
                    SubTitle = docAttribute.SubTitle,
                    Type = _typeNameMap[GetDescription(typeof(DocumentationType), docAttribute.Type)],
                    Desc = string.Empty,
                    ApiDoc = string.Empty,
                    Cols = docAttribute.Columns,
                    Cover = docAttribute.CoverImageUrl,
                }));
            }

            return componentList;
        }

        private static string GetDescription(Type enumType, object enumValue)
        {
            var enumName = Enum.GetName(enumType, enumValue);
            var fieldInfo = enumType.GetField(enumName);
            return fieldInfo.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? string.Empty;
        }
    }
}
