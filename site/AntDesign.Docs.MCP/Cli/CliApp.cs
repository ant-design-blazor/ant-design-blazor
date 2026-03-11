using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using AntDesign.Docs.MCP.Tools;

namespace AntDesign.Docs.MCP.Cli
{
    internal static class CliApp
    {
        public static async Task<int> RunAsync(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "AntDesign.Docs.MCP";
            app.Description = "CLI for AntDesign.Docs.MCP";

            // Global legacy options
            var demoOption = app.Option("-d|--demo <COMPONENT>", "List demos for a component", CommandOptionType.SingleValue);
            var demoSourceOption = app.Option("-s|--demo-source <QUERY>", "Print demo source for a component[:Scenario]", CommandOptionType.SingleValue);

            // component
            app.Command("component", componentCmd =>
            {
                componentCmd.Description = "Component commands";

                componentCmd.Command("list", listCmd =>
                {
                    listCmd.Description = "List all components";
                    listCmd.OnExecuteAsync(async cancellationToken =>
                    {
                        return await AntDesign.Docs.MCP.Cli.ComponentCommand.RunAsync(Array.Empty<string>());
                    });
                });

                componentCmd.Command("search", searchCmd =>
                {
                    searchCmd.Description = "Search components by comma-separated names";
                    var namesArg = searchCmd.Argument("names", "Comma-separated component names");
                    searchCmd.OnExecuteAsync(async cancellationToken =>
                    {
                        var names = namesArg.Value;
                        return await AntDesign.Docs.MCP.Cli.ComponentCommand.RunAsync(new[] { "search", names });
                    });
                });
            });

            // demo
            app.Command("demo", demoCmd =>
            {
                demoCmd.Description = "Demo commands";

                demoCmd.Command("list", listCmd =>
                {
                    listCmd.Description = "List demos for a component";
                    var compArg = listCmd.Argument("component", "Component name");
                    listCmd.OnExecuteAsync(async cancellationToken =>
                    {
                        var component = compArg.Value;
                        return await AntDesign.Docs.MCP.Cli.DemoCommand.RunAsync(new[] { "list", component });
                    });
                });

                demoCmd.Command("source", srcCmd =>
                {
                    srcCmd.Description = "Print demo source for Component[:Scenario]";
                    var qArg = srcCmd.Argument("query", "Component[:Scenario]");
                    srcCmd.OnExecuteAsync(async cancellationToken =>
                    {
                        var query = qArg.Value;
                        return await AntDesign.Docs.MCP.Cli.DemoCommand.RunAsync(new[] { "source", query });
                    });
                });
            });
            // debug
            app.Command("debug", debugCmd =>
            {
                debugCmd.Description = "Debugging commands";

                debugCmd.Command("data", dataCmd =>
                {
                    dataCmd.Description = "Print data files found in MCP output directories and first 2KB of each file";
                    dataCmd.OnExecute(() =>
                    {
                        var roots = new[]
                        {
                            // runtime output
                            AppContext.BaseDirectory,
                            // project data directory (when running from source)
                            System.IO.Path.Combine(Environment.CurrentDirectory, "site", "AntDesign.Docs.MCP", "data"),
                            // repo-level MCP data (just in case)
                            System.IO.Path.Combine(Environment.CurrentDirectory, "site", "AntDesign.Docs.MCP", "data")
                        };

                        var sb = new System.Text.StringBuilder();
                        var seen = new HashSet<string>();
                        foreach (var root in roots)
                        {
                            try
                            {
                                var dataDir = System.IO.Path.Combine(root, "data");
                                if (!Directory.Exists(root) && !Directory.Exists(dataDir)) continue;

                                var searchDir = Directory.Exists(dataDir) ? dataDir : root;
                                sb.AppendLine($"Scanning: {searchDir}");
                                var files = Directory.GetFiles(searchDir, "*.json", SearchOption.AllDirectories);
                                foreach (var f in files)
                                {
                                    if (!seen.Add(f)) continue;
                                    sb.AppendLine($"--- FILE: {f}");
                                    try
                                    {
                                        var text = File.ReadAllText(f);
                                        var snippet = text.Length <= 2048 ? text : text.Substring(0, 2048) + "...";
                                        sb.AppendLine(snippet);
                                    }
                                    catch (Exception ex)
                                    {
                                        sb.AppendLine($"(failed to read: {ex.Message})");
                                    }
                                }
                            }
                            catch { }
                        }

                        Console.WriteLine(sb.ToString());
                        return 0;
                    });
                });

                debugCmd.Command("version", vcmd =>
                {
                    vcmd.Description = "Print package id, current version and cached latest (for testing).";
                    vcmd.OnExecute(() =>
                    {
                        Console.WriteLine($"PackageId: {AntDesignTools.PackageId}");
                        Console.WriteLine($"CurrentPackageVersion: {AntDesignTools.CurrentPackageVersion}");
                        Console.WriteLine($"CachedLatest (NuGet / env override): {AntDesign.Docs.MCP.Services.NuGetService.GetCachedLatest()}");
                        return 0;
                    });
                });
            });
            // Root-level execution for legacy options
            app.OnExecuteAsync(async cancellationToken =>
            {
                if (demoOption.HasValue())
                {
                    var comp = demoOption.Value();
                    return await AntDesign.Docs.MCP.Cli.DemoCommand.RunAsync(new[] { "list", comp });
                }

                if (demoSourceOption.HasValue())
                {
                    var q = demoSourceOption.Value();
                    return await AntDesign.Docs.MCP.Cli.DemoCommand.RunAsync(new[] { "source", q });
                }

                app.ShowHelp();
                return 0;
            });

            return app.Execute(args);
        }
    }
}