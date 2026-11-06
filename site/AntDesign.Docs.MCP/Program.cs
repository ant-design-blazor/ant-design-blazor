// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AntDesign.Docs.MCP.Tools;

if (args.Length > 0)
{
    var exit = await AntDesign.Docs.MCP.Cli.CliApp.RunAsync(args);
    Environment.Exit(exit);
    return;
}

var builder = Host.CreateApplicationBuilder(args);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

// Add the MCP services: the transport to use (stdio) and the tools to register.
// Register Component & Demo services for DI and the hosted prefetch service
builder.Services.AddSingleton<AntDesign.Docs.MCP.Services.ComponentService>();
builder.Services.AddSingleton<AntDesign.Docs.MCP.Services.DemoService>();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<AntDesignTools>();

// Register background prefetch service to perform initial data & NuGet checks
builder.Services.AddHostedService<AntDesign.Docs.MCP.Services.PrefetchBackgroundService>();

await builder.Build().RunAsync();
