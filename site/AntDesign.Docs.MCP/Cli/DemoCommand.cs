// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.MCP.Services;
using SmartComponents.LocalEmbeddings;
using AntDesign.Docs.MCP.Tools;

namespace AntDesign.Docs.MCP.Cli
{
    internal static class DemoCommand
    {
        public static async Task<int> RunAsync(string[] args)
        {
            var svc = new DemoService();
            try
            {
                var demos = await svc.LoadDemosAsync();

                if (args.Length == 0)
                {
                    Console.WriteLine("Usage: antdesign-docs-mcp demo <list|source> <args>");
                    return 1;
                }

                var verb = args[0].ToLowerInvariant();

                var tools = new AntDesignTools();

                if (verb == "list")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: antdesign-docs-mcp demo list <Component>");
                        return 1;
                    }

                    var component = args[1];
                    var outStr = await tools.ListDemosForComponent(component);
                    Console.WriteLine(outStr);
                    return 0;
                }

                if (verb == "source")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: antdesign-docs-mcp demo source <Component[:Scenario]>");
                        return 1;
                    }

                    var query = args[1];
                    var outStr = await tools.GetDemoSource(query);
                    Console.WriteLine(outStr);
                    return 0;
                }

                Console.WriteLine("Unknown demo command. Available: list, source");
                return 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load demos: {ex.Message}");
                return 1;
            }
        }
    }
}