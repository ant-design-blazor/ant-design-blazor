// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Docs.MCP.Services;
using AntDesign.Docs.MCP.Tools;

namespace AntDesign.Docs.MCP.Cli
{
    internal static class ComponentCommand
    {
        public static async Task<int> RunAsync(string[] args)
        {
            var svc = new ComponentService();
            try
            {
                await svc.LoadComponentsAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load components: {ex.Message}");
                return 1;
            }

            var tools = new AntDesignTools(svc, new DemoService());

            if (args.Length == 0 || args[0].Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                var outStr = await tools.ListComponents();
                Console.WriteLine(outStr);
                return 0;
            }

            var verb = args[0].ToLowerInvariant();
            if (verb == "search" || verb == "find" || verb == "get")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: antdesign-docs-mcp component search <name1[,name2]>\nExample: antdesign-docs-mcp component search Button,Input");
                    return 1;
                }

                var names = args[1];
                var outStr = await tools.SearchComponents(names);
                Console.WriteLine(outStr);
                return 0;
            }

            Console.WriteLine("Unknown component command. Available: list, search");
            return 1;
        }
    }
}